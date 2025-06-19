using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using NLog;

namespace GuardX.Helper
{
    public class EncryptionDecryptionService
    {
        //Logger
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        //Initialization Vector should always be same for encryption and decryption process.
        private static byte[] IV;

        /// <summary>
        /// Encrypt the Protected Text with the Profile Password using AES-128 encryption alogrithm.
        /// </summary>
        /// <param name="profileUniqueText"></param>
        /// <param name="profilePwd"></param>
        /// <returns>
        /// Cipher Text
        /// </returns>
        public static string Encrypt(String profileUniqueText, String profilePwd)
        {
            string cipherText = String.Empty;

            try
            {
                //Default AES-128 Initialization
                using (Aes aes = Aes.Create())
                {
                    //Pad Key to expand it to 16byte long.
                    aes.Key = Encoding.UTF8.GetBytes(profilePwd.PadRight(16, '0'));
                    aes.IV = IV;

                    //Encryptor Object that performs the actual AES transformation using the key and IV
                    ICryptoTransform encryptorTransform = aes.CreateEncryptor(aes.Key, aes.IV);

                    //MemoryStream object to store the encrypted data in memory
                    using (MemoryStream msEncryptor = new MemoryStream())
                    {
                        //Wraps the memory stream in a CryptoStream which encrypts any data written to it.
                        //The output of encryption goes into msEncryptor.
                        using (CryptoStream csEncrypt = new CryptoStream(msEncryptor, encryptorTransform, CryptoStreamMode.Write))
                        {
                            //Wraps the CryptoStream in a StreamWriter so we can write plain text directly.
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                //Writes the input string into the stream.
                                //It gets encrypted and stored in memory via the MemoryStream.
                                swEncrypt.Write(profileUniqueText);
                            }
                            //Converts the encrypted byte array from msEncryptor to a Base64 string.
                            cipherText = Convert.ToBase64String(msEncryptor.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"ENCRYPTION_FAILED_#_Message:{ex.Message}_#_StackTrace:{ex.StackTrace}");
            }

            return cipherText;
        }

        /// <summary>
        /// Decrypt the Cipher Text with the User Input Password using AES-128 decryption alogrithm.
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="profilePwd"></param>
        /// <returns></returns>
        public static string Decrypt(String cipherText,String profilePwd)
        {
            string plainText = String.Empty;
            try
            {
                //Default AES-128 Initialization
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(profilePwd.PadRight(16, '0'));
                    aes.IV = IV;

                    //Creates a decryption transform object that can decrypt using the specified key and IV.
                    ICryptoTransform decryptorTransform = aes.CreateDecryptor(aes.Key, aes.IV);

                    //Decodes the input cipherText from Base64 into raw encrypted bytes.
                    byte[] cipherByteArray = Convert.FromBase64String(cipherText);

                    //Creates a MemoryStream that reads from the encrypted byte array.
                    using (MemoryStream msDecryptor = new MemoryStream(cipherByteArray))
                    {
                        //Wraps the MemoryStream in a CryptoStream which will automatically decrypt the data on-the-fly using the decryption transform.
                        using (CryptoStream csDecryptor = new CryptoStream(msDecryptor, decryptorTransform, CryptoStreamMode.Read))
                        {
                            //Wraps the CryptoStream in a StreamReader, which allows you to read decrypted data as text.
                            using (StreamReader srDecrypt = new StreamReader(csDecryptor))
                            {
                                //ReadToEnd() reads all decrypted data and stores it in plainText.
                                plainText = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }                
            }
            catch(Exception ex)
            {
                _logger.Error($"DECRYPTION_FAILED_#_Message:{ex.Message}_#_StackTrace:{ex.StackTrace}");
            }
            return plainText;
        }

        /// <summary>
        /// Returns the static IV or generate new when running for first time.
        /// </summary>
        /// <returns></returns>
        public static byte[] GetIVForAES()
        {
            if(IV == null || IV.Length == 0)
            {
                var aes = Aes.Create();
                aes.GenerateIV();
                IV = aes.IV;
            }
            return IV;
        }

        /// <summary>
        /// Set the Initialization Vector to static variable
        /// </summary>
        /// <param name="iv"></param>
        public static void SetIVForAES(byte[] iv)
        {
            if(iv != null && iv.Length > 0)
            {
                IV = iv;
            }
        }
    }
}
