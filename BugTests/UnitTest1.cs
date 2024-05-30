namespace BugTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void InitialStateIsOpen()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.AreEqual(Bug.State.Open, bug.getState());
    }

    [TestMethod]
    public void AssignFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void CloseFromAssigned()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void DeferFromAssigned()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }

    [TestMethod]
    public void AssignFromDefered()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void CreateFixFromClosed()
    {
        var bug = new Bug(Bug.State.CreatedFixes);
        bug.CreateFix();
        Assert.AreEqual(Bug.State.CreatedFixes, bug.getState());
    }

    [TestMethod]
    public void AcceptFixFromCreatedFixes()
    {
        var bug = new Bug(Bug.State.CreatedFixes);
        bug.AcceptFix();
        Assert.AreEqual(Bug.State.AcceptedFixes, bug.getState());
    }

    [TestMethod]
    public void DeclineFixFromCreatedFixes()
    {
        var bug = new Bug(Bug.State.CreatedFixes);
        bug.DeclineFix();
        Assert.AreEqual(Bug.State.DeclinedFixes, bug.getState());
    }

    [TestMethod]
    public void CreateFixFromDeclinedFixes()
    {
        var bug = new Bug(Bug.State.DeclinedFixes);
        bug.CreateFix();
        Assert.AreEqual(Bug.State.CreatedFixes, bug.getState());
    }

    [TestMethod]
    public void CloseFromAcceptedFixes()
    {
        var bug = new Bug(Bug.State.AcceptedFixes);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void IgnoreAssignInAssigned()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void AssignFromClosed()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void ComplexStateTransitions()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Assign();
        bug.Defer();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }
    
    [TestMethod]
    public void AcceptFixAndClose()
    {
        var bug = new Bug(Bug.State.CreatedFixes);
        bug.AcceptFix();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void AssignFromDeferedToAssigned()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

}