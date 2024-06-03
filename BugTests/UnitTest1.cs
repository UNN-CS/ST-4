namespace BugTests;

[TestClass]
public class BugTests
{
    [TestMethod]
    public void Bug_InitialState_Open()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.AreEqual(Bug.State.Open, bug.getState());
    }

    [TestMethod]
    public void Bug_Assign_From_Open_To_Assigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void Bug_Ignore_Assign_From_Assigned()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void Bug_Close_From_Assigned_To_Closed()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void Bug_Defer_From_Assigned_To_Defered()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }

    [TestMethod]
    public void Bug_Assign_From_Closed_To_Assigned()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void Bug_Assign_From_Defered_To_Assigned()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Bug_Close_From_Closed_State_Should_Throw_Exception()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Close();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Bug_Defer_From_Defered_State_Should_Throw_Exception()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Defer();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Bug_Cannot_Defer_From_Open_State()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
    }
}