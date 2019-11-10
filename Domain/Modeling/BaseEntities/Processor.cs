using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Modeling.Abstract;

namespace Domain.Modeling.BaseEntities
{
    public class Processor : SchemaElementBase, ISchemaQueue<EventBase>
    {
        #region Statistic
        public int MaxQueue{ get; private set; }
        public double MeanQueue { get; private set; }
        public int FailedEvents { get; private set; }
        public double WorkingTime { get => cores.Select(c => c.WorkingTime).Sum() / cores.Count(); }
        #endregion

        public int QueueLimit { get; set; }
        public virtual IEnumerable<EventBase> Queue { get => eventsQueue; set => eventsQueue = new Queue<EventBase>(value); }
        public override ExecutionState ExecutionState { get => cores.Select(c => c.ExecutionState).Min(); }
        public override double NextEventTime { get => cores.Select(c => c.NextEventTime).Min(); }
        public override double CurrentTime
        {
            get => base.CurrentTime;
            set
            {
                base.CurrentTime = value;
                foreach(var core in cores)
                {
                    core.CurrentTime = value;
                }
            }
        }

        protected IEnumerable<Core> cores { get; set; }
        protected Queue<EventBase> eventsQueue { get; set; }

        public Processor(string name, Func<double> delayGenerator, int maxQueue = 5, int cores = 1) 
            : base(name, delayGenerator)
        {
            QueueLimit = maxQueue;
            eventsQueue = new Queue<EventBase>(maxQueue);

            var coreList = new List<Core>(cores);
            for (int i = 0; i < cores; i++)
            {
                coreList.Add(new Core(delayGenerator));
            }

            this.cores = coreList;
        }

        public override void MakeJob(double changedTime)
        {
            // usually only one
            var finishedEventCores = cores.Where(c => c.NextEventTime <= CurrentTime).ToList();

            foreach(var core in finishedEventCores)
            {
                base.OutAct();
                var finishedEvent = core.OutAct();
                NextElement.InAct(finishedEvent);

                if (Queue.Any())
                {
                    core.InAct(Queue.First());
                }
            }
        }

        public override void InAct(EventBase e)
        {
            base.InAct(e);
            var freeProcc = cores.FirstOrDefault(proc => proc.ExecutionState == ExecutionState.Idle);

            if (freeProcc != null)
            {
                freeProcc.InAct(e);
            }
            else
            {
                if (Queue.Count() < QueueLimit)
                {
                    eventsQueue.Enqueue(e);
                }
                else
                {
                    FailedEvents++;
                }
            }
        }

        public override EventBase OutAct()
        {
            return null;
        }

        public override void DisplayStatistic(double simulateTime)
        {
            Console.WriteLine($"[Processor] ===== {Name}");
            Console.WriteLine($"Mean length of queue = " + MeanQueue / simulateTime);
            Console.WriteLine($"Max length of queue = " + MaxQueue);
            Console.WriteLine($"Failure probability  = {FailedEvents / (double)(InputEvents)} % (succesful - {CompletedEvents}, failures - {FailedEvents}, total - {InputEvents})");
            Console.WriteLine($"Working time = {WorkingTime / simulateTime} % (time - {cores.Select(c => c.WorkingTime).Sum()})");
        }

        public override void DoStatistic(double changedTime)
        {
            var currentQueue = Queue.Count();
            MeanQueue += changedTime * currentQueue;
            MaxQueue = MaxQueue > currentQueue ? MaxQueue : currentQueue;
        }
    }
}
