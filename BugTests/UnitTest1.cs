namespace BugTests;

using static Bug;

[TestClass]
public class UnitTest1
{
    [TestMethod]
	[ExpectedException(typeof(InvalidOperationException))]
    public void TestDeferClosed()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Close();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Closed, state);
    }

    public void TestDeferDefered()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Defer();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Defered, state);
    }

    [TestMethod]
    public void TestDeferAssigned()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }
    [TestMethod]
	[ExpectedException(typeof(InvalidOperationException))]
    public void TestOpenClosed()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Close();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Closed, state);
    }

    [TestMethod]
    public void TestOpenAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }

    [TestMethod]
	[ExpectedException(typeof(InvalidOperationException))]
    public void TestOpenDefered()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Defered, state);
    }
    [TestMethod]
    public void TestAssignedDefered()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Defered, state);
    }
    [TestMethod]
    public void TestAssignedAssign()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }

    [TestMethod]
    public void TestAssignedClosed()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Closed, state);
    }

    [TestMethod]
	[ExpectedException(typeof(InvalidOperationException))]
    public void TestClosedDefered()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Defer();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Defered, state);
    }

    [TestMethod]
    public void TestClosedAssigned()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }

    [TestMethod]
	[ExpectedException(typeof(InvalidOperationException))]
    public void TestClosedClosed()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Close();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Closed, state);
    }

}