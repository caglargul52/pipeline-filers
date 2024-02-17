
namespace PipelineFilters.Steps
{
    public class GenerateActivationGuidStep : StepBase<PrepareActivationStepDto>
    {
        private readonly List<IStep<PrepareActivationStepDto>> loginIsNullSteps;
        private readonly List<IStep<PrepareActivationStepDto>> loginIsNotNullSteps;

        public GenerateActivationGuidStep()
        {

        }

        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            Console.WriteLine("GenerateActivationGuidStep");

            if (input.CentralLogin is null)
            {

            }
            else
            {
            }

            return await Task.FromResult(input);
        }
    }
}
