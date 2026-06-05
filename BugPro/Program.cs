using System;
using System.Collections.Generic;
using System.Linq;
using Stateless;

namespace BugPro
{
    public class Bug
    {
        public enum State
        {
            New, Sort, InProgress, AwaitingInfo, Deferred, Verified, ReadyForRelease, Closed, Reopened
        }

        public enum Trigger
        {
            StartSort, AssignDev, MarkFixed, RequestInfo, ProvideInfo,
            Defer, Go, VerifyPass, VerifyFail, Reopen, Close, Release
        }

        private readonly StateMachine<State, Trigger> _engine;
        private readonly List<string> _history;

        public string Id { get; }
        public string Title { get; }
        public int Priority { get; }
        public State TekushayaState => _engine.State;
        public IReadOnlyList<string> History => _history.AsReadOnly();

        public Bug(string id, string title, int priority = 3)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Priority = priority;
            _history = new List<string>();

            _engine = new StateMachine<State, Trigger>(State.New);
            ConfigureWorkflow();
            Log($"Created: {Id} - {Title}");
        }

        private void ConfigureWorkflow()
        {
            _engine.Configure(State.New)
                .Permit(Trigger.StartSort, State.Sort)
                .OnEntry(() => Log("Entered New"));

            _engine.Configure(State.Sort)
                .Permit(Trigger.AssignDev, State.InProgress)
                .Permit(Trigger.Defer, State.Deferred)
                .Permit(Trigger.Close, State.Closed)
                .Ignore(Trigger.RequestInfo)
                .Ignore(Trigger.StartSort)
                .OnEntry(() => Log("Entered Sort"));

            _engine.Configure(State.InProgress)
                .Permit(Trigger.MarkFixed, State.Verified)
                .Permit(Trigger.RequestInfo, State.AwaitingInfo)
                .Permit(Trigger.Defer, State.Deferred)
                .OnEntry(() => Log("Entered InProgress"));

            _engine.Configure(State.AwaitingInfo)
                .Permit(Trigger.ProvideInfo, State.InProgress)
                .OnEntry(() => Log("Entered AwaitingInfo"));

            _engine.Configure(State.Deferred)
                .Permit(Trigger.Go, State.InProgress)
                .OnEntry(() => Log("Entered Deferred"));

            _engine.Configure(State.Verified)
                .Permit(Trigger.VerifyPass, State.ReadyForRelease)
                .Permit(Trigger.VerifyFail, State.InProgress)
                .OnEntry(() => Log("Entered Verified"));

            _engine.Configure(State.ReadyForRelease)
                .Permit(Trigger.Release, State.Closed)
                .OnEntry(() => Log("Entered ReadyForRelease"));

            _engine.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Reopened)
                .Ignore(Trigger.Release)
                .Ignore(Trigger.MarkFixed)
                .OnEntry(() => Log("Entered Closed"));

            _engine.Configure(State.Reopened)
                .Permit(Trigger.StartSort, State.Sort)
                .Ignore(Trigger.Reopen)
                .OnEntry(() => Log("Entered Reopen"));

            _engine.OnTransitioned(t =>
                Log($"Transition: {t.Source} -> {t.Destination} via {t.Trigger}"));
        }

        private void Log(string message) => _history.Add($"[{DateTime.Now:HH:mm:ss}] {message}");

        public void StartSort() => _engine.Fire(Trigger.StartSort);
        public void AssignDev() => _engine.Fire(Trigger.AssignDev);
        public void MarkFixed() => _engine.Fire(Trigger.MarkFixed);
        public void RequestInfo() => _engine.Fire(Trigger.RequestInfo);
        public void ProvideInfo() => _engine.Fire(Trigger.ProvideInfo);
        public void Defer() => _engine.Fire(Trigger.Defer);
        public void Go() => _engine.Fire(Trigger.Go);
        public void VerifyPass() => _engine.Fire(Trigger.VerifyPass);
        public void VerifyFail() => _engine.Fire(Trigger.VerifyFail);
        public void Reopen() => _engine.Fire(Trigger.Reopen);
        public void Close() => _engine.Fire(Trigger.Close);
        public void Release() => _engine.Fire(Trigger.Release);

        public bool TryMarkFixed() => ExecuteIfAllowed(Trigger.MarkFixed);
        public bool TryReopen() => ExecuteIfAllowed(Trigger.Reopen);
        public bool TryRelease() => ExecuteIfAllowed(Trigger.Release);

        private bool ExecuteIfAllowed(Trigger trigger)
        {
            if (!_engine.CanFire(trigger)) return false;
            _engine.Fire(trigger);
            return true;
        }

        public IEnumerable<Trigger> GetPermittedTriggers() => _engine.PermittedTriggers;
        public bool CanFire(Trigger trigger) => _engine.CanFire(trigger);
        public bool IsInState(State expected) => _engine.State == expected;

        public override string ToString() => $"Bug#{Id} [{TekushayaState}] {Title}";
    }

    public class Program
    {
        public static void Main(string[] args)
        {

            var bug = new Bug("BUG", "Crash", priority: 1);
            Console.WriteLine(bug);

            bug.StartSort();
            Console.WriteLine($"{bug.TekushayaState}");

            bug.AssignDev();
            Console.WriteLine($"{bug.TekushayaState}");

            bug.MarkFixed();
            Console.WriteLine($"{bug.TekushayaState}");

            bug.VerifyPass();
            Console.WriteLine($"{bug.TekushayaState}");

            bug.Release();
            Console.WriteLine($"{bug.TekushayaState}");

            foreach (var entry in bug.History) Console.WriteLine(entry);
        }
    }
}