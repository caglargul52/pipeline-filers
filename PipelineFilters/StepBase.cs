namespace PipelineFilters
{
    public class StepError
    {
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public string Severity { get; set; }
    }

    public abstract class StepBase<T> : IStep<T> where T : class
    {
        public PipelineContext? Context { get; private set; }
        public Pipeline<T> Pipeline { get; private set; }

        public StepError? Error { get; set; }

        public bool IsBypassed { get; set; } = false;

        public abstract Task<T> ProcessAsync(T input);

        public StepBase(bool isBypassed = false)
        {
            IsBypassed = isBypassed;
        }

        public void ThrowStepError(string errorMessage, string errorCode, string severity)
        {
            Error ??= new StepError();

            Error.ErrorMessage = errorMessage;
            Error.ErrorCode = errorCode;
            Error.Severity = severity;
        }

        public async Task<T> ExecuteAsync(T input)
        {
            if (IsBypassed)
            {
                return await Task.FromResult(input);
            }

            T current = input;

            try
            {
                current = await ProcessAsync(input);
            }
            catch (Exception ex)
            {
                if (this.Error is null)
                {
                    this.Error = new StepError
                    {
                        ErrorMessage = ex.Message,
                        ErrorCode = "",
                        Severity = ""
                    };
                }
            }

            // Loglama vs. işlemleri yapılabilir.

            return current;
        }

        public void AddContext(PipelineContext context)
        {
            Context = context;
        }

        public void AddPipeline(Pipeline<T> pipeline)
        {
            Pipeline = pipeline;
        }
    }
}
