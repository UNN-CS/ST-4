using Stateless;

public class Bug {
   public enum State {Open, Assigned, Defered, Closed, Verified, Reopened, InProgress, Testing, Fixed, Rejected, OnHold}
   private enum Trigger {Assign, Defer, Close, Verify, Reopen, StartWork, StartTest, Fix, Reject, Hold, Resume}
   private StateMachine<State, Trigger> sm;
   public string Title { get; set; }
   public int Priority { get; set; }
   public string AssignedTo { get; set; }

   public Bug(State state, string title = "Без названия", int priority = 1) {
      Title = title;
      Priority = priority;
      sm = new StateMachine<State, Trigger>(state);
      
      sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Defer, State.Defered)
            .Permit(Trigger.Reject, State.Rejected)
            .Permit(Trigger.Hold, State.OnHold);
            
      sm.Configure(State.Assigned)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Defer, State.Defered)
            .Permit(Trigger.StartWork, State.InProgress)
            .Permit(Trigger.Hold, State.OnHold)
            .Ignore(Trigger.Assign);
            
      sm.Configure(State.Closed)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Verify, State.Verified)
            .Permit(Trigger.Reopen, State.Reopened);
            
      sm.Configure(State.Defered)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Resume, State.Open); 
            
      sm.Configure(State.Verified)
            .Permit(Trigger.Reopen, State.Reopened);
            
      sm.Configure(State.Reopened)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Fix, State.Fixed);
            
      sm.Configure(State.InProgress)
            .Permit(Trigger.Fix, State.Fixed)
            .Permit(Trigger.Defer, State.Defered)
            .Permit(Trigger.Hold, State.OnHold);
            
      sm.Configure(State.Testing)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);
            
      sm.Configure(State.Fixed)
            .Permit(Trigger.StartTest, State.Testing)
            .Permit(Trigger.Reopen, State.Reopened)
            .Permit(Trigger.Close, State.Closed);
            
      sm.Configure(State.Rejected)
            .Permit(Trigger.Reopen, State.Reopened);
            
      sm.Configure(State.OnHold)
            .Permit(Trigger.Resume, State.Open);
   }
   
   public void Close() {
      sm.Fire(Trigger.Close);
      Console.WriteLine("Close");
   }
   
   public void Assign(string assignedTo = "") {
      sm.Fire(Trigger.Assign);
      if (!string.IsNullOrEmpty(assignedTo)) {
          AssignedTo = assignedTo;
      }
      Console.WriteLine("Assign");   
   }
   
   public void Defer() {
      sm.Fire(Trigger.Defer);
      Console.WriteLine("Defer");   
   }
   
   public void Verify() {
      sm.Fire(Trigger.Verify);
      Console.WriteLine("Verify");
   }
   
   public void Reopen() {
      sm.Fire(Trigger.Reopen);
      Console.WriteLine("Reopen");
   }
   
   public void StartWork() {
      sm.Fire(Trigger.StartWork);
      Console.WriteLine("StartWork");
   }
   
   public void StartTest() {
      sm.Fire(Trigger.StartTest);
      Console.WriteLine("StartTest");
   }
   
   public void Fix() {
      sm.Fire(Trigger.Fix);
      Console.WriteLine("Fix");
   }
   
   public void Reject() {
      sm.Fire(Trigger.Reject);
      Console.WriteLine("Reject");
   }
   
   public void Hold() {
      sm.Fire(Trigger.Hold);
      Console.WriteLine("Hold");
   }
   
   public void Resume() {
      sm.Fire(Trigger.Resume);
      Console.WriteLine("Resume");
   }
   
   public State GetState() {
      return sm.State;
   }
   
   public void SetPriority(int priority) {
      Priority = priority;
   }
}

public class Program {
   public static void Main(string[] args) {
      var bug = new Bug(Bug.State.Open, "Ошибка авторизации", 2);
      bug.Assign("Иванов");
      bug.StartWork();
      bug.Fix();
      bug.StartTest();
      bug.Close();
      bug.Verify();
      Console.WriteLine($"Конечное состояние: {bug.GetState()}");
   }
}
