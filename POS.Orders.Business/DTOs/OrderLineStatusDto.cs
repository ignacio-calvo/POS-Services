﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace POS.Orders.Business.DTOs
{
    public class OrderLineStatusDto
    {
        [Key]
        public byte Id { get; set; }

        public string? Status { get; set; }
    }
}
