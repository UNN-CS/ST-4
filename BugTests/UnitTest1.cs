using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stateless;

[TestClass]
public class BugTests
{
    [TestMethod]
    public void Assign_FromOpen_ChangesStateToAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Close_FromOpen_ThrowsException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Close();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Defer_FromOpen_ThrowsException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
    }

    [TestMethod]
    public void Assign_FromAssigned_RemainsAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void Close_FromAssigned_ChangesStateToClosed()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void Defer_FromAssigned_ChangesStateToDefered()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }

    [TestMethod]
    public void Assign_FromDefered_ChangesStateToAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Defer_FromClosed_ThrowsException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Defer();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Close_FromDefered_ThrowsException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Close();
    }

    [TestMethod]
    public void Assign_FromClosed_ChangesStateToAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void FullCycle_OpenAssignedDeferedAssignedClosed()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Assign();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void InvalidTransition_OpenToClosedDirectly_ThrowsException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Close();
    }

    [TestMethod]
    public void MultipleValidTransitions_EndsInCorrectState()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Assign();
        bug.Close();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void InvalidTransition_DeferedToClosed_ThrowsException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Close();
    }

    [TestMethod]
    public void IgnoredTransition_AssignInAssignedState()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void VerifyAllPossibleStates()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.AreEqual(Bug.State.Open, bug.getState());
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
    [ExpectedException(typeof(InvalidOperationException))]
    public void Close_FromDefered_WithNoPermit_ThrowsException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Close();
    }

    [TestMethod]
    public void NewBug_IsInCorrectInitialState()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.AreEqual(Bug.State.Open, bug.getState());
    }

    [TestMethod]
    public void FullCycle_OpenAssignedClosedAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void FullCycle_OpenAssignedDeferedAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }
}
