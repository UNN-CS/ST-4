using Stateless;
using System;
public class Bug
{
    public enum State { Open, Assigned, Defered, Closed, Reopened, Verified, Rejected }
    private enum Trigger { Assign, Defer, Close, Reopen, Verify, Reject }
    private StateMachine<State, Trigger> sm;

    public Bug(State state)
    {
        sm = new StateMachine<State, Trigger>(state);
        
        sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Close, State.Closed)
            .Ignore(Trigger.Defer);
        
        sm.Configure(State.Assigned)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Defer, State.Defered)
            .Permit(Trigger.Reject, State.Rejected)
            .Ignore(Trigger.Assign);
        
        sm.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Reopened)
            .Permit(Trigger.Verify, State.Verified)
            .Ignore(Trigger.Close);
        
        sm.Configure(State.Defered)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Close, State.Closed);
        
        sm.Configure(State.Reopened)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Close, State.Closed);
        
        sm.Configure(State.Verified)
            .Permit(Trigger.Reopen, State.Reopened)
            .Permit(Trigger.Close, State.Closed);
        
        sm.Configure(State.Rejected)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Close, State.Closed);
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

    public void Defer()
    {
        sm.Fire(Trigger.Defer);
        Console.WriteLine("Defer");
    }

    public void Reopen()
    {
        sm.Fire(Trigger.Reopen);
        Console.WriteLine("Reopen");
    }

    public void Verify()
    {
        sm.Fire(Trigger.Verify);
        Console.WriteLine("Verify");
    }

    public void Reject()
    {
        sm.Fire(Trigger.Reject);
        Console.WriteLine("Reject");
    }

    public State GetState()
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
        bug.Close();
        bug.Reopen();
        bug.Assign();
        bug.Defer();
        bug.Assign();
        bug.Reject();
        bug.Assign();
        bug.Close();
        bug.Verify();
        Console.WriteLine(bug.GetState());
    }
}