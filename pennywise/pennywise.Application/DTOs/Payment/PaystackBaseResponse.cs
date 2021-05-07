using System;
using System.Collections.Generic;
using System.Text;

namespace pennywise.Application.DTOs.Payment
{
    public class PaystackBaseResponse<T>
    {
        public bool status { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}
