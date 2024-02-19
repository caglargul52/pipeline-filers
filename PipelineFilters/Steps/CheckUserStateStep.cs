namespace PipelineFilters.Steps
{
    public class CheckUserStateStep : StepBase<PrepareActivationStepDto>
    {
        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            Console.WriteLine("CheckUserStateStep");

            return await Task.FromResult(input);
        }
    }
}
