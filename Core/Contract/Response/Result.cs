using Core.Contract.Errors;
using System.Text.Json.Serialization;

namespace Core.Contract.Response;

public interface IResult
{
}

public class Result : IResult
{
    public bool Success { get; private set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object Data { get; private set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<IError>? Errors { get; private set; }

    public Result(bool success, IEnumerable<IError>? errors, object data = null!)
    {
        Success = success;
        Data = data;
        Errors = errors;
    }
}
