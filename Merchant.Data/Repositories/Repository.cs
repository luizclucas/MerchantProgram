using Merchant.Domain.Expressions;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace Merchant.Data.Repositories
{
    public class Repository
    {
        private Dictionary<Expression, Expression> ConstantsTable { get; set; }
        private Dictionary<Expression, UnitExpression> ValuablesTable { get; set; }

        public Repository()
        {
            ConstantsTable = new Dictionary<Expression, Expression>();
            ValuablesTable = new Dictionary<Expression, UnitExpression>();
        }

        public Dictionary<Expression, Expression> GetConstantsTable()
        {            
            return ConstantsTable;     
        }

        public void SaveToConstantTable(Expression constant, Expression roman)
        {   
            ConstantsTable.Add(constant, roman);
        }

        public Dictionary<Expression, UnitExpression> GetValuableTable()
        {
            return ValuablesTable;
        }

        public void SaveToValuableTable(Expression valuable, UnitExpression unit)
        {
            ValuablesTable.Add(valuable, unit);
        }
    }
}
