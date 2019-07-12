using Merchant.Domain.Expressions;
using System.Collections.Generic;

namespace Merchant.Domain.Commands
{
    public class Command
    {
        public Command(IList<Expression> sentence)
        {
            Sentence = sentence;
        }

        public IList<Expression> Sentence { get; protected set; }
    }
}
