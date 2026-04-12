using BugPro;

namespace BugTests;

[TestClass]
public class BugStateMachineTests
{
    [TestMethod]
    public void InitialState_IsOpen()
    {
        var bug = new Bug();
        Assert.AreEqual(Bug.State.Open, bug.CurrentState);
    }

    [TestMethod]
    public void Assign_TransitionsTo_Assigned()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.Assign);
        Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
    }

    [TestMethod]
    public void AssignThenStart_TransitionsTo_InProgress()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.Assign);
        bug.Fire(Bug.Trigger.Start);
        Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
    }

    [TestMethod]
    public void FullPath_WithReview_Reaches_Closed()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.Assign);
        bug.Fire(Bug.Trigger.Start);
        bug.Fire(Bug.Trigger.SubmitForReview);
        bug.Fire(Bug.Trigger.ApproveReview);
        bug.Fire(Bug.Trigger.Verify);
        bug.Fire(Bug.Trigger.Close);
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void InProgress_MarkFixed_SkipsReview_ToVerifiedPath()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.Assign);
        bug.Fire(Bug.Trigger.Start);
        bug.Fire(Bug.Trigger.MarkFixed);
        Assert.AreEqual(Bug.State.Fixed, bug.CurrentState);
        bug.Fire(Bug.Trigger.Verify);
        bug.Fire(Bug.Trigger.Close);
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void Defer_ThenResume_ReturnsTo_InProgress()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.Assign);
        bug.Fire(Bug.Trigger.Start);
        bug.Fire(Bug.Trigger.Defer);
        Assert.AreEqual(Bug.State.Deferred, bug.CurrentState);
        bug.Fire(Bug.Trigger.Resume);
        Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
    }

    [TestMethod]
    public void Assigned_MarkDuplicate_GoesTo_Duplicate()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.Assign);
        bug.Fire(Bug.Trigger.MarkDuplicate);
        Assert.AreEqual(Bug.State.Duplicate, bug.CurrentState);
    }

    [TestMethod]
    public void Duplicate_Reopen_ReturnsTo_Open()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.Assign);
        bug.Fire(Bug.Trigger.MarkDuplicate);
        bug.Fire(Bug.Trigger.Reopen);
        Assert.AreEqual(Bug.State.Open, bug.CurrentState);
    }

    [TestMethod]
    public void InReview_RequestChanges_ReturnsTo_InProgress()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.Assign);
        bug.Fire(Bug.Trigger.Start);
        bug.Fire(Bug.Trigger.SubmitForReview);
        Assert.AreEqual(Bug.State.InReview, bug.CurrentState);
        bug.Fire(Bug.Trigger.RequestChanges);
        Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
    }

    [TestMethod]
    public void Fixed_RequestChanges_ReturnsTo_InProgress()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.Assign);
        bug.Fire(Bug.Trigger.Start);
        bug.Fire(Bug.Trigger.MarkFixed);
        bug.Fire(Bug.Trigger.RequestChanges);
        Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
    }

    [TestMethod]
    public void Verified_Reopen_GoesTo_InProgress()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.Assign);
        bug.Fire(Bug.Trigger.Start);
        bug.Fire(Bug.Trigger.MarkFixed);
        bug.Fire(Bug.Trigger.Verify);
        bug.Fire(Bug.Trigger.Reopen);
        Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
    }

    [TestMethod]
    public void Closed_Reopen_GoesTo_Open()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.Assign);
        bug.Fire(Bug.Trigger.Start);
        bug.Fire(Bug.Trigger.MarkFixed);
        bug.Fire(Bug.Trigger.Verify);
        bug.Fire(Bug.Trigger.Close);
        bug.Fire(Bug.Trigger.Reopen);
        Assert.AreEqual(Bug.State.Open, bug.CurrentState);
    }

    [TestMethod]
    public void CanFire_FromOpen_AssignTrue_CloseFalse()
    {
        var bug = new Bug();
        Assert.IsTrue(bug.CanFire(Bug.Trigger.Assign));
        Assert.IsFalse(bug.CanFire(Bug.Trigger.Close));
    }

    [TestMethod]
    public void PermittedTriggers_FromOpen_ContainsOnly_Assign()
    {
        var bug = new Bug();
        CollectionAssert.AreEquivalent(
            new[] { Bug.Trigger.Assign },
            bug.PermittedTriggers.ToArray());
    }

    [TestMethod]
    public void Fire_Start_FromOpen_Throws_InvalidOperationException()
    {
        var bug = new Bug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Bug.Trigger.Start));
    }

    [TestMethod]
    public void Fire_Verify_FromInProgress_Throws_InvalidOperationException()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.Assign);
        bug.Fire(Bug.Trigger.Start);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Bug.Trigger.Verify));
    }

    [TestMethod]
    public void Fire_Close_FromAssigned_Throws_InvalidOperationException()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.Assign);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Bug.Trigger.Close));
    }

    [TestMethod]
    public void Fire_Resume_FromOpen_Throws_InvalidOperationException()
    {
        var bug = new Bug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Bug.Trigger.Resume));
    }

    [TestMethod]
    public void Fire_SubmitForReview_FromAssigned_Throws_InvalidOperationException()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.Assign);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Bug.Trigger.SubmitForReview));
    }

    [TestMethod]
    public void Fire_Reopen_FromFixed_Throws_InvalidOperationException()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.Assign);
        bug.Fire(Bug.Trigger.Start);
        bug.Fire(Bug.Trigger.MarkFixed);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Bug.Trigger.Reopen));
    }

    [TestMethod]
    public void InitialState_CanBeOverridden_ForTests()
    {
        var bug = new Bug(Bug.State.InProgress);
        Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
    }
}
