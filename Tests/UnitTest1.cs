namespace Tests;
[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void Bug_CanStartProgressFromAssigned()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.StartProgress();
        Assert.AreEqual(Bug.State.InProgress, bug.GetState());
    }

    [TestMethod]
    public void Bug_CanResolveFromInProgress()
    {
        var bug = new Bug(Bug.State.InProgress);
        bug.Resolve();
        Assert.AreEqual(Bug.State.Resolved, bug.GetState());
    }

    [TestMethod]
    public void Bug_CanReopenFromResolved()
    {
        var bug = new Bug(Bug.State.Resolved);
        bug.Reopen();
        Assert.AreEqual(Bug.State.Open, bug.GetState());
    }

    [TestMethod]
    public void Bug_CanDeferFromInProgress()
    {
        var bug = new Bug(Bug.State.InProgress);
        bug.Defer();
        Assert.AreEqual(Bug.State.Deferred, bug.GetState());
    }

    [TestMethod]
    public void Bug_CanCloseFromInProgress()
    {
        var bug = new Bug(Bug.State.InProgress);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void Bug_CanAssignFromInProgress()
    {
        var bug = new Bug(Bug.State.InProgress);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void Bug_CanStartProgressFromDeferred()
    {
        var bug = new Bug(Bug.State.Deferred);
        bug.StartProgress();
        Assert.AreEqual(Bug.State.InProgress, bug.GetState());
    }

    [TestMethod]
    public void Bug_CanResolveFromClosed()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Resolve();
        Assert.AreEqual(Bug.State.Resolved, bug.GetState());
    }

    [TestMethod]
    public void Bug_CanReopenFromClosed()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Reopen();
        Assert.AreEqual(Bug.State.Open, bug.GetState());
    }

    [TestMethod]
    public void Bug_CanDeferFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
        Assert.AreEqual(Bug.State.Deferred, bug.GetState());
    }

    [TestMethod]
    public void Bug_CanReopenFromAssigned()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Reopen();
        Assert.AreEqual(Bug.State.Open, bug.GetState());
    }

    [TestMethod]
    public void Bug_CanReopenFromInProgress()
    {
        var bug = new Bug(Bug.State.InProgress);
        bug.Reopen();
        Assert.AreEqual(Bug.State.Open, bug.GetState());
    }

    [TestMethod]
    public void Bug_CanDeferFromInProgressAndReopen()
    {
        var bug = new Bug(Bug.State.InProgress);
        bug.Defer();
        Assert.AreEqual(Bug.State.Deferred, bug.GetState());
        bug.Reopen();
        Assert.AreEqual(Bug.State.Open, bug.GetState());
    }
}
