using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void Constructor_SetsInitialStateToNew()
    {
        var bug = CreateBug();

        Assert.AreEqual(BugState.New, bug.State);
    }

    [TestMethod]
    public void Constructor_WithEmptyTitle_ThrowsArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => new Bug(" "));
    }

    [TestMethod]
    public void NewBug_CanStartTriage()
    {
        var bug = CreateBug();

        Assert.IsTrue(bug.CanFire(BugTrigger.StartTriage));
    }

    [TestMethod]
    public void NewBug_HasSinglePermittedTrigger()
    {
        var bug = CreateBug();

        CollectionAssert.AreEquivalent(
            new[] { BugTrigger.StartTriage },
            bug.PermittedTriggers.ToArray());
    }

    [TestMethod]
    public void StartTriage_MovesBugToTriage()
    {
        var bug = CreateBug();

        bug.StartTriage();

        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void RequestInfo_FromTriage_MovesBugToWaitingForInfo()
    {
        var bug = CreateTriagedBug();

        bug.RequestInfo();

        Assert.AreEqual(BugState.WaitingForInfo, bug.State);
    }

    [TestMethod]
    public void Triage_HasExpectedPermittedTriggers()
    {
        var bug = CreateTriagedBug();

        CollectionAssert.AreEquivalent(
            new[]
            {
                BugTrigger.RequestInfo,
                BugTrigger.Postpone,
                BugTrigger.StartFix,
                BugTrigger.RejectAsNotABug,
                BugTrigger.RejectAsDuplicate,
                BugTrigger.RejectAsCannotReproduce,
                BugTrigger.RejectAsWontFix
            },
            bug.PermittedTriggers.ToArray());
    }

    [TestMethod]
    public void ProvideInfo_FromWaitingForInfo_ReturnsBugToTriage()
    {
        var bug = CreateTriagedBug();
        bug.RequestInfo();

        bug.ProvideInfo();

        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Postpone_FromTriage_MovesBugToPostponed()
    {
        var bug = CreateTriagedBug();

        bug.Postpone();

        Assert.AreEqual(BugState.Postponed, bug.State);
    }

    [TestMethod]
    public void Resume_FromPostponed_ReturnsBugToTriage()
    {
        var bug = CreateTriagedBug();
        bug.Postpone();

        bug.Resume();

        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void StartFix_FromTriage_MovesBugToInProgress()
    {
        var bug = CreateTriagedBug();

        bug.StartFix();

        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void RequestInfo_FromInProgress_ReturnsBugToWaitingForInfo()
    {
        var bug = CreateInProgressBug();

        bug.RequestInfo();

        Assert.AreEqual(BugState.WaitingForInfo, bug.State);
    }

    [TestMethod]
    public void Resolve_FromInProgress_MovesBugToResolved()
    {
        var bug = CreateInProgressBug();

        bug.Resolve();

        Assert.AreEqual(BugState.Resolved, bug.State);
    }

    [TestMethod]
    public void Close_FromResolved_MovesBugToClosed()
    {
        var bug = CreateResolvedBug();

        bug.Close();

        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Reopen_FromClosed_MovesBugToReopened()
    {
        var bug = CreateClosedBug();

        bug.Reopen();

        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void StartFix_FromReopened_MovesBugToInProgress()
    {
        var bug = CreateClosedBug();
        bug.Reopen();

        bug.StartFix();

        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void SendToTriage_FromReopened_MovesBugToTriage()
    {
        var bug = CreateClosedBug();
        bug.Reopen();

        bug.SendToTriage();

        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void RejectAsNotABug_FromTriage_MovesBugToRejected()
    {
        var bug = CreateTriagedBug();

        bug.RejectAsNotABug();

        Assert.AreEqual(BugState.Rejected, bug.State);
    }

    [TestMethod]
    public void RejectAsDuplicate_FromTriage_MovesBugToRejected()
    {
        var bug = CreateTriagedBug();

        bug.RejectAsDuplicate();

        Assert.AreEqual(BugState.Rejected, bug.State);
    }

    [TestMethod]
    public void RejectAsCannotReproduce_FromTriage_MovesBugToRejected()
    {
        var bug = CreateTriagedBug();

        bug.RejectAsCannotReproduce();

        Assert.AreEqual(BugState.Rejected, bug.State);
    }

    [TestMethod]
    public void RejectAsWontFix_FromTriage_MovesBugToRejected()
    {
        var bug = CreateTriagedBug();

        bug.RejectAsWontFix();

        Assert.AreEqual(BugState.Rejected, bug.State);
    }

    [TestMethod]
    public void Reopen_FromRejected_MovesBugToReopened()
    {
        var bug = CreateTriagedBug();
        bug.RejectAsDuplicate();

        bug.Reopen();

        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void Resolve_FromNew_ThrowsInvalidOperationException()
    {
        var bug = CreateBug();

        Assert.ThrowsException<InvalidOperationException>(() => bug.Resolve());
    }

    [TestMethod]
    public void ProvideInfo_FromTriage_ThrowsInvalidOperationException()
    {
        var bug = CreateTriagedBug();

        Assert.ThrowsException<InvalidOperationException>(() => bug.ProvideInfo());
    }

    [TestMethod]
    public void Resume_FromWaitingForInfo_ThrowsInvalidOperationException()
    {
        var bug = CreateTriagedBug();
        bug.RequestInfo();

        Assert.ThrowsException<InvalidOperationException>(() => bug.Resume());
    }

    [TestMethod]
    public void Close_FromTriage_ThrowsInvalidOperationException()
    {
        var bug = CreateTriagedBug();

        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void HappyPath_ReachesClosedState()
    {
        var bug = CreateBug();

        bug.StartTriage();
        bug.StartFix();
        bug.Resolve();
        bug.Close();

        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void InfoRequestPath_ReachesClosedStateAfterReturningToTriage()
    {
        var bug = CreateBug();

        bug.StartTriage();
        bug.RequestInfo();
        bug.ProvideInfo();
        bug.StartFix();
        bug.Resolve();
        bug.Close();

        Assert.AreEqual(BugState.Closed, bug.State);
    }

    private static Bug CreateBug() => new("Checkout button is broken");

    private static Bug CreateTriagedBug()
    {
        var bug = CreateBug();
        bug.StartTriage();
        return bug;
    }

    private static Bug CreateInProgressBug()
    {
        var bug = CreateTriagedBug();
        bug.StartFix();
        return bug;
    }

    private static Bug CreateResolvedBug()
    {
        var bug = CreateInProgressBug();
        bug.Resolve();
        return bug;
    }

    private static Bug CreateClosedBug()
    {
        var bug = CreateResolvedBug();
        bug.Close();
        return bug;
    }
}
