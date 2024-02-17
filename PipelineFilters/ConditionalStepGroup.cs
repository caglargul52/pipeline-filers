using PipelineFilters.Steps;

namespace PipelineFilters
{
    public static class ConditionalStepGroup
    {
        public static List<IStep<PrepareActivationStepDto>> GetCentralLoginSteps(PrepareActivationStepDto result)
        {
            List<IStep<PrepareActivationStepDto>> group = new List<IStep<PrepareActivationStepDto>>();

            if (result.CentralLogin is null)
            {
                group.Add(new IncrementPasswordRetryCountStep());
                group.Add(new AddLoginHistoryStep());
            }
            else
            {
                group.Add(new CheckTempPasswordExpiredStep());

                group.Add(new ResetPasswordRetryCountStep());
                group.Add(new CheckPasswordExpiredStep());
                group.Add(new AddLoginHistoryStep());
                group.Add(new GetCustomerInfoStep());
                group.Add(new UpdateLastLoginDateStep());
            }

            return group;
        }
    }
}
