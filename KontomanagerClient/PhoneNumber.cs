using System.Text.RegularExpressions;

namespace KontomanagerClient
{

    public class PhoneNumber
    {
        public PhoneNumber(string number)
        {
            RawNumber = number;
            Number = NormalizePhoneNumber(number);
        }
        
        /// <summary>
        /// The raw string used to initialize the phone number, which may contain formatting characters like "+", "-", "/", or spaces.
        /// </summary>
        public string RawNumber { get; }
        public string Number { get; }
        
        /// <summary>
        /// User chosen name for the number.
        /// </summary>
        public string Name { get; set; }
        public string SubscriberId { get; set; }
        
        public bool Selected { get; set; }
        
        /// <summary>
        /// Output the phone number in the specified format.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToFormattedPhoneNumber(PhoneNumberFormat format)
        {
            switch (format)
            {
                case PhoneNumberFormat.International:
                    return "+" + Number;
                case PhoneNumberFormat.Local:
                    if (Number.StartsWith("43"))
                    {
                        return "0" + Number.Substring(2);
                    }
                    return Number; // If it doesn't start with 43, we assume it's already in local format
                case PhoneNumberFormat.LocalWithSeparator:
                    if (Number.StartsWith("43"))
                    {
                        return "0" + Number.Substring(2, 3) + "/" + Number.Substring(5);
                    }
                    return Number; // If it doesn't start with 43, we assume it's already in local format
                case PhoneNumberFormat.NumericOnly:
                    return Number;
                default:
                    return Number;
            }
        }

        public override string ToString()
        {
            if (Selected) return $"{Number} (Selected)";
            return Number;
        }

        public override bool Equals(object obj)
        {
            if (obj is PhoneNumber other)
            {
                return NormalizePhoneNumber(Number) == NormalizePhoneNumber(other.Number);
            }
            return base.Equals(obj);
        }
        
        public override int GetHashCode()
        {
            return NormalizePhoneNumber(Number).GetHashCode();
        }
        
        public static string NormalizePhoneNumber(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            // 1. Remove all non-numeric characters (/, -, spaces, etc.)
            // We keep the '+' temporarily to identify international format
            string clean = Regex.Replace(input, @"[^0-9+]", "");

            // 2. Handle the "+" prefix
            if (clean.StartsWith("+"))
            {
                return clean.Substring(1);
            }

            // 3. Handle local format (starting with 0) 
            // Example: 0681... becomes 43681...
            if (clean.StartsWith("0"))
            {
                return "43" + clean.Substring(1);
            }

            return clean;
        }
    }

    public enum PhoneNumberFormat
    {
        /// <summary>
        /// +436811234567
        /// </summary>
        International,
        /// <summary>
        /// 06811234567
        /// </summary>
        Local,
        /// <summary>
        /// 0681/1234567
        /// </summary>
        LocalWithSeparator,
        /// <summary>
        /// 436811234567
        /// </summary>
        NumericOnly
    }
}