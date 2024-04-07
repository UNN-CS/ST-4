namespace BugTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestInitialStateIsOpen()
    {
	var bug = new Bug(Bug.State.Open);
	var state = bug.getState();
	Assert.AreEqual(Bug.State.Open, state);
    }

    [TestMethod]
    public void TestAssignBug()
    {
        var bug = new Bug(Bug.State.Open);
	bug.Assign();
	var state = bug.getState();
	Assert.AreEqual(Bug.State.Assigned, state);
    }

    [TestMethod]
    public void TestAssignAndCloseBug()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        var state = bug.getState();
        Assert.AreEqual(Bug.State.Closed, state);
    }

    [TestMethod]
    public void TestAssignedAndDeferBug()
    {
	var bug = new Bug(Bug.State.Assigned);
	bug.Defer();
	var state = bug.getState();
	Assert.AreEqual(Bug.State.Defered, state);
    }

    [TestMethod]
    public void TestClosedAndAssignBug()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        var state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }

    [TestMethod]
    public void TestAssignedAndAssignBug()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Assign();
        var state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }

    [TestMethod]
    public void TestAssignAndDeferBug()
    {
	var bug = new Bug(Bug.State.Open);
	bug.Assign();
	bug.Defer();
	var state = bug.getState();
	Assert.AreEqual(Bug.State.Defered, state);
    }
   
    [TestMethod]
    public void TestOpenAndDeferException()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());	
    }

    [TestMethod]
    public void TestOpenAndCloseException()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void TestDeferAndCloseException()
    {
        var bug = new Bug(Bug.State.Defered);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }

}
