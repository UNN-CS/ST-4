using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stateless;

namespace BugTests;

[TestClass]
public class BugTests
{
    [TestMethod]
    public void TestInitialState()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.AreEqual(Bug.State.Open, bug.getState());
    }

    [TestMethod]
    public void TestOpenToAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void TestAssignedToClosed()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void TestAssignedToDefered()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }

    [TestMethod]
    public void TestClosedToAssigned()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void TestDeferedToAssigned()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestInvalidTransitionFromOpenToClosed()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Close();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestInvalidTransitionFromOpenToDefered()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestInvalidTransitionFromClosedToDefered()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Defer();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestInvalidTransitionFromDeferedToClosed()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Close();
    }

    [TestMethod]
    public void TestMultipleAssignsInAssignedState()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Assign(); // Should be ignored
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void TestFullWorkflow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void TestReopenWorkflow()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void TestDeferWorkflow()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestInvalidTransitionFromClosedToClosed()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Close();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestInvalidTransitionFromDeferedToDefered()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Defer();
    }

    [TestMethod]
    public void TestStateTransitionsAfterDefer()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void TestStateTransitionsAfterClose()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }

    [TestMethod]
    public void TestInvalidTransitionFromOpenToOpen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign(); // Первый assign переводит в Assigned
        bug.Assign(); // Второй assign игнорируется
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }
}