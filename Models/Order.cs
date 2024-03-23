using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace eticaret.Models
{
    public class Order
    {
        public int Id { get; set; }

        [DisplayName("Sipariş ID")]
        public int OrderId { get; set; }

        [DisplayName("Ürün ID")]
        public string ProductID { get; set; }

        [DisplayName("Adet")]
        public int piece { get; set; }



        [DisplayName("Satıcı ID")]
        public int SellerId { get; set; }

        [DisplayName("Müşteri ID")]
        public int CustomerId { get; set; }

        [DisplayName("Sipariş Zamanı")]
        public DateTime orderTime { get; set; }




        [DisplayName("Durum")]
        public string status { get; set; }
    }
}