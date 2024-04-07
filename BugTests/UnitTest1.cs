namespace BugTests;

using static Bug;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void Test_Closed_to_Assign()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }

    [TestMethod]
    public void Test_Open_to_Assign()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }

    [TestMethod]
    public void Test_Defer_to_Assigned()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }

        [TestMethod]
    public void Test_Assigned_to_Defer()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Defered, state);
    }

    [TestMethod]
    public void Test_Assigned_to_Closed()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Closed, state);
    }

    [TestMethod]
    public void Test_Open_to_Fetched()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Fetch();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Fetched, state);
    }

    [TestMethod]
    public void Test_Assigned_to_Fetched()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Fetch();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Fetched, state);
    }
    [TestMethod]
    public void Test_Deferred_to_Fetched()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Fetch();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Fetched, state);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Test_Closed_to_Fetched()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Fetch();
        Bug.State state = bug.getState();
        Assert.AreNotEqual(Bug.State.Fetched, state);
        Assert.AreEqual(Bug.State.Closed, state);
    }

        [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Test_Open_to_Defer()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
        Bug.State state = bug.getState();
        Assert.AreNotEqual(Bug.State.Defered, state);
        Assert.AreEqual(Bug.State.Open, state);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Test_Open_to_Close()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Close();
        Bug.State state = bug.getState();
        Assert.AreNotEqual(Bug.State.Closed, state);
        Assert.AreEqual(Bug.State.Open, state);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Test_Closed_to_Defer()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Defer();
        Bug.State state = bug.getState();
        Assert.AreNotEqual(Bug.State.Defered, state);
        Assert.AreEqual(Bug.State.Closed, state);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Test_Defer_to_Closed()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Close();
        Bug.State state = bug.getState();
        Assert.AreNotEqual(Bug.State.Closed, state);
        Assert.AreEqual(Bug.State.Defered, state);
    }
}