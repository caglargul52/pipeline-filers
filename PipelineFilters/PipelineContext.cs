namespace PipelineFilters
{
    public class PipelineContext
    {
        public IAuthService AuthService { get; }

        public PipelineContext(IAuthService authService)
        {
            AuthService = authService;
        }
    }
}
