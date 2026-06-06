using System;
using Stateless;

namespace BugPro
{
    public enum BugState
    {
        NewDefect,
        Analysis,
        InProgress,
        Closed,
        Reopened
    }

    public enum BugTrigger
    {
        StartAnalysis,
        SendToFix,
        NeedMoreInfo,
        NoTime,
        SeparateSolution,
        OtherProduct,
        NotDefect,
        WonFix,
        Duplicate,
        NotReproducible,
        Fix,
        VerifyOK,
        VerifyFail,
        Reopen
    }

    public class Bug
    {
        private readonly StateMachine<BugState, BugTrigger> _machine;
        public BugState State => _machine.State;
        public string? CloseReason { get; private set; }
        public int Id { get; }
        public string Title { get; }

        public Bug(int id, string title)
        {
            Id = id;
            Title = title;
            _machine = new StateMachine<BugState, BugTrigger>(BugState.NewDefect);
            CloseReason = null;

            ConfigureStateMachine();
        }

        private void ConfigureStateMachine()
        {
            _machine.Configure(BugState.NewDefect)
                .Permit(BugTrigger.StartAnalysis, BugState.Analysis);

            _machine.Configure(BugState.Analysis)
                .PermitReentry(BugTrigger.NeedMoreInfo)
                .PermitReentry(BugTrigger.NoTime)
                .PermitReentry(BugTrigger.SeparateSolution)
                .PermitReentry(BugTrigger.OtherProduct)
                .Permit(BugTrigger.SendToFix, BugState.InProgress)
                .Permit(BugTrigger.NotDefect, BugState.Closed)
                .Permit(BugTrigger.WonFix, BugState.Closed)
                .Permit(BugTrigger.Duplicate, BugState.Closed)
                .Permit(BugTrigger.NotReproducible, BugState.Closed)
                .OnEntryFrom(BugTrigger.NotDefect, () => CloseReason = "Not a defect")
                .OnEntryFrom(BugTrigger.WonFix, () => CloseReason = "Won't fix")
                .OnEntryFrom(BugTrigger.Duplicate, () => CloseReason = "Duplicate")
                .OnEntryFrom(BugTrigger.NotReproducible, () => CloseReason = "Not reproducible");

            _machine.Configure(BugState.InProgress)
                .Permit(BugTrigger.Fix, BugState.Analysis)
                .Permit(BugTrigger.VerifyOK, BugState.Closed)
                .Permit(BugTrigger.VerifyFail, BugState.Analysis);

            _machine.Configure(BugState.Closed)
                .Permit(BugTrigger.Reopen, BugState.Reopened);

            _machine.Configure(BugState.Reopened)
                .Permit(BugTrigger.StartAnalysis, BugState.Analysis);
        }

        public void StartAnalysis() => _machine.Fire(BugTrigger.StartAnalysis);
        public void SendToFix() => _machine.Fire(BugTrigger.SendToFix);
        public void NeedMoreInfo() => _machine.Fire(BugTrigger.NeedMoreInfo);
        public void NoTime() => _machine.Fire(BugTrigger.NoTime);
        public void SeparateSolution() => _machine.Fire(BugTrigger.SeparateSolution);
        public void OtherProduct() => _machine.Fire(BugTrigger.OtherProduct);
        public void SetNotDefect() => _machine.Fire(BugTrigger.NotDefect);
        public void SetNoFix() => _machine.Fire(BugTrigger.WonFix);
        public void SetDuplicate() => _machine.Fire(BugTrigger.Duplicate);
        public void SetNotReproducible() => _machine.Fire(BugTrigger.NotReproducible);
        public void Fix() => _machine.Fire(BugTrigger.Fix);
        public void VerifyOK() => _machine.Fire(BugTrigger.VerifyOK);
        public void VerifyFail() => _machine.Fire(BugTrigger.VerifyFail);
        public void Reopen() => _machine.Fire(BugTrigger.Reopen);
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Bug Workflow Demo (by scheme) ===\n");

            var bug = new Bug(101, "App crashes on startup");
            Console.WriteLine($"Created bug #{bug.Id}: '{bug.Title}'");
            Console.WriteLine($"Initial state: {bug.State}\n");

            bug.StartAnalysis();
            Console.WriteLine($"After StartAnalysis: {bug.State}");

            bug.SendToFix();
            Console.WriteLine($"After SendToFix: {bug.State}");

            bug.VerifyOK();
            Console.WriteLine($"After VerifyOK: {bug.State}\n");

            Console.WriteLine("Workflow demo completed!");
        }
    }
}
