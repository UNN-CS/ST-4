using Stateless;

namespace BugPro;

public class Bug
{
    public enum State { Open, Assigned, Defered, Closed, Verified, Reopened }
    private enum Trigger { Assign, Defer, Close, Verify, Reopen }

    private readonly StateMachine<State, Trigger> _sm;

    public Bug(State state)
    {
        _sm = new StateMachine<State, Trigger>(state);

        _sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned);

        _sm.Configure(State.Assigned)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Defer, State.Defered)
            .Ignore(Trigger.Assign);

        _sm.Configure(State.Closed)
            .Permit(Trigger.Verify, State.Verified)
            .Permit(Trigger.Reopen, State.Reopened);

        _sm.Configure(State.Defered)
            .Permit(Trigger.Assign, State.Assigned);

        _sm.Configure(State.Verified)
            .Permit(Trigger.Reopen, State.Reopened);

        _sm.Configure(State.Reopened)
            .Permit(Trigger.Assign, State.Assigned);
    }

    public void Close() { _sm.Fire(Trigger.Close); Console.WriteLine("Close"); }
    public void Assign() { _sm.Fire(Trigger.Assign); Console.WriteLine("Assign"); }
    public void Defer() { _sm.Fire(Trigger.Defer); Console.WriteLine("Defer"); }
    public void Verify() { _sm.Fire(Trigger.Verify); Console.WriteLine("Verify"); }
    public void Reopen() { _sm.Fire(Trigger.Reopen); Console.WriteLine("Reopen"); }

    public State GetState() => _sm.State;
}

public class Program
{
    public static void Main(string[] args)
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Verify();
        bug.Reopen();
        bug.Assign();
        bug.Defer();
        bug.Assign();
        Console.WriteLine(bug.GetState());
    }
}