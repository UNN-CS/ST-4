namespace BugTests;

using BugPro;

[TestClass]
public sealed class BugTests
{
    [TestMethod]
    public void InitialState_New()
    {
        var bug = new Bug();
        Assert.AreEqual(BugState.New, bug.CurrentState);
    }

    [TestMethod]
    public void New_StartTriage_GoesToInTriage()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        Assert.AreEqual(BugState.InTriage, bug.CurrentState);
    }

    [TestMethod]
    public void New_InvalidAction_Throws()
    {
        var bug = new Bug();
        Assert.Throws<InvalidOperationException>(() => bug.PerformAction(BugAction.Approve));
    }

    // Transitions from InTriage
    [TestMethod]
    public void InTriage_SetNotTimeNow_GoesToNotTimeNow()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetNotTimeNow);
        Assert.AreEqual(BugState.NotTimeNow, bug.CurrentState);
    }

    [TestMethod]
    public void InTriage_SetNeedSeparateFix_GoesToNeedSeparateFix()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetNeedSeparateFix);
        Assert.AreEqual(BugState.NeedSeparateFix, bug.CurrentState);
    }

    [TestMethod]
    public void InTriage_SetExternalProblem_GoesToExternalProblem()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetExternalProblem);
        Assert.AreEqual(BugState.ExternalProblem, bug.CurrentState);
    }

    [TestMethod]
    public void InTriage_SetNeedMoreInfo_GoesToNeedMoreInfo()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetNeedMoreInfo);
        Assert.AreEqual(BugState.NeedMoreInfo, bug.CurrentState);
    }

    [TestMethod]
    public void InTriage_SetNotABug_GoesToNotABug()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetNotABug);
        Assert.AreEqual(BugState.NotABug, bug.CurrentState);
    }

    [TestMethod]
    public void InTriage_SetNotFixing_GoesToNotFixing()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetNotFixing);
        Assert.AreEqual(BugState.NotFixing, bug.CurrentState);
    }

    [TestMethod]
    public void InTriage_SetDuplicate_GoesToDuplicate()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetDuplicate);
        Assert.AreEqual(BugState.Duplicate, bug.CurrentState);
    }

    [TestMethod]
    public void InTriage_StartFixing_GoesToFixing()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.StartFixing);
        Assert.AreEqual(BugState.Fixing, bug.CurrentState);
    }

    // Transitions from NotTimeNow
    [TestMethod]
    public void NotTimeNow_StartTriage_GoesToInTriage()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetNotTimeNow);
        bug.PerformAction(BugAction.StartTriage);
        Assert.AreEqual(BugState.InTriage, bug.CurrentState);
    }

    [TestMethod]
    public void NotTimeNow_StartFixing_GoesToFixing()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetNotTimeNow);
        bug.PerformAction(BugAction.StartFixing);
        Assert.AreEqual(BugState.Fixing, bug.CurrentState);
    }

    // Transitions from NeedSeparateFix
    [TestMethod]
    public void NeedSeparateFix_StartTriage_GoesToInTriage()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetNeedSeparateFix);
        bug.PerformAction(BugAction.StartTriage);
        Assert.AreEqual(BugState.InTriage, bug.CurrentState);
    }

    [TestMethod]
    public void NeedSeparateFix_StartFixing_GoesToFixing()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetNeedSeparateFix);
        bug.PerformAction(BugAction.StartFixing);
        Assert.AreEqual(BugState.Fixing, bug.CurrentState);
    }

    // Transitions from ExternalProblem
    [TestMethod]
    public void ExternalProblem_StartTriage_GoesToInTriage()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetExternalProblem);
        bug.PerformAction(BugAction.StartTriage);
        Assert.AreEqual(BugState.InTriage, bug.CurrentState);
    }

    [TestMethod]
    public void ExternalProblem_StartFixing_GoesToFixing()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetExternalProblem);
        bug.PerformAction(BugAction.StartFixing);
        Assert.AreEqual(BugState.Fixing, bug.CurrentState);
    }

    // Transitions from NeedMoreInfo
    [TestMethod]
    public void NeedMoreInfo_StartTriage_GoesToInTriage()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetNeedMoreInfo);
        bug.PerformAction(BugAction.StartTriage);
        Assert.AreEqual(BugState.InTriage, bug.CurrentState);
    }

    [TestMethod]
    public void NeedMoreInfo_StartFixing_GoesToFixing()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetNeedMoreInfo);
        bug.PerformAction(BugAction.StartFixing);
        Assert.AreEqual(BugState.Fixing, bug.CurrentState);
    }

    // Transitions from NotABug
    [TestMethod]
    public void NotABug_VerifyIsOK_GoesToIsOK()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetNotABug);
        bug.PerformAction(BugAction.VerifyIsOK);
        Assert.AreEqual(BugState.IsOK, bug.CurrentState);
    }

    // Transitions from NotFixing
    [TestMethod]
    public void NotFixing_VerifyIsOK_GoesToIsOK()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetNotFixing);
        bug.PerformAction(BugAction.VerifyIsOK);
        Assert.AreEqual(BugState.IsOK, bug.CurrentState);
    }

    // Transitions from Duplicate
    [TestMethod]
    public void Duplicate_VerifyIsOK_GoesToIsOK()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetDuplicate);
        bug.PerformAction(BugAction.VerifyIsOK);
        Assert.AreEqual(BugState.IsOK, bug.CurrentState);
    }

    // Transitions from CannotReproduce
    [TestMethod]
    public void CannotReproduce_VerifyIsOK_GoesToIsOK()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.StartFixing);
        bug.PerformAction(BugAction.SetCannotReproduce);
        bug.PerformAction(BugAction.VerifyIsOK);
        Assert.AreEqual(BugState.IsOK, bug.CurrentState);
    }


    // Transitions from Fixing
    [TestMethod]
    public void Fixing_VerifyIsProblemSolved_GoesToIsProblemSolved()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.StartFixing);
        bug.PerformAction(BugAction.VerifyIsProblemSolved);
        Assert.AreEqual(BugState.IsProblemSolved, bug.CurrentState);
    }

    [TestMethod]
    public void Fixing_SetNeedMoreInfo_GoesToNeedMoreInfo()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.StartFixing);
        bug.PerformAction(BugAction.SetNeedMoreInfo);
        Assert.AreEqual(BugState.NeedMoreInfo, bug.CurrentState);
    }

    [TestMethod]
    public void Fixing_SetCannotReproduce_GoesToCannotReproduce()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.StartFixing);
        bug.PerformAction(BugAction.SetCannotReproduce);
        Assert.AreEqual(BugState.CannotReproduce, bug.CurrentState);
    }

    // Transitions from IsOK
    [TestMethod]
    public void IsOK_Approve_GoesToClosed()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetNotABug);
        bug.PerformAction(BugAction.VerifyIsOK);
        bug.PerformAction(BugAction.Approve);
        Assert.AreEqual(BugState.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void IsOK_Reject_GoesToReturned()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.SetNotABug);
        bug.PerformAction(BugAction.VerifyIsOK);
        bug.PerformAction(BugAction.Reject);
        Assert.AreEqual(BugState.Returned, bug.CurrentState);
    }

    // Transitions from IsProblemSolved
    [TestMethod]
    public void IsProblemSolved_Approve_GoesToClosed()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.StartFixing);
        bug.PerformAction(BugAction.VerifyIsProblemSolved);
        bug.PerformAction(BugAction.Approve);
        Assert.AreEqual(BugState.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void IsProblemSolved_Reject_GoesToReopened()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.StartFixing);
        bug.PerformAction(BugAction.VerifyIsProblemSolved);
        bug.PerformAction(BugAction.Reject);
        Assert.AreEqual(BugState.Reopened, bug.CurrentState);
    }

    // Transitions from Closed
    [TestMethod]
    public void Closed_Reopen_GoesToReopened()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.StartFixing);
        bug.PerformAction(BugAction.VerifyIsProblemSolved);
        bug.PerformAction(BugAction.Approve);
        bug.PerformAction(BugAction.Reopen);
        Assert.AreEqual(BugState.Reopened, bug.CurrentState);
    }

    // Transitions from Reopened
    [TestMethod]
    public void Reopened_StartTriage_GoesToInTriage()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.StartFixing);
        bug.PerformAction(BugAction.VerifyIsProblemSolved);
        bug.PerformAction(BugAction.Reject);
        bug.PerformAction(BugAction.StartTriage);
        Assert.AreEqual(BugState.InTriage, bug.CurrentState);
    }


    // Invalid transitions (expect exceptions)
    [TestMethod]
    public void FromNew_Approve_Throws()
    {
        var bug = new Bug();
        Assert.Throws<InvalidOperationException>(() => bug.PerformAction(BugAction.Approve));
    }

    [TestMethod]
    public void FromInTriage_Approve_Throws()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        Assert.Throws<InvalidOperationException>(() => bug.PerformAction(BugAction.Approve));
    }

    [TestMethod]
    public void FromFixing_StartTriage_Throws()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.StartFixing);
        Assert.Throws<InvalidOperationException>(() => bug.PerformAction(BugAction.StartTriage));
    }

    [TestMethod]
    public void FromClosed_StartFixing_Throws()
    {
        var bug = new Bug();
        bug.PerformAction(BugAction.StartTriage);
        bug.PerformAction(BugAction.StartFixing);
        bug.PerformAction(BugAction.VerifyIsProblemSolved);
        bug.PerformAction(BugAction.Approve);
        Assert.Throws<InvalidOperationException>(() => bug.PerformAction(BugAction.StartFixing));
    }
}
