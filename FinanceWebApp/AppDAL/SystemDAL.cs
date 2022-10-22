using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;


namespace MudasirRehmanAlp.AppDAL
{
    public class SystemDAL
    {
        private string[] numbers = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        private string[] tens = { "", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = BitConverter.ToString(adapter.GetPhysicalAddress().GetAddressBytes()).Replace('-', ':');

                }
            }
            return sMacAddress;
        }
        public string GenerateAmountInWord(string AmountLabel)
        {
            string Amount = "";
            try
            {
                decimal numberrs = Convert.ToDecimal(AmountLabel);
                string input = AmountLabel;
                string a = "";
                string b = "";

                // take decimal part of input. convert it to word. add it at the end of method.
                string decimals = "";

                if (input.Contains("."))
                {
                    decimals = input.Substring(input.IndexOf(".") + 1);
                    // remove decimal part from input
                    input = input.Remove(input.IndexOf("."));
                }
                string strWords = getAmountInWord(Convert.ToInt64(input));

                if (!AmountLabel.Contains("."))
                {
                    a = strWords + " Rupees Only";
                }
                else
                {
                    a = strWords + " Rupees";
                }

                if (decimals.Length > 0)
                {
                    // if there is any decimal part convert it to words and add it to strWords.
                    int Paisa = Convert.ToInt32(decimals);
                    if (Paisa != 0)
                    {
                        string strwords2 = getAmountInWord(Convert.ToInt64(decimals));
                        b = " and " + strwords2 + " Paisa Only ";
                    }
                    else
                    {
                        b = " Only";
                    }
                }
                Amount = a + b;
            }
            catch (Exception)
            {
                Amount = "";
            }
            return Amount;
        }
        public string getAmountInWord(long inputNumber)
        {
            long inputNo = inputNumber;
            if (inputNo == 0)
                return "Zero";

            long[] numbers = new long[4];
            long first = 0;
            long u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (inputNo < 0)
            {
                sb.Append("Minus ");
                inputNo = -inputNo;
            }

            string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
            "Five " ,"Six ", "Seven ", "Eight ", "Nine "};
            string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
            "Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};
            string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
            "Seventy ","Eighty ", "Ninety "};
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };

            numbers[0] = inputNo % 1000; // units
            numbers[1] = inputNo / 1000;
            numbers[2] = inputNo / 100000;
            numbers[1] = numbers[1] - 100 * numbers[2]; // thousands
            numbers[3] = inputNo / 10000000; // crores
            numbers[2] = numbers[2] - 100 * numbers[3]; // lakhs

            for (int i = 3; i > 0; i--)
            {
                if (numbers[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (long i = first; i >= 0; i--)
            {
                if (numbers[i] == 0) continue;
                u = numbers[i] % 10; // ones
                t = numbers[i] / 10;
                h = numbers[i] / 100; // hundreds
                t = t - 10 * h; // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i == 0) sb.Append("");
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }

        public string NumberToWords(long number)
        {

            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += " ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
        public string convertToString(long number)
        {
            char[] digits = number.ToString().ToCharArray();
            string words = null;

            if (number >= 0 && number <= 19)
            {
                words = words + this.numbers[number];
            }
            else if (number >= 20 && number <= 99)
            {
                int firstDigit = (int)Char.GetNumericValue(digits[0]);
                long secondPart = number % 10;

                words = words + this.tens[firstDigit];

                if (secondPart > 0)
                {
                    words = words + " " + convertToString(secondPart);
                }
            }
            else if (number >= 100 && number <= 999)
            {
                int firstDigit = (int)Char.GetNumericValue(digits[0]);
                long secondPart = number % 100;

                words = words + this.numbers[firstDigit] + " hundred";

                if (secondPart > 0)
                {
                    words = words + " and " + convertToString(secondPart);
                }
            }
            else if (number >= 1000 && number <= 19999)
            {
                int firstPart = (int)Char.GetNumericValue(digits[0]);
                if (number >= 10000)
                {
                    string twoDigits = digits[0].ToString() + digits[1].ToString();
                    firstPart = Convert.ToInt16(twoDigits);
                }
                long secondPart = number % 1000;

                words = words + this.numbers[firstPart] + " thousand";

                if (secondPart > 0 && secondPart <= 99)
                {
                    words = words + " and " + convertToString(secondPart);
                }
                else if (secondPart >= 100)
                {
                    words = words + " " + convertToString(secondPart);
                }
            }
            else if (number >= 20000 && number <= 999999)
            {
                string firstStringPart = Char.GetNumericValue(digits[0]).ToString() + Char.GetNumericValue(digits[1]).ToString();

                if (number >= 100000)
                {
                    firstStringPart = firstStringPart + Char.GetNumericValue(digits[2]).ToString();
                }

                int firstPart = Convert.ToInt16(firstStringPart);
                long secondPart = number - (firstPart * 1000);

                words = words + convertToString(firstPart) + " thousand";

                if (secondPart > 0 && secondPart <= 99)
                {
                    words = words + " and " + convertToString(secondPart);
                }
                else if (secondPart >= 100)
                {
                    words = words + " " + convertToString(secondPart);
                }

            }
            else if (number == 1000000)
            {
                words = words + "One million";
            }

            return words;
        }
    }
}