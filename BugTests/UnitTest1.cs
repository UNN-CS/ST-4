using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

[TestClass]
public class BugStateMachineTests
{
    [TestMethod]
    public void NewBug_ShouldBeInOpenState()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.AreEqual(Bug.State.Open, bug.CurrentState);
    }

    [TestMethod]
    public void Assign_FromOpen_ShouldChangeToAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign("assignee");
        Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Close_FromOpen_ShouldThrowException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Close();
    }

    [TestMethod]
    public void MarkAsDuplicate_FromOpen_ShouldChangeToDuplicate()
    {
        var bug = new Bug(Bug.State.Open);
        bug.MarkAsDuplicate();
        Assert.AreEqual(Bug.State.Duplicate, bug.CurrentState);
    }

    [TestMethod]
    public void FullWorkflow_ShouldReachClosedState()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign("assignee");
        bug.StartWork();
        bug.RequestReview();
        bug.Approve();
        bug.Verify();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void Reject_FromNeedsReview_ShouldChangeToInProgress()
    {
        var bug = new Bug(Bug.State.NeedsReview);
        bug.Reject();
        Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
    }

    [TestMethod]
    public void Reopen_FromClosed_ShouldChangeToReopened()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void StartWork_FromClosed_ShouldThrowException()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.StartWork();
    }

    [TestMethod]
    public void Defer_FromAssigned_ShouldChangeToDeferred()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(Bug.State.Deferred, bug.CurrentState);
    }

    [TestMethod]
    public void Reopen_FromDuplicate_ShouldChangeToReopened()
    {
        var bug = new Bug(Bug.State.Duplicate);
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void MarkAsWontFix_FromAssigned_ShouldChangeToWontFix()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.MarkAsWontFix();
        Assert.AreEqual(Bug.State.WontFix, bug.CurrentState);
    }

    [TestMethod]
    public void Verify_FromResolved_ShouldChangeToVerified()
    {
        var bug = new Bug(Bug.State.Resolved);
        bug.Verify();
        Assert.AreEqual(Bug.State.Verified, bug.CurrentState);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Verify_FromOpen_ShouldThrowException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Verify();
    }

    [TestMethod]
    public void Assign_FromReopened_ShouldChangeToAssigned()
    {
        var bug = new Bug(Bug.State.Reopened);
        bug.Assign("assignee");
        Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
    }

    [TestMethod]
    public void MultipleDeferrals_ShouldWorkCorrectly()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        bug.Assign("assignee");
        bug.Defer();
        Assert.AreEqual(Bug.State.Deferred, bug.CurrentState);
    }

    [TestMethod]
    public void Close_FromVerified_ShouldChangeToClosed()
    {
        var bug = new Bug(Bug.State.Verified);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Approve_FromInProgress_ShouldThrowException()
    {
        var bug = new Bug(Bug.State.InProgress);
        bug.Approve();
    }

    [TestMethod]
    public void CanClose_FromVerified_ShouldReturnTrue()
    {
        var bug = new Bug(Bug.State.Verified);
        Assert.IsTrue(bug.CanClose);
    }

    [TestMethod]
    public void CanClose_FromOpen_ShouldReturnFalse()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.IsFalse(bug.CanClose);
    }

    [TestMethod]
    public void TransitionThroughAllStates_ShouldWork()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.IsTrue(bug.CanAssign);

        bug.Assign("assignee");
        Assert.IsTrue(bug.CanStartWork);

        bug.StartWork();
        Assert.IsTrue(bug.CanRequestReview);

        bug.RequestReview();
        Assert.IsTrue(bug.CanApprove);

        bug.Approve();
        Assert.AreEqual(Bug.State.Resolved, bug.CurrentState);
        Assert.IsTrue(bug.CanVerify);

        bug.Verify();
        Assert.IsTrue(bug.CanClose);

        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void Reopen_FromWontFix_ShouldChangeToReopened()
    {
        var bug = new Bug(Bug.State.WontFix);
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Resolve_FromOpen_ShouldThrowException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Resolve();
    }

    [TestMethod]
    public void MarkAsWontFix_FromInProgress_ShouldChangeToWontFix()
    {
        var bug = new Bug(Bug.State.InProgress);
        bug.MarkAsWontFix();
        Assert.AreEqual(Bug.State.WontFix, bug.CurrentState);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void MarkAsDuplicate_FromClosed_ShouldThrowException()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.MarkAsDuplicate();
    }

    [TestMethod]
    public void CanReopen_FromDuplicate_ShouldReturnTrue()
    {
        var bug = new Bug(Bug.State.Duplicate);
        Assert.IsTrue(bug.CanReopen);
    }

    [TestMethod]
    public void CanDefer_FromAssigned_ShouldReturnTrue()
    {
        var bug = new Bug(Bug.State.Assigned);
        Assert.IsTrue(bug.CanDefer);
    }

    [TestMethod]
    public void MultipleReopens_ShouldWorkCorrectly()
    {
        var bug = new Bug(Bug.State.Open);
        bug.MarkAsDuplicate();
        bug.Reopen();
        bug.Assign("assignee");
        bug.StartWork();
        bug.RequestReview();
        bug.Approve();
        bug.Verify();
        bug.Close();
        bug.Reopen();

        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void Verify_FromVerified_ShouldRemainVerified()
    {
        var bug = new Bug(Bug.State.Verified);
        bug.Verify();
        Assert.AreEqual(Bug.State.Verified, bug.CurrentState);
    }

    [TestMethod]
    public void ComplexWorkflow_WithDeferAndReopen_ShouldWork()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign("assignee");
        bug.Defer();
        bug.Assign("assignee");
        bug.StartWork();
        bug.RequestReview();
        bug.Reject();

        bug.MarkAsWontFix();

        bug.Reopen();
        bug.Assign("assignee");
        bug.StartWork();
        bug.RequestReview();
        bug.Approve();
        bug.Verify();
        bug.Close();

        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }
}
