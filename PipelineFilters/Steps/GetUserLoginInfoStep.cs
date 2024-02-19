using System;

namespace PipelineFilters.Steps
{
    public class GetUserLoginInfoStep : StepBase<PrepareActivationStepDto>
    {
        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            Console.WriteLine("GetUserLoginInfoStep");

            return await Task.FromResult(input);
        }
    }
}
