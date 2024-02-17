namespace PipelineFilters.Steps
{
    public class IncrementPasswordRetryCountStep : StepBase<PrepareActivationStepDto>
    {
        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            Console.WriteLine("IncrementPasswordRetryCountStep");

            return await Task.FromResult(input);
        }
    }
}
