using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stateless;

namespace BugTests;

[TestClass]
public class BugStateTests
{
    private Bug _bug;

    [TestInitialize]
    public void Setup()
    {
        _bug = new Bug();
    }

    [TestMethod]
    public void InitialState_ShouldBeOpen()
    {
        Assert.AreEqual(BugState.Open, _bug.CurrentState);
    }

    [TestMethod]
    public void Open_Assign_TransitionsToAssigned()
    {
        _bug.Assign();
        Assert.AreEqual(BugState.Assigned, _bug.CurrentState);
    }

    [TestMethod]
    public void Open_Reject_TransitionsToRejected()
    {
        _bug.Reject();
        Assert.AreEqual(BugState.Rejected, _bug.CurrentState);
    }

    [TestMethod]
    public void Assigned_StartProgress_TransitionsToInProgress()
    {
        _bug.Assign();
        _bug.StartProgress();
        Assert.AreEqual(BugState.InProgress, _bug.CurrentState);
    }

    [TestMethod]
    public void Assigned_Reject_TransitionsToRejected()
    {
        _bug.Assign();
        _bug.Reject();
        Assert.AreEqual(BugState.Rejected, _bug.CurrentState);
    }

    [TestMethod]
    public void InProgress_Fix_TransitionsToFixed()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        Assert.AreEqual(BugState.Fixed, _bug.CurrentState);
    }

    [TestMethod]
    public void InProgress_Reject_TransitionsToRejected()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Reject();
        Assert.AreEqual(BugState.Rejected, _bug.CurrentState);
    }

    [TestMethod]
    public void Fixed_Verify_TransitionsToVerified()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        _bug.Verify();
        Assert.AreEqual(BugState.Verified, _bug.CurrentState);
    }

    [TestMethod]
    public void Fixed_Reopen_TransitionsToReopened()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        _bug.Reopen();
        Assert.AreEqual(BugState.Reopened, _bug.CurrentState);
    }

    [TestMethod]
    public void Verified_Close_TransitionsToClosed()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        _bug.Verify();
        _bug.Close();
        Assert.AreEqual(BugState.Closed, _bug.CurrentState);
    }

    [TestMethod]
    public void Verified_Reopen_TransitionsToReopened()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        _bug.Verify();
        _bug.Reopen();
        Assert.AreEqual(BugState.Reopened, _bug.CurrentState);
    }

    [TestMethod]
    public void Closed_Reopen_TransitionsToReopened()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        _bug.Verify();
        _bug.Close();
        _bug.Reopen();
        Assert.AreEqual(BugState.Reopened, _bug.CurrentState);
    }

    [TestMethod]
    public void Reopened_Assign_TransitionsToAssigned()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        _bug.Reopen();
        _bug.Assign();
        Assert.AreEqual(BugState.Assigned, _bug.CurrentState);
    }

    [TestMethod]
    public void Reopened_Reject_TransitionsToRejected()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        _bug.Reopen();
        _bug.Reject();
        Assert.AreEqual(BugState.Rejected, _bug.CurrentState);
    }

    [TestMethod]
    public void Rejected_Reopen_TransitionsToReopened()
    {
        _bug.Reject();
        _bug.Reopen();
        Assert.AreEqual(BugState.Reopened, _bug.CurrentState);
    }

    [TestMethod]
    public void CanAssign_WhenOpen_ReturnsTrue()
    {
        Assert.IsTrue(_bug.CanAssign());
    }

    [TestMethod]
    public void CanAssign_WhenNotOpen_ReturnsFalse()
    {
        _bug.Assign();
        Assert.IsFalse(_bug.CanAssign());
    }

    [TestMethod]
    public void CanStartProgress_WhenAssigned_ReturnsTrue()
    {
        _bug.Assign();
        Assert.IsTrue(_bug.CanStartProgress());
    }

    [TestMethod]
    public void CanFix_WhenInProgress_ReturnsTrue()
    {
        _bug.Assign();
        _bug.StartProgress();
        Assert.IsTrue(_bug.CanFix());
    }

    [TestMethod]
    public void CanVerify_WhenFixed_ReturnsTrue()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        Assert.IsTrue(_bug.CanVerify());
    }

    [TestMethod]
    public void CanClose_WhenVerified_ReturnsTrue()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        _bug.Verify();
        Assert.IsTrue(_bug.CanClose());
    }

    [TestMethod]
    public void CanReopen_WhenClosed_ReturnsTrue()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        _bug.Verify();
        _bug.Close();
        Assert.IsTrue(_bug.CanReopen());
    }

    [TestMethod]
    public void CanReopen_WhenRejected_ReturnsTrue()
    {
        _bug.Reject();
        Assert.IsTrue(_bug.CanReopen());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Assign_WhenNotOpen_ThrowsException()
    {
        _bug.Assign();
        _bug.Assign();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void StartProgress_WhenNotAssigned_ThrowsException()
    {
        _bug.StartProgress();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Fix_WhenNotInProgress_ThrowsException()
    {
        _bug.Fix();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Verify_WhenNotFixed_ThrowsException()
    {
        _bug.Verify();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Close_WhenNotVerified_ThrowsException()
    {
        _bug.Close();
    }

    [TestMethod]
    public void FullWorkflow_CompletesSuccessfully()
    {
        _bug.Assign();
        Assert.AreEqual(BugState.Assigned, _bug.CurrentState);

        _bug.StartProgress();
        Assert.AreEqual(BugState.InProgress, _bug.CurrentState);

        _bug.Fix();
        Assert.AreEqual(BugState.Fixed, _bug.CurrentState);

        _bug.Verify();
        Assert.AreEqual(BugState.Verified, _bug.CurrentState);

        _bug.Close();
        Assert.AreEqual(BugState.Closed, _bug.CurrentState);
    }

    [TestMethod]
    public void ReopenWorkflow_CompletesSuccessfully()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        _bug.Verify();
        _bug.Close();
        _bug.Reopen();
        Assert.AreEqual(BugState.Reopened, _bug.CurrentState);

        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        _bug.Verify();
        _bug.Close();
        Assert.AreEqual(BugState.Closed, _bug.CurrentState);
    }

    [TestMethod]
    public void RejectWorkflow_CompletesSuccessfully()
    {
        _bug.Reject();
        Assert.AreEqual(BugState.Rejected, _bug.CurrentState);

        _bug.Reopen();
        Assert.AreEqual(BugState.Reopened, _bug.CurrentState);
    }
}