﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Domain.Models
{
    public class Response
    {
        public string? Message { get; set; }
        public object? Content { get; set; }
        public int StatusCode { get; set; }
    }
}
