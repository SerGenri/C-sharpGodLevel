using System;
using System.Collections.ObjectModel;

namespace WpfAppMailSender.Service
{
	class XmlFileAccessService : IDataAccessService
	{
		public ObservableCollection<Email> GetEmails() => throw new	NotImplementedException();
        public void CreateEmail(Email email) => throw new NotImplementedException();
        public void SaveChanges() => throw new NotImplementedException();
	}
}