using Domain.Common.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Coin
{
    public class InsufficientStockException : DomainException
    {
        public InsufficientStockException(int available, int requested)
            : base(ErrorCodes.Coin.InsufficientStock,
                   $"موجودی کافی نیست. موجودی فعلی: {available}، درخواستی: {requested}")
        { }
    }

}
