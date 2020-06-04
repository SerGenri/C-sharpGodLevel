using System.Collections.Generic;

namespace WpfAppMailSender
{
    public static class SendersClass
    {
        public static Dictionary<string, string> SendersDictionary { get; } = new Dictionary<string, string>
        {
            {"SendSpamBot@yandex.ru" , EncrypterDll.Encrypter.Encrypt("P@$$w0rd") },
            {"SendSpamBot@yandex.ru1" , EncrypterDll.Encrypter.Encrypt("P@$$w0rd") },
            {"SendSpamBot@yandex.ru2" , EncrypterDll.Encrypter.Encrypt("P@$$w0rd") },
            {"SendSpamBot@yandex.ru3" , EncrypterDll.Encrypter.Encrypt("P@$$w0rd") },
            {"SendSpamBot@yandex.ru4" , EncrypterDll.Encrypter.Encrypt("P@$$w0rd") }
        };
    }
}