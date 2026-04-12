using System.Text;
using Stateless;
using Stateless.Graph;

namespace BugPro;

public enum State
{
    New,
    Triage,
    AwaitingInformation,
    InWork,
    Review,
    Returned,
    Deferred,
    Closed
}

public enum Trigger
{
    Submit,
    AskForInfo,
    ProvideInfo,
    Assign,
    CompleteWork,
    Accept,
    Reject,
    Defer,
    Resume,
    Cancel,
    Reopen
}

public sealed class Bug
{
    private readonly StateMachine<State, Trigger> _machine;

    public Bug(string title)
    {
        Title = title;
        _machine = new StateMachine<State, Trigger>(State.New);
        ConfigureMachine();
    }

    public string Title { get; }
    public State CurrentState => _machine.State;
    public int TransitionCount { get; private set; }

    public void Fire(Trigger trigger) => _machine.Fire(trigger);

    public bool CanFire(Trigger trigger) => _machine.CanFire(trigger);

    public IEnumerable<Trigger> PermittedTriggers => _machine.PermittedTriggers;

    private void ConfigureMachine()
    {
        _machine.OnTransitioned(t => TransitionCount++);

        _machine.Configure(State.New)
            .Permit(Trigger.Submit, State.Triage)
            .Permit(Trigger.Cancel, State.Closed);

        _machine.Configure(State.Triage)
            .Permit(Trigger.Assign, State.InWork)
            .Permit(Trigger.AskForInfo, State.AwaitingInformation)
            .Permit(Trigger.Defer, State.Deferred)
            .Permit(Trigger.Reject, State.Closed);

        _machine.Configure(State.AwaitingInformation)
            .Permit(Trigger.ProvideInfo, State.Triage)
            .Permit(Trigger.Cancel, State.Closed);

        _machine.Configure(State.InWork)
            .Permit(Trigger.CompleteWork, State.Review)
            .Permit(Trigger.AskForInfo, State.AwaitingInformation)
            .Permit(Trigger.Defer, State.Deferred);

        _machine.Configure(State.Review)
            .Permit(Trigger.Accept, State.Closed)
            .Permit(Trigger.Reject, State.Returned);

        _machine.Configure(State.Returned)
            .Permit(Trigger.Assign, State.InWork)
            .Permit(Trigger.Defer, State.Deferred)
            .Permit(Trigger.AskForInfo, State.AwaitingInformation);

        _machine.Configure(State.Deferred)
            .Permit(Trigger.Resume, State.Triage);

        _machine.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Triage);
    }

    public string ToDotGraph() => UmlDotGraph.Format(_machine.GetInfo());
}

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("=== BugPro: Workflow Demo ===");

        var bug = new Bug("UI glitch on main screen");
        Console.WriteLine($"Bug: {bug.Title}");
        Console.WriteLine($"Initial State: {bug.CurrentState}");

        Console.WriteLine("\nSubmitting bug...");
        bug.Fire(Trigger.Submit);
        Console.WriteLine($"Current State: {bug.CurrentState}");

        Console.WriteLine("\nAssigning to developer...");
        bug.Fire(Trigger.Assign);
        Console.WriteLine($"Current State: {bug.CurrentState}");

        Console.WriteLine("\nCompleting work...");
        bug.Fire(Trigger.CompleteWork);
        Console.WriteLine($"Current State: {bug.CurrentState}");

        Console.WriteLine("\nAccepting fix...");
        bug.Fire(Trigger.Accept);
        Console.WriteLine($"Current State: {bug.CurrentState}");

        Console.WriteLine($"\nTotal transitions: {bug.TransitionCount}");
        
        Console.WriteLine("\nDOT Graph Preview:");
        var dot = bug.ToDotGraph();
        Console.WriteLine(dot.Length > 500 ? dot[..500] + "..." : dot);
    }
}
