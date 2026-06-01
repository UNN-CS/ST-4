using System;
using System.Collections.Generic;
using Stateless;

namespace BugPro
{
    public class Bug
    {
        public enum State
        {
            NewDefect, Triage, Fix, Closed, Reopened
        }

        public enum Trigger
        {
            StartTriage, NoTime, NeedSeparateSolution, OtherProduct,
            NeedMoreInfo, FixDone, NotDefect, DoNotFix, Duplicate,
            NotReproducible, VerifyOk, VerifyFailed, Reopen
        }

        private readonly StateMachine<State, Trigger> _machine;
        public event Action<State> StateChanged;

        public Bug(State initialState)
        {
            _machine = new StateMachine<State, Trigger>(initialState);
            ConfigureStateMachine();
            _machine.OnTransitioned(OnTransitionedHandler);
        }

        public Bug() : this(State.NewDefect) { }

        public State CurrentState
        {
            get { return _machine.State; }
        }

        private void ConfigureStateMachine()
        {
            _machine.Configure(State.NewDefect)
                .Permit(Trigger.StartTriage, State.Triage);

            _machine.Configure(State.Triage)
                .Permit(Trigger.NoTime, State.Fix)
                .Permit(Trigger.NeedSeparateSolution, State.Fix)
                .Permit(Trigger.OtherProduct, State.Closed)
                .Permit(Trigger.NotDefect, State.Closed)
                .Permit(Trigger.DoNotFix, State.Closed)
                .Permit(Trigger.Duplicate, State.Closed)
                .Permit(Trigger.NotReproducible, State.Closed)
                .Permit(Trigger.FixDone, State.Closed)
                .Ignore(Trigger.NeedMoreInfo)
                .Ignore(Trigger.StartTriage)
                .Ignore(Trigger.Reopen);

            _machine.Configure(State.Fix)
                .Permit(Trigger.FixDone, State.Closed)
                .Permit(Trigger.NotDefect, State.Closed)
                .Permit(Trigger.DoNotFix, State.Closed)
                .Permit(Trigger.Duplicate, State.Closed)
                .Permit(Trigger.NotReproducible, State.Closed)
                .Permit(Trigger.Reopen, State.Reopened);

            _machine.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Reopened)
                .Ignore(Trigger.FixDone)
                .Ignore(Trigger.NotDefect)
                .Ignore(Trigger.DoNotFix)
                .Ignore(Trigger.Duplicate)
                .Ignore(Trigger.NotReproducible);

            _machine.Configure(State.Reopened)
                .Permit(Trigger.StartTriage, State.Triage)
                .Permit(Trigger.FixDone, State.Closed)
                .Ignore(Trigger.Reopen);
        }

        private void OnTransitionedHandler(StateMachine<State, Trigger>.Transition transition)
        {
            if (StateChanged != null)
            {
                StateChanged(transition.Destination);
            }
        }

        public void StartTriage() { _machine.Fire(Trigger.StartTriage); Console.WriteLine("StartTriage"); }
        public void NoTime() { _machine.Fire(Trigger.NoTime); Console.WriteLine("NoTime"); }
        public void NeedSeparateSolution() { _machine.Fire(Trigger.NeedSeparateSolution); Console.WriteLine("NeedSeparateSolution"); }
        public void OtherProduct() { _machine.Fire(Trigger.OtherProduct); Console.WriteLine("OtherProduct"); }
        public void NeedMoreInfo() { _machine.Fire(Trigger.NeedMoreInfo); Console.WriteLine("NeedMoreInfo"); }
        public void FixDone() { _machine.Fire(Trigger.FixDone); Console.WriteLine("FixDone"); }
        public void NotDefect() { _machine.Fire(Trigger.NotDefect); Console.WriteLine("NotDefect"); }
        public void DoNotFix() { _machine.Fire(Trigger.DoNotFix); Console.WriteLine("DoNotFix"); }
        public void Duplicate() { _machine.Fire(Trigger.Duplicate); Console.WriteLine("Duplicate"); }
        public void NotReproducible() { _machine.Fire(Trigger.NotReproducible); Console.WriteLine("NotReproducible"); }
        public void VerifyOk() { _machine.Fire(Trigger.VerifyOk); Console.WriteLine("VerifyOk"); }
        public void VerifyFailed() { _machine.Fire(Trigger.VerifyFailed); Console.WriteLine("VerifyFailed"); }
        public void Reopen() { _machine.Fire(Trigger.Reopen); Console.WriteLine("Reopen"); }

        public IEnumerable<Trigger> GetPermittedTriggers() { return _machine.PermittedTriggers; }
        public bool CanFire(Trigger trigger) { return _machine.CanFire(trigger); }
        public bool IsInState(State expected) { return _machine.State == expected; }
        public override string ToString() { return string.Format("Bug(state={0})", CurrentState); }
    }

    public class Program
    {
        public static void Main(string[] args)
        {

            var bug = new Bug();
            Console.WriteLine($"Initial state: {bug.CurrentState}");

            bug.StartTriage();
            Console.WriteLine($"After StartTriage: {bug.CurrentState}");

            bug.NeedSeparateSolution();
            Console.WriteLine($"After NeedSeparateSolution: {bug.CurrentState}");

            bug.FixDone();
            Console.WriteLine($"After FixDone: {bug.CurrentState}");

            bug.Reopen();
            Console.WriteLine($"After Reopen: {bug.CurrentState}");

            bug.StartTriage();
            bug.NotDefect();
            Console.WriteLine($"Final state: {bug.CurrentState}");

        }
    }
}