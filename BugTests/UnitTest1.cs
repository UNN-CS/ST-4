using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void StartsInOpenState()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.AreEqual(Bug.State.Open, bug.getState());
    }

    [TestMethod]
    public void Open_Assign_Assigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void Assigned_Close_Closed()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void Assigned_Defer_Defered()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }

    [TestMethod]
    public void Assigned_Assign_Ignored()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void Defered_Assign_Assigned()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void Closed_Assign_Assigned()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void FullCycle_Open_Assign_Close_Assign_Defer_Assign()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Assign();
        bug.Defer();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void Defer_Without_Assign_Throws()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void Close_Without_Assign_Throws()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void Close_From_Defered_Throws()
    {
        var bug = new Bug(Bug.State.Defered);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void Defer_From_Closed_Throws()
    {
        var bug = new Bug(Bug.State.Closed);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void Assign_From_Defered_Works()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void Assign_Close_Assign_Sequence()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void DoubleClose_Throws()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void Assign_From_Closed_Works()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void Assign_Defer_Assign_Close()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Assign();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void Close_From_Assigned_Twice_Throws()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void Defer_From_Assigned_Then_Valid_Assign()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void Stay_In_Assigned_On_Second_Assign()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }
}