namespace BugTests;
using BugPro;
[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestInit()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.AreEqual(Bug.State.Open, bug.getState());
    }
    [TestMethod]
    public void TestAssigned1()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }
    [TestMethod]
    public void TestAssigned2()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }
    [TestMethod]
    public void TestAssigned3()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }
    [TestMethod]
    public void TestAssigned2Exception()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Defer();
            }
            );
    }
    [TestMethod]
    public void TestGetState1()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }
    [TestMethod]
    public void TestGetState2()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Reject();
        Assert.AreEqual(Bug.State.Rejected, bug.getState());
    }
    [TestMethod]
    public void TestGetState3()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }
    [TestMethod]
    public void TestGetState4()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Reject();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }
    [TestMethod]
    public void TestGetState5()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Reject();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }
    [TestMethod]
    public void TestGetState6()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }
    [TestMethod]
    public void TestGetState7()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Assign();
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }
    [TestMethod]
    public void TestGetState8()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Defer();
        bug.Assign();
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }
    [TestMethod]
    public void TestGetState9()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Reject();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }
    [TestMethod]
    public void TestAssigned4Exception()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Close();
            }
            );
    }
    [TestMethod]
    public void TestAssigned5Exception()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Defer();
            }
            );
    }
    [TestMethod]
    public void TestAssigned6Exception()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Reject();
            }
            );
    }
    [TestMethod]
    public void TestClosedException1()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Close();
            }
            );
    }
    [TestMethod]
    public void TestClosedException2()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Defer();
            }
            );
    }
    [TestMethod]
    public void TestClosedException3()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Reject();
            }
            );
    }
    [TestMethod]
    public void TestRejectException1()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Reject();
        Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Defer();
            }
            );
    }
    [TestMethod]
    public void TestRejectException2()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Reject();
        Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Reject(); 
            }
            );
    }
}
