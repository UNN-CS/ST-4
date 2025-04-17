using Stateless;

public class Bug
{
    public enum State { Open, Assigned, Defered, Closed, Reopened, Verified }
    private enum Trigger { Assign, Defer, Close, Reopen, Verify, Deny }

    private StateMachine<State, Trigger> sm;

    public Bug(State state)
    {
        sm = new StateMachine<State, Trigger>(state);

        sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned);

        sm.Configure(State.Assigned)
            .Permit(Trigger.Defer, State.Defered)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Verify, State.Verified)
            .Ignore(Trigger.Assign);

        sm.Configure(State.Defered)
            .Permit(Trigger.Assign, State.Assigned);

        sm.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);

        sm.Configure(State.Reopened)
            .Permit(Trigger.Assign, State.Assigned);

        sm.Configure(State.Verified)
            .Permit(Trigger.Deny, State.Reopened);
    }

    public void Assign() => Fire(Trigger.Assign, "Assign");
    public void Close() => Fire(Trigger.Close, "Close");
    public void Defer() => Fire(Trigger.Defer, "Defer");
    public void Reopen() => Fire(Trigger.Reopen, "Reopen");
    public void Verify() => Fire(Trigger.Verify, "Verify");
    public void Deny() => Fire(Trigger.Deny, "Deny");

    private void Fire(Trigger trigger, string action)
    {
        sm.Fire(trigger);
        Console.WriteLine(action);
    }

    public State GetState() => sm.State;
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
        bug.Verify();
        bug.Deny();
        Console.WriteLine(bug.GetState());
    }
}