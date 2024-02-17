namespace PipelineFilters.Steps
{
    public class GetCustomerInfoStep : StepBase<PrepareActivationStepDto>
    {
        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            Console.WriteLine("GetCustomerInfoStep");

            return await Task.FromResult(input);
        }
    }
}
