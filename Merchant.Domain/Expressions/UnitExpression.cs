namespace Merchant.Domain.Expressions
{
    public class UnitExpression : Expression
    {
        public double MultiplierFactor { get; set; }
        public UnitExpression(string name) : base(name, ExpressionType.Unit)
        {
        }
    }
}
