using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EncrypterDll.Test
{
    [TestClass]
    public class EncrypterDllTest
    {
        [TestMethod]
        public void Encrypt_adc_bcd()
        {
            // arrange
            const string strIn = "abc";
            const string strExpected = "bcd";
            
            // act
            string strActual = Encrypter.Encrypt(strIn);
            
            //assert
            Assert.AreEqual(strExpected, strActual);
        }


        [TestMethod]
        public void Deencrypt_bcd_abc()
        {
            // arrange
            const string strIn = "bcd";
            const string strExpected = "abc";

            // act
            string strActual = Encrypter.Deencrypt(strIn);

            //assert
            Assert.AreEqual(strExpected, strActual, "Error Deencrypt_bcd_abc");
        }


        [TestMethod]
        public void Encrypt_empty_empty()
        {
            // arrange
            string strIn = string.Empty;
            string strExpected = string.Empty;

            // act
            string strActual = Encrypter.Encrypt(strIn);

            //assert
            Assert.AreEqual(strExpected, strActual);
        }


    }
}
