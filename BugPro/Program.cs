using Stateless;

public class Bug {
   public enum State {Open, Assigned, Defered, Resolved, Closed, Reopened}
   private enum Trigger {Assign, Defer, Close, Resolve, Reopen}
   private StateMachine<State, Trigger> sm;

   public Bug(State state) {
        sm = new StateMachine<State, Trigger>(state);

        sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned);

        sm.Configure(State.Assigned)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Defer, State.Deferred)
            .Permit(Trigger.Resolve, State.Resolved)
            .Ignore(Trigger.Assign);

        sm.Configure(State.Resolved)
            .Permit(Trigger.Close, State.Closed);

        sm.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);

        sm.Configure(State.Reopened)
            .Permit(Trigger.Assign, State.Assigned);

        sm.Configure(State.Deferred)
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

   public void Resolve() {
      sm.Fire(Trigger.Resolve);
      Console.WriteLine("Resolve");
   }

   public void Reopen() {
      sm.Fire(Trigger.Reopen);
      Console.WriteLine("Reopen");
    }   

   public State getState() {
      return sm.State;
   }
}

public class Program {
   public static void Main(string[] args) {
      var bug = new Bug(Bug.State.Open);
      bug.Assign();
      bug.Resolve();
      bug.Close();
      bug.Reopen();
      bug.Assign();
      Console.WriteLine(bug.getState());
   }
}