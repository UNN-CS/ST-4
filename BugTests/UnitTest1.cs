namespace BugTests;

using static Bug;

[TestClass]
public class UnitTest1
{
    [TestMethod]
	[ExpectedException(typeof(InvalidOperationException))]
    public void TestDeferClosed()
    {
        var bug = new Bug(Bug.Condition.Def);
        bug.Close();
        Bug.Condition state = bug.getCondition();
        Assert.AreEqual(Bug.Condition.Closed, state);
    }

    public void TestDeferDefered()
    {
        var bug = new Bug(Bug.Condition.Def);
        bug.Defer();
        Bug.Condition state = bug.getCondition();
        Assert.AreEqual(Bug.Condition.Def, state);
    }

    [TestMethod]
    public void TestDeferAssigned()
    {
        var bug = new Bug(Bug.Condition.Def);
        bug.Assign();
        Bug.Condition state = bug.getCondition();
        Assert.AreEqual(Bug.Condition.Assigned, state);
    }
    [TestMethod]
	[ExpectedException(typeof(InvalidOperationException))]
    public void TestOpenClosed()
    {
        var bug = new Bug(Bug.Condition.Open);
        bug.Close();
        Bug.Condition state = bug.getCondition();
        Assert.AreEqual(Bug.Condition.Closed, state);
    }

    [TestMethod]
    public void TestOpenAssigned()
    {
        var bug = new Bug(Bug.Condition.Open);
        bug.Assign();
        Bug.Condition state = bug.getCondition();
        Assert.AreEqual(Bug.Condition.Assigned, state);
    }

    [TestMethod]
	[ExpectedException(typeof(InvalidOperationException))]
    public void TestOpenDefered()
    {
        var bug = new Bug(Bug.Condition.Open);
        bug.Defer();
        Bug.Condition state = bug.getCondition();
        Assert.AreEqual(Bug.Condition.Def, state);
    }
    [TestMethod]
    public void TestAssignedDefered()
    {
        var bug = new Bug(Bug.Condition.Assigned);
        bug.Defer();
        Bug.Condition state = bug.getCondition();
        Assert.AreEqual(Bug.Condition.Def, state);
    }
    [TestMethod]
    public void TestAssignedAssign()
    {
        var bug = new Bug(Bug.Condition.Assigned);
        bug.Assign();
        Bug.Condition state = bug.getCondition();
        Assert.AreEqual(Bug.Condition.Assigned, state);
    }

    [TestMethod]
    public void TestAssignedClosed()
    {
        var bug = new Bug(Bug.Condition.Assigned);
        bug.Close();
        Bug.Condition state = bug.getCondition();
        Assert.AreEqual(Bug.Condition.Closed, state);
    }

    [TestMethod]
	[ExpectedException(typeof(InvalidOperationException))]
    public void TestClosedDefered()
    {
        var bug = new Bug(Bug.Condition.Closed);
        bug.Defer();
        Bug.Condition state = bug.getCondition();
        Assert.AreEqual(Bug.Condition.Def, state);
    }

    [TestMethod]
    public void TestClosedAssigned()
    {
        var bug = new Bug(Bug.Condition.Closed);
        bug.Assign();
        Bug.Condition state = bug.getCondition();
        Assert.AreEqual(Bug.Condition.Assigned, state);
    }

    [TestMethod]
	[ExpectedException(typeof(InvalidOperationException))]
    public void TestClosedClosed()
    {
        var bug = new Bug(Bug.Condition.Closed);
        bug.Close();
        Bug.Condition state = bug.getCondition();
        Assert.AreEqual(Bug.Condition.Closed, state);
    }

}