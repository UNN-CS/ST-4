namespace BugTests;

using BugPro;

[TestClass]
public class BugTests
{
    [TestMethod]
    public void OpenAssign() {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void CloseAssign() {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void FromFixCreatedAssign() {
        var bug = new Bug(Bug.State.Closed);
        bug.AcceptFix();
    }

    [TestMethod]
    public void FromDeferedAssign() {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void FromClosedAssign() {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void CloseFromAssigned() {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CloseFromFixCreated() {
        var bug = new Bug(Bug.State.FixCreated);
        bug.Close();
    }

    [TestMethod]
    public void CloseFromFixAccepted() {
        var bug = new Bug(Bug.State.FixAccepted);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void DeferFromAssigned() {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeferFromFixCreated() {
        var bug = new Bug(Bug.State.FixCreated);
        bug.Defer();
    }
}
