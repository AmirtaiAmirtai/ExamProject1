﻿using ExamProject.Models;

namespace ExamProject1.Models
{
    public class Sale
    {

        public int Id { get; set; }
        public required int LeadId { get; set; }
        public required int? SellerId { get; set; }
        public required DateTime AgreementDate { get; set; }
        public Lead Lead { get; set; }
        public User Seller { get; set; }
    }
}
