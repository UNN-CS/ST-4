using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public class Test1
{
    [TestMethod]
    public void Constructor_Should_CreateBugInNewState()
    {
        var bug = new Bug("Login error");

        Assert.AreEqual(BugState.New, bug.State);
    }

    [TestMethod]
    public void Constructor_Should_SaveTitle()
    {
        var bug = new Bug("Button does not work");

        Assert.AreEqual("Button does not work", bug.Title);
    }

    [TestMethod]
    public void Constructor_WithEmptyTitle_ShouldThrowArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => new Bug(""));
    }

    [TestMethod]
    public void Constructor_WithWhitespaceTitle_ShouldThrowArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => new Bug("   "));
    }

    [TestMethod]
    public void NewState_ShouldAllowAssign()
    {
        var bug = new Bug("Bug 1");

        Assert.IsTrue(bug.CanFire(BugTrigger.Assign));
    }

    [TestMethod]
    public void NewState_ShouldNotAllowClose()
    {
        var bug = new Bug("Bug 2");

        Assert.IsFalse(bug.CanFire(BugTrigger.Close));
    }

    [TestMethod]
    public void Assign_FromNew_ShouldMoveToAssigned()
    {
        var bug = new Bug("Bug 3");

        bug.Fire(BugTrigger.Assign);

        Assert.AreEqual(BugState.Assigned, bug.State);
    }

    [TestMethod]
    public void Reject_FromNew_ShouldMoveToRejected()
    {
        var bug = new Bug("Bug 4");

        bug.Fire(BugTrigger.Reject);

        Assert.AreEqual(BugState.Rejected, bug.State);
    }

    [TestMethod]
    public void MarkDuplicate_FromNew_ShouldMoveToDuplicate()
    {
        var bug = new Bug("Bug 5");

        bug.Fire(BugTrigger.MarkDuplicate);

        Assert.AreEqual(BugState.Duplicate, bug.State);
    }

    [TestMethod]
    public void MarkCannotReproduce_FromNew_ShouldMoveToCannotReproduce()
    {
        var bug = new Bug("Bug 6");

        bug.Fire(BugTrigger.MarkCannotReproduce);

        Assert.AreEqual(BugState.CannotReproduce, bug.State);
    }

    [TestMethod]
    public void StartProgress_FromAssigned_ShouldMoveToInProgress()
    {
        var bug = new Bug("Bug 7");
        bug.Fire(BugTrigger.Assign);

        bug.Fire(BugTrigger.StartProgress);

        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void Reject_FromAssigned_ShouldMoveToRejected()
    {
        var bug = new Bug("Bug 8");
        bug.Fire(BugTrigger.Assign);

        bug.Fire(BugTrigger.Reject);

        Assert.AreEqual(BugState.Rejected, bug.State);
    }

    [TestMethod]
    public void Resolve_FromInProgress_ShouldMoveToFixed()
    {
        var bug = new Bug("Bug 9");
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);

        bug.Fire(BugTrigger.Resolve);

        Assert.AreEqual(BugState.Fixed, bug.State);
    }

    [TestMethod]
    public void Close_FromFixed_ShouldMoveToClosed()
    {
        var bug = new Bug("Bug 10");
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Resolve);

        bug.Fire(BugTrigger.Close);

        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void ReturnToWork_FromFixed_ShouldMoveToReopened()
    {
        var bug = new Bug("Bug 11");
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Resolve);

        bug.Fire(BugTrigger.ReturnToWork);

        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void Reopen_FromClosed_ShouldMoveToReopened()
    {
        var bug = new Bug("Bug 12");
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Resolve);
        bug.Fire(BugTrigger.Close);

        bug.Fire(BugTrigger.Reopen);

        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void StartProgress_FromReopened_ShouldMoveToInProgress()
    {
        var bug = new Bug("Bug 13");
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Resolve);
        bug.Fire(BugTrigger.Close);
        bug.Fire(BugTrigger.Reopen);

        bug.Fire(BugTrigger.StartProgress);

        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void FullWorkflow_ShouldEndInClosedState()
    {
        var bug = new Bug("Bug 14");

        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Resolve);
        bug.Fire(BugTrigger.Close);

        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Close_FromNew_ShouldThrowInvalidOperationException()
    {
        var bug = new Bug("Bug 15");

        Assert.ThrowsException<InvalidOperationException>(() =>
            bug.Fire(BugTrigger.Close));
    }

    [TestMethod]
    public void Resolve_FromAssigned_ShouldThrowInvalidOperationException()
    {
        var bug = new Bug("Bug 16");
        bug.Fire(BugTrigger.Assign);

        Assert.ThrowsException<InvalidOperationException>(() =>
            bug.Fire(BugTrigger.Resolve));
    }

    [TestMethod]
    public void Assign_FromClosed_ShouldThrowInvalidOperationException()
    {
        var bug = new Bug("Bug 17");
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Resolve);
        bug.Fire(BugTrigger.Close);

        Assert.ThrowsException<InvalidOperationException>(() =>
            bug.Fire(BugTrigger.Assign));
    }

    [TestMethod]
    public void StartProgress_FromRejected_ShouldThrowInvalidOperationException()
    {
        var bug = new Bug("Bug 18");
        bug.Fire(BugTrigger.Reject);

        Assert.ThrowsException<InvalidOperationException>(() =>
            bug.Fire(BugTrigger.StartProgress));
    }

    [TestMethod]
    public void Close_FromDuplicate_ShouldThrowInvalidOperationException()
    {
        var bug = new Bug("Bug 19");
        bug.Fire(BugTrigger.MarkDuplicate);

        Assert.ThrowsException<InvalidOperationException>(() =>
            bug.Fire(BugTrigger.Close));
    }

    [TestMethod]
    public void Reopen_FromCannotReproduce_ShouldThrowInvalidOperationException()
    {
        var bug = new Bug("Bug 20");
        bug.Fire(BugTrigger.MarkCannotReproduce);

        Assert.ThrowsException<InvalidOperationException>(() =>
            bug.Fire(BugTrigger.Reopen));
    }
}