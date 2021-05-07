using System;
using System.Collections.Generic;
using System.Text;

namespace pennywise.Application.Features.Plan.Queries
{
    public class GetAllPlansViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool isActivated { get; set; }
        public DateTime? ActivationDate { get; set; }
        public double Amount { get; set; }
    }
}
