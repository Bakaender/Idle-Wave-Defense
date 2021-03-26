using System;

namespace EndlessWaveTD
{
    public enum NumberFormatting
    {
        Scientific,
        Letter
    }

    [Serializable]
    public class BigDouble : IEquatable<BigDouble>
    {
        public static int DigitsToTrack = 10;
        public static int NumDigitsToDisplay = 4;
        public static int ShowScientificAfterExponent = 6;
        public static NumberFormatting numberFormatting;

        public double Number;
        public long Exponent;

        public BigDouble(double number = 0)
        {
            if (number <= 0)
            {
                Number = 0;
                Exponent = 0;
            }
            else if (number < 10)
            {
                Number = number;
                Exponent = 0;
            }
            else
            {
                while (number >= 10)
                {
                    number /= 10;
                    Exponent++;
                }

                Number = number;
            }
        }

        public BigDouble(double number, long exponent)
        {        
            Number = number;
            Exponent = exponent;

            if (Number <= 0)
            {
                Number = 0;
                Exponent = 0;
            }

            //Probably needs adjusted for numbers less than 0.1
            //Probably just While instead of if.
            if (Number < 1 && Exponent > 0)
            {
                Number *= 10;
                Exponent--;
            }

            while (Number >= 10)
            {
                Number /= 10;
                Exponent++;
            }         
        }

        public override string ToString()
        {
            switch (numberFormatting)
            {
                case NumberFormatting.Scientific:
                    return Scientific;
                case NumberFormatting.Letter:
                    return Letter;
                default:
                    return Scientific;
            }
        }

        private string Scientific
        {
            get
            {
                if (Exponent >= ShowScientificAfterExponent)
                {
                    string num = Number.ToString();
                    if (num.Length > NumDigitsToDisplay)
                    {
                        return num.Substring(0, NumDigitsToDisplay + 1) + "e" + Exponent;
                    }
                    else
                    {
                        return num + "e" + Exponent;
                    }
                }
                else
                {
                    return BeforeScientific;
                }
            }
        }

        private string BeforeScientific
        {
            get
            {
                double returnNum = Number;

                for (int i = 0; i < Exponent; i++)
                {
                    returnNum *= 10;
                }

                if (returnNum < 1)
                {
                    return returnNum.ToString("0.###");
                }
                else if (returnNum < 10)
                {
                    return returnNum.ToString("#.###");
                }
                else if (returnNum < 100)
                {
                    return returnNum.ToString("##.##");
                }
                else if (returnNum < 1000)
                {
                    return returnNum.ToString("#,###.#"); //Because 999.99 rounds up to 1000, having the comma keeps everything consistent.
                }

                return returnNum.ToString("#,###,###");
            }
        }

        //May not be fully accurate above 2 letters.
        private string Letter
        {
            get
            {
                if (Exponent > 2)
                {
                    float num = (float)Number;
                    string result = "";
                    long N = (Exponent / 3);

                    if (N > 26)
                    {
                        N -= 1;

                        long First = N / 26;

                        while (First > 26)
                        {
                            First -= 1;
                            result.Insert(0, ((char)(96 + (First % 26) + 1)).ToString());
                            First /= 26;
                        }

                        result.Insert(0, ((char)(96 + First)).ToString());

                        result += ((char)(96 + (N % 26) + 1));
                    }
                    else
                    {
                        result += (char)(96 + N);
                    }

                    num *= (float)Math.Pow(10, Exponent % 3);

                    return num.ToString("####.#") + result;
                }
                else
                {
                    return BeforeScientific;
                }
            }
        }

        public double Double
        {
            get
            {
                double num = Number;
                for (int i = 0; i < Exponent; i++)
                {
                    num *= 10;
                }

                return num;
            }
        }

        public float Float
        {
            get
            {
                return (float)Double;
            }
        }

        public float PercentOf(BigDouble bigDouble)
        {
            BigDouble result = new BigDouble(Number, Exponent) / bigDouble;
            for (int i = 0; i < result.Exponent; i++)
            {
                result.Number *= 10;
            }
            return (float)result.Number;
        }

