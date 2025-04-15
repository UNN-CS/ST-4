using System;
using System.Diagnostics;

namespace BugTests;

[TestClass]
public sealed class BugTests
{
    [TestMethod]
    public void TestInitialState()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.AreEqual(Bug.State.Open, bug.GetState());
    }

    [TestMethod]
    public void TestAssignFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void TestAssignWithName()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign("Петров");
        Assert.AreEqual("Петров", bug.AssignedTo);
    }

    [TestMethod]
    public void TestDeferFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.GetState());
    }

    [TestMethod]
    public void TestCloseFromAssigned()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void TestRejectFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Reject();
        Assert.AreEqual(Bug.State.Rejected, bug.GetState());
    }

    [TestMethod]
    public void TestReopenFromRejected()
    {
        var bug = new Bug(Bug.State.Rejected);
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.GetState());
    }

    [TestMethod]
    public void TestHoldFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Hold();
        Assert.AreEqual(Bug.State.OnHold, bug.GetState());
    }

    [TestMethod]
    public void TestResumeFromOnHold()
    {
        var bug = new Bug(Bug.State.OnHold);
        bug.Resume();
        Assert.AreEqual(Bug.State.Open, bug.GetState());
    }

    [TestMethod]
    public void TestStartWorkFromAssigned()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.StartWork();
        Assert.AreEqual(Bug.State.InProgress, bug.GetState());
    }

    [TestMethod]
    public void TestFixFromInProgress()
    {
        var bug = new Bug(Bug.State.InProgress);
        bug.Fix();
        Assert.AreEqual(Bug.State.Fixed, bug.GetState());
    }

    [TestMethod]
    public void TestStartTestFromFixed()
    {
        var bug = new Bug(Bug.State.Fixed);
        bug.StartTest();
        Assert.AreEqual(Bug.State.Testing, bug.GetState());
    }

    [TestMethod]
    public void TestCloseFromTesting()
    {
        var bug = new Bug(Bug.State.Testing);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void TestReopenFromTesting()
    {
        var bug = new Bug(Bug.State.Testing);
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.GetState());
    }

    [TestMethod]
    public void TestVerifyFromClosed()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Verify();
        Assert.AreEqual(Bug.State.Verified, bug.GetState());
    }

    [TestMethod]
    public void TestFullWorkflow()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign("Иванов");
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        
        bug.StartWork();
        Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        
        bug.Fix();
        Assert.AreEqual(Bug.State.Fixed, bug.GetState());
        
        bug.StartTest();
        Assert.AreEqual(Bug.State.Testing, bug.GetState());
        
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
        
        bug.Verify();
        Assert.AreEqual(Bug.State.Verified, bug.GetState());
    }

    [TestMethod]
    public void TestDoubleAssign()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void TestSetPriority()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetPriority(3);
        Assert.AreEqual(3, bug.Priority);
    }

    [TestMethod]
    public void TestInitialPriority()
    {
        var bug = new Bug(Bug.State.Open, "Тестовый баг", 5);
        Assert.AreEqual(5, bug.Priority);
    }

    [TestMethod]
    public void TestInitialTitle()
    {
        var bug = new Bug(Bug.State.Open, "Тестовый баг");
        Assert.AreEqual("Тестовый баг", bug.Title);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestInvalidTransitionCloseFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Close();
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestInvalidTransitionFixFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Fix();
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestInvalidTransitionStartTestFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.StartTest();
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestInvalidTransitionVerifyFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Verify();
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestInvalidTransitionReopenFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Reopen();
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestInvalidTransitionStartWorkFromOpen()
    {
        var bug = new Bug(Bug.State.Open);
        bug.StartWork(); 
    }
    
    [TestMethod]
    public void TestReopenFromVerified()
    {
        var bug = new Bug(Bug.State.Verified);
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.GetState());
    }
    
    [TestMethod]
    public void TestResumeFromDeferWillFail()
    {
        var bug = new Bug(Bug.State.Defered);
        try {
            bug.Resume();
            Assert.AreEqual(Bug.State.Open, bug.GetState());
        }
        catch (InvalidOperationException) {
            Assert.Fail("Переход должен быть разрешен");
        }
    }
}
