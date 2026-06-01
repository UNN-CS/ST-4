using Stateless;

namespace BugPro;

public sealed class Bug
{
    public enum State
    {
        Draft,
        Reported,
        Analyzing,
        Fixing,
        ReadyForReview,
        Released,
        Suspended,
        WonTDo,
        Duplicate,
        Returned
    }

    public enum Trigger
    {
        Submit,
        StartAnalysis,
        ApproveFix,
        Pause,
        Resume,
        SendToReview,
        Ship,
        RejectReview,
        MarkWonTDo,
        MarkDuplicate,
        Reopen
    }

    private readonly List<string> _journal = [];
    private readonly StateMachine<State, Trigger> _machine;

    public Bug()
    {
        _machine = new StateMachine<State, Trigger>(State.Draft);
        BuildStateMachine();
    }

    public State CurrentState => _machine.State;

    public IReadOnlyList<string> Journal => _journal.AsReadOnly();

    public bool CanFire(Trigger trigger) => _machine.CanFire(trigger);

    public bool IsTerminal =>
        CurrentState is State.Released or State.WonTDo or State.Duplicate;

    public void Submit() => Fire(Trigger.Submit);

    public void StartAnalysis() => Fire(Trigger.StartAnalysis);

    public void ApproveFix() => Fire(Trigger.ApproveFix);

    public void Pause() => Fire(Trigger.Pause);

    public void Resume() => Fire(Trigger.Resume);

    public void SendToReview() => Fire(Trigger.SendToReview);

    public void Ship() => Fire(Trigger.Ship);

    public void RejectReview() => Fire(Trigger.RejectReview);

    public void MarkWonTDo() => Fire(Trigger.MarkWonTDo);

    public void MarkDuplicate() => Fire(Trigger.MarkDuplicate);

    public void Reopen() => Fire(Trigger.Reopen);

    public int CountAvailableActions() =>
        Enum.GetValues<Trigger>().Count(CanFire);

    public string Describe() =>
        $"state={CurrentState}; terminal={IsTerminal}; transitions={Journal.Count}";

    public override string ToString() => Describe();

    private void BuildStateMachine()
    {
        _machine.OnTransitioned(transition => _journal.Add(
            $"{transition.Source} => {transition.Trigger} => {transition.Destination}"));

        _machine.Configure(State.Draft)
            .Permit(Trigger.Submit, State.Reported);

        _machine.Configure(State.Reported)
            .Permit(Trigger.StartAnalysis, State.Analyzing)
            .Permit(Trigger.MarkWonTDo, State.WonTDo)
            .Permit(Trigger.MarkDuplicate, State.Duplicate);

        _machine.Configure(State.Analyzing)
            .Permit(Trigger.ApproveFix, State.Fixing)
            .Permit(Trigger.Pause, State.Suspended)
            .Permit(Trigger.MarkWonTDo, State.WonTDo);

        _machine.Configure(State.Fixing)
            .Permit(Trigger.SendToReview, State.ReadyForReview)
            .Permit(Trigger.Pause, State.Suspended);

        _machine.Configure(State.ReadyForReview)
            .Permit(Trigger.Ship, State.Released)
            .Permit(Trigger.RejectReview, State.Returned);

        _machine.Configure(State.Returned)
            .Permit(Trigger.ApproveFix, State.Fixing)
            .Permit(Trigger.MarkWonTDo, State.WonTDo);

        _machine.Configure(State.Suspended)
            .Permit(Trigger.Resume, State.Analyzing);

        _machine.Configure(State.Released)
            .Permit(Trigger.Reopen, State.Returned);

        _machine.Configure(State.WonTDo)
            .Permit(Trigger.Reopen, State.Returned);

        _machine.Configure(State.Duplicate)
            .Permit(Trigger.Reopen, State.Returned);
    }

    private void Fire(Trigger trigger) => _machine.Fire(trigger);
}

public static class Program
{
    public static void Main()
    {
        var bug = new Bug();

        Console.WriteLine("Release workflow demo");
        Console.WriteLine(bug.Describe());

        bug.Submit();
        bug.StartAnalysis();
        bug.ApproveFix();
        bug.SendToReview();
        bug.RejectReview();

        Console.WriteLine(bug.Describe());
        foreach (var entry in bug.Journal)
        {
            Console.WriteLine(entry);
        }
    }
}
