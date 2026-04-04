using System;
using Stateless;

namespace BugPro;

public enum State
{
    New,                
    Triage,             
    InDevelopment,      
    NotADefect,         
    WontFix,            
    Duplicate,          
    CannotReproduce,    
    ReadyForTesting,    
    Closed,             
    Archived            
}

public enum Trigger
{
    StartTriage,        
    SendToDev,       
    MarkNotADefect,     
    MarkWontFix,        
    MarkDuplicate,   

    DeferNoTime,            
    DeferSeparateDecision,  
    DeferOtherProduct,      
    RequestMoreInfo,        

    ProvideFix,         
    MarkCannotReproduce,
    VerifyOk,           
    VerifyFailed,       
    Reopen,             
    Archive             
}

public class Bug
{
    private readonly StateMachine<State, Trigger> _machine;

    public State CurrentState => _machine.State;

    public Bug()
    {
        _machine = new StateMachine<State, Trigger>(State.New);

        _machine.Configure(State.New)
            .Permit(Trigger.StartTriage, State.Triage);

        _machine.Configure(State.Triage)
            .Permit(Trigger.SendToDev, State.InDevelopment)
            .Permit(Trigger.MarkNotADefect, State.NotADefect)
            .Permit(Trigger.MarkWontFix, State.WontFix)
            .Permit(Trigger.MarkDuplicate, State.Duplicate);

        _machine.Configure(State.InDevelopment)
            .Permit(Trigger.ProvideFix, State.ReadyForTesting)
            .Permit(Trigger.MarkCannotReproduce, State.CannotReproduce)
            .Permit(Trigger.DeferNoTime, State.Triage)
            .Permit(Trigger.DeferSeparateDecision, State.Triage)
            .Permit(Trigger.DeferOtherProduct, State.Triage)
            .Permit(Trigger.RequestMoreInfo, State.Triage);

        _machine.Configure(State.ReadyForTesting)
            .Permit(Trigger.VerifyOk, State.Closed)
            .Permit(Trigger.VerifyFailed, State.Triage); 

        _machine.Configure(State.CannotReproduce)
            .Permit(Trigger.VerifyOk, State.Closed)
            .Permit(Trigger.VerifyFailed, State.Triage); 

        _machine.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Triage) 
            .Permit(Trigger.Archive, State.Archived);

        _machine.Configure(State.Archived)
            .Ignore(Trigger.Archive);
    }

    public void Fire(Trigger trigger)
    {
        _machine.Fire(trigger);
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Запуск симуляции WorkFlow бага ===\n");
        var bug = new Bug();
        Console.WriteLine($"[1] Создан дефект. Текущее состояние: {bug.CurrentState}");

        bug.Fire(Trigger.StartTriage);
        Console.WriteLine($"[2] Взят в работу. Текущее состояние: {bug.CurrentState}");

        bug.Fire(Trigger.SendToDev);
        Console.WriteLine($"[3] Назначен разработчику. Текущее состояние: {bug.CurrentState}");

        bug.Fire(Trigger.ProvideFix);
        Console.WriteLine($"[4] Исправлен, ожидает проверки. Текущее состояние: {bug.CurrentState}");

        bug.Fire(Trigger.VerifyOk);
        Console.WriteLine($"[5] Проверен и закрыт. Текущее состояние: {bug.CurrentState}");

        bug.Fire(Trigger.Archive);
        Console.WriteLine($"[6] Перемещен в архив. Текущее состояние: {bug.CurrentState}");

        Console.WriteLine("\nПроцесс завершен без ошибок.");
    }
}