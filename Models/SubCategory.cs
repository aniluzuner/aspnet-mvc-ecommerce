using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eticaret.Models
{
    public class SubCategory
    {
        public int Id { get; set; }

        [DisplayName("Kategori Adı")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Üst Kategori ID'si")]
        [Required]
        public string ParentCategoryID { get; set; }

    }
}