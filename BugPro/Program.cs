using System;
using Stateless;
using Stateless.Graph;

namespace BugPro;

public class Bug
{
    public enum State
    {
        Open,
        Assigned,
        InProgress,
        Resolved,
        Verified,
        Deferred,
        Reopened,
        Closed
    }

    public enum Trigger
    {
        Assign,
        StartWork,
        Resolve,
        Verify,
        Close,
        Defer,
        Reopen
    }

    private readonly StateMachine<State, Trigger> _machine;
    private readonly StateMachine<State, Trigger>.TriggerWithParameters<string> _assignTrigger;

    private readonly string _title;
    private string? _assignee;

    public Bug(string title)
    {
        _title = title ?? throw new ArgumentNullException(nameof(title));

        _machine = new StateMachine<State, Trigger>(State.Open);
        _assignTrigger = _machine.SetTriggerParameters<string>(Trigger.Assign);

        _machine.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned);

        _machine.Configure(State.Assigned)
            .SubstateOf(State.Open)
            .OnEntryFrom(_assignTrigger, OnAssigned)
            .PermitReentry(Trigger.Assign)
            .Permit(Trigger.StartWork, State.InProgress)
            .Permit(Trigger.Defer, State.Deferred)
            .Permit(Trigger.Close, State.Closed)
            .OnExit(OnDeassigned);

        _machine.Configure(State.InProgress)
            .Permit(Trigger.Resolve, State.Resolved)
            .Permit(Trigger.Defer, State.Deferred);

        _machine.Configure(State.Resolved)
            .Permit(Trigger.Verify, State.Verified)
            .Permit(Trigger.Defer, State.Deferred);

        _machine.Configure(State.Verified)
            .Permit(Trigger.Close, State.Closed);

        _machine.Configure(State.Deferred)
            .OnEntry(() => _assignee = null)
            .Permit(Trigger.Assign, State.Assigned);

        _machine.Configure(State.Reopened)
            .Permit(Trigger.Assign, State.Assigned);

        _machine.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);
    }

    public State CurrentState => _machine.State;

    public string? Assignee => _assignee;

    public string Title => _title;

    public void Assign(string assignee) => _machine.Fire(_assignTrigger, assignee);

    public void StartWork() => _machine.Fire(Trigger.StartWork);

    public void Resolve() => _machine.Fire(Trigger.Resolve);

    public void Verify() => _machine.Fire(Trigger.Verify);

    public void Close() => _machine.Fire(Trigger.Close);

    public void Defer() => _machine.Fire(Trigger.Defer);

    public void Reopen() => _machine.Fire(Trigger.Reopen);

    public bool CanAssign => _machine.CanFire(Trigger.Assign);

    public bool CanStartWork => _machine.CanFire(Trigger.StartWork);

    public bool CanResolve => _machine.CanFire(Trigger.Resolve);

    public bool CanVerify => _machine.CanFire(Trigger.Verify);

    public bool CanClose => _machine.CanFire(Trigger.Close);

    public bool CanDefer => _machine.CanFire(Trigger.Defer);

    public bool CanReopen => _machine.CanFire(Trigger.Reopen);

    public string ToDotGraph() => UmlDotGraph.Format(_machine.GetInfo());

    private void OnAssigned(string assignee)
    {
        if (_assignee != null && assignee != _assignee)
            SendEmailToAssignee("Don't forget to help the new employee!");

        _assignee = assignee;
        SendEmailToAssignee("You own it.");
    }

    private void OnDeassigned() => SendEmailToAssignee("You're off the hook.");

    private void SendEmailToAssignee(string message)
    {
        var recipient = _assignee ?? "(unassigned)";
        Console.WriteLine("{0}, RE {1}: {2}", recipient, _title, message);
    }
}

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("=== BugPro: демонстрация workflow бага ===");
        Console.WriteLine();

        var bug = new Bug("Incorrect stock count");

        Console.WriteLine("Начальное состояние: {0}", bug.CurrentState);
        Console.WriteLine();

        bug.Assign("Joe");
        Console.WriteLine("После назначения Joe: {0}, исполнитель: {1}", bug.CurrentState, bug.Assignee);
        Console.WriteLine();

        bug.StartWork();
        Console.WriteLine("После начала работы: {0}", bug.CurrentState);

        bug.Resolve();
        Console.WriteLine("После исправления: {0}", bug.CurrentState);

        bug.Verify();
        Console.WriteLine("После проверки: {0}", bug.CurrentState);

        bug.Close();
        Console.WriteLine("После закрытия: {0}", bug.CurrentState);
        Console.WriteLine();

        Console.WriteLine("--- Второй сценарий: отложенный баг ---");
        var deferred = new Bug("UI glitch on login");
        deferred.Assign("Anna");
        deferred.Defer();
        Console.WriteLine("Отложен: {0}, исполнитель сброшен: {1}", deferred.CurrentState, deferred.Assignee ?? "(null)");
        deferred.Assign("Boris");
        Console.WriteLine("Снова назначен: {0}, исполнитель: {1}", deferred.CurrentState, deferred.Assignee);
        Console.WriteLine();

        Console.WriteLine("Граф автомата (DOT/Uml):");
        Console.WriteLine(bug.ToDotGraph());
    }
}
