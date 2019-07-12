using Merchant.Domain.Expressions;
using System.Collections.Generic;

namespace Merchant.Domain.Commands
{
    public class QuestionCommand : Command
    {
        public QuestionCommand(IList<Expression> sentence) : base(sentence)
        {
        }
    }
}
