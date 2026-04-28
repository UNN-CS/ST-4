using Stateless;

namespace BugPro;

// Состояния бага
public enum BugState
{
    Open,           // Открыт
    Assigned,       // Назначен
    InProgress,     // В работе
    Fixed,          // Исправлен
    Verified,       // Проверен
    Closed,         // Закрыт
    Reopened,       // Переоткрыт
    Rejected,       // Отклонен
    NeedMoreInfo    // Требуется больше информации
}

// Действия-триггеры
public enum BugTrigger
{
    Assign,         // Назначить
    StartWork,      // Начать работу
    Fix,            // Исправить
    Verify,         // Проверить
    Close,          // Закрыть
    Reopen,         // Переоткрыть
    Reject,         // Отклонить
    RequestInfo,    // Запросить информацию
    ProvideInfo     // Предоставить информацию
}

public class Bug
{
    private readonly StateMachine<BugState, BugTrigger> _machine;

    public BugState State => _machine.State;
    public string Title { get; set; }
    public string Description { get; set; }

    public Bug(string title, string description)
    {
        Title = title;
        Description = description;

        _machine = new StateMachine<BugState, BugTrigger>(BugState.Open);
        ConfigureMachine();
    }

    private void ConfigureMachine()
    {
        _machine.Configure(BugState.Open)
            .Permit(BugTrigger.Assign, BugState.Assigned)
            .Permit(BugTrigger.Reject, BugState.Rejected)
            .Permit(BugTrigger.RequestInfo, BugState.NeedMoreInfo);

        _machine.Configure(BugState.Assigned)
            .Permit(BugTrigger.StartWork, BugState.InProgress)
            .Permit(BugTrigger.Reject, BugState.Rejected)
            .Permit(BugTrigger.RequestInfo, BugState.NeedMoreInfo);

        _machine.Configure(BugState.InProgress)
            .Permit(BugTrigger.Fix, BugState.Fixed)
            .Permit(BugTrigger.Reject, BugState.Rejected);

        _machine.Configure(BugState.Fixed)
            .Permit(BugTrigger.Verify, BugState.Verified)
            .Permit(BugTrigger.Reopen, BugState.Reopened)
            .Permit(BugTrigger.Reject, BugState.Rejected);

        _machine.Configure(BugState.Verified)
            .Permit(BugTrigger.Close, BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _machine.Configure(BugState.Closed);

        _machine.Configure(BugState.Reopened)
            .Permit(BugTrigger.Assign, BugState.Assigned)
            .Permit(BugTrigger.Reject, BugState.Rejected);

        _machine.Configure(BugState.Rejected)
            .Permit(BugTrigger.Reopen, BugState.Reopened)
            .Permit(BugTrigger.Assign, BugState.Assigned);

        _machine.Configure(BugState.NeedMoreInfo)
            .Permit(BugTrigger.ProvideInfo, BugState.Open);
    }
    public void Assign() => _machine.Fire(BugTrigger.Assign);
    public void StartWork() => _machine.Fire(BugTrigger.StartWork);
    public void Fix() => _machine.Fire(BugTrigger.Fix);
    public void Verify() => _machine.Fire(BugTrigger.Verify);
    public void Close() => _machine.Fire(BugTrigger.Close);
    public void Reopen() => _machine.Fire(BugTrigger.Reopen);
    public void Reject() => _machine.Fire(BugTrigger.Reject);
    public void RequestInfo() => _machine.Fire(BugTrigger.RequestInfo);
    public void ProvideInfo() => _machine.Fire(BugTrigger.ProvideInfo);

    public void PrintStatus()
    {
        Console.WriteLine($"Баг: {Title}");
        Console.WriteLine($"Статус: {State}");
        Console.WriteLine($"Описание: {Description}");
        Console.WriteLine("---");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Система управления багами ===\n");
        
        var bug = new Bug("Ошибка входа", "Пользователь не может войти в систему");
        
        bug.PrintStatus();
        
        bug.Assign();
        Console.WriteLine($"Назначен баг -> {bug.State}");
        
        bug.StartWork();
        Console.WriteLine($"Начата работа -> {bug.State}");
        
        bug.Fix();
        Console.WriteLine($"Исправлен -> {bug.State}");
        
        bug.Verify();
        Console.WriteLine($"Проверен -> {bug.State}");
        
        bug.Close();
        Console.WriteLine($"Закрыт -> {bug.State}");
        
        Console.WriteLine("\nДемонстрация переоткрытия:");
        var reopenedBug = new Bug("Ошибка входа", "Пользователь не может войти в систему");
        reopenedBug.Assign();
        reopenedBug.StartWork();
        reopenedBug.Fix();
        reopenedBug.Reopen();
        Console.WriteLine($"Переоткрыт -> {reopenedBug.State}");
        
        Console.WriteLine("\nДемонстрация запроса информации:");
        var infoBug = new Bug("UI баг", "Кнопка не на месте");
        infoBug.RequestInfo();
        Console.WriteLine($"Запрошена информация -> {infoBug.State}");
        infoBug.ProvideInfo();
        Console.WriteLine($"Информация предоставлена -> {infoBug.State}");
        
        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}
