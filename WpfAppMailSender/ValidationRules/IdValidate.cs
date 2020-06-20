using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using WpfAppMailSender.ViewModel;

namespace WpfAppMailSender.ValidationRules
{
    public class IdValidate : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string str)
            {
                if (!int.TryParse(str, out var i)) 
                    return new ValidationResult(false, "Строка имела неверный формат");
                value = i;
            }

            if (!(value is int id)) 
                return new ValidationResult(false, "Некорректный ввод");

            if (id < 0) 
                return new ValidationResult(false, "Индификатор должен быть больше нуля");

            ViewModelLocator objLocator = new ViewModelLocator();
            if (objLocator.Main.ListEmails.Any(x => x.Id.Equals(value)))
                return new ValidationResult(false, "ID не должен повторяться");

            return ValidationResult.ValidResult;
        }
    }
}