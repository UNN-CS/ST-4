using System;
using Stateless;

namespace BugPro
{
    public class Bug
    {
        public enum State
        {
            Open,
            Assigned,
            Deferred,
            Closed,
            Reopened,
            Resolved,
            InProgress,
            Verified,
            Rejected
        }

        private enum Trigger
        {
            Assign,
            Defer,
            Close,
            Reopen,
            Resolve,
            StartWork,
            Verify,
            Reject
        }

        private StateMachine<State, Trigger> sm;

        public Bug(State initialState)
        {
            sm = new StateMachine<State, Trigger>(initialState);

            sm.Configure(State.Open)
                .Permit(Trigger.Assign, State.Assigned)
                .Permit(Trigger.Close, State.Closed)
                .Permit(Trigger.Reject, State.Rejected);

            sm.Configure(State.Assigned)
                .Permit(Trigger.StartWork, State.InProgress)
                .Permit(Trigger.Close, State.Closed)
                .Permit(Trigger.Defer, State.Deferred)
                .Permit(Trigger.Reject, State.Rejected)
                .Ignore(Trigger.Assign);

            sm.Configure(State.InProgress)
                .Permit(Trigger.Resolve, State.Resolved)
                .Permit(Trigger.Reject, State.Rejected);

            sm.Configure(State.Resolved)
                .Permit(Trigger.Verify, State.Verified)
                .Permit(Trigger.Reject, State.Rejected);

            sm.Configure(State.Verified)
                .Permit(Trigger.Close, State.Closed)
                .Permit(Trigger.Reopen, State.Reopened)
                .Permit(Trigger.Reject, State.Rejected);

            sm.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Reopened);

            sm.Configure(State.Deferred)
                .Permit(Trigger.Assign, State.Assigned);

            sm.Configure(State.Reopened)
                .Permit(Trigger.Assign, State.Assigned)
                .Permit(Trigger.Resolve, State.Resolved);

            sm.Configure(State.Rejected)
                .Permit(Trigger.Reopen, State.Reopened);
        }

        public void Assign() { sm.Fire(Trigger.Assign); Console.WriteLine("Assign"); }
        public void Defer() { sm.Fire(Trigger.Defer); Console.WriteLine("Defer"); }
        public void Close() { sm.Fire(Trigger.Close); Console.WriteLine("Close"); }
        public void Reopen() { sm.Fire(Trigger.Reopen); Console.WriteLine("Reopen"); }
        public void Resolve() { sm.Fire(Trigger.Resolve); Console.WriteLine("Resolve"); }
        public void StartWork() { sm.Fire(Trigger.StartWork); Console.WriteLine("StartWork"); }
        public void Verify() { sm.Fire(Trigger.Verify); Console.WriteLine("Verify"); }
        public void Reject() { sm.Fire(Trigger.Reject); Console.WriteLine("Reject"); }

        public State GetState() => sm.State;
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var bug = new Bug(Bug.State.Open);

            bug.Assign();       // Open -> Assigned
            bug.StartWork();    // Assigned -> InProgress
            bug.Resolve();      // InProgress -> Resolved
            bug.Verify();       // Resolved -> Verified
            bug.Close();        // Verified -> Closed

            Console.WriteLine($"Final state: {bug.GetState()}");
        }
    }
}
