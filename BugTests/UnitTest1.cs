using BugPro;

namespace BugTests;

[TestClass]
public sealed class Test
{
    [TestMethod]
    public void OpenedToAssignedTransitionSuccessful()
    {
        var bug = new Bug(Bug.State.Opened);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void AssignedToCloseTransitionSuccessful()
    {
        var bug = new Bug(Bug.State.Opened);
        bug.Assign();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void AssignedToDeferTransitionSuccessful()
    {
        var bug = new Bug(Bug.State.Opened);
        bug.Assign();
        bug.Defer();
        Assert.AreEqual(Bug.State.Deferred, bug.GetState());
    }

    [TestMethod]
    public void AssignedToReviewTransitionSuccessful()
    {
        var bug = new Bug(Bug.State.Opened);
        bug.Assign();
        bug.Review();
        Assert.AreEqual(Bug.State.Reviewed, bug.GetState());
    }

    [TestMethod]
    public void DeferredToAssignedTransitionSuccessful()
    {
        var bug = new Bug(Bug.State.Opened);
        bug.Assign();
        bug.Defer();
        bug.Resume();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void ClosedToOpenedTransitionSuccessful()
    {
        var bug = new Bug(Bug.State.Opened);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        Assert.AreEqual(Bug.State.Opened, bug.GetState());
    }

    [TestMethod]
    public void ReviewedToClosedTransitionSuccessful()
    {
        var bug = new Bug(Bug.State.Opened);
        bug.Assign();
        bug.Review();
        bug.Approve();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void ReviewedToAssignedTransitionSuccessful()
    {
        var bug = new Bug(Bug.State.Opened);
        bug.Assign();
        bug.Review();
        bug.Return();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void OpenedToCloseTransitionFails()
    {
        var bug = new Bug(Bug.State.Opened);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void OpenedToDeferTransitionFails()
    {
        var bug = new Bug(Bug.State.Opened);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void OpenedToReviewTransitionFails()
    {
        var bug = new Bug(Bug.State.Opened);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Review());
    }

    [TestMethod]
    public void DeferredToReviewTransitionFails()
    {
        var bug = new Bug(Bug.State.Opened);
        bug.Assign();
        bug.Defer();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Review());
    }

    [TestMethod]
    public void ClosedToAssignedTransitionFails()
    {
        var bug = new Bug(Bug.State.Opened);
        bug.Assign();
        bug.Close();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Assign());
    }

    [TestMethod]
    public void DeferredToCloseTransitionFails()
    {
        var bug = new Bug(Bug.State.Opened);
        bug.Assign();
        bug.Defer();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void InitialStateIsAsExpected()
    {
        var bug = new Bug(Bug.State.Opened);
        Assert.AreEqual(Bug.State.Opened, bug.GetState());
    }

    [TestMethod]
    public void FullLifecycleReturnsCorrectFinalState()
    {
        var bug = new Bug(Bug.State.Opened);
        bug.Assign();
        bug.Review();
        bug.Return();
        bug.Review();
        bug.Approve();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void ReopenedBugCanBeAssigned()
    {
        var bug = new Bug(Bug.State.Opened);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void DeferredBugCanBeClosedAfterResumeAndClose()
    {
        var bug = new Bug(Bug.State.Opened);
        bug.Assign();
        bug.Defer();
        bug.Resume();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void CreateBugWithNonDefaultState()
    {
        var bug = new Bug(Bug.State.Assigned);
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void VerifyComplexTransitionChain()
    {
        var bug = new Bug(Bug.State.Opened);
        bug.Assign();
        bug.Defer();
        bug.Resume();
        bug.Review();
        bug.Return();
        bug.Review();
        bug.Approve();
        bug.Reopen();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }
}