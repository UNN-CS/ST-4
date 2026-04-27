using BugPro;

namespace BugTests;

[TestClass]
public sealed class UnitTest1
{
    [TestMethod]
    public void New_To_Triage_On_StartTriage()
    {
        var bug = new Bug();
        bug.Apply(BugTrigger.StartTriage);
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Triage_To_NeedMoreInfo_On_RequestMoreInfo()
    {
        var bug = MoveToTriage();
        bug.Apply(BugTrigger.RequestMoreInfo);
        Assert.AreEqual(BugState.NeedMoreInfo, bug.State);
    }

    [TestMethod]
    public void NeedMoreInfo_To_Triage_On_ProvideMoreInfo()
    {
        var bug = MoveToNeedMoreInfo();
        bug.Apply(BugTrigger.ProvideMoreInfo);
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Triage_To_Deferred_On_Defer()
    {
        var bug = MoveToTriage();
        bug.Apply(BugTrigger.Defer);
        Assert.AreEqual(BugState.Deferred, bug.State);
    }

    [TestMethod]
    public void Deferred_To_Triage_On_Resume()
    {
        var bug = MoveToDeferred();
        bug.Apply(BugTrigger.Resume);
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Triage_To_InProgress_On_StartFix()
    {
        var bug = MoveToTriage();
        bug.Apply(BugTrigger.StartFix);
        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void InProgress_To_Resolved_On_Resolve()
    {
        var bug = MoveToInProgress();
        bug.Apply(BugTrigger.Resolve);
        Assert.AreEqual(BugState.Resolved, bug.State);
    }

    [TestMethod]
    public void Resolved_To_Closed_On_Close()
    {
        var bug = MoveToResolved();
        bug.Apply(BugTrigger.Close);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Resolved_To_Reopened_On_Reopen()
    {
        var bug = MoveToResolved();
        bug.Apply(BugTrigger.Reopen);
        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void Closed_To_Reopened_On_Reopen()
    {
        var bug = MoveToClosed();
        bug.Apply(BugTrigger.Reopen);
        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void Reopened_To_Triage_On_ReturnToTriage()
    {
        var bug = MoveToReopened();
        bug.Apply(BugTrigger.ReturnToTriage);
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Triage_To_Rejected_On_MarkAsDuplicate()
    {
        var bug = MoveToTriage();
        bug.Apply(BugTrigger.MarkAsDuplicate);
        Assert.AreEqual(BugState.Rejected, bug.State);
    }

    [TestMethod]
    public void Triage_To_Rejected_On_MarkAsNotBug()
    {
        var bug = MoveToTriage();
        bug.Apply(BugTrigger.MarkAsNotBug);
        Assert.AreEqual(BugState.Rejected, bug.State);
    }

    [TestMethod]
    public void Triage_To_Rejected_On_MarkAsNotReproducible()
    {
        var bug = MoveToTriage();
        bug.Apply(BugTrigger.MarkAsNotReproducible);
        Assert.AreEqual(BugState.Rejected, bug.State);
    }

    [TestMethod]
    public void Rejected_To_Reopened_On_Reopen()
    {
        var bug = MoveToRejected();
        bug.Apply(BugTrigger.Reopen);
        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void InProgress_To_NeedMoreInfo_On_RequestMoreInfo()
    {
        var bug = MoveToInProgress();
        bug.Apply(BugTrigger.RequestMoreInfo);
        Assert.AreEqual(BugState.NeedMoreInfo, bug.State);
    }

    [TestMethod]
    public void CanApply_Returns_True_For_Valid_Trigger()
    {
        var bug = MoveToTriage();
        Assert.IsTrue(bug.CanApply(BugTrigger.StartFix));
    }

    [TestMethod]
    public void CanApply_Returns_False_For_Invalid_Trigger()
    {
        var bug = new Bug();
        Assert.IsFalse(bug.CanApply(BugTrigger.Resolve));
    }

    [TestMethod]
    public void Apply_Throws_InvalidOperationException_When_Trigger_Not_Permitted()
    {
        var bug = new Bug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Apply(BugTrigger.Resolve));
    }

    [TestMethod]
    public void Apply_Throws_InvalidOperationException_When_Close_From_Triage()
    {
        var bug = MoveToTriage();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Apply(BugTrigger.Close));
    }

    [TestMethod]
    public void Apply_Throws_InvalidOperationException_When_StartFix_From_Closed()
    {
        var bug = MoveToClosed();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Apply(BugTrigger.StartFix));
    }

    [TestMethod]
    public void Apply_Throws_InvalidOperationException_When_ProvideMoreInfo_From_Triage()
    {
        var bug = MoveToTriage();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Apply(BugTrigger.ProvideMoreInfo));
    }

    private static Bug MoveToTriage()
    {
        var bug = new Bug();
        bug.Apply(BugTrigger.StartTriage);
        return bug;
    }

    private static Bug MoveToNeedMoreInfo()
    {
        var bug = MoveToTriage();
        bug.Apply(BugTrigger.RequestMoreInfo);
        return bug;
    }

    private static Bug MoveToDeferred()
    {
        var bug = MoveToTriage();
        bug.Apply(BugTrigger.Defer);
        return bug;
    }

    private static Bug MoveToInProgress()
    {
        var bug = MoveToTriage();
        bug.Apply(BugTrigger.StartFix);
        return bug;
    }

    private static Bug MoveToResolved()
    {
        var bug = MoveToInProgress();
        bug.Apply(BugTrigger.Resolve);
        return bug;
    }

    private static Bug MoveToClosed()
    {
        var bug = MoveToResolved();
        bug.Apply(BugTrigger.Close);
        return bug;
    }

    private static Bug MoveToReopened()
    {
        var bug = MoveToClosed();
        bug.Apply(BugTrigger.Reopen);
        return bug;
    }

    private static Bug MoveToRejected()
    {
        var bug = MoveToTriage();
        bug.Apply(BugTrigger.MarkAsDuplicate);
        return bug;
    }
}
