namespace BugTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void Test_AssignToAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }

    [TestMethod]
    public void Test_AssignToClosed()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Closed, state);
    }

    [TestMethod]
    public void Test_CloseWhileOpen()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void Test_DeferWhileAssigned()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Defered, state);
    }

    [TestMethod]
    public void Test_DeferWhileClosed()
    {
        var bug = new Bug(Bug.State.Closed);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void Test_CloseWhileAssigned()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Closed, state);
    }

    [TestMethod]
    public void Test_AssignWhileDeferred()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }

    [TestMethod]
    public void Test_AssignWhileAssigned2()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }

    [TestMethod]
    public void Test_AssignWhileOpen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }

    [TestMethod]
    public void Test_DeferWhileClosed2()
    {
        var bug = new Bug(Bug.State.Closed);
        Assert.ThrowsException<System.InvalidOperationException>(() => bug.Defer());
    }
}