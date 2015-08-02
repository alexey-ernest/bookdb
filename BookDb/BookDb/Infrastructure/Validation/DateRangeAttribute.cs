using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BookDb.Infrastructure.Validation
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private const string DateFormat = "yyyy-MM-dd";
        private const string DefaultErrorMessage = "'{0}' must be a date between {1:d} and {2:d}.";

        public DateRangeAttribute()
            : base(DefaultErrorMessage)
        {
        }

        public DateRangeAttribute(string minDate, string maxDate)
            : base(DefaultErrorMessage)
        {
            MinDate = minDate;
            MaxDate = maxDate;
        }

        public string MinDate { get; set; }
        public string MaxDate { get; set; }

        public override bool IsValid(object value)
        {
            if (!(value is DateTime))
            {
                return true;
            }

            var dateValue = (DateTime) value;
            if (!string.IsNullOrEmpty(MinDate) && !string.IsNullOrEmpty(MaxDate))
            {
                var min = ParseDate(MinDate);
                var max = ParseDate(MaxDate);
                return min <= dateValue && dateValue <= max;
            }
            if (!string.IsNullOrEmpty(MinDate))
            {
                var min = ParseDate(MinDate);
                return min <= dateValue;
            }
            if (!string.IsNullOrEmpty(MaxDate))
            {
                var max = ParseDate(MaxDate);
                return dateValue <= max;
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, MinDate, MaxDate);
        }

        private static DateTime ParseDate(string dateValue)
        {
            return DateTime.ParseExact(dateValue, DateFormat,
                CultureInfo.InvariantCulture);
        }
    }
}