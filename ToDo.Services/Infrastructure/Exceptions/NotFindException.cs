using System;

namespace ToDo.Services.Infrastructure.Exceptions
{
    public class NotFindException : Exception
    {
        public NotFindException(string methodName)
            : base(String.Format("Service method " + methodName + "cannot find object"))
        {
        }
    }
}
