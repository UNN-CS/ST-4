using System.Collections.ObjectModel;
using System.Text;
using Stateless;
using Stateless.Graph;

namespace BugPro;

/// <summary>
/// Жизненный цикл дефекта по диаграмме workflow (разбор, отложенные статусы, отклонение с OK/возврат,
/// исправление и верификация, закрытие и переоткрытие). Расширение: эскалация из разбора.
/// </summary>
public enum State
{
    NewDefect,
    DefectAnalysis,
    DeferredNoTimeNow,
    DeferredNeedSeparateDecision,
    DeferredOtherProduct,
    DeferredNeedMoreInfo,
    RejectedNotDefect,
    RejectedWontFix,
    RejectedDuplicate,
    RejectedNotReproducible,
    Fixing,
    Return,
    Closed,
    Escalated
}

public enum Trigger
{
    SubmitToTriage,
    DeferNoTimeNow,
    DeferNeedSeparateDecision,
    DeferOtherProduct,
    DeferNeedMoreInfo,
    ResumeFromDefer,
    RouteToNotDefect,
    RouteToWontFix,
    RouteToDuplicate,
    RouteToNotReproducible,
    ConfirmRejectionOk,
    ConfirmRejectionNotOk,
    StartFixing,
    RequestMoreInfoWhileFixing,
    VerifyFixAccepted,
    VerifyFixRejected,
    CompleteReturnToTriage,
    ReopenClosed,
    Escalate,
    Deescalate
}

public sealed class Bug
{
    private readonly StateMachine<State, Trigger> _machine;

    public Bug()
    {
        _machine = new StateMachine<State, Trigger>(() => CurrentState, s => CurrentState = s, FiringMode.Queued);
        CurrentState = State.NewDefect;
        BuildMachine();
    }

    public State CurrentState { get; private set; }

    /// <summary>Сколько раз автомат входил в состояние «Разбор дефектов» (удобно для тестов).</summary>
    public int DefectAnalysisEntryCount { get; private set; }

    public static string GetStateDisplayName(State state) => StateDisplayNames[state];

    public string CurrentStateDisplayName => GetStateDisplayName(CurrentState);

    public bool CanFire(Trigger trigger) => _machine.CanFire(trigger);

    public void Fire(Trigger trigger) => _machine.Fire(trigger);

    public IReadOnlyCollection<Trigger> GetPermittedTriggers() => _machine.PermittedTriggers.ToList().AsReadOnly();

    public string ToDotGraph() => UmlDotGraph.Format(_machine.GetInfo());

    private void BuildMachine()
    {
        void OnEnterAnalysis()
        {
            DefectAnalysisEntryCount++;
        }

        _machine.Configure(State.NewDefect)
            .Permit(Trigger.SubmitToTriage, State.DefectAnalysis);

        _machine.Configure(State.DefectAnalysis)
            .OnEntry(OnEnterAnalysis)
            .Permit(Trigger.DeferNoTimeNow, State.DeferredNoTimeNow)
            .Permit(Trigger.DeferNeedSeparateDecision, State.DeferredNeedSeparateDecision)
            .Permit(Trigger.DeferOtherProduct, State.DeferredOtherProduct)
            .Permit(Trigger.DeferNeedMoreInfo, State.DeferredNeedMoreInfo)
            .Permit(Trigger.RouteToNotDefect, State.RejectedNotDefect)
            .Permit(Trigger.RouteToWontFix, State.RejectedWontFix)
            .Permit(Trigger.RouteToDuplicate, State.RejectedDuplicate)
            .Permit(Trigger.RouteToNotReproducible, State.RejectedNotReproducible)
            .Permit(Trigger.StartFixing, State.Fixing)
            .Permit(Trigger.Escalate, State.Escalated);

        ConfigureDeferred(State.DeferredNoTimeNow);
        ConfigureDeferred(State.DeferredNeedSeparateDecision);
        ConfigureDeferred(State.DeferredOtherProduct);
        ConfigureDeferred(State.DeferredNeedMoreInfo);

        ConfigureRejected(State.RejectedNotDefect);
        ConfigureRejected(State.RejectedWontFix);
        ConfigureRejected(State.RejectedDuplicate);
        ConfigureRejected(State.RejectedNotReproducible);

        _machine.Configure(State.Return)
            .Permit(Trigger.CompleteReturnToTriage, State.DefectAnalysis);

        _machine.Configure(State.Fixing)
            .Permit(Trigger.RequestMoreInfoWhileFixing, State.DeferredNeedMoreInfo)
            .Permit(Trigger.VerifyFixAccepted, State.Closed)
            .Permit(Trigger.VerifyFixRejected, State.Return);

        _machine.Configure(State.Closed)
            .Permit(Trigger.ReopenClosed, State.DefectAnalysis);

        _machine.Configure(State.Escalated)
            .Permit(Trigger.Deescalate, State.DefectAnalysis)
            .Permit(Trigger.StartFixing, State.Fixing);

        void ConfigureDeferred(State deferred) =>
            _machine.Configure(deferred)
                .Permit(Trigger.ResumeFromDefer, State.DefectAnalysis);

        void ConfigureRejected(State rejected) =>
            _machine.Configure(rejected)
                .Permit(Trigger.ConfirmRejectionOk, State.Closed)
                .Permit(Trigger.ConfirmRejectionNotOk, State.Return);
    }

    private static readonly ReadOnlyDictionary<State, string> StateDisplayNames =
        new Dictionary<State, string>
        {
            [State.NewDefect] = "Новый дефект",
            [State.DefectAnalysis] = "Разбор дефектов",
            [State.DeferredNoTimeNow] = "Нет времени сейчас",
            [State.DeferredNeedSeparateDecision] = "Нужно отдельное решение",
            [State.DeferredOtherProduct] = "Проблема другого продукта",
            [State.DeferredNeedMoreInfo] = "Нужно больше информации",
            [State.RejectedNotDefect] = "Не дефект",
            [State.RejectedWontFix] = "Не исправлять",
            [State.RejectedDuplicate] = "Дублирование",
            [State.RejectedNotReproducible] = "Не воспроизводимо",
            [State.Fixing] = "Исправление",
            [State.Return] = "Возврат",
            [State.Closed] = "Закрытие",
            [State.Escalated] = "Эскалация"
        }.AsReadOnly();

    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        var bug = new Bug();
        Console.WriteLine("BugPro — демонстрация конечного автомата дефекта (Stateless).");
        Console.WriteLine($"Начальное состояние: {bug.CurrentStateDisplayName} ({bug.CurrentState})");
        Console.WriteLine($"Допустимые триггеры: {string.Join(", ", bug.GetPermittedTriggers())}");
        bug.Fire(Trigger.SubmitToTriage);
        Console.WriteLine($"После разбора: {bug.CurrentStateDisplayName}; входов в разбор: {bug.DefectAnalysisEntryCount}");
        bug.Fire(Trigger.StartFixing);
        bug.Fire(Trigger.VerifyFixAccepted);
        Console.WriteLine($"После исправления и приёмки: {bug.CurrentStateDisplayName}");
        Console.WriteLine();
        Console.WriteLine("Фрагмент DOT-графа (UML):");
        var dot = bug.ToDotGraph();
        var preview = dot.Length > 800 ? dot[..800] + "\n... (обрезано)" : dot;
        Console.WriteLine(preview);
    }
}