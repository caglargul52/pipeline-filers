using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PipelineFilters
{
    public interface IStep<T> where T : class
    {
        StepError? Error { get; set; }
        Task<T> ExecuteAsync(T input);
        void AddContext(PipelineContext context);

        void AddPipeline(Pipeline<T> pipeline);
    }
}
