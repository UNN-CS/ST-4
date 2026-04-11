using System;
using Stateless;

namespace BugPro
{
    public enum State
    {
        New,
        Analysis,
        Deferred,
        Fixing,
        Unreproducible,
        Returned,
        Rejected,
        Closed
    }

    public enum Trigger
    {
        Assign,
        Defer,
        Reject,
        StartFixing,
        CannotReproduce,
        Resolve,
        VerifyFail,
        VerifySuccess,
        ReturnToAnalysis,
        Reopen
    }

    public class Bug
    {
        private readonly StateMachine<State, Trigger> _machine;

        public State CurrentState => _machine.State;

        public Bug(State initialState = State.New)
        {
            _machine = new StateMachine<State, Trigger>(initialState);

            _machine.Configure(State.New)
                .Permit(Trigger.Assign, State.Analysis);

            _machine.Configure(State.Analysis)
                .Permit(Trigger.StartFixing, State.Fixing)
                .Permit(Trigger.Reject, State.Rejected)
                .Permit(Trigger.Defer, State.Deferred);

            _machine.Configure(State.Deferred)
                .Permit(Trigger.ReturnToAnalysis, State.Analysis);

            _machine.Configure(State.Fixing)
                .Permit(Trigger.Resolve, State.Closed)
                .Permit(Trigger.VerifyFail, State.Returned)
                .Permit(Trigger.CannotReproduce, State.Unreproducible);

            _machine.Configure(State.Unreproducible)
                .Permit(Trigger.VerifySuccess, State.Closed)
                .Permit(Trigger.VerifyFail, State.Returned);

            _machine.Configure(State.Returned)
                .Permit(Trigger.ReturnToAnalysis, State.Analysis);

            _machine.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Analysis);

            _machine.OnTransitioned(LogTransition);
        }

        private void LogTransition(StateMachine<State, Trigger>.Transition t)
        {
            Console.WriteLine($"{t.Source} -> {t.Trigger} -> {t.Destination}");
        }

        public void Fire(Trigger trigger) => _machine.Fire(trigger);
    }

    class Program
    {
        static void Main()
        {
            var bug = new Bug();

            Console.WriteLine($"Начальный статус дефекта: {bug.CurrentState}");
            
            bug.Fire(Trigger.Assign);
            Console.WriteLine($"Текущий статус: {bug.CurrentState}");

            bug.Fire(Trigger.StartFixing);
            Console.WriteLine($"Текущий статус: {bug.CurrentState}");

            bug.Fire(Trigger.VerifyFail);
            Console.WriteLine($"Текущий статус: {bug.CurrentState}");

            bug.Fire(Trigger.ReturnToAnalysis);
            Console.WriteLine($"Текущий статус: {bug.CurrentState}");

            bug.Fire(Trigger.Reject);
            Console.WriteLine($"Финальный статус: {bug.CurrentState}");
            
            Console.WriteLine("\nРабота завершена.");
        }
    }
}