using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Entities.DataTransferObject
{
    public abstract class CategoryForManipulationDto
    {
        public int categoryId { get; set; }

        [Required(ErrorMessage = "CategoryName is required")]
        [MaxLength(40, ErrorMessage = "Maximum length for categoryId is 40 characters")]
        public string categoryName { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(40, ErrorMessage = "Maximum length for categoryId is 40 characters")]
        public string description { get; set; }
    }
}
