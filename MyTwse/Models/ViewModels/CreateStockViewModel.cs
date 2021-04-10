using System;
using System.ComponentModel.DataAnnotations;

namespace MyTwse.Models.ViewModels
{
    public class CreateStockViewModel
    {
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
    }
}
