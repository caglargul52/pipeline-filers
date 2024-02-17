namespace PipelineFilters.Steps
{
    public class ResetPasswordRetryCountStep : StepBase<PrepareActivationStepDto>
    {
        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            Console.WriteLine("ResetPasswordRetryCountStep");

            return await Task.FromResult(input);
        }
    }
}
