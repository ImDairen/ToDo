using System;

namespace ToDo.Services.Infrastructure.Exceptions
{
    public class DoSetDoneException : Exception
    {
        public DoSetDoneException()
            : base(String.Format("To do can not be done"))
        {
        }
    }
}
