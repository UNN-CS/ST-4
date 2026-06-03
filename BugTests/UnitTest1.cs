using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public sealed class UnitTest1
{
    private Bug bug = null!;

    [TestInitialize]
    public void SetUp()
    {
        bug = new Bug();
    }

    [TestCleanup]
    public void TearDown()
    {
        bug = null!;
    }

    [TestMethod]
    public void InitialStateIsNew()
    {
        Assert.AreEqual(BugState.New, bug.State);
    }

    [TestMethod]
    public void NewBugCanStartAnalysis()
    {
        Assert.IsTrue(bug.CanStartAnalysis());
    }

    [TestMethod]
    public void StartAnalysisMovesBugToAnalysis()
    {
        bug.StartAnalysis();

        Assert.AreEqual(BugState.Analysis, bug.State);
    }

    [TestMethod]
    public void RequestInfoMovesBugToNeedMoreInfo()
    {
        bug.StartAnalysis();
        bug.RequestInfo();

        Assert.AreEqual(BugState.NeedMoreInfo, bug.State);
    }

    [TestMethod]
    public void NeedMoreInfoCanReturnToAnalysis()
    {
        bug.StartAnalysis();
        bug.RequestInfo();

        Assert.IsTrue(bug.CanReturnToAnalysis());
    }

    [TestMethod]
    public void ReturnToAnalysisMovesBugFromNeedMoreInfoToAnalysis()
    {
        bug.StartAnalysis();
        bug.RequestInfo();
        bug.ReturnToAnalysis();

        Assert.AreEqual(BugState.Analysis, bug.State);
    }

    [TestMethod]
    public void StartFixMovesBugToInProgress()
    {
        bug.StartAnalysis();
        bug.StartFix();

        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void InProgressBugCanBeFixed()
    {
        bug.StartAnalysis();
        bug.StartFix();

        Assert.IsTrue(bug.CanFix());
    }

    [TestMethod]
    public void FixMovesBugToFixed()
    {
        bug.StartAnalysis();
        bug.StartFix();
        bug.Fix();

        Assert.AreEqual(BugState.Fixed, bug.State);
    }

    [TestMethod]
    public void FixedBugCanBeClosed()
    {
        bug.StartAnalysis();
        bug.StartFix();
        bug.Fix();

        Assert.IsTrue(bug.CanClose());
    }

    [TestMethod]
    public void ConfirmResolvedClosesFixedBug()
    {
        bug.StartAnalysis();
        bug.StartFix();
        bug.Fix();
        bug.ConfirmResolved();

        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void ClosedBugCanBeReopened()
    {
        bug.StartAnalysis();
        bug.StartFix();
        bug.Fix();
        bug.ConfirmResolved();

        Assert.IsTrue(bug.CanReopen());
    }

    [TestMethod]
    public void ReopenMovesClosedBugToReopened()
    {
        bug.StartAnalysis();
        bug.StartFix();
        bug.Fix();
        bug.ConfirmResolved();
        bug.Reopen();

        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void RejectFixMovesFixedBugToReopened()
    {
        bug.StartAnalysis();
        bug.StartFix();
        bug.Fix();
        bug.RejectFix();

        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void ReopenedBugCanReturnToAnalysis()
    {
        bug.StartAnalysis();
        bug.StartFix();
        bug.Fix();
        bug.RejectFix();
        bug.ReturnToAnalysis();

        Assert.AreEqual(BugState.Analysis, bug.State);
    }

    [TestMethod]
    public void ReopenedBugCanStartFixAgain()
    {
        bug.StartAnalysis();
        bug.StartFix();
        bug.Fix();
        bug.RejectFix();
        bug.StartFix();

        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void MarkDuplicateMovesBugToDuplicate()
    {
        bug.StartAnalysis();
        bug.MarkDuplicate();

        Assert.AreEqual(BugState.Duplicate, bug.State);
    }

    [TestMethod]
    public void DuplicateBugCanBeClosed()
    {
        bug.StartAnalysis();
        bug.MarkDuplicate();
        bug.ConfirmResolved();

        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void MarkNotReproducibleMovesBugToNotReproducible()
    {
        bug.StartAnalysis();
        bug.MarkNotReproducible();

        Assert.AreEqual(BugState.NotReproducible, bug.State);
    }

    [TestMethod]
    public void NotReproducibleBugCanBeClosed()
    {
        bug.StartAnalysis();
        bug.MarkNotReproducible();
        bug.ConfirmResolved();

        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void NewBugCanBeDeferred()
    {
        bug.Defer();

        Assert.AreEqual(BugState.Deferred, bug.State);
    }

    [TestMethod]
    public void DeferredBugCanReturnToAnalysis()
    {
        bug.Defer();
        bug.ReturnToAnalysis();

        Assert.AreEqual(BugState.Analysis, bug.State);
    }

    [TestMethod]
    public void CannotFixNewBug()
    {
        Assert.IsFalse(bug.CanFix());
    }

    [TestMethod]
    public void CannotCloseNewBug()
    {
        Assert.IsFalse(bug.CanClose());
    }

    [TestMethod]
    public void CannotReopenNewBug()
    {
        Assert.IsFalse(bug.CanReopen());
    }

    [TestMethod]
    public void InvalidFixFromNewThrows()
    {
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fix());
    }

    [TestMethod]
    public void InvalidCloseFromAnalysisThrows()
    {
        bug.StartAnalysis();

        Assert.ThrowsException<InvalidOperationException>(
            () => bug.ConfirmResolved());
    }

    [TestMethod]
    public void InvalidReopenFromFixedThrows()
    {
        bug.StartAnalysis();
        bug.StartFix();
        bug.Fix();

        Assert.ThrowsException<InvalidOperationException>(() => bug.Reopen());
    }

    [TestMethod]
    public void InvalidStartAnalysisFromClosedThrows()
    {
        bug.StartAnalysis();
        bug.StartFix();
        bug.Fix();
        bug.ConfirmResolved();

        Assert.ThrowsException<InvalidOperationException>(
            () => bug.StartAnalysis());
    }

    [TestMethod]
    public void InvalidReturnToAnalysisFromNewThrows()
    {
        Assert.ThrowsException<InvalidOperationException>(
            () => bug.ReturnToAnalysis());
    }
}
