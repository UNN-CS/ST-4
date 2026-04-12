using System;
using Stateless; 

namespace BugPro
{
    public enum State { New, Analysis, Fix, Closed, Rejected, MoreInfoRequired }
    public enum Trigger { Assign, StartFix, Resolve, Reject, NeedInfo, Reopen, ReturnToAnalysis }

    public class Bug
    {
        public State CurrentState => _machine.State;
        private readonly StateMachine<State, Trigger> _machine;

        public Bug()
        {
            _machine = new StateMachine<State, Trigger>(State.New);

            _machine.Configure(State.New)
                .Permit(Trigger.Assign, State.Analysis);

            _machine.Configure(State.Analysis)
                .Permit(Trigger.StartFix, State.Fix)
                .Permit(Trigger.Reject, State.Rejected)
                .Permit(Trigger.NeedInfo, State.MoreInfoRequired);

            _machine.Configure(State.MoreInfoRequired)
                .Permit(Trigger.ReturnToAnalysis, State.Analysis);

            _machine.Configure(State.Fix)
                .Permit(Trigger.Resolve, State.Closed)
                .Permit(Trigger.ReturnToAnalysis, State.Analysis);

            _machine.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Analysis);
        }

        public void Fire(Trigger trigger) => _machine.Fire(trigger);
    }

    class Program
    {
        static void Main(string[] args)
        {
            var bug = new Bug();
            Console.WriteLine($"Начальное состояние: {bug.CurrentState}");

            bug.Fire(Trigger.Assign);
            Console.WriteLine($"После разбора: {bug.CurrentState}");

            Console.WriteLine("Приложение BugPro запущено успешно.");
        }
    }
}