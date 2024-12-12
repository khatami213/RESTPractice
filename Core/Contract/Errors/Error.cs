
using System.Text.Json.Serialization;

namespace Core.Contract.Errors;

public class Error : IError
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
    public List<Error>? Errors { get; set; }

    public Error()
    {

    }

    public Error(string code, string message, string fieldName = null)
    {
        Code = code;
        Message = message;
        FieldName = fieldName;
    }
}
