using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public class BugStateMachineTests
{
    [TestMethod]
    public void NewBugStartsInNewState()
    {
        var bug = new Bug();
        Assert.AreEqual(BugState.New, bug.State);
        Assert.IsFalse(bug.IsClosed);
    }

    [TestMethod]
    public void SubmitForTriageMovesBugToUnderReview()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        Assert.AreEqual(BugState.UnderReview, bug.State);
    }

    [TestMethod]
    public void AcceptForFixFromUnderReviewMovesToInProgress()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        bug.AcceptForFix();
        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void DeveloperCanRequestMoreInformationAndReturnToReview()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        bug.AcceptForFix();

        bug.DeveloperNeeds(DeveloperReturnReason.NeedMoreInformation);
        Assert.AreEqual(BugState.UnderReview, bug.State);
        Assert.AreEqual("Нужно больше информации", bug.LastReason);
    }

    [TestMethod]
    public void DeveloperCanReturnBugDueToNoTimeNow()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        bug.AcceptForFix();

        bug.DeveloperNeeds(DeveloperReturnReason.NoTimeNow);
        Assert.AreEqual(BugState.UnderReview, bug.State);
        Assert.AreEqual("Нет времени сейчас", bug.LastReason);
    }

    [TestMethod]
    public void DeveloperCanReturnBugDueToSeparateSolutionNeed()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        bug.AcceptForFix();

        bug.DeveloperNeeds(DeveloperReturnReason.NeedSeparateSolution);
        Assert.AreEqual(BugState.UnderReview, bug.State);
        Assert.AreEqual("Нужно отдельное решение", bug.LastReason);
    }

    [TestMethod]
    public void DeveloperCanReturnBugDueToExternalProductIssue()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        bug.AcceptForFix();

        bug.DeveloperNeeds(DeveloperReturnReason.ExternalProductIssue);
        Assert.AreEqual(BugState.UnderReview, bug.State);
        Assert.AreEqual("Проблема другого продукта", bug.LastReason);
    }

    [TestMethod]
    public void BugRejectedAsNotBugBecomesClosed()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        bug.RejectAsNotBug();

        Assert.AreEqual(BugState.Closed, bug.State);
        Assert.AreEqual(CloseReason.NotBug, bug.FinalCloseReason);
        Assert.AreEqual("Не дефект", bug.LastReason);
        Assert.IsTrue(bug.IsClosed);
    }

    [TestMethod]
    public void BugRejectedAsWontFixBecomesClosed()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        bug.RejectAsWontFix();

        Assert.AreEqual(BugState.Closed, bug.State);
        Assert.AreEqual(CloseReason.WontFix, bug.FinalCloseReason);
        Assert.AreEqual("Не исправлять", bug.LastReason);
    }

    [TestMethod]
    public void BugRejectedAsDuplicateBecomesClosed()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        bug.RejectAsDuplicate();

        Assert.AreEqual(BugState.Closed, bug.State);
        Assert.AreEqual(CloseReason.Duplicate, bug.FinalCloseReason);
        Assert.AreEqual("Дублирование", bug.LastReason);
    }

    [TestMethod]
    public void CannotReproduceTransitionLeadsToCannotReproduceState()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        bug.AcceptForFix();
        bug.CannotReproduce();

        Assert.AreEqual(BugState.CannotReproduce, bug.State);
        Assert.AreEqual("Не воспроизводится", bug.LastReason);
    }

    [TestMethod]
    public void TesterConfirmsNoReproClosesBug()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        bug.AcceptForFix();
        bug.CannotReproduce();
        bug.TesterConfirmsNoRepro();

        Assert.AreEqual(BugState.Closed, bug.State);
        Assert.AreEqual(CloseReason.NoRepro, bug.FinalCloseReason);
        Assert.IsTrue(bug.IsClosed);
    }

    [TestMethod]
    public void TesterDisagreesReturnsBugToReview()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        bug.AcceptForFix();
        bug.CannotReproduce();
        bug.TesterDisagreesAndReturns();

        Assert.AreEqual(BugState.UnderReview, bug.State);
        Assert.AreEqual("Тестировщик возвращает на разбор", bug.LastReason);
    }

    [TestMethod]
    public void FixCompletedMovesBugToTesting()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        bug.AcceptForFix();
        bug.FixCompleted();

        Assert.AreEqual(BugState.Testing, bug.State);
        Assert.AreEqual("Исправление готово", bug.LastReason);
    }

    [TestMethod]
    public void VerifyFixedClosesBugAfterTesting()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        bug.AcceptForFix();
        bug.FixCompleted();
        bug.VerifyFixed();

        Assert.AreEqual(BugState.Closed, bug.State);
        Assert.AreEqual(CloseReason.Fixed, bug.FinalCloseReason);
        Assert.AreEqual("Баг исправлен", bug.LastReason);
    }

    [TestMethod]
    public void VerifyNotFixedReturnsBugToReview()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        bug.AcceptForFix();
        bug.FixCompleted();
        bug.VerifyNotFixed();

        Assert.AreEqual(BugState.UnderReview, bug.State);
        Assert.AreEqual("Баг не исправлен", bug.LastReason);
    }

    [TestMethod]
    public void ReopenFromClosedReturnsBugToReview()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        bug.RejectAsNotBug();
        bug.Reopen();

        Assert.AreEqual(BugState.UnderReview, bug.State);
        Assert.AreEqual(CloseReason.Reopened, bug.FinalCloseReason);
    }

    [TestMethod]
    public void CannotAcceptForFixFromNewThrowsInvalidOperationException()
    {
        var bug = new Bug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.AcceptForFix());
    }

    [TestMethod]
    public void CannotVerifyFixedFromInProgressThrowsInvalidOperationException()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        bug.AcceptForFix();
        Assert.ThrowsException<InvalidOperationException>(() => bug.VerifyFixed());
    }

    [TestMethod]
    public void CannotReopenFromUnderReviewThrowsInvalidOperationException()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Reopen());
    }

    [TestMethod]
    public void CannotDeveloperNeedsFromClosedThrowsInvalidOperationException()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        bug.RejectAsDuplicate();
        Assert.ThrowsException<InvalidOperationException>(() => bug.DeveloperNeeds(DeveloperReturnReason.NoTimeNow));
    }

    [TestMethod]
    public void TrackingStateDescriptionIsUpdatedAfterTransition()
    {
        var bug = new Bug();
        bug.SubmitForTriage();
        Assert.AreEqual("UnderReview", bug.CurrentStateDescription);
    }
}
