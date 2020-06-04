using System.Linq;

namespace WpfAppMailSender
{
    internal static class DataBaseClass
    {
        private static readonly EmailsDataClassesDataContext EmailsDataClassesDataContext = new EmailsDataClassesDataContext();

        public static IQueryable<Email> Emails => from mail in EmailsDataClassesDataContext.Email select mail;
    }
}