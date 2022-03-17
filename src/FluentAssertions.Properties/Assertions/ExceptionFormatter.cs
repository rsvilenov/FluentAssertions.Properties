using FluentAssertions.Formatting;
using System;

namespace FluentAssertions.Properties.Assertions
{
    internal class ExceptionFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return value is Exception;
        }

        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            string exceptionString = ((Exception)value).ToString();
            int cutIndex = exceptionString.IndexOf($"{Environment.NewLine}--- ");
            return cutIndex > 1 ? exceptionString.Substring(0, cutIndex) : exceptionString;
        }
    }
}
