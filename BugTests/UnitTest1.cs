using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BugTests;

[TestClass]
public class BugTests
{
    // Базовые переходы
    [TestMethod]
    public void InitialState_IsOpen()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.AreEqual(Bug.State.Open, bug.GetState());
    }

    [TestMethod]
    public void Assign_FromOpen_ToAssigned()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void Close_FromAssigned_ToClosed()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void Defer_FromAssigned_ToDefered()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.GetState());
    }

    [TestMethod]
    public void Assign_FromDefered_ToAssigned()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void Verify_FromClosed_ToVerified()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Verify();
        Assert.AreEqual(Bug.State.Verified, bug.GetState());
    }

    [TestMethod]
    public void Reopen_FromClosed_ToReopened()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.GetState());
    }

    // Игнорируемые триггеры
    [TestMethod]
    public void Assign_FromAssigned_Ignored()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Assign(); // Должно игнорироваться
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    // Исключения
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Close_FromOpen_ThrowsException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Close();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Verify_FromOpen_ThrowsException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Verify();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Reopen_FromOpen_ThrowsException()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Reopen();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Defer_FromClosed_ThrowsException()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Defer();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Assign_FromVerified_ThrowsException()
    {
        var bug = new Bug(Bug.State.Verified);
        bug.Assign();
    }

    // Комплексные сценарии
    [TestMethod]
    public void MultipleTransitions_OpenToClosed()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Reopen();
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void FullLifecycle_WithVerification()
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.Close();
        bug.Verify();
        bug.Reopen();
        bug.Assign();
        bug.Defer();
        bug.Assign();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    // Граничные случаи
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Close_FromDefered_ThrowsException()
    {
        var bug = new Bug(Bug.State.Defered);
        bug.Close();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Verify_FromReopened_ThrowsException()
    {
        var bug = new Bug(Bug.State.Reopened);
        bug.Verify();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Defer_FromReopened_ThrowsException()
    {
        var bug = new Bug(Bug.State.Reopened);
        bug.Defer();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Reopen_FromAssigned_ThrowsException()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Reopen();
    }

    [TestMethod]
    public void State_AfterMultipleIgnores()
    {
        var bug = new Bug(Bug.State.Assigned);
        bug.Assign(); // Игнор
        bug.Assign(); // Игнор
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }
}