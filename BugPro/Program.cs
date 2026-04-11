using Stateless;

namespace BugPro;

public enum BugState {
    New,                // Новый дефект
    InTriage,           // Разбор дефектов

    NotTimeNow,         // Нет времени сейчас
    NeedSeparateFix,    // Нужно отдельное решение
    ExternalProblem,    // Проблема другого продукта
    NeedMoreInfo,       // Нужно больше информации

    NotABug,            // Не дефект
    NotFixing,          // Не исправлять
    Duplicate,          // Дублирование
    CannotReproduce,    // Не воспроизводимо

    Fixing,             // Исправление

    IsOK,               // ОК?
    IsProblemSolved,    // Проблема решена?


    Closed,             // Закрытие
    Reopened,           // Переоткрытие
    Returned            // Возврат
};

public enum BugAction {
    StartTriage,        // Начать разбор дефектов
    SetNotTimeNow,      // Установить "Нет времени сейчас"
    SetNeedSeparateFix, // Установить "Нужно отдельное решение"
    SetExternalProblem, // Установить "Проблема другого продукта"
    SetNeedMoreInfo,    // Установить "Нужно больше информации"

    SetNotABug,         // Установить "Не дефект"
    SetNotFixing,       // Установить "Не исправлять"
    SetDuplicate,       // Установить "Дублирование"
    SetCannotReproduce, // Установить "Не воспроизводимо"

    StartFixing,        // Начать исправление

    VerifyIsOK,             // Проверить "ОК?"
    VerifyIsProblemSolved,  // Проверить "Проблема решена?"

    Approve,            // Утвердить
    Reject,             // Отклонить
    Reopen,             // Переоткрыть
};

public class Bug {
    private StateMachine<BugState, BugAction> _stateMachine;
    public BugState CurrentState  => _stateMachine.State;

    public Bug() {
        _stateMachine = new StateMachine<BugState, BugAction>(BugState.New);
        Configure();
    }

    private void Configure() {
        // Новый дефект
        _stateMachine.Configure(BugState.New)
            .Permit(BugAction.StartTriage, BugState.InTriage);

        // Разбор дефектов
        _stateMachine.Configure(BugState.InTriage)
            .Permit(BugAction.SetNotTimeNow, BugState.NotTimeNow)
            .Permit(BugAction.SetNeedSeparateFix, BugState.NeedSeparateFix)
            .Permit(BugAction.SetExternalProblem, BugState.ExternalProblem)
            .Permit(BugAction.SetNeedMoreInfo, BugState.NeedMoreInfo)
            .Permit(BugAction.SetNotABug, BugState.NotABug)
            .Permit(BugAction.SetNotFixing, BugState.NotFixing)
            .Permit(BugAction.SetDuplicate, BugState.Duplicate)
            .Permit(BugAction.StartFixing, BugState.Fixing);

        // Нет времени сейчас
        _stateMachine.Configure(BugState.NotTimeNow)
            .Permit(BugAction.StartTriage, BugState.InTriage)
            .Permit(BugAction.StartFixing, BugState.Fixing);
        
        // Нужно отдельное решение
        _stateMachine.Configure(BugState.NeedSeparateFix)
            .Permit(BugAction.StartTriage, BugState.InTriage)
            .Permit(BugAction.StartFixing, BugState.Fixing);
        
        // Проблема другого продукта
        _stateMachine.Configure(BugState.ExternalProblem)
            .Permit(BugAction.StartTriage, BugState.InTriage)
            .Permit(BugAction.StartFixing, BugState.Fixing);
        
        // Нужно больше информации
        _stateMachine.Configure(BugState.NeedMoreInfo)
            .Permit(BugAction.StartTriage, BugState.InTriage)
            .Permit(BugAction.StartFixing, BugState.Fixing);

        // Не дефект
        _stateMachine.Configure(BugState.NotABug)
            .Permit(BugAction.VerifyIsOK, BugState.IsOK);
        
        // Не исправлять
        _stateMachine.Configure(BugState.NotFixing)
            .Permit(BugAction.VerifyIsOK, BugState.IsOK);

        // Дублирование
        _stateMachine.Configure(BugState.Duplicate)
            .Permit(BugAction.VerifyIsOK, BugState.IsOK);

        // Не воспроизводимо
        _stateMachine.Configure(BugState.CannotReproduce)
            .Permit(BugAction.VerifyIsOK, BugState.IsOK);

        // Исправление
        _stateMachine.Configure(BugState.Fixing)
            .Permit(BugAction.VerifyIsProblemSolved, BugState.IsProblemSolved)
            .Permit(BugAction.SetNeedMoreInfo, BugState.NeedMoreInfo)
            .Permit(BugAction.SetCannotReproduce, BugState.CannotReproduce);

        // ОК?
        _stateMachine.Configure(BugState.IsOK)
            .Permit(BugAction.Approve, BugState.Closed)
            .Permit(BugAction.Reject, BugState.Returned);

        // Проблема решена?
        _stateMachine.Configure(BugState.IsProblemSolved)
            .Permit(BugAction.Approve, BugState.Closed)
            .Permit(BugAction.Reject, BugState.Reopened);

        // Закрытие
        _stateMachine.Configure(BugState.Closed)
            .Permit(BugAction.Reopen, BugState.Reopened);

        // Переоткрытие
        _stateMachine.Configure(BugState.Reopened)
            .Permit(BugAction.StartTriage, BugState.InTriage);
        
        _stateMachine.OnTransitioned(e => {
            Console.WriteLine($"Transition from [{e.Source}] to [{e.Destination}] with trigger [{e.Trigger}]");
        });
    }

    public void PerformAction(BugAction action) {
        if (_stateMachine.CanFire(action)) {
            _stateMachine.Fire(action);
        } else {
            throw new InvalidOperationException($"Invalid action: [{action}] in state [{_stateMachine.State}]");
        }
    }

    public void PrintCurrentState() {
        Console.WriteLine($"Current state of the bug: [{_stateMachine.State}]");
    }
}

internal class Program {
    private static void Main(string[] args) {
        var bug = new Bug();
        bug.PrintCurrentState();

        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetNotTimeNow);
        bug.PerformAction(BugAction.StartFixing);
        bug.PerformAction(BugAction.VerifyIsProblemSolved);
        bug.PerformAction(BugAction.Approve);

        bug.PerformAction(BugAction.StartTriage); // Invalid action

        bug.PrintCurrentState();
    }
}