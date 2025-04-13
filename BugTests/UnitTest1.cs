using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OpenToStart_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Start();  
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OpenToStartReview_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartReview();  
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OpenToApprove_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Approve();  
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void OpenToClose_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();  
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OpenToReopen_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Reopen();  
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AssignedToStartReview_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.StartReview();  
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AssignedToApprove_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Approve();  
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AssignedToClose_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();  
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AssignedToDefer_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();  
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InProgressToAssign_InvalidTransition()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Assign();  
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InProgressToClose_InvalidTransition()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Close();  
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InReviewToStart_InvalidTransition()
        {
            var bug = new Bug(Bug.State.InReview);
            bug.Start();  
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ApprovedToStartReview_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Approved);
            bug.StartReview();  
        }

        [TestMethod]
        public void ClosedToReopen()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();  
            Assert.AreEqual(Bug.State.Reopened, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ReopenedToClose_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Close();  
        }

        [TestMethod]
        public void OpenToAssigned_ValidTransition()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign(); // Переход в Assigned
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void AssignedToInProgress_ValidTransition()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Start(); // Переход в InProgress
            Assert.AreEqual(Bug.State.InProgress, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void CloseToOpen_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();  
        }
        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ReopenToProgress_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Start();  
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void RejectednToApproved_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Rejected);
            bug.Approve();  
        }
        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ClosenToApproved_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Approve();  
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ClosedToRejected_InvalidTransition()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reject();  
        }

    }
}
