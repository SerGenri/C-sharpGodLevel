using System.Collections.ObjectModel;
using System.Linq;

namespace WpfAppMailSender.Service
{
	public class DataBaseAccessService : IDataAccessService
	{
		private readonly EmailsDataClassesDataContext _dataContext = new EmailsDataClassesDataContext();

		public ObservableCollection<Email> GetEmails() => new ObservableCollection<Email>(_dataContext.Email);

		public void CreateEmail(Email email)
		{
            _dataContext.Email.InsertOnSubmit(email);
		}

        public void SaveChanges()
        {
            _dataContext.SubmitChanges();
        }
	}
}