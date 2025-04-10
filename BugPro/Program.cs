using Stateless;

namespace BugPro
{
    public class Bug
    {
        public enum State { Open, Assigned, InProgress, Resolved, Defered, OnHold, Rejected, Closed }
        private enum Trigger { Assign, Start, Resolve, Defer, Close, Reopen, Hold, Reject }
        private StateMachine<State, Trigger> sm;

        public Bug(State state)
        {
            sm = new StateMachine<State, Trigger>(state);

            sm.Configure(State.Open)
                .Permit(Trigger.Assign, State.Assigned)
                .Permit(Trigger.Close, State.Closed)
                .Permit(Trigger.Reject, State.Rejected);

            sm.Configure(State.Assigned)
                .Permit(Trigger.Start, State.InProgress)
                .Permit(Trigger.Defer, State.Defered)
                .Permit(Trigger.Close, State.Closed)
                .Permit(Trigger.Hold, State.OnHold)
                .Ignore(Trigger.Assign);

            sm.Configure(State.InProgress)
                .Permit(Trigger.Resolve, State.Resolved)
                .Permit(Trigger.Defer, State.Defered)
                .Permit(Trigger.Hold, State.OnHold);

            sm.Configure(State.Resolved)
                .Permit(Trigger.Close, State.Closed)
                .Permit(Trigger.Reopen, State.Open)
                .Permit(Trigger.Reject, State.Rejected);

            sm.Configure(State.Defered)
                .Permit(Trigger.Assign, State.Assigned)
                .Permit(Trigger.Close, State.Closed);

            sm.Configure(State.OnHold)
                .Permit(Trigger.Assign, State.Assigned)
                .Permit(Trigger.Reject, State.Rejected);

            sm.Configure(State.Rejected)
                .Permit(Trigger.Reopen, State.Open);

            sm.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Open);
        }

        public void Assign() => sm.Fire(Trigger.Assign);
        public void Start() => sm.Fire(Trigger.Start);
        public void Resolve() => sm.Fire(Trigger.Resolve);
        public void Defer() => sm.Fire(Trigger.Defer);
        public void Close() => sm.Fire(Trigger.Close);
        public void Reopen() => sm.Fire(Trigger.Reopen);
        public void Hold() => sm.Fire(Trigger.Hold);
        public void Reject() => sm.Fire(Trigger.Reject);

        public State GetState() => sm.State;
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var bug = new Bug(Bug.State.Open);
            Console.WriteLine($"Initial state: {bug.GetState()}");

            bug.Assign();    // Open -> Assigned
            bug.Start();     // Assigned -> InProgress
            bug.Hold();      // InProgress -> OnHold
            bug.Assign();    // OnHold -> Assigned
            bug.Start();     // Assigned -> InProgress
            bug.Resolve();   // InProgress -> Resolved
            bug.Close();     // Resolved -> Closed
            bug.Reopen();    // Closed -> Open
            bug.Reject();    // Open -> Rejected

            Console.WriteLine($"Final state: {bug.GetState()}");
        }
    }
}