using Domain.Common.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Quote
{
    public class QuoteExpiredExceptions : DomainException
    {
        public QuoteExpiredExceptions()
            : base(ErrorCodes.Quote.Expired, "Quote expired")
        { }
    }
}
