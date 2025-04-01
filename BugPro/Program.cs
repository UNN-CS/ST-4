using Stateless;
using System;

public class Bug
{
    // Состояния бага
    public enum State { Open, Assigned, InProgress, Defered, Closed, Reopened, Rejected }

    // Триггеры (события)
    private enum Trigger { Assign, StartWork, Defer, Close, Reopen, Reject }

    private StateMachine<State, Trigger> sm;

    public Bug(State state)
    {
        sm = new StateMachine<State, Trigger>(state);

        // Из Open можно перейти только в Assigned (Reject из Open не разрешён)
        sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned);

        // Из Assigned можно перейти в InProgress, Closed, Defered или Rejected
        sm.Configure(State.Assigned)
            .Permit(Trigger.StartWork, State.InProgress)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Defer, State.Defered)
            .Permit(Trigger.Reject, State.Rejected);

        // Из InProgress можно перейти в Closed или Defered
        sm.Configure(State.InProgress)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Defer, State.Defered);

        // Из Closed можно перейти в Reopened или обратно в Assigned
        sm.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Reopened)
            .Permit(Trigger.Assign, State.Assigned);

        // Из Defered можно перейти в Assigned, Closed или Reopened
        sm.Configure(State.Defered)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);

        // Из Reopened можно перейти в InProgress или Closed
        sm.Configure(State.Reopened)
            .Permit(Trigger.StartWork, State.InProgress)
            .Permit(Trigger.Close, State.Closed);

       
    }

    // Метод для выполнения действия; если переход недопустим, исключение пробрасывается
    private void ExecuteAction(Trigger trigger)
    {
        sm.Fire(trigger);
        Console.WriteLine($"Action '{trigger}' executed successfully. Current state: {sm.State}");
    }

    public void Assign() => ExecuteAction(Trigger.Assign);
    public void StartWork() => ExecuteAction(Trigger.StartWork);
    public void Close() => ExecuteAction(Trigger.Close);
    public void Defer() => ExecuteAction(Trigger.Defer);
    public void Reopen() => ExecuteAction(Trigger.Reopen);
    public void Reject() => ExecuteAction(Trigger.Reject);

    public State getState() => sm.State;
}

public class Program
{
    public static void Main(string[] args)
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();   // Open -> Assigned
        bug.Close();    // Assigned -> Closed
        bug.Assign();   // Closed -> Assigned
        bug.Defer();    // Assigned -> Defered
        bug.Assign();   // Defered -> Assigned
        Console.WriteLine($"Final state: {bug.getState()}");
    }
}
