using Merchant.Domain.Expressions;
using System.Collections.Generic;

namespace Merchant.Domain.Commands
{
    public class ConstantCommand : Command
    {
        public ConstantCommand(IList<Expression> sentence) : base(sentence)
        {
        }
    }
}
