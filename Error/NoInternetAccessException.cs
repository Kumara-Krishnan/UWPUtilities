using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.Error
{
    public class NoInternetAccessException : Exception
    {
        public NoInternetAccessException() : base("No internet access") { }

        public NoInternetAccessException(string message) : base(message) { }

        public NoInternetAccessException(string message, Exception innerException) : base(message, innerException) { }
    }
}
