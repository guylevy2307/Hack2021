using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hack.Model;
using Hack2021.Data;
using static Hack.Model.Transaction;

namespace Hack2021.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly Hack2021Context _context;
        private List<string> mid_list_auto = new List<string>();

        public TransactionsController(Hack2021Context context)
        {
            _context = context;
            this.mid_list_auto.Add("Stripe");
            this.mid_list_auto.Add("Dwolla");
            this.mid_list_auto.Add("Braintree");
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Transaction.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.TransactionID == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }
        public ActionResult Cancel(int? id)
        {
            Transaction model = _context.Transaction.FirstOrDefault(t => t.TransactionID == id);
            if (model != null)
                return PartialView("_Cancel", model);
            else
                return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Cancel(Transaction model)
        {
            //validate user  
            if (!ModelState.IsValid)
                return PartialView("_CreateEdit", model);


            //save user into database   
            return RedirectToAction("index");
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewBag.cardsList = new SelectList(_context.CreditCard.Select(a=>a.Number).ToList());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            if (_context.Transaction.FirstOrDefault(t => t.TransactionID == transaction.TransactionID) == null)
            {
                if (ModelState.IsValid)
                {
                    Transaction newT = new Transaction();

                    newT.TransactionDate = transaction.TransactionDate;
                    newT.Amount = transaction.Amount;
                    newT.mId = transaction.mId;
                    newT.Status = Transaction.StatusEnum.AUTO;
                    newT.CardNumber = transaction.CardNumber;
                    newT.CreditCardInfo = _context.CreditCard.Find(transaction.CardNumber);
                   
                    _context.Add(newT);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewBag.statusList = new SelectList(Enum.GetNames(typeof(StatusEnum)));
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionID,Amount,TransactionDate,Status,mId")] Transaction transaction)
        {
            if (id != transaction.TransactionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.TransactionID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.TransactionID == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public JsonResult GetCardsByPrefix(string prefix)
        {
            return Json(_context.CreditCard.Where(c => c.Number.StartsWith(prefix)).Select(a => new { value = a.Number }), System.Web.Mvc.JsonRequestBehavior.AllowGet);

        }
        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.TransactionID == id);
        }
        
         /*public IActionResult SplitIt()
         {
             return View();
         }*/

        
        public async Task<IActionResult> SplitIt(int id)
        {
            Transaction transaction = _context.Transaction.Find(id);
            if (this.mid_list_auto.Contains(transaction.mId))
            {
                SDK_splitIt.CreateSplit(transaction, "5");
                return RedirectToAction(nameof(Index));
            }
        
             else
             {

                String email = transaction.CreditCardInfo.email;
                  string name = transaction.CreditCardInfo.FullName;
                  string CardNumber = transaction.CreditCardInfo.Number;
                  string CardCvv = transaction.CreditCardInfo.CVV;
                  string CardExpMonth = transaction.TransactionDate.Month.ToString();
                  string CardExpYear = transaction.TransactionDate.Year.ToString();
                SDK_splitIt.TestApi( name, email, CardNumber, CardCvv, CardExpMonth, CardExpYear);
              }

                 return View(transaction);
             }
         }
        
    }
}

