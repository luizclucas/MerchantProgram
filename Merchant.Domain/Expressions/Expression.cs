namespace Merchant.Domain.Expressions
{
    public class Expression
    {
        public Expression(string name, ExpressionType type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; protected set; }
        public ExpressionType Type { get; protected set; }

    }
}
