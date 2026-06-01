using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public class BugWorkflowTests
{
    [TestMethod]
    public void DraftBugHasDraftState()
    {
        var bug = new Bug();

        Assert.AreEqual(Bug.State.Draft, bug.CurrentState);
    }

    [TestMethod]
    public void SubmitMovesBugToReported()
    {
        var bug = new Bug();

        bug.Submit();

        Assert.AreEqual(Bug.State.Reported, bug.CurrentState);
    }

    [TestMethod]
    public void ReportedBugCanMoveToAnalyzing()
    {
        var bug = CreateReportedBug();

        bug.StartAnalysis();

        Assert.AreEqual(Bug.State.Analyzing, bug.CurrentState);
    }

    [TestMethod]
    public void AnalyzingBugCanMoveToFixing()
    {
        var bug = CreateAnalyzingBug();

        bug.ApproveFix();

        Assert.AreEqual(Bug.State.Fixing, bug.CurrentState);
    }

    [TestMethod]
    public void FixingBugCanBeSentToReview()
    {
        var bug = CreateFixingBug();

        bug.SendToReview();

        Assert.AreEqual(Bug.State.ReadyForReview, bug.CurrentState);
    }

    [TestMethod]
    public void ReviewCanFinishWithRelease()
    {
        var bug = CreateReadyForReviewBug();

        bug.Ship();

        Assert.AreEqual(Bug.State.Released, bug.CurrentState);
    }

    [TestMethod]
    public void ReviewRejectionReturnsBug()
    {
        var bug = CreateReadyForReviewBug();

        bug.RejectReview();

        Assert.AreEqual(Bug.State.Returned, bug.CurrentState);
    }

    [TestMethod]
    public void ReturnedBugCanGoBackToFixing()
    {
        var bug = CreateReturnedBug();

        bug.ApproveFix();

        Assert.AreEqual(Bug.State.Fixing, bug.CurrentState);
    }

    [TestMethod]
    public void ReportedBugMayBeMarkedAsDuplicate()
    {
        var bug = CreateReportedBug();

        bug.MarkDuplicate();

        Assert.AreEqual(Bug.State.Duplicate, bug.CurrentState);
    }

    [TestMethod]
    public void ReportedBugMayBeMarkedAsWontDo()
    {
        var bug = CreateReportedBug();

        bug.MarkWonTDo();

        Assert.AreEqual(Bug.State.WonTDo, bug.CurrentState);
    }

    [TestMethod]
    public void AnalyzingBugMayBePaused()
    {
        var bug = CreateAnalyzingBug();

        bug.Pause();

        Assert.AreEqual(Bug.State.Suspended, bug.CurrentState);
    }

    [TestMethod]
    public void FixingBugMayAlsoBePaused()
    {
        var bug = CreateFixingBug();

        bug.Pause();

        Assert.AreEqual(Bug.State.Suspended, bug.CurrentState);
    }

    [TestMethod]
    public void SuspendedBugCanBeResumedToAnalyzing()
    {
        var bug = CreateSuspendedBugFromAnalysis();

        bug.Resume();

        Assert.AreEqual(Bug.State.Analyzing, bug.CurrentState);
    }

    [TestMethod]
    public void ReleasedBugCanBeReopened()
    {
        var bug = CreateReleasedBug();

        bug.Reopen();

        Assert.AreEqual(Bug.State.Returned, bug.CurrentState);
    }

    [TestMethod]
    public void DuplicateBugCanBeReopened()
    {
        var bug = CreateReportedBug();
        bug.MarkDuplicate();

        bug.Reopen();

        Assert.AreEqual(Bug.State.Returned, bug.CurrentState);
    }

    [TestMethod]
    public void WontDoBugCanBeReopened()
    {
        var bug = CreateReportedBug();
        bug.MarkWonTDo();

        bug.Reopen();

        Assert.AreEqual(Bug.State.Returned, bug.CurrentState);
    }

    [TestMethod]
    public void InvalidTransitionFromDraftThrows()
    {
        var bug = new Bug();

        Assert.ThrowsException<InvalidOperationException>(() => bug.Ship());
    }

    [TestMethod]
    public void InvalidResumeFromReportedThrows()
    {
        var bug = CreateReportedBug();

        Assert.ThrowsException<InvalidOperationException>(() => bug.Resume());
    }

    [TestMethod]
    public void InvalidReviewTriggerFromFixingThrows()
    {
        var bug = CreateFixingBug();

        Assert.ThrowsException<InvalidOperationException>(() => bug.Ship());
    }

    [TestMethod]
    public void ExceptionMessageContainsTriggerName()
    {
        var bug = new Bug();

        var exception = Assert.ThrowsException<InvalidOperationException>(() => bug.Ship());

        StringAssert.Contains(exception.Message, "Ship");
    }

    [TestMethod]
    public void ExceptionMessageContainsStateName()
    {
        var bug = CreateReportedBug();

        var exception = Assert.ThrowsException<InvalidOperationException>(() => bug.Resume());

        StringAssert.Contains(exception.Message, "Reported");
    }

    [TestMethod]
    public void CanFireShowsAvailableActionInReportedState()
    {
        var bug = CreateReportedBug();

        Assert.IsTrue(bug.CanFire(Bug.Trigger.StartAnalysis));
        Assert.IsFalse(bug.CanFire(Bug.Trigger.Ship));
    }

    [TestMethod]
    public void ReleasedStateIsTerminal()
    {
        var bug = CreateReleasedBug();

        Assert.IsTrue(bug.IsTerminal);
    }

    [TestMethod]
    public void DuplicateStateIsTerminal()
    {
        var bug = CreateReportedBug();
        bug.MarkDuplicate();

        Assert.IsTrue(bug.IsTerminal);
    }

    [TestMethod]
    public void ReturnedStateIsNotTerminal()
    {
        var bug = CreateReturnedBug();

        Assert.IsFalse(bug.IsTerminal);
    }

    [TestMethod]
    public void JournalCollectsAllTransitions()
    {
        var bug = new Bug();
        bug.Submit();
        bug.StartAnalysis();
        bug.ApproveFix();
        bug.SendToReview();

        Assert.AreEqual(4, bug.Journal.Count);
        StringAssert.Contains(bug.Journal[0], "Draft => Submit => Reported");
    }

    [TestMethod]
    public void JournalStoresReleaseTransition()
    {
        var bug = CreateReadyForReviewBug();

        bug.Ship();

        StringAssert.Contains(bug.Journal[^1], "ReadyForReview => Ship => Released");
    }

    [TestMethod]
    public void DescribeContainsCurrentState()
    {
        var bug = CreateFixingBug();

        var report = bug.Describe();

        StringAssert.Contains(report, "Fixing");
    }

    [TestMethod]
    public void ToStringMatchesDescribe()
    {
        var bug = CreateReturnedBug();

        Assert.AreEqual(bug.Describe(), bug.ToString());
    }

    [TestMethod]
    public void DraftBugHasSingleAvailableAction()
    {
        var bug = new Bug();

        Assert.AreEqual(1, bug.CountAvailableActions());
    }

    [TestMethod]
    public void ReportedBugHasThreeAvailableActions()
    {
        var bug = CreateReportedBug();

        Assert.AreEqual(3, bug.CountAvailableActions());
    }

    private static Bug CreateReportedBug()
    {
        var bug = new Bug();
        bug.Submit();
        return bug;
    }

    private static Bug CreateAnalyzingBug()
    {
        var bug = CreateReportedBug();
        bug.StartAnalysis();
        return bug;
    }

    private static Bug CreateFixingBug()
    {
        var bug = CreateAnalyzingBug();
        bug.ApproveFix();
        return bug;
    }

    private static Bug CreateReadyForReviewBug()
    {
        var bug = CreateFixingBug();
        bug.SendToReview();
        return bug;
    }

    private static Bug CreateReturnedBug()
    {
        var bug = CreateReadyForReviewBug();
        bug.RejectReview();
        return bug;
    }

    private static Bug CreateReleasedBug()
    {
        var bug = CreateReadyForReviewBug();
        bug.Ship();
        return bug;
    }

    private static Bug CreateSuspendedBugFromAnalysis()
    {
        var bug = CreateAnalyzingBug();
        bug.Pause();
        return bug;
    }
}
