using System;
using Stateless;

namespace BugPro
{
    public enum BugState
    {
        NewDefect,           // НОВЫЙ ДЕФЕКТ
        Analysis,            // РАЗБОР ДЕФЕКТОВ
        NeedMoreInfo,        // НУЖНО БОЛЬШЕ ИНФОРМАЦИИ
        Fixed,               // ИСПРАВЛЕНИЕ
        NotDefect,           // НЕ ДЕФЕКТ
        WonTFix,             // НЕ ИСПРАВЛЯТЬ
        Duplicate,           // ДУБЛИРОВАНИЕ
        NotReproducible,     // НЕ ВОСПРОИЗВОДИМО
        SeparateSolution,    // НУЖНО ОТДЕЛЬНОЕ РЕШЕНИЕ
        NoTimeNow,           // НЕТ ВРЕМЕНИ СЕЙЧАС
        OtherProduct         // ПРОБЛЕМА ДРУГОГО ПРОДУКТА
    }

    public enum BugTrigger
    {
        StartAnalysis,
        ProvideInfo,
        MarkAsFixed,
        MarkAsNotDefect,
        MarkAsWonTFix,
        MarkAsDuplicate,
        MarkAsNotReproducible,
        NeedMoreInfo,
        MarkAsSeparateSolution,
        MarkAsNoTimeNow,
        MarkAsOtherProduct
    }

    public class Bug
    {
        private StateMachine<BugState, BugTrigger> _machine;
        public BugState CurrentState => _machine.State;

        public Bug()
        {
            _machine = new StateMachine<BugState, BugTrigger>(BugState.NewDefect);

            _machine.Configure(BugState.NewDefect)
                .Permit(BugTrigger.StartAnalysis, BugState.Analysis);

            _machine.Configure(BugState.Analysis)
                .Permit(BugTrigger.NeedMoreInfo, BugState.NeedMoreInfo)
                .Permit(BugTrigger.MarkAsFixed, BugState.Fixed)
                .Permit(BugTrigger.MarkAsNotDefect, BugState.NotDefect)
                .Permit(BugTrigger.MarkAsWonTFix, BugState.WonTFix)
                .Permit(BugTrigger.MarkAsDuplicate, BugState.Duplicate)
                .Permit(BugTrigger.MarkAsNotReproducible, BugState.NotReproducible)
                .Permit(BugTrigger.MarkAsSeparateSolution, BugState.SeparateSolution)
                .Permit(BugTrigger.MarkAsNoTimeNow, BugState.NoTimeNow)
                .Permit(BugTrigger.MarkAsOtherProduct, BugState.OtherProduct);

            _machine.Configure(BugState.NeedMoreInfo)
                .Permit(BugTrigger.ProvideInfo, BugState.Analysis);

            // Терминальные состояния (никаких исходящих переходов)
            _machine.Configure(BugState.Fixed);
            _machine.Configure(BugState.NotDefect);
            _machine.Configure(BugState.WonTFix);
            _machine.Configure(BugState.Duplicate);
            _machine.Configure(BugState.NotReproducible);
            _machine.Configure(BugState.SeparateSolution);
            _machine.Configure(BugState.NoTimeNow);
            _machine.Configure(BugState.OtherProduct);
        }

        public void StartAnalysis() => _machine.Fire(BugTrigger.StartAnalysis);
        public void ProvideInfo() => _machine.Fire(BugTrigger.ProvideInfo);
        public void MarkAsFixed() => _machine.Fire(BugTrigger.MarkAsFixed);
        public void MarkAsNotDefect() => _machine.Fire(BugTrigger.MarkAsNotDefect);
        public void MarkAsWonTFix() => _machine.Fire(BugTrigger.MarkAsWonTFix);
        public void MarkAsDuplicate() => _machine.Fire(BugTrigger.MarkAsDuplicate);
        public void MarkAsNotReproducible() => _machine.Fire(BugTrigger.MarkAsNotReproducible);
        public void NeedMoreInfo() => _machine.Fire(BugTrigger.NeedMoreInfo);
        public void MarkAsSeparateSolution() => _machine.Fire(BugTrigger.MarkAsSeparateSolution);
        public void MarkAsNoTimeNow() => _machine.Fire(BugTrigger.MarkAsNoTimeNow);
        public void MarkAsOtherProduct() => _machine.Fire(BugTrigger.MarkAsOtherProduct);
    }

    class Program
    {
        static void Main(string[] args)
        {
            var bug = new Bug();
            Console.WriteLine($"Начальное состояние: {bug.CurrentState}");

            bug.StartAnalysis();
            Console.WriteLine($"После StartAnalysis: {bug.CurrentState}");

            bug.MarkAsFixed();
            Console.WriteLine($"После MarkAsFixed: {bug.CurrentState}");

            try
            {
                bug.MarkAsDuplicate(); // недопустимый переход
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Исключение (ожидаемо): {ex.Message}");
            }

            Console.WriteLine("Работа автомата завершена.");
        }
    }
}
