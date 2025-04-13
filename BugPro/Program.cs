using Stateless;

public class Bug {
    public enum State { Open, Assigned, Deferred, Closed, InProgress, Reviewed }
    private enum Trigger { Assign, Defer, Close, InProgress, Review }
    private StateMachine<State, Trigger> sm;

    public Bug(State state) {
        sm = new StateMachine<State, Trigger>(state);
        sm.Configure(State.Open)
              .Permit(Trigger.Assign, State.Assigned);
        sm.Configure(State.Assigned)
              .Permit(Trigger.Close, State.Closed)
              .Permit(Trigger.Defer, State.Deferred)
              .Permit(Trigger.InProgress, State.InProgress)
              .Ignore(Trigger.Assign);
        sm.Configure(State.Closed)
              .Permit(Trigger.Assign, State.Assigned);
        sm.Configure(State.Deferred)
              .Permit(Trigger.Assign, State.Assigned)
              .Permit(Trigger.InProgress, State.InProgress);
        sm.Configure(State.InProgress)
              .Permit(Trigger.Defer, State.Deferred)
              .Permit(Trigger.Review, State.Reviewed);
        sm.Configure(State.Reviewed)
              .Permit(Trigger.InProgress, State.InProgress)
              .Permit(Trigger.Assign, State.Assigned)
              .Permit(Trigger.Close, State.Closed)
              .Ignore(Trigger.Review);
    }

    public void Close() {
        sm.Fire(Trigger.Close);
        Console.WriteLine("Close");
    }
    public void Assign() {
        sm.Fire(Trigger.Assign);
        Console.WriteLine("Assign");
    }
    public void Defer() {
        sm.Fire(Trigger.Defer);
        Console.WriteLine("Defer");
    }
    public void InProgress() {
        sm.Fire(Trigger.InProgress);
        Console.WriteLine("In Progress");
    }
    public void Review() {
        sm.Fire(Trigger.Review);
        Console.WriteLine("Review");
    }

    public State getState() {
        return sm.State;
    }
}

public class Program {
    public static void Main(string[] args) {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Assign();
        bug.Defer();
        bug.Assign();
        bug.Review();
        bug.InProgress();
        bug.InProgress();
        bug.Defer();
        bug.InProgress();
        bug.Review();
        bug.Close();
        bug.Assign();
        bug.Close();
        Console.WriteLine(bug.getState());
    }
} 
