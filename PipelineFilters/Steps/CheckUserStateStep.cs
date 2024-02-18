namespace PipelineFilters.Steps
{
    public class CheckUserStateStep : StepBase<PrepareActivationStepDto>
    {
        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            Console.WriteLine("CheckUserStateStep");

            input.IsPasswordExpired = true;
            input.Token = Guid.NewGuid().ToString();

            return await Task.FromResult(input);
        }
    }
}
