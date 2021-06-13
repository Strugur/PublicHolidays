using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PublicHolidays.Data.Models
{
    public class Region
    {
        public int Id { get; set; }
        [Required]
        [StringLength(3)]
        public string Code { get; set; }

    }
}
