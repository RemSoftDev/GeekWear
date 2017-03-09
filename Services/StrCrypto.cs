using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace GeekWear.Services
{
	public class CryptoModelBase<T>
	{
		public string Encrypt()
		{
			return HttpServerUtility.UrlTokenEncode(StrCrypto.Encrypt(JsonConvert.SerializeObject(this)));
		}

		public static T Decrypt(string token)
		{
			return JsonConvert.DeserializeObject<T>(StrCrypto.Decrypt(HttpServerUtility.UrlTokenDecode(token)));
		}
	}

	public class OrderStrCrypto : CryptoModelBase<OrderStrCrypto>
	{
		[JsonProperty("uid")]
		public string UserId { get; set; }

		[JsonProperty("oid")]
		public int OrderId { get; set; }
	}
	public class StrCrypto
	{
		private const string initVector = "r5dm5fgm24mfhfku";
		private const string passPhrase = "77AC9A45-7281";
		private const int keysize = 256;

		public static byte[] Encrypt(string plainText)
		{
			if (string.IsNullOrEmpty(plainText)) throw new ArgumentNullException("plainText");

			byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

			using (var password = new PasswordDeriveBytes(passPhrase, null))
			using (var symmetricKey = new RijndaelManaged())
			{
				symmetricKey.Mode = CipherMode.CBC;
				byte[] keyBytes = password.GetBytes(keysize / 8);

				using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
				using (var memoryStream = new MemoryStream())
				using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
				{
					cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
					cryptoStream.FlushFinalBlock();
					byte[] cipherTextBytes = memoryStream.ToArray();
					return cipherTextBytes;
					//return Convert.ToBase64String(cipherTextBytes);
				}
			}
		}

		public static string Decrypt(byte[] cipherText)
		{
			if (cipherText == null) throw new ArgumentNullException("cipherText");

			byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			byte[] cipherTextBytes = cipherText;//Convert.FromBase64String(cipherText);

			using (var password = new PasswordDeriveBytes(passPhrase, null))
			{
				byte[] keyBytes = password.GetBytes(keysize / 8);
				using (var symmetricKey = new RijndaelManaged())
				{
					symmetricKey.Mode = CipherMode.CBC;
					using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
					using (var memoryStream = new MemoryStream(cipherTextBytes))
					using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
					{
						byte[] plainTextBytes = new byte[cipherTextBytes.Length];
						int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
						return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
					}
				}
			}
		}
	}
}