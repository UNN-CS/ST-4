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
        public void StartProgress_FromOpen_ShouldChangeToInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        public void MarkAsDone_FromInProgress_ShouldChangeToInReview()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.MarkAsDone();
            Assert.AreEqual(Bug.State.InReview, bug.GetState());
        }

        [TestMethod]
        public void Close_FromInReview_ShouldChangeToClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.MarkAsDone();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void Verify_FromClosed_ShouldChangeToVerified()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.MarkAsDone();
            bug.Close();
            bug.Verify();
            Assert.AreEqual(Bug.State.Verified, bug.GetState());
        }

        [TestMethod]
        public void Reopen_FromVerified_ShouldChangeToReopened()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.MarkAsDone();
            bug.Close();
            bug.Verify();
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Close_FromOpen_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MarkAsDone_FromOpen_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.MarkAsDone();
        }

        [TestMethod]
        public void Reject_FromOpen_ShouldChangeToRejected()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.GetState());
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
        public void Defer_FromAssigned_ShouldChangeToDeferred()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            Assert.AreEqual(Bug.State.Deferred, bug.GetState());
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
        public void Reject_FromAssigned_ShouldChangeToRejected()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.GetState());
        }

        [TestMethod]
        public void Reject_FromInProgress_ShouldChangeToRejected()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.GetState());
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
        public void ComplexWorkflow1_ShouldWorkCorrectly()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.MarkAsDone();
            bug.Close();
            bug.Verify();
            Assert.AreEqual(Bug.State.Verified, bug.GetState());
        }

        [TestMethod]
        public void ComplexWorkflow2_ShouldWorkCorrectly()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.StartProgress();
            bug.MarkAsDone();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidTransition_DeferredToInProgress_ShouldThrow()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.StartProgress();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidTransition_ReopenedToInProgress_ShouldThrow()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Reopen();
            bug.StartProgress();
        }

        [TestMethod]
        public void FullLifecycle_ShouldWorkCorrectly()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.GetState());

            bug.StartProgress();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());

            bug.MarkAsDone();
            Assert.AreEqual(Bug.State.InReview, bug.GetState());

            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());

            bug.Verify();
            Assert.AreEqual(Bug.State.Verified, bug.GetState());

            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());

            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }
    }
}