using BugPro;

namespace BugTests;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void InitialState_IsNew()
    {
        var bug = new Bug();
        Assert.AreEqual(Bug.State.New, bug.CurrentState);
    }

    [TestMethod]
    public void OpenForTriaging_MovesFromNewToTriaging()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.OpenForTriaging);
        Assert.AreEqual(Bug.State.Triaging, bug.CurrentState);
    }

    [TestMethod]
    public void NeedMoreInfo_MovesTriagingToWaitingForInfo()
    {
        var bug = CreateTriagedBug();
        bug.Fire(Bug.Trigger.NeedMoreInfo);
        Assert.AreEqual(Bug.State.WaitingForInfo, bug.CurrentState);
    }

    [TestMethod]
    public void InfoProvided_MovesWaitingForInfoToTriaging()
    {
        var bug = CreateTriagedBug();
        bug.Fire(Bug.Trigger.NeedMoreInfo);
        bug.Fire(Bug.Trigger.InfoProvided);
        Assert.AreEqual(Bug.State.Triaging, bug.CurrentState);
    }

    [TestMethod]
    public void Postpone_MovesTriagingToPostponed()
    {
        var bug = CreateTriagedBug();
        bug.Fire(Bug.Trigger.Postpone);
        Assert.AreEqual(Bug.State.Postponed, bug.CurrentState);
    }

    [TestMethod]
    public void ResumeTriaging_MovesPostponedToTriaging()
    {
        var bug = CreateTriagedBug();
        bug.Fire(Bug.Trigger.Postpone);
        bug.Fire(Bug.Trigger.ResumeTriaging);
        Assert.AreEqual(Bug.State.Triaging, bug.CurrentState);
    }

    [TestMethod]
    public void StartFix_MovesTriagingToInProgress()
    {
        var bug = CreateTriagedBug();
        bug.Fire(Bug.Trigger.StartFix);
        Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
    }

    [TestMethod]
    public void RequestMoreInfoFromFix_MovesInProgressToWaitingForInfo()
    {
        var bug = CreateInProgressBug();
        bug.Fire(Bug.Trigger.RequestMoreInfoFromFix);
        Assert.AreEqual(Bug.State.WaitingForInfo, bug.CurrentState);
    }

    [TestMethod]
    public void Resolve_MovesInProgressToResolved()
    {
        var bug = CreateInProgressBug();
        bug.Fire(Bug.Trigger.Resolve);
        Assert.AreEqual(Bug.State.Resolved, bug.CurrentState);
    }

    [TestMethod]
    public void VerifyPassed_MovesResolvedToClosed()
    {
        var bug = CreateResolvedBug();
        bug.Fire(Bug.Trigger.VerifyPassed);
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void VerifyFailed_MovesResolvedToReopened()
    {
        var bug = CreateResolvedBug();
        bug.Fire(Bug.Trigger.VerifyFailed);
        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void Reevaluate_MovesReopenedToTriaging()
    {
        var bug = CreateReopenedBug();
        bug.Fire(Bug.Trigger.Reevaluate);
        Assert.AreEqual(Bug.State.Triaging, bug.CurrentState);
    }

    [TestMethod]
    public void Reopen_MovesClosedToReopened()
    {
        var bug = CreateClosedBug();
        bug.Fire(Bug.Trigger.Reopen);
        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void MarkAsNotABug_MovesTriagingToRejected()
    {
        var bug = CreateTriagedBug();
        bug.Fire(Bug.Trigger.MarkAsNotABug);
        Assert.AreEqual(Bug.State.Rejected, bug.CurrentState);
    }

    [TestMethod]
    public void MarkAsDuplicate_MovesTriagingToDuplicate()
    {
        var bug = CreateTriagedBug();
        bug.Fire(Bug.Trigger.MarkAsDuplicate);
        Assert.AreEqual(Bug.State.Duplicate, bug.CurrentState);
    }

    [TestMethod]
    public void MarkAsCannotReproduce_MovesTriagingToCannotReproduce()
    {
        var bug = CreateTriagedBug();
        bug.Fire(Bug.Trigger.MarkAsCannotReproduce);
        Assert.AreEqual(Bug.State.CannotReproduce, bug.CurrentState);
    }

    [TestMethod]
    public void Reopen_MovesRejectedToReopened()
    {
        var bug = CreateTriagedBug();
        bug.Fire(Bug.Trigger.MarkAsNotABug);
        bug.Fire(Bug.Trigger.Reopen);
        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void CanFire_ReturnsTrue_ForAllowedTrigger()
    {
        var bug = new Bug();
        Assert.IsTrue(bug.CanFire(Bug.Trigger.OpenForTriaging));
    }

    [TestMethod]
    public void CanFire_ReturnsFalse_ForForbiddenTrigger()
    {
        var bug = new Bug();
        Assert.IsFalse(bug.CanFire(Bug.Trigger.Resolve));
    }

    [TestMethod]
    public void PermittedTriggers_InNewState_ContainsOnlyOpenForTriaging()
    {
        var bug = new Bug();
        CollectionAssert.AreEquivalent(
            new[] { Bug.Trigger.OpenForTriaging },
            bug.GetPermittedTriggers().ToArray());
    }

    [TestMethod]
    public void Fire_ThrowsInvalidOperation_ForIllegalTriggerInNew()
    {
        var bug = new Bug();
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.Fire(Bug.Trigger.Resolve));
    }

    [TestMethod]
    public void Fire_ThrowsInvalidOperation_ForIllegalTriggerInTriaging()
    {
        var bug = CreateTriagedBug();
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.Fire(Bug.Trigger.VerifyPassed));
    }

    [TestMethod]
    public void Fire_ThrowsInvalidOperation_ForSecondResolve()
    {
        var bug = CreateResolvedBug();
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.Fire(Bug.Trigger.Resolve));
    }

    [TestMethod]
    public void FullHappyPath_EndsInClosed()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.OpenForTriaging);
        bug.Fire(Bug.Trigger.StartFix);
        bug.Fire(Bug.Trigger.Resolve);
        bug.Fire(Bug.Trigger.VerifyPassed);
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void ReopenLoop_WorksAfterVerificationFailure()
    {
        var bug = CreateResolvedBug();
        bug.Fire(Bug.Trigger.VerifyFailed);
        bug.Fire(Bug.Trigger.Reevaluate);
        bug.Fire(Bug.Trigger.StartFix);
        Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
    }

    private static Bug CreateTriagedBug()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.OpenForTriaging);
        return bug;
    }

    private static Bug CreateInProgressBug()
    {
        var bug = CreateTriagedBug();
        bug.Fire(Bug.Trigger.StartFix);
        return bug;
    }

    private static Bug CreateResolvedBug()
    {
        var bug = CreateInProgressBug();
        bug.Fire(Bug.Trigger.Resolve);
        return bug;
    }

    private static Bug CreateReopenedBug()
    {
        var bug = CreateResolvedBug();
        bug.Fire(Bug.Trigger.VerifyFailed);
        return bug;
    }

    private static Bug CreateClosedBug()
    {
        var bug = CreateResolvedBug();
        bug.Fire(Bug.Trigger.VerifyPassed);
        return bug;
    }
}
