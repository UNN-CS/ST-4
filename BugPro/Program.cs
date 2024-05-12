namespace BugPro;

using Stateless;

public class Bug
{
    public enum State
    {
        Open,
        Assigned,
        FixCreated,
        FixAccepted,
        FixDeclined,
        Deferred,
        Closed
    }

    private enum Trigger
    {
        Assign,
        CreateFix,
        AcceptFix,
        DeclineFix,
        Defer,
        Close
    }

    private readonly StateMachine<State, Trigger> sm;

    public Bug(State state)
    {
        sm = new StateMachine<State, Trigger>(state);
        sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned);
        sm.Configure(State.Assigned)
            .Permit(Trigger.CreateFix, State.FixCreated)
            .Permit(Trigger.Defer, State.Deferred)
            .Permit(Trigger.Close, State.Closed)
            .Ignore(Trigger.Assign);
        sm.Configure(State.FixCreated)
            .Permit(Trigger.AcceptFix, State.FixAccepted)
            .Permit(Trigger.DeclineFix, State.FixDeclined)
            .Ignore(Trigger.CreateFix);
        sm.Configure(State.FixDeclined)
            .Permit(Trigger.CreateFix, State.FixCreated);
        sm.Configure(State.FixAccepted)
            .Permit(Trigger.Close, State.Closed);
        sm.Configure(State.Closed)
            .Permit(Trigger.Assign, State.Assigned);
        sm.Configure(State.Deferred)
            .Permit(Trigger.Assign, State.Assigned);
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

    public void CreateFix()
    {
        sm.Fire(Trigger.CreateFix);
        Console.WriteLine("Create Fix");
    }

    public void AcceptFix()
    {
        sm.Fire(Trigger.AcceptFix);
        Console.WriteLine("Accept Fix");
    }

    public void DeclineFix()
    {
        sm.Fire(Trigger.DeclineFix);
        Console.WriteLine("Decline Fix");
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
        bug.Close();
        bug.Assign();
        bug.Defer();
        bug.Assign();
        Console.WriteLine(bug.getState());
    }
}