        //Addition
        public static BigDouble operator +(BigDouble bigDouble1, BigDouble bigDouble2)
        {
            long shiftDigits = bigDouble1.Exponent - bigDouble2.Exponent;

            if (shiftDigits >= DigitsToTrack * -1 && shiftDigits <= DigitsToTrack)
            {
                BigDouble result = new BigDouble();

                if (shiftDigits >= 0)
                {
                    result.Number = bigDouble2.Number;
                    result.Exponent = bigDouble1.Exponent;

                    for (int i = 0; i < shiftDigits; i++)
                    {
                        result.Number /= 10;
                    }

                    result.Number += bigDouble1.Number;

                    if (result.Number >= 10)
                    {
                        result.Number /= 10;
                        result.Exponent++;
                    }
                }
                else
                {
                    shiftDigits *= -1;

                    result.Number = bigDouble1.Number;
                    result.Exponent = bigDouble2.Exponent;

                    for (int i = 0; i < shiftDigits; i++)
                    {
                        result.Number /= 10;
                    }

                    result.Number += bigDouble2.Number;

                    if (result.Number >= 10)
                    {
                        result.Number /= 10;
                        result.Exponent++;
                    }
                }

                return result;
            }

            return shiftDigits > 0 ? bigDouble1 : bigDouble2;
        }

        public static BigDouble operator +(BigDouble bigDouble1, float num2) => bigDouble1 + new BigDouble(num2);

        public static BigDouble operator +(BigDouble bigDouble1, int num2) => bigDouble1 + new BigDouble(num2);

        //Subtraction
        public static BigDouble operator -(BigDouble bigDouble1, BigDouble bigDouble2)
        {
            BigDouble result = new BigDouble();

            long shiftDigits = bigDouble1.Exponent - bigDouble2.Exponent;

            if (shiftDigits > 0) //bigDouble1 exponent > bigDouble2 exponent
            {
                if (shiftDigits >= DigitsToTrack)
                {
                    result.Number = bigDouble1.Number;
                    result.Exponent = bigDouble1.Exponent;
                    return result;
                }
                else
                {
                    result.Number = bigDouble2.Number;
                    result.Exponent = bigDouble1.Exponent;

                    for (int i = 0; i < shiftDigits; i++)
                    {
                        result.Number /= 10;
                    }

                    result.Number = bigDouble1.Number - result.Number;

                    if (result.Number < 1)
                    {
                        result.Number *= 10;
                        result.Exponent--;
                    }

                    return result;
                }
            }
            else if (shiftDigits < 0) //bigDouble1 exponent < bigDouble2 exponent
            {
                //Number below 0.
                return new BigDouble();
            }
            else //bigDouble1 exponent == bigDouble2 exponent
            {
                result.Number = bigDouble1.Number - bigDouble2.Number;
                result.Exponent = bigDouble1.Exponent;

                if (result.Number <= 0)
                {
                    //Number below 0
                    result.Number = 0;
                    result.Exponent = 0;
                }
                else if (result.Number < 1)
                {
                    if (result.Exponent > 0)
                    {
                        result.Number *= 10;
                        result.Exponent--;
                    }
                }

                return result;
            }
        }

        public static BigDouble operator -(BigDouble bigDouble1, float num2) => bigDouble1 - new BigDouble(num2);

        public static BigDouble operator -(BigDouble bigDouble1, int num2) => bigDouble1 - new BigDouble(num2);

        //Multiplication
        public static BigDouble operator *(BigDouble bigDouble1, BigDouble bigDouble2)
        {
            BigDouble result = new BigDouble(bigDouble1.Number * bigDouble2.Number, bigDouble1.Exponent + bigDouble2.Exponent);

            if (result.Number >= 10)
            {
                result.Number /= 10;
                result.Exponent++;
            }

            while (result.Number < 1 && result.Exponent > 0)
            {
                result.Number *= 10;
                result.Exponent--;
            }

            return result;
        }

        public static BigDouble operator *(BigDouble bigDouble1, float num2) => bigDouble1 * new BigDouble(num2);

        public static BigDouble operator *(BigDouble bigDouble1, int num2) => bigDouble1 * new BigDouble(num2);

        //Division
        public static BigDouble operator /(BigDouble bigDouble1, BigDouble bigDouble2)
        {
            BigDouble result = new BigDouble(bigDouble1.Number / bigDouble2.Number, bigDouble1.Exponent - bigDouble2.Exponent);

            while (result.Number >= 10)
            {
                result.Number /= 10;
                result.Exponent++;
            }

            while (result.Exponent < 0)
            {
                result.Number /= 10;
                result.Exponent++;
            }

            return result;
        }

