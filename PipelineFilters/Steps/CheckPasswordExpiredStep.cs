namespace PipelineFilters.Steps
{
    public class CheckPasswordExpiredStep : StepBase<PrepareActivationStepDto>
    {
        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            Console.WriteLine("CheckPasswordExpiredStep");

            ThrowStepError("CheckPasswordExpiredStep", "", "");
            return await Task.FromResult(input);
        }
    }
}
