[TestClass]
public sealed class BugTests
{
    [TestMethod]
    public void InitialStateIsSet()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.AreEqual(Bug.State.Open, bug.getState());
    }

    [TestMethod]
    public void CanChangeFromOpenToAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void CanChangeFromAssignedToClosed()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void CanChangeFromClosedToAssigned()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void CanChangeFromAssignedToDefered()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }

    [TestMethod]
    public void CanChangeFromDeferedToAssigned()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void CanChangeFromAssignedToSelected()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Select();
        Assert.AreEqual(Bug.State.Selected, bug.getState());
    }

    [TestMethod]
    public void CanChangeFromSelectedToClosed()
    {
        var bug = new Bug(Bug.State.Selected);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void OpenToClosedThrowsException()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void ClosedToDeferThrowsException()
    {
        var bug = new Bug(Bug.State.Closed);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
    }

    [TestMethod]
    public void DeferedToCloseThrowsException()
    {
        var bug = new Bug(Bug.State.Defered);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void SelectedToAssignThrowsException()
    {
        var bug = new Bug(Bug.State.Selected);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Assign());
    }

    [TestMethod]
    public void AssignedToAssignedIsIdempotent()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void SelectedToSelectedIsIdempotent()
    {
        var bug = new Bug(Bug.State.Selected);
        bug.Select();
        Assert.AreEqual(Bug.State.Selected, bug.getState());
    }
}