namespace PipelineFilters
{
    public class StepError
    {
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public string Severity { get; set; }
    }

    public abstract class StepBase<T> : IStep<T> where T : class, new()
    {
        public PipelineContext? Context { get; private set; }
        public Pipeline<T> Pipeline { get; private set; }

        public StepError? Error { get; set; }
        
        private T CurrentDto;
        
        public abstract Task<T> ProcessAsync(T input);

        protected T ThrowStepError(string errorMessage, string errorCode, string severity)
        {
            Error ??= new StepError();

            Error.ErrorMessage = errorMessage;
            Error.ErrorCode = errorCode;
            Error.Severity = severity;

            return CurrentDto;
        }

        public async Task<T> ExecuteAsync(T input)
        {
            CurrentDto = input;

            try
            {
                CurrentDto = await ProcessAsync(input);
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

            return CurrentDto;
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
