using System;
using Stateless;

namespace BugPro
{
    public enum Role
    {
        Tester, // ТЕСТИРОВЩИК
        ProductTeam, // ПРОДУКТОВАЯ КОМАНДА
        Developer // РАЗРАБОТЧИК
    }
    
    public enum BugState
    {
        New, // НОВЫЙ ДЕФЕКТ
        Triage, // РАЗБОР ДЕФЕКТОВ
        Fixing, // ИСПРАВЛЕНИЕ
        
        NotBug, // НЕ ДЕФЕКТ
        DontFix, // НЕ ИСПРАВЛЯТЬ
        Duplicate, // ДУБЛИРОВАНИЕ
        
        NotReproducible, // НЕ ВОСПРОИЗВОДИМО
        
        NoTime, // НЕТ ВРЕМЕНИ СЕЙЧАС
        NeedSeparateSolution, // НУЖНО ОТДЕЛЬНОЕ РЕШЕНИЕ
        OtherProductProblem, // ПРОБЛЕМА ДРУГОГО ПРОДУКТА
        RequestMoreInfo, // НУЖНО БОЛЬШЕ ИНФОРМАЦИИ
        
        Review, // ПРОБЛЕМА РЕШЕНА?
        Closed // ЗАКРЫТИЕ
    }

    public enum BugTrigger
    {
        StartTriage, // РАЗБОР ДЕФЕКТОВ
        StartFix, // ИСПРАВЛЕНИЕ

        NotBug, // НЕ ДЕФЕКТ
        DontFix, // НЕ ИСПРАВЛЯТЬ
        Duplicate, // ДУБЛИРОВАНИЕ
        NotReproducible, // НЕ ВОСПРОИЗВОДИМО

        NoTime, // НЕТ ВРЕМЕНИ СЕЙЧАС
        NeedSeparateSolution, // НУЖНО ОТДЕЛЬНОЕ РЕШЕНИЕ
        OtherProductProblem, // ПРОБЛЕМА ДРУГОГО ПРОДУКТА
        RequestMoreInfo, // НУЖНО БОЛЬШЕ ИНФОРМАЦИИ

        EndFix, // ПРОБЛЕМА РЕШЕНА?
        Approve, // ЗАКРЫТИЕ
        Reject, // ВОЗВРАТ
        Reopen // ПЕРЕОТКРЫТИЕ
    }

    public class Bug
    {
        private readonly StateMachine<BugState, BugTrigger> _machine;
        private Role _role;

        public BugState State => _machine.State;

        public Bug(BugState initialState = BugState.New)
        {
            _machine = new StateMachine<BugState, BugTrigger>(initialState);
            _role = Role.Tester;
            Configure();
        }

        public void AssignRole(Role role) => _role = role;

