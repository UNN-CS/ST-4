// Copyright 2024 Nazyrov A.A.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests;

[TestClass]
public class BugWorkflowTests
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
    public void Assign_FromOpen_ShouldChangeToAssigned()
    {
        _bug.Assign();
        Assert.AreEqual(BugState.Assigned, _bug.CurrentState);
    }

    [TestMethod]
    public void StartProgress_FromAssigned_ShouldChangeToInProgress()
    {
        _bug.Assign();
        _bug.StartProgress();
        Assert.AreEqual(BugState.InProgress, _bug.CurrentState);
    }

    [TestMethod]
    public void Fix_FromInProgress_ShouldChangeToFixed()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        Assert.AreEqual(BugState.Fixed, _bug.CurrentState);
    }

    [TestMethod]
    public void Verify_FromFixed_ShouldChangeToVerified()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        _bug.Verify();
        Assert.AreEqual(BugState.Verified, _bug.CurrentState);
    }

    [TestMethod]
    public void Close_FromVerified_ShouldChangeToClosed()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        _bug.Verify();
        _bug.Close();
        Assert.AreEqual(BugState.Closed, _bug.CurrentState);
    }

    [TestMethod]
    public void Reject_FromOpen_ShouldChangeToRejected()
    {
        _bug.Reject();
        Assert.AreEqual(BugState.Rejected, _bug.CurrentState);
    }

    [TestMethod]
    public void Reject_FromAssigned_ShouldChangeToRejected()
    {
        _bug.Assign();
        _bug.Reject();
        Assert.AreEqual(BugState.Rejected, _bug.CurrentState);
    }

    [TestMethod]
    public void Reopen_FromFixed_ShouldChangeToReopened()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        _bug.Reopen();
        Assert.AreEqual(BugState.Reopened, _bug.CurrentState);
    }

    [TestMethod]
    public void Reopen_FromVerified_ShouldChangeToReopened()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        _bug.Verify();
        _bug.Reopen();
        Assert.AreEqual(BugState.Reopened, _bug.CurrentState);
    }

    [TestMethod]
    public void Reopen_FromRejected_ShouldChangeToReopened()
    {
        _bug.Reject();
        _bug.Reopen();
        Assert.AreEqual(BugState.Reopened, _bug.CurrentState);
    }

    [TestMethod]
    public void Assign_FromReopened_ShouldChangeToAssigned()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        _bug.Reopen();
        _bug.Assign();
        Assert.AreEqual(BugState.Assigned, _bug.CurrentState);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void StartProgress_FromOpen_ShouldThrowException()
    {
        _bug.StartProgress();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Fix_FromOpen_ShouldThrowException()
    {
        _bug.Fix();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Verify_FromAssigned_ShouldThrowException()
    {
        _bug.Assign();
        _bug.Verify();
    }

    [TestMethod]
    public void CanAssign_FromOpen_ShouldReturnTrue()
    {
        Assert.IsTrue(_bug.CanAssign());
    }

    [TestMethod]
    public void CanAssign_FromAssigned_ShouldReturnFalse()
    {
        _bug.Assign();
        Assert.IsFalse(_bug.CanAssign());
    }

    [TestMethod]
    public void CanReject_FromOpen_ShouldReturnTrue()
    {
        Assert.IsTrue(_bug.CanReject());
    }

    [TestMethod]
    public void CanReopen_FromFixed_ShouldReturnTrue()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        Assert.IsTrue(_bug.CanReopen());
    }

    [TestMethod]
    public void FullWorkflowToClosed_ShouldSucceed()
    {
        _bug.Assign();
        _bug.StartProgress();
        _bug.Fix();
        _bug.Verify();
        _bug.Close();
        Assert.AreEqual(BugState.Closed, _bug.CurrentState);
    }

    [TestMethod]
    public void RejectReopenAssignWorkflow_ShouldEndInAssigned()
    {
        _bug.Reject();
        _bug.Reopen();
        _bug.Assign();
        Assert.AreEqual(BugState.Assigned, _bug.CurrentState);
    }
}
