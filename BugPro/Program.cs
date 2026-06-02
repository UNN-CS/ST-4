// BugPro/Program.cs
using System;
using Stateless;

namespace BugPro
{
    public enum BugState
    {
        New,
        Assigned,
        Active,
        Resolved,
        UnderReview,
        Closed,
        Reopened,
        Deferred,
        Duplicate,
        InfoRequested
    }

    public enum BugTrigger
    {
        Assign,
        StartWork,
        Resolve,
        StartReview,
        Close,
        Reopen,
        Defer,
        MarkDuplicate,
        AskForDetails,
        SubmitDetails
    }

    public class Bug
    {
        private readonly StateMachine<BugState, BugTrigger> _stateMachine;
        private readonly StateMachine<BugState, BugTrigger>.TriggerWithParameters<string> _assignParameterTrigger;
        private string _assignedDeveloper = string.Empty;

        public BugState CurrentState => _stateMachine.State;
        public string AssignedTo => _assignedDeveloper;

        public Bug()
        {
            _stateMachine = new StateMachine<BugState, BugTrigger>(BugState.New);
            
            _assignParameterTrigger = _stateMachine.SetTriggerParameters<string>(BugTrigger.Assign);

            ConfigureWorkflow();
            
            ConfigureLogs();
        }

        private void ConfigureWorkflow()
        {
            _stateMachine.Configure(BugState.New)
                .Permit(BugTrigger.Assign, BugState.Assigned)
                .Permit(BugTrigger.Defer, BugState.Deferred)
                .Permit(BugTrigger.MarkDuplicate, BugState.Duplicate)
                .Permit(BugTrigger.AskForDetails, BugState.InfoRequested);

            _stateMachine.Configure(BugState.Assigned)
                .Permit(BugTrigger.StartWork, BugState.Active)
                .Permit(BugTrigger.Defer, BugState.Deferred)
                .Permit(BugTrigger.AskForDetails, BugState.InfoRequested);

            _stateMachine.Configure(BugState.Active)
                .Permit(BugTrigger.Resolve, BugState.Resolved)
                .Permit(BugTrigger.Defer, BugState.Deferred);

            _stateMachine.Configure(BugState.Resolved)
                .Permit(BugTrigger.StartReview, BugState.UnderReview);

            _stateMachine.Configure(BugState.UnderReview)
                .Permit(BugTrigger.Close, BugState.Closed)
                .Permit(BugTrigger.Reopen, BugState.Reopened);

            _stateMachine.Configure(BugState.Closed)
                .Permit(BugTrigger.Reopen, BugState.Reopened);

            _stateMachine.Configure(BugState.Reopened)
                .Permit(BugTrigger.Assign, BugState.Assigned);

            _stateMachine.Configure(BugState.Deferred)
                .Permit(BugTrigger.Assign, BugState.New)
                .Permit(BugTrigger.SubmitDetails, BugState.New);

            _stateMachine.Configure(BugState.Duplicate)
                .Permit(BugTrigger.Close, BugState.Closed);

            _stateMachine.Configure(BugState.InfoRequested)
                .Permit(BugTrigger.SubmitDetails, BugState.New)
                .Permit(BugTrigger.Defer, BugState.Deferred);
        }

        private void ConfigureLogs()
        {
            _stateMachine.OnTransitioned(t =>
            {
                Console.WriteLine($"[Transition] State changed from {t.Source} to {t.Destination} by trigger {t.Trigger}");
            });
        }

        public void Assign(string developerName)
        {
            _assignedDeveloper = developerName;
            _stateMachine.Fire(_assignParameterTrigger, developerName);
        }

        public void StartWork() => _stateMachine.Fire(BugTrigger.StartWork);
        public void Resolve() => _stateMachine.Fire(BugTrigger.Resolve);
        public void StartReview() => _stateMachine.Fire(BugTrigger.StartReview);
        public void Close() => _stateMachine.Fire(BugTrigger.Close);
        public void Reopen() => _stateMachine.Fire(BugTrigger.Reopen);
        public void Defer() => _stateMachine.Fire(BugTrigger.Defer);
        public void MarkDuplicate() => _stateMachine.Fire(BugTrigger.MarkDuplicate);
        public void AskForDetails() => _stateMachine.Fire(BugTrigger.AskForDetails);
        public void SubmitDetails() => _stateMachine.Fire(BugTrigger.SubmitDetails);
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== StateMachine Defect Workflow Demo ===\n");
            
            var bug = new Bug();
            Console.WriteLine($"Initial State: {bug.CurrentState}");
            
            bug.Assign("Alice Developer");
            Console.WriteLine($"Current State: {bug.CurrentState}, Assigned Developer: {bug.AssignedTo}");
            
            bug.StartWork();
            Console.WriteLine($"Current State: {bug.CurrentState}");
            
            bug.Resolve();
            Console.WriteLine($"Current State: {bug.CurrentState}");
            
            bug.StartReview();
            Console.WriteLine($"Current State: {bug.CurrentState}");
            
            bug.Close();
            Console.WriteLine($"Current State: {bug.CurrentState}");
            
            bug.Reopen();
            Console.WriteLine($"Current State: {bug.CurrentState}");
            
            bug.Assign("Bob Developer");
            Console.WriteLine($"Current State: {bug.CurrentState}");
            
            Console.WriteLine("\n=== End of Workflow Demo ===");
        }
    }
}
