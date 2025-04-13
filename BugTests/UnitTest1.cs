namespace BugTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void CheckAssignToDeferAndAssign()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }

    [TestMethod]
    public void CheckInitialState()
    {
        var bug = new Bug(Bug.State.Open);
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Open, state);
    }

    [TestMethod]
    public void CheckOpenToAssignAndClose()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Closed, state);
    }

    [TestMethod]
    public void CheckDeferToAssignAndClose()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        bug.Close();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Closed, state);
    }

    [TestMethod]
    public void CheckMultipleTransitions()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Assign();
        bug.Close();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Closed, state);
    }


    [TestMethod]
    public void CheckMultipleAssigns()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Assign();
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }

    [TestMethod]
    public void CheckClosedToDeferAndAssign()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }

}
