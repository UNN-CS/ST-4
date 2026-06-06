using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using Stateless;

namespace BugTests
{
    [TestClass]
    public class BugStateMachineTests
    {
        private Bug CreateDefaultBug()
        {
            return new Bug("Test Bug");
        }

        // Тест 1: Проверка начального состояния
        [TestMethod]
        public void NewBug_ShouldStartInOpenState()
        {
            var bug = CreateDefaultBug();
            Assert.AreEqual(State.Open, bug.CurrentState);
        }

        // Тест 2: Open -> Assigned
        [TestMethod]
        public void Assign_FromOpen_ShouldMoveToAssigned()
        {
            var bug = CreateDefaultBug();
            bug.Assign("Developer1");
            Assert.AreEqual(State.Assigned, bug.CurrentState);
            Assert.AreEqual("Developer1", bug.Assignee);
        }

        // Тест 3: Open -> Rejected
        [TestMethod]
        public void Reject_FromOpen_ShouldMoveToRejected()
        {
            var bug = CreateDefaultBug();
            bug.Reject();
            Assert.AreEqual(State.Rejected, bug.CurrentState);
        }

        // Тест 4: Open -> Duplicate
        [TestMethod]
        public void MarkAsDuplicate_FromOpen_ShouldMoveToDuplicate()
        {
            var bug = CreateDefaultBug();
            bug.MarkAsDuplicate();
            Assert.AreEqual(State.Duplicate, bug.CurrentState);
        }

        // Тест 5: Assigned -> InProgress
        [TestMethod]
        public void StartProgress_FromAssigned_ShouldMoveToInProgress()
        {
            var bug = CreateDefaultBug();
            bug.Assign("Dev");
            bug.StartProgress();
            Assert.AreEqual(State.InProgress, bug.CurrentState);
        }

        // Тест 6: Assigned -> Deferred
        [TestMethod]
        public void Defer_FromAssigned_ShouldMoveToDeferred()
        {
            var bug = CreateDefaultBug();
            bug.Assign("Dev");
            bug.Defer();
            Assert.AreEqual(State.Deferred, bug.CurrentState);
        }

        // Тест 7: InProgress -> Fixed
        [TestMethod]
        public void Fix_FromInProgress_ShouldMoveToFixed()
        {
            var bug = CreateDefaultBug();
            bug.Assign("Dev");
            bug.StartProgress();
            bug.Fix();
            Assert.AreEqual(State.Fixed, bug.CurrentState);
        }

        // Тест 8: InProgress -> Deferred
        [TestMethod]
        public void Defer_FromInProgress_ShouldMoveToDeferred()
        {
            var bug = CreateDefaultBug();
            bug.Assign("Dev");
            bug.StartProgress();
            bug.Defer();
            Assert.AreEqual(State.Deferred, bug.CurrentState);
        }

        // Тест 9: Fixed -> Verified
        [TestMethod]
        public void Verify_FromFixed_ShouldMoveToVerified()
        {
            var bug = CreateDefaultBug();
            bug.Assign("Dev");
            bug.StartProgress();
            bug.Fix();
            bug.Verify();
            Assert.AreEqual(State.Verified, bug.CurrentState);
        }

