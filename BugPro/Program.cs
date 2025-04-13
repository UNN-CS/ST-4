using Stateless;
using System;

namespace BugPro
{
    public class Bug
    {
        public enum State { Open, Assigned, Defered, Closed, Resolved, Reopened }
        private enum Trigger { Assign, Defer, Close, Resolve, Reopen }

        private StateMachine<State, Trigger> sm;

        public Bug(State state)
        {
            sm = new StateMachine<State, Trigger>(state);

            // Состояние Open: разрешен только переход в Assigned
            sm.Configure(State.Open)
                .Permit(Trigger.Assign, State.Assigned);

            // Состояние Assigned
            sm.Configure(State.Assigned)
                .Permit(Trigger.Close, State.Closed)
                .Permit(Trigger.Defer, State.Defered)
                .Permit(Trigger.Resolve, State.Resolved)
                .Ignore(Trigger.Assign);

            // Состояние Defered
            sm.Configure(State.Defered)
                .Permit(Trigger.Assign, State.Assigned)
                .Permit(Trigger.Close, State.Closed);

            // Состояние Closed
            sm.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Reopened)
                .Permit(Trigger.Assign, State.Assigned);

            // Состояние Resolved
            sm.Configure(State.Resolved)
                .Permit(Trigger.Close, State.Closed)
                .Permit(Trigger.Defer, State.Defered);

            // Состояние Reopened
            sm.Configure(State.Reopened)
                .Permit(Trigger.Assign, State.Assigned)
                .Permit(Trigger.Defer, State.Defered);
        }

        public void Close() => sm.Fire(Trigger.Close);
        public void Assign() => sm.Fire(Trigger.Assign);
        public void Defer() => sm.Fire(Trigger.Defer);
        public void Resolve() => sm.Fire(Trigger.Resolve);
        public void Reopen() => sm.Fire(Trigger.Reopen);
        public State GetState() => sm.State;
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            Console.WriteLine(bug.GetState());
        }
    }
}