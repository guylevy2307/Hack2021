using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hack.Model
{
    public class SplitItTransaction
    {
        public string TransactionID { get; set; }

        public double TotalAmount { get; set; }

        public int NumPayments { get; set; }

        public List<Payment> Payments { get; set; }

 
    }
}
