namespace BugTests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

[TestClass]
public class BugTests
{
    [DataTestMethod]
    [DataRow(Bug.State.Assigned)]
    [DataRow(Bug.State.FixAccepted)]
    public void CloseTest(Bug.State initialState)
    {
        var bug = new Bug(initialState);
        bug.Close();
        Assert.AreEqual(bug.getState(), Bug.State.Closed);
    }

    [DataTestMethod]
    [DataRow(Bug.State.Open)]
    [DataRow(Bug.State.FixCreated)]
    [DataRow(Bug.State.FixDeclined)]
    [DataRow(Bug.State.Deferred)]
    [DataRow(Bug.State.Closed)]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CloseTestException(Bug.State initialState)
    {
        var bug = new Bug(initialState);
        bug.Close();
    }

    [DataTestMethod]
    [DataRow(Bug.State.Open)]
    [DataRow(Bug.State.Assigned)]
    [DataRow(Bug.State.Deferred)] 
    [DataRow(Bug.State.Closed)]
    public void AssignTest(Bug.State initialState)
    {
        var bug = new Bug(initialState);
        bug.Assign();
        Assert.AreEqual(bug.getState(), Bug.State.Assigned);
    }

    [DataTestMethod]
    [DataRow(Bug.State.FixCreated)]
    [DataRow(Bug.State.FixAccepted)]
    [DataRow(Bug.State.FixDeclined)]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AssignTestException(Bug.State initialState)
    {
        var bug = new Bug(initialState);
        bug.Assign();
    }

    [DataTestMethod]
    [DataRow(Bug.State.Assigned)]
    public void DeferTest(Bug.State initialState)
    {
        var bug = new Bug(initialState);
        bug.Defer();
        Assert.AreEqual(bug.getState(), Bug.State.Deferred);
    }

    [DataTestMethod]
    [DataRow(Bug.State.Open)]
    [DataRow(Bug.State.FixCreated)]
    [DataRow(Bug.State.FixAccepted)]
    [DataRow(Bug.State.FixDeclined)]
    [DataRow(Bug.State.Deferred)]
    [DataRow(Bug.State.Closed)]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeferTestException(Bug.State initialState)
    {
        var bug = new Bug(initialState);
        bug.Defer();
    }

    [DataTestMethod]
    [DataRow(Bug.State.Assigned)]
    [DataRow(Bug.State.FixCreated)]
    [DataRow(Bug.State.FixDeclined)]
    public void CreateFixTest(Bug.State initialState)
    {
        var bug = new Bug(initialState);
        bug.CreateFix();
        Assert.AreEqual(bug.getState(), Bug.State.FixCreated);
    }

    [DataTestMethod]
    [DataRow(Bug.State.Open)]
    [DataRow(Bug.State.FixAccepted)]
    [DataRow(Bug.State.Deferred)]
    [DataRow(Bug.State.Closed)]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CreateFixTestException(Bug.State initialState)
    {
        var bug = new Bug(initialState);
        bug.CreateFix();
    }


    [DataTestMethod]
    [DataRow(Bug.State.FixCreated)]
    public void AcceptFixTest(Bug.State initialState)
    {
        var bug = new Bug(initialState);
        bug.AcceptFix();
        Assert.AreEqual(bug.getState(), Bug.State.FixAccepted);
    }

    [DataTestMethod]
    [DataRow(Bug.State.Open)]
    [DataRow(Bug.State.Assigned)]
    [DataRow(Bug.State.FixAccepted)]
    [DataRow(Bug.State.FixDeclined)]
    [DataRow(Bug.State.Deferred)]
    [DataRow(Bug.State.Closed)]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AcceptFixTestException(Bug.State initialState)
    {
        var bug = new Bug(initialState);
        bug.AcceptFix();
    }

    [DataTestMethod]
    [DataRow(Bug.State.FixCreated)]
    public void DeclineFixTest(Bug.State initialState)
    {
        var bug = new Bug(initialState);
        bug.DeclineFix();
        Assert.AreEqual(bug.getState(), Bug.State.FixDeclined);
    }

    [DataTestMethod]
    [DataRow(Bug.State.Open)]
    [DataRow(Bug.State.Assigned)]
    [DataRow(Bug.State.FixAccepted)]
    [DataRow(Bug.State.FixDeclined)]
    [DataRow(Bug.State.Deferred)]
    [DataRow(Bug.State.Closed)]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeclineFixTestException(Bug.State initialState)
    {
        var bug = new Bug(initialState);
        bug.DeclineFix();
    }
}
