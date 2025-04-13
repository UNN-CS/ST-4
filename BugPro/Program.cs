using Stateless;
using System;

public class Bug
{
    public enum State {
        Open,
        Assigned,
        Rejected, 
        InProgress,
        InReview,
        Approved,
        Defered, 
        Closed,
        Reopened
    }
    private enum Trigger {
        Reject,
        Assign,
        Start,
        StartReview,
        Approve,
        Defer, 
        Close,
        Reopen
    }
    private StateMachine<State, Trigger> sm;

    public Bug(State state)
    {
        sm = new StateMachine<State, Trigger>(state);

        sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Reject, State.Rejected);

        sm.Configure(State.Assigned)
              .Permit(Trigger.Reject, State.Rejected)
              .Permit(Trigger.Start, State.InProgress)
              .Ignore(Trigger.Assign).Ignore(Trigger.Reopen);

        sm.Configure(State.Rejected)
              .Permit(Trigger.Assign, State.Assigned)
               .Permit(Trigger.Defer, State.Defered)
               .Permit(Trigger.Close, State.Closed)
              .Ignore(Trigger.Assign);


        sm.Configure(State.InProgress)
              .Permit(Trigger.StartReview, State.InReview)
              .Permit(Trigger.Reject, State.Rejected)
              .Permit(Trigger.Defer, State.Defered)
              .Ignore(Trigger.Start);

        sm.Configure(State.InReview)
              .Permit(Trigger.Close, State.Closed)
              .Permit(Trigger.Approve, State.Approved)
               .Permit(Trigger.Reject, State.Rejected)
              .Ignore(Trigger.StartReview);

        sm.Configure(State.Approved)
      .Permit(Trigger.Close, State.Closed)
       .Permit(Trigger.Reject, State.Rejected)
      .Ignore(Trigger.Approve).Ignore(Trigger.Reject);

        sm.Configure(State.Defered)
               .Permit(Trigger.Reject, State.Rejected)
              .Permit(Trigger.Start, State.InProgress)
              .Ignore(Trigger.Assign);

        sm.Configure(State.Closed)
              .Permit(Trigger.Reopen, State.Reopened)
              .Ignore(Trigger.Close);

        sm.Configure(State.Reopened)
              .Permit(Trigger.Assign, State.Assigned)
              .Permit(Trigger.Reject, State.Rejected)
              .Ignore(Trigger.Reopen);

    }
    public void Close()
    {
        sm.Fire(Trigger.Close);
        Console.WriteLine("Close");
    }
    public void Assign()
    {
        sm.Fire(Trigger.Assign);
        Console.WriteLine("Assign");
    }
    public void Start()
    {
        sm.Fire(Trigger.Start);
        Console.WriteLine("Progress");
    }

    public void Defer()
    {
        sm.Fire(Trigger.Defer);
        Console.WriteLine("Defer");
    }

    public void StartReview()
    {
        sm.Fire(Trigger.StartReview);
        Console.WriteLine("InReview");
    }

    public void Approve()
    {
        sm.Fire(Trigger.Approve);
        Console.WriteLine("Approved");
    }
    public void Reject()
    {
        sm.Fire(Trigger.Reject);
        Console.WriteLine("Rejected");
    }

    public void Reopen()
    {
        sm.Fire(Trigger.Reopen);
        Console.WriteLine("Reopened");
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
        bug.Start();
        bug.Defer();
        bug.Start();
        bug.StartReview();
        bug.Reject();
        bug.Close();
        bug.Reopen();
        bug.Assign();
        bug.Start();
        bug.StartReview();
        bug.Approve();
        Console.WriteLine(bug.getState());
    }
}