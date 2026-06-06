using Stateless;

namespace BugPro
{
    public enum State
    {
        Open,
        Assigned,
        InProgress,
        Fixed,
        Verified,
        Closed,
        Reopened,
        Deferred,
        Rejected,
        Duplicate
    }

    public enum Trigger
    {
        Assign,
        StartProgress,
        Fix,
        Verify,
        Close,
        Reopen,
        Defer,
        Reject,
        MarkAsDuplicate,
        Resolve
    }

    public class Bug
    {
        private readonly StateMachine<State, Trigger> _machine;
        private readonly string _title;
        private string _assignee;

        public State CurrentState => _machine.State;
        public string Title => _title;
        public string Assignee => _assignee;

        public Bug(string title)
        {
            _title = title;
            _assignee = string.Empty;

            _machine = new StateMachine<State, Trigger>(State.Open);

            // Open -> Assigned
            _machine.Configure(State.Open)
                .Permit(Trigger.Assign, State.Assigned)
                .Permit(Trigger.Reject, State.Rejected)
                .Permit(Trigger.MarkAsDuplicate, State.Duplicate);

            // Assigned -> InProgress, Deferred, Rejected
            _machine.Configure(State.Assigned)
                .Permit(Trigger.StartProgress, State.InProgress)
                .Permit(Trigger.Defer, State.Deferred)
                .Permit(Trigger.Reject, State.Rejected);

            // InProgress -> Fixed, Deferred
            _machine.Configure(State.InProgress)
                .Permit(Trigger.Fix, State.Fixed)
                .Permit(Trigger.Defer, State.Deferred);

            // Fixed -> Verified
            _machine.Configure(State.Fixed)
                .Permit(Trigger.Verify, State.Verified)
                .Permit(Trigger.Reopen, State.Reopened);

            // Verified -> Closed, Reopened
            _machine.Configure(State.Verified)
                .Permit(Trigger.Close, State.Closed)
                .Permit(Trigger.Reopen, State.Reopened);

            // Closed -> Reopened
            _machine.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Reopened);

            // Reopened -> Assigned, Deferred
            _machine.Configure(State.Reopened)
                .Permit(Trigger.Assign, State.Assigned)
                .Permit(Trigger.Defer, State.Deferred);

            // Deferred -> Assigned
            _machine.Configure(State.Deferred)
                .Permit(Trigger.Assign, State.Assigned);

            // Rejected -> Closed
            _machine.Configure(State.Rejected)
                .Permit(Trigger.Close, State.Closed);

            // Duplicate -> Closed
            _machine.Configure(State.Duplicate)
                .Permit(Trigger.Close, State.Closed);
        }

        public void Assign(string assignee)
        {
            _assignee = assignee;
            _machine.Fire(Trigger.Assign);
        }

        public void StartProgress()
        {
            _machine.Fire(Trigger.StartProgress);
        }

        public void Fix()
        {
            _machine.Fire(Trigger.Fix);
        }

        public void Verify()
        {
            _machine.Fire(Trigger.Verify);
        }

        public void Close()
        {
            _machine.Fire(Trigger.Close);
        }

        public void Reopen()
        {
            _machine.Fire(Trigger.Reopen);
        }

        public void Defer()
        {
            _machine.Fire(Trigger.Defer);
        }

        public void Reject()
        {
            _machine.Fire(Trigger.Reject);
        }

        public void MarkAsDuplicate()
        {
            _machine.Fire(Trigger.MarkAsDuplicate);
        }

        public void PrintStateInfo()
        {
            Console.WriteLine($"Bug: {_title}");
            Console.WriteLine($"State: {CurrentState}");
            Console.WriteLine($"Assignee: {_assignee}");
            Console.WriteLine(new string('-', 40));
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Bug Workflow Demonstration");
            Console.WriteLine(new string('=', 50));

            // Демонстрация нормального workflow
            var bug1 = new Bug("Login page crash");
            bug1.PrintStateInfo();

            bug1.Assign("John");
            bug1.PrintStateInfo();

            bug1.StartProgress();
            bug1.PrintStateInfo();

            bug1.Fix();
            bug1.PrintStateInfo();

            bug1.Verify();
            bug1.PrintStateInfo();

            bug1.Close();
            bug1.PrintStateInfo();

            // Демонстрация переоткрытия
            Console.WriteLine("\n--- Reopen Demo ---");
            bug1.Reopen();
            bug1.PrintStateInfo();

            bug1.Assign("Jane");
            bug1.PrintStateInfo();

            bug1.StartProgress();
            bug1.PrintStateInfo();

            bug1.Fix();
            bug1.PrintStateInfo();

            bug1.Verify();
            bug1.PrintStateInfo();

            bug1.Close();
            bug1.PrintStateInfo();

            // Демонстрация отклонения
            var bug2 = new Bug("UI misalignment");
            bug2.PrintStateInfo();

            bug2.Assign("Bob");
            bug2.PrintStateInfo();

            bug2.Reject();
            bug2.PrintStateInfo();

            bug2.Close();
            bug2.PrintStateInfo();

            // Демонстрация дубликата
            var bug3 = new Bug("Typo in header");
            bug3.PrintStateInfo();

            bug3.MarkAsDuplicate();
            bug3.PrintStateInfo();

            bug3.Close();
            bug3.PrintStateInfo();

            Console.WriteLine("\nAll demonstrations completed successfully!");
        }
    }
}