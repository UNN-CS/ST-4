using System;
using Stateless;

namespace BugPro
{
    public class Bug
    {
        public enum State { Open, Assigned, Defered, Closed, Testing, Reopened }
        private enum Trigger { Assign, Defer, Close, Test, Verify, Reopen }
        private StateMachine<State, Trigger> sm;

        public Bug(State state)
        {
            sm = new StateMachine<State, Trigger>(state);
            ConfigureStateMachine();
        }

        private void ConfigureStateMachine()
        {
            sm.Configure(State.Open)
                .Permit(Trigger.Assign, State.Assigned);

            sm.Configure(State.Assigned)
                .Permit(Trigger.Close, State.Closed)
                .Permit(Trigger.Defer, State.Defered)
                .Permit(Trigger.Test, State.Testing)
                .Ignore(Trigger.Assign);

            sm.Configure(State.Defered)
                .Permit(Trigger.Assign, State.Assigned);

            sm.Configure(State.Closed)
                .Permit(Trigger.Assign, State.Assigned)
                .Permit(Trigger.Reopen, State.Reopened);

            sm.Configure(State.Testing)
                .Permit(Trigger.Verify, State.Closed)
                .Permit(Trigger.Reopen, State.Reopened);

            sm.Configure(State.Reopened)
                .Permit(Trigger.Assign, State.Assigned);
        }

        public void Close() => sm.Fire(Trigger.Close);
        public void Assign() => sm.Fire(Trigger.Assign);
        public void Defer() => sm.Fire(Trigger.Defer);
        public void Test() => sm.Fire(Trigger.Test);
        public void Verify() => sm.Fire(Trigger.Verify);
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
            bug.Assign();
            bug.Defer();
            bug.Assign();
            Console.WriteLine(bug.GetState());
        }
    }
}
