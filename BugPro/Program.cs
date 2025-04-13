using System;
using Stateless;

public class Bug
{
    public enum State
    {
        Open, Assigned, InProgress, NeedsReview,
        Resolved, Verified, Closed, Reopened,
        Deferred, Rejected, Duplicate, WontFix
    }

    private enum Trigger
    {
        Assign, StartWork, RequestReview, Approve,
        Reject, Resolve, Verify, Close, Reopen,
        Defer, MarkAsDuplicate, MarkAsWontFix
    }

    private readonly StateMachine<State, Trigger> sm;
    private readonly StateMachine<State, Trigger>.TriggerWithParameters<string> assignTrigger;

    public Bug(State state)
    {
        sm = new StateMachine<State, Trigger>(state);
        assignTrigger = sm.SetTriggerParameters<string>(Trigger.Assign);
        ConfigureStateMachine();
    }

    private void ConfigureStateMachine()
    {
        sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.MarkAsDuplicate, State.Duplicate)
            .Permit(Trigger.MarkAsWontFix, State.WontFix);

        sm.Configure(State.Assigned)
            .Permit(Trigger.StartWork, State.InProgress)
            .Permit(Trigger.Defer, State.Deferred)
            .Permit(Trigger.MarkAsDuplicate, State.Duplicate)
            .Permit(Trigger.MarkAsWontFix, State.WontFix);

        sm.Configure(State.InProgress)
            .Permit(Trigger.MarkAsWontFix, State.WontFix)
            .Permit(Trigger.RequestReview, State.NeedsReview)
            .Permit(Trigger.Defer, State.Deferred);

        sm.Configure(State.NeedsReview)
            .Permit(Trigger.Approve, State.Resolved)
            .Permit(Trigger.Reject, State.InProgress);

        sm.Configure(State.Resolved)
            .Permit(Trigger.Verify, State.Verified)
            .Permit(Trigger.Reopen, State.Reopened);

        sm.Configure(State.Verified)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Reopen, State.Reopened)
            .Ignore(Trigger.Verify);

        sm.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);

        sm.Configure(State.Reopened)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Defer, State.Deferred);

        sm.Configure(State.Deferred)
            .Permit(Trigger.Assign, State.Assigned);

        sm.Configure(State.Duplicate)
            .Permit(Trigger.Reopen, State.Reopened)
            .Permit(Trigger.Close, State.Closed);

        sm.Configure(State.WontFix)
            .Permit(Trigger.Reopen, State.Reopened)
            .Permit(Trigger.Close, State.Closed);

        sm.OnTransitioned(t =>
            Console.WriteLine($"Transition: {t.Source} -> {t.Destination}"));
    }

    public void Assign(string assignee) => sm.Fire(assignTrigger, assignee);
    public void StartWork() => sm.Fire(Trigger.StartWork);
    public void RequestReview() => sm.Fire(Trigger.RequestReview);
    public void Approve() => sm.Fire(Trigger.Approve);
    public void Reject() => sm.Fire(Trigger.Reject);
    public void Resolve() => sm.Fire(Trigger.Resolve);
    public void Verify() => sm.Fire(Trigger.Verify);
    public void Close() => sm.Fire(Trigger.Close);
    public void Reopen() => sm.Fire(Trigger.Reopen);
    public void Defer() => sm.Fire(Trigger.Defer);
    public void MarkAsDuplicate() => sm.Fire(Trigger.MarkAsDuplicate);
    public void MarkAsWontFix() => sm.Fire(Trigger.MarkAsWontFix);

    public State CurrentState => sm.State;
    public bool CanAssign => sm.CanFire(Trigger.Assign);
    public bool CanStartWork => sm.CanFire(Trigger.StartWork);
    public bool CanRequestReview => sm.CanFire(Trigger.RequestReview);
    public bool CanApprove => sm.CanFire(Trigger.Approve);
    public bool CanReject => sm.CanFire(Trigger.Reject);
    public bool CanResolve => sm.CanFire(Trigger.Resolve);
    public bool CanVerify => sm.CanFire(Trigger.Verify);
    public bool CanClose => sm.CanFire(Trigger.Close);
    public bool CanReopen => sm.CanFire(Trigger.Reopen);
    public bool CanDefer => sm.CanFire(Trigger.Defer);
    public bool CanMarkAsDuplicate => sm.CanFire(Trigger.MarkAsDuplicate);
    public bool CanMarkAsWontFix => sm.CanFire(Trigger.MarkAsWontFix);
}

public class Program
{
    public static void Main(string[] args)
    {
        var bug1 = new Bug(Bug.State.Open);
        Console.WriteLine($"Bug1 initial state: {bug1.CurrentState}");

        SafeExecute(() => bug1.Assign("dev1@company.com"));
        SafeExecute(bug1.StartWork);
        SafeExecute(bug1.RequestReview);
        SafeExecute(bug1.Approve);
        SafeExecute(bug1.Resolve);
        SafeExecute(bug1.Verify);
        SafeExecute(bug1.Close);

        Console.WriteLine($"Bug1 final state: {bug1.CurrentState}\n");

        var bug2 = new Bug(Bug.State.Open);
        Console.WriteLine($"Bug2 initial state: {bug2.CurrentState}");

        SafeExecute(() => bug2.Assign("dev2@company.com"));
        SafeExecute(bug2.StartWork);
        SafeExecute(bug2.RequestReview);
        SafeExecute(bug2.Reject);
        SafeExecute(bug2.RequestReview);
        SafeExecute(bug2.Approve);
        SafeExecute(bug2.Resolve);
        SafeExecute(bug2.Verify);
        SafeExecute(bug2.Close);

        Console.WriteLine($"Bug2 final state: {bug2.CurrentState}\n");

        var bug3 = new Bug(Bug.State.Closed);
        Console.WriteLine($"Bug3 initial state: {bug3.CurrentState}");

        SafeExecute(bug3.StartWork);

        Console.WriteLine($"Bug3 final state: {bug3.CurrentState}\n");

        var bug4 = new Bug(Bug.State.Open);
        Console.WriteLine($"Bug4 initial state: {bug4.CurrentState}");

        SafeExecute(bug4.MarkAsDuplicate);
        SafeExecute(bug4.Reopen);
        SafeExecute(() => bug4.Assign("dev4@company.com"));

        Console.WriteLine($"Bug4 final state: {bug4.CurrentState}");
    }

    private static void SafeExecute(Action action)
    {
        try
        {
            action();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private static void SafeExecute(Action<string> action, string arg)
    {
        try
        {
            action(arg);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}