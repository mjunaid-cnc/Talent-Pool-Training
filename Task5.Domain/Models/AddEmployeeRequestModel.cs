﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task5.Domain.Models
{
    public class AddEmployeeRequestModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [Required]
        [Range(1, 1000000)]
        public double Salary { get; set; }
        [EmailAddress]
        public string Email { get; set; }

    }
}
