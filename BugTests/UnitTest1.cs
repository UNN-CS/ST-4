using BugPro;

namespace BugTests;

[TestClass]
public sealed class UnitTest1
{
    [TestMethod]
    public void NewBug_CurrentStateIsNew()
    {
        var bug = new Bug();

        Assert.AreEqual(BugState.New, bug.CurrentState);
    }

    [TestMethod]
    public void NewBug_HasOnlyStartTriageAsPermittedAction()
    {
        var bug = new Bug();

        AssertPermittedActions(bug, BugAction.StartTriage);
    }

    [TestMethod]
    public void StartTriage_FromNew_MovesBugToInTriage()
    {
        var bug = CreateBugAfter(BugAction.StartTriage);

        Assert.AreEqual(BugState.InTriage, bug.CurrentState);
    }

    [TestMethod]
    public void InTriage_SetNotTimeNow_MovesBugToNotTimeNow()
    {
        var bug = CreateBugAfter(BugAction.StartTriage, BugAction.SetNotTimeNow);

        Assert.AreEqual(BugState.NotTimeNow, bug.CurrentState);
    }

    [TestMethod]
    public void InTriage_SetNeedSeparateFix_MovesBugToNeedSeparateFix()
    {
        var bug = CreateBugAfter(BugAction.StartTriage, BugAction.SetNeedSeparateFix);

        Assert.AreEqual(BugState.NeedSeparateFix, bug.CurrentState);
    }

    [TestMethod]
    public void InTriage_SetExternalProblem_MovesBugToExternalProblem()
    {
        var bug = CreateBugAfter(BugAction.StartTriage, BugAction.SetExternalProblem);

        Assert.AreEqual(BugState.ExternalProblem, bug.CurrentState);
    }

    [TestMethod]
    public void InTriage_SetNeedMoreInfo_MovesBugToNeedMoreInfo()
    {
        var bug = CreateBugAfter(BugAction.StartTriage, BugAction.SetNeedMoreInfo);

        Assert.AreEqual(BugState.NeedMoreInfo, bug.CurrentState);
    }

    [TestMethod]
    public void InTriage_SetNotABug_MovesBugToNotABug()
    {
        var bug = CreateBugAfter(BugAction.StartTriage, BugAction.SetNotABug);

        Assert.AreEqual(BugState.NotABug, bug.CurrentState);
    }

    [TestMethod]
    public void InTriage_SetNotFixing_MovesBugToNotFixing()
    {
        var bug = CreateBugAfter(BugAction.StartTriage, BugAction.SetNotFixing);

        Assert.AreEqual(BugState.NotFixing, bug.CurrentState);
    }

    [TestMethod]
    public void InTriage_SetDuplicate_MovesBugToDuplicate()
    {
        var bug = CreateBugAfter(BugAction.StartTriage, BugAction.SetDuplicate);

        Assert.AreEqual(BugState.Duplicate, bug.CurrentState);
    }

    [TestMethod]
    public void InTriage_SetCannotReproduce_ThrowsInvalidOperationException()
    {
        var bug = CreateBugAfter(BugAction.StartTriage);

        Assert.ThrowsException<InvalidOperationException>(() =>
            bug.PerformAction(BugAction.SetCannotReproduce));
    }

    [TestMethod]
    public void InTriage_StartFixing_MovesBugToFixing()
    {
        var bug = CreateBugAfter(BugAction.StartTriage, BugAction.StartFixing);

        Assert.AreEqual(BugState.Fixing, bug.CurrentState);
    }

