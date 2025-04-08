using Stateless;

public class Bug {
    public enum State { Open, Assigned, WIP, Defered, Reviewed, Closed }
    private enum Trigger { Assign, WIP, Defer, Review, Close }
    private StateMachine<State, Trigger> sm;

    public Bug(State state) {
        sm = new StateMachine<State, Trigger>(state);
        sm.Configure(State.Open)
              .Permit(Trigger.Assign, State.Assigned);
        sm.Configure(State.Assigned)
              .Permit(Trigger.Close, State.Closed)
              .Permit(Trigger.Defer, State.Defered)
              .Permit(Trigger.WIP, State.WIP)
              .Ignore(Trigger.Assign);
        sm.Configure(State.Closed)
              .Permit(Trigger.Assign, State.Assigned);
        sm.Configure(State.Defered)
              .Permit(Trigger.Assign, State.Assigned)
              .Permit(Trigger.WIP, State.WIP);
        sm.Configure(State.WIP)
            .Permit(Trigger.Defer, State.Defered)
            .Permit(Trigger.Review, State.Reviewed);
        sm.Configure(State.Reviewed)
            .Ignore(Trigger.Review)
            .Permit(Trigger.WIP, State.WIP)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Close, State.Closed);
    }

    // public void Open() {}

    public void Assign() {
        sm.Fire(Trigger.Assign);
        Console.WriteLine("Assign");
    }
    public void Defer() {
        sm.Fire(Trigger.Defer);
        Console.WriteLine("Defer");
    }
    public void WorkInProgress() {
        sm.Fire(Trigger.WIP);
        Console.WriteLine("Work In Progress");
    }
    public void Review() {
        sm.Fire(Trigger.Review);
        Console.WriteLine("Review");
    }
    public void Close() {
        sm.Fire(Trigger.Close);
        Console.WriteLine("Close");
    }

    public State getState() {
        return sm.State;
    }
}

public class Program {
    public static void Main(string[] args) {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Assign();
        bug.Assign();
        bug.WorkInProgress();
        bug.Defer();
        bug.WorkInProgress();
        bug.Review();
        bug.WorkInProgress();
        bug.Review();
        bug.Close();
        bug.Assign();
        bug.WorkInProgress();
        bug.Review();
        bug.Close();
        Console.WriteLine(bug.getState());
    }
}