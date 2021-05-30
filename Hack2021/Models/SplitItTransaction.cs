using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hack.Model
{
    public class SplitItTransaction
    {
        [Key]
        public string TransactionID { get; set; }

        public double TotalAmount { get; set; }

        public int NumPayments { get; set; }

        public List<Payment> Payments { get; set; }

        public class Payment
        {

            public DateTime DueDate { get; set; }
            public double Amount { get; set; }

        }
    }
}
