using System.Collections.Generic;

namespace WpfAppMailSender
{
    public class SmtpServerClass
    {
        public static Dictionary<string, int> SmtpServerDictionary { get; } = new Dictionary<string, int>
        {
            {"smtp.yandex.ru" , 25 },
            {"smtp.mail.ru" , 25 },
            {"smtp.google.ru" , 25 }
        };
    }
}