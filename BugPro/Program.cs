using System;
using Stateless;

public enum State { New, Open, InProgress, Resolved, Closed, Reopened, Rejected }
public enum Trigger { Assign, Start, Resolve, Close, Reopen, Reject }

public class Bug
{
    private readonly StateMachine<State, Trigger> _machine;
    public State CurrentState => _machine.State;

    public Bug()
    {
        _machine = new StateMachine<State, Trigger>(State.New);

        _machine.Configure(State.New)
            .Permit(Trigger.Assign, State.Open)
            .Permit(Trigger.Reject, State.Rejected);

        _machine.Configure(State.Open)
            .Permit(Trigger.Start, State.InProgress)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Reject, State.Rejected);

        _machine.Configure(State.InProgress)
            .Permit(Trigger.Resolve, State.Resolved)
            .Permit(Trigger.Close, State.Closed);

        _machine.Configure(State.Resolved)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);

        _machine.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);

        _machine.Configure(State.Reopened)
            .Permit(Trigger.Assign, State.Open);
            
        _machine.Configure(State.Rejected);
    }

    public void Assign() => _machine.Fire(Trigger.Assign);
    public void Start() => _machine.Fire(Trigger.Start);
    public void Resolve() => _machine.Fire(Trigger.Resolve);
    public void Close() => _machine.Fire(Trigger.Close);
    public void Reopen() => _machine.Fire(Trigger.Reopen);
    public void Reject() => _machine.Fire(Trigger.Reject);
}

class Program
{
    static void Main()
    {
        var bug = new Bug();
        Console.WriteLine($"Initial state: {bug.CurrentState}");
        bug.Assign();
        Console.WriteLine($"After assign: {bug.CurrentState}");
    }
}
