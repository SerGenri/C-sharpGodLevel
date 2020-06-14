using System.Collections.ObjectModel;

namespace WpfAppMailSender.Service
{
	public interface IDataAccessService
	{
		ObservableCollection<Email> GetEmails();
		void CreateEmail(Email email);
        void SaveChanges();
    }
}