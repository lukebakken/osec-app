namespace OSecApp
{
    using System.Globalization;
    using System.Windows.Controls;

    public class NonEmptyStringRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var rv = new ValidationResult(true, null);

            string s = value as string;

            if (Validation.NonEmptyString(s) == false)
            {
                rv = new ValidationResult(false, Properties.Resources.Validation_StringMustHaveValue);
            }

            return rv;
        }
    }
}
