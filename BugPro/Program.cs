using Stateless;

public class Bug {
  public enum State {Open, NotABug, Duplicate, Assigned, Defered, Fixed, Verified, Closed}
  private enum Trigger {MarkNotABug, MarkDuplicate, Assign, Defer, Fix, Verify, Close}
  private StateMachine<State, Trigger> sm;

  public Bug(State state) {
    sm = new StateMachine<State, Trigger>(state);
    sm.Configure(State.Open)
      .Permit(Trigger.MarkNotABug, State.NotABug)
      .Permit(Trigger.MarkDuplicate, State.Duplicate)
      .Permit(Trigger.Assign, State.Assigned)
      .Permit(Trigger.Defer, State.Defered);
    sm.Configure(State.NotABug)
      .Permit(Trigger.Close, State.Closed);
    sm.Configure(State.Duplicate)
      .Permit(Trigger.Close, State.Closed);
    sm.Configure(State.Assigned)
      .Permit(Trigger.MarkNotABug, State.NotABug)
      .Permit(Trigger.MarkDuplicate, State.Duplicate)
      .Ignore(Trigger.Assign)
      .Permit(Trigger.Defer, State.Defered)
      .Permit(Trigger.Fix, State.Fixed);
    sm.Configure(State.Defered)
      .Permit(Trigger.Assign, State.Assigned);
    sm.Configure(State.Fixed)
      .Permit(Trigger.Verify, State.Verified);
    sm.Configure(State.Verified)
      .Permit(Trigger.Close, State.Closed);
  }
  public void MarkNotABug() {
    sm.Fire(Trigger.MarkNotABug);
    Console.WriteLine("MarkNotABug");
  }
  public void MarkDuplicate() {
    sm.Fire(Trigger.MarkDuplicate);
    Console.WriteLine("MarkDuplicate");
  }
  public void Assign() {
    sm.Fire(Trigger.Assign);
    Console.WriteLine("Assign");   
  }
  public void Defer() {
    sm.Fire(Trigger.Defer);
    Console.WriteLine("Defer");   
  }
  public void Fix() {
    sm.Fire(Trigger.Fix);
    Console.WriteLine("Fix");
  }
  public void Verify() {
    sm.Fire(Trigger.Verify);
    Console.WriteLine("Verify");
  }
  public void Close() {
    sm.Fire(Trigger.Close);
    Console.WriteLine("Close");
  }
  public State getState() {
    return sm.State;
  }
}

public class Program {
  public static void Main(string[] args) {
    var bug = new Bug(Bug.State.Open);
    bug.Assign();
    bug.Defer();
    bug.Assign();
    bug.Fix();
    bug.Verify();
    bug.Close();
  }
}
