namespace Tests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void Bug_StartsInOpenState()
    {
        var bug = new Bug(Bug.State.Open);
        var currentState = bug.getState();

        Assert.AreEqual(Bug.State.Open, currentState);
    }

    [TestMethod]
    public void Bug_Assign_FromOpenState()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();

        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void Bug_ChangeToOriginalState()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        bug.Assign();

        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void Bug_Close_FromAssignedState()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();

        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void Bug_Defer_FromAssignedState()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();

        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }

    [TestMethod]
    public void Bug_Assign_FromDeferedState()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Assign();

        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void Bug_Close_NotAllowedFromClosedState()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        
        Assert.ThrowsException<InvalidOperationException>(()=>bug.Close());
    }

    [TestMethod]
    public void Bug_Defer_NotAllowedFromDeferState()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();

        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void Bug_Defer_NotAllowedFromClosedState()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();

        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void Bug_IgnoreAssign_InAssignedState()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Assign();

        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }
}