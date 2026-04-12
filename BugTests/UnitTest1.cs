using System;
using BugPro;
using Stateless;

namespace BugTests;

[TestClass]
public sealed class UnitTest1
{
    [TestMethod]
    public void NewBug_StartsInOpenState()
    {
        var bug = new Bug();
        Assert.AreEqual(Bug.State.Open, bug.CurrentState);
    }

    [TestMethod]
    public void NewBug_HasNoAssignee()
    {
        var bug = new Bug();
        Assert.IsNull(bug.Assignee);
    }

    [TestMethod]
    public void Open_Assign_MovesToAssigned()
    {
        var bug = new Bug();
        bug.Assign("Alice");
        Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
    }

    [TestMethod]
    public void Assign_StoresAssigneeName()
    {
        var bug = new Bug();
        bug.Assign("Alice");
        Assert.AreEqual("Alice", bug.Assignee);
    }

    [TestMethod]
    public void Assigned_Resolve_MovesToResolved()
    {
        var bug = new Bug();
        bug.Assign("Alice");
        bug.Resolve();
        Assert.AreEqual(Bug.State.Resolved, bug.CurrentState);
    }

    [TestMethod]
    public void Resolved_Close_MovesToClosed()
    {
        var bug = new Bug();
        bug.Assign("Alice");
        bug.Resolve();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void Closed_Reopen_MovesToOpen()
    {
        var bug = new Bug();
        bug.Assign("Alice");
        bug.Resolve();
        bug.Close();
        bug.Reopen();
        Assert.AreEqual(Bug.State.Open, bug.CurrentState);
    }

    [TestMethod]
    public void Resolved_Reopen_MovesToOpen()
    {
        var bug = new Bug();
        bug.Assign("Bob");
        bug.Resolve();
        bug.Reopen();
        Assert.AreEqual(Bug.State.Open, bug.CurrentState);
    }

    [TestMethod]
    public void Open_Defer_MovesToDeferred()
    {
        var bug = new Bug();
        bug.Defer();
        Assert.AreEqual(Bug.State.Deferred, bug.CurrentState);
    }

    [TestMethod]
    public void Assigned_Defer_MovesToDeferred()
    {
        var bug = new Bug();
        bug.Assign("Carl");
        bug.Defer();
        Assert.AreEqual(Bug.State.Deferred, bug.CurrentState);
    }

    [TestMethod]
    public void Deferred_ClearsAssignee()
    {
        var bug = new Bug();
        bug.Assign("Carl");
        bug.Defer();
        Assert.IsNull(bug.Assignee);
    }

    [TestMethod]
    public void Deferred_Reactivate_MovesToOpen()
    {
        var bug = new Bug();
        bug.Defer();
        bug.Reactivate();
        Assert.AreEqual(Bug.State.Open, bug.CurrentState);
    }

    [TestMethod]
    public void Assign_Reassign_KeepsStateAndUpdatesAssignee()
    {
        var bug = new Bug();
        bug.Assign("Alice");
        bug.Assign("Bob");
        Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
        Assert.AreEqual("Bob", bug.Assignee);
    }

    [TestMethod]
    public void FullLifecycle_OpenAssignResolveClose()
    {
        var bug = new Bug();
        bug.Assign("Dev");
        bug.Resolve();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void Open_Resolve_ThrowsInvalidOperation()
    {
        var bug = new Bug();
        Assert.Throws<InvalidOperationException>(() => bug.Resolve());
    }

    [TestMethod]
    public void Open_Close_ThrowsInvalidOperation()
    {
        var bug = new Bug();
        Assert.Throws<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void Open_Reopen_ThrowsInvalidOperation()
    {
        var bug = new Bug();
        Assert.Throws<InvalidOperationException>(() => bug.Reopen());
    }

    [TestMethod]
    public void Open_Reactivate_ThrowsInvalidOperation()
    {
        var bug = new Bug();
        Assert.Throws<InvalidOperationException>(() => bug.Reactivate());
    }

    [TestMethod]
    public void Deferred_Assign_ThrowsInvalidOperation()
    {
        var bug = new Bug();
        bug.Defer();
        Assert.Throws<InvalidOperationException>(() => bug.Assign("X"));
    }

    [TestMethod]
    public void Deferred_Resolve_ThrowsInvalidOperation()
    {
        var bug = new Bug();
        bug.Defer();
        Assert.Throws<InvalidOperationException>(() => bug.Resolve());
    }

    [TestMethod]
    public void Resolved_Assign_ThrowsInvalidOperation()
    {
        var bug = new Bug();
        bug.Assign("Alice");
        bug.Resolve();
        Assert.Throws<InvalidOperationException>(() => bug.Assign("Bob"));
    }

    [TestMethod]
    public void Closed_Close_ThrowsInvalidOperation()
    {
        var bug = new Bug();
        bug.Assign("A");
        bug.Resolve();
        bug.Close();
        Assert.Throws<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void Closed_Defer_ThrowsInvalidOperation()
    {
        var bug = new Bug();
        bug.Assign("A");
        bug.Resolve();
        bug.Close();
        Assert.Throws<InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void Assign_NullName_ThrowsArgumentException()
    {
        var bug = new Bug();
        Assert.Throws<ArgumentException>(() => bug.Assign(null!));
    }

    [TestMethod]
    public void Assign_EmptyName_ThrowsArgumentException()
    {
        var bug = new Bug();
        Assert.Throws<ArgumentException>(() => bug.Assign(""));
    }

    [TestMethod]
    public void CanFire_OpenState_AllowsAssignAndDefer()
    {
        var bug = new Bug();
        Assert.IsTrue(bug.CanFire(Bug.Trigger.Assign));
        Assert.IsTrue(bug.CanFire(Bug.Trigger.Defer));
        Assert.IsFalse(bug.CanFire(Bug.Trigger.Resolve));
    }

    [TestMethod]
    public void ReopenedBug_CanBeAssignedAgain()
    {
        var bug = new Bug();
        bug.Assign("Alice");
        bug.Resolve();
        bug.Reopen();
        bug.Assign("Bob");
        Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
        Assert.AreEqual("Bob", bug.Assignee);
    }

    [TestMethod]
    public void DeferredThenReactivated_CanBeAssigned()
    {
        var bug = new Bug();
        bug.Assign("Alice");
        bug.Defer();
        bug.Reactivate();
        bug.Assign("Bob");
        Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
        Assert.AreEqual("Bob", bug.Assignee);
    }
}
