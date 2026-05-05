using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public sealed class UnitTest1
{
    [TestMethod]
    public void NewBug_HasNewState()
    {
        var bug = new Bug("Login button is broken");

        Assert.AreEqual(BugState.New, bug.State);
        Assert.IsFalse(bug.IsClosed);
        Assert.AreEqual(1, bug.History.Count);
    }

    [TestMethod]
    public void Constructor_WithEmptyTitle_ThrowsArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => new Bug("   "));
    }

    [TestMethod]
    public void Open_FromNew_MovesBugToOpen()
    {
        var bug = new Bug("Login button is broken");

        bug.Fire(BugTrigger.Open);

        Assert.AreEqual(BugState.Open, bug.State);
    }

    [TestMethod]
    public void Open_FromOpen_ThrowsStatelessInvalidOperationException()
    {
        var bug = CreateOpenBug();

        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Open));
        Assert.AreEqual(BugState.Open, bug.State);
    }

    [TestMethod]
    public void Close_FromNew_ThrowsStatelessInvalidOperationException()
    {
        var bug = new Bug("Login button is broken");

        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Close));
        Assert.AreEqual(BugState.New, bug.State);
    }

    [TestMethod]
    public void SubmitForReview_FromOpen_ThrowsStatelessInvalidOperationException()
    {
        var bug = CreateOpenBug();

        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.SubmitForReview));
        Assert.AreEqual(BugState.Open, bug.State);
    }

    [TestMethod]
    public void HappyPath_ReachesClosedState()
    {
        var bug = CreateClosedBug();

        Assert.AreEqual(BugState.Closed, bug.State);
        Assert.IsTrue(bug.IsClosed);
        Assert.IsNotNull(bug.ClosedAt);
    }

    [TestMethod]
    public void Assign_FromOpen_MovesBugToInProgress()
    {
        var bug = CreateOpenBug();

        bug.Fire(BugTrigger.Assign);

        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void SubmitForReview_FromInProgress_MovesBugToCodeReview()
    {
        var bug = CreateInProgressBug();

        bug.Fire(BugTrigger.SubmitForReview);

        Assert.AreEqual(BugState.CodeReview, bug.State);
    }

    [TestMethod]
    public void RequestChanges_FromCodeReview_MovesBugBackToInProgress()
    {
        var bug = CreateBugInState(BugState.CodeReview);

        bug.Fire(BugTrigger.RequestChanges);

        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void ApproveReview_FromCodeReview_MovesBugToReadyForTesting()
    {
        var bug = CreateBugInState(BugState.CodeReview);

        bug.Fire(BugTrigger.ApproveReview);

        Assert.AreEqual(BugState.ReadyForTesting, bug.State);
    }

    [TestMethod]
    public void StartTesting_FromReadyForTesting_MovesBugToInTesting()
    {
        var bug = CreateBugInState(BugState.ReadyForTesting);

        bug.Fire(BugTrigger.StartTesting);

        Assert.AreEqual(BugState.InTesting, bug.State);
    }

    [TestMethod]
    public void PassTesting_FromInTesting_MovesBugToVerified()
    {
        var bug = CreateBugInState(BugState.InTesting);

        bug.Fire(BugTrigger.PassTesting);

        Assert.AreEqual(BugState.Verified, bug.State);
    }

    [TestMethod]
    public void FailTesting_FromInTesting_MovesBugToReopened()
    {
        var bug = CreateBugInState(BugState.InTesting);

        bug.Fire(BugTrigger.FailTesting);

        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void Resolve_FromVerified_MovesBugToResolved()
    {
        var bug = CreateBugInState(BugState.Verified);

        bug.Fire(BugTrigger.Resolve);

        Assert.AreEqual(BugState.Resolved, bug.State);
    }

    [TestMethod]
    public void Close_FromResolved_MovesBugToClosed()
    {
        var bug = CreateBugInState(BugState.Resolved);

        bug.Fire(BugTrigger.Close);

        Assert.AreEqual(BugState.Closed, bug.State);
        Assert.IsNotNull(bug.ClosedAt);
    }

    [TestMethod]
    public void Reopen_FromResolved_MovesBugToReopened()
    {
        var bug = CreateBugInState(BugState.Resolved);

        bug.Fire(BugTrigger.Reopen);

        Assert.AreEqual(BugState.Reopened, bug.State);
        Assert.IsFalse(bug.IsClosed);
    }

    [TestMethod]
    public void Assign_FromReopened_MovesBugToInProgress()
    {
        var bug = CreateBugInState(BugState.Reopened);

        bug.Fire(BugTrigger.Assign);

        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void Defer_FromOpen_MovesBugToDeferred()
    {
        var bug = CreateOpenBug();

        bug.Fire(BugTrigger.Defer);

        Assert.AreEqual(BugState.Deferred, bug.State);
    }

    [TestMethod]
    public void Resume_FromDeferred_MovesBugToOpen()
    {
        var bug = CreateBugInState(BugState.Deferred);

        bug.Fire(BugTrigger.Resume);

        Assert.AreEqual(BugState.Open, bug.State);
    }

    [TestMethod]
    public void Cancel_FromOpen_MovesBugToCancelled()
    {
        var bug = CreateOpenBug();

        bug.Fire(BugTrigger.Cancel);

        Assert.AreEqual(BugState.Cancelled, bug.State);
        Assert.IsTrue(bug.IsClosed);
        Assert.IsNotNull(bug.ClosedAt);
    }

    [TestMethod]
    public void Reopen_FromCancelled_ThrowsStatelessInvalidOperationException()
    {
        var bug = CreateOpenBug();
        bug.Fire(BugTrigger.Cancel);

        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Reopen));
        Assert.AreEqual(BugState.Cancelled, bug.State);
    }

    [TestMethod]
    public void Reopen_FromClosed_ThrowsStatelessInvalidOperationException()
    {
        var bug = CreateClosedBug();

        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Reopen));
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void CanFire_ReturnsTrueForAllowedTrigger()
    {
        var bug = new Bug("Login button is broken");

        Assert.IsTrue(bug.CanFire(BugTrigger.Open));
    }

    [TestMethod]
    public void CanFire_ReturnsFalseForForbiddenTrigger()
    {
        var bug = new Bug("Login button is broken");

        Assert.IsFalse(bug.CanFire(BugTrigger.Close));
    }

    [TestMethod]
    public void PermittedTriggers_FromOpen_ContainsAllowedWorkflowActions()
    {
        var bug = CreateOpenBug();

        CollectionAssert.AreEquivalent(
            new[] { BugTrigger.Assign, BugTrigger.Defer, BugTrigger.Cancel },
            bug.PermittedTriggers.ToArray());
    }

    [TestMethod]
    public void History_ContainsEveryTransition()
    {
        var bug = CreateClosedBug();

        Assert.AreEqual(9, bug.History.Count);
        StringAssert.Contains(bug.History[1], "New --Open--> Open");
        StringAssert.Contains(bug.History[^1], "Resolved --Close--> Closed");
    }

    [TestMethod]
    public void TryFire_WithForbiddenTrigger_ReturnsFalseAndKeepsState()
    {
        var bug = new Bug("Login button is broken");

        var result = bug.TryFire(BugTrigger.Close);

        Assert.IsFalse(result);
        Assert.AreEqual(BugState.New, bug.State);
    }

    [TestMethod]
    public void TryFire_WithAllowedTrigger_ReturnsTrueAndChangesState()
    {
        var bug = new Bug("Login button is broken");

        var result = bug.TryFire(BugTrigger.Open);

        Assert.IsTrue(result);
        Assert.AreEqual(BugState.Open, bug.State);
    }

    [TestMethod]
    public void UpdatedAt_ChangesAfterTransition()
    {
        var bug = new Bug("Login button is broken");
        var initialUpdatedAt = bug.UpdatedAt;

        bug.Fire(BugTrigger.Open);

        Assert.IsTrue(bug.UpdatedAt >= initialUpdatedAt);
    }

    private static Bug CreateOpenBug()
    {
        var bug = new Bug("Login button is broken");
        bug.Fire(BugTrigger.Open);
        return bug;
    }

    private static Bug CreateInProgressBug()
    {
        var bug = CreateOpenBug();
        bug.Fire(BugTrigger.Assign);
        return bug;
    }

    private static Bug CreateClosedBug()
    {
        var bug = new Bug("Login button is broken");

        foreach (var trigger in new[]
        {
            BugTrigger.Open,
            BugTrigger.Assign,
            BugTrigger.SubmitForReview,
            BugTrigger.ApproveReview,
            BugTrigger.StartTesting,
            BugTrigger.PassTesting,
            BugTrigger.Resolve,
            BugTrigger.Close
        })
        {
            bug.Fire(trigger);
        }

        return bug;
    }

    private static Bug CreateBugInState(BugState state)
    {
        return state switch
        {
            BugState.New => new Bug("Login button is broken"),
            BugState.Open => CreateOpenBug(),
            BugState.InProgress => CreateInProgressBug(),
            BugState.CodeReview => MoveThrough(BugTrigger.Open, BugTrigger.Assign, BugTrigger.SubmitForReview),
            BugState.ReadyForTesting => MoveThrough(BugTrigger.Open, BugTrigger.Assign, BugTrigger.SubmitForReview, BugTrigger.ApproveReview),
            BugState.InTesting => MoveThrough(BugTrigger.Open, BugTrigger.Assign, BugTrigger.SubmitForReview, BugTrigger.ApproveReview, BugTrigger.StartTesting),
            BugState.Verified => MoveThrough(BugTrigger.Open, BugTrigger.Assign, BugTrigger.SubmitForReview, BugTrigger.ApproveReview, BugTrigger.StartTesting, BugTrigger.PassTesting),
            BugState.Resolved => MoveThrough(BugTrigger.Open, BugTrigger.Assign, BugTrigger.SubmitForReview, BugTrigger.ApproveReview, BugTrigger.StartTesting, BugTrigger.PassTesting, BugTrigger.Resolve),
            BugState.Reopened => MoveThrough(BugTrigger.Open, BugTrigger.Assign, BugTrigger.SubmitForReview, BugTrigger.ApproveReview, BugTrigger.StartTesting, BugTrigger.FailTesting),
            BugState.Deferred => MoveThrough(BugTrigger.Open, BugTrigger.Defer),
            BugState.Cancelled => MoveThrough(BugTrigger.Open, BugTrigger.Cancel),
            BugState.Closed => CreateClosedBug(),
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }

    private static Bug MoveThrough(params BugTrigger[] triggers)
    {
        var bug = new Bug("Login button is broken");

        foreach (var trigger in triggers)
        {
            bug.Fire(trigger);
        }

        return bug;
    }
}
