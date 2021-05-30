using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hack.Model
{
    public class CreditCard
    {
        public string Number { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string CVV { get; set; }
    }
}