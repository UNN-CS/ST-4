using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BugTests;

[TestClass]
public class BugWorkflowTests
{
    private static Bug MakeTriagedBug()
    {
        var bug = new Bug();
        bug.Submit();
        return bug;
    }

    private static Bug MakeAcceptedBug()
    {
        var bug = MakeTriagedBug();
        bug.Accept();
        return bug;
    }

    private static Bug MakeBugInProgress()
    {
        var bug = MakeAcceptedBug();
        bug.StartWork();
        return bug;
    }

    [TestMethod]
    public void NewBugStartsAsDraft()
    {
        Assert.AreEqual(BugState.Draft, new Bug().CurrentState);
    }

    [TestMethod]
    public void DraftBugCanBeSubmitted()
    {
        var bug = new Bug();
        bug.Submit();
        Assert.AreEqual(BugState.Triaged, bug.CurrentState);
    }

    [TestMethod]
    public void DraftBugCanBeDeferred()
    {
        var bug = new Bug();
        bug.Postpone();
        Assert.AreEqual(BugState.Deferred, bug.CurrentState);
    }

    [TestMethod]
    public void TriagedBugCanRequestReporterDetails()
    {
        var bug = MakeTriagedBug();
        bug.AskDetails();
        Assert.AreEqual(BugState.WaitingForReporter, bug.CurrentState);
    }

    [TestMethod]
    public void DetailsReturnBugToTriage()
    {
        var bug = MakeTriagedBug();
        bug.AskDetails();
        bug.AddDetails();
        Assert.AreEqual(BugState.Triaged, bug.CurrentState);
    }

    [TestMethod]
    public void TriagedBugCanBeAccepted()
    {
        var bug = MakeTriagedBug();
        bug.Accept();
        Assert.AreEqual(BugState.Accepted, bug.CurrentState);
    }

    [TestMethod]
    public void TriagedBugCanBeRejected()
    {
        var bug = MakeTriagedBug();
        bug.Reject();
        Assert.AreEqual(BugState.Rejected, bug.CurrentState);
    }

    [TestMethod]
    public void AcceptedBugCanMoveToWork()
    {
        var bug = MakeAcceptedBug();
        bug.StartWork();
        Assert.AreEqual(BugState.InProgress, bug.CurrentState);
    }

    [TestMethod]
    public void AcceptedBugCanBeDeferred()
    {
        var bug = MakeAcceptedBug();
        bug.Postpone();
        Assert.AreEqual(BugState.Deferred, bug.CurrentState);
    }

    [TestMethod]
    public void InProgressBugCanMoveToVerification()
    {
        var bug = MakeBugInProgress();
        bug.SendToCheck();
        Assert.AreEqual(BugState.Verification, bug.CurrentState);
    }

    [TestMethod]
    public void InProgressBugCanAskForDetails()
    {
        var bug = MakeBugInProgress();
        bug.AskDetails();
        Assert.AreEqual(BugState.WaitingForReporter, bug.CurrentState);
    }

    [TestMethod]
    public void VerificationCanCloseBug()
    {
        var bug = MakeBugInProgress();
        bug.SendToCheck();
        bug.ApproveFix();
        Assert.AreEqual(BugState.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void VerificationCanReturnBugToWork()
    {
        var bug = MakeBugInProgress();
        bug.SendToCheck();
        bug.DeclineFix();
        Assert.AreEqual(BugState.InProgress, bug.CurrentState);
    }

    [TestMethod]
    public void ClosedBugCanBeReopenedForTriage()
    {
        var bug = MakeBugInProgress();
        bug.SendToCheck();
        bug.ApproveFix();
        bug.Reopen();
        Assert.AreEqual(BugState.Triaged, bug.CurrentState);
    }

    [TestMethod]
    public void DeferredBugCanBeRestored()
    {
        var bug = new Bug();
        bug.Postpone();
        bug.Restore();
        Assert.AreEqual(BugState.Triaged, bug.CurrentState);
    }

    [TestMethod]
    public void DeferredBugCanBeRejected()
    {
        var bug = new Bug();
        bug.Postpone();
        bug.Reject();
        Assert.AreEqual(BugState.Rejected, bug.CurrentState);
    }

    [TestMethod]
    public void PositiveWorkflowEndsClosed()
    {
        var bug = MakeBugInProgress();
        bug.SendToCheck();
        bug.ApproveFix();
        Assert.IsTrue(bug.IsClosed);
    }

    [TestMethod]
    public void RejectedWorkflowIsTerminalForBusinessStatus()
    {
        var bug = MakeTriagedBug();
        bug.Reject();
        Assert.IsTrue(bug.IsClosed);
    }

    [TestMethod]
    public void FailedVerificationCanStillBeFixed()
    {
        var bug = MakeBugInProgress();
        bug.SendToCheck();
        bug.DeclineFix();
        bug.SendToCheck();
        bug.ApproveFix();
        Assert.AreEqual(BugState.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void ReporterDetailsCanLeadToAcceptedBug()
    {
        var bug = MakeTriagedBug();
        bug.AskDetails();
        bug.AddDetails();
        bug.Accept();
        Assert.AreEqual(BugState.Accepted, bug.CurrentState);
    }

    [TestMethod]
    public void CanReportsAllowedAction()
    {
        var bug = MakeTriagedBug();
        Assert.IsTrue(bug.Can(BugAction.Accept));
    }

    [TestMethod]
    public void CanReportsForbiddenAction()
    {
        var bug = new Bug();
        Assert.IsFalse(bug.Can(BugAction.StartWork));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CannotStartWorkFromDraft()
    {
        new Bug().StartWork();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CannotApproveBeforeVerification()
    {
        MakeBugInProgress().ApproveFix();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CannotReopenRejectedBug()
    {
        var bug = MakeTriagedBug();
        bug.Reject();
        bug.Reopen();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CannotRestoreClosedBug()
    {
        var bug = MakeBugInProgress();
        bug.SendToCheck();
        bug.ApproveFix();
        bug.Restore();
    }
}
