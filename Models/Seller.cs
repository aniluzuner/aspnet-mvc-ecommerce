using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eticaret.Models
{
    public class Seller
    {
        public int Id { get; set; }

        [DisplayName("Mağaza Adı")]
        [Required]
        public string Name { get; set; }

        [DisplayName("E-posta")]
        [Required]
        public string Email { get; set; }

        [DisplayName("Şifre")]
        [Required]
        public string Password { get; set; }


        public bool isApproved { get; set; }

    }
}