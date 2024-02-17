namespace PipelineFilters
{
    public class PrepareActivationStepDto
    {
        public string Message { get; set; }
        public object CentralLogin { get; internal set; }
        public bool IsPasswordExpired { get; set; }
        public string Token { get; set; }
    }
}
