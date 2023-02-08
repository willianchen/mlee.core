using System.IO;

namespace mlee.Core.Library.IpTools
{
    public class InvalidDatabaseException : IOException
    {
        public InvalidDatabaseException(string message) : base(message)
        {
        }
    }
}