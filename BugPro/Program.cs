using System;
using Stateless;

namespace BugPro
{
    public enum BugState
    {
        New,                // НОВЫЙ ДЕФЕКТ
        Triage,             // РАЗБОР ДЕФЕКТОВ
        Fixing,             // ИСПРАВЛЕНИЕ
        Testing,            // ТЕСТИРОВЩИК
        ProductTeam,        // ПРОДУКТОВАЯ КОМАНДА
        Developer,          // РАЗРАБОТЧИК
        ProblemSolving,     // ПРОБЛЕМА РЕШЕНИЯ?
        Closing,            // ЗАКРЫТИЕ
        NotADefect,         // НЕ ДЕФЕКТ
        WontFix,            // НЕ ИСПРАВЛЯТЬ
        Duplicate,          // ДУБЛИРОВАНИЕ
        CannotReproduce,    // НЕ ВОСПРОИЗВОДИМО
        OkCheck,            // OK?
        Reopened            // ПЕРЕОТКРЫТИЕ
    }

    public enum BugTrigger
    {
        AssignToTriage,
        NoTime,
        SeparateSolution,
        OtherProduct,
        NeedMoreInfo,
        StartFix,
        ToTester,
        ToProductTeam,
        ToDeveloper,
        ProblemNo,
        ProblemYes,
        NotADefect,
        WontFix,
        Duplicate,
        CannotReproduce,
        OkNo,
        OkYes,
        Reopen
    }

    public class Bug
    {
        private StateMachine<BugState, BugTrigger> _machine;
        private string _bugId;

        public BugState State => _machine.State;
        public string BugId => _bugId;

        public Bug(string bugId = "BUG-001")
        {
            _bugId = bugId;
            _machine = new StateMachine<BugState, BugTrigger>(BugState.New);
            ConfigureStates();
        }

        private void ConfigureStates()
        {
            // НОВЫЙ ДЕФЕКТ -> РАЗБОР ДЕФЕКТОВ
            _machine.Configure(BugState.New)
                .Permit(BugTrigger.AssignToTriage, BugState.Triage);

            // РАЗБОР ДЕФЕКТОВ
            _machine.Configure(BugState.Triage)
                .PermitReentry(BugTrigger.NoTime)
                .PermitReentry(BugTrigger.SeparateSolution)
                .PermitReentry(BugTrigger.OtherProduct)
                .PermitReentry(BugTrigger.NeedMoreInfo)
                .Permit(BugTrigger.StartFix, BugState.Fixing)
                .Permit(BugTrigger.NotADefect, BugState.NotADefect)
                .Permit(BugTrigger.WontFix, BugState.WontFix)
                .Permit(BugTrigger.Duplicate, BugState.Duplicate)
                .Permit(BugTrigger.CannotReproduce, BugState.CannotReproduce);

            // ИСПРАВЛЕНИЕ
            _machine.Configure(BugState.Fixing)
                .Permit(BugTrigger.ToTester, BugState.Testing)
                .Permit(BugTrigger.ToProductTeam, BugState.ProductTeam)
                .Permit(BugTrigger.ToDeveloper, BugState.Developer);

            // ТЕСТИРОВЩИК
            _machine.Configure(BugState.Testing)
                .Permit(BugTrigger.ProblemNo, BugState.Closing)
                .Permit(BugTrigger.ProblemYes, BugState.ProblemSolving);

            // ПРОДУКТОВАЯ КОМАНДА
            _machine.Configure(BugState.ProductTeam)
                .Permit(BugTrigger.ProblemNo, BugState.Closing)
                .Permit(BugTrigger.ProblemYes, BugState.ProblemSolving);

            // РАЗРАБОТЧИК
            _machine.Configure(BugState.Developer)
                .Permit(BugTrigger.ProblemNo, BugState.Closing)
                .Permit(BugTrigger.ProblemYes, BugState.ProblemSolving);

            // ПРОБЛЕМА РЕШЕНИЯ?
            _machine.Configure(BugState.ProblemSolving)
                .Permit(BugTrigger.OkNo, BugState.OkCheck)
                .Permit(BugTrigger.OkYes, BugState.Closing);

            // OK?
            _machine.Configure(BugState.OkCheck)
                .Permit(BugTrigger.OkNo, BugState.Triage)
                .Permit(BugTrigger.OkYes, BugState.Closing);

            // Терминальные состояния -> Reopened
            _machine.Configure(BugState.NotADefect)
                .Permit(BugTrigger.Reopen, BugState.Reopened);
            _machine.Configure(BugState.WontFix)
                .Permit(BugTrigger.Reopen, BugState.Reopened);
            _machine.Configure(BugState.Duplicate)
                .Permit(BugTrigger.Reopen, BugState.Reopened);
            _machine.Configure(BugState.CannotReproduce)
                .Permit(BugTrigger.Reopen, BugState.Reopened);

            // ЗАКРЫТИЕ -> Reopened
            _machine.Configure(BugState.Closing)
                .Permit(BugTrigger.Reopen, BugState.Reopened);

            // ПЕРЕОТКРЫТИЕ -> Triage
            _machine.Configure(BugState.Reopened)
                .Permit(BugTrigger.AssignToTriage, BugState.Triage);
        }

        public void Fire(BugTrigger trigger)
        {
            if (_machine.CanFire(trigger))
            {
                _machine.Fire(trigger);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Невозможно выполнить {trigger} в состоянии {_machine.State}");
            }
        }

        public bool CanFire(BugTrigger trigger) => _machine.CanFire(trigger);
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("WorkFlow бага");
            Console.WriteLine();

            var bug = new Bug("TEST-001");
            Console.WriteLine("Начальное состояние: " + bug.State);
            Console.WriteLine();

            try
            {
                bug.Fire(BugTrigger.AssignToTriage);
                Console.WriteLine("AssignToTriage -> " + bug.State);

                bug.Fire(BugTrigger.StartFix);
                Console.WriteLine("StartFix -> " + bug.State);

                bug.Fire(BugTrigger.ToTester);
                Console.WriteLine("ToTester -> " + bug.State);

                bug.Fire(BugTrigger.ProblemNo);
                Console.WriteLine("ProblemNo -> " + bug.State);

                Console.WriteLine();
                Console.WriteLine("Конечное состояние: " + bug.State);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }
    }
}