using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hack.Model;
using Hack2021.Data;
using Splitit.SDK.Client.Model;

namespace Hack2021.Controllers
{
    public class SplitItTransactionsController : Controller
    {
        private readonly Hack2021Context _context;

        public SplitItTransactionsController(Hack2021Context context)
        {
            _context = context;
        }

        // GET: SplitItTransactions
        public async Task<IActionResult> Index()
        {
            return View(await _context.SplitItTransaction.ToListAsync());
        }

        // GET: SplitItTransactions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var splitItTransaction = await _context.SplitItTransaction
                .FirstOrDefaultAsync(m => m.TransactionID == id);
            if (splitItTransaction == null)
            {
                return NotFound();
            }

            return View(splitItTransaction);
        }

        // GET: SplitItTransactions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SplitItTransactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionID,TotalAmount,NumPayments")] SplitItTransaction splitItTransaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(splitItTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(splitItTransaction);
        }

        public async Task<IActionResult> CreateSplit(int tokenNumbeer, int numberOfPay)
        {
           Transaction token= _context.Transaction.Find(tokenNumbeer);
            SplitItTransaction temp = new SplitItTransaction();
            temp.NumPayments = numberOfPay;
            temp.TotalAmount = token.Amount;
            temp.TransactionID = token.TransactionID.ToString();
            DateTime date = token.TransactionDate;
            double onePay = token.Amount / numberOfPay;
            for (int i = 1; i <= numberOfPay; i++)
            {
                SplitItTransaction.Payment tamPay = new SplitItTransaction.Payment();
                tamPay.TransactionID= token.TransactionID.ToString();
                tamPay.DueDate = date;
                tamPay.Amount = onePay;
                temp.Payments.Add(tamPay);
                date.AddMonths(1);
            }

            _context.Add(temp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: SplitItTransactions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var splitItTransaction = await _context.SplitItTransaction.FindAsync(id);
            if (splitItTransaction == null)
            {
                return NotFound();
            }
            return View(splitItTransaction);
        }

        // POST: SplitItTransactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TransactionID,TotalAmount,NumPayments")] SplitItTransaction splitItTransaction)
        {
            if (id != splitItTransaction.TransactionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(splitItTransaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SplitItTransactionExists(splitItTransaction.TransactionID))
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
            return View(splitItTransaction);
        }

        // GET: SplitItTransactions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var splitItTransaction = await _context.SplitItTransaction
                .FirstOrDefaultAsync(m => m.TransactionID == id);
            if (splitItTransaction == null)
            {
                return NotFound();
            }

            return View(splitItTransaction);
        }

        // POST: SplitItTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var splitItTransaction = await _context.SplitItTransaction.FindAsync(id);
            _context.SplitItTransaction.Remove(splitItTransaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SplitItTransactionExists(string id)
        {
            return _context.SplitItTransaction.Any(e => e.TransactionID == id);
        }



    }

    
}
