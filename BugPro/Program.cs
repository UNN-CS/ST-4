using Stateless;

public class Bug 
{
    public enum State { Open, Assigned, Deferred, Closed, Reopened, Verified, Rejected, InProgress, InReview }
    private enum Trigger { Assign, Defer, Close, Reopen, Verify, Reject, StartProgress, MarkAsDone }
    
    private StateMachine<State, Trigger> sm;

    public Bug(State state) 
    {
        sm = new StateMachine<State, Trigger>(state);

        sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.StartProgress, State.InProgress)
            .Permit(Trigger.Reject, State.Rejected);

        sm.Configure(State.Assigned)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Defer, State.Deferred)
            .Permit(Trigger.StartProgress, State.InProgress)
            .Permit(Trigger.Reject, State.Rejected)
            .Ignore(Trigger.Assign);

        sm.Configure(State.InProgress)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Defer, State.Deferred)
            .Permit(Trigger.MarkAsDone, State.InReview)
            .Permit(Trigger.Reject, State.Rejected);

        sm.Configure(State.Deferred)
            .Permit(Trigger.Assign, State.Assigned);

        sm.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Reopened)
            .Permit(Trigger.Verify, State.Verified);

        sm.Configure(State.Reopened)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Reject, State.Rejected);

        sm.Configure(State.Verified)
            .Permit(Trigger.Reopen, State.Reopened);

        sm.Configure(State.Rejected)
            .Permit(Trigger.Assign, State.Assigned);

        sm.Configure(State.InReview)
            .Permit(Trigger.MarkAsDone, State.Closed);
    }

    public void Close() => sm.Fire(Trigger.Close);
    public void Assign() => sm.Fire(Trigger.Assign);
    public void Defer() => sm.Fire(Trigger.Defer);
    public void Reopen() => sm.Fire(Trigger.Reopen);
    public void Verify() => sm.Fire(Trigger.Verify);
    public void Reject() => sm.Fire(Trigger.Reject);
    public void StartProgress() => sm.Fire(Trigger.StartProgress);
    public void MarkAsDone() => sm.Fire(Trigger.MarkAsDone);

    public State GetState() => sm.State;
}

public class Program 
{
    public static void Main(string[] args) 
    {
        var bug = new Bug(Bug.State.Open);
        Console.WriteLine($"Initial state: {bug.GetState()}");

        bug.StartProgress();
        Console.WriteLine($"After StartProgress: {bug.GetState()}");

        bug.MarkAsDone();
        Console.WriteLine($"After MarkAsDone: {bug.GetState()}");

        bug.Close();
        Console.WriteLine($"After Close: {bug.GetState()}");

        bug.Verify();
        Console.WriteLine($"Final state: {bug.GetState()}");
    }
}