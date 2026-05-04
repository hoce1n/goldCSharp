using Domain.Common.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Coin
{
    public class InvalidStockException : DomainException
    {
        public InvalidStockException()
            : base(ErrorCodes.Coin.InvalidStock, "موجودی نمی‌تواند منفی باشد.") { }
    }
}