        private void Configure()
        {
            _machine.Configure(BugState.New)
                .PermitIf(BugTrigger.StartTriage, BugState.Triage, () => _role == Role.ProductTeam);

            _machine.Configure(BugState.Triage)
                .PermitIf(BugTrigger.StartFix, BugState.Fixing, () => _role == Role.Developer)
                .PermitIf(BugTrigger.NotBug, BugState.NotBug, () => _role == Role.ProductTeam)
                .PermitIf(BugTrigger.DontFix, BugState.DontFix, () => _role == Role.ProductTeam)
                .PermitIf(BugTrigger.Duplicate, BugState.Duplicate, () => _role == Role.ProductTeam)
                .PermitIf(BugTrigger.NotReproducible, BugState.NotReproducible, () => _role == Role.Developer);

            _machine.Configure(BugState.Fixing)
                .PermitIf(BugTrigger.EndFix, BugState.Review, () => _role == Role.Developer)
                .PermitIf(BugTrigger.NoTime, BugState.NoTime, () => _role == Role.Developer)
                .PermitIf(BugTrigger.NeedSeparateSolution, BugState.NeedSeparateSolution, () => _role == Role.Developer)
                .PermitIf(BugTrigger.OtherProductProblem, BugState.OtherProductProblem, () => _role == Role.Developer)
                .PermitIf(BugTrigger.RequestMoreInfo, BugState.RequestMoreInfo, () => _role == Role.Tester);

            _machine.Configure(BugState.NotReproducible)
                .PermitIf(BugTrigger.Approve, BugState.Closed, () => _role == Role.Tester)
                .PermitIf(BugTrigger.Reject, BugState.Triage, () => _role == Role.Tester);

            _machine.Configure(BugState.NotBug)
                .PermitIf(BugTrigger.Approve, BugState.Closed, () => _role == Role.Tester)
                .PermitIf(BugTrigger.Reject, BugState.Triage, () => _role == Role.Tester);
            _machine.Configure(BugState.DontFix)
                .PermitIf(BugTrigger.Approve, BugState.Closed, () => _role == Role.Tester)
                .PermitIf(BugTrigger.Reject, BugState.Triage, () => _role == Role.Tester);
            _machine.Configure(BugState.Duplicate)
                .PermitIf(BugTrigger.Approve, BugState.Closed, () => _role == Role.Tester)
                .PermitIf(BugTrigger.Reject, BugState.Triage, () => _role == Role.Tester);

            _machine.Configure(BugState.NoTime)
                .PermitIf(BugTrigger.StartTriage, BugState.Triage, () => _role == Role.ProductTeam)
                .PermitIf(BugTrigger.StartFix, BugState.Fixing, () => _role == Role.Developer);
            _machine.Configure(BugState.NeedSeparateSolution)
                .PermitIf(BugTrigger.StartTriage, BugState.Triage, () => _role == Role.ProductTeam)
                .PermitIf(BugTrigger.StartFix, BugState.Fixing, () => _role == Role.Developer);
            _machine.Configure(BugState.OtherProductProblem)
                .PermitIf(BugTrigger.StartTriage, BugState.Triage, () => _role == Role.ProductTeam)
                .PermitIf(BugTrigger.StartFix, BugState.Fixing, () => _role == Role.Developer);
            _machine.Configure(BugState.RequestMoreInfo)
                .PermitIf(BugTrigger.StartTriage, BugState.Triage, () => _role == Role.ProductTeam)
                .PermitIf(BugTrigger.StartFix, BugState.Fixing, () => _role == Role.Developer);

            _machine.Configure(BugState.Review)
                .PermitIf(BugTrigger.Approve, BugState.Closed, () => _role == Role.Tester)
                .PermitIf(BugTrigger.Reject, BugState.Triage, () => _role == Role.Tester);

            _machine.Configure(BugState.Closed)
                .PermitIf(BugTrigger.Reopen, BugState.Triage, () => _role == Role.Tester);
        }

        public void StartTriage() => _machine.Fire(BugTrigger.StartTriage);
        public void StartFix() => _machine.Fire(BugTrigger.StartFix);
        public void NotBug() => _machine.Fire(BugTrigger.NotBug);
        public void DontFix() => _machine.Fire(BugTrigger.DontFix);
        public void Duplicate() => _machine.Fire(BugTrigger.Duplicate);
        public void NotReproducible() => _machine.Fire(BugTrigger.NotReproducible);
        public void NoTime() => _machine.Fire(BugTrigger.NoTime);
        public void NeedSeparateSolution() => _machine.Fire(BugTrigger.NeedSeparateSolution);
        public void OtherProductProblem() => _machine.Fire(BugTrigger.OtherProductProblem);
        public void RequestMoreInfo() => _machine.Fire(BugTrigger.RequestMoreInfo);
        public void EndFix() => _machine.Fire(BugTrigger.EndFix);
        public void Approve() => _machine.Fire(BugTrigger.Approve);
        public void Reject() => _machine.Fire(BugTrigger.Reject);
        public void Reopen() => _machine.Fire(BugTrigger.Reopen);
    }

    class Program
    {
        static void Main(string[] args)
        {
            var bug = new Bug();

            bug.AssignRole(Role.ProductTeam);
            bug.StartTriage();
            Console.WriteLine($"{bug.State}");

            bug.AssignRole(Role.Developer);
            bug.StartFix();
            Console.WriteLine($"{bug.State}");

            bug.EndFix();
            Console.WriteLine($"{bug.State}");

            bug.AssignRole(Role.Tester);
            bug.Approve();
            Console.WriteLine($"{bug.State}");

            bug.Reopen();
            Console.WriteLine($"{bug.State}");
        }
    }
}
