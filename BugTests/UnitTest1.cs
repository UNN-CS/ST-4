using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestInitialState()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.getState());
        }

        [TestMethod]
        public void TestOpenToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestAssignedToClosed()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void TestAssignedToDeferred()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void TestClosedToAssigned()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestDeferredToAssigned()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestAssignedToInProgress()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Start();
            Assert.AreEqual(Bug.State.InProgress, bug.getState());
        }

        [TestMethod]
        public void TestInProgressToVerified()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Verify();
            Assert.AreEqual(Bug.State.Verified, bug.getState());
        }

        [TestMethod]
        public void TestVerifiedToRejected()
        {
            var bug = new Bug(Bug.State.Verified);
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.getState());
        }

        [TestMethod]
        public void TestRejectedToAssigned()
        {
            var bug = new Bug(Bug.State.Rejected);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestClosedToReOpen()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.ReOpen();
            Assert.AreEqual(Bug.State.Open, bug.getState());
        }

        [TestMethod]
        public void TestOpenToNeedMoreInfo()
        {
            var bug = new Bug(Bug.State.Open);
            bug.NeedMoreInfo();
            Assert.AreEqual(Bug.State.NeedInfo, bug.getState());
        }

        [TestMethod]
        public void TestNeedInfoToProvideInfo()
        {
            var bug = new Bug(Bug.State.NeedInfo);
            bug.ProvideInfo();
            Assert.AreEqual(Bug.State.Open, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInvalidTransitionOpenToClose()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInvalidTransitionOpenToVerify()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInvalidTransitionRejectedToVerify()
        {
            var bug = new Bug(Bug.State.Rejected);
            bug.Verify();
        }

        [TestMethod]
        public void TestMultipleTransitions()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Start();
            bug.Verify();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void TestCanCloseFromInProgress()
        {
            var bug = new Bug(Bug.State.InProgress);
            Assert.IsTrue(bug.CanClose());
        }

        [TestMethod]
        public void TestCannotCloseFromOpen()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.IsFalse(bug.CanClose());
        }

        [TestMethod]
        public void TestCanReOpenFromClosed()
        {
            var bug = new Bug(Bug.State.Closed);
            Assert.IsTrue(bug.CanReOpen());
        }

        [TestMethod]
        public void TestCannotReOpenFromAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            Assert.IsFalse(bug.CanReOpen());
        }

        [TestMethod]
        public void TestIgnoreAssignFromAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestComplexWorkflow()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Start();
            bug.Verify();
            bug.Reject();
            bug.Start();
            bug.Verify();
            bug.Close();
            bug.ReOpen();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }
    }
}