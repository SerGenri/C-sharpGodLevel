using System.Linq;

namespace EncrypterDll
{
    public static class Encrypter
    {
        /// <summary>
        /// Зашифровать
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string str, int key = 1) 
            => new string(str.Select(c => (char) (c + key)).ToArray());

        /// <summary>
        /// Расшифровать
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Deencrypt(string str, int key = 1) 
            => new string(str.Select(c => (char)(c - key)).ToArray());

    }
}
