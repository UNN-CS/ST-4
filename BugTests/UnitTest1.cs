using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        // Тест проверки начального состояния
        [TestMethod]
        public void TestInitialState()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.GetState());
        }

        // Корректный переход: Open -> Assigned
        [TestMethod]
        public void TestOpenToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        // Корректный переход: Open -> InProgress
        [TestMethod]
        public void TestOpenToInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        // Корректный переход: Assigned -> InProgress
        [TestMethod]
        public void TestAssignedToInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();         // Open -> Assigned
            bug.StartProgress();  // Assigned -> InProgress
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        // Корректный переход: Assigned -> Defered
        [TestMethod]
        public void TestAssignedToDefered()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();  // Open -> Assigned
            bug.Defer();   // Assigned -> Defered
            Assert.AreEqual(Bug.State.Defered, bug.GetState());
        }

        // Корректный переход: Defered -> Assigned
        [TestMethod]
        public void TestDeferedToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();  // Open -> Assigned
            bug.Defer();   // Assigned -> Defered
            bug.Assign();  // Defered -> Assigned
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        // Корректный переход: InProgress -> Resolved
        [TestMethod]
        public void TestInProgressToResolved()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();  // Open -> InProgress
            bug.Resolve();        // InProgress -> Resolved
            Assert.AreEqual(Bug.State.Resolved, bug.GetState());
        }

        // Корректный переход: Resolved -> Closed
        [TestMethod]
        public void TestResolvedToClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();  // Open -> InProgress
            bug.Resolve();        // InProgress -> Resolved
            bug.Close();          // Resolved -> Closed
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        // Корректный переход: Resolved -> Reopened
        [TestMethod]
        public void TestResolvedToReopened()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();  // Open -> InProgress
            bug.Resolve();        // InProgress -> Resolved
            bug.Reopen();         // Resolved -> Reopened
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        // Корректный переход: Closed -> Reopened
        [TestMethod]
        public void TestClosedToReopened()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();  // Open -> InProgress
            bug.Resolve();        // InProgress -> Resolved
            bug.Close();          // Resolved -> Closed
            bug.Reopen();         // Closed -> Reopened
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        // Корректный переход: Reopened -> Assigned
        [TestMethod]
        public void TestReopenedToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            bug.Reopen();         // Closed -> Reopened
            bug.Assign();         // Reopened -> Assigned
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        // Корректный переход: Reopened -> InProgress
        [TestMethod]
        public void TestReopenedToInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            bug.Reopen();         // Closed -> Reopened
            bug.StartProgress();  // Reopened -> InProgress
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        // Сценарий последовательных переходов
        [TestMethod]
        public void TestMultipleTransitions1()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();           // Open -> Assigned
            bug.StartProgress();    // Assigned -> InProgress
            bug.Resolve();          // InProgress -> Resolved
            bug.Reopen();           // Resolved -> Reopened
            bug.Assign();           // Reopened -> Assigned
            bug.StartProgress();    // Assigned -> InProgress
            bug.Resolve();          // InProgress -> Resolved
            bug.Close();            // Resolved -> Closed
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        // Тест: повторный вызов Assign в состоянии Assigned (trigger игнорируется)
        [TestMethod]
        public void TestIgnoreTriggerAssign()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();   // Open -> Assigned
            bug.Assign();   // В состоянии Assigned trigger Assign игнорируется
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        // Попытка выполнения недопустимого перехода: вызов Resolve из состояния Assigned
        [TestMethod]
        public void TestInvalidTransitionAssignedToResolve()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();  // Open -> Assigned

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Resolve();  // Для Assigned переход Resolve не настроен
            });
        }

        // Попытка выполнения недопустимого перехода: вызов Close из состояния Assigned
        [TestMethod]
        public void TestInvalidTransitionAssignedToClose()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();  // Open -> Assigned

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Close();  // Для Assigned переход Close не настроен
            });
        }

        // Попытка выполнения недопустимого перехода: вызов Reopen из состояния Open
        [TestMethod]
        public void TestInvalidTransitionOpenToReopen()
        {
            var bug = new Bug(Bug.State.Open);

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Reopen(); // В состоянии Open нет перехода Reopen
            });
        }

        // Тест последовательности: Open -> Assigned -> Defered -> Assigned -> InProgress -> Resolved -> Reopen -> InProgress
        [TestMethod]
        public void TestComplexTransitionSequence()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();           // Open -> Assigned
            bug.Defer();            // Assigned -> Defered
            bug.Assign();           // Defered -> Assigned
            bug.StartProgress();    // Assigned -> InProgress
            bug.Resolve();          // InProgress -> Resolved
            bug.Reopen();           // Resolved -> Reopened
            bug.StartProgress();    // Reopened -> InProgress
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        // Тест: Попытка выполнить Defer в неподходящем состоянии (например, в состоянии InProgress)
        [TestMethod]
        public void TestInvalidDeferFromInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();  // Open -> InProgress

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Defer();    // Для InProgress переход Defer не определён
            });
        }

        // Тест: Выполнение нескольких корректных переходов последовательно без ошибок
        [TestMethod]
        public void TestMultipleValidTransitions()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();         // Open -> Assigned
            bug.StartProgress();  // Assigned -> InProgress
            bug.Resolve();        // InProgress -> Resolved
            bug.Reopen();         // Resolved -> Reopened
            bug.StartProgress();  // Reopened -> InProgress
            bug.Resolve();        // InProgress -> Resolved
            bug.Close();          // Resolved -> Closed
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        // Тест: Проверка, что повторный вызов Trigger, не приводящий к переходу, не меняет состояние
        [TestMethod]
        public void TestRepeatIgnoredTrigger()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign(); // Open -> Assigned
            // Вызов Assign дважды (второй игнорируется)
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        // Тест: Неправильный порядок переходов должен выбрасывать исключение (например, Attempt Reopen из Assigned)
        [TestMethod]
        public void TestInvalidReopenFromAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign(); // Open -> Assigned

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Reopen(); // В состоянии Assigned нет перехода Reopen
            });
        }

        // Тест: Последовательность без ошибок при переходах от Reopened
        [TestMethod]
        public void TestReopenedTransitionPaths()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();         // Open -> Assigned
            bug.Defer();          // Assigned -> Defered
            bug.Assign();         // Defered -> Assigned
            bug.StartProgress();  // Assigned -> InProgress
            bug.Resolve();        // InProgress -> Resolved
            bug.Close();          // Resolved -> Closed
            bug.Reopen();         // Closed -> Reopened

            // Из Reopened возможны два перехода. Тестируем оба.
            bug.Assign();         // Reopened -> Assigned
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());

            // Создадим новый баг и перейдём через Reopened -> InProgress
            bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            bug.Reopen();
            bug.StartProgress();  // Reopened -> InProgress
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }
    }
}
