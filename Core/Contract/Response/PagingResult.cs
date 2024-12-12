namespace Core.Contract.Response;

public class PagingResult
{
    public int? Count { get; set; }
    //public object Result { get; set; }
}

public class PagingResult<TDataModel> : PagingResult
{
    public IEnumerable<TDataModel> Result { get; private set; }


    public PagingResult(IEnumerable<TDataModel> result) => Result = result;

    public PagingResult(IEnumerable<TDataModel> result, int totalCount)
    {
        Count = totalCount;
        Result = result;
    }
}
