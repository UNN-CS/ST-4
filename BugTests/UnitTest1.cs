using BugPro;

namespace BugTests;

[TestClass]
public sealed class UnitTest1
{
    [TestMethod]
    public void VerifyInitialStateIsNewDefect()
    {
        var activeBug = new Bug();
        Assert.AreEqual(BugState.NewDefect, activeBug.State);
    }

    [TestMethod]
    public void VerifySubmitToTriageChangesStateToDefectTriage()
    {
        var activeBug = new Bug();
        activeBug.Fire(BugTrigger.SubmitToTriage);
        Assert.AreEqual(BugState.DefectTriage, activeBug.State);
    }

    [TestMethod]
    public void VerifyFromTriageStartFixingTransitionsToFixing()
    {
        var activeBug = CreateBugInTriageState();
        activeBug.Fire(BugTrigger.StartFixing);
        Assert.AreEqual(BugState.Fixing, activeBug.State);
    }

    [TestMethod]
    public void VerifyFixingProblemSolvedTransitionsToClosed()
    {
        var activeBug = CreateBugInFixingState();
        activeBug.Fire(BugTrigger.ProblemSolved);
        Assert.AreEqual(BugState.Closed, activeBug.State);
    }

    [TestMethod]
    public void VerifyFullWorkflowFromTriageToFixAndClose()
    {
        var activeBug = new Bug();
        activeBug.Fire(BugTrigger.SubmitToTriage);
        activeBug.Fire(BugTrigger.StartFixing);
        activeBug.Fire(BugTrigger.ProblemSolved);
        Assert.AreEqual(BugState.Closed, activeBug.State);
    }

    [TestMethod]
    public void VerifyTriageCloseAsNotABugTransitionsToClosed()
    {
        var activeBug = CreateBugInTriageState();
        activeBug.Fire(BugTrigger.CloseAsNotABug);
        Assert.AreEqual(BugState.Closed, activeBug.State);
    }

    [TestMethod]
    public void VerifyTriageCloseAsWontFixTransitionsToClosed()
    {
        var activeBug = CreateBugInTriageState();
        activeBug.Fire(BugTrigger.CloseAsWontFix);
        Assert.AreEqual(BugState.Closed, activeBug.State);
    }

    [TestMethod]
    public void VerifyTriageCloseAsDuplicateTransitionsToClosed()
    {
        var activeBug = CreateBugInTriageState();
        activeBug.Fire(BugTrigger.CloseAsDuplicate);
        Assert.AreEqual(BugState.Closed, activeBug.State);
    }

    [TestMethod]
    public void VerifyFailedFixMovesToReturnedThenBackToTriage()
    {
        var activeBug = CreateBugInFixingState();
        activeBug.Fire(BugTrigger.ReturnAfterFailedFix);
        Assert.AreEqual(BugState.Returned, activeBug.State);

        activeBug.Fire(BugTrigger.CompleteReturnToTriage);
        Assert.AreEqual(BugState.DefectTriage, activeBug.State);
    }

    [TestMethod]
    public void VerifyTriageMarkNotReproducibleTransitionsToNotReproducible()
    {
        var activeBug = CreateBugInTriageState();
        activeBug.Fire(BugTrigger.MarkNotReproducible);
        Assert.AreEqual(BugState.NotReproducible, activeBug.State);
    }

    [TestMethod]
    public void VerifyNotReproducibleConfirmOkTransitionsToClosed()
    {
        var activeBug = CreateBugInTriageState();
        activeBug.Fire(BugTrigger.MarkNotReproducible);
        activeBug.Fire(BugTrigger.ConfirmNotReproducibleOk);
        Assert.AreEqual(BugState.Closed, activeBug.State);
    }

    [TestMethod]
    public void VerifyNotReproducibleRejectMovesToReturnedThenToTriage()
    {
        var activeBug = CreateBugInTriageState();
        activeBug.Fire(BugTrigger.MarkNotReproducible);
        activeBug.Fire(BugTrigger.RejectNotReproducible);
        Assert.AreEqual(BugState.Returned, activeBug.State);

        activeBug.Fire(BugTrigger.CompleteReturnToTriage);
        Assert.AreEqual(BugState.DefectTriage, activeBug.State);
    }

    [TestMethod]
    public void VerifyFixingCanTransitionToNotReproducibleState()
    {
        var activeBug = CreateBugInFixingState();
        activeBug.Fire(BugTrigger.MarkNotReproducible);
        Assert.AreEqual(BugState.NotReproducible, activeBug.State);
    }

    [TestMethod]
    public void VerifyFixingRequestMoreInfoTransitionsToNeedMoreInfo()
    {
        var activeBug = CreateBugInFixingState();
        activeBug.Fire(BugTrigger.RequestMoreInfo);
        Assert.AreEqual(BugState.NeedMoreInfo, activeBug.State);
    }

    [TestMethod]
    public void VerifyDescribeMethodReturnsCorrectStringForReturnedState()
    {
        string textLabel = Bug.Describe(BugState.Returned);
        Assert.AreEqual("ВОЗВРАТ", textLabel);
    }

    [TestMethod]
    public void VerifyReturnedStateThrowsExceptionOnInvalidStartFixingTrigger()
    {
        var activeBug = CreateBugInFixingState();
        activeBug.Fire(BugTrigger.ReturnAfterFailedFix);
        Assert.AreEqual(BugState.Returned, activeBug.State);

        Assert.ThrowsException<InvalidOperationException>(() => activeBug.Fire(BugTrigger.StartFixing));
    }

