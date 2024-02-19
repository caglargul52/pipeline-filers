namespace PipelineFilters.Steps
{
    public class CompletePrepareActivationStep : StepBase<PrepareActivationStepDto>
    {
        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            Console.WriteLine("CompletePrepareActivationStep");

            Console.WriteLine("Token Alındı : " + input.Token);

            return await Task.FromResult(input);
        }
    }
}
