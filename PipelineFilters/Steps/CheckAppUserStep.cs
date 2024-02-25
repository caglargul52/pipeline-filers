namespace PipelineFilters.Steps
{
    public class CheckAppUserStep : StepBase<PrepareActivationStepDto>
    {
        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            Console.WriteLine("CheckAppUserStep");

            var output = input with { Message = "Deneme" };

            return Next(output);
        }
    }
}
