using System;
using System.Linq;

namespace WCFService.Extensions
{
    public static class CurrencyConverter
    {
        const int INT_MAX = 999999999;
        public static string ConvertStringNumberToWords(this string numberString)
        {
            if (string.IsNullOrEmpty(numberString)) return string.Empty;
            if (numberString.Count(x => x == ',' || x == '.') > 1) throw new FormatException("The number is not valid!");

            decimal value = 0m;
            var canConvert = Decimal.TryParse(numberString.Replace(',', '.').Replace(" ", ""), out value);

            if (!canConvert) throw new FormatException("Input string was not in a correct format.");

            decimal intValue = Math.Truncate(value);
            decimal fractionValue = value - intValue;

            if (intValue > INT_MAX) throw new NotSupportedException($"The maximum number is {INT_MAX}.");

            if (fractionValue.ToString().Length > 4) throw new FormatException("The maximum number of cents is 99 (2 digit)!");

            return $"{ConvertNumberToString(intValue)} {(intValue == 1 ? "dollar" : "dollars")}" +
                (fractionValue > 0 ? $" and {ConvertNumberToString(fractionValue * 100)} {(fractionValue == 0.01m ? "cent" : "cents")}" : "");
        }
        private static string ConvertNumberToString(decimal n)
        {
            if (n < 0)
                throw new NotSupportedException("negative numbers not supported");
            if (n == 0)
                return "zero";
            if (n < 10)
                return ConvertDigitToString(n);
            if (n < 20)
                return ConvertTeensToString(n);
            if (n < 100)
                return ConvertHighTensToString(n);
            if (n < 1000)
                return ConvertBigNumberToString(n, (int)1e2, "hundred");
            if (n < 1000000)
                return ConvertBigNumberToString(n, (int)1e3, "thousand");
            //if (n <= 999999999)
                return ConvertBigNumberToString(n, (int)1e6, "million");
            //if (n < 1e12)
            //return ConvertBigNumberToString(n, (int)1e9, "billion");
        }
        /// <summary>
        /// This is the primary conversion method which can convert any integer bigger than 99
        /// </summary>
        /// <param name="n">The numeric value of the integer to be translated ("textified")</param>
        /// <param name="baseNum">Represents the order of magnitude of the number (e.g., 100 or 1000 or 1e6, etc)</param>
        /// <param name="baseNumStr">The string representation of the base number (e.g. "hundred", "thousand", or "million", etc)</param>
        /// <returns>Textual representation of any integer</returns>
        private static string ConvertBigNumberToString(decimal n, int baseNum, string baseNumStr)
        {
            // Strategy: translate the first portion of the number, then recursively translate the remaining sections.
            // Step 1: strip off first portion, and convert it to string:
            decimal bigPart = (decimal)(Math.Floor((double)n / baseNum));
            string bigPartStr = ConvertNumberToString(bigPart) + " " + baseNumStr;
            // Step 2: check to see whether we're done:
            if (n % baseNum == 0) return bigPartStr;
            // Step 3: concatenate 1st part of string with recursively generated remainder:
            decimal restOfNumber = n - bigPart * (decimal)baseNum;
            return $"{bigPartStr} {ConvertNumberToString(restOfNumber)}";
        }
        /// <summary>
        /// assumes a number between 1 & 9
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static string ConvertDigitToString(decimal i)
        {
            switch (i)
            {
                case 0: return "";
                case 1: return "one";
                case 2: return "two";
                case 3: return "three";
                case 4: return "four";
                case 5: return "five";
                case 6: return "six";
                case 7: return "seven";
                case 8: return "eight";
                case 9: return "nine";
                default:
                    throw new IndexOutOfRangeException(String.Format("{0} not a digit", i));
            }
        }

        /// <summary>
        /// assumes a number between 10 & 19
        /// </summary>
        /// <param name="n">Number between(10-19)</param>
        /// <returns></returns>
        private static string ConvertTeensToString(decimal n)
        {
            switch (n)
            {
                case 10: return "ten";
                case 11: return "eleven";
                case 12: return "twelve";
                case 13: return "thirteen";
                case 14: return "fourteen";
                case 15: return "fiveteen";
                case 16: return "sixteen";
                case 17: return "seventeen";
                case 18: return "eighteen";
                case 19: return "nineteen";
                default:
                    throw new IndexOutOfRangeException(String.Format("{0} not a teen", n));
            }
        }

        /// <summary>
        /// assumes a number between 20 and 99
        /// </summary>
        /// <param name="n">Number between(20-99)</param>
        /// <returns></returns>
        private static string ConvertHighTensToString(decimal n)
        {
            int tensDigit = (int)(Math.Floor((double)n / 10.0));

            string tensStr;
            switch (tensDigit)
            {
                case 2: tensStr = "twenty"; break;
                case 3: tensStr = "thirty"; break;
                case 4: tensStr = "forty"; break;
                case 5: tensStr = "fifty"; break;
                case 6: tensStr = "sixty"; break;
                case 7: tensStr = "seventy"; break;
                case 8: tensStr = "eighty"; break;
                case 9: tensStr = "ninety"; break;
                default:
                    throw new IndexOutOfRangeException(String.Format("{0} not in range 20-99", n));
            }
            if (n % 10 == 0) return tensStr;
            string onesStr = ConvertDigitToString(n - tensDigit * 10);
            return tensStr + "-" + onesStr;
        }

    }
}
