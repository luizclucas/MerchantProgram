using Merchant.Data.Repositories;
using Merchant.Domain;
using Merchant.Domain.Commands;
using Merchant.Domain.Constants;
using Merchant.Domain.Expressions;
using Merchant.Domain.Roman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Merchant.Core
{
    public class Calculate
    {
        private Repository _repository;
        public Calculate(Repository repository)
        {
            _repository = repository;
        }

        public CommandResponse SaveConstant(ConstantCommand command)
        {
            var constantExpression = command.Sentence.FirstOrDefault(p => p.Type == ExpressionType.Constant);

            if (_repository.GetValuableTable().Any(p => p.Key.Name.ToUpper() == constantExpression.Name.ToUpper()))
            {
                throw new DuplicateDeclarationException();
            }

            var romanExpression = command.Sentence.FirstOrDefault(p => p.Type == ExpressionType.Roman);

            _repository.SaveToConstantTable(constantExpression, romanExpression);

            return new CommandResponse
            {
                Information = String.Format("Information Saved for constant: \"{0}\"", command.Sentence.FirstOrDefault().Name),
                Success = true
            };
        }

        public CommandResponse SaveValuableCommand(ValuableCommand command)
        {
            var valuable = command.Sentence.FirstOrDefault(p => p.Type == ExpressionType.Valuable);

            if (_repository.GetValuableTable().Any(p => p.Key.Name.ToUpper() == valuable.Name.ToUpper()))
            {
                throw new DuplicateDeclarationException();
            }

            var unit = (UnitExpression)command.Sentence.FirstOrDefault(p => p.Type == ExpressionType.Unit);
            var value = Convert.ToDouble(command.Sentence.FirstOrDefault(p => p.Type == ExpressionType.Number).Name);
            unit.MultiplierFactor = CalculateUnitFactor(command.Sentence.Where(p => p.Type == ExpressionType.Constant).ToList(), value);

            _repository.SaveToValuableTable(valuable, unit);
     
            return new CommandResponse
            {
                Information = String.Format("Information Saved for valuable: \"{0}\"", valuable.Name),
                Success = true
            };
        }

        public CommandResponse CalculateQuestion(QuestionCommand command)
        {
            var queryType = command.Sentence.FirstOrDefault(p => p.Type == ExpressionType.SubQuestion).Name;

            string messageText = null;

            var constants = command.Sentence.Where(p => p.Type == ExpressionType.Constant).ToList();
            var value = GetDecimalValue(constants);
            var constantsName = string.Join(" ", constants.Select(c => c.Name.ToString()));

            if (queryType == Keys.SubQuestion.Much)
            {
                messageText = string.Format("{0} is {1}", constantsName, value);
            }
            else if (queryType == Keys.SubQuestion.Many)
            {
                var valuable = command.Sentence.FirstOrDefault(p => p.Type == ExpressionType.Valuable);
                var unit = command.Sentence.FirstOrDefault(p => p.Type == ExpressionType.Unit) as UnitExpression;

                var valuableAtTable = _repository.GetValuableTable().FirstOrDefault(p => p.Key.Name == valuable.Name && p.Key.Type == valuable.Type);
                value *= valuableAtTable.Value.MultiplierFactor;

                messageText = string.Format("{0} {1} is {2} {3}", constantsName, valuable.Name, value, unit.Name);
            }
            else
            {
                throw new Exception();
            }

            return new CommandResponse
            {
                Information = messageText,
                Success = true
            };
        }

        private double GetDecimalValue(IList<Expression> constants)
        {
            var romanNumber = new StringBuilder();

            foreach (var constant in constants)
            {
                romanNumber.Append(_repository.GetConstantsTable().FirstOrDefault(p => p.Key.Name == constant.Name).Value.Name);
            }            
            return RomanToDecimalConverter.Convert(romanNumber.ToString());
        }

        public double CalculateUnitFactor(IList<Expression> constants, double value)
        {
            return value / GetDecimalValue(constants);
        }
    }
}
