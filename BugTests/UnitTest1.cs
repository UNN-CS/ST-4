using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stateless;
using System;


[TestClass]
public class BugTest
{
    [TestMethod]
    public void NewBug_ShouldBeInOpenState()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.AreEqual(Bug.State.Open, bug.GetState());
    }

    [TestMethod]
    public void Assign_FromOpen_ShouldTransitionToAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void Defer_FromAssigned_ShouldTransitionToDefered()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.GetState());
    }

    [TestMethod]
    public void Reopen_FromClosed_ShouldTransitionToReopened()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.GetState());
    }

    [TestMethod]
    public void Verify_FromClosed_ShouldTransitionToVerified()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Verify();
        Assert.AreEqual(Bug.State.Verified, bug.GetState());
    }

    [TestMethod]
    public void Reject_FromAssigned_ShouldTransitionToRejected()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Reject();
        Assert.AreEqual(Bug.State.Rejected, bug.GetState());
    }

    [TestMethod]
    public void Assign_FromRejected_ShouldTransitionToAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Reject();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void Assign_FromDefered_ShouldTransitionToAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void Close_FromDefered_ShouldTransitionToClosed()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Close_FromOpen_ShouldThrowException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Close();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Verify_FromOpen_ShouldThrowException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Verify();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Reopen_FromOpen_ShouldThrowException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Reopen();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Reject_FromOpen_ShouldThrowException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Reject();
    }

    [TestMethod]
    public void Assign_FromAssigned_ShouldBeIgnored()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        var initialState = bug.GetState();
        bug.Assign();
        Assert.AreEqual(initialState, bug.GetState());
    }

    [TestMethod]
    public void MultipleTransitions_ShouldEndInCorrectState()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Assign();
        bug.Reject();
        bug.Assign();
        bug.Close();
        bug.Reopen();
        bug.Assign();
        bug.Close();
        bug.Verify();
        Assert.AreEqual(Bug.State.Verified, bug.GetState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Verify_FromReopened_ShouldThrowException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        bug.Verify();
    }

    [TestMethod]
    public void Close_FromRejected_ShouldTransitionToClosed()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Reject();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void Reopen_FromVerified_ShouldTransitionToReopened()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Verify();
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.GetState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Defer_FromClosed_ShouldThrowException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Defer();
    }

    [TestMethod]
    public void Close_FromAssigned_ShouldTransitionToClosed()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void Close_FromReopened_ShouldTransitionToClosed()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void Assign_FromReopened_ShouldTransitionToAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Assign_FromVerified_ShouldThrowException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Verify();
        bug.Assign();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Reject_FromDefered_ShouldThrowException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Reject();
    }
}
