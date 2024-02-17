namespace PipelineFilters.Steps
{
    public class CheckAppUserStep : StepBase<PrepareActivationStepDto>
    {
        public CheckAppUserStep(bool isBypassed = false) : base(isBypassed)
        {
        }

        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            Console.WriteLine("CheckAppUserStep");

            //var token = _authService.GetJwtToken();

            //input.Message = token;

            return await Task.FromResult(input);
        }

        public void SubProcess1()
        {

        }
        public void SubProcess2()
        {

        }
    }
}
