namespace PipelineFilters.Steps
{
    public class AddLoginHistoryStep : StepBase<PrepareActivationStepDto>
    {
        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            Console.WriteLine("AddLoginHistoryStep");

            return await Task.FromResult(input);
        }
    }
}
