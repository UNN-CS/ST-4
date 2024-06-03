//Copyright Kutarin Alexandr

namespace Tests;

[TestClass]
public class BugTests
{
    [TestMethod]
    public void TestMethod1(){
        Assert.IsTrue(true);
    }

    [TestMethod]
    public void TestCloseAssign(){
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void TestOpenAssign(){
        var bug = new Bug(Bug.State.Open);
        bug.Close();
        Assert.AreNotEqual(Bug.State.Open, bug.getState());
    }

    [TestMethod]
    public void TestDeferAssign(){
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
        Assert.AreNotEqual(Bug.State.Open, bug.getState());
    }

    [TestMethod]
    public void TestAssignClose(){
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void TestDeferAssignClose(){
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }

    [TestMethod]
    public void TestDeferToAssigned(){
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void TestDeferToClosed(){
        var bug = new Bug(Bug.State.Defered);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void TestAssignedToDefer(){
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }

    [TestMethod]
    public void TestAssignedToClosed(){
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void TestOpenToFeached(){
        var bug = new Bug(Bug.State.Open);
        bug.Feach();
        Assert.AreEqual(Bug.State.Feached, bug.getState());
    }

    [TestMethod]
    public void TestAssignedToFeached(){
        var bug = new Bug(Bug.State.Assigned);
        bug.Feach();
        Assert.AreEqual(Bug.State.Feached, bug.getState());
    }

    [TestMethod]
    public void TestDeferedToFeached(){
        var bug = new Bug(Bug.State.Defered);
        bug.Feach();
        Assert.AreEqual(Bug.State.Feached, bug.getState());
    }

    [TestMethod]
    public void TestClosedToFeached(){
        var bug = new Bug(Bug.State.Closed);
        bug.Feach();
        Assert.AreNotEqual(Bug.State.Closed, bug.getState());
    }
}
