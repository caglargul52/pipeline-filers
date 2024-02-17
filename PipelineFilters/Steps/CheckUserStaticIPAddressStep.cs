namespace PipelineFilters.Steps
{
    public class CheckUserStaticIPAddressStep : StepBase<PrepareActivationStepDto>
    {
        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            Console.WriteLine("CheckUserStaticIPAddressStep");
            return await Task.FromResult(input);
        }
    }
}
