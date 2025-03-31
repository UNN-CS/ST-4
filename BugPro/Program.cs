using Stateless;

public class Bug
{
    public enum State { Open, Assigned, Defered, Closed, UnderReview, Fixed, Rejected, Reopened }
    private enum Trigger { Assign, Defer, Close, Review, Fix, Reject, Reopen }
    private StateMachine<State, Trigger> sm;
    public string Title { get; private set; }
    public int Priority { get; private set; }

    public Bug(State state, string title = "Default Bug", int priority = 1)
    {
        Title = title;
        Priority = priority;
        sm = new StateMachine<State, Trigger>(state);

        // Конфигурация Open состояния
        sm.Configure(State.Open)
              .Permit(Trigger.Assign, State.Assigned)
              .Permit(Trigger.Review, State.UnderReview)
              .Permit(Trigger.Reject, State.Rejected);

        // Конфигурация Assigned состояния
        sm.Configure(State.Assigned)
              .Permit(Trigger.Close, State.Closed)
              .Permit(Trigger.Defer, State.Defered)
              .Permit(Trigger.Fix, State.Fixed)
              .Permit(Trigger.Review, State.UnderReview)
              .Ignore(Trigger.Assign);

        // Конфигурация Closed состояния
        sm.Configure(State.Closed)
              .Permit(Trigger.Assign, State.Assigned)
              .Permit(Trigger.Reopen, State.Reopened);

        // Конфигурация Defered состояния
        sm.Configure(State.Defered)
              .Permit(Trigger.Assign, State.Assigned)
              .Permit(Trigger.Reopen, State.Reopened)
              .Permit(Trigger.Close, State.Closed);

        // Конфигурация UnderReview состояния
        sm.Configure(State.UnderReview)
              .Permit(Trigger.Assign, State.Assigned)
              .Permit(Trigger.Fix, State.Fixed)
              .Permit(Trigger.Reject, State.Rejected);

        // Конфигурация Fixed состояния
        sm.Configure(State.Fixed)
              .Permit(Trigger.Close, State.Closed)
              .Permit(Trigger.Reopen, State.Reopened);

        // Конфигурация Rejected состояния
        sm.Configure(State.Rejected)
              .Permit(Trigger.Reopen, State.Reopened)
              .Permit(Trigger.Close, State.Closed);

        // Конфигурация Reopened состояния
        sm.Configure(State.Reopened)
              .Permit(Trigger.Assign, State.Assigned)
              .Permit(Trigger.Review, State.UnderReview);
    }

    public void Close()
    {
        sm.Fire(Trigger.Close);
        Console.WriteLine("Close");
    }

    public void Assign()
    {
        sm.Fire(Trigger.Assign);
        Console.WriteLine("Assign");
    }

    public void Defer()
    {
        sm.Fire(Trigger.Defer);
        Console.WriteLine("Defer");
    }

    public void Review()
    {
        sm.Fire(Trigger.Review);
        Console.WriteLine("Review");
    }

    public void Fix()
    {
        sm.Fire(Trigger.Fix);
        Console.WriteLine("Fix");
    }

    public void Reject()
    {
        sm.Fire(Trigger.Reject);
        Console.WriteLine("Reject");
    }

    public void Reopen()
    {
        sm.Fire(Trigger.Reopen);
        Console.WriteLine("Reopen");
    }

    public void SetPriority(int priority)
    {
        if (priority < 1 || priority > 5)
            throw new ArgumentOutOfRangeException("Priority must be between 1 and 5");
        Priority = priority;
    }

    public State GetState()
    {
        return sm.State;
    }

    public bool CanClose()
    {
        return sm.CanFire(Trigger.Close);
    }

    public bool CanAssign()
    {
        return sm.CanFire(Trigger.Assign);
    }

    public bool CanDefer()
    {
        return sm.CanFire(Trigger.Defer);
    }

    public bool CanReview()
    {
        return sm.CanFire(Trigger.Review);
    }

    public bool CanFix()
    {
        return sm.CanFire(Trigger.Fix);
    }

    public bool CanReject()
    {
        return sm.CanFire(Trigger.Reject);
    }

    public bool CanReopen()
    {
        return sm.CanFire(Trigger.Reopen);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var bug = new Bug(Bug.State.Open, "Critical login issue", 1);
        Console.WriteLine($"Bug created: {bug.Title}, Priority: {bug.Priority}, State: {bug.GetState()}");

        bug.Assign();
        Console.WriteLine($"Current state: {bug.GetState()}");

        bug.Fix();
        Console.WriteLine($"Current state: {bug.GetState()}");

        bug.Close();
        Console.WriteLine($"Current state: {bug.GetState()}");

        bug.Reopen();
        Console.WriteLine($"Current state: {bug.GetState()}");

        bug.Assign();
        Console.WriteLine($"Current state: {bug.GetState()}");

        bug.Review();
        Console.WriteLine($"Current state: {bug.GetState()}");

        bug.Fix();
        Console.WriteLine($"Current state: {bug.GetState()}");

        bug.Close();
        Console.WriteLine($"Final state: {bug.GetState()}");
    }
}