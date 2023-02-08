using System;

namespace mlee.Core.Library.IpTools
{
    public class NotFoundException : Exception
    {

        public NotFoundException(string name) : base(name)
        {
        }
    }
}
