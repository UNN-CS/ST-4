using Stateless;

namespace BugPro
{
    public class Bug
    {
        public enum State { Open, Assigned, InProgress, Deferred, Resolved, Closed, Reopened }
        private enum Trigger { Assign, Start, Defer, Resolve, Close, Reopen }
        private StateMachine<State, Trigger> sm;

        public Bug(State state)
        {
            sm = new StateMachine<State, Trigger>(state);
            sm.Configure(State.Open)
                .Permit(Trigger.Assign, State.Assigned);
            sm.Configure(State.Assigned)
                .Permit(Trigger.Start, State.InProgress)
                .Permit(Trigger.Defer, State.Deferred)
                .Permit(Trigger.Close, State.Closed)
                .Ignore(Trigger.Assign);
            sm.Configure(State.InProgress)
                .Permit(Trigger.Resolve, State.Resolved)
                .Permit(Trigger.Defer, State.Deferred);
            sm.Configure(State.Deferred)
                .Permit(Trigger.Assign, State.Assigned);
            sm.Configure(State.Resolved)
                .Permit(Trigger.Close, State.Closed)
                .Permit(Trigger.Assign, State.Assigned);
            sm.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Reopened)
                .Permit(Trigger.Assign, State.Assigned);
            sm.Configure(State.Reopened)
                .Permit(Trigger.Assign, State.Assigned);
        }

        public void Assign()
        {
            sm.Fire(Trigger.Assign);
            Console.WriteLine("Assign");
        }

        public void Start()
        {
            sm.Fire(Trigger.Start);
            Console.WriteLine("Start");
        }

        public void Defer()
        {
            sm.Fire(Trigger.Defer);
            Console.WriteLine("Defer");
        }

        public void Resolve()
        {
            sm.Fire(Trigger.Resolve);
            Console.WriteLine("Resolve");
        }

        public void Close()
        {
            sm.Fire(Trigger.Close);
            Console.WriteLine("Close");
        }

        public void Reopen()
        {
            sm.Fire(Trigger.Reopen);
            Console.WriteLine("Reopen");
        }

        public State GetState()
        {
            return sm.State;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Start();
            bug.Resolve();
            bug.Close();
            bug.Reopen();
            bug.Assign();
            Console.WriteLine($"Final state: {bug.GetState()}");
        }
    }
}