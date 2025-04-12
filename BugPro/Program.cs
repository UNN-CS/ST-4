using System;
using Stateless;

namespace BugPro
{
    // Класс Bug реализует конечный автомат с расширенной моделью состояний.
    public class Bug
    {
        // Определяем состояния бага.
        public enum State
        {
            Open,        // начальное состояние
            Assigned,    // баг назначен исполнителю
            Defered,     // баг отложен
            InProgress,  // работа над багом началась
            Resolved,    // баг решён
            Closed,      // баг закрыт
            Reopened     // баг вновь открыт
        }

        // Определяем возможные триггеры (события).
        private enum Trigger
        {
            Assign,         // назначить баг
            Defer,          // отложить баг
            StartProgress,  // начать работу над багом
            Resolve,        // решить баг
            Close,          // закрыть баг
            Reopen          // вновь открыть баг
        }

        // Используем Stateless для реализации конечного автомата.
        private readonly StateMachine<State, Trigger> sm;

        public Bug(State initialState)
        {
            sm = new StateMachine<State, Trigger>(initialState);

            // Конфигурация для состояния Open
            sm.Configure(State.Open)
              .Permit(Trigger.Assign, State.Assigned)
              .Permit(Trigger.StartProgress, State.InProgress);

            // Конфигурация для состояния Assigned
            sm.Configure(State.Assigned)
              .Permit(Trigger.StartProgress, State.InProgress)
              .Permit(Trigger.Defer, State.Defered)
              .Ignore(Trigger.Assign);

            // Конфигурация для состояния InProgress
            sm.Configure(State.InProgress)
              .Permit(Trigger.Resolve, State.Resolved);

            // Конфигурация для состояния Resolved
            sm.Configure(State.Resolved)
              .Permit(Trigger.Close, State.Closed)
              .Permit(Trigger.Reopen, State.Reopened);

            // Конфигурация для состояния Closed
            sm.Configure(State.Closed)
              .Permit(Trigger.Reopen, State.Reopened);

            // Конфигурация для состояния Defered
            sm.Configure(State.Defered)
              .Permit(Trigger.Assign, State.Assigned);

            // Конфигурация для состояния Reopened
            sm.Configure(State.Reopened)
              .Permit(Trigger.Assign, State.Assigned)
              .Permit(Trigger.StartProgress, State.InProgress);
        }

        // Методы, инициирующие переходы автомата.
        public void Assign()
        {
            sm.Fire(Trigger.Assign);
            Console.WriteLine("Assign");
        }

        public void Defer()
        {
            sm.Fire(Trigger.Defer);
            Console.WriteLine("Defer");
        }

        public void StartProgress()
        {
            sm.Fire(Trigger.StartProgress);
            Console.WriteLine("Start Progress");
        }

        public void Resolve()
        {
            sm.Fire(Trigger.Resolve);
            Console.WriteLine("Resolve");
        }

        public void Close()
        {
            sm.Fire(Trigger.Close);
            Console.WriteLine("Close");
        }

        public void Reopen()
        {
            sm.Fire(Trigger.Reopen);
            Console.WriteLine("Reopen");
        }

        // Метод для получения текущего состояния
        public State GetState()
        {
            return sm.State;
        }
    }

    // Точка входа консольного приложения.
    public class Program
    {
        public static void Main(string[] args)
        {
            // Пример использования автомата Bug
            var bug = new Bug(Bug.State.Open);

            // Демонстрация последовательных переходов
            bug.Assign();           // Open -> Assigned
            bug.StartProgress();    // Assigned -> InProgress
            bug.Resolve();          // InProgress -> Resolved
            bug.Close();            // Resolved -> Closed
            bug.Reopen();           // Closed -> Reopened
            bug.Assign();           // Reopened -> Assigned

            Console.WriteLine("Final State: " + bug.GetState());

            // Для того, чтобы окно консоли не закрывалось сразу
            Console.WriteLine("Нажмите любую клавишу для завершения...");
            Console.ReadKey();
        }
    }
}
