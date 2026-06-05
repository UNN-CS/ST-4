using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public sealed class BugWorkflowTests
{
    private Bug subject = null!;

    [TestInitialize]
    public void CreateFreshBug()
    {
        subject = new Bug();
    }

    [TestCleanup]
    public void ReleaseSubject()
    {
        subject = null!;
    }

    [TestMethod]
    public void FreshTicketStartsInNewPhase()
    {
        AssertPhase(TicketPhase.New);
    }

    [TestMethod]
    public void NewTicketAllowsAnalysisStart()
    {
        Assert.IsTrue(subject.CanStartAnalysis());
    }

    [TestMethod]
    public void AnalysisStartUpdatesPhase()
    {
        subject.StartAnalysis();

        AssertPhase(TicketPhase.Analysis);
    }

    [TestMethod]
    public void InfoRequestFromAnalysisGoesToNeedMoreInfo()
    {
        EnterAnalysis();
        subject.RequestInfo();

        AssertPhase(TicketPhase.NeedMoreInfo);
    }

    [TestMethod]
    public void NeedMoreInfoAllowsReturnToAnalysis()
    {
        EnterNeedMoreInfo();

        Assert.IsTrue(subject.CanReturnToAnalysis());
    }

    [TestMethod]
    public void ReturnFromNeedMoreInfoRestoresAnalysis()
    {
        EnterNeedMoreInfo();
        subject.ReturnToAnalysis();

        AssertPhase(TicketPhase.Analysis);
    }

    [TestMethod]
    public void FixWorkBeginsFromAnalysis()
    {
        EnterAnalysis();
        subject.StartFix();

        AssertPhase(TicketPhase.InProgress);
    }

    [TestMethod]
    public void InProgressTicketCanBeFixed()
    {
        EnterInProgress();

        Assert.IsTrue(subject.CanFix());
    }

    [TestMethod]
    public void FixActionSetsFixedPhase()
    {
        EnterInProgress();
        subject.Fix();

        AssertPhase(TicketPhase.Fixed);
    }

    [TestMethod]
    public void FixedTicketCanBeClosed()
    {
        EnterFixed();

        Assert.IsTrue(subject.CanClose());
    }

    [TestMethod]
    public void ConfirmResolvedClosesTicket()
    {
        EnterFixed();
        subject.ConfirmResolved();

        AssertPhase(TicketPhase.Closed);
    }

    [TestMethod]
    public void ClosedTicketCanReopen()
    {
        EnterClosed();

        Assert.IsTrue(subject.CanReopen());
    }

    [TestMethod]
    public void ReopenFromClosedSetsReopenedPhase()
    {
        EnterClosed();
        subject.Reopen();

        AssertPhase(TicketPhase.Reopened);
    }

    [TestMethod]
    public void RejectFixFromFixedSetsReopenedPhase()
    {
        EnterFixed();
        subject.RejectFix();

        AssertPhase(TicketPhase.Reopened);
    }

    [TestMethod]
    public void ReopenedTicketReturnsToAnalysis()
    {
        EnterReopened();
        subject.ReturnToAnalysis();

        AssertPhase(TicketPhase.Analysis);
    }

    [TestMethod]
    public void ReopenedTicketCanResumeFix()
    {
        EnterReopened();
        subject.StartFix();

        AssertPhase(TicketPhase.InProgress);
    }

    [TestMethod]
    public void DuplicateMarkFromAnalysis()
    {
        EnterAnalysis();
        subject.MarkDuplicate();

        AssertPhase(TicketPhase.Duplicate);
    }

    [TestMethod]
    public void DuplicateTicketCanBeClosed()
    {
        EnterAnalysis();
        subject.MarkDuplicate();
        subject.ConfirmResolved();

        AssertPhase(TicketPhase.Closed);
    }

    [TestMethod]
    public void NotReproducibleMarkFromAnalysis()
    {
        EnterAnalysis();
        subject.MarkNotReproducible();

        AssertPhase(TicketPhase.NotReproducible);
    }

    [TestMethod]
    public void NotReproducibleTicketCanBeClosed()
    {
        EnterAnalysis();
        subject.MarkNotReproducible();
        subject.ConfirmResolved();

        AssertPhase(TicketPhase.Closed);
    }

    [TestMethod]
    public void NewTicketCanBeDeferred()
    {
        subject.Defer();

        AssertPhase(TicketPhase.Deferred);
    }

    [TestMethod]
    public void DeferredTicketReturnsToAnalysis()
    {
        subject.Defer();
        subject.ReturnToAnalysis();

        AssertPhase(TicketPhase.Analysis);
    }

    [TestMethod]
    public void NewTicketCannotBeFixedYet()
    {
        Assert.IsFalse(subject.CanFix());
    }

    [TestMethod]
    public void NewTicketCannotBeClosedYet()
    {
        Assert.IsFalse(subject.CanClose());
    }

    [TestMethod]
    public void NewTicketCannotReopenYet()
    {
        Assert.IsFalse(subject.CanReopen());
    }

    [TestMethod]
    public void FixFromNewRaisesInvalidOperation()
    {
        AssertInvalidTransition(() => subject.Fix());
    }

    [TestMethod]
    public void CloseFromAnalysisRaisesInvalidOperation()
    {
        EnterAnalysis();

        AssertInvalidTransition(() => subject.ConfirmResolved());
    }

    [TestMethod]
    public void ReopenFromFixedRaisesInvalidOperation()
    {
        EnterFixed();

        AssertInvalidTransition(() => subject.Reopen());
    }

    [TestMethod]
    public void AnalysisFromClosedRaisesInvalidOperation()
    {
        EnterClosed();

        AssertInvalidTransition(() => subject.StartAnalysis());
    }

    [TestMethod]
    public void ReturnToAnalysisFromNewRaisesInvalidOperation()
    {
        AssertInvalidTransition(() => subject.ReturnToAnalysis());
    }

    private void AssertPhase(TicketPhase expected)
    {
        Assert.AreEqual(expected, subject.State);
    }

    private static void AssertInvalidTransition(Action action)
    {
        Assert.ThrowsException<InvalidOperationException>(action);
    }

    private void EnterAnalysis()
    {
        subject.StartAnalysis();
    }

    private void EnterNeedMoreInfo()
    {
        EnterAnalysis();
        subject.RequestInfo();
    }

    private void EnterInProgress()
    {
        EnterAnalysis();
        subject.StartFix();
    }

    private void EnterFixed()
    {
        EnterInProgress();
        subject.Fix();
    }

    private void EnterClosed()
    {
        EnterFixed();
        subject.ConfirmResolved();
    }

    private void EnterReopened()
    {
        EnterFixed();
        subject.RejectFix();
    }
}
