using BugPro;
using System.Collections.Concurrent;

namespace BugTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestBugIsOpened()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.AreEqual(Bug.State.Open, bug.getState());
    }
    [TestMethod]
    public void TestBugIsAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }
    [TestMethod]
    public void TestBugIsDefered()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual (Bug.State.Defered, bug.getState());
    }
    [TestMethod]
    public void TestBugDeferedException()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }
    [TestMethod]
    public void TestBugIsClosed()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }
    [TestMethod]
    public void TestBugClosedException()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }
    [TestMethod]
    public void TestBugIsAssignedAndClosed()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }
    [TestMethod]
    public void TestBugInitialAssigned()
    {
        var bug = new Bug(Bug.State.Assigned);
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }
    [TestMethod]
    public void TestBugInitialAssignedAndClosed()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }
    [TestMethod]
    public void TestBugInitialDefered()
    {
        var bug = new Bug(Bug.State.Defered);
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }
    [TestMethod]
    public void TestBugInitialDeferedAndClosed()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }
    [TestMethod]
    public void TestBugInitialClosed()
    {
        var bug = new Bug(Bug.State.Closed);
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }
}
