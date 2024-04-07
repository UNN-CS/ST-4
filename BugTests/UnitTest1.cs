namespace BugTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void OpenAssign()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(bug.getState(), Bug.State.Assigned);
    }

    [TestMethod]
    public void AssignClose()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(bug.getState(), Bug.State.Closed);
    }

    [TestMethod]
    public void AssignDefer()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(bug.getState(), Bug.State.Defered);
    }

    [TestMethod]
    public void CloseAssign()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Assert.AreEqual(bug.getState(), Bug.State.Assigned);
    }

    [TestMethod]
    public void DeferAssign()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Assert.AreEqual(bug.getState(), Bug.State.Assigned);
    }

    [TestMethod]
    public void CreateFixAcceptFix()
    {
        var bug = new Bug(Bug.State.CreatedFixes);
        bug.AcceptFix();
        Assert.AreEqual(bug.getState(), Bug.State.AcceptedFixes);
    }

    [TestMethod]
    public void CreateFixDeclineFix()
    {
        var bug = new Bug(Bug.State.CreatedFixes);
        bug.DeclineFix();
        Assert.AreEqual(bug.getState(), Bug.State.DeclinedFixes);
    }

    [TestMethod]
    public void DeclineFixCreateFix()
    {
        var bug = new Bug(Bug.State.DeclinedFixes);
        bug.CreateFix();
        Assert.AreEqual(bug.getState(), Bug.State.CreatedFixes);
    }

    [TestMethod]
    public void AcceptFixClose()
    {
        var bug = new Bug(Bug.State.AcceptedFixes);
        bug.Close();
        Assert.AreEqual(bug.getState(), Bug.State.Closed);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AcceptFixFromClosed()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.AcceptFix();
    }
}