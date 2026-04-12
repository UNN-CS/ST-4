using System.Text;
using BugPro;
using Stateless;
using Stateless.Graph;

namespace BugPro
{

    public class Bug
    {
        private readonly StateMachine<BugState, BugTrigger> _machine;
        private readonly List<string> _history = [];

        public Bug(string title, string description = "")
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description;
            CreatedAt = DateTime.UtcNow;
            _machine = new StateMachine<BugState, BugTrigger>(BugState.New);
            ConfigureMachine();
        }

        public string Title { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }
        public DateTime? ClosedAt { get; private set; }
        public BugState CurrentState => _machine.State;
        public int TransitionCount { get; private set; }
        public IReadOnlyList<string> History => _history.AsReadOnly();
        public string? ClosedReason { get; private set; }

        public void Fire(BugTrigger trigger)
        {
            var oldState = CurrentState;
            _machine.Fire(trigger);
            _history.Add($"{DateTime.Now:HH:mm:ss} {oldState} -> {CurrentState} via {trigger}");

            if (CurrentState != BugState.Closed) return;
            ClosedAt = DateTime.UtcNow;
            ClosedReason = DetermineClosedReason(oldState, trigger);
        }

        public bool CanFire(BugTrigger trigger) => _machine.CanFire(trigger);

        [Obsolete("Obsolete")]
        public IEnumerable<BugTrigger> PermittedTriggers => _machine.PermittedTriggers;

        private static string DetermineClosedReason(BugState oldState, BugTrigger trigger)
        {
            return trigger switch
            {
                BugTrigger.Cancel => "Cancelled",
                BugTrigger.Reject when oldState == BugState.Triage => "Rejected during triage",
                BugTrigger.MarkNotBug => "Not a bug",
                BugTrigger.MarkWontFix => "Won't fix",
                BugTrigger.MarkDuplicate => "Duplicate",
                BugTrigger.MarkNotRepro => "Not reproducible",
                BugTrigger.Accept => "Fixed and accepted",
                _ => "Closed"
            };
        }

        private void ConfigureMachine()
        {
            _machine.OnTransitioned(t => TransitionCount++);

            _machine.Configure(BugState.New)
                .Permit(BugTrigger.Submit, BugState.Triage)
                .Permit(BugTrigger.Cancel, BugState.Closed);

            _machine.Configure(BugState.Triage)
                .Permit(BugTrigger.Assign, BugState.InWork)
                .Permit(BugTrigger.AskForInfo, BugState.AwaitingInfo)
                .Permit(BugTrigger.MarkOtherProduct, BugState.OtherProduct)
                .Permit(BugTrigger.MarkNeedDecisionLater, BugState.NeedDecisionLater)
                .Permit(BugTrigger.MarkNotBug, BugState.NotABug)
                .Permit(BugTrigger.MarkWontFix, BugState.WontFix)
                .Permit(BugTrigger.MarkDuplicate, BugState.Duplicate)
                .Permit(BugTrigger.MarkNotRepro, BugState.NotReproducible)
                .Permit(BugTrigger.Defer, BugState.Deferred)
                .Permit(BugTrigger.Reject, BugState.Closed);

            _machine.Configure(BugState.AwaitingInfo)
                .Permit(BugTrigger.ProvideInfo, BugState.Triage)
                .Permit(BugTrigger.Cancel, BugState.Closed);

            _machine.Configure(BugState.OtherProduct)
                .Permit(BugTrigger.Return, BugState.Triage)
                .Permit(BugTrigger.Cancel, BugState.Closed);

            _machine.Configure(BugState.NeedDecisionLater)
                .Permit(BugTrigger.Return, BugState.Triage)
                .Permit(BugTrigger.Cancel, BugState.Closed);

            foreach (var state in new[]
                         { BugState.NotABug, BugState.WontFix, BugState.Duplicate, BugState.NotReproducible })
            {
                _machine.Configure(state)
                    .Permit(BugTrigger.Return, BugState.Triage)
                    .Permit(BugTrigger.Cancel, BugState.Closed);
            }
            
            _machine.Configure(BugState.InWork)
                .Permit(BugTrigger.CompleteWork, BugState.Review)
                .Permit(BugTrigger.AskForInfo, BugState.AwaitingInfo)
                .Permit(BugTrigger.Defer, BugState.Deferred);

            _machine.Configure(BugState.Review)
                .Permit(BugTrigger.Accept, BugState.Closed)
                .Permit(BugTrigger.Reject, BugState.Returned);

            _machine.Configure(BugState.Returned)
                .Permit(BugTrigger.Assign, BugState.InWork)
                .Permit(BugTrigger.AskForInfo, BugState.AwaitingInfo)
                .Permit(BugTrigger.Defer, BugState.Deferred)
                .Permit(BugTrigger.Cancel, BugState.Closed);

            _machine.Configure(BugState.Deferred)
                .Permit(BugTrigger.Resume, BugState.Triage)
                .Permit(BugTrigger.Cancel, BugState.Closed);

            _machine.Configure(BugState.Closed)
                .Permit(BugTrigger.Reopen, BugState.Triage);
        }

        public string ToDotGraph() => UmlDotGraph.Format(_machine.GetInfo());

        public string GetFullReport()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"=== BUG REPORT ===");
            sb.AppendLine($"Title: {Title}");
            sb.AppendLine($"Description: {Description ?? "(none)"}");
            sb.AppendLine($"Created: {CreatedAt:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Current State: {CurrentState}");
            sb.AppendLine($"Transition Count: {TransitionCount}");
            if (ClosedAt.HasValue)
                sb.AppendLine($"Closed: {ClosedAt.Value:yyyy-MM-dd HH:mm:ss} (Reason: {ClosedReason})");
            sb.AppendLine($"\nHistory:");
            foreach (var entry in History.TakeLast(10))
                sb.AppendLine($"  {entry}");
            return sb.ToString();
        }
    }
}

internal static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var bug = new Bug("UI glitch on main screen", "Buttons are misaligned on mobile view");
        Console.WriteLine(bug.GetFullReport());

        Console.WriteLine("\n--- Starting workflow ---\n");

        bug.Fire(BugTrigger.Submit);
        Console.WriteLine($"After Submit: {bug.CurrentState}");

        bug.Fire(BugTrigger.MarkNotBug);
        Console.WriteLine($"After MarkNotBug: {bug.CurrentState}");

        bug.Fire(BugTrigger.Return);
        Console.WriteLine($"After Return to Triage: {bug.CurrentState}");

        bug.Fire(BugTrigger.Assign);
        Console.WriteLine($"After Assign: {bug.CurrentState}");

        bug.Fire(BugTrigger.CompleteWork);
        Console.WriteLine($"After CompleteWork: {bug.CurrentState}");

        bug.Fire(BugTrigger.Accept);
        Console.WriteLine($"After Accept: {bug.CurrentState}");

        Console.WriteLine($"\n{bug.GetFullReport()}");

        Console.WriteLine("\nDOT Graph (first 500 chars):");
        var dot = bug.ToDotGraph();
        Console.WriteLine(dot.Length > 500 ? dot[..500] + "..." : dot);
    }
}