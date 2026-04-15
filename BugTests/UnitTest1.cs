using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public class BugWorkflowTests
{
    [TestMethod]
    public void Constructor_ShouldStartInNewState()
    {
        var bug = new Bug("Test bug");
        Assert.AreEqual(BugState.New, bug.State);
    }

    [TestMethod]
    public void Constructor_ShouldStoreTitle()
    {
        const string title = "UI glitch";
        var bug = new Bug(title);
        Assert.AreEqual(title, bug.Title);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Constructor_WithEmptyTitle_ThrowsException()
    {
        _ = new Bug("");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Constructor_WithWhitespaceTitle_ThrowsException()
    {
        _ = new Bug("   ");
    }

    [TestMethod]
    public void New_CanAssign()
    {
        var bug = new Bug("B1");
        Assert.IsTrue(bug.CanFire(BugTrigger.Assign));
    }

    [TestMethod]
    public void New_CannotClose()
    {
        var bug = new Bug("B2");
        Assert.IsFalse(bug.CanFire(BugTrigger.Close));
    }

    [TestMethod]
    public void Assign_FromNew_MovesToAssigned()
    {
        var bug = new Bug("B3");
        bug.Fire(BugTrigger.Assign);
        Assert.AreEqual(BugState.Assigned, bug.State);
    }

    [TestMethod]
    public void Reject_FromNew_MovesToRejected()
    {
        var bug = new Bug("B4");
        bug.Fire(BugTrigger.Reject);
        Assert.AreEqual(BugState.Rejected, bug.State);
    }

    [TestMethod]
    public void MarkDuplicate_FromNew_MovesToDuplicate()
    {
        var bug = new Bug("B5");
        bug.Fire(BugTrigger.MarkDuplicate);
        Assert.AreEqual(BugState.Duplicate, bug.State);
    }

    [TestMethod]
    public void MarkCannotReproduce_FromNew_MovesToCannotReproduce()
    {
        var bug = new Bug("B6");
        bug.Fire(BugTrigger.MarkCannotReproduce);
        Assert.AreEqual(BugState.CannotReproduce, bug.State);
    }

    [TestMethod]
    public void StartProgress_FromAssigned_MovesToInProgress()
    {
        var bug = new Bug("B7");
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void Reject_FromAssigned_MovesToRejected()
    {
        var bug = new Bug("B8");
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.Reject);
        Assert.AreEqual(BugState.Rejected, bug.State);
    }

    [TestMethod]
    public void Resolve_FromInProgress_MovesToFixed()
    {
        var bug = new Bug("B9");
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Resolve);
        Assert.AreEqual(BugState.Fixed, bug.State);
    }

    [TestMethod]
    public void Close_FromFixed_MovesToClosed()
    {
        var bug = new Bug("B10");
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Resolve);
        bug.Fire(BugTrigger.Close);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void ReturnToWork_FromFixed_MovesToReopened()
    {
        var bug = new Bug("B11");
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Resolve);
        bug.Fire(BugTrigger.ReturnToWork);
        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void Reopen_FromClosed_MovesToReopened()
    {
        var bug = new Bug("B12");
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Resolve);
        bug.Fire(BugTrigger.Close);
        bug.Fire(BugTrigger.Reopen);
        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void StartProgress_FromReopened_MovesToInProgress()
    {
        var bug = new Bug("B13");
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Resolve);
        bug.Fire(BugTrigger.Close);
        bug.Fire(BugTrigger.Reopen);
        bug.Fire(BugTrigger.StartProgress);
        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void FullCycle_NewToClosed()
    {
        var bug = new Bug("B14");
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Resolve);
        bug.Fire(BugTrigger.Close);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Close_FromNew_ThrowsException()
    {
        var bug = new Bug("B15");
        bug.Fire(BugTrigger.Close);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Resolve_FromAssigned_ThrowsException()
    {
        var bug = new Bug("B16");
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.Resolve);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Assign_FromClosed_ThrowsException()
    {
        var bug = new Bug("B17");
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Resolve);
        bug.Fire(BugTrigger.Close);
        bug.Fire(BugTrigger.Assign);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void StartProgress_FromRejected_ThrowsException()
    {
        var bug = new Bug("B18");
        bug.Fire(BugTrigger.Reject);
        bug.Fire(BugTrigger.StartProgress);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Close_FromDuplicate_ThrowsException()
    {
        var bug = new Bug("B19");
        bug.Fire(BugTrigger.MarkDuplicate);
        bug.Fire(BugTrigger.Close);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Reopen_FromCannotReproduce_ThrowsException()
    {
        var bug = new Bug("B20");
        bug.Fire(BugTrigger.MarkCannotReproduce);
        bug.Fire(BugTrigger.Reopen);
    }
}