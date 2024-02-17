namespace PipelineFilters.Steps
{
    public class CheckTempPasswordExpiredStep : StepBase<PrepareActivationStepDto>
    {
        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            Console.WriteLine("CheckTempPasswordExpiredStep");

            return await Task.FromResult(input);
        }
    }
}
