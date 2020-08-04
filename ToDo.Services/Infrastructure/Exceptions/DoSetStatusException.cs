using System;
using ToDo.Data.Models;
using ToDo.Data.Models.Static;

namespace ToDo.Services.Infrastructure.Exceptions
{
    public class DoSetStatusException : Exception
    {
        public DoSetStatusException(DoStatus status, Do entity)
            : base(String.Format("Attempt to appropriate " +
                status.ToString()
                + "status for Do that already have "
                + entity.Status.ToString()
                + " status"))
        {
        }
    }
}
