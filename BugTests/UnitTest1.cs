using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestCreateOpen()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.AreEqual(bug.getState(), Bug.State.Open);
    }
    [TestMethod]
    public void TestAssignFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(bug.getState(), Bug.State.Assigned);
    }
    [TestMethod]
    public void TestDeferFromAssign()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        Assert.AreEqual(bug.getState(), Bug.State.Defered);
    }
    [TestMethod]
    public void TestCloseFromAssign()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        Assert.AreEqual(bug.getState(), Bug.State.Closed);
    }
    [TestMethod]
    public void TestAssignFromDefer()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Assign();
        Assert.AreEqual(bug.getState(), Bug.State.Assigned);
    }
    [TestMethod]
    public void TestReopenFromClose()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        Assert.AreEqual(bug.getState(), Bug.State.Reopened);
    }
    [TestMethod]
    public void TestAssignFromReopen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        bug.Assign();
        Assert.AreEqual(bug.getState(), Bug.State.Assigned);
    }
    [TestMethod]
    public void TestResolveFromReopen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        bug.Resolve();
        Assert.AreEqual(bug.getState(), Bug.State.Resolved);
    }
    [TestMethod]
    public void TestCloseFromResolve()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        bug.Resolve();
        bug.Close();
        Assert.AreEqual(bug.getState(), Bug.State.Closed);
    }
    [TestMethod]
    public void TestReopenFromResolve()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        bug.Resolve();
        bug.Reopen();
        Assert.AreEqual(bug.getState(), Bug.State.Reopened);
    }

    [TestMethod]
    public void TestIgnoreAssignFromAssign()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Assign();
        Assert.AreEqual(bug.getState(), Bug.State.Assigned);
    }
    [TestMethod]
    public void TestIgnoreReopenFromReopen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        bug.Reopen();
        Assert.AreEqual(bug.getState(), Bug.State.Reopened);
    }
    [TestMethod]
    public void TestIgnoreResolveFromResolve()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        bug.Resolve();
        Assert.AreEqual(bug.getState(), Bug.State.Resolved);
    }

    [TestMethod]
    public void TestThrowsCloseFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.Throws<Exception>(() => bug.Close());
    }
    [TestMethod]
    public void TestThrowsDeferFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.Throws<Exception>(() => bug.Defer());
    }
    [TestMethod]
    public void TestThrowsReopenFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.Throws<Exception>(() => bug.Reopen());
    }
    [TestMethod]
    public void TestThrowsResolveFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.Throws<Exception>(() => bug.Resolve());
    }
    [TestMethod]
    public void TestThrowsReopenFromAssign()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.Throws<Exception>(() => bug.Reopen());
    }
    [TestMethod]
    public void TestThrowsAssignFromClose()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        Assert.Throws<Exception>(() => bug.Assign());
    }
    [TestMethod]
    public void TestThrowsDeferFromClose()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        Assert.Throws<Exception>(() => bug.Defer());
    }
    [TestMethod]
    public void TestThrowsResolveFromClose()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        Assert.Throws<Exception>(() => bug.Resolve());
    }
    [TestMethod]
    public void TestThrowsCloseFromDefer()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        Assert.Throws<Exception>(() => bug.Close());
    }
    [TestMethod]
    public void TestThrowsReopenFromDefer()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        Assert.Throws<Exception>(() => bug.Reopen());
    }
    [TestMethod]
    public void TestThrowsResolveFromDefer()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        Assert.Throws<Exception>(() => bug.Resolve());
    }
    [TestMethod]
    public void TestThrowsCloseFromReopen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        Assert.Throws<Exception>(() => bug.Close());
    }
    [TestMethod]
    public void TestThrowsDeferFromReopen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        Assert.Throws<Exception>(() => bug.Defer());
    }
    [TestMethod]
    public void TestThrowsAssignFromResolve()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        bug.Resolve();
        Assert.Throws<Exception>(() => bug.Assign());
    }
    [TestMethod]
    public void TestThrowsDeferFromResolve()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        bug.Resolve();
        Assert.Throws<Exception>(() => bug.Defer());
    }
}