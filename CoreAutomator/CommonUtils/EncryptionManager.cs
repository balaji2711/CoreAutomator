using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CoreAutomator.CommonUtils
{
    public static class EncryptionManager
    {
        public static string Encrypt(string value)
        {
            string encryptValue = string.Concat(value, value.Length.ToString());
            for (int i = 0; i < value.Length; i++)
            {
                encryptValue = string.Concat(encryptValue, Convert.ToInt16(Convert.ToChar(value[i])));
            }
            UTF8Encoding textConverter = new UTF8Encoding();
            byte[] passBytes = textConverter.GetBytes(encryptValue);
            return Convert.ToBase64String(new SHA384Managed().ComputeHash(passBytes));
        }

        public static bool IsMatch(string value, string encryptedValue)
        {
            return Encrypt(value) == encryptedValue;
        }

        public static string Encrypt(string inputData, string password, int bits)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(inputData);
            PasswordDeriveBytes pwdBytes = new PasswordDeriveBytes(password, new byte[] { 0x10, 0x40, 0x00, 0x34, 0x1A, 0x70, 0x01, 0x34, 0x56, 0xFF, 0x99, 0x77, 0x4C, 0x22, 0x49 });

            if (bits == 128)
            {
                byte[] encryptedData = Encrypt(bytes, pwdBytes.GetBytes(16), pwdBytes.GetBytes(16));
                return Convert.ToBase64String(encryptedData);
            }
            else if (bits == 192)
            {
                byte[] encryptedData = Encrypt(bytes, pwdBytes.GetBytes(24), pwdBytes.GetBytes(24));
                return Convert.ToBase64String(encryptedData);
            }
            else if (bits == 256)
            {
                byte[] encryptedData = Encrypt(bytes, pwdBytes.GetBytes(32), pwdBytes.GetBytes(32));
                return Convert.ToBase64String(encryptedData);
            }
            else
            {
                return string.Concat(bits);
            }
        }

        public static string Decrypt(string inputData, string password, int bits)
        {
            try
            {
                byte[] Bytes = Convert.FromBase64String(inputData);
                PasswordDeriveBytes pwdBytes = new PasswordDeriveBytes(password,
                  new byte[] { 0x10, 0x40, 0x00, 0x34, 0x1A, 0x70, 0x01, 0x34, 0x56, 0xFF, 0x99, 0x77, 0x4C, 0x22, 0x49 });
                if (bits == 128)
                {
                    byte[] decryptedData = Decrypt(Bytes, pwdBytes.GetBytes(16), pwdBytes.GetBytes(16));
                    return Encoding.Unicode.GetString(decryptedData);
                }
                else if (bits == 192)
                {
                    byte[] decryptedData = Decrypt(Bytes, pwdBytes.GetBytes(24), pwdBytes.GetBytes(16));
                    return Encoding.Unicode.GetString(decryptedData);
                }
                else if (bits == 256)
                {
                    byte[] decryptedData = Decrypt(Bytes, pwdBytes.GetBytes(32), pwdBytes.GetBytes(16));
                    return Encoding.Unicode.GetString(decryptedData);
                }
                else
                {
                    return string.Concat(bits);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static byte[] Encrypt(byte[] inputData, byte[] password, byte[] values)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Rijndael rijndael = Rijndael.Create();
                rijndael.Key = password;
                rijndael.IV = values;
                CryptoStream cStream = new CryptoStream(stream, rijndael.CreateEncryptor(rijndael.Key, rijndael.IV), CryptoStreamMode.Write);
                cStream.Write(inputData, 0, inputData.Length);
                cStream.Close();
                byte[] encryptedData = stream.ToArray();
                return encryptedData;
            }
        }

        private static byte[] Decrypt(byte[] outputData, byte[] password, byte[] value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Rijndael rijndael = Rijndael.Create();
                rijndael.Key = password;
                rijndael.IV = value;
                CryptoStream cStream = new CryptoStream(stream, rijndael.CreateDecryptor(rijndael.Key, rijndael.IV), CryptoStreamMode.Write);
                cStream.Write(outputData, 0, outputData.Length);
                cStream.Close();
                byte[] decryptedData = stream.ToArray();
                return decryptedData;
            }
        }

        public static string GenerateAPassKey(string passphrase)
        {
            string passPhrase = passphrase;
            string saltValue = passphrase;
            string hashAlgorithm = "SHA1";
            int passwordIterations = 2;
            int keySize = 256;
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] Key = pdb.GetBytes(keySize / 11);
            string KeyString = Convert.ToBase64String(Key);
            return KeyString;
        }

        public static string Encrypt(string plainStr, string KeyString)
        {
            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.KeySize = 256;
            aesEncryption.BlockSize = 128;
            aesEncryption.Mode = CipherMode.ECB;
            aesEncryption.Padding = PaddingMode.ISO10126;
            byte[] KeyInBytes = Encoding.UTF8.GetBytes(KeyString);
            aesEncryption.Key = KeyInBytes;
            byte[] plainText = Encoding.UTF8.GetBytes(plainStr);
            ICryptoTransform crypto = aesEncryption.CreateEncryptor();
            byte[] cipherText = crypto.TransformFinalBlock(plainText, 0, plainText.Length);
            return Convert.ToBase64String(cipherText);
        }

        public static string Decrypt(string encryptedText, string KeyString)
        {
            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.KeySize = 256;
            aesEncryption.BlockSize = 128;
            aesEncryption.Mode = CipherMode.ECB;
            aesEncryption.Padding = PaddingMode.ISO10126;
            byte[] KeyInBytes = Encoding.UTF8.GetBytes(KeyString);
            aesEncryption.Key = KeyInBytes;
            ICryptoTransform decrypto = aesEncryption.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64CharArray(encryptedText.ToCharArray(), 0, encryptedText.Length);
            return Encoding.UTF8.GetString(decrypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length));
        }
    }
}