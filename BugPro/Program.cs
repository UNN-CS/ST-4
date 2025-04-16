using System;
using Stateless;

public class Bug
{
    public enum State { Open, Assigned, Deferred, InProgress, Closed, InReview }
    private enum Trigger { Assign, Defer, StartProgress, Close, MarkAsDone }

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
          .Permit(Trigger.Defer, State.Deferred)
          .Permit(Trigger.MarkAsDone, State.InReview);

        sm.Configure(State.Deferred)
          .Permit(Trigger.Assign, State.Assigned);

        sm.Configure(State.Closed)
          .Permit(Trigger.Assign, State.Assigned)
          .Permit(Trigger.MarkAsDone, State.InProgress);

        sm.Configure(State.InReview)
          .Permit(Trigger.MarkAsDone, State.Closed);
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

    public void MarkAsDone()
    {
        sm.Fire(Trigger.MarkAsDone);
        Console.WriteLine("Mark As Done");
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
        bug.MarkAsDone();
        bug.Close();
        Console.WriteLine("Final State: " + bug.getState());
    }
}
