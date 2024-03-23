using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eticaret.Models
{
    public class Review
    {

        public int Id { get; set; }


        [DisplayName("Değerlendirenin Idsi")]
        [Required]
        public int UserId { get; set; }

        [DisplayName("Müşteri Yorumu")]
        [Required]
        public string Comment { get; set; }

        
        [DisplayName("Değerlendirenin Zamanı")]
        [Required]
        public DateTime ReviewTime { get; set; }


        [Required]
        public float Rating { get; set; }

        

    }
}