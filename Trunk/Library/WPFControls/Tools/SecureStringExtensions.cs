using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Mnk.Library.WpfControls.Tools
{
    public static class SecureStringExtensions
    {
        private static readonly byte[] Entropy = Encoding.Unicode.GetBytes("{1A93489D-99EA-4186-A2C0-5545B0B44E9A}");

        internal static string EncryptString(this SecureString input)
        {
            var encryptedData = ProtectedData.Protect(
                Encoding.Unicode.GetBytes(ToInsecureString(input)),
                Entropy,
                DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        internal static SecureString DecryptString(this string encryptedData)
        {
            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(
                    Convert.FromBase64String(encryptedData),
                    Entropy,
                    DataProtectionScope.CurrentUser);
                return ToSecureString(Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        internal static SecureString ToSecureString(this string input)
        {
            var secure = new SecureString();
            foreach (var c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        internal static string ToInsecureString(this SecureString input)
        {
            var ptr = Marshal.SecureStringToBSTR(input);
            try
            {
                return Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }
        }

        public static string DecryptPassword(this string value)
        {
            using (var ss = value.DecryptString())
            {
                return ss.ToInsecureString();
            }
        }

        public static string EncryptPassword(this string value)
        {
            using (var ss = value.ToSecureString())
            {
                return ss.EncryptString();
            }
        }
    }
}
