namespace Tests;
[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestInitialState()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.AreEqual(Bug.State.Open, bug.getState());
    }

    [TestMethod]
    public void TestAssignFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void TestCloseFromAssigned()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void TestDeferFromAssigned()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }

    [TestMethod]
    public void TestReassignFromDefered()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void TestReassignFromClosed()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void TestIgnoreAssignFromAssigned()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void TestInvalidCloseFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void TestInvalidDeferFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void TestStateAfterSequenceOfTransitions()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Assign();
        bug.Close();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }
}
