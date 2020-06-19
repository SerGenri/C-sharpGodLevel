using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WpfAppMailSender.Test
{
    [TestClass]
    public class SchedulerClassTest
    {
        private SchedulerClass _sc;
        private TimeSpan _ts;

        [TestInitialize]
        public void TestInitialize()
        {
            Debug.WriteLine("Test Initialize");

             _sc = new SchedulerClass();
             _ts = new TimeSpan();
        }

        [TestMethod]
        public void GetSendTime​_empty_ts()
        {
            string strTimeTest = string.Empty;
            
            TimeSpan tsTest = _sc.GetSendTime​(strTimeTest);

            Assert.AreEqual(_ts, tsTest);
        }

        [TestMethod]
        public void GetSendTime​_asd_ts()
        {
            const string strTimeTest = "asd";

            TimeSpan tsTest = _sc.GetSendTime​(strTimeTest);

            Assert.AreEqual(_ts, tsTest);
        }

        [TestMethod]
        public void GetSendTime​_correctTime_ts()
        {
            const string strTimeTest = "12:12";

            TimeSpan tsCorrect = new TimeSpan(12,12,0);

           TimeSpan tsTest = _sc.GetSendTime​(strTimeTest);

            Assert.AreEqual(tsCorrect, tsTest);
        }

        [TestMethod]
        public void GetSendTime​_inCorrectHour_ts()
        {
            const string strTimeTest = "25:12";

            TimeSpan tsTest = _sc.GetSendTime​(strTimeTest);

            Assert.AreEqual(_ts, tsTest);
        }

        [TestMethod]
        public void GetSendTime​_inCorrectMin_ts()
        {
            const string strTimeTest = "12:65";

            TimeSpan tsTest = _sc.GetSendTime​(strTimeTest);

            Assert.AreEqual(_ts, tsTest);
        }


        [TestCleanup]
        public void TestCleanup()
        {
            Console.WriteLine("Cleanup");

            _sc.Dispose();
        }
    }


    [TestClass]
    public class SchedulerClassTestStatic
    {
        private static SchedulerClass _sc;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Debug.WriteLine("Class Initialize");

            _sc = new SchedulerClass
            {
                EmailsTimeDictionary = new ObservableCollection<EmailDataTimeTextClass>
                {
                    new EmailDataTimeTextClass
                    {
                        DataSend = new DateTime(2016, 12, 24, 22, 0, 0),
                        TimeSend = "12:21",
                        Body = "Body",
                        Subject = "Subject"
                    },
                    new EmailDataTimeTextClass
                    {
                        DataSend = new DateTime(2016, 12, 24, 22, 30, 0),
                        TimeSend = "12:21",
                        Body = "Body",
                        Subject = "Subject"
                    },
                    new EmailDataTimeTextClass
                    {
                        DataSend = new DateTime(2016, 12, 24, 23, 0, 0),
                        TimeSend = "12:21",
                        Body = "Body",
                        Subject = "Subject"
                    }
                }
            };

        }

        [TestMethod]
        public void TimeTick_Dictionare_correct()
        {
            DateTime dt1 = new DateTime(2016, 12, 24, 22, 0, 0);
            DateTime dt2 = new DateTime(2016, 12, 24, 22, 30, 0);
            DateTime dt3 = new DateTime(2016, 12, 24, 23, 0, 0);

            CollectionAssert.AllItemsAreUnique(_sc.EmailsTimeDictionary);

            if (_sc.EmailsTimeDictionary.First().DataSend.ToShortTimeString() == dt1.ToShortTimeString())
            {
                Debug.WriteLine("Body " + _sc.EmailsTimeDictionary.First().DataSend);
                Debug.WriteLine($"Subject Рассылка от " +
                                  $"{ _sc.EmailsTimeDictionary.First().DataSend.ToShortDateString()} " +
                                  $"{ _sc.EmailsTimeDictionary.First().DataSend.ToShortTimeString()}​ ");

                EmailDataTimeTextClass objRemove = _sc.EmailsTimeDictionary.First();
                _sc.EmailsTimeDictionary.Remove(objRemove);
            }

            if (_sc.EmailsTimeDictionary.First().DataSend.ToShortTimeString() == dt2.ToShortTimeString())
            {
                Debug.WriteLine("Body " + _sc.EmailsTimeDictionary.First().DataSend);
                Debug.WriteLine($"Subject Рассылка от " +
                                $"{ _sc.EmailsTimeDictionary.First().DataSend.ToShortDateString()} " +
                                $"{ _sc.EmailsTimeDictionary.First().DataSend.ToShortTimeString()}​ ");

                EmailDataTimeTextClass objRemove = _sc.EmailsTimeDictionary.First();
                _sc.EmailsTimeDictionary.Remove(objRemove);
            }

            if (_sc.EmailsTimeDictionary.First().DataSend.ToShortTimeString() == dt3.ToShortTimeString())
            {
                Debug.WriteLine("Body " + _sc.EmailsTimeDictionary.First().DataSend);
                Debug.WriteLine($"Subject Рассылка от " +
                                $"{ _sc.EmailsTimeDictionary.First().DataSend.ToShortDateString()} " +
                                $"{ _sc.EmailsTimeDictionary.First().DataSend.ToShortTimeString()}​ ");

                EmailDataTimeTextClass objRemove = _sc.EmailsTimeDictionary.First();
                _sc.EmailsTimeDictionary.Remove(objRemove);
            }

            Assert.AreEqual(0, _sc.EmailsTimeDictionary.Count);

        }
    }

}
