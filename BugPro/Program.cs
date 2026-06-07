// Copyright 2026 NNTU-CS
using Stateless;
using System;

namespace BugPro {
  public class Bug {
    public enum State {
      Open, Assigned, InProgress, Resolved, Closed, Reopened
    }

    public enum Trigger {
      Assign, Start, Resolve, Close, Reopen, Reset
    }

    private readonly StateMachine<State, Trigger> _machine;
    private string _title;

    public State CurrentState => _machine.State;

    public Bug(string title) {
      _title = title;
      _machine = new StateMachine<State, Trigger>(State.Open);

      _machine.Configure(State.Open)
        .Permit(Trigger.Assign, State.Assigned);

      _machine.Configure(State.Assigned)
        .Permit(Trigger.Start, State.InProgress)
        .Permit(Trigger.Reset, State.Open);

      _machine.Configure(State.InProgress)
        .Permit(Trigger.Resolve, State.Resolved)
        .Permit(Trigger.Reset, State.Open);

      _machine.Configure(State.Resolved)
        .Permit(Trigger.Close, State.Closed)
        .Permit(Trigger.Reopen, State.Reopened);

      _machine.Configure(State.Closed)
        .Permit(Trigger.Reopen, State.Reopened);

      _machine.Configure(State.Reopened)
        .Permit(Trigger.Assign, State.Assigned)
        .Permit(Trigger.Resolve, State.Resolved);
    }

    public void Assign() => _machine.Fire(Trigger.Assign);
    public void Start() => _machine.Fire(Trigger.Start);
    public void Resolve() => _machine.Fire(Trigger.Resolve);
    public void Close() => _machine.Fire(Trigger.Close);
    public void Reopen() => _machine.Fire(Trigger.Reopen);
    public void Reset() => _machine.Fire(Trigger.Reset);

    public bool CanAssign() => _machine.CanFire(Trigger.Assign);
    public bool CanStart() => _machine.CanFire(Trigger.Start);
    public bool CanResolve() => _machine.CanFire(Trigger.Resolve);
    public bool CanClose() => _machine.CanFire(Trigger.Close);
    public bool CanReopen() => _machine.CanFire(Trigger.Reopen);
  }

  class Program {
    static void Main(string[] args) {
      var bug = new Bug("Sample Bug");
      Console.WriteLine($"Initial state: {bug.CurrentState}");

      bug.Assign();
      Console.WriteLine($"After Assign: {bug.CurrentState}");

      bug.Start();
      Console.WriteLine($"After Start: {bug.CurrentState}");

      bug.Resolve();
      Console.WriteLine($"After Resolve: {bug.CurrentState}");

      bug.Close();
      Console.WriteLine($"After Close: {bug.CurrentState}");

      bug.Reopen();
      Console.WriteLine($"After Reopen: {bug.CurrentState}");
    }
  }
}
