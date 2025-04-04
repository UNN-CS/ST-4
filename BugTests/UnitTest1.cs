using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static Bug;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void TestInitialState()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.GetState());
        }

        [TestMethod]
        public void TestAssignFromOpen()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void TestDeferFromAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.GetState());
        }

        [TestMethod]
        public void TestCloseFromAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void TestAssignFromClosed()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void TestAssignFromDeferred()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void TestIgnoreAssignFromAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign(); // Should be ignored
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestCloseFromOpen()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close(); // Should throw InvalidOperationException
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDeferFromOpen()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Defer(); // Should throw InvalidOperationException
        }

        [TestMethod]
        public void TestReviewFromOpen()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Review();
            Assert.AreEqual(Bug.State.UnderReview, bug.GetState());
        }

        [TestMethod]
        public void TestRejectFromOpen()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.GetState());
        }

        [TestMethod]
        public void TestFixFromAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Fix();
            Assert.AreEqual(Bug.State.Fixed, bug.GetState());
        }

        [TestMethod]
        public void TestReviewFromAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Review();
            Assert.AreEqual(Bug.State.UnderReview, bug.GetState());
        }

        [TestMethod]
        public void TestCloseFromFixed()
        {
            var bug = new Bug(Bug.State.Fixed);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void TestReopenFromFixed()
        {
            var bug = new Bug(Bug.State.Fixed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void TestReopenFromClosed()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void TestAssignFromReopened()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void TestReviewFromReopened()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Review();
            Assert.AreEqual(Bug.State.UnderReview, bug.GetState());
        }

        [TestMethod]
        public void TestFixFromUnderReview()
        {
            var bug = new Bug(Bug.State.UnderReview);
            bug.Fix();
            Assert.AreEqual(Bug.State.Fixed, bug.GetState());
        }

        [TestMethod]
        public void TestAssignFromUnderReview()
        {
            var bug = new Bug(Bug.State.UnderReview);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void TestRejectFromUnderReview()
        {
            var bug = new Bug(Bug.State.UnderReview);
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.GetState());
        }

        [TestMethod]
        public void TestCloseFromRejected()
        {
            var bug = new Bug(Bug.State.Rejected);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void TestReopenFromRejected()
        {
            var bug = new Bug(Bug.State.Rejected);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestFixFromRejected()
        {
            var bug = new Bug(Bug.State.Rejected);
            bug.Fix(); // Should throw InvalidOperationException
        }

        [TestMethod]
        public void TestTitlePropertyInitialization()
        {
            var bug = new Bug(Bug.State.Open, "Test Bug");
            Assert.AreEqual("Test Bug", bug.Title);
        }

        [TestMethod]
        public void TestPriorityPropertyInitialization()
        {
            var bug = new Bug(Bug.State.Open, "Test Bug", 3);
            Assert.AreEqual(3, bug.Priority);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidPrioritySetting()
        {
            var bug = new Bug(Bug.State.Open);
            bug.SetPriority(6); // Should throw ArgumentOutOfRangeException
        }

        [TestMethod]
        public void TestCanAssignMethodPositive()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.IsTrue(bug.CanAssign());
        }

        [TestMethod]
        public void TestCanAssignMethodNegative()
        {
            var bug = new Bug(Bug.State.Fixed);
            Assert.IsFalse(bug.CanAssign());
        }

        [TestMethod]
        public void TestCanCloseMethodPositive()
        {
            var bug = new Bug(Bug.State.Assigned);
            Assert.IsTrue(bug.CanClose());
        }

        [TestMethod]
        public void TestCanCloseMethodNegative()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.IsFalse(bug.CanClose());
        }

        [TestMethod]
        public void TestCloseFromDeferredState()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void TestComplexWorkflow()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.Fix();
            bug.Close();
            bug.Reopen();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }
    }
}
