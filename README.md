# ST-4 Модульное тестирование в .NET Core с использованием MSTest (C#) и библиотеки Stateless


![GitHub pull requests](https://img.shields.io/github/issues-pr/UNN-CS/ST-4)
![GitHub closed pull requests](https://img.shields.io/github/issues-pr-closed/UNN-CS/ST-4)

Срок выполнения задания:

**по 13.04.2025** ![Relative date](https://img.shields.io/date/1744578000)


Данная работа демонстрирует модульное тестирование с помощью фреймворка MSTest на платформе .NET версии 9.x
Для выполнения работы необходимо добавить ссылку на пакет **Stateless** .NET.

## Задание №1

В рамках существующего решения (solution) **ST-4** добавить проект **BugPro** в виде консольного приложения и поместить в файл 
**BugPro/Program.cs** код класса **Bug**.

```csharp
using Stateless;

public class Bug {
   public enum State {Open, Assigned, Defered, Closed}
   private enum Trigger {Assign, Defer, Close}
   private StateMachine<State, Trigger> sm;

   public Bug(State state) {
      sm = new StateMachine<State, Trigger>(state);
      sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned);
      sm.Configure(State.Assigned)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Defer, State.Defered)
            .Ignore(Trigger.Assign);
      sm.Configure(State.Closed)
            .Permit(Trigger.Assign, State.Assigned);
      sm.Configure(State.Defered)
            .Permit(Trigger.Assign, State.Assigned); 
   }
   public void Close() {
      sm.Fire(Trigger.Close);
      Console.WriteLine("Close");
   }
   public void Assign() {
      sm.Fire(Trigger.Assign);
      Console.WriteLine("Assign");   
   }
   public void Defer() {
      sm.Fire(Trigger.Defer);
      Console.WriteLine("Defer");   
   }   
   public State getState() {
      return sm.State;
   }
}

public class Program {
   public static void Main(string[] args) {
      var bug = new Bug(Bug.State.Open);
      bug.Assign();
      bug.Close();
      bug.Assign();
      bug.Defer();
      bug.Assign();
      Console.WriteLine(bug.getState());
   }
}
```

Расширить данный проект, добавив в workflow бага новые состояния и переходы.

Далее, построить проект и убедиться, что консольное приложение запускается без ошибок и выводит на экран некоторую информацию.

## Задание №2

Добавить в решение еще один проект **BugTests**, указав тип проекта - **MSTest**. В файл **BugTests/UnitTest1.cs** поместить несколько тестовых методов, тестирующих класс-автомат, описанный в **BugPro/Program.cs**. Количество тестов - не менее 20.

Для реализации бОльшего числа тестов, рекомендуется расширить класс **Bug**, добавив новые состояния и новые функции переходов. Обязательно должны присутствовать тесты, отлавливающие выбрасываемые **Stateless** исключения! 

## Примечание

После создания пул-запроса убедиться, что тесты корректно запускаются. Для этого нужно зайти в журнал GH Actions и убедиться, что все тесты запущены и успешно выполнены.





