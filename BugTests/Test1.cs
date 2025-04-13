using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stateless;
using System;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void NewBug_ShouldBeInOpenState()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.GetState());
        }

        [TestMethod]
        public void OpenBug_CanBeAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void OpenBug_CanBeClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OpenBug_CannotBeDeferred()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Defer();
        }

        [TestMethod]
        public void AssignedBug_CanBeClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void AssignedBug_CanBeDeferred()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.GetState());
        }

        [TestMethod]
        public void AssignedBug_CanBeRejected()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AssignedBug_CannotBeAssignedAgain()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Assign();
        }

        [TestMethod]
        public void ClosedBug_CanBeReopened()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void ClosedBug_CanBeVerified()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Verify();
            Assert.AreEqual(Bug.State.Verified, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ClosedBug_CannotBeClosedAgain()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Close();
        }

        [TestMethod]
        public void DeferedBug_CanBeAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void DeferedBug_CanBeClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void ReopenedBug_CanBeAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Reopen();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void ReopenedBug_CanBeClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Reopen();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void VerifiedBug_CanBeReopened()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Verify();
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void VerifiedBug_CanBeClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Verify();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void RejectedBug_CanBeAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Reject();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void RejectedBug_CanBeClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Reject();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void VerifiedBug_CannotBeAssignedDirectly()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Verify();
            bug.Assign();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ReopenedBug_CannotBeDeferred()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Reopen();
            bug.Defer();
        }

        [TestMethod]
        public void ComplexWorkflow_ShouldEndInVerifiedState()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Reject();
            bug.Assign();
            bug.Close();
            bug.Verify();
            Assert.AreEqual(Bug.State.Verified, bug.GetState());
        }

        [TestMethod]
        public void ComplexWorkflowWithReopen_ShouldEndInClosedState()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Reopen();
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }
    }
}