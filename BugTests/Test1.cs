using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using Stateless;
using System;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void OpenToAssigned_ValidTransition()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void OpenToClosed_ValidTransition()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OpenToStart_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Start();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OpenToResolve_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Resolve();
        }

        [TestMethod]
        public void AssignedToInProgress_ValidTransition()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Start();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        public void AssignedToDeferred_ValidTransition()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Deferred, bug.GetState());
        }

        [TestMethod]
        public void AssignedToClosed_ValidTransition()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void AssignedIgnoreAssign_NoStateChange()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AssignedToReopen_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Reopen();
        }

        [TestMethod]
        public void InProgressToResolved_ValidTransition()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Resolve();
            Assert.AreEqual(Bug.State.Resolved, bug.GetState());
        }

        [TestMethod]
        public void InProgressToDeferred_ValidTransition()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Defer();
            Assert.AreEqual(Bug.State.Deferred, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InProgressToClose_InvalidTransition()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Close();
        }

        [TestMethod]
        public void DeferredToAssigned_ValidTransition()
        {
            var bug = new Bug(Bug.State.Deferred);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void DeferredToClosed_ValidTransition()
        {
            var bug = new Bug(Bug.State.Deferred);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeferredToStart_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Deferred);
            bug.Start();
        }

        [TestMethod]
        public void ResolvedToClosed_ValidTransition()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void ResolvedToReopened_ValidTransition()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ResolvedToAssign_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Assign();
        }

        [TestMethod]
        public void ClosedToReopened_ValidTransition()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ClosedToResolve_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Resolve();
        }

        [TestMethod]
        public void ReopenedToAssigned_ValidTransition()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void ReopenedToClosed_ValidTransition()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ReopenedToResolve_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Resolve();
        }

        [TestMethod]
        public void FullWorkflowTest()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
            bug.Start();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
            bug.Resolve();
            Assert.AreEqual(Bug.State.Resolved, bug.GetState());
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
            bug.Defer();
            Assert.AreEqual(Bug.State.Deferred, bug.GetState());
        }
    }
}