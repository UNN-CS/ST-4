using Stateless;

namespace BugPro;

public enum BugState
{
    New,
    InTriage,
    NotTimeNow,
    NeedSeparateFix,
    ExternalProblem,
    NeedMoreInfo,
    NotABug,
    NotFixing,
    Duplicate,
    CannotReproduce,
    Fixing,
    IsOK,
    IsProblemSolved,
    Closed,
    Reopened,
    Returned
}

public enum BugAction
{
    StartTriage,
    SetNotTimeNow,
    SetNeedSeparateFix,
    SetExternalProblem,
    SetNeedMoreInfo,
    SetNotABug,
    SetNotFixing,
    SetDuplicate,
    SetCannotReproduce,
    StartFixing,
    VerifyIsOK,
    VerifyIsProblemSolved,
    Approve,
    Reject,
    Reopen
}

public class Bug
{
    private readonly StateMachine<BugState, BugAction> _stateMachine;
    private readonly List<string> _history = [];

    public Bug()
    {
        _stateMachine = new StateMachine<BugState, BugAction>(BugState.New);
        Configure();
    }

    public BugState CurrentState => _stateMachine.State;

    public IReadOnlyCollection<string> History => _history.AsReadOnly();

    public IEnumerable<BugAction> PermittedActions => _stateMachine.GetPermittedTriggers();

    public bool CanPerformAction(BugAction action) => _stateMachine.CanFire(action);

    public void PerformAction(BugAction action) => _stateMachine.Fire(action);

    public void PrintCurrentState()
    {
        Console.WriteLine($"Current state of the bug: [{_stateMachine.State}]");
    }

    private void Configure()
    {
        _stateMachine.Configure(BugState.New)
            .Permit(BugAction.StartTriage, BugState.InTriage);

        _stateMachine.Configure(BugState.InTriage)
            .Permit(BugAction.SetNotTimeNow, BugState.NotTimeNow)
            .Permit(BugAction.SetNeedSeparateFix, BugState.NeedSeparateFix)
            .Permit(BugAction.SetExternalProblem, BugState.ExternalProblem)
            .Permit(BugAction.SetNeedMoreInfo, BugState.NeedMoreInfo)
            .Permit(BugAction.SetNotABug, BugState.NotABug)
            .Permit(BugAction.SetNotFixing, BugState.NotFixing)
            .Permit(BugAction.SetDuplicate, BugState.Duplicate)
            .Permit(BugAction.StartFixing, BugState.Fixing);

        _stateMachine.Configure(BugState.NotTimeNow)
            .Permit(BugAction.StartTriage, BugState.InTriage)
            .Permit(BugAction.StartFixing, BugState.Fixing);

        _stateMachine.Configure(BugState.NeedSeparateFix)
            .Permit(BugAction.StartTriage, BugState.InTriage)
            .Permit(BugAction.StartFixing, BugState.Fixing);

        _stateMachine.Configure(BugState.ExternalProblem)
            .Permit(BugAction.StartTriage, BugState.InTriage)
            .Permit(BugAction.StartFixing, BugState.Fixing);

        _stateMachine.Configure(BugState.NeedMoreInfo)
            .Permit(BugAction.StartTriage, BugState.InTriage)
            .Permit(BugAction.StartFixing, BugState.Fixing);

        _stateMachine.Configure(BugState.NotABug)
            .Permit(BugAction.VerifyIsOK, BugState.IsOK);

        _stateMachine.Configure(BugState.NotFixing)
            .Permit(BugAction.VerifyIsOK, BugState.IsOK);

        _stateMachine.Configure(BugState.Duplicate)
            .Permit(BugAction.VerifyIsOK, BugState.IsOK);

        _stateMachine.Configure(BugState.CannotReproduce)
            .Permit(BugAction.VerifyIsOK, BugState.IsOK);

        _stateMachine.Configure(BugState.Fixing)
            .Permit(BugAction.VerifyIsProblemSolved, BugState.IsProblemSolved)
            .Permit(BugAction.SetNeedMoreInfo, BugState.NeedMoreInfo)
            .Permit(BugAction.SetCannotReproduce, BugState.CannotReproduce);

        _stateMachine.Configure(BugState.IsOK)
            .Permit(BugAction.Approve, BugState.Closed)
            .Permit(BugAction.Reject, BugState.Returned);

        _stateMachine.Configure(BugState.IsProblemSolved)
            .Permit(BugAction.Approve, BugState.Closed)
            .Permit(BugAction.Reject, BugState.Reopened);

        _stateMachine.Configure(BugState.Closed)
            .Permit(BugAction.Reopen, BugState.Reopened);

        _stateMachine.Configure(BugState.Reopened)
            .Permit(BugAction.StartTriage, BugState.InTriage);

        _stateMachine.OnTransitioned(transition =>
            _history.Add(
                $"Transition from [{transition.Source}] to [{transition.Destination}] with trigger [{transition.Trigger}]"));
    }
}

class Program
{
    static void Main(string[] args)
    {
        var bug = new Bug();

        Console.WriteLine("Bug workflow demonstration");
        PrintBugStatus(bug);

        PerformAndPrint(bug, BugAction.StartTriage);
        PerformAndPrint(bug, BugAction.SetNeedMoreInfo);
        PerformAndPrint(bug, BugAction.StartFixing);
        PerformAndPrint(bug, BugAction.VerifyIsProblemSolved);
        PerformAndPrint(bug, BugAction.Reject);
        PerformAndPrint(bug, BugAction.StartTriage);
        PerformAndPrint(bug, BugAction.StartFixing);
        PerformAndPrint(bug, BugAction.VerifyIsProblemSolved);
        PerformAndPrint(bug, BugAction.Approve);

        Console.WriteLine();
        Console.WriteLine("Transition history:");
        foreach (var transition in bug.History)
        {
            Console.WriteLine(transition);
        }
    }

    private static void PerformAndPrint(Bug bug, BugAction action)
    {
        Console.WriteLine();
        Console.WriteLine($"Action: [{action}]");
        bug.PerformAction(action);
        PrintBugStatus(bug);
    }

    private static void PrintBugStatus(Bug bug)
    {
        bug.PrintCurrentState();
        Console.WriteLine($"Permitted actions: [{string.Join(", ", bug.PermittedActions)}]");
    }
}
