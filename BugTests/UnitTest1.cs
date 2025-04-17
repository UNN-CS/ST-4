using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void OpenToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void AssignedToClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void AssignedToDefered()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.GetState());
        }

        [TestMethod]
        public void DeferedToAssigned()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void ClosedToReopened()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void ReopenedToAssigned()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void AssignedToVerified()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Verify();
            Assert.AreEqual(Bug.State.Verified, bug.GetState());
        }

        [TestMethod]
        public void VerifiedToReopenedViaDeny()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Verify();
            bug.Deny();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotDeferFromOpen()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Defer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotCloseFromOpen()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotAssignFromVerified()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Verify();
            bug.Assign();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotVerifyFromOpen()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Verify();
        }

        [TestMethod]
        public void IgnoreAssignWhenAlreadyAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DoubleReopenThrowsException()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            bug.Reopen();
        }

        [TestMethod]
        public void FullLifecycleToVerified()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Verify();
            Assert.AreEqual(Bug.State.Verified, bug.GetState());
        }

        [TestMethod]
        public void DenyAndReassign()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Verify();
            bug.Deny();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void DeferAssignVerifyDeny()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.Verify();
            bug.Deny();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void DirectReopenAssignClose()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void VerifyToDenyToVerify()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Verify();
            bug.Deny();
            bug.Assign();
            bug.Verify();
            Assert.AreEqual(Bug.State.Verified, bug.GetState());
        }

        [TestMethod]
        public void DeferAndCloseNotAllowed()
        {
            var bug = new Bug(Bug.State.Defered);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
        }
    }
}
