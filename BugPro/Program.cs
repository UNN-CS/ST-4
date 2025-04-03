using Stateless;
using System;

public class Bug
{
    public enum State { Open, Assigned, Defered, Resolved, Verified, Closed, Reopened, Testing, Rejected }
    private enum Trigger { Assign, Defer, Resolve, Verify, Close, Reopen, Test, Reject }

    private readonly StateMachine<State, Trigger> sm;

    public Bug(State state)
    {
        sm = new StateMachine<State, Trigger>(state);

        // Конфигурация переходов
        sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Reopen, State.Reopened);

        sm.Configure(State.Assigned)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Defer, State.Defered)
            .Permit(Trigger.Resolve, State.Resolved)
            .Ignore(Trigger.Assign);

        sm.Configure(State.Defered)
            .Permit(Trigger.Assign, State.Assigned);

        sm.Configure(State.Resolved)
            .Permit(Trigger.Verify, State.Verified)
            .Permit(Trigger.Reopen, State.Reopened)
            .Permit(Trigger.Test, State.Testing);

        sm.Configure(State.Verified)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);

        sm.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);

        sm.Configure(State.Reopened)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Close, State.Closed);

        sm.Configure(State.Testing)
            .Permit(Trigger.Verify, State.Verified)
            .Permit(Trigger.Reject, State.Rejected);

        sm.Configure(State.Rejected)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Reopen, State.Reopened);
    }

    public void Assign() => Execute(Trigger.Assign, "Assign");
    public void Defer() => Execute(Trigger.Defer, "Defer");
    public void Resolve() => Execute(Trigger.Resolve, "Resolve");
    public void Verify() => Execute(Trigger.Verify, "Verify");
    public void Close() => Execute(Trigger.Close, "Close");
    public void Reopen() => Execute(Trigger.Reopen, "Reopen");
    public void Test() => Execute(Trigger.Test, "Test");
    public void Reject() => Execute(Trigger.Reject, "Reject");

    private void Execute(Trigger trigger, string action)
    {
        if (sm.CanFire(trigger))
        {
            sm.Fire(trigger);
            Console.WriteLine($"{action} executed. New state: {sm.State}");
        }
        else
        {
            throw new InvalidOperationException(
                $"Invalid action '{action}' for current state '{sm.State}'");
        }
    }

    public State GetState() => sm.State;
}

public class Program
{
    public static void Main(string[] args)
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Resolve();
        bug.Test();
        bug.Reject();
        bug.Assign();
        bug.Resolve();
        bug.Verify();
        bug.Close();
        Console.WriteLine($"Final state: {bug.GetState()}");
    }
}