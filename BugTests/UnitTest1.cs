using BugPro;

namespace BugTests;

[TestClass]
public sealed class UnitTest1
{
    [TestMethod]
    public void Constructor_SetsInitialStateToNew()
    {
        var bug = CreateBug();

        Assert.AreEqual(BugState.New, bug.State);
    }

    [TestMethod]
    public void Constructor_SavesTitle()
    {
        var bug = new Bug("Search button is disabled");

        Assert.AreEqual("Search button is disabled", bug.Title);
    }

    [TestMethod]
    public void Constructor_WithEmptyTitle_ThrowsArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => new Bug(" "));
    }

    [TestMethod]
    public void Triage_FromNew_MovesToTriaged()
    {
        var bug = CreateBug();

        bug.Triage();

        Assert.AreEqual(BugState.Triaged, bug.State);
    }

    [TestMethod]
    public void Reject_FromNew_MovesToRejected()
    {
        var bug = CreateBug();

        bug.Reject();

        Assert.AreEqual(BugState.Rejected, bug.State);
    }

    [TestMethod]
    public void Defer_FromNew_MovesToDeferred()
    {
        var bug = CreateBug();

        bug.Defer();

        Assert.AreEqual(BugState.Deferred, bug.State);
    }

    [TestMethod]
    public void Reconsider_FromDeferred_MovesToTriaged()
    {
        var bug = CreateBug();
        bug.Defer();

        bug.Reconsider();

        Assert.AreEqual(BugState.Triaged, bug.State);
    }

    [TestMethod]
    public void Assign_FromTriaged_MovesToAssigned()
    {
        var bug = CreateTriagedBug();

        bug.AssignTo("Alice");

        Assert.AreEqual(BugState.Assigned, bug.State);
    }

    [TestMethod]
    public void Assign_FromTriaged_SavesAssignee()
    {
        var bug = CreateTriagedBug();

        bug.AssignTo("Alice");

        Assert.AreEqual("Alice", bug.Assignee);
    }

    [TestMethod]
    public void Assign_WithEmptyAssignee_ThrowsArgumentException()
    {
        var bug = CreateTriagedBug();

        Assert.ThrowsException<ArgumentException>(() => bug.AssignTo(""));
    }

    [TestMethod]
    public void StartProgress_FromAssigned_MovesToInProgress()
    {
        var bug = CreateAssignedBug();

        bug.StartProgress();

        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void Resolve_FromInProgress_MovesToFixed()
    {
        var bug = CreateInProgressBug();

        bug.Resolve("Fixed null reference.");

        Assert.AreEqual(BugState.Fixed, bug.State);
    }

    [TestMethod]
    public void Resolve_FromInProgress_SavesResolution()
    {
        var bug = CreateInProgressBug();

        bug.Resolve("Fixed null reference.");

        Assert.AreEqual("Fixed null reference.", bug.Resolution);
    }

    [TestMethod]
    public void Resolve_WithEmptyText_ThrowsArgumentException()
    {
        var bug = CreateInProgressBug();

        Assert.ThrowsException<ArgumentException>(() => bug.Resolve(" "));
    }

    [TestMethod]
    public void SendToTesting_FromFixed_MovesToReadyForTesting()
    {
        var bug = CreateFixedBug();

        bug.SendToTesting();

        Assert.AreEqual(BugState.ReadyForTesting, bug.State);
    }

    [TestMethod]
    public void Verify_FromReadyForTesting_MovesToClosed()
    {
        var bug = CreateReadyForTestingBug();

        bug.Verify();

        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Verify_SetsIsClosedToTrue()
    {
        var bug = CreateReadyForTestingBug();

        bug.Verify();

        Assert.IsTrue(bug.IsClosed);
    }

    [TestMethod]
    public void Reopen_FromReadyForTesting_MovesToReopened()
    {
        var bug = CreateReadyForTestingBug();

        bug.Reopen();

        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void Reopen_FromClosed_MovesToReopened()
    {
        var bug = CreateClosedBug();

        bug.Reopen();

        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void Reopen_IncrementsCounter()
    {
        var bug = CreateClosedBug();

        bug.Reopen();

        Assert.AreEqual(1, bug.ReopenCount);
    }

    [TestMethod]
    public void Assign_FromReopened_MovesToAssigned()
    {
        var bug = CreateReopenedBug();

        bug.AssignTo("Bob");

        Assert.AreEqual(BugState.Assigned, bug.State);
    }

    [TestMethod]
    public void StartProgress_FromReopened_MovesToInProgress()
    {
        var bug = CreateReopenedBug();

        bug.StartProgress();

        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void Defer_FromAssigned_MovesToDeferred()
    {
        var bug = CreateAssignedBug();

        bug.Defer();

        Assert.AreEqual(BugState.Deferred, bug.State);
    }

    [TestMethod]
    public void PermittedTriggers_ForNewState_AreCorrect()
    {
        var bug = CreateBug();
        var triggers = bug.PermittedTriggers.ToHashSet();

        CollectionAssert.AreEquivalent(
            new[] { BugTrigger.Triage, BugTrigger.Reject, BugTrigger.Defer },
            triggers.ToArray());
    }

    [TestMethod]
    public void CanFire_ReturnsFalseForInvalidTrigger()
    {
        var bug = CreateBug();

        Assert.IsFalse(bug.CanFire(BugTrigger.StartProgress));
    }

    [TestMethod]
    public void InvalidTransition_FromNew_ThrowsInvalidOperationException()
    {
        var bug = CreateBug();

        Assert.ThrowsException<InvalidOperationException>(() => bug.StartProgress());
    }

    [TestMethod]
    public void InvalidTransition_FromRejected_ThrowsInvalidOperationException()
    {
        var bug = CreateBug();
        bug.Reject();

        Assert.ThrowsException<InvalidOperationException>(() => bug.Reconsider());
    }

    [TestMethod]
    public void InvalidTransition_FromFixed_ThrowsInvalidOperationException()
    {
        var bug = CreateFixedBug();

        Assert.ThrowsException<InvalidOperationException>(() => bug.Verify());
    }

    [TestMethod]
    public void FullHappyPath_EndsInClosedState()
    {
        var bug = CreateBug();

        bug.Triage();
        bug.AssignTo("Alice");
        bug.StartProgress();
        bug.Resolve("Added input validation.");
        bug.SendToTesting();
        bug.Verify();

        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void History_ContainsTransitionRecords()
    {
        var bug = CreateBug();
        bug.Triage();
        bug.AssignTo("Alice");

        Assert.IsTrue(bug.History.Any(record => record.Contains("New --Triage--> Triaged", StringComparison.Ordinal)));
        Assert.IsTrue(bug.History.Any(record => record.Contains("Assigned to Alice.", StringComparison.Ordinal)));
    }

    [TestMethod]
    public void Reopen_AfterClose_MakesIsClosedFalse()
    {
        var bug = CreateClosedBug();

        bug.Reopen();

        Assert.IsFalse(bug.IsClosed);
    }

    private static Bug CreateBug() => new("Login page returns 500");

    private static Bug CreateTriagedBug()
    {
        var bug = CreateBug();
        bug.Triage();
        return bug;
    }

    private static Bug CreateAssignedBug()
    {
        var bug = CreateTriagedBug();
        bug.AssignTo("Alice");
        return bug;
    }

    private static Bug CreateInProgressBug()
    {
        var bug = CreateAssignedBug();
        bug.StartProgress();
        return bug;
    }

    private static Bug CreateFixedBug()
    {
        var bug = CreateInProgressBug();
        bug.Resolve("Fixed null reference.");
        return bug;
    }

    private static Bug CreateReadyForTestingBug()
    {
        var bug = CreateFixedBug();
        bug.SendToTesting();
        return bug;
    }

    private static Bug CreateClosedBug()
    {
        var bug = CreateReadyForTestingBug();
        bug.Verify();
        return bug;
    }

    private static Bug CreateReopenedBug()
    {
        var bug = CreateClosedBug();
        bug.Reopen();
        return bug;
    }
}
