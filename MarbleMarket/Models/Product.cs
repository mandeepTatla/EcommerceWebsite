using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MarbleMarket.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
       
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        [Range(1, Double.MaxValue)]

        public double Price { get; set; }

        public string ImageUrl { get; set; }
        [Display(Name = "Category Type")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
       
        
        [Display(Name = "Application Type")]
        public int ApplicationTypeId { get; set; }
        [ForeignKey("ApplicationTypeId")]
        public virtual ApplicationType ApplicationType { get; set; }
    }
}
