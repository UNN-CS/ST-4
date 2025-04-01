using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        private Bug bug;

        [TestInitialize]
        public void Setup()
        {
            // Инициализируем баг в состоянии Open для каждого теста
            bug = new Bug(Bug.State.Open);
        }

        // Проверка корректных переходов
        [TestMethod]
        public void TestAssignStateChange()
        {
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestCloseStateChange()
        {
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void TestDeferStateChange()
        {
            bug.Assign();
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void TestReopenAfterClosed()
        {
            bug.Assign();
            bug.Close();
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.getState());
        }

        [TestMethod]
        public void TestStartWorkFromAssigned()
        {
            bug.Assign();
            bug.StartWork();
            Assert.AreEqual(Bug.State.InProgress, bug.getState());
        }

        // Тесты, проверяющие выбрасывание исключений при недопустимых переходах

        // Попытка выполнить Close из состояния Open (переход не разрешён)
        [TestMethod]
        public void TestInvalidTransition_CloseFromOpen()
        {
            Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
        }

        // Попытка выполнить Reopen из состояния Open (переход не разрешён)
        [TestMethod]
        public void TestInvalidReopenFromOpen()
        {
            Assert.ThrowsException<InvalidOperationException>(() => bug.Reopen());
        }

        // Попытка выполнить Defer из состояния Closed
        [TestMethod]
        public void TestDeferFromClosed()
        {
            bug.Assign();
            bug.Close();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
        }

        // Из Assigned выполняем Reject, а затем пытаемся выполнить Assign — переход из Rejected должен быть недопустим
        [TestMethod]
        public void TestInvalidAssignFromRejected()
        {
            bug.Assign();
            bug.Reject();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Assign());
        }

        // Из Assigned выполняем Reject, проверяем, что состояние действительно Rejected
        [TestMethod]
        public void TestRejectStateChange()
        {
            bug.Assign();
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.getState());
        }

        // Если баг уже Rejected, попытка выполнить любое действие должна выбрасывать исключение.
        [TestMethod]
        public void TestAnyActionFromRejected()
        {
            bug.Assign();
            bug.Reject();
            Assert.ThrowsException<InvalidOperationException>(() => bug.StartWork());
            Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
            Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
            Assert.ThrowsException<InvalidOperationException>(() => bug.Reopen());
        }

        // Из Defered — успешный переход через Reopen (если разрешён)
        [TestMethod]
        public void TestReopenAfterDefer()
        {
            bug.Assign();
            bug.Defer();
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.getState());
        }

        // Если баг уже Reopened, повторный вызов Reopen должен выбрасывать исключение
        [TestMethod]
        public void TestMultipleReopens()
        {
            bug.Assign();
            bug.Close();
            bug.Reopen();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Reopen());
        }

        // Проверка перехода из Defered в Closed через триггер Close (если такой переход разрешён)
        [TestMethod]
        public void TestDeferAndClose()
        {
            bug.Assign();
            bug.Defer();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        // Из Reopened переход в InProgress через StartWork
        [TestMethod]
        public void TestStartWorkFromReopened()
        {
            bug.Assign();
            bug.Close();
            bug.Reopen();
            bug.StartWork();
            Assert.AreEqual(Bug.State.InProgress, bug.getState());
        }

        // Из InProgress переход в Closed через Close
        [TestMethod]
        public void TestCloseFromInProgress()
        {
            bug.Assign();
            bug.StartWork();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        // Из InProgress переход в Defered через Defer
        [TestMethod]
        public void TestDeferFromInProgress()
        {
            bug.Assign();
            bug.StartWork();
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        // Попытка выполнить StartWork из состояния Closed должна вызвать исключение
        [TestMethod]
        public void TestInvalidStartWorkFromClosed()
        {
            bug.Assign();
            bug.Close();
            Assert.ThrowsException<InvalidOperationException>(() => bug.StartWork());
        }

        // Попытка выполнить Reject из состояния Open должна вызвать исключение
        [TestMethod]
        public void TestInvalidRejectFromOpen()
        {
            Assert.ThrowsException<InvalidOperationException>(() => bug.Reject());
        }

        // Из Defered, переход Assign должен вернуть баг в Assigned
        [TestMethod]
        public void TestAssignFromDefered()
        {
            bug.Assign();
            bug.Defer();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }
    }
}
