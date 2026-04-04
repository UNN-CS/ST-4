using BugPro;

namespace BugTests;

[TestClass]
public sealed class UnitTest1
{
    #region Lifecycle

    [TestMethod]
    public void InitialState_IsNewDefect()
    {
        var bug = new Bug();
        Assert.AreEqual(State.NewDefect, bug.CurrentState);
        Assert.AreEqual("Новый дефект", bug.CurrentStateDisplayName);
    }

    [TestMethod]
    public void SubmitToTriage_TransitionsToDefectAnalysis()
    {
        var bug = new Bug();
        bug.Fire(Trigger.SubmitToTriage);
        Assert.AreEqual(State.DefectAnalysis, bug.CurrentState);
        Assert.AreEqual(1, bug.DefectAnalysisEntryCount);
    }

    [TestMethod]
    public void Analysis_Fixing_Accepted_ClosesBug()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.StartFixing);
        bug.Fire(Trigger.VerifyFixAccepted);
        Assert.AreEqual(State.Closed, bug.CurrentState);
        Assert.AreEqual("Закрытие", bug.CurrentStateDisplayName);
    }

    [TestMethod]
    public void Closed_Reopen_GoesBackToAnalysis()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.StartFixing);
        bug.Fire(Trigger.VerifyFixAccepted);
        bug.Fire(Trigger.ReopenClosed);
        Assert.AreEqual(State.DefectAnalysis, bug.CurrentState);
        Assert.IsGreaterThanOrEqualTo(bug.DefectAnalysisEntryCount, 2);
    }

    [TestMethod]
    public void Return_CompleteReturnToTriage_EntersAnalysisAgain()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.RouteToNotDefect);
        bug.Fire(Trigger.ConfirmRejectionNotOk);
        Assert.AreEqual(State.Return, bug.CurrentState);
        bug.Fire(Trigger.CompleteReturnToTriage);
        Assert.AreEqual(State.DefectAnalysis, bug.CurrentState);
    }

    #endregion

    #region DeferredFromTriage

    [TestMethod]
    public void DeferNoTimeNow_Resume_ReturnsToAnalysis()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.DeferNoTimeNow);
        Assert.AreEqual(State.DeferredNoTimeNow, bug.CurrentState);
        bug.Fire(Trigger.ResumeFromDefer);
        Assert.AreEqual(State.DefectAnalysis, bug.CurrentState);
    }

    [TestMethod]
    public void DeferNeedSeparateDecision_Resume_ReturnsToAnalysis()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.DeferNeedSeparateDecision);
        bug.Fire(Trigger.ResumeFromDefer);
        Assert.AreEqual(State.DefectAnalysis, bug.CurrentState);
    }

    [TestMethod]
    public void DeferOtherProduct_Resume_ReturnsToAnalysis()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.DeferOtherProduct);
        bug.Fire(Trigger.ResumeFromDefer);
        Assert.AreEqual(State.DefectAnalysis, bug.CurrentState);
    }

    [TestMethod]
    public void DeferNeedMoreInfo_Resume_ReturnsToAnalysis()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.DeferNeedMoreInfo);
        bug.Fire(Trigger.ResumeFromDefer);
        Assert.AreEqual(State.DefectAnalysis, bug.CurrentState);
    }

    #endregion

    #region Rejection

    [TestMethod]
    public void Rejection_ConfirmOk_Closes_FromNotDefect()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.RouteToNotDefect);
        bug.Fire(Trigger.ConfirmRejectionOk);
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void Rejection_ConfirmOk_Closes_FromWontFix()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.RouteToWontFix);
        bug.Fire(Trigger.ConfirmRejectionOk);
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void Rejection_ConfirmOk_Closes_FromDuplicate()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.RouteToDuplicate);
        bug.Fire(Trigger.ConfirmRejectionOk);
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void Rejection_ConfirmOk_Closes_FromNotReproducible()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.RouteToNotReproducible);
        bug.Fire(Trigger.ConfirmRejectionOk);
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    #endregion

    #region FixingAndVerification

    [TestMethod]
    public void Fixing_VerifyRejected_GoesToReturn_ThenBackToAnalysis()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.StartFixing);
        bug.Fire(Trigger.VerifyFixRejected);
        Assert.AreEqual(State.Return, bug.CurrentState);
        bug.Fire(Trigger.CompleteReturnToTriage);
        Assert.AreEqual(State.DefectAnalysis, bug.CurrentState);
    }

    [TestMethod]
    public void Fixing_RequestMoreInfoWhileFixing_GoesToDeferredNeedMoreInfo()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.StartFixing);
        bug.Fire(Trigger.RequestMoreInfoWhileFixing);
        Assert.AreEqual(State.DeferredNeedMoreInfo, bug.CurrentState);
        bug.Fire(Trigger.ResumeFromDefer);
        Assert.AreEqual(State.DefectAnalysis, bug.CurrentState);
    }

    #endregion

    #region EscalationExtension

    [TestMethod]
    public void Escalate_FromAnalysis_GoesToEscalated()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.Escalate);
        Assert.AreEqual(State.Escalated, bug.CurrentState);
        Assert.AreEqual("Эскалация", bug.CurrentStateDisplayName);
    }

    [TestMethod]
    public void Escalated_Deescalate_ReturnsToAnalysis()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.Escalate);
        bug.Fire(Trigger.Deescalate);
        Assert.AreEqual(State.DefectAnalysis, bug.CurrentState);
    }

    [TestMethod]
    public void Escalated_StartFixing_GoesToFixing()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.Escalate);
        bug.Fire(Trigger.StartFixing);
        Assert.AreEqual(State.Fixing, bug.CurrentState);
    }

    #endregion

    #region PermittedTriggersAndCanFire

    [TestMethod]
    public void NewDefect_CanOnlySubmitToTriage()
    {
        var bug = new Bug();
        Assert.IsTrue(bug.CanFire(Trigger.SubmitToTriage));
        Assert.IsFalse(bug.CanFire(Trigger.StartFixing));
        CollectionAssert.Contains(bug.GetPermittedTriggers().ToList(), Trigger.SubmitToTriage);
    }

    [TestMethod]
    public void Closed_CanOnlyReopen()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.StartFixing);
        bug.Fire(Trigger.VerifyFixAccepted);
        Assert.IsTrue(bug.CanFire(Trigger.ReopenClosed));
        Assert.IsFalse(bug.CanFire(Trigger.StartFixing));
    }

    #endregion

    #region InvalidTransitions_StatelessExceptions

    [TestMethod]
    public void Fire_StartFixing_FromNewDefect_ThrowsInvalidOperationException()
    {
        var bug = new Bug();
        var ex = Assert.ThrowsExactly<InvalidOperationException>(() => bug.Fire(Trigger.StartFixing));
        Assert.IsFalse(string.IsNullOrWhiteSpace(ex.Message));
    }

    [TestMethod]
    public void Fire_SubmitToTriage_FromAnalysis_ThrowsInvalidOperationException()
    {
        var bug = NewInAnalysis();
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.Fire(Trigger.SubmitToTriage));
    }

    [TestMethod]
    public void Fire_ConfirmRejectionOk_FromAnalysis_ThrowsInvalidOperationException()
    {
        var bug = NewInAnalysis();
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.Fire(Trigger.ConfirmRejectionOk));
    }

    [TestMethod]
    public void Fire_VerifyFixAccepted_FromAnalysis_ThrowsInvalidOperationException()
    {
        var bug = NewInAnalysis();
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.Fire(Trigger.VerifyFixAccepted));
    }

    [TestMethod]
    public void Fire_ReopenClosed_FromFixing_ThrowsInvalidOperationException()
    {
        var bug = NewInAnalysis();
        bug.Fire(Trigger.StartFixing);
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.Fire(Trigger.ReopenClosed));
    }

    #endregion

    private static Bug NewInAnalysis()
    {
        var bug = new Bug();
        bug.Fire(Trigger.SubmitToTriage);
        return bug;
    }
}
