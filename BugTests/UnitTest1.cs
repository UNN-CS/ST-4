using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using Stateless;

namespace BugTests;

[TestClass]
public sealed class UnitTest1
{
    [TestMethod]
    public void OpenToAssigned_ValidTransition()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void AssignedToInProgress_ValidTransition()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Start();
        Assert.AreEqual(Bug.State.InProgress, bug.GetState());
    }

    [TestMethod]
    public void InProgressToResolved_ValidTransition()
    {
        var bug = new Bug(Bug.State.InProgress);
        bug.Resolve();
        Assert.AreEqual(Bug.State.Resolved, bug.GetState());
    }

    [TestMethod]
    public void ResolvedToClosed_ValidTransition()
    {
        var bug = new Bug(Bug.State.Resolved);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void ClosedToReopened_ValidTransition()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.GetState());
    }

    [TestMethod]
    public void ClosedToAssigned_ValidTransition()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void AssignedToDeferred_ValidTransition()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(Bug.State.Deferred, bug.GetState());
    }

    [TestMethod]
    public void InProgressToDeferred_ValidTransition()
    {
        var bug = new Bug(Bug.State.InProgress);
        bug.Defer();
        Assert.AreEqual(Bug.State.Deferred, bug.GetState());
    }

    [TestMethod]
    public void DeferredToAssigned_ValidTransition()
    {
        var bug = new Bug(Bug.State.Deferred);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void ReopenedToAssigned_ValidTransition()
    {
        var bug = new Bug(Bug.State.Reopened);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void OpenToClose_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void OpenToDefer_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void OpenToResolve_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Resolve());
    }

    [TestMethod]
    public void OpenToReopen_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Reopen());
    }

    [TestMethod]
    public void AssignedToResolve_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Assigned);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Resolve());
    }

    [TestMethod]
    public void DeferredToClose_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Deferred);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void DeferredToResolve_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Deferred);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Resolve());
    }

    [TestMethod]
    public void DeferredToStart_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Deferred);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Start());
    }

    [TestMethod]
    public void ResolvedToStart_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Resolved);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Start());
    }

    [TestMethod]
    public void ResolvedToDefer_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Resolved);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void ClosedToDefer_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Closed);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void ClosedToResolve_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Closed);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Resolve());
    }

    [TestMethod]
    public void ReopenedToClose_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Reopened);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void ReopenedToDefer_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Reopened);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void ReopenedToResolve_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Reopened);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Resolve());
    }

    [TestMethod]
    public void ReopenedToStart_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Reopened);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Start());
    }

    [TestMethod]
    public void InProgressToAssign_InvalidTransition()
    {
        var bug = new Bug(Bug.State.InProgress);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Assign());
    }

    [TestMethod]
    public void InProgressToReopen_InvalidTransition()
    {
        var bug = new Bug(Bug.State.InProgress);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Reopen());
    }

    [TestMethod]
    public void InProgressToClose_InvalidTransition()
    {
        var bug = new Bug(Bug.State.InProgress);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void AssignInAssigned_Ignored()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void MultipleTransitions_ValidSequence()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Start();
        bug.Resolve();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void DeferredAndBack_ValidSequence()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void ReopenedToInProgress_ValidSequence()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Reopen();
        bug.Assign();
        bug.Start();
        Assert.AreEqual(Bug.State.InProgress, bug.GetState());
    }
}