    [TestMethod]
    public void VerifyDeferNoTimeCanCycleToTriageOrFixingState()
    {
        var activeBug = CreateBugInTriageState();
        activeBug.Fire(BugTrigger.DeferNoTime);
        Assert.AreEqual(BugState.DeferredNoTime, activeBug.State);

        activeBug.Fire(BugTrigger.ReturnToTriage);
        Assert.AreEqual(BugState.DefectTriage, activeBug.State);

        activeBug.Fire(BugTrigger.DeferNoTime);
        activeBug.Fire(BugTrigger.StartFixing);
        Assert.AreEqual(BugState.Fixing, activeBug.State);
    }

    [TestMethod]
    public void VerifyNeedsSeparateDecisionTransitionsRoundTrip()
    {
        var activeBug = CreateBugInTriageState();
        activeBug.Fire(BugTrigger.RequireSeparateDecision);
        Assert.AreEqual(BugState.NeedsSeparateDecision, activeBug.State);

        activeBug.Fire(BugTrigger.ReturnToTriage);
        Assert.AreEqual(BugState.DefectTriage, activeBug.State);
    }

    [TestMethod]
    public void VerifyOtherProductProblemCanTransitionToFixingState()
    {
        var activeBug = CreateBugInTriageState();
        activeBug.Fire(BugTrigger.MarkOtherProduct);
        activeBug.Fire(BugTrigger.StartFixing);
        Assert.AreEqual(BugState.Fixing, activeBug.State);
    }

    [TestMethod]
    public void VerifyNeedMoreInfoStateAllowsStartFixingTrigger()
    {
        var activeBug = CreateBugInTriageState();
        activeBug.Fire(BugTrigger.RequestMoreInfo);
        activeBug.Fire(BugTrigger.StartFixing);
        Assert.AreEqual(BugState.Fixing, activeBug.State);
    }

    [TestMethod]
    public void VerifyClosedReopenWorkflowReturnsToTriageCorrectly()
    {
        var activeBug = CreateBugInFixingState();
        activeBug.Fire(BugTrigger.ProblemSolved);
        activeBug.Fire(BugTrigger.Reopen);
        Assert.AreEqual(BugState.Reopened, activeBug.State);

        activeBug.Fire(BugTrigger.EnterTriageAfterReopen);
        Assert.AreEqual(BugState.DefectTriage, activeBug.State);
    }

    [TestMethod]
    public void VerifyCanFireReturnsFalseForInvalidTriggerInCurrentState()
    {
        var activeBug = new Bug();
        bool checkResult = activeBug.CanFire(BugTrigger.StartFixing);
        Assert.IsFalse(checkResult);
    }

    [TestMethod]
    public void VerifyDescribeMethodReturnsCorrectStringForDefectTriage()
    {
        string textLabel = Bug.Describe(BugState.DefectTriage);
        Assert.AreEqual("РАЗБОР ДЕФЕКТОВ", textLabel);
    }

    [TestMethod]
    public void VerifyNewDefectStateThrowsExceptionOnForbiddenTrigger()
    {
        var activeBug = new Bug();
        var errorInfo = Assert.ThrowsException<InvalidOperationException>(() => activeBug.Fire(BugTrigger.ProblemSolved));
        StringAssert.Contains(errorInfo.Message, BugTrigger.ProblemSolved.ToString(), StringComparison.Ordinal);
    }

    [TestMethod]
    public void VerifyClosedStateThrowsExceptionOnForbiddenTrigger()
    {
        var activeBug = new Bug(BugState.Closed);
        var errorInfo = Assert.ThrowsException<InvalidOperationException>(() => activeBug.Fire(BugTrigger.StartFixing));
        StringAssert.Contains(errorInfo.Message, BugTrigger.StartFixing.ToString(), StringComparison.Ordinal);
    }

    [TestMethod]
    public void VerifyFixingStateThrowsExceptionOnForbiddenSubmitToTriageTrigger()
    {
        var activeBug = CreateBugInFixingState();
        Assert.ThrowsException<InvalidOperationException>(() => activeBug.Fire(BugTrigger.SubmitToTriage));
    }

    [TestMethod]
    public void VerifyReopenedStateThrowsExceptionBeforeEnteringTriageState()
    {
        var activeBug = new Bug(BugState.Closed);
        activeBug.Fire(BugTrigger.Reopen);
        Assert.ThrowsException<InvalidOperationException>(() => activeBug.Fire(BugTrigger.StartFixing));
    }

    [TestMethod]
    public void VerifyNotReproducibleStateThrowsExceptionOnForbiddenReopenTrigger()
    {
        var activeBug = CreateBugInTriageState();
        activeBug.Fire(BugTrigger.MarkNotReproducible);
        Assert.ThrowsException<InvalidOperationException>(() => activeBug.Fire(BugTrigger.Reopen));
    }

    private static Bug CreateBugInTriageState()
    {
        var activeBug = new Bug();
        activeBug.Fire(BugTrigger.SubmitToTriage);
        return activeBug;
    }

    private static Bug CreateBugInFixingState()
    {
        var activeBug = CreateBugInTriageState();
        activeBug.Fire(BugTrigger.StartFixing);
        return activeBug;
    }
}
