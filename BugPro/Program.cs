using Stateless;
using System;

public class Bug
{
    public enum State { Open, Assigned, Selected, Defered, Closed }
    private enum Trigger { Open, Assign, Select, Defer, Close }
    private StateMachine<State, Trigger> sm;

    public Bug(State state)
    {
        sm = new StateMachine<State, Trigger>(state);
        sm.Configure(State.Open)
              .Permit(Trigger.Assign, State.Assigned)
              .Ignore(Trigger.Open);
        sm.Configure(State.Assigned)
              .Permit(Trigger.Close, State.Closed)
              .Permit(Trigger.Defer, State.Defered)
              .Permit(Trigger.Select, State.Selected)
              .Ignore(Trigger.Assign);
        sm.Configure(State.Selected)
              .Permit(Trigger.Close, State.Closed)
              .Permit(Trigger.Defer, State.Defered)
              .Ignore(Trigger.Select);
        sm.Configure(State.Closed)
              .Permit(Trigger.Assign, State.Assigned)
              .Ignore(Trigger.Close);
        sm.Configure(State.Defered)
              .Permit(Trigger.Assign, State.Assigned)
              .Ignore(Trigger.Defer);
    }
    public void Open()
    {
        sm.Fire(Trigger.Open);
        Console.WriteLine("Open");
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
    public void Select()
    {
        sm.Fire(Trigger.Select);
        Console.WriteLine("Select");
    }
    public void Defer()
    {
        sm.Fire(Trigger.Defer);
        Console.WriteLine("Defer");
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
        bug.Close();
        bug.Assign();
        bug.Defer();
        bug.Assign();
        Console.WriteLine(bug.GetState());
    }
}