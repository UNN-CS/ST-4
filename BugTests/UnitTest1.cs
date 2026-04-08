using BugPro;
using Stateless;

namespace BugTests;

[TestClass]
public sealed class UnitTest1
{
    private Bug _bug = null!;

    [TestInitialize]
    public void Setup()
    {
        _bug = new Bug("Test Bug");
    }

    [TestMethod]
    public void InitialState_IsOpen()
    {
        Assert.AreEqual(Bug.State.Open, _bug.CurrentState);
    }

    [TestMethod]
    public void Assign_MovesToTriage()
    {
        _bug.Assign();
        Assert.AreEqual(Bug.State.Triage, _bug.CurrentState);
    }

    [TestMethod]
    public void Fix_MovesToInProgress()
    {
        _bug.Assign();
        _bug.Fix();
        Assert.AreEqual(Bug.State.InProgress, _bug.CurrentState);
    }

    [TestMethod]
    public void Resolve_MovesToResolved()
    {
        _bug.Assign();
        _bug.Fix();
        _bug.Resolve();
        Assert.AreEqual(Bug.State.Resolved, _bug.CurrentState);
    }

    [TestMethod]
    public void Verify_MovesToClosed()
    {
        _bug.Assign();
        _bug.Fix();
        _bug.Resolve();
        _bug.Verify();
        Assert.AreEqual(Bug.State.Closed, _bug.CurrentState);
    }

    [TestMethod]
    public void Reopen_MovesToReopened()
    {
        _bug.Assign();
        _bug.Fix();
        _bug.Resolve();
        _bug.Verify();
        _bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, _bug.CurrentState);
    }

    [TestMethod]
    public void BackToTriage_FromReopened()
    {
        _bug.Assign();
        _bug.Fix();
        _bug.Resolve();
        _bug.Verify();
        _bug.Reopen();
        _bug.BackToTriage();
        Assert.AreEqual(Bug.State.Triage, _bug.CurrentState);
    }

    [TestMethod]
    public void Defer_MovesToDeferred()
    {
        _bug.Assign();
        _bug.Defer();
        Assert.AreEqual(Bug.State.Deferred, _bug.CurrentState);
    }

    [TestMethod]
    public void ReturnFromDefer_BackToTriage()
    {
        _bug.Assign();
        _bug.Defer();
        _bug.ReturnFromDefer();
        Assert.AreEqual(Bug.State.Triage, _bug.CurrentState);
    }

    [TestMethod]
    public void Reject_MovesToRejected()
    {
        _bug.Assign();
        _bug.Reject();
        Assert.AreEqual(Bug.State.Rejected, _bug.CurrentState);
    }

    [TestMethod]
    public void AcceptReject_MovesToClosed()
    {
        _bug.Assign();
        _bug.Reject();
        _bug.AcceptReject();
        Assert.AreEqual(Bug.State.Closed, _bug.CurrentState);
    }

    [TestMethod]
    public void DenyReject_BackToTriage()
    {
        _bug.Assign();
        _bug.Reject();
        _bug.DenyReject();
        Assert.AreEqual(Bug.State.Triage, _bug.CurrentState);
    }

    [TestMethod]
    public void ReturnFromResolve_BackToTriage()
    {
        _bug.Assign();
        _bug.Fix();
        _bug.Resolve();
        _bug.ReturnFromResolve();
        Assert.AreEqual(Bug.State.Triage, _bug.CurrentState);
    }

    [TestMethod]
    public void FullCycle_RejectDenyAndFix()
    {
        _bug.Assign();
        _bug.Reject();
        _bug.DenyReject();
        _bug.Fix();
        _bug.Resolve();
        _bug.Verify();
        Assert.AreEqual(Bug.State.Closed, _bug.CurrentState);
    }

    [TestMethod]
    public void FullCycle_DeferReturnAndFix()
    {
        _bug.Assign();
        _bug.Defer();
        _bug.ReturnFromDefer();
        _bug.Fix();
        _bug.Resolve();
        _bug.Verify();
        Assert.AreEqual(Bug.State.Closed, _bug.CurrentState);
    }

    [TestMethod]
    public void FullCycle_ReopenAndFixAgain()
    {
        _bug.Assign();
        _bug.Fix();
        _bug.Resolve();
        _bug.Verify();
        _bug.Reopen();
        _bug.BackToTriage();
        _bug.Fix();
        _bug.Resolve();
        _bug.Verify();
        Assert.AreEqual(Bug.State.Closed, _bug.CurrentState);
    }

    [TestMethod]
    public void CanFire_AssignFromOpen()
    {
        Assert.IsTrue(_bug.CanFire(Bug.Trigger.Assign));
    }

    [TestMethod]
    public void CannotFire_FixFromOpen()
    {
        Assert.IsFalse(_bug.CanFire(Bug.Trigger.Fix));
    }

    [TestMethod]
    public void CannotFire_ResolveFromTriage()
    {
        _bug.Assign();
        Assert.IsFalse(_bug.CanFire(Bug.Trigger.Resolve));
    }

    [TestMethod]
    public void Throws_FixFromOpen()
    {
        Assert.ThrowsExactly<InvalidOperationException>(() => _bug.Fix());
    }

    [TestMethod]
    public void Throws_ResolveFromOpen()
    {
        Assert.ThrowsExactly<InvalidOperationException>(() => _bug.Resolve());
    }

    [TestMethod]
    public void Throws_VerifyFromOpen()
    {
        Assert.ThrowsExactly<InvalidOperationException>(() => _bug.Verify());
    }

    [TestMethod]
    public void Throws_ReopenFromOpen()
    {
        Assert.ThrowsExactly<InvalidOperationException>(() => _bug.Reopen());
    }

    [TestMethod]
    public void Throws_AssignFromTriage()
    {
        _bug.Assign();
        Assert.ThrowsExactly<InvalidOperationException>(() => _bug.Assign());
    }

    [TestMethod]
    public void Throws_ResolveFromTriage()
    {
        _bug.Assign();
        Assert.ThrowsExactly<InvalidOperationException>(() => _bug.Resolve());
    }

    [TestMethod]
    public void ToString_ContainsState()
    {
        StringAssert.Contains(_bug.ToString(), "Open");
    }
}
