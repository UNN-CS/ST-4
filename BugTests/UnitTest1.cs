using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stateless;

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
        public void Assign_FromOpen_ShouldChangeToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Reject_FromOpen_ShouldChangeToRejected()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Close_FromOpen_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
        }

        [TestMethod]
        public void Close_FromAssigned_ShouldChangeToClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void Defer_FromAssigned_ShouldChangeToDeferred()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            Assert.AreEqual(Bug.State.Deferred, bug.GetState());
        }

        [TestMethod]
        public void Reject_FromAssigned_ShouldChangeToRejected()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.GetState());
        }

        [TestMethod]
        public void Assign_FromDeferred_ShouldChangeToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Close_FromDeferred_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Close();
        }
        [TestMethod]
        public void Reopen_FromClosed_ShouldChangeToReopened()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void Verify_FromClosed_ShouldChangeToVerified()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Verify();
            Assert.AreEqual(Bug.State.Verified, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Assign_FromClosed_WithoutReopen_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Assign();
        }

        [TestMethod]
        public void Assign_FromReopened_ShouldChangeToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Reopen();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Reject_FromReopened_ShouldChangeToRejected()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Reopen();
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Close_FromReopened_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Reopen();
            bug.Close();
        }

        [TestMethod]
        public void Reopen_FromVerified_ShouldChangeToReopened()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Verify();
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Assign_FromVerified_WithoutReopen_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Verify();
            bug.Assign();
        }

        [TestMethod]
        public void Assign_FromRejected_ShouldChangeToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Reject();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Close_FromRejected_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Reject();
            bug.Close();
        }

        [TestMethod]
        public void MultipleAssign_ShouldBeIgnored()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Assign(); // Should be ignored
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void ComplexWorkflow_ShouldEndInVerifiedState()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.Close();
            bug.Verify();
            Assert.AreEqual(Bug.State.Verified, bug.GetState());
        }

        [TestMethod]
        public void ComplexWorkflowWithReject_ShouldEndInAssignedState()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Reject();
            bug.Assign();
            bug.Close();
            bug.Reopen();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Verify_FromOpen_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Reopen_FromOpen_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Reopen();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Defer_FromOpen_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Defer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Verify_FromAssigned_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Reopen_FromAssigned_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Reopen();
        }

        [TestMethod]
        public void FullLifecycle_ShouldWorkCorrectly()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.GetState());
            
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
            
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
            
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
            
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
            
            bug.Defer();
            Assert.AreEqual(Bug.State.Deferred, bug.GetState());
            
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
            
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
            
            bug.Verify();
            Assert.AreEqual(Bug.State.Verified, bug.GetState());
        }
    }
}