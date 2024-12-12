
using System.Text.Json.Serialization;

namespace Core.Contract.Errors;

public class ValidationCollectionError : IError
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string FieldName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Code { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? RowId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Error> Errors { get; set; }
}
