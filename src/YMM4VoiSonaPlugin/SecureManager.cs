using System.Security.Cryptography;
using System.Text;

namespace YMM4VoiSonaPlugin;

public static class SecureManager
{
	public static string Encrypt(string target)
	{
		var data = Encoding.UTF8.GetBytes(target);
		var enBytes = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
		return Convert.ToBase64String(enBytes);
	}

	public static string Decrypt(string encrypted)
	{
		if (string.IsNullOrEmpty(encrypted))
		{
			return string.Empty;
		}
		try
		{
			var enBytes = Convert.FromBase64String(encrypted);
			var deBytes = ProtectedData.Unprotect(enBytes, null, DataProtectionScope.CurrentUser);
			return Encoding.UTF8.GetString(deBytes);
		}
		catch (CryptographicException)
		{
			return string.Empty;
		}
	}
}
