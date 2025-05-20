using Stateless;

public class Bug {
   public enum State { Open, Assigned, Defered, Closed, InProgress, Blocked, Resolved }
   private enum Trigger { Assign, Defer, Close, Start, Block, Unblock, Resolve }
   private StateMachine<State, Trigger> sm;

   public Bug(State state) {
      sm = new StateMachine<State, Trigger>(state);
      
      sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Block, State.Blocked);

      sm.Configure(State.Assigned)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Defer, State.Defered)
            .Permit(Trigger.Start, State.InProgress)
            .Permit(Trigger.Block, State.Blocked)
            .Ignore(Trigger.Assign);

      sm.Configure(State.InProgress)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Block, State.Blocked)
            .Permit(Trigger.Resolve, State.Resolved)
            .Permit(Trigger.Defer, State.Defered);

      sm.Configure(State.Blocked)
            .Permit(Trigger.Unblock, State.Assigned)
            .Permit(Trigger.Assign, State.Assigned);

      sm.Configure(State.Closed)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Resolve, State.Resolved);

      sm.Configure(State.Defered)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Block, State.Blocked);

      sm.Configure(State.Resolved)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Close, State.Closed);
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

   public void Start() {
      sm.Fire(Trigger.Start);
      Console.WriteLine("Start");
   }

   public void Block() {
      sm.Fire(Trigger.Block);
      Console.WriteLine("Block");
   }

   public void Unblock() {
      sm.Fire(Trigger.Unblock);
      Console.WriteLine("Unblock");
   }

   public void Resolve() {
      sm.Fire(Trigger.Resolve);
      Console.WriteLine("Resolve");
   }
   
   public State getState() {
      return sm.State;
   }
}

public class Program {
   public static void Main(string[] args) {
      var bug = new Bug(Bug.State.Open);
      
      Console.WriteLine($"Initial state: {bug.getState()}");
      
      bug.Assign();
      Console.WriteLine($"After Assign: {bug.getState()}");
      
      bug.Start();
      Console.WriteLine($"After Start: {bug.getState()}");
      
      bug.Block();
      Console.WriteLine($"After Block: {bug.getState()}");
      
      bug.Unblock();
      Console.WriteLine($"After Unblock: {bug.getState()}");
      
      bug.Resolve();
      Console.WriteLine($"After Resolve: {bug.getState()}");
      
      bug.Close();
      Console.WriteLine($"Final state: {bug.getState()}");
   }
}
