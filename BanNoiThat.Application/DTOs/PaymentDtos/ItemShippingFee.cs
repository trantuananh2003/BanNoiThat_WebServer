using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanNoiThat.Application.DTOs.PaymentDtos
{
    public class ItemShippingFee
    {
        public string Name { get; set; }
        public int? Quantity { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public int? Lenght { get; set; }
        public int? Width { get; set; }
    }
}
