using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public sealed class BugWorkflowTests
{
    private Bug CreateBug() => new Bug("Test Bug");

    private Bug BugInTriage()
    {
        var bug = CreateBug();
        bug.Fire(Trigger.Submit);
        return bug;
    }

    private Bug BugInWork()
    {
        var bug = BugInTriage();
        bug.Fire(Trigger.Assign);
        return bug;
    }

    #region Happy Path Tests

    [TestMethod]
    public void NewBug_InitialState_IsNew()
    {
        var bug = CreateBug();
        Assert.AreEqual(State.New, bug.CurrentState);
        Assert.AreEqual(0, bug.TransitionCount);
    }

    [TestMethod]
    public void NewBug_Submit_TransitionsToTriage()
    {
        var bug = CreateBug();
        bug.Fire(Trigger.Submit);
        Assert.AreEqual(State.Triage, bug.CurrentState);
        Assert.AreEqual(1, bug.TransitionCount);
    }

    [TestMethod]
    public void Triage_Assign_TransitionsToInWork()
    {
        var bug = BugInTriage();
        bug.Fire(Trigger.Assign);
        Assert.AreEqual(State.InWork, bug.CurrentState);
    }

    [TestMethod]
    public void InWork_CompleteWork_TransitionsToReview()
    {
        var bug = BugInWork();
        bug.Fire(Trigger.CompleteWork);
        Assert.AreEqual(State.Review, bug.CurrentState);
    }

    [TestMethod]
    public void Review_Accept_TransitionsToClosed()
    {
        var bug = BugInWork();
        bug.Fire(Trigger.CompleteWork);
        bug.Fire(Trigger.Accept);
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void Review_Reject_TransitionsToReturned()
    {
        var bug = BugInWork();
        bug.Fire(Trigger.CompleteWork);
        bug.Fire(Trigger.Reject);
        Assert.AreEqual(State.Returned, bug.CurrentState);
    }

    [TestMethod]
    public void Returned_Assign_TransitionsBackToInWork()
    {
        var bug = BugInWork();
        bug.Fire(Trigger.CompleteWork);
        bug.Fire(Trigger.Reject);
        bug.Fire(Trigger.Assign);
        Assert.AreEqual(State.InWork, bug.CurrentState);
    }

    #endregion

    #region Information Request Tests

    [TestMethod]
    public void Triage_AskForInfo_TransitionsToAwaitingInformation()
    {
        var bug = BugInTriage();
        bug.Fire(Trigger.AskForInfo);
        Assert.AreEqual(State.AwaitingInformation, bug.CurrentState);
    }

    [TestMethod]
    public void AwaitingInformation_ProvideInfo_TransitionsToTriage()
    {
        var bug = BugInTriage();
        bug.Fire(Trigger.AskForInfo);
        bug.Fire(Trigger.ProvideInfo);
        Assert.AreEqual(State.Triage, bug.CurrentState);
    }

    [TestMethod]
    public void InWork_AskForInfo_TransitionsToAwaitingInformation()
    {
        var bug = BugInWork();
        bug.Fire(Trigger.AskForInfo);
        Assert.AreEqual(State.AwaitingInformation, bug.CurrentState);
    }

    #endregion

    #region Deferred Tests

    [TestMethod]
    public void Triage_Defer_TransitionsToDeferred()
    {
        var bug = BugInTriage();
        bug.Fire(Trigger.Defer);
        Assert.AreEqual(State.Deferred, bug.CurrentState);
    }

    [TestMethod]
    public void Deferred_Resume_TransitionsToTriage()
    {
        var bug = BugInTriage();
        bug.Fire(Trigger.Defer);
        bug.Fire(Trigger.Resume);
        Assert.AreEqual(State.Triage, bug.CurrentState);
    }

    #endregion

    #region Reopen/Cancel Tests

    [TestMethod]
    public void Closed_Reopen_TransitionsToTriage()
    {
        var bug = BugInTriage();
        bug.Fire(Trigger.Reject); // Close from triage
        bug.Fire(Trigger.Reopen);
        Assert.AreEqual(State.Triage, bug.CurrentState);
    }

    [TestMethod]
    public void New_Cancel_TransitionsToClosed()
    {
        var bug = CreateBug();
        bug.Fire(Trigger.Cancel);
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void AwaitingInformation_Cancel_TransitionsToClosed()
    {
        var bug = BugInTriage();
        bug.Fire(Trigger.AskForInfo);
        bug.Fire(Trigger.Cancel);
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    #endregion

    #region Stateless Exception Tests (Negative Tests)

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void NewBug_Assign_ThrowsException()
    {
        var bug = CreateBug();
        bug.Fire(Trigger.Assign);
    }

    [TestMethod]
    public void Triage_Submit_ThrowsException_CheckMessage()
    {
        var bug = BugInTriage();
        var ex = Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Trigger.Submit));
        Assert.IsNotNull(ex.Message);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Closed_Accept_ThrowsException()
    {
        var bug = BugInTriage();
        bug.Fire(Trigger.Reject);
        bug.Fire(Trigger.Accept);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Deferred_CompleteWork_ThrowsException()
    {
        var bug = BugInTriage();
        bug.Fire(Trigger.Defer);
        bug.Fire(Trigger.CompleteWork);
    }

    #endregion

    #region Utility Methods Tests

    [TestMethod]
    public void CanFire_ReturnsCorrectValues()
    {
        var bug = CreateBug();
        Assert.IsTrue(bug.CanFire(Trigger.Submit));
        Assert.IsFalse(bug.CanFire(Trigger.Assign));
        Assert.IsTrue(bug.CanFire(Trigger.Cancel));
    }

    [TestMethod]
    public void PermittedTriggers_ContainsExpectedTriggers()
    {
        var bug = BugInTriage();
        var triggers = bug.PermittedTriggers.ToList();
        CollectionAssert.Contains(triggers, Trigger.Assign);
        CollectionAssert.Contains(triggers, Trigger.AskForInfo);
        CollectionAssert.Contains(triggers, Trigger.Defer);
        CollectionAssert.Contains(triggers, Trigger.Reject);
        Assert.IsFalse(triggers.Contains(Trigger.Submit));
    }

    #endregion
}
