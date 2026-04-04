using BugPro;

namespace BugTests;

[TestClass]
public sealed class UnitTest1
{
    [TestMethod]
    public void NewBug_InitialState_IsOpen()
    {
        var bug = new Bug("test");
        Assert.AreEqual(Bug.State.Open, bug.CurrentState);
    }

    [TestMethod]
    public void Assign_FromOpen_MovesToAssigned()
    {
        var bug = new Bug("test");
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
    }
    
    [TestMethod]
    public void Defer_FromAssigned_MovesToDeferred()
    {
        var bug = new Bug("test");
        bug.Assign();
        bug.Defer();
        Assert.AreEqual(Bug.State.Deferred, bug.CurrentState);
    }
    
    [TestMethod]
    public void Assign_FromDeferred_MovesToAssigned()
    {
        var bug = new Bug("test");
        bug.Assign();
        bug.Defer();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
    }
    
    [TestMethod]
    public void StartFix_FromAssigned_MovesToInProgress()
    {
        var bug = new Bug("test");
        bug.Assign();
        bug.StartFix();
        Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
    }

    [TestMethod]
    public void Resolve_FromInProgress_MovesToResolved()
    {
        var bug = new Bug("test");
        bug.Assign();
        bug.StartFix();
        bug.Resolve();
        Assert.AreEqual(Bug.State.Resolved, bug.CurrentState);
    }

    [TestMethod]
    public void Close_FromResolved_MovesToClosed()
    {
        var bug = new Bug("test");
        bug.Assign();
        bug.StartFix();
        bug.Resolve();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void StartFix_FromResolved_MovesBackToInProgress()
    {
        var bug = new Bug("test");
        bug.Assign();
        bug.StartFix();
        bug.Resolve();
        bug.StartFix();
        Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
    }

    [TestMethod]
    public void Reopen_FromClosed_MovesToReopened()
    {
        var bug = new Bug("test");
        bug.Assign();
        bug.StartFix();
        bug.Resolve();
        bug.Close();
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void ReturnToTriage_FromReopened_MovesToAssigned()
    {
        var bug = new Bug("test");
        bug.Assign();
        bug.StartFix();
        bug.Resolve();
        bug.Close();
        bug.Reopen();
        bug.ReturnToTriage();
        Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
    }

    [TestMethod]
    public void MarkNotABug_FromAssigned_MovesToClosed()
    {
        var bug = new Bug("test");
        bug.Assign();
        bug.MarkNotABug();
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void MarkDuplicate_FromAssigned_MovesToClosed()
    {
        var bug = new Bug("test");
        bug.Assign();
        bug.MarkDuplicate();
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void MarkCannotReproduce_FromAssigned_MovesToClosed()
    {
        var bug = new Bug("test");
        bug.Assign();
        bug.MarkCannotReproduce();
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void MarkDoNotFix_FromAssigned_MovesToClosed()
    {
        var bug = new Bug("test");
        bug.Assign();
        bug.MarkDoNotFix();
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void FullLifecycle_WorksCorrectly()
    {
        var bug = new Bug("test");
        bug.Assign();
        bug.StartFix();
        bug.Resolve();
        bug.Close();
        bug.Reopen();
        bug.ReturnToTriage();
        Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
    }

    [TestMethod]
    public void Constructor_WithInitialState_SetsCorrectState()
    {
        var bug = new Bug("test", Bug.State.Assigned);
        Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
    }

    [TestMethod]
    public void CanFire_ValidTrigger_ReturnsTrue()
    {
        var bug = new Bug("test");
        Assert.IsTrue(bug.CanFire(Bug.Trigger.Assign));
    }
    
    [TestMethod]
    public void CanFire_InvalidTrigger_ReturnsFalse()
    {
        var bug = new Bug("test");
        Assert.IsFalse(bug.CanFire(Bug.Trigger.Close));
    }
    
    [TestMethod]
    public void GetPermittedTriggers_OpenState_ReturnsOnlyAssign()
    {
        var bug = new Bug("test");
        var triggers = bug.GetPermittedTriggers().ToList();
        Assert.AreEqual(1, triggers.Count);
        Assert.IsTrue(triggers.Contains(Bug.Trigger.Assign));
    }
    
    [TestMethod]
    public void GetPermittedTriggers_AssignedState_ReturnsMultipleTriggers()
    {
        var bug = new Bug("test");
        bug.Assign();
        var triggers = bug.GetPermittedTriggers().ToList();
        Assert.IsTrue(triggers.Contains(Bug.Trigger.Defer));
        Assert.IsTrue(triggers.Contains(Bug.Trigger.StartFix));
        Assert.IsTrue(triggers.Contains(Bug.Trigger.NotABug));
        Assert.IsTrue(triggers.Contains(Bug.Trigger.Duplicate));
        Assert.IsTrue(triggers.Contains(Bug.Trigger.CannotReproduce));
        Assert.IsTrue(triggers.Contains(Bug.Trigger.DoNotFix));
    }
    
    [TestMethod]
    public void ToString_ReturnsCorrectFormat()
    {
        var bug = new Bug("My Bug");
        Assert.AreEqual("[My Bug] State: Open", bug.ToString());
    }
    
    [TestMethod]
    public void Close_FromOpen_ThrowsException()
    {
        var bug = new Bug("test");
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void Resolve_FromOpen_ThrowsException()
    {
        var bug = new Bug("test");
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.Resolve());
    }

    [TestMethod]
    public void Defer_FromOpen_ThrowsException()
    {
        var bug = new Bug("test");
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void StartFix_FromOpen_ThrowsException()
    {
        var bug = new Bug("test");
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.StartFix());
    }

    [TestMethod]
    public void Reopen_FromOpen_ThrowsException()
    {
        var bug = new Bug("test");
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.Reopen());
    }

    [TestMethod]
    public void Assign_FromInProgress_ThrowsException()
    {
        var bug = new Bug("test");
        bug.Assign();
        bug.StartFix();
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.Assign());
    }

    [TestMethod]
    public void Close_FromInProgress_ThrowsException()
    {
        var bug = new Bug("test");
        bug.Assign();
        bug.StartFix();
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void Reopen_FromAssigned_ThrowsException()
    {
        var bug = new Bug("test");
        bug.Assign();
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.Reopen());
    }

    [TestMethod]
    public void ReturnToTriage_FromClosed_ThrowsException()
    {
        var bug = new Bug("test");
        bug.Assign();
        bug.MarkNotABug();
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.ReturnToTriage());
    }

    [TestMethod]
    public void Assign_FromAssigned_ThrowsException()
    {
        var bug = new Bug("test");
        bug.Assign();
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.Assign());
    }

    [TestMethod]
    public void Defer_FromDeferred_ThrowsException()
    {
        var bug = new Bug("test");
        bug.Assign();
        bug.Defer();
        Assert.ThrowsExactly<InvalidOperationException>(() => bug.Defer());
    }
}
