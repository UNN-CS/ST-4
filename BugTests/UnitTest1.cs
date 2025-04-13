using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stateless;

[TestClass]
public class BugTests
{
    [TestMethod]
    public void OpenToAssigned_Transition_Success()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void AssignedToClosed_Transition_Success()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void AssignedToDefered_Transition_Success()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.GetState());
    }

    [TestMethod]
    public void DeferedToAssigned_Transition_Success()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void ClosedToReopened_Transition_Success()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.GetState());
    }

    [TestMethod]
    public void ReopenedToAssigned_Transition_Success()
    {
        var bug = new Bug(Bug.State.Reopened);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void ReopenedToClosed_Transition_Success()
    {
        var bug = new Bug(Bug.State.Reopened);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void ClosedToVerified_Transition_Success()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Verify();
        Assert.AreEqual(Bug.State.Verified, bug.GetState());
    }

    [TestMethod]
    public void VerifiedToReopened_Transition_Success()
    {
        var bug = new Bug(Bug.State.Verified);
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.GetState());
    }

    [TestMethod]
    public void VerifiedToClosed_Transition_Success()
    {
        var bug = new Bug(Bug.State.Verified);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void OpenToClose_InvalidTransition_ThrowsException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Close();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeferedToClosed_InvalidTransition_ThrowsException()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Close();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void OpenToVerify_InvalidTransition_ThrowsException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Verify();
    }

    [TestMethod]
    public void MultipleTransitions_EndsInCorrectState()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        bug.Assign();
        bug.Defer();
        bug.Assign();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void InitialState_Open_Correct()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.AreEqual(Bug.State.Open, bug.GetState());
    }

    [TestMethod]
    public void InitialState_Assigned_Correct()
    {
        var bug = new Bug(Bug.State.Assigned);
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void InitialState_Closed_Correct()
    {
        var bug = new Bug(Bug.State.Closed);
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void InitialState_Defered_Correct()
    {
        var bug = new Bug(Bug.State.Defered);
        Assert.AreEqual(Bug.State.Defered, bug.GetState());
    }

    [TestMethod]
    public void InitialState_Reopened_Correct()
    {
        var bug = new Bug(Bug.State.Reopened);
        Assert.AreEqual(Bug.State.Reopened, bug.GetState());
    }
} // end
