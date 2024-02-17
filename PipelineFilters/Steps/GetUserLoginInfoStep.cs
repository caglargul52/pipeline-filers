using System;

namespace PipelineFilters.Steps
{
    public class GetUserLoginInfoStep : StepBase<PrepareActivationStepDto>
    {
        private readonly List<IStep<PrepareActivationStepDto>> loginIsNullSteps;
        private readonly List<IStep<PrepareActivationStepDto>> loginIsNotNullSteps;

        public GetUserLoginInfoStep()
        {
            loginIsNullSteps = new List<IStep<PrepareActivationStepDto>>()
            {
                new IncrementPasswordRetryCountStep(),
                new AddLoginHistoryStep(),
            };

            loginIsNotNullSteps = new List<IStep<PrepareActivationStepDto>>()
            {
                new CheckTempPasswordExpiredStep(),
                new ResetPasswordRetryCountStep(),
                new CheckPasswordExpiredStep(),
                new AddLoginHistoryStep(),
                new GetCustomerInfoStep(),
                new UpdateLastLoginDateStep(),
            };
        }

        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            PrepareActivationStepDto output = new();

            if (input.CentralLogin != null)
            {
                foreach (var step in loginIsNotNullSteps)
                {
                    output = await step.ExecuteAsync(input);
                }
            }
            else
            {
                foreach (var step in loginIsNullSteps)
                {
                    output = await step.ExecuteAsync(input);
                }
            }

            return await Task.FromResult(output);
        }
    }
}
