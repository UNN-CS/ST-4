namespace BugPro;

using Stateless;

public class Bug {
   public enum State {FixCreated, FixAccepted, Open, Assigned, Defered, Closed}
   private enum Trigger {Assign, Defer, Close, AcceptFix, DeclineFix}
   private StateMachine<State, Trigger> sm;

   public Bug(State state) {
      sm = new StateMachine<State, Trigger>(state);
        sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned);
        sm.Configure(State.Assigned)
            .Permit(Trigger.Defer, State.Defered)
            .Permit(Trigger.Close, State.Closed)
            .Ignore(Trigger.Assign);
        sm.Configure(State.FixCreated)
            .Permit(Trigger.AcceptFix, State.FixAccepted);
        sm.Configure(State.FixAccepted)
            .Permit(Trigger.Close, State.Closed);
        sm.Configure(State.Closed)
            .Permit(Trigger.Assign, State.Assigned);
        sm.Configure(State.Defered)
            .Permit(Trigger.Assign, State.Assigned); 
   }
   public void Close() {
      sm.Fire(Trigger.Close);
      Console.WriteLine("Close");
   }
   public void Assign() {
      sm.Fire(Trigger.Assign);
      Console.WriteLine("Assign");   
   }
   public void Defer() {
      sm.Fire(Trigger.Defer);
      Console.WriteLine("Defer");   
   }   
   public void AcceptFix() {
      sm.Fire(Trigger.AcceptFix);
      Console.WriteLine("Accept Fix");
   }

    public void DeclineFix() {
      sm.Fire(Trigger.DeclineFix);
      Console.WriteLine("Decline Fix");
   }
   public State getState() {
      return sm.State;
   }
}

public class Program {
   public static void Main(string[] args) {
      var bug = new Bug(Bug.State.Open);
      bug.Assign();
      bug.Close();
      bug.Assign();
      bug.Defer();
      bug.Assign();
      Console.WriteLine(bug.getState());
   }
}
