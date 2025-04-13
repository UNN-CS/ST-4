using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using System;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        // Тесты для состояния Open
        [TestMethod]
        public void Assign_FromOpen_TransitionsToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Close_FromOpen_ThrowsException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Resolve_FromOpen_ThrowsException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Resolve();
        }

        // Тесты для состояния Assigned
        [TestMethod]
        public void Close_FromAssigned_TransitionsToClosed()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void Defer_FromAssigned_TransitionsToDefered()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.GetState());
        }

        [TestMethod]
        public void Resolve_FromAssigned_TransitionsToResolved()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Resolve();
            Assert.AreEqual(Bug.State.Resolved, bug.GetState());
        }

        [TestMethod]
        public void Assign_FromAssigned_StateRemainsAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign(); // Игнорируемый триггер
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        // Тесты для состояния Defered
        [TestMethod]
        public void Assign_FromDefered_TransitionsToAssigned()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Close_FromDefered_TransitionsToClosed()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Resolve_FromDefered_ThrowsException()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Resolve();
        }

        // Тесты для состояния Closed
        [TestMethod]
        public void Reopen_FromClosed_TransitionsToReopened()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void Assign_FromClosed_TransitionsToAssigned()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Defer_FromClosed_ThrowsException()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Defer();
        }

        // Тесты для состояния Resolved
        [TestMethod]
        public void Close_FromResolved_TransitionsToClosed()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void Defer_FromResolved_TransitionsToDefered()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Assign_FromResolved_ThrowsException()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Assign();
        }

        // Тесты для состояния Reopened
        [TestMethod]
        public void Assign_FromReopened_TransitionsToAssigned()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Defer_FromReopened_TransitionsToDefered()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Close_FromReopened_ThrowsException()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Close();
        }

        // Комбинированные сценарии
        [TestMethod]
        public void Open_Assigned_Closed_Reopened_Assigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Reopen();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }
    }
}