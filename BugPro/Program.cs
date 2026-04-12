using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stateless;

namespace BugPro
{
    public enum BugState
    {
        New,
        Analysis,
        MoreInfo,
        Fixed,
        NotBug,
        NoFix,
        Duplicate,
        NotRepro,
        Separate,
        NoTime,
        Other
    }

    public enum BugTrigger
    {
        Start,
        AddInfo,
        Fix,
        SetNotBug,
        SetNoFix,
        SetDuplicate,
        SetNotRepro,
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
            machine = new StateMachine<BugState, BugTrigger>(BugState.New);

            machine.Configure(BugState.New)
                .Permit(BugTrigger.Start, BugState.Analysis);

            machine.Configure(BugState.Analysis)
                .Permit(BugTrigger.RequestInfo, BugState.MoreInfo)
                .Permit(BugTrigger.Fix, BugState.Fixed)
                .Permit(BugTrigger.SetNotBug, BugState.NotBug)
                .Permit(BugTrigger.SetNoFix, BugState.NoFix)
                .Permit(BugTrigger.SetDuplicate, BugState.Duplicate)
                .Permit(BugTrigger.SetNotRepro, BugState.NotRepro)
                .Permit(BugTrigger.SetSeparate, BugState.Separate)
                .Permit(BugTrigger.SetNoTime, BugState.NoTime)
                .Permit(BugTrigger.SetOther, BugState.Other);

            machine.Configure(BugState.MoreInfo)
                .Permit(BugTrigger.AddInfo, BugState.Analysis);

            machine.Configure(BugState.Fixed);
            machine.Configure(BugState.NotBug);
            machine.Configure(BugState.NoFix);
            machine.Configure(BugState.Duplicate);
            machine.Configure(BugState.NotRepro);
            machine.Configure(BugState.Separate);
            machine.Configure(BugState.NoTime);
            machine.Configure(BugState.Other);
        }

        public void Start() => machine.Fire(BugTrigger.Start);
        public void AddInfo() => machine.Fire(BugTrigger.AddInfo);
        public void Fix() => machine.Fire(BugTrigger.Fix);
        public void SetNotBug() => machine.Fire(BugTrigger.SetNotBug);
        public void SetNoFix() => machine.Fire(BugTrigger.SetNoFix);
        public void SetDuplicate() => machine.Fire(BugTrigger.SetDuplicate);
        public void SetNotRepro() => machine.Fire(BugTrigger.SetNotRepro);
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
            Console.WriteLine($"Начальное состояние: {bug.State}");

            bug.Start();
            Console.WriteLine($"После Start: {bug.State}");

            bug.Fix();
            Console.WriteLine($"После Fix: {bug.State}");

            try
            {
                bug.SetDuplicate();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Исключение: {ex.Message}");
            }

            Console.WriteLine("Завершено.");
        }
    }

}
