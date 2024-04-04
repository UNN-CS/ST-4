namespace BugTests;

using System.Reflection;
using BugPro;

[TestClass]
public class BugTests
{
    [TestMethod]
    public void AssignFromOpen()
    {
        var bug = new Bug(BugPro.State.Open);
        bug.Assign();
        Assert.AreEqual(State.Assigned, bug.State);
    }

    [TestMethod]
    public void AssignFromAssigned()
    {
        var bug = new Bug(BugPro.State.Assigned);
        bug.Assign();
        Assert.AreEqual(State.Assigned, bug.State);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AssignFromFixCreated()
    {
        var bug = new Bug(BugPro.State.FixCreated);
        bug.Assign();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AssignFromFixAccepted()
    {
        var bug = new Bug(BugPro.State.FixAccepted);
        bug.Assign();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AssignFromFixDeclined()
    {
        var bug = new Bug(BugPro.State.FixDeclined);
        bug.Assign();
    }
    
    [TestMethod]
    public void AssignFromDeferred()
    {
        var bug = new Bug(BugPro.State.Deferred);
        bug.Assign();
        Assert.AreEqual(State.Assigned, bug.State);
    }
    [TestMethod]
    public void AssignFromClosed()
    {
        var bug = new Bug(BugPro.State.Closed);
        bug.Assign();
        Assert.AreEqual(State.Assigned, bug.State);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CreateFixFromOpen()
    {
        var bug = new Bug(BugPro.State.Open);
        bug.CreateFix();
    }
    [TestMethod]
    public void CreateFixFromAssigned()
    {
        var bug = new Bug(BugPro.State.Assigned);
        bug.CreateFix();
        Assert.AreEqual(BugPro.State.FixCreated, bug.State);
    }
    [TestMethod]
    public void CreateFixFromFixCreated()
    {
        var bug = new Bug(BugPro.State.FixCreated);
        bug.CreateFix();
        Assert.AreEqual(BugPro.State.FixCreated, bug.State);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CreateFixFromFixAccepted()
    {
        var bug = new Bug(BugPro.State.FixAccepted);
        bug.CreateFix();
    }
    [TestMethod]
    public void CreateFixFromFixDeclined()
    {
        var bug = new Bug(BugPro.State.FixDeclined);
        bug.CreateFix();
        Assert.AreEqual(BugPro.State.FixCreated, bug.State);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CreateFixFromDeferred()
    {
        var bug = new Bug(BugPro.State.Deferred);
        bug.CreateFix();
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CreateFixFromClosed()
    {
        var bug = new Bug(BugPro.State.Closed);
        bug.CreateFix();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AcceptFixFromOpen()
    {
        var bug = new Bug(BugPro.State.Open);
        bug.AcceptFix();
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AcceptFixFromAssigned()
    {
        var bug = new Bug(BugPro.State.Assigned);
        bug.AcceptFix();
    }
    [TestMethod]
    public void AcceptFixFromFixCreated()
    {
        var bug = new Bug(BugPro.State.FixCreated);
        bug.AcceptFix();
        Assert.AreEqual(BugPro.State.FixAccepted, bug.State);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AcceptFixFromFixAccepted()
    {
        var bug = new Bug(BugPro.State.FixAccepted);
        bug.AcceptFix();
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AcceptFixFromFixDeclined()
    {
        var bug = new Bug(BugPro.State.FixDeclined);
        bug.AcceptFix();
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AcceptFixFromDeferred()
    {
        var bug = new Bug(BugPro.State.Deferred);
        bug.AcceptFix();
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AcceptFixFromClosed()
    {
        var bug = new Bug(BugPro.State.Closed);
        bug.AcceptFix();
    }
    
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeclineFixFromOpen()
    {
        var bug = new Bug(BugPro.State.Open);
        bug.DeclineFix();
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeclineFixFromAssigned()
    {
        var bug = new Bug(BugPro.State.Assigned);
        bug.DeclineFix();
    }
    [TestMethod]
    public void DeclineFixFromFixCreated()
    {
        var bug = new Bug(BugPro.State.FixCreated);
        bug.DeclineFix();
        Assert.AreEqual(BugPro.State.FixDeclined, bug.State);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeclineFixFromFixAccepted()
    {
        var bug = new Bug(BugPro.State.FixAccepted);
        bug.DeclineFix();
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeclineFixFromFixDeclined()
    {
        var bug = new Bug(BugPro.State.FixDeclined);
        bug.DeclineFix();
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeclineFixFromDeferred()
    {
        var bug = new Bug(BugPro.State.Deferred);
        bug.DeclineFix();
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeclineFixFromClosed()
    {
        var bug = new Bug(BugPro.State.Closed);
        bug.DeclineFix();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeferFromOpen()
    {
        var bug = new Bug(BugPro.State.Open);
        bug.Defer();
    }
    [TestMethod]
    public void DeferFromAssigned()
    {
        var bug = new Bug(BugPro.State.Assigned);
        bug.Defer();
        Assert.AreEqual(BugPro.State.Deferred, bug.State);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeferFromFixCreated()
    {
        var bug = new Bug(BugPro.State.FixCreated);
        bug.Defer();
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeferFromFixAccepted()
    {
        var bug = new Bug(BugPro.State.FixAccepted);
        bug.Defer();
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeferFromFixDeclined()
    {
        var bug = new Bug(BugPro.State.FixDeclined);
        bug.Defer();
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeferFromDeferred()
    {
        var bug = new Bug(BugPro.State.Deferred);
        bug.Defer();
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeferFromClosed()
    {
        var bug = new Bug(BugPro.State.Closed);
        bug.Defer();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CloseFromOpen()
    {
        var bug = new Bug(BugPro.State.Open);
        bug.Close();
    }
    [TestMethod]
    public void CloseFromAssigned()
    {
        var bug = new Bug(BugPro.State.Assigned);
        bug.Close();
        Assert.AreEqual(BugPro.State.Closed, bug.State);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CloseFromFixCreated()
    {
        var bug = new Bug(BugPro.State.FixCreated);
        bug.Close();
    }
    [TestMethod]
    public void CloseFromFixAccepted()
    {
        var bug = new Bug(BugPro.State.FixAccepted);
        bug.Close();
        Assert.AreEqual(BugPro.State.Closed, bug.State);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CloseFromFixDeclined()
    {
        var bug = new Bug(BugPro.State.FixDeclined);
        bug.Close();
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CloseFromDeferred()
    {
        var bug = new Bug(BugPro.State.Deferred);
        bug.Close();
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CloseFromClosed()
    {
        var bug = new Bug(BugPro.State.Closed);
        bug.Close();
    }
}