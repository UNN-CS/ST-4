using Stateless;
using System;

public class Bug
{
    public enum State { Open, Assigned, Defered, Closed, InProgress, Verified, Rejected, NeedInfo }
    private enum Trigger { Assign, Defer, Close, Start, Verify, Reject, ReOpen, NeedMoreInfo, ProvideInfo }
    private StateMachine<State, Trigger> sm;

    public Bug(State state)
    {
        sm = new StateMachine<State, Trigger>(state);
        sm.Configure(State.Open)
              .Permit(Trigger.Assign, State.Assigned)
              .Permit(Trigger.Defer, State.Defered)
              .Permit(Trigger.NeedMoreInfo, State.NeedInfo);

        sm.Configure(State.Assigned)
              .Permit(Trigger.Close, State.Closed)
              .Permit(Trigger.Defer, State.Defered)
              .Permit(Trigger.Start, State.InProgress)
              .Permit(Trigger.NeedMoreInfo, State.NeedInfo)
              .Ignore(Trigger.Assign);

        sm.Configure(State.Closed)
              .Permit(Trigger.Assign, State.Assigned)
              .Permit(Trigger.ReOpen, State.Open);

        sm.Configure(State.Defered)
              .Permit(Trigger.Assign, State.Assigned)
              .Permit(Trigger.ReOpen, State.Open);

        sm.Configure(State.InProgress)
              .Permit(Trigger.Close, State.Closed)
              .Permit(Trigger.Verify, State.Verified)
              .Permit(Trigger.Defer, State.Defered)
              .Permit(Trigger.NeedMoreInfo, State.NeedInfo);

        sm.Configure(State.Verified)
              .Permit(Trigger.Close, State.Closed)
              .Permit(Trigger.Reject, State.Rejected);

        sm.Configure(State.Rejected)
              .Permit(Trigger.Assign, State.Assigned)
              .Permit(Trigger.Start, State.InProgress);

        sm.Configure(State.NeedInfo)
              .Permit(Trigger.ProvideInfo, State.Open)
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

    public void Start()
    {
        sm.Fire(Trigger.Start);
        Console.WriteLine("Start");
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

    public void ReOpen()
    {
        sm.Fire(Trigger.ReOpen);
        Console.WriteLine("ReOpen");
    }

    public void NeedMoreInfo()
    {
        sm.Fire(Trigger.NeedMoreInfo);
        Console.WriteLine("NeedMoreInfo");
    }

    public void ProvideInfo()
    {
        sm.Fire(Trigger.ProvideInfo);
        Console.WriteLine("ProvideInfo");
    }

    public State getState()
    {
        return sm.State;
    }

    public bool CanClose()
    {
        return sm.CanFire(Trigger.Close);
    }

    public bool CanAssign()
    {
        return sm.CanFire(Trigger.Assign);
    }

    public bool CanDefer()
    {
        return sm.CanFire(Trigger.Defer);
    }

    public bool CanStart()
    {
        return sm.CanFire(Trigger.Start);
    }

    public bool CanVerify()
    {
        return sm.CanFire(Trigger.Verify);
    }

    public bool CanReject()
    {
        return sm.CanFire(Trigger.Reject);
    }

    public bool CanReOpen()
    {
        return sm.CanFire(Trigger.ReOpen);
    }

    public bool CanNeedMoreInfo()
    {
        return sm.CanFire(Trigger.NeedMoreInfo);
    }

    public bool CanProvideInfo()
    {
        return sm.CanFire(Trigger.ProvideInfo);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Start();
        bug.Verify();
        bug.Close();
        bug.ReOpen();
        bug.Assign();
        bug.Defer();
        bug.Assign();
        Console.WriteLine(bug.getState());
    }
}