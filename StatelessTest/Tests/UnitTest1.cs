namespace BugTests;

using BugPro;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestOpenBugAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }

    [TestMethod]
    public void TestAssignedBugClosed()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Closed, state);
    }
    
    [TestMethod]
    public void TestAssignedBugDefered()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Defered, state);
    }
    
    [TestMethod]
    public void TestAssignedBugAssigned()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }
    
    [TestMethod]
    public void TestClosedBugAssigned()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }
    
    [TestMethod]
    public void TestDeferedBugAssigned()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Bug.State state = bug.getState();
        Assert.AreEqual(Bug.State.Assigned, state);
    }

    [TestMethod] 
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestDeferedBugCannotClosed()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Close();
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestOpenBugCannotDefered()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestOpenBugCannotClosed()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Close();
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestClosedBugCannotDefered()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Defer();
    }
}