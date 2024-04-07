namespace BugTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void CheckOpenToAssign()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }
    [TestMethod]
    public void CheckAssignToClose()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Closed, state);
    }
    [TestMethod]
    public void CheckAssignToDefer()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Defered, state);
    }
    [TestMethod]
    public void CheckCloseToAssign()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }
    [TestMethod]
    public void CheckDeferToAssign()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }
    [TestMethod]
    public void CheckAssignToAssign()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }
    [TestMethod]
    public void CheckOpenToCloseException()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }
    [TestMethod]
    public void CheckOpneToDeferException()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }
    [TestMethod]
    public void CheckDeferToCloseException()
    {
        var bug = new Bug(Bug.State.Defered);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }
    [TestMethod]
    public void CheckCloseToDeferException()
    {
        var bug = new Bug(Bug.State.Closed);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }
}
