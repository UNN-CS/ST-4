using System;
using Stateless;

namespace BugPro
{
    public enum State
    {
        New,
        Triaged,
        Assigned,
        InProgress,
        Testing,
        Fixed,
        Closed,
        Reopened,
        Deferred
    }

    public enum Trigger
    {
        Triage,
        Assign,
        StartWork,
        SendToTesting,
        Fix,
        Close,
        Reopen,
        Defer,
        Reject
    }

    public class Bug
    {
        private State _state = State.New;
        private readonly StateMachine<State, Trigger> _machine;

        public string Title { get; }
        public string? Developer { get; private set; }

        public State CurrentState => _machine.State;

        private readonly StateMachine<State, Trigger>
            .TriggerWithParameters<string> _assignTrigger;

        public Bug(string title)
        {
            Title = title;

            _machine = new StateMachine<State, Trigger>(
                () => _state,
                s => _state = s);

            _assignTrigger =
                _machine.SetTriggerParameters<string>(Trigger.Assign);

            _machine.Configure(State.New)
                .Permit(Trigger.Triage, State.Triaged);

            _machine.Configure(State.Triaged)
                .PermitDynamic(
                    Trigger.Assign,
                    () => State.Assigned)
                .Permit(Trigger.Defer, State.Deferred);

            _machine.Configure(State.Assigned)
                .OnEntryFrom(
                    _assignTrigger,
                    dev => Developer = dev)
                .Permit(Trigger.StartWork, State.InProgress)
                .PermitReentry(Trigger.Assign);

            _machine.Configure(State.InProgress)
                .Permit(Trigger.SendToTesting, State.Testing)
                .Permit(Trigger.Defer, State.Deferred);

            _machine.Configure(State.Testing)
                .Permit(Trigger.Fix, State.Fixed)
                .Permit(Trigger.Reject, State.Reopened);

            _machine.Configure(State.Fixed)
                .Permit(Trigger.Close, State.Closed)
                .Permit(Trigger.Reopen, State.Reopened);

            _machine.Configure(State.Reopened)
                .Permit(Trigger.Assign, State.Assigned);

            _machine.Configure(State.Deferred)
                .Permit(Trigger.Assign, State.Assigned);

            _machine.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Reopened);
        }

        public void Triage()
        {
            _machine.Fire(Trigger.Triage);
        }

        public void Assign(string developer)
        {
            _machine.Fire(_assignTrigger, developer);
        }

        public void StartWork()
        {
            _machine.Fire(Trigger.StartWork);
        }

        public void SendToTesting()
        {
            _machine.Fire(Trigger.SendToTesting);
        }

        public void Fix()
        {
            _machine.Fire(Trigger.Fix);
        }

        public void Close()
        {
            _machine.Fire(Trigger.Close);
        }

        public void Reopen()
        {
            _machine.Fire(Trigger.Reopen);
        }

        public void Defer()
        {
            _machine.Fire(Trigger.Defer);
        }

        public void Reject()
        {
            _machine.Fire(Trigger.Reject);
        }

        public bool CanFire(Trigger trigger)
        {
            return _machine.CanFire(trigger);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Bug bug = new Bug("Ошибка авторизации");

            Console.WriteLine(
                $"Bug: {bug.Title}\nState: {bug.CurrentState}");

            bug.Triage();
            Console.WriteLine(
                $"After triage: {bug.CurrentState}");

            bug.Assign("AlenaDev");
            Console.WriteLine(
                $"Assigned to: {bug.Developer}");

            bug.StartWork();
            Console.WriteLine(
                $"Current state: {bug.CurrentState}");

            bug.SendToTesting();
            Console.WriteLine(
                $"Current state: {bug.CurrentState}");

            bug.Fix();
            Console.WriteLine(
                $"Current state: {bug.CurrentState}");

            bug.Close();
            Console.WriteLine(
                $"Final state: {bug.CurrentState}");
        }
    }
}
