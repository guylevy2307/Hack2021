using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hack.Model
{
    public class Transaction
    {
        public Transaction()
        {
            this.Status = StatusEnum.AUTO;

        }
        [Key]
        public int TransactionID { get; set; }

        public double Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        public StatusEnum Status { get; set; }

        public String CardNumber  { get; set; }

        public virtual CreditCard CreditCardInfo { get; set; }

        public String mId { get; set; }

        public enum StatusEnum 
        { 
            AUTO,
            CAPTURE,
            VOID,
            REFUND
        }


    }
}
