using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lair.HttpListener
{
    public static class NetSh
    {
        private const string NetshCommand = "netsh";

        /// <summary>
        /// Add a url reservation
        /// </summary>
        /// <param name="url">Url to add</param>
        /// <param name="user">User to add the reservation for</param>
        /// <returns>True if successful, false otherwise.</returns>
        public static bool AddUrlAcl(string url, string user)
        {
            try
            {
                var arguments = GetAddParameters(url, user);

                return UacHelper.RunElevated(NetshCommand, arguments);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        /// <summary>
        /// Delete a url reservation
        /// </summary>
        /// <param name="url">Url to delte</param>
        /// <param name="user">User to delete the reservation for</param>
        /// <returns>True if successful, false otherwise.</returns>
        public static bool DeleteUrlAcl(string url, string user)
        {
            try
            {
                var arguments = GetDeleteParameters(url, user);

                return UacHelper.RunElevated(NetshCommand, arguments);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static string GetAddParameters(string url, string user)
        {
            return string.Format("http add urlacl url=\"{0}\" user=\"{1}\"", url, user);
        }

        internal static string GetDeleteParameters(string url, string user)
        {
            return string.Format("http delete urlacl url=\"{0}\"", url);
        }
    }
}
