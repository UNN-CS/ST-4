namespace Tests;
[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        Assert.IsTrue(true);
    }
    [TestMethod]
    public void CheckOpenToAssign()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CheckOpenToClose()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Close();
        Bug.State state = bug.getState();
        Assert.AreNotEqual(Bug.State.Closed, state);
        Assert.AreEqual(Bug.State.Open, state);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CheckOpenToDefer()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
        Bug.State state = bug.getState();
        Assert.AreNotEqual(Bug.State.Defered, state);
        Assert.AreEqual(Bug.State.Open, state);
    }
    [TestMethod]
    public void CheckClosedToAssign()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CheckClosedToDefer()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Defer();
        Bug.State state = bug.getState();
        Assert.AreNotEqual(Bug.State.Defered, state);
        Assert.AreEqual(Bug.State.Closed, state);
    }
    [TestMethod]
    public void CheckDeferToAssigned()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CheckDeferToClosed()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Close();
        Bug.State state = bug.getState();
        Assert.AreNotEqual(Bug.State.Closed, state);
        Assert.AreEqual(Bug.State.Defered, state);
    }
    [TestMethod]
    public void CheckAssignedToDefer()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Defered, state);
    }
    [TestMethod]
    public void CheckAssignedToClosed()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Closed, state);
    }
    [TestMethod]
    public void CheckOpenToFeached()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Feach();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Feached, state);
    }
    [TestMethod]
    public void CheckAssignedToFeached()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Feach();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Feached, state);
    }
    [TestMethod]
    public void CheckDeferedToFeached()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Feach();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Feached, state);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CheckClosedToFeached()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Feach();
        Bug.State state = bug.getState();
        Assert.AreNotEqual(Bug.State.Feached, state);
        Assert.AreEqual(Bug.State.Closed, state);
    }
}