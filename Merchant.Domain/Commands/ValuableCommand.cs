using Merchant.Domain.Expressions;
using System.Collections.Generic;

namespace Merchant.Domain.Commands
{
    public class ValuableCommand : Command
    {
        public ValuableCommand(IList<Expression> sentence) : base(sentence)
        {
        }
    }
}
