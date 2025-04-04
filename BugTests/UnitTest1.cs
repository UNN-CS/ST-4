using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using Stateless;

[TestClass]
public class BugTests
{
    [TestMethod]
    public void OpenToAssigned_ValidTransition()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void AssignedToInProgress_ValidTransition()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Start();
        Assert.AreEqual(Bug.State.InProgress, bug.GetState());
    }

    [TestMethod]
    public void InProgressToResolved_ValidTransition()
    {
        var bug = new Bug(Bug.State.InProgress);
        bug.Resolve();
        Assert.AreEqual(Bug.State.Resolved, bug.GetState());
    }

    [TestMethod]
    public void ResolvedToClosed_ValidTransition()
    {
        var bug = new Bug(Bug.State.Resolved);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void ClosedToReopened_ValidTransition()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.GetState());
    }

    [TestMethod]
    public void ReopenedToAssigned_ValidTransition()
    {
        var bug = new Bug(Bug.State.Reopened);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void AssignedToDefered_ValidTransition()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.GetState());
    }

    [TestMethod]
    public void DeferedToAssigned_ValidTransition()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void OpenToClosed_ValidTransition()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void AssignedToClosed_ValidTransition()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void OpenToResolve_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Resolve();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClosedToResolve_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Resolve();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeferedToClose_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Close();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void InProgressToAssign_InvalidTransition()
    {
        var bug = new Bug(Bug.State.InProgress);
        bug.Assign();
    }

    [TestMethod]
    public void AssignedToAssign_IgnoredTransition()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void FullCycle_OpenToClosed()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Start();
        bug.Resolve();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void ReopenCycle_ClosedToAssigned()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Reopen();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ResolvedToDefer_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Resolved);
        bug.Defer();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void OpenToStart_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Start();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ReopenedToResolve_InvalidTransition()
    {
        var bug = new Bug(Bug.State.Reopened);
        bug.Resolve();
    }

    [TestMethod]
    public void InProgressToDefered_ValidTransition()
    {
        var bug = new Bug(Bug.State.InProgress);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.GetState());
    }
}