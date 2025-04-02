using System;
using Stateless;

public class Bug
{
    public enum State { Open, Assigned, Deferred, InProgress, Closed, Reopened }
    private enum Trigger { Assign, Defer, StartProgress, Close, Reopen }

    private StateMachine<State, Trigger> sm;

    public Bug(State state)
    {
        sm = new StateMachine<State, Trigger>(state);

        sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.StartProgress, State.InProgress);

        sm.Configure(State.Assigned)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Defer, State.Deferred)
            .Ignore(Trigger.Assign);

        sm.Configure(State.InProgress)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Defer, State.Deferred);

        sm.Configure(State.Deferred)
            .Permit(Trigger.Assign, State.Assigned);

        sm.Configure(State.Closed)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Reopen, State.Reopened);

        sm.Configure(State.Reopened)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.StartProgress, State.InProgress);
    }

    public void Assign()
    {
        sm.Fire(Trigger.Assign);
        Console.WriteLine("Assign");
    }

    public void Defer()
    {
        sm.Fire(Trigger.Defer);
        Console.WriteLine("Defer");
    }

    public void StartProgress()
    {
        sm.Fire(Trigger.StartProgress);
        Console.WriteLine("Start Progress");
    }

    public void Close()
    {
        sm.Fire(Trigger.Close);
        Console.WriteLine("Close");
    }

    public void Reopen()
    {
        sm.Fire(Trigger.Reopen);
        Console.WriteLine("Reopen");
    }

    public State getState()
    {
        return sm.State;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.StartProgress();
        bug.Defer();
        bug.Assign();
        bug.Close();
        bug.Reopen();
        bug.Assign();
        Console.WriteLine("Final State: " + bug.getState());
    }
}
