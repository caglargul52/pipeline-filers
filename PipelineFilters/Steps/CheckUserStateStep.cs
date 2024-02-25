﻿namespace PipelineFilters.Steps
{
    public class CheckUserStateStep : StepBase<PrepareActivationStepDto>
    {
        public override async Task<PrepareActivationStepDto> ProcessAsync(PrepareActivationStepDto input)
        {
            Console.WriteLine("CheckUserStateStep");
            
            var token = "dasıdjasd";

            Console.WriteLine("Token oluşturuldu : " + token);

            var output = input with { Token = token };

            return Stop(output);
        }
    }
}
