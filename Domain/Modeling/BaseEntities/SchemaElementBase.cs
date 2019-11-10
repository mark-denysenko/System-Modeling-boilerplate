using System;
using Domain.Modeling.Abstract;

namespace Domain.Modeling.BaseEntities
{
    public abstract class SchemaElementBase : IEventProducer, 
        ISchemaElementProcessing<EventBase>, IWorkingElement<EventBase>, IDelayGenerator
    {
        public string Name { get; protected set; }
        public SchemaElementBase NextElement { get; set; }
        public EventBase CurrentEvent { get; set; }
        public int InputEvents { get; protected set; }
        public int CompletedEvents { get; protected set; }
        public Func<double> GetDelay { get; set; }
        private double currentTime;
        public virtual ExecutionState ExecutionState { get; set; } = ExecutionState.Idle;

        public virtual double CurrentTime
        {
            get => currentTime;
            set
            {
                var deltaTime = value - currentTime;
                currentTime = value;
                MakeJob(deltaTime);
                DoStatistic(deltaTime);
            }
        }

        public virtual double NextEventTime { get; set; } = double.MaxValue;

        public SchemaElementBase(string name, Func<double> delayGenerator)
        {
            Name = name;
            GetDelay = delayGenerator;
        }

        public abstract void DoStatistic(double changedTime);
        public abstract void DisplayStatistic(double simulateTime);

        public virtual void MakeJob(double changedTime)
        {
            if (CurrentTime >= NextEventTime)
            {
                var finishedEvent = OutAct();
                if(finishedEvent != null)
                {
                    NextElement?.InAct(finishedEvent);
                }
            }
        }

        public virtual void InAct(EventBase element)
        {
            InputEvents++;
            CurrentEvent = element;
            ExecutionState = ExecutionState.Work;
            NextEventTime = CurrentTime + GetDelay();
        }

        public virtual EventBase OutAct()
        {
            NextEventTime = double.MaxValue;
            ExecutionState = ExecutionState.Idle;

            var finishedEvent = CurrentEvent;
            CompletedEvents++;
            CurrentEvent = null;

            return finishedEvent;
        }
    }
}
