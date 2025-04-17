using Stateless;

namespace BugPro;

public class Bug
{
    public enum State
    {
        Opened,
        Assigned,
        Deferred,
        Closed,
        Reviewed,
    }

    private enum Trigger
    {
        Assign,
        Defer,
        Close,
        Reopen,
        Review,
        Approve,
        Return,
        Resume,
    }

    private readonly StateMachine<State, Trigger> _sm;

    public Bug(State state)
    {
        _sm = new StateMachine<State, Trigger>(state);

        _sm.Configure(State.Opened)
            .Permit(Trigger.Assign, State.Assigned);

        _sm.Configure(State.Assigned)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Defer, State.Deferred)
            .Permit(Trigger.Review, State.Reviewed);

        _sm.Configure(State.Deferred)
            .Permit(Trigger.Resume, State.Assigned);

        _sm.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Opened);

        _sm.Configure(State.Reviewed)
            .Permit(Trigger.Approve, State.Closed)
            .Permit(Trigger.Return, State.Assigned);
    }

    public void Assign()
    {
        _sm.Fire(Trigger.Assign);
        Console.WriteLine("Assign");
    }

    public void Defer()
    {
        _sm.Fire(Trigger.Defer);
        Console.WriteLine("Defer");
    }

    public void Close()
    {
        _sm.Fire(Trigger.Close);
        Console.WriteLine("Close");
    }

    public void Reopen()
    {
        _sm.Fire(Trigger.Reopen);
        Console.WriteLine("Reopen");
    }

    public void Review()
    {
        _sm.Fire(Trigger.Review);
        Console.WriteLine("Review");
    }

    public void Approve()
    {
        _sm.Fire(Trigger.Approve);
        Console.WriteLine("Approve");
    }

    public void Return()
    {
        _sm.Fire(Trigger.Return);
        Console.WriteLine("Return");
    }

    public void Resume()
    {
        _sm.Fire(Trigger.Resume);
        Console.WriteLine("Resume");
    }

    public State GetState()
    {
        return _sm.State;
    }
}

internal static class Program
{
    public static void Main()
    {
        var bug = new Bug(Bug.State.Opened);
        bug.Assign();
        bug.Review();
        bug.Return();
        bug.Review();
        bug.Approve();
        bug.Reopen();
        bug.Assign();
        bug.Defer();
        bug.Resume();
        bug.Close();
    }
}