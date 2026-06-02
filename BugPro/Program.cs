using Stateless;

namespace BugPro;

public enum BugState
{
    NewDefect,
    DefectTriage,
    Fixing,
    NotReproducible,
    DeferredNoTime,
    NeedsSeparateDecision,
    OtherProductProblem,
    NeedMoreInfo,
    Returned,
    Closed,
    Reopened
}

public enum BugTrigger
{
    SubmitToTriage,
    StartFixing,
    CloseAsNotABug,
    CloseAsWontFix,
    CloseAsDuplicate,
    MarkNotReproducible,
    DeferNoTime,
    RequireSeparateDecision,
    MarkOtherProduct,
    RequestMoreInfo,
    ReturnToTriage,
    ProblemSolved,
    ReturnAfterFailedFix,
    ConfirmNotReproducibleOk,
    RejectNotReproducible,
    CompleteReturnToTriage,
    Reopen,
    EnterTriageAfterReopen
}

public sealed class Bug
{
    private BugState _currentState;
    private readonly StateMachine<BugState, BugTrigger> _stateMachine;

    public Bug(BugState initialState = BugState.NewDefect)
    {
        _currentState = initialState;
        _stateMachine = new StateMachine<BugState, BugTrigger>(() => _currentState, next => _currentState = next);

        _stateMachine.Configure(BugState.NewDefect)
            .Permit(BugTrigger.SubmitToTriage, BugState.DefectTriage);

        _stateMachine.Configure(BugState.DefectTriage)
            .Permit(BugTrigger.StartFixing, BugState.Fixing)
            .Permit(BugTrigger.CloseAsNotABug, BugState.Closed)
            .Permit(BugTrigger.CloseAsWontFix, BugState.Closed)
            .Permit(BugTrigger.CloseAsDuplicate, BugState.Closed)
            .Permit(BugTrigger.MarkNotReproducible, BugState.NotReproducible)
            .Permit(BugTrigger.DeferNoTime, BugState.DeferredNoTime)
            .Permit(BugTrigger.RequireSeparateDecision, BugState.NeedsSeparateDecision)
            .Permit(BugTrigger.MarkOtherProduct, BugState.OtherProductProblem)
            .Permit(BugTrigger.RequestMoreInfo, BugState.NeedMoreInfo);

        void SetupParkedState(BugState parkedState)
        {
            _stateMachine.Configure(parkedState)
                .Permit(BugTrigger.ReturnToTriage, BugState.DefectTriage)
                .Permit(BugTrigger.StartFixing, BugState.Fixing);
        }

        SetupParkedState(BugState.DeferredNoTime);
        SetupParkedState(BugState.NeedsSeparateDecision);
        SetupParkedState(BugState.OtherProductProblem);
        SetupParkedState(BugState.NeedMoreInfo);

        _stateMachine.Configure(BugState.Fixing)
            .Permit(BugTrigger.MarkNotReproducible, BugState.NotReproducible)
            .Permit(BugTrigger.RequestMoreInfo, BugState.NeedMoreInfo)
            .Permit(BugTrigger.ProblemSolved, BugState.Closed)
            .Permit(BugTrigger.ReturnAfterFailedFix, BugState.Returned);

        _stateMachine.Configure(BugState.NotReproducible)
            .Permit(BugTrigger.ConfirmNotReproducibleOk, BugState.Closed)
            .Permit(BugTrigger.RejectNotReproducible, BugState.Returned);

        _stateMachine.Configure(BugState.Returned)
            .Permit(BugTrigger.CompleteReturnToTriage, BugState.DefectTriage);

        _stateMachine.Configure(BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _stateMachine.Configure(BugState.Reopened)
            .Permit(BugTrigger.EnterTriageAfterReopen, BugState.DefectTriage);
    }

    public BugState State => _stateMachine.State;

    public bool CanFire(BugTrigger trigger) => _stateMachine.CanFire(trigger);

    public void Fire(BugTrigger trigger) => _stateMachine.Fire(trigger);

    public static string Describe(BugState state)
    {
        switch (state)
        {
            case BugState.NewDefect: return "НОВЫЙ ДЕФЕКТ";
            case BugState.DefectTriage: return "РАЗБОР ДЕФЕКТОВ";
            case BugState.Fixing: return "ИСПРАВЛЕНИЕ";
            case BugState.NotReproducible: return "НЕ ВОСПРОИЗВОДИМО";
            case BugState.DeferredNoTime: return "НЕТ ВРЕМЕНИ";
            case BugState.NeedsSeparateDecision: return "НУЖНО ОТДЕЛЬНОЕ РЕШЕНИЕ";
            case BugState.OtherProductProblem: return "ПРОБЛЕМА ДРУГОГО ПРОДУКТА";
            case BugState.NeedMoreInfo: return "НУЖНО БОЛЬШЕ ИНФОРМАЦИИ";
            case BugState.Returned: return "ВОЗВРАТ";
            case BugState.Closed: return "ЗАКРЫТИЕ";
            case BugState.Reopened: return "ПЕРЕОТКРЫТИЕ";
            default: return state.ToString();
        }
    }

    public string StateDescription => Describe(State);
}

internal static class Program
{
    private static void Main()
    {
        var targetBug = new Bug();

        Console.WriteLine("=== Демонстрация работы жизненного цикла бага (Stateless) ===");
        Console.WriteLine($"Начальный статус: {targetBug.StateDescription} ({targetBug.State})");

        targetBug.Fire(BugTrigger.SubmitToTriage);
        Console.WriteLine($"Отправлено на разбор: {targetBug.StateDescription}");

        targetBug.Fire(BugTrigger.RequestMoreInfo);
        Console.WriteLine($"Запрошены дополнительные данные: {targetBug.StateDescription}");

        targetBug.Fire(BugTrigger.ReturnToTriage);
        Console.WriteLine($"Возврат на этап разбора: {targetBug.StateDescription}");

        targetBug.Fire(BugTrigger.StartFixing);
        Console.WriteLine($"Передано в работу инженеру: {targetBug.StateDescription}");

        targetBug.Fire(BugTrigger.RequestMoreInfo);
        Console.WriteLine($"Из процесса работы запрошена информация: {targetBug.StateDescription}");

        targetBug.Fire(BugTrigger.StartFixing);
        Console.WriteLine($"Возобновлено исправление: {targetBug.StateDescription}");

        targetBug.Fire(BugTrigger.ReturnAfterFailedFix);
        Console.WriteLine($"Проверка не пройдена (возврат): {targetBug.StateDescription}");

        targetBug.Fire(BugTrigger.CompleteReturnToTriage);
        Console.WriteLine($"После возврата отправлено на разбор: {targetBug.StateDescription}");

        targetBug.Fire(BugTrigger.StartFixing);
        Console.WriteLine($"Повторно передано инженеру: {targetBug.StateDescription}");

        targetBug.Fire(BugTrigger.ProblemSolved);
        Console.WriteLine($"Успешно исправлено и закрыто: {targetBug.StateDescription}");

        targetBug.Fire(BugTrigger.Reopen);
        Console.WriteLine($"Дефект переоткрыт: {targetBug.StateDescription}");

        targetBug.Fire(BugTrigger.EnterTriageAfterReopen);
        Console.WriteLine($"Направлено на разбор после переоткрытия: {targetBug.StateDescription}");

        Console.WriteLine();
        Console.WriteLine("Доступные команды (триггеры) для текущего шага:");
        foreach (BugTrigger triggerItem in Enum.GetValues<BugTrigger>())
        {
            if (targetBug.CanFire(triggerItem))
                Console.WriteLine($"  -> {triggerItem}");
        }
    }
}
