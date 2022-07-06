using System;
using System.Text.RegularExpressions;

namespace ParseSums
{
    public class ParseExpression
    {
        static void Main(string[] args)
        {
            string expression = "1 / 2";
            ParseExpression sumParser = new ParseExpression();
            bool parseResult = sumParser.Evaluate(expression, out int sumResult);
            Console.WriteLine(parseResult + " " + sumResult);
        }

        public string EvaluateMatchSum(Match match)
        {
            // Get the string that has been matched with the regex
            string sumToCheck = match.Value;

            // Remove any brackets
            if(sumToCheck[0] == '(')
            {
                sumToCheck = sumToCheck.Substring(1);
            }

            if(sumToCheck[sumToCheck.Length - 1] == ')')
            {
                sumToCheck = sumToCheck.Substring(0, sumToCheck.Length - 1);
            }

            // Split the sum on the symbol
            char[] symbols = { '+', '-', '*', '/' };
            string[] parts = sumToCheck.Split(symbols);

            bool parsed = Int32.TryParse(parts[0].Trim(), out int firstNumberParsed);

            if (!parsed)
            {
                return sumToCheck;
            }

            bool parsed2 = Int32.TryParse(parts[1].Trim(), out int secondNumParsed);

            if (!parsed2)
            {
                return sumToCheck;
            }

            // Perform the calulation based on the symbol
            char symbol = sumToCheck[parts[0].Length];
            int result;
            switch (symbol)
            {
                case '+':
                    result = firstNumberParsed + secondNumParsed;
                    break;
                case '-':
                    result = firstNumberParsed - secondNumParsed;
                    break;
                case '*':
                    result = firstNumberParsed * secondNumParsed;
                    break;
                case '/':
                    if(secondNumParsed == 0)
                    {
                        return sumToCheck;
                    }
                    result = firstNumberParsed / secondNumParsed;
                    break;
                default:
                    return sumToCheck;
            }

            return result.ToString();
        }

        public bool Evaluate(string expression, out int result)
        {
            result = 0;

            // Check if the number of opening brackets and closing brackets are the same
            if(expression.Split('(').Length != expression.Split(')').Length)
            {
                return false;
            }

            // This regular expression checks for a sum in the format (Number Symbol Number) with optional brackets and whitespace
            Regex regularExp = new Regex(@"\(?\d+\s*[+\-*/]{1}\s*\d+\)?");
            string sumToCheck = expression;
            string prev = "";

            while (!sumToCheck.Equals(prev))
            {
                prev = sumToCheck;
                // Replace the first occurence of (Number Symbol Number) with the answer of the sum. 
                sumToCheck = regularExp.Replace(sumToCheck, EvaluateMatchSum, 1);
            }

            // Get rid of excess brackets
            sumToCheck = Regex.Replace(sumToCheck, @"[\(\)]", "");

            bool parsed = Int32.TryParse(sumToCheck, out result);
            if (!parsed)
            {
                return false;
            }

            return true;
        }
    }
}
