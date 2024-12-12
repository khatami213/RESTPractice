using Core.Contract.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contract.Application.Events;

public class ValidationFailed : Event
{
    public IEnumerable<Error> Errors { get; private set; }

    public ValidationFailed(IEnumerable<Error> errors) => Errors = errors;

}
