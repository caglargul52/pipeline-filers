﻿namespace PipelineFilters
{
    public class Pipeline<T> where T : class, new()
    {
        public List<IStep<T>> Steps { get; } = new();
        public IStep<T>? CurrentStep { get; set; }
        public int CurrentStepIndex { get; set; } = 0;

        private readonly PipelineContext _context;

        private T _currentDto;

        public Pipeline(PipelineContext context)
        {
            _context = context;
            _currentDto = new T();
        }
        
        public Pipeline<T> AddStep(IStep<T> step)
        {
            step.AddContext(_context);
            step.AddPipeline(this);

            Steps.Add(step);

            return this;
        }

        public Pipeline<T> AddRangeStep(List<IStep<T>> steps)
        {
            foreach (IStep<T> step in steps)
            {
                step.AddContext(_context);

                Steps.Add(step);
            }

            return this;
        }

        public async Task<ExecutionResult<T>> ExecuteAsync()
        {
            if (CurrentStep?.Error is not null)
            {
                return new ExecutionResult<T>(false, _currentDto, CurrentStep?.Error);
            }
            
            if (CurrentStep?.IsContinueProcess == false)
            {
                return new ExecutionResult<T>(true, _currentDto, null);
            }
            
            for (int i = CurrentStepIndex; i < Steps.Count; i++)
            {
                _currentDto = await Steps[i].ExecuteAsync(_currentDto);

                CurrentStep = Steps[i];

                if (Steps[i].Error is not null)
                {
                    return new ExecutionResult<T>(false, _currentDto, Steps[i].Error);
                }
                
                if (!Steps[i].IsContinueProcess)
                {
                    break;
                }
                
                CurrentStepIndex++;
            }

            return new ExecutionResult<T>(true, _currentDto, null);
        }
    }
}
