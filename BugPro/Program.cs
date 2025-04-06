using Stateless;

public class Bug {
    public enum State {
        Open,
        Assigned,
        Defered,
        Closed,
        Reopened,
        Verified,
        Rejected
    }

    private enum Trigger {
        Assign,
        Defer,
        Close,
        Verify,
        Reject,
        Reopen
    }

    private StateMachine<State, Trigger> sm;

    public Bug(State state) {
        sm = new StateMachine<State, Trigger>(state);

        sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Reject, State.Rejected);

        sm.Configure(State.Assigned)
            .Permit(Trigger.Defer, State.Defered)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Verify, State.Verified)
            .Permit(Trigger.Reject, State.Rejected)
            .Ignore(Trigger.Assign);

        sm.Configure(State.Defered)
            .Permit(Trigger.Assign, State.Assigned);

        sm.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Reopened)
            .Permit(Trigger.Assign, State.Assigned);

        sm.Configure(State.Reopened)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Reject, State.Rejected);

        sm.Configure(State.Rejected)
            .Permit(Trigger.Reopen, State.Reopened);

        sm.Configure(State.Verified)
            .Permit(Trigger.Close, State.Closed);
    }

    public void Assign() {
        sm.Fire(Trigger.Assign);
        Console.WriteLine("Assign");
    }

    public void Close() {
        sm.Fire(Trigger.Close);
        Console.WriteLine("Close");
    }

    public void Defer() {
        sm.Fire(Trigger.Defer);
        Console.WriteLine("Defer");
    }

    public void Verify() {
        sm.Fire(Trigger.Verify);
        Console.WriteLine("Verify");
    }

    public void Reject() {
        sm.Fire(Trigger.Reject);
        Console.WriteLine("Reject");
    }

    public void Reopen() {
        sm.Fire(Trigger.Reopen);
        Console.WriteLine("Reopen");
    }

    public State getState() {
        return sm.State;
    }
}

public class Program
{
	public static void Main(string[] args)
	{
		var bug = new Bug(Bug.State.Open);

		bug.Assign();
		bug.Verify();
		bug.Close();
		bug.Reopen();
		bug.Assign();
		bug.Defer();
		bug.Assign();
		bug.Reject();
		bug.Reopen();
		bug.Assign();

		Console.WriteLine($"Final state: {bug.getState()}");
	}
}