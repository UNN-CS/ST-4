using System;
using Stateless;
public class Bug
{
    public enum State { Open, SetUpEnviroment, Assigned, InProgress, Syncronizing, Defered, RollUpEnviroment, Closed }
    private enum Trigger { SetUpEnv, Assign, InProg, Defer, Sync, RollUpEnv, Close }
    private StateMachine<State, Trigger> sm;
    private bool eniromentSetUped = false;
    private bool enviromentRollUped = false;

    public Bug(State state)
    {
        sm = new StateMachine<State, Trigger>(state);
        sm.Configure(State.Open)
              .Permit(Trigger.SetUpEnv, State.SetUpEnviroment);
        sm.Configure(State.SetUpEnviroment)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.RollUpEnv, State.RollUpEnviroment)
            .Permit(Trigger.Sync, State.Syncronizing)
            .Ignore(Trigger.SetUpEnv);
        sm.Configure(State.Assigned)
              .Permit(Trigger.Close, State.Closed)
              .Permit(Trigger.Defer, State.Defered)
              .Permit(Trigger.Sync, State.Syncronizing)
              .Permit(Trigger.InProg, State.InProgress)
              .Permit(Trigger.RollUpEnv, State.RollUpEnviroment)
              .Ignore(Trigger.Assign);
        sm.Configure(State.InProgress)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.RollUpEnv, State.RollUpEnviroment)
            .Permit(Trigger.Sync, State.Syncronizing)
            .Permit(Trigger.Defer, State.Defered)
            .Ignore(Trigger.InProg);
        sm.Configure(State.Syncronizing)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Defer, State.Defered)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.RollUpEnv, State.RollUpEnviroment)
            .Ignore(Trigger.Sync);
        sm.Configure(State.RollUpEnviroment)
            .Permit(Trigger.Close, State.Closed)
            .Ignore(Trigger.RollUpEnv);
        sm.Configure(State.Closed)
              .Permit(Trigger.Assign, State.Assigned);
        sm.Configure(State.Defered)
              .Permit(Trigger.Assign, State.Assigned);
    }
    private void CheckEnviromentSetUpedStatus()
    {
        if (!eniromentSetUped)
        {
            throw new Exception("Enviroment isn't set uped!");
        }
    }
    private void CheckEnviromentRollUpedStatus() { 
        if (!enviromentRollUped)
        {
            throw new Exception("Enviroment isn't roll uped!");
        }
    }
    public void Close()
    {
        CheckEnviromentRollUpedStatus();
        sm.Fire(Trigger.Close);
        eniromentSetUped = false;
        enviromentRollUped = false;
        Console.WriteLine("Close");
    }
    public void Syncronizing()
    {
        CheckEnviromentSetUpedStatus();
        sm.Fire(Trigger.Sync);
        Console.WriteLine("Syncronized");
    }
    public void Assign()
    {
        CheckEnviromentSetUpedStatus();
        sm.Fire(Trigger.Assign);
        Console.WriteLine("Assign");
    }
    public void SetUpEnviroment()
    {
        if (!eniromentSetUped)
        {
            eniromentSetUped = true;
        }
        sm.Fire(Trigger.SetUpEnv);
        Console.WriteLine("SetUpEnviroment");
    }
    public void InProgress()
    {
        CheckEnviromentSetUpedStatus();
        sm.Fire(Trigger.InProg);
        Console.WriteLine("InProg");
        if (!enviromentRollUped)
        {
            enviromentRollUped = true;
        }
    }
    public void Defer()
    {
        CheckEnviromentSetUpedStatus();
        sm.Fire(Trigger.Defer);
        Console.WriteLine("Defer");
    }
    public void RollUpEnviroment()
    {
        if (eniromentSetUped)
        {
            enviromentRollUped = true;
            sm.Fire(Trigger.RollUpEnv);
            Console.WriteLine("RollUpEnviroment");
        }
    }
    public State getState()
    {
        return sm.State;
    }
    public bool isEnviromentSetUped()
    {
        return eniromentSetUped;

    }
    public bool isEnviromentRollUped()
    {
        return enviromentRollUped;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.SetUpEnviroment();
        bug.RollUpEnviroment();
        bug.Close();
        bug.Assign();
        bug.Syncronizing();
        bug.InProgress();
        bug.Defer();
        bug.Assign();
        Console.WriteLine(bug.getState());
    }
}