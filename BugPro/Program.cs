using System;
using Stateless;

namespace BugPro
{
    public class Bug
    {
        public enum State { Open, Assigned, InReview, Defered, Closed, Reopened }
        private enum Trigger { Assign, Defer, Close, Review, Reopen }

        private StateMachine<State, Trigger> sm;

        public Bug(State state)
        {
            sm = new StateMachine<State, Trigger>(state);

            sm.Configure(State.Open)
                .Permit(Trigger.Assign, State.Assigned);

            sm.Configure(State.Assigned)
                .Permit(Trigger.Close, State.Closed)
                .Permit(Trigger.Defer, State.Defered)
                .Permit(Trigger.Review, State.InReview)
                .Ignore(Trigger.Assign);

            sm.Configure(State.InReview)
                .Permit(Trigger.Close, State.Closed)
                .Permit(Trigger.Reopen, State.Reopened);

            sm.Configure(State.Closed)
                .Permit(Trigger.Assign, State.Assigned)
                .Permit(Trigger.Reopen, State.Reopened);

            sm.Configure(State.Defered)
                .Permit(Trigger.Assign, State.Assigned);

            sm.Configure(State.Reopened)
                .Permit(Trigger.Assign, State.Assigned);
        }

        public void Close()
        {
            sm.Fire(Trigger.Close);
            Console.WriteLine("Close");
        }

        public void Assign()
        {
            sm.Fire(Trigger.Assign);
            Console.WriteLine("Assign");
        }

        public void Defer()
        {
            sm.Fire(Trigger.Defer);
            Console.WriteLine("Defer");
        }

        public void Review()
        {
            sm.Fire(Trigger.Review);
            Console.WriteLine("Review");
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
            Console.WriteLine("Initial State: " + bug.GetState());

            bug.Assign();
            Console.WriteLine("State: " + bug.GetState());

            bug.Review();
            Console.WriteLine("State: " + bug.GetState());

            bug.Close();
            Console.WriteLine("State: " + bug.GetState());

            bug.Reopen();
            Console.WriteLine("State: " + bug.GetState());

            bug.Assign();
            Console.WriteLine("State: " + bug.GetState());

            bug.Defer();
            Console.WriteLine("State: " + bug.GetState());

            bug.Assign();
            Console.WriteLine("State: " + bug.GetState());
        }
    }
}
