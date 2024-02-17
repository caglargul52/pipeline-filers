namespace PipelineFilters.Steps
{
    public class GetActivationStep : StepBase<PrepareActivationStepDto>
    {
        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            Console.WriteLine("GetActivationStep");

            return await Task.FromResult(input);
        }
    }
}
