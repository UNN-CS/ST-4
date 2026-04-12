// See https://aka.ms/new-console-template for more information
using System;
using Stateless;

namespace BugPro
{
    public class Bug
    {
        public enum State { Open, Assigned, Deferred, Resolved, Closed }
        public enum Trigger { Assign, Defer, Resolve, Close, Reopen, Reactivate }

        private State _state;
        private string? _assignee;
        private readonly StateMachine<State, Trigger> _machine;
        private readonly StateMachine<State, Trigger>.TriggerWithParameters<string> _assignTrigger;

        public Bug(State initial = State.Open)
        {
            _state = initial;
            _machine = new StateMachine<State, Trigger>(() => _state, s => _state = s);
            _assignTrigger = _machine.SetTriggerParameters<string>(Trigger.Assign);

            _machine.Configure(State.Open)
                .Permit(Trigger.Assign, State.Assigned)
                .Permit(Trigger.Defer, State.Deferred);

            _machine.Configure(State.Assigned)
                .OnEntryFrom(_assignTrigger, assignee => OnAssigned(assignee))
                .Permit(Trigger.Resolve, State.Resolved)
                .Permit(Trigger.Defer, State.Deferred)
                .PermitReentry(Trigger.Assign);

            _machine.Configure(State.Deferred)
                .OnEntry(() => _assignee = null)
                .Permit(Trigger.Reactivate, State.Open);

            _machine.Configure(State.Resolved)
                .Permit(Trigger.Close, State.Closed)
                .Permit(Trigger.Reopen, State.Open);

            _machine.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Open);
        }

        public State CurrentState => _state;
        public string? Assignee => _assignee;

        public void Assign(string assignee)
        {
            if (string.IsNullOrWhiteSpace(assignee))
                throw new ArgumentException("Assignee must be provided", nameof(assignee));
            _machine.Fire(_assignTrigger, assignee);
        }

        public void Defer() => _machine.Fire(Trigger.Defer);
        public void Resolve() => _machine.Fire(Trigger.Resolve);
        public void Close() => _machine.Fire(Trigger.Close);
        public void Reopen() => _machine.Fire(Trigger.Reopen);
        public void Reactivate() => _machine.Fire(Trigger.Reactivate);

        public bool CanFire(Trigger t) => _machine.CanFire(t);

        private void OnAssigned(string assignee)
        {
            _assignee = assignee;
        }
    }

    public static class Program
    {
        public static void Main()
        {
            var bug = new Bug();
            Console.WriteLine($"Initial: {bug.CurrentState}");
            bug.Assign("Alice");
            Console.WriteLine($"After Assign: {bug.CurrentState}, assignee={bug.Assignee}");
            bug.Resolve();
            Console.WriteLine($"After Resolve: {bug.CurrentState}");
            bug.Close();
            Console.WriteLine($"After Close: {bug.CurrentState}");
        }
    }
}
