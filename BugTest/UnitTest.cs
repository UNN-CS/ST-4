// BugTest/UnitTest.cs
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using Stateless;

namespace BugTest
{
    [TestClass]
    public class BugStateTests
    {
        [TestMethod]
        public void TestInitialState()
        {
            var bug = new Bug();
            Assert.AreEqual(BugState.New, bug.CurrentState);
        }

        [TestMethod]
        public void TestAssign()
        {
            var bug = new Bug();
            bug.Assign("Developer Alice");
            Assert.AreEqual(BugState.Assigned, bug.CurrentState);
        }

        [TestMethod]
        public void TestAssignedName()
        {
            var bug = new Bug();
            string expectedDev = "Developer Bob";
            bug.Assign(expectedDev);
            Assert.AreEqual(expectedDev, bug.AssignedTo);
        }

        [TestMethod]
        public void TestStartWork()
        {
            var bug = new Bug();
            bug.Assign("Developer Alice");
            bug.StartWork();
            Assert.AreEqual(BugState.Active, bug.CurrentState);
        }

        [TestMethod]
        public void TestResolve()
        {
            var bug = new Bug();
            bug.Assign("Developer Alice");
            bug.StartWork();
            bug.Resolve();
            Assert.AreEqual(BugState.Resolved, bug.CurrentState);
        }

        [TestMethod]
        public void TestStartReview()
        {
            var bug = new Bug();
            bug.Assign("Developer Alice");
            bug.StartWork();
            bug.Resolve();
            bug.StartReview();
            Assert.AreEqual(BugState.UnderReview, bug.CurrentState);
        }

        [TestMethod]
        public void TestClose()
        {
            var bug = new Bug();
            bug.Assign("Developer Alice");
            bug.StartWork();
            bug.Resolve();
            bug.StartReview();
            bug.Close();
            Assert.AreEqual(BugState.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void TestReopenFromClosed()
        {
            var bug = new Bug();
            bug.Assign("Developer Alice");
            bug.StartWork();
            bug.Resolve();
            bug.StartReview();
            bug.Close();
            bug.Reopen();
            Assert.AreEqual(BugState.Reopened, bug.CurrentState);
        }

        [TestMethod]
        public void TestReopenFromReview()
        {
            var bug = new Bug();
            bug.Assign("Developer Alice");
            bug.StartWork();
            bug.Resolve();
            bug.StartReview();
            bug.Reopen();
            Assert.AreEqual(BugState.Reopened, bug.CurrentState);
        }

        [TestMethod]
        public void TestAssignAfterReopen()
        {
            var bug = new Bug();
            bug.Assign("Developer Alice");
            bug.StartWork();
            bug.Resolve();
            bug.StartReview();
            bug.Close();
            bug.Reopen();
            bug.Assign("Developer Charlie");
            Assert.AreEqual(BugState.Assigned, bug.CurrentState);
        }

        [TestMethod]
        public void TestDeferFromNew()
        {
            var bug = new Bug();
            bug.Defer();
            Assert.AreEqual(BugState.Deferred, bug.CurrentState);
        }

        [TestMethod]
        public void TestDeferFromAssigned()
        {
            var bug = new Bug();
            bug.Assign("Developer Alice");
            bug.Defer();
            Assert.AreEqual(BugState.Deferred, bug.CurrentState);
        }

        [TestMethod]
        public void TestDeferFromActive()
        {
            var bug = new Bug();
            bug.Assign("Developer Alice");
            bug.StartWork();
            bug.Defer();
            Assert.AreEqual(BugState.Deferred, bug.CurrentState);
        }

        [TestMethod]
        public void TestAssignFromDeferred()
        {
            var bug = new Bug();
            bug.Defer();
            bug.Assign("Developer Bob");
            Assert.AreEqual(BugState.New, bug.CurrentState);
        }

        [TestMethod]
        public void TestSubmitDetailsFromDeferred()
        {
            var bug = new Bug();
            bug.Defer();
            bug.SubmitDetails();
            Assert.AreEqual(BugState.New, bug.CurrentState);
        }

        [TestMethod]
        public void TestMarkDuplicate()
        {
            var bug = new Bug();
            bug.MarkDuplicate();
            Assert.AreEqual(BugState.Duplicate, bug.CurrentState);
        }

        [TestMethod]
        public void TestCloseDuplicate()
        {
            var bug = new Bug();
            bug.MarkDuplicate();
            bug.Close();
            Assert.AreEqual(BugState.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void TestAskForDetailsFromNew()
        {
            var bug = new Bug();
            bug.AskForDetails();
            Assert.AreEqual(BugState.InfoRequested, bug.CurrentState);
        }

        [TestMethod]
        public void TestAskForDetailsFromAssigned()
        {
            var bug = new Bug();
            bug.Assign("Developer Alice");
            bug.AskForDetails();
            Assert.AreEqual(BugState.InfoRequested, bug.CurrentState);
        }

        [TestMethod]
        public void TestSubmitDetails()
        {
            var bug = new Bug();
            bug.AskForDetails();
            bug.SubmitDetails();
            Assert.AreEqual(BugState.New, bug.CurrentState);
        }

        [TestMethod]
        public void TestDeferFromInfo()
        {
            var bug = new Bug();
            bug.AskForDetails();
            bug.Defer();
            Assert.AreEqual(BugState.Deferred, bug.CurrentState);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInvalidStartWork()
        {
            var bug = new Bug();
            bug.StartWork();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInvalidResolve()
        {
            var bug = new Bug();
            bug.Assign("Developer Alice");
            bug.Resolve();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInvalidStartReview()
        {
            var bug = new Bug();
            bug.Assign("Developer Alice");
            bug.StartWork();
            bug.StartReview();
        }
    }
}
