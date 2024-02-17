namespace PipelineFilters.Steps
{
    public class UpdateLastLoginDateStep : StepBase<PrepareActivationStepDto>
    {
        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            Console.WriteLine("UpdateLastLoginDateStep");

            return await Task.FromResult(input);
        }
    }
}
