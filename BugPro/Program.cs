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
            Defered,
            InProgress,
            Resolved,
            Closed,
            Reopened
        }

        private enum Trigger
        {
            Assign,
            Defer,
            StartProgress,
            Resolve,
            Close,
            Reopen
        }

        private readonly StateMachine<State, Trigger> sm;

        public Bug(State initialState)
        {
            sm = new StateMachine<State, Trigger>(initialState);

            sm.Configure(State.Open)
              .Permit(Trigger.Assign, State.Assigned)
              .Permit(Trigger.StartProgress, State.InProgress);

            sm.Configure(State.Assigned)
              .Permit(Trigger.StartProgress, State.InProgress)
              .Permit(Trigger.Defer, State.Defered)
              .Ignore(Trigger.Assign);

            sm.Configure(State.InProgress)
              .Permit(Trigger.Resolve, State.Resolved);

            sm.Configure(State.Resolved)
              .Permit(Trigger.Close, State.Closed)
              .Permit(Trigger.Reopen, State.Reopened);

            sm.Configure(State.Closed)
              .Permit(Trigger.Reopen, State.Reopened);

            sm.Configure(State.Defered)
              .Permit(Trigger.Assign, State.Assigned);

            sm.Configure(State.Reopened)
              .Permit(Trigger.Assign, State.Assigned)
              .Permit(Trigger.StartProgress, State.InProgress);
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

        public void StartProgress()
        {
            sm.Fire(Trigger.StartProgress);
            Console.WriteLine("Start Progress");
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
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            bug.Reopen();
            bug.Assign();

            Console.WriteLine("Final State: " + bug.GetState());

            Console.WriteLine("Нажмите любую клавишу для завершения...");
            Console.ReadKey();
        }
    }
}