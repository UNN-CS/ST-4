using Stateless;

public class Bug
{
    public enum State { Open, Assigned, Defered, Closed, InProgress, Resolved }
    private enum Trigger { Assign, Defer, Close, StartProgress, Resolve }
    private StateMachine<State, Trigger> sm;

    public Bug(State state)
    {
        sm = new StateMachine<State, Trigger>(state);
        sm.Configure(State.Open)
              .Permit(Trigger.Assign, State.Assigned);
        sm.Configure(State.Assigned)
              .Permit(Trigger.Close, State.Closed)
              .Permit(Trigger.Defer, State.Defered)
              .Permit(Trigger.StartProgress, State.InProgress)
              .Ignore(Trigger.Assign);
        sm.Configure(State.Closed)
              .Permit(Trigger.Assign, State.Assigned);
        sm.Configure(State.Defered)
              .Permit(Trigger.Assign, State.Assigned);
        sm.Configure(State.InProgress)
              .Permit(Trigger.Resolve, State.Resolved);
        sm.Configure(State.Resolved)
              .Ignore(Trigger.Assign)
              .Ignore(Trigger.Defer)
              .Ignore(Trigger.Close)
              .Ignore(Trigger.StartProgress)
              .Ignore(Trigger.Resolve);
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

    public void StartProgress()
    {
        sm.Fire(Trigger.StartProgress);
        Console.WriteLine("StartProgress");
    }

    public void Resolve()
    {
        sm.Fire(Trigger.Resolve);
        Console.WriteLine("Resolve");
    }

    public State GetState()
    {
        return sm.State;
    }
}
public class Program
{
    public static void Main(string[] args)
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.StartProgress();
        bug.Resolve();
        bug.Close();
        Console.WriteLine(bug.GetState());
    }
}