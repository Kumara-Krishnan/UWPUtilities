using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.UseCase
{
    public enum ErrorType
    {
        Unknown,
        DB,
        Network,
        File
    }

    public class UseCaseError
    {
        public ErrorType Type { get; set; }

        public Exception Exception { get; set; }

        public UseCaseError(ErrorType type, Exception exception)
        {
            Type = type;
            Exception = exception;
        }

        public override string ToString()
        {
            return $"Error Type: {Type.ToString()}, Info: {Exception.ToString()}";
        }
    }
}
