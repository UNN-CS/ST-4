using Stateless;
using System;

namespace BugPro
{
    public class Bug
    {
        public enum State { Open, Assigned, InProgress, Deferred, Resolved, Closed, Reopened }
        public enum Trigger { Assign, Start, Defer, Resolve, Close, Reopen }

        private readonly StateMachine<State, Trigger> _sm;

        public Bug(State state)
        {
            _sm = new StateMachine<State, Trigger>(state);

            // Конфигурация состояний и переходов
            _sm.Configure(State.Open)
                .Permit(Trigger.Assign, State.Assigned)
                .Permit(Trigger.Close, State.Closed);

            _sm.Configure(State.Assigned)
                .Permit(Trigger.Start, State.InProgress)
                .Permit(Trigger.Defer, State.Deferred)
                .Permit(Trigger.Close, State.Closed)
                .Ignore(Trigger.Assign); // Игнорируем повторный Assign

            _sm.Configure(State.InProgress)
                .Permit(Trigger.Resolve, State.Resolved)
                .Permit(Trigger.Defer, State.Deferred);

            _sm.Configure(State.Deferred)
                .Permit(Trigger.Assign, State.Assigned)
                .Permit(Trigger.Close, State.Closed);

            _sm.Configure(State.Resolved)
                .Permit(Trigger.Close, State.Closed)
                .Permit(Trigger.Reopen, State.Reopened);

            _sm.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Reopened);

            _sm.Configure(State.Reopened)
                .Permit(Trigger.Assign, State.Assigned)
                .Permit(Trigger.Close, State.Closed);
        }

        public void Assign()
        {
            _sm.Fire(Trigger.Assign);
            Console.WriteLine("Assign");
        }

        public void Start()
        {
            _sm.Fire(Trigger.Start);
            Console.WriteLine("Start");
        }

        public void Defer()
        {
            _sm.Fire(Trigger.Defer);
            Console.WriteLine("Defer");
        }

        public void Resolve()
        {
            _sm.Fire(Trigger.Resolve);
            Console.WriteLine("Resolve");
        }

        public void Close()
        {
            _sm.Fire(Trigger.Close);
            Console.WriteLine("Close");
        }

        public void Reopen()
        {
            _sm.Fire(Trigger.Reopen);
            Console.WriteLine("Reopen");
        }

        public State GetState()
        {
            return _sm.State;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var bug = new Bug(Bug.State.Open);
            Console.WriteLine($"Initial state: {bug.GetState()}");

            bug.Assign();
            Console.WriteLine($"State: {bug.GetState()}");

            bug.Start();
            Console.WriteLine($"State: {bug.GetState()}");

            bug.Resolve();
            Console.WriteLine($"State: {bug.GetState()}");

            bug.Close();
            Console.WriteLine($"State: {bug.GetState()}");

            bug.Reopen();
            Console.WriteLine($"State: {bug.GetState()}");

            bug.Assign();
            Console.WriteLine($"State: {bug.GetState()}");

            bug.Defer();
            Console.WriteLine($"State: {bug.GetState()}");

            Console.WriteLine($"Final state: {bug.GetState()}");
        }
    }
}