using Stateless;

namespace BugPro
{
    public class Bug
    {
        public enum State
        {
            Open,
            Triage,
            InProgress,
            Resolved,
            Closed,
            Reopened,
            Deferred,
            Rejected
        }

        public enum Trigger
        {
            Assign,
            Defer,
            ReturnFromDefer,
            Reject,
            AcceptReject,
            DenyReject,
            Fix,
            Resolve,
            Verify,
            ReturnFromResolve,
            Reopen,
            BackToTriage
        }

        private readonly StateMachine<State, Trigger> _machine;
        private readonly string _title;

        public State CurrentState => _machine.State;

        public Bug(string title)
        {
            _title = title;
            _machine = new StateMachine<State, Trigger>(State.Open);

            _machine.Configure(State.Open)
                .Permit(Trigger.Assign, State.Triage);

            _machine.Configure(State.Triage)
                .Permit(Trigger.Fix, State.InProgress)
                .Permit(Trigger.Defer, State.Deferred)
                .Permit(Trigger.Reject, State.Rejected);

            _machine.Configure(State.Deferred)
                .Permit(Trigger.ReturnFromDefer, State.Triage);

            _machine.Configure(State.Rejected)
                .Permit(Trigger.AcceptReject, State.Closed)
                .Permit(Trigger.DenyReject, State.Triage);

            _machine.Configure(State.InProgress)
                .Permit(Trigger.Resolve, State.Resolved);

            _machine.Configure(State.Resolved)
                .Permit(Trigger.Verify, State.Closed)
                .Permit(Trigger.ReturnFromResolve, State.Triage);

            _machine.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Reopened);

            _machine.Configure(State.Reopened)
                .Permit(Trigger.BackToTriage, State.Triage);
        }

        public void Assign() => _machine.Fire(Trigger.Assign);
        public void Defer() => _machine.Fire(Trigger.Defer);
        public void ReturnFromDefer() => _machine.Fire(Trigger.ReturnFromDefer);
        public void Reject() => _machine.Fire(Trigger.Reject);
        public void AcceptReject() => _machine.Fire(Trigger.AcceptReject);
        public void DenyReject() => _machine.Fire(Trigger.DenyReject);
        public void Fix() => _machine.Fire(Trigger.Fix);
        public void Resolve() => _machine.Fire(Trigger.Resolve);
        public void Verify() => _machine.Fire(Trigger.Verify);
        public void ReturnFromResolve() => _machine.Fire(Trigger.ReturnFromResolve);
        public void Reopen() => _machine.Fire(Trigger.Reopen);
        public void BackToTriage() => _machine.Fire(Trigger.BackToTriage);

        public bool CanFire(Trigger trigger) => _machine.CanFire(trigger);

        public override string ToString()
        {
            return $"Bug \"{_title}\": {_machine.State}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var bug = new Bug("Кнопка не работает");
            Console.WriteLine(bug);

            bug.Assign();
            Console.WriteLine(bug);

            bug.Fix();
            Console.WriteLine(bug);

            bug.Resolve();
            Console.WriteLine(bug);

            bug.Verify();
            Console.WriteLine(bug);

            bug.Reopen();
            Console.WriteLine(bug);

            bug.BackToTriage();
            Console.WriteLine(bug);

            bug.Reject();
            Console.WriteLine(bug);

            bug.AcceptReject();
            Console.WriteLine(bug);
        }
    }
}
