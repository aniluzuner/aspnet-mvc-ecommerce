using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eticaret.Models
{
    public class Product
    {
        public int Id { get; set; }

        [DisplayName("Marka Id")]
        [Required]
        public int BrandID { get; set; }

        [DisplayName("Ürün Adı")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Fiyat")]
        [Required]
        public int Price { get; set; }

        [DisplayName("Stok")]
        [Required]
        public int Stock { get; set; }

        [DisplayName("Değerlendirme")]
        public float Rating { get; set; }

        [DisplayName("KategoriID")]
        public int CategoryID { get; set; }
        
        [DisplayName("Alt Kategori ID")]
        public int SubCategoryID { get; set; }

        [DisplayName("SatıcıID")]
        public int SellerID { get; set; }

        [DisplayName("Onaylandı mı ?")]
        public bool isApproved { get; set; }

        [DisplayName("Açıklama")]
        [Required]
        public string Detail { get; set; }

        
     
    }
}