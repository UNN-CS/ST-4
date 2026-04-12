using Stateless;
using System;

namespace BugPro
{
    public enum BugState
    {
        NewDefect,
        Analysis,
        NoTime,
        SeparateSolution,
        OtherProduct,
        MoreInfo,
        NoDefect,
        NoFix,
        Duplicate,
        NoReproducible,
        Fixed
    }
    public enum BugTrigger
    {
        Start,
        AddInfo,
        Fix,
        SetNoDefect,
        SetNoFix,
        SetDuplicate,
        SetNoReproducible,
        RequestInfo,
        SetSeparate,
        SetNoTime,
        SetOther
    }

    public class Bug
    {
        private StateMachine<BugState, BugTrigger> machine;
        public BugState State => machine.State;

        public Bug()
        {
            machine = new StateMachine<BugState, BugTrigger>(BugState.NewDefect);

            machine.Configure(BugState.NewDefect)
                .Permit(BugTrigger.Start, BugState.Analysis);

            machine.Configure(BugState.Analysis)
                .Permit(BugTrigger.RequestInfo, BugState.MoreInfo)
                .Permit(BugTrigger.Fix, BugState.Fixed)
                .Permit(BugTrigger.SetNoDefect, BugState.NoDefect)
                .Permit(BugTrigger.SetNoFix, BugState.NoFix)
                .Permit(BugTrigger.SetDuplicate, BugState.Duplicate)
                .Permit(BugTrigger.SetNoReproducible, BugState.NoReproducible)
                .Permit(BugTrigger.SetSeparate, BugState.SeparateSolution)
                .Permit(BugTrigger.SetNoTime, BugState.NoTime)
                .Permit(BugTrigger.SetOther, BugState.OtherProduct);

            machine.Configure(BugState.MoreInfo)
                .Permit(BugTrigger.AddInfo, BugState.Analysis);

            machine.Configure(BugState.Fixed);
            machine.Configure(BugState.NoDefect);
            machine.Configure(BugState.NoFix);
            machine.Configure(BugState.Duplicate);
            machine.Configure(BugState.NoReproducible);
            machine.Configure(BugState.SeparateSolution);
            machine.Configure(BugState.NoTime);
            machine.Configure(BugState.OtherProduct);
        }

        public void Start() => machine.Fire(BugTrigger.Start);
        public void AddInfo() => machine.Fire(BugTrigger.AddInfo);
        public void Fix() => machine.Fire(BugTrigger.Fix);
        public void SetNotBug() => machine.Fire(BugTrigger.SetNoDefect);
        public void SetNoFix() => machine.Fire(BugTrigger.SetNoFix);
        public void SetDuplicate() => machine.Fire(BugTrigger.SetDuplicate);
        public void SetNotRepro() => machine.Fire(BugTrigger.SetNoReproducible);
        public void RequestInfo() => machine.Fire(BugTrigger.RequestInfo);
        public void SetSeparate() => machine.Fire(BugTrigger.SetSeparate);
        public void SetNoTime() => machine.Fire(BugTrigger.SetNoTime);
        public void SetOther() => machine.Fire(BugTrigger.SetOther);
    }

    class Program
    {
        static void Main(string[] args)
        {
            var bug = new Bug();
            Console.WriteLine($"Initial State: {bug.State}");

            bug.Start();
            Console.WriteLine($"After Start: {bug.State}");

            bug.Fix();
            Console.WriteLine($"After Fix: {bug.State}");

            try
            {
                bug.SetDuplicate();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            Console.WriteLine("Completed.");
        }
    }
}
