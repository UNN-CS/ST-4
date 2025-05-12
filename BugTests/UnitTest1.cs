using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BugPro;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_Open_To_Assigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Test_Assigned_To_InProgress()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.StartWork();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        public void Test_InProgress_To_Resolved()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Resolve();
            Assert.AreEqual(Bug.State.Resolved, bug.GetState());
        }

        [TestMethod]
        public void Test_Resolved_To_Verified()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Verify();
            Assert.AreEqual(Bug.State.Verified, bug.GetState());
        }

        [TestMethod]
        public void Test_Verified_To_Closed()
        {
            var bug = new Bug(Bug.State.Verified);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void Test_Verified_To_Reopened()
        {
            var bug = new Bug(Bug.State.Verified);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void Test_Closed_To_Reopened()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void Test_Reopened_To_Assigned()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Test_Assigned_Ignore_Assign()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Test_Assigned_To_Deferred()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Deferred, bug.GetState());
        }

        [TestMethod]
        public void Test_Deferred_To_Assigned()
        {
            var bug = new Bug(Bug.State.Deferred);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Test_Open_To_Closed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void Test_Open_To_Rejected()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.GetState());
        }

        [TestMethod]
        public void Test_Assigned_To_Rejected()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.GetState());
        }

        [TestMethod]
        public void Test_InProgress_To_Rejected()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.GetState());
        }

        [TestMethod]
        public void Test_Resolved_To_Rejected()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.GetState());
        }

        [TestMethod]
        public void Test_Verified_To_Rejected()
        {
            var bug = new Bug(Bug.State.Verified);
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.GetState());
        }

        [TestMethod]
        public void Test_Rejected_To_Reopened()
        {
            var bug = new Bug(Bug.State.Rejected);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void Test_Invalid_Transition_Open_Verify()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void Test_Invalid_Transition_Closed_Verify()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void Test_Invalid_Transition_Resolved_StartWork()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.StartWork();
        }
    }
}
