namespace BugLib;
using Stateless;

public class Bug {
   public enum State {Open, Assigned, Defered, Closed, UnderReview, Resolved}
   private enum Trigger {Assign, Defer, Close, Review, Resolve}
   private StateMachine<State, Trigger> sm;

   public Bug(State state) {
      sm = new StateMachine<State, Trigger>(state);
      sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned);
			
      sm.Configure(State.Assigned)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Defer, State.Defered)
			.Permit(Trigger.Review, State.UnderReview)
            .Ignore(Trigger.Assign);
			
      sm.Configure(State.Defered)
            .Permit(Trigger.Assign, State.Assigned);
	  sm.Configure(State.Closed)
            .Permit(Trigger.Assign, State.Assigned);	
      sm.Configure(State.UnderReview)
            .Permit(Trigger.Assign, State.Assigned)
			.Permit(Trigger.Resolve, State.Resolved);

			
      sm.Configure(State.Resolved)
            .Permit(Trigger.Close, State.Closed)
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
   public void Review() {
      sm.Fire(Trigger.Review);
      Console.WriteLine("Review");   
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
      bug.Assign();
      bug.Review();
      bug.Resolve();
      bug.Close();
      Console.WriteLine(bug.getState());
   }
}