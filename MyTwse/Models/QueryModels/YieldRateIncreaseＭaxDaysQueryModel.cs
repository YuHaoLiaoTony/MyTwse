using System;
using System.ComponentModel.DataAnnotations;

namespace MyTwse.Models.QueryModels
{
    public class YieldRateIncreaseＭaxDaysQueryModel
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
    }
}