    [TestMethod]
    public void NotTimeNow_StartTriage_ReturnsBugToInTriage()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.SetNotTimeNow,
            BugAction.StartTriage);

        Assert.AreEqual(BugState.InTriage, bug.CurrentState);
    }

    [TestMethod]
    public void NotTimeNow_StartFixing_MovesBugToFixing()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.SetNotTimeNow,
            BugAction.StartFixing);

        Assert.AreEqual(BugState.Fixing, bug.CurrentState);
    }

    [TestMethod]
    public void NeedSeparateFix_StartTriage_ReturnsBugToInTriage()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.SetNeedSeparateFix,
            BugAction.StartTriage);

        Assert.AreEqual(BugState.InTriage, bug.CurrentState);
    }

    [TestMethod]
    public void NeedSeparateFix_StartFixing_MovesBugToFixing()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.SetNeedSeparateFix,
            BugAction.StartFixing);

        Assert.AreEqual(BugState.Fixing, bug.CurrentState);
    }

    [TestMethod]
    public void ExternalProblem_StartTriage_ReturnsBugToInTriage()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.SetExternalProblem,
            BugAction.StartTriage);

        Assert.AreEqual(BugState.InTriage, bug.CurrentState);
    }

    [TestMethod]
    public void ExternalProblem_StartFixing_MovesBugToFixing()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.SetExternalProblem,
            BugAction.StartFixing);

        Assert.AreEqual(BugState.Fixing, bug.CurrentState);
    }

    [TestMethod]
    public void NeedMoreInfo_StartTriage_ReturnsBugToInTriage()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.SetNeedMoreInfo,
            BugAction.StartTriage);

        Assert.AreEqual(BugState.InTriage, bug.CurrentState);
    }

    [TestMethod]
    public void NeedMoreInfo_StartFixing_MovesBugToFixing()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.SetNeedMoreInfo,
            BugAction.StartFixing);

        Assert.AreEqual(BugState.Fixing, bug.CurrentState);
    }

    [TestMethod]
    public void NotABug_VerifyIsOK_MovesBugToIsOK()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.SetNotABug,
            BugAction.VerifyIsOK);

        Assert.AreEqual(BugState.IsOK, bug.CurrentState);
    }

    [TestMethod]
    public void NotFixing_VerifyIsOK_MovesBugToIsOK()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.SetNotFixing,
            BugAction.VerifyIsOK);

        Assert.AreEqual(BugState.IsOK, bug.CurrentState);
    }

    [TestMethod]
    public void Duplicate_VerifyIsOK_MovesBugToIsOK()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.SetDuplicate,
            BugAction.VerifyIsOK);

        Assert.AreEqual(BugState.IsOK, bug.CurrentState);
    }

    [TestMethod]
    public void CannotReproduce_VerifyIsOK_MovesBugToIsOK()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.StartFixing,
            BugAction.SetCannotReproduce,
            BugAction.VerifyIsOK);

        Assert.AreEqual(BugState.IsOK, bug.CurrentState);
    }

    [TestMethod]
    public void CannotReproduce_ApproveAfterVerifyIsOK_MovesBugToClosed()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.StartFixing,
            BugAction.SetCannotReproduce,
            BugAction.VerifyIsOK,
            BugAction.Approve);

        Assert.AreEqual(BugState.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void CannotReproduce_RejectAfterVerifyIsOK_ReturnsBugToInTriage()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.StartFixing,
            BugAction.SetCannotReproduce,
            BugAction.VerifyIsOK,
            BugAction.Reject,
            BugAction.StartTriage);

        Assert.AreEqual(BugState.InTriage, bug.CurrentState);
    }

    [TestMethod]
    public void Fixing_VerifyIsProblemSolved_MovesBugToIsProblemSolved()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.StartFixing,
            BugAction.VerifyIsProblemSolved);

        Assert.AreEqual(BugState.IsProblemSolved, bug.CurrentState);
    }

    [TestMethod]
    public void Fixing_SetNeedMoreInfo_MovesBugToNeedMoreInfo()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.StartFixing,
            BugAction.SetNeedMoreInfo);

        Assert.AreEqual(BugState.NeedMoreInfo, bug.CurrentState);
    }

    [TestMethod]
    public void Fixing_SetCannotReproduce_MovesBugToCannotReproduce()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.StartFixing,
            BugAction.SetCannotReproduce);

        Assert.AreEqual(BugState.CannotReproduce, bug.CurrentState);
    }

    [TestMethod]
    public void IsOK_Approve_MovesBugToClosed()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.SetNotABug,
            BugAction.VerifyIsOK,
            BugAction.Approve);

        Assert.AreEqual(BugState.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void IsOK_Reject_MovesBugToReturned()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.SetNotABug,
            BugAction.VerifyIsOK,
            BugAction.Reject);

        Assert.AreEqual(BugState.Returned, bug.CurrentState);
    }

    [TestMethod]
    public void Returned_StartTriage_ReturnsBugToInTriage()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.SetNotABug,
            BugAction.VerifyIsOK,
            BugAction.Reject,
            BugAction.StartTriage);

        Assert.AreEqual(BugState.InTriage, bug.CurrentState);
    }

    [TestMethod]
    public void IsProblemSolved_Approve_MovesBugToClosed()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.StartFixing,
            BugAction.VerifyIsProblemSolved,
            BugAction.Approve);

        Assert.AreEqual(BugState.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void IsProblemSolved_Reject_MovesBugToReopened()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.StartFixing,
            BugAction.VerifyIsProblemSolved,
            BugAction.Reject);

        Assert.AreEqual(BugState.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void Closed_Reopen_MovesBugToReopened()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.StartFixing,
            BugAction.VerifyIsProblemSolved,
            BugAction.Approve,
            BugAction.Reopen);

        Assert.AreEqual(BugState.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void Reopened_StartTriage_ReturnsBugToInTriage()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.StartFixing,
            BugAction.VerifyIsProblemSolved,
            BugAction.Reject,
            BugAction.StartTriage);

        Assert.AreEqual(BugState.InTriage, bug.CurrentState);
    }

    [TestMethod]
    public void NewBug_StartFixing_ThrowsInvalidOperationException()
    {
        var bug = new Bug();

        Assert.ThrowsException<InvalidOperationException>(() =>
            bug.PerformAction(BugAction.StartFixing));
    }

    [TestMethod]
    public void Closed_Approve_ThrowsInvalidOperationException()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.StartFixing,
            BugAction.VerifyIsProblemSolved,
            BugAction.Approve);

        Assert.ThrowsException<InvalidOperationException>(() =>
            bug.PerformAction(BugAction.Approve));
    }

    [TestMethod]
    public void Returned_Reopen_ThrowsInvalidOperationException()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.SetNotABug,
            BugAction.VerifyIsOK,
            BugAction.Reject);

        Assert.ThrowsException<InvalidOperationException>(() =>
            bug.PerformAction(BugAction.Reopen));
    }

    [TestMethod]
    public void CanPerformAction_ForValidAction_ReturnsTrue()
    {
        var bug = new Bug();

        Assert.IsTrue(bug.CanPerformAction(BugAction.StartTriage));
    }

    [TestMethod]
    public void CanPerformAction_ForInvalidAction_ReturnsFalse()
    {
        var bug = new Bug();

        Assert.IsFalse(bug.CanPerformAction(BugAction.Approve));
    }

    [TestMethod]
    public void History_RecordsSuccessfulTransitions()
    {
        var bug = CreateBugAfter(
            BugAction.StartTriage,
            BugAction.StartFixing,
            BugAction.VerifyIsProblemSolved);

        Assert.AreEqual(3, bug.History.Count);
    }

    private static Bug CreateBugAfter(params BugAction[] actions)
    {
        var bug = new Bug();

        foreach (var action in actions)
        {
            bug.PerformAction(action);
        }

        return bug;
    }

    private static void AssertPermittedActions(Bug bug, params BugAction[] expectedActions)
    {
        CollectionAssert.AreEquivalent(
            expectedActions.ToList(),
            bug.PermittedActions.ToList());
    }
}
