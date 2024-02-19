namespace PipelineFilters
{
    public record PrepareActivationStepDto
    {
        public string Message { get; init; }
        public object CentralLogin { get; init; }
        public bool IsPasswordExpired { get; init; }
        public string Token { get; init; }
    }
}
