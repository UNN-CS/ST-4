using BugPro;

namespace BugTests;

[TestClass]
public class BugStateMachineTests
{
    private static Bug CreateBug() => new("Test bug");

    [TestMethod]
    public void NewBug_IsInOpenState()
    {
        var bug = CreateBug();
        Assert.AreEqual(Bug.State.Open, bug.CurrentState);
    }

    [TestMethod]
    public void Open_CanAssign_CannotCloseOrDefer()
    {
        var bug = CreateBug();
        Assert.IsTrue(bug.CanAssign);
        Assert.IsFalse(bug.CanClose);
        Assert.IsFalse(bug.CanDefer);
        Assert.IsFalse(bug.CanStartWork);
    }

    [TestMethod]
    public void Assign_FromOpen_MovesToAssigned()
    {
        var bug = CreateBug();
        bug.Assign("Alice");
        Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
        Assert.AreEqual("Alice", bug.Assignee);
    }

    [TestMethod]
    public void Assigned_CanStartWorkDeferCloseOrReassign()
    {
        var bug = CreateBug();
        bug.Assign("Bob");
        Assert.IsTrue(bug.CanStartWork);
        Assert.IsTrue(bug.CanDefer);
        Assert.IsTrue(bug.CanClose);
        Assert.IsTrue(bug.CanAssign);
        Assert.IsFalse(bug.CanResolve);
    }

    [TestMethod]
    public void Reassign_InAssigned_UpdatesAssignee()
    {
        var bug = CreateBug();
        bug.Assign("Bob");
        bug.Assign("Carol");
        Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
        Assert.AreEqual("Carol", bug.Assignee);
    }

    [TestMethod]
    public void StartWork_FromAssigned_MovesToInProgress()
    {
        var bug = CreateBug();
        bug.Assign("Dev");
        bug.StartWork();
        Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
    }

    [TestMethod]
    public void Close_FromAssigned_SkipsToClosed()
    {
        var bug = CreateBug();
        bug.Assign("Dev");
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void InProgress_CanResolveOrDefer_NotVerify()
    {
        var bug = CreateBug();
        bug.Assign("Dev");
        bug.StartWork();
        Assert.IsTrue(bug.CanResolve);
        Assert.IsTrue(bug.CanDefer);
        Assert.IsFalse(bug.CanVerify);
        Assert.IsFalse(bug.CanAssign);
    }

    [TestMethod]
    public void Resolve_FromInProgress_MovesToResolved()
    {
        var bug = CreateBug();
        bug.Assign("Dev");
        bug.StartWork();
        bug.Resolve();
        Assert.AreEqual(Bug.State.Resolved, bug.CurrentState);
    }

    [TestMethod]
    public void Resolved_CanVerify_NotClose()
    {
        var bug = CreateBug();
        bug.Assign("Dev");
        bug.StartWork();
        bug.Resolve();
        Assert.IsTrue(bug.CanVerify);
        Assert.IsFalse(bug.CanClose);
    }

    [TestMethod]
    public void Verify_FromResolved_MovesToVerified()
    {
        var bug = CreateBug();
        bug.Assign("Dev");
        bug.StartWork();
        bug.Resolve();
        bug.Verify();
        Assert.AreEqual(Bug.State.Verified, bug.CurrentState);
    }

    [TestMethod]
    public void Close_FromVerified_MovesToClosed()
    {
        var bug = FullLifecycleBug();
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
        Assert.IsTrue(bug.CanReopen);
    }

    [TestMethod]
    public void Defer_FromAssigned_ClearsAssignee()
    {
        var bug = CreateBug();
        bug.Assign("Anna");
        bug.Defer();
        Assert.AreEqual(Bug.State.Deferred, bug.CurrentState);
        Assert.IsNull(bug.Assignee);
    }

    [TestMethod]
    public void Defer_FromInProgress_MovesToDeferred()
    {
        var bug = CreateBug();
        bug.Assign("Anna");
        bug.StartWork();
        bug.Defer();
        Assert.AreEqual(Bug.State.Deferred, bug.CurrentState);
    }

    [TestMethod]
    public void Assign_FromDeferred_ReturnsToAssigned()
    {
        var bug = CreateBug();
        bug.Assign("Anna");
        bug.Defer();
        bug.Assign("Boris");
        Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
        Assert.AreEqual("Boris", bug.Assignee);
    }

    [TestMethod]
    public void Deferred_CannotCloseOrStartWork()
    {
        var bug = CreateBug();
        bug.Assign("Anna");
        bug.Defer();
        Assert.IsFalse(bug.CanClose);
        Assert.IsFalse(bug.CanStartWork);
        Assert.IsTrue(bug.CanAssign);
    }

    [TestMethod]
    public void Reopen_FromClosed_MovesToReopened()
    {
        var bug = FullLifecycleBug();
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void Assign_FromReopened_MovesToAssigned()
    {
        var bug = FullLifecycleBug();
        bug.Reopen();
        bug.Assign("NewOwner");
        Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
        Assert.AreEqual("NewOwner", bug.Assignee);
    }

    [TestMethod]
    public void Title_IsPreserved()
    {
        var bug = new Bug("My specific title");
        Assert.AreEqual("My specific title", bug.Title);
    }

    [TestMethod]
    public void Constructor_NullTitle_ThrowsArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new Bug(null!));
    }

    [TestMethod]
    public void ToDotGraph_ReturnsNonEmptyString()
    {
        var bug = CreateBug();
        bug.Assign("X");
        var graph = bug.ToDotGraph();
        Assert.IsFalse(string.IsNullOrWhiteSpace(graph));
        StringAssert.Contains(graph, "digraph");
    }

    [TestMethod]
    public void Close_FromOpen_ThrowsInvalidOperationException()
    {
        var bug = CreateBug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void Defer_FromOpen_ThrowsInvalidOperationException()
    {
        var bug = CreateBug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void StartWork_FromOpen_ThrowsInvalidOperationException()
    {
        var bug = CreateBug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.StartWork());
    }

    [TestMethod]
    public void Resolve_FromAssigned_ThrowsInvalidOperationException()
    {
        var bug = CreateBug();
        bug.Assign("Dev");
        Assert.ThrowsException<InvalidOperationException>(() => bug.Resolve());
    }

    [TestMethod]
    public void Verify_FromInProgress_ThrowsInvalidOperationException()
    {
        var bug = CreateBug();
        bug.Assign("Dev");
        bug.StartWork();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Verify());
    }

    [TestMethod]
    public void Reopen_FromOpen_ThrowsInvalidOperationException()
    {
        var bug = CreateBug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Reopen());
    }

    [TestMethod]
    public void Close_FromDeferred_ThrowsInvalidOperationException()
    {
        var bug = CreateBug();
        bug.Assign("Anna");
        bug.Defer();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void Assign_FromClosed_ThrowsInvalidOperationException()
    {
        var bug = FullLifecycleBug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Assign("Nobody"));
    }

    private static Bug FullLifecycleBug()
    {
        var bug = CreateBug();
        bug.Assign("Dev");
        bug.StartWork();
        bug.Resolve();
        bug.Verify();
        bug.Close();
        return bug;
    }
}
