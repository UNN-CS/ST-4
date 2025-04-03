using Stateless;
using System;

public class Bug {
    public enum State {
        Open,
        Assigned,
        Defered,
        Closed,
        Verified,
        Rejected,
        Canceled
    }

    private enum Trigger {
        Assign,
        Defer,
        Close,
        Verify,
        Reject,
        Cancel
    }

    private StateMachine<State, Trigger> sm;

    public Bug(State state) {
        sm = new StateMachine<State, Trigger>(state);

        sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Cancel, State.Canceled);

        sm.Configure(State.Assigned)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Defer, State.Defered)
            .Permit(Trigger.Verify, State.Verified)
            .Permit(Trigger.Reject, State.Rejected)
            .Ignore(Trigger.Assign);

        sm.Configure(State.Closed)
            .Permit(Trigger.Assign, State.Assigned);

        sm.Configure(State.Defered)
            .Permit(Trigger.Assign, State.Assigned);

        sm.Configure(State.Verified)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Assign, State.Assigned);

        sm.Configure(State.Rejected)
            .Permit(Trigger.Assign, State.Assigned);
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

    public void Verify() {
        sm.Fire(Trigger.Verify);
        Console.WriteLine("Verify");
    }

    public void Reject() {
        sm.Fire(Trigger.Reject);
        Console.WriteLine("Reject");
    }

    public void Cancel() {
        sm.Fire(Trigger.Cancel);
        Console.WriteLine("Cancel");
    }

    public State getState() {
        return sm.State;
    }
}

public class Program {
    public static void Main(string[] args) {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Verify();
        bug.Close();
        bug.Assign();
        bug.Defer();
        bug.Assign();
        bug.Reject();
        bug.Assign();
        Console.WriteLine(bug.getState());
    }
}
