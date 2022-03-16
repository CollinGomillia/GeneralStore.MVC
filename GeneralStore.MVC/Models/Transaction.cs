using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeneralStore.MVC.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
        [ForeignKey(nameof(Customer))]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}