        public static BigDouble operator /(BigDouble bigDouble1, float num2) => bigDouble1 / new BigDouble(num2);

        public static BigDouble operator /(BigDouble bigDouble1, int num2) => bigDouble1 / new BigDouble(num2);

        #region Comparisons

        //Equals & HashCode
        public override bool Equals(object obj) => obj is BigDouble && Equals((BigDouble)obj);

        public bool Equals(BigDouble other) => (Number, Exponent) == (other.Number, other.Exponent);

        public override int GetHashCode() => (Number, Exponent).GetHashCode();

        //BigDouble
        public static bool operator ==(BigDouble double1, BigDouble double2) => double1.Equals(double2);

        public static bool operator !=(BigDouble double1, BigDouble double2) => !(double1 == double2);

        public static bool operator <(BigDouble bigDouble1, BigDouble bigDouble2) => bigDouble1.Exponent < bigDouble2.Exponent
            || bigDouble1.Exponent == bigDouble2.Exponent && bigDouble1.Number < bigDouble2.Number;

        public static bool operator >(BigDouble bigDouble1, BigDouble bigDouble2) => bigDouble1.Exponent > bigDouble2.Exponent
            || bigDouble1.Exponent == bigDouble2.Exponent && bigDouble1.Number > bigDouble2.Number;

        public static bool operator <=(BigDouble bigDouble1, BigDouble bigDouble2) => bigDouble1.Exponent < bigDouble2.Exponent
            || bigDouble1.Exponent == bigDouble2.Exponent && bigDouble1.Number <= bigDouble2.Number;

        public static bool operator >=(BigDouble bigDouble1, BigDouble bigDouble2) => bigDouble1.Exponent > bigDouble2.Exponent
            || bigDouble1.Exponent == bigDouble2.Exponent && bigDouble1.Number >= bigDouble2.Number;

        //Int
        public static bool operator <(BigDouble bigDouble1, int num2) => bigDouble1 < new BigDouble(num2);

        public static bool operator >(BigDouble bigDouble1, int num2) => bigDouble1 > new BigDouble(num2);

        public static bool operator <=(BigDouble bigDouble1, int num2) => bigDouble1 <= new BigDouble(num2);

        public static bool operator >=(BigDouble bigDouble1, int num2) => bigDouble1 >= new BigDouble(num2);

        public static bool operator ==(BigDouble bigDouble1, int num2) => bigDouble1 == new BigDouble(num2);

        public static bool operator !=(BigDouble bigDouble1, int num2) => bigDouble1 != new BigDouble(num2);

        //Float
        public static bool operator <(BigDouble bigDouble1, float num2) => bigDouble1 < new BigDouble(num2);

        public static bool operator >(BigDouble bigDouble1, float num2) => bigDouble1 > new BigDouble(num2);

        public static bool operator <=(BigDouble bigDouble1, float num2) => bigDouble1 <= new BigDouble(num2);

        public static bool operator >=(BigDouble bigDouble1, float num2) => bigDouble1 >= new BigDouble(num2);

        public static bool operator ==(BigDouble bigDouble1, float num2) => bigDouble1 == new BigDouble(num2);

        public static bool operator !=(BigDouble bigDouble1, float num2) => bigDouble1 != new BigDouble(num2);

        //Double
        public static bool operator <(BigDouble bigDouble1, double num2) => bigDouble1 < new BigDouble(num2);

        public static bool operator >(BigDouble bigDouble1, double num2) => bigDouble1 > new BigDouble(num2);

        public static bool operator <=(BigDouble bigDouble1, double num2) => bigDouble1 <= new BigDouble(num2);
                                                             
        public static bool operator >=(BigDouble bigDouble1, double num2) => bigDouble1 >= new BigDouble(num2);
                                                             
        public static bool operator ==(BigDouble bigDouble1, double num2) => bigDouble1 == new BigDouble(num2);
                                                             
        public static bool operator !=(BigDouble bigDouble1, double num2) => bigDouble1 != new BigDouble(num2);

        #endregion
    }
}
