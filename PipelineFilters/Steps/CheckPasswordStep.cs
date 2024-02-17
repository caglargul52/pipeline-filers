namespace PipelineFilters.Steps
{
    public class CheckPasswordStep : StepBase<PrepareActivationStepDto>
    {
        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            ThrowStepError("deneme", "", "");
            return await Task.FromResult(input);
        }
    }
}
