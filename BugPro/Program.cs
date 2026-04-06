namespace BugPro
{
    using System;
    using Stateless;

    public enum BugState
    {
        New,                       // Новый дефект
        Triage,                    // Разбор дефектов

        NoTimeNow,                 // Нет времени сейчас
        NeedSeparateSolution,      // Нужно отдельное решение
        ExternalProblem,           // Проблема другого продукта
        NeedMoreInfo,              // Нужно больше информации

        NotABug,                   // Не дефект
        WontFix,                   // Не исправлять
        Duplicate,                 // Дублирование
        CannotReproduce,           // Не воспроизводимо

        Fixing,                    // Исправление

        OkCheck,                   // OK?
        ResolutionCheck,           // Проблема решена?

        Returned,                  // Возврат
        Closed,                    // Закрытие
        Reopened                   // Переоткрытие
    }

    public enum BugAction
    {
        StartTriage,

        NoTime,
        NeedSolution,
        External,
        RequestInfo,

        MarkNotABug,
        MarkWontFix,
        MarkDuplicate,
        MarkCannotReproduce,

        StartFix,

        ToOkCheck,
        ToResolutionCheck,

        Approve,
        Reject,

        Reopen
    }

    public class Bug
    {
        private readonly StateMachine<BugState, BugAction> _machine;

        public BugState State => _machine.State;

        public Bug()
        {
            _machine = new StateMachine<BugState, BugAction>(BugState.New);
            Configure();
        }

        private void Configure()
        {
            // Новый дефект
            _machine.Configure(BugState.New)
                .Permit(BugAction.StartTriage, BugState.Triage);

            // Разбор дефекта
            _machine.Configure(BugState.Triage)
                .Permit(BugAction.NoTime, BugState.NoTimeNow)
                .Permit(BugAction.NeedSolution, BugState.NeedSeparateSolution)
                .Permit(BugAction.External, BugState.ExternalProblem)

                .Permit(BugAction.MarkNotABug, BugState.NotABug)
                .Permit(BugAction.MarkWontFix, BugState.WontFix)
                .Permit(BugAction.MarkDuplicate, BugState.Duplicate)

                .Permit(BugAction.StartFix, BugState.Fixing);

            // Брокеры
            _machine.Configure(BugState.NoTimeNow)
                .Permit(BugAction.StartTriage, BugState.Triage);

            _machine.Configure(BugState.NeedSeparateSolution)
                .Permit(BugAction.StartTriage, BugState.Triage);

            _machine.Configure(BugState.ExternalProblem)
                .Permit(BugAction.StartTriage, BugState.Triage);


            // Исправление
            _machine.Configure(BugState.Fixing)
                .Permit(BugAction.ToResolutionCheck, BugState.ResolutionCheck)
                .Permit(BugAction.RequestInfo, BugState.NeedMoreInfo)
                .Permit(BugAction.MarkCannotReproduce, BugState.CannotReproduce);


            // Нужно больше информации
            _machine.Configure(BugState.NeedMoreInfo)
                .Permit(BugAction.StartFix, BugState.Fixing);

            // Не дефект
            _machine.Configure(BugState.NotABug)
                .Permit(BugAction.ToOkCheck, BugState.OkCheck);
            
            // Не исправлять
            _machine.Configure(BugState.WontFix)
                .Permit(BugAction.ToOkCheck, BugState.OkCheck);

            // Дублирование
            _machine.Configure(BugState.Duplicate)
                .Permit(BugAction.ToOkCheck, BugState.OkCheck);

            // Не воспроизводимо
            _machine.Configure(BugState.CannotReproduce)
                .Permit(BugAction.ToOkCheck, BugState.OkCheck);

            // OK?
            _machine.Configure(BugState.OkCheck)
                .Permit(BugAction.Approve, BugState.Closed)
                .Permit(BugAction.Reject, BugState.Returned);

            // Проблема решена?
            _machine.Configure(BugState.ResolutionCheck)
                .Permit(BugAction.Approve, BugState.Closed)
                .Permit(BugAction.Reject, BugState.Returned);

            // Возврат -> только в Разбор
            _machine.Configure(BugState.Returned)
                .Permit(BugAction.StartTriage, BugState.Triage);

            // Закрытие -> Переоткрытие
            _machine.Configure(BugState.Closed)
                .Permit(BugAction.Reopen, BugState.Reopened);

            // Переоткрытие -> Разбор
            _machine.Configure(BugState.Reopened)
                .Permit(BugAction.StartTriage, BugState.Triage);

            _machine.OnTransitioned(t =>
            {
                Console.WriteLine($"{t.Source} -> {t.Destination} via {t.Trigger}");
            });
        }

        public void Fire(BugAction trigger)
        {
            _machine.Fire(trigger);
        }

        public bool IsFinal()
        {
            return !_machine.PermittedTriggers.Any();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var bug = new Bug();

            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.StartFix);
            bug.Fire(BugAction.ToResolutionCheck);
            bug.Fire(BugAction.Reject);

            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.StartFix);
            bug.Fire(BugAction.ToResolutionCheck);
            bug.Fire(BugAction.Approve);

            bug.Fire(BugAction.Reopen);
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.MarkWontFix);

            Console.WriteLine($"Final: {bug.State}");
        }
    }

}
