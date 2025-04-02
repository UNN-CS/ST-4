using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void Test_OpenToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Test_OpenToInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            Assert.AreEqual(Bug.State.InProgress, bug.getState());
        }

        [TestMethod]
        public void Test_AssignedToClosed()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Test_AssignedToDeferred()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Deferred, bug.getState());
        }

        [TestMethod]
        public void Test_InProgressToClosed()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Test_InProgressToDeferred()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Defer();
            Assert.AreEqual(Bug.State.Deferred, bug.getState());
        }

        [TestMethod]
        public void Test_DeferredToAssigned()
        {
            var bug = new Bug(Bug.State.Deferred);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Test_ClosedToAssigned()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Test_ClosedToReopened()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.getState());
        }

        [TestMethod]
        public void Test_ReopenedToAssigned()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Test_ReopenedToInProgress()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.StartProgress();
            Assert.AreEqual(Bug.State.InProgress, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_InvalidTransition_OpenToDefer()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Defer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_InvalidTransition_OpenToClose()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_InvalidTransition_OpenToReopen()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Reopen();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_InvalidTransition_AssignedToStartProgress()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.StartProgress();
        }

        [TestMethod]
        public void Test_MultipleTransitions_1()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Test_MultipleTransitions_2()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.Defer();
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_InvalidTransition_DeferredToStartProgress()
        {
            var bug = new Bug(Bug.State.Deferred);
            bug.StartProgress();
        }

        [TestMethod]
        public void Test_CycleTransitions()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            bug.Assign();
            bug.Close();
            bug.Reopen();
            bug.StartProgress();
            Assert.AreEqual(Bug.State.InProgress, bug.getState());
        }

        [TestMethod]
        public void Test_IgnoreTransition_AssignedAssign()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Test_IgnoreTransition_ReopenedAssignTwice()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_InvalidTransition_ClosedToDefer()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Defer();
        }
    }
}
