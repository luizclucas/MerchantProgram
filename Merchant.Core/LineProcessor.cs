using Merchant.Domain;
using Merchant.Domain.Commands;
using Merchant.Domain.Constants;
using Merchant.Domain.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Merchant.Core
{
    public class LineProcessor
    {

        private Calculate _calculate;
        public LineProcessor(Calculate calculate)
        {
            _calculate = calculate;
        }

        public CommandResponse ParseAndCalculate(string sentence)
        {
            try
            {
                var command = Parse(sentence);

                if (command is ConstantCommand)
                {
                    return _calculate.SaveConstant(command as ConstantCommand);
                }
                if (command is QuestionCommand)
                {
                    return _calculate.CalculateQuestion(command as QuestionCommand);

                }
                if (command is ValuableCommand)
                {
                    return _calculate.SaveValuableCommand(command as ValuableCommand);
                }
            }
            catch (DuplicateDeclarationException)
            {
                return new CommandResponse { Information = @"You already said it", Success = false };
            }
            catch (Exception e)
            {
                return new CommandResponse { Information = @"I have no idea what you are talking about", Success = false };
            }

            throw new NotSupportedException(@"Command Not Supported");
        }

        private Command Parse(string wholeSentence)
        {
            IList<Expression> sentence = GetSentence(wholeSentence.Split(' '));

            if (IsQuestionSentence(sentence))
                return new QuestionCommand(sentence);

            if (IsValuableSentence(sentence))
                return new ValuableCommand(sentence);

            if (IsConstantSentence(sentence))
                return new ConstantCommand(sentence);

            throw new LineParsingException();
        }

        private IList<Expression> GetSentence(string[] words)
        {
            var characterList = new List<Expression>();

            foreach (var word in words)
            {
                switch (word)
                {
                    case Keys.Operators.Is:
                        characterList.Add(new Expression(word, ExpressionType.Operator));
                        break;
                    case Keys.RomanCharacters.I:
                    case Keys.RomanCharacters.V:
                    case Keys.RomanCharacters.X:
                    case Keys.RomanCharacters.L:
                    case Keys.RomanCharacters.C:
                    case Keys.RomanCharacters.D:
                    case Keys.RomanCharacters.M:
                        characterList.Add(new Expression(word, ExpressionType.Roman));
                        break;
                    case Keys.Question.How:
                        characterList.Add(new Expression(word, ExpressionType.Question));
                        break;
                    case Keys.SubQuestion.Many:
                    case Keys.SubQuestion.Much:
                        characterList.Add(new Expression(word, ExpressionType.SubQuestion));
                        break;
                    case Keys.QuestionMark.Quetions:
                        characterList.Add(new Expression(word, ExpressionType.QuestionMark));
                        break;
                    case Keys.Valuable.Gold:
                    case Keys.Valuable.Silver:
                    case Keys.Valuable.Iron:
                        characterList.Add(new Expression(word, ExpressionType.Valuable));
                        break;
                    case Keys.Unit.Credits:
                        characterList.Add(new UnitExpression(word));
                        break;

                    default:
                        int number = 0;
                        try
                        {
                             number = Convert.ToInt32(word);
                        }
                        catch (Exception)
                        {
                        }

                        if(number == 0)
                        {
                            characterList.Add(new Expression(word, ExpressionType.Constant));
                        }
                        else
                        {
                            characterList.Add(new Expression(word, ExpressionType.Number));
                        }
                        break;

                }
            }

            return characterList;
        }

        private static bool IsQuestionSentence(IList<Expression> expressions)
        {
            return expressions.First().Type == ExpressionType.Question
                   && expressions.ElementAt(1).Type == ExpressionType.SubQuestion
                   && expressions.Any(s => s.Type == ExpressionType.Constant)
                   && expressions.Any(s => s.Type == ExpressionType.Operator)
                   && expressions.Last().Type == ExpressionType.QuestionMark;
        }

        private static bool IsValuableSentence(IList<Expression> expressions)
        {
            return expressions.First().Type == ExpressionType.Constant
                   && expressions.Any(s => s.Type == ExpressionType.Valuable)
                   && expressions.Any(s => s.Type == ExpressionType.Operator)
                   && expressions.Any(s => s.Type == ExpressionType.Number)
                   && expressions.Last().Type == ExpressionType.Unit;
        }

        private static bool IsConstantSentence(IList<Expression> expressions)
        {
            return expressions.Count == 3
                   && expressions.First().Type == ExpressionType.Constant
                   && expressions.ElementAt(1).Type == ExpressionType.Operator
                   && expressions.Last().Type == ExpressionType.Roman;
        }
    }
}
