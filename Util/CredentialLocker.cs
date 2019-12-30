using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Extension;
using Windows.Security.Credentials;

namespace UWPUtilities.Util
{
    public static class CredentialLocker
    {
        private static readonly PasswordVault Vault = new PasswordVault();

        public static void StoreCredential(string resource, string userName, string password)
        {
            Vault.Add(new PasswordCredential(resource, userName, password));
        }

        public static PasswordCredential RetrieveCredential(string resource, string userName)
        {
            return Vault.Retrieve(resource, userName);
        }

        public static string RetrievePassword(string resource, string userName)
        {
            return Vault.Retrieve(resource, userName)?.Password;
        }

        public static IReadOnlyList<PasswordCredential> FindAllByResource(string resource)
        {
            return Vault.FindAllByResource(resource);
        }

        public static IReadOnlyList<PasswordCredential> FindAllByUserName(string userName)
        {
            return Vault.FindAllByUserName(userName);
        }

        public static IReadOnlyList<PasswordCredential> RetrieveAll()
        {
            return Vault.RetrieveAll();
        }

        public static void DeleteCredential(string resource, string userName)
        {
            var credential = RetrieveCredential(resource, userName);
            if (credential != default)
            {
                Vault.Remove(credential);
            }
        }

        public static void DeleteCredentialsByUserName(string userName)
        {
            var credentials = FindAllByUserName(userName);
            if (credentials.IsNullOrEmpty()) { return; }
            foreach (var credential in credentials)
            {
                Vault.Remove(credential);
            }
        }

        public static void DeleteCredentialsByResource(string resource)
        {
            var credentials = FindAllByResource(resource);
            if (credentials.IsNullOrEmpty()) { return; }
            foreach (var credential in credentials)
            {
                Vault.Remove(credential);
            }
        }

        public static void DeleteAllCredentials()
        {
            var credentials = RetrieveAll();
            if (credentials.IsNullOrEmpty()) { return; }
            foreach (var credential in credentials)
            {
                Vault.Remove(credential);
            }
        }
    }
}
