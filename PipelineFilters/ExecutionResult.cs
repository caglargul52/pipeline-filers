namespace PipelineFilters;

public class ExecutionResult<T>
{
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public StepError? Error { get; set; }

    public ExecutionResult(bool isSuccess, T data, StepError? error)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
    }
}