        // Тест 10: Verified -> Closed
        [TestMethod]
        public void Close_FromVerified_ShouldMoveToClosed()
        {
            var bug = CreateDefaultBug();
            bug.Assign("Dev");
            bug.StartProgress();
            bug.Fix();
            bug.Verify();
            bug.Close();
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        // Тест 11: Verified -> Reopened
        [TestMethod]
        public void Reopen_FromVerified_ShouldMoveToReopened()
        {
            var bug = CreateDefaultBug();
            bug.Assign("Dev");
            bug.StartProgress();
            bug.Fix();
            bug.Verify();
            bug.Reopen();
            Assert.AreEqual(State.Reopened, bug.CurrentState);
        }

        // Тест 12: Fixed -> Reopened
        [TestMethod]
        public void Reopen_FromFixed_ShouldMoveToReopened()
        {
            var bug = CreateDefaultBug();
            bug.Assign("Dev");
            bug.StartProgress();
            bug.Fix();
            bug.Reopen();
            Assert.AreEqual(State.Reopened, bug.CurrentState);
        }

        // Тест 13: Closed -> Reopened
        [TestMethod]
        public void Reopen_FromClosed_ShouldMoveToReopened()
        {
            var bug = CreateDefaultBug();
            bug.Assign("Dev");
            bug.StartProgress();
            bug.Fix();
            bug.Verify();
            bug.Close();
            bug.Reopen();
            Assert.AreEqual(State.Reopened, bug.CurrentState);
        }

        // Тест 14: Reopened -> Assigned
        [TestMethod]
        public void Assign_FromReopened_ShouldMoveToAssigned()
        {
            var bug = CreateDefaultBug();
            bug.Assign("Dev");
            bug.StartProgress();
            bug.Fix();
            bug.Reopen();
            bug.Assign("NewDev");
            Assert.AreEqual(State.Assigned, bug.CurrentState);
            Assert.AreEqual("NewDev", bug.Assignee);
        }

        // Тест 15: Reopened -> Deferred
        [TestMethod]
        public void Defer_FromReopened_ShouldMoveToDeferred()
        {
            var bug = CreateDefaultBug();
            bug.Assign("Dev");
            bug.StartProgress();
            bug.Fix();
            bug.Reopen();
            bug.Defer();
            Assert.AreEqual(State.Deferred, bug.CurrentState);
        }

        // Тест 16: Deferred -> Assigned
        [TestMethod]
        public void Assign_FromDeferred_ShouldMoveToAssigned()
        {
            var bug = CreateDefaultBug();
            bug.Assign("Dev");
            bug.Defer();
            bug.Assign("NewDev");
            Assert.AreEqual(State.Assigned, bug.CurrentState);
        }

        // Тест 17: Rejected -> Closed
        [TestMethod]
        public void Close_FromRejected_ShouldMoveToClosed()
        {
            var bug = CreateDefaultBug();
            bug.Reject();
            bug.Close();
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        // Тест 18: Duplicate -> Closed
        [TestMethod]
        public void Close_FromDuplicate_ShouldMoveToClosed()
        {
            var bug = CreateDefaultBug();
            bug.MarkAsDuplicate();
            bug.Close();
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        // Тест 19: Полный жизненный цикл бага
        [TestMethod]
        public void FullLifecycle_ShouldWorkCorrectly()
        {
            var bug = CreateDefaultBug();

            bug.Assign("Dev");
            Assert.AreEqual(State.Assigned, bug.CurrentState);

            bug.StartProgress();
            Assert.AreEqual(State.InProgress, bug.CurrentState);

            bug.Fix();
            Assert.AreEqual(State.Fixed, bug.CurrentState);

            bug.Verify();
            Assert.AreEqual(State.Verified, bug.CurrentState);

            bug.Close();
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        // Тест 20: Проверка исключения - неверный переход из Open
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Fix_FromOpen_ShouldThrowException()
        {
            var bug = CreateDefaultBug();
            bug.Fix(); // Должно выбросить исключение
        }

        // Тест 21: Проверка исключения - неверный переход из Closed
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Assign_FromClosed_ShouldThrowException()
        {
            var bug = CreateDefaultBug();
            bug.Assign("Dev");
            bug.StartProgress();
            bug.Fix();
            bug.Verify();
            bug.Close();
            bug.Assign("NewDev"); // Должно выбросить исключение
        }

        // Тест 22: Проверка исключения - неверный переход из Verified
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Fix_FromVerified_ShouldThrowException()
        {
            var bug = CreateDefaultBug();
            bug.Assign("Dev");
            bug.StartProgress();
            bug.Fix();
            bug.Verify();
            bug.Fix(); // Должно выбросить исключение
        }

        // Тест 23: Тест заголовка бага
        [TestMethod]
        public void Bug_ShouldStoreTitleCorrectly()
        {
            var title = "Critical Security Bug";
            var bug = new Bug(title);
            Assert.AreEqual(title, bug.Title);
        }

        // Тест 24: Несколько циклов Reopen
        [TestMethod]
        public void MultipleReopen_ShouldWorkCorrectly()
        {
            var bug = CreateDefaultBug();

            // Первый цикл
            bug.Assign("Dev1");
            bug.StartProgress();
            bug.Fix();
            bug.Reopen();
            Assert.AreEqual(State.Reopened, bug.CurrentState);

            // Второй цикл
            bug.Assign("Dev2");
            bug.StartProgress();
            bug.Fix();
            bug.Reopen();
            Assert.AreEqual(State.Reopened, bug.CurrentState);
        }

        // Тест 25: Проверка исключения - Verify из Open
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Verify_FromOpen_ShouldThrowException()
        {
            var bug = CreateDefaultBug();
            bug.Verify(); // Должно выбросить исключение
        }
    }
}