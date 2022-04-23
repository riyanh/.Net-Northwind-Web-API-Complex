using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Entities.DataTransferObject
{
    public class CategoryUpdateDto
    {
        [Required(ErrorMessage = "CategoryName is required")]
        [MaxLength(40, ErrorMessage = "Maximum length for categoryId is 40 characters")]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(40, ErrorMessage = "Maximum length for categoryId is 40 characters")]
        public string Description { get; set; }
    }
}
