using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace WpfAppMailSender.ValidationRules
{
    public class EmailAddressValidate:ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!(value is string address)) 
                return new ValidationResult(false, "Некорректные данные");

            if(!Regex.IsMatch(address, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")) 
                return new ValidationResult(false, "Введен некорректный адрес");

            return ValidationResult.ValidResult;
        }
    }
}