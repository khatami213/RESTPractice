using Core.Contract.Errors;

namespace Core.Contract.Application.Events;

public class CollectionValidationsFailed : Event
{
    public List<ValidationCollectionError> ValidationErrors { get; set; }
    public DateTime CreateDateTime => DateTime.Now;
}
