namespace BugTests;

[TestClass]
public sealed class ValidTests
{
    [TestMethod]
    public void OpenToNotABugNoThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.MarkNotABug();
        Assert.AreEqual(Bug.State.NotABug, bug.getState());
    }
    [TestMethod]
    public void OpenToDuplicateNoThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.MarkDuplicate();
        Assert.AreEqual(Bug.State.Duplicate, bug.getState());
    }
    [TestMethod]
    public void OpenToAssignedNoThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }
    [TestMethod]
    public void OpenToDeferedNoThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }

    [TestMethod]
    public void NotABugToClosedNoThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.MarkNotABug();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void DuplicateToClosedNoThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.MarkDuplicate();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void AssignedToNotABugNoThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.MarkNotABug();
        Assert.AreEqual(Bug.State.NotABug, bug.getState());
    }
    [TestMethod]
    public void AssignedToDuplicateNoThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.MarkDuplicate();
        Assert.AreEqual(Bug.State.Duplicate, bug.getState());
    }
    [TestMethod]
    public void AssignedToDeferedNoThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }
    [TestMethod]
    public void AssignedToFixedNoThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Fix();
        Assert.AreEqual(Bug.State.Fixed, bug.getState());
    }

    [TestMethod]
    public void DeferedToAssignedNoThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void FixedToVerifiedNoThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Fix();
        bug.Verify();
        Assert.AreEqual(Bug.State.Verified, bug.getState());
    }

    [TestMethod]
    public void VerifiedToClosedNoThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Fix();
        bug.Verify();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void OpenToFixedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fix());
    }
    [TestMethod]
    public void OpenToVerifiedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Verify());
    }
    [TestMethod]
    public void OpenToClosedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void NotABugToDuplicateThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.MarkNotABug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.MarkDuplicate());
    }
    [TestMethod]
    public void NotABugToAssignedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.MarkNotABug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Assign());
    }
    [TestMethod]
    public void NotABugToDeferedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.MarkNotABug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }
    [TestMethod]
    public void NotABugToFixedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.MarkNotABug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fix());
    }
    [TestMethod]
    public void NotABugToVerifiedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.MarkNotABug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Verify());
    }

    [TestMethod]
    public void DuplicateToNotABugThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.MarkDuplicate();
        Assert.ThrowsException<InvalidOperationException>(() => bug.MarkNotABug());
    }
    [TestMethod]
    public void DuplicateToAssignedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.MarkDuplicate();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Assign());
    }
    [TestMethod]
    public void DuplicateToDeferedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.MarkDuplicate();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }
    [TestMethod]
    public void DuplicateToFixedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.MarkDuplicate();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fix());
    }
    [TestMethod]
    public void DuplicateToVerifiedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.MarkDuplicate();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Verify());
    }

    [TestMethod]
    public void AssignedToVerifiedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Verify());
    }
    [TestMethod]
    public void AssignedToClosedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void DeferedToNotABugThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
        Assert.ThrowsException<InvalidOperationException>(() => bug.MarkNotABug());
    }
    [TestMethod]
    public void DeferedToDuplicateThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
        Assert.ThrowsException<InvalidOperationException>(() => bug.MarkDuplicate());
    }
    [TestMethod]
    public void DeferedToFixedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fix());
    }
    [TestMethod]
    public void DeferedToVerifiedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Verify());
    }
    [TestMethod]
    public void DeferedToClosedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void FixedToNotABugThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Fix();
        Assert.ThrowsException<InvalidOperationException>(() => bug.MarkNotABug());
    }
    [TestMethod]
    public void FixedToDuplicateThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Fix();
        Assert.ThrowsException<InvalidOperationException>(() => bug.MarkDuplicate());
    }
    [TestMethod]
    public void FixedToAssignedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Fix();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Assign());
    }
    [TestMethod]
    public void FixedToDeferedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Fix();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }
    [TestMethod]
    public void FixedToClosedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Fix();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void VerifiedToNotABugThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Fix();
        bug.Verify();
        Assert.ThrowsException<InvalidOperationException>(() => bug.MarkNotABug());
    }
    [TestMethod]
    public void VerifiedToDuplicateThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Fix();
        bug.Verify();
        Assert.ThrowsException<InvalidOperationException>(() => bug.MarkDuplicate());
    }
    [TestMethod]
    public void VerifiedToAssignedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Fix();
        bug.Verify();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Assign());
    }
    [TestMethod]
    public void VerifiedToDeferedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Fix();
        bug.Verify();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }
    [TestMethod]
    public void VerifiedToFixedThrow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Fix();
        bug.Verify();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fix());
    }
}
