using BugPro;

namespace BugTests;

[TestClass]
public sealed class UnitTest1
{
    [TestMethod]
    public void Initial_state_is_NewDefect()
    {
        var bug = new Bug();
        Assert.AreEqual(BugState.NewDefect, bug.State);
    }

    [TestMethod]
    public void SubmitToTriage_moves_to_DefectTriage()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        Assert.AreEqual(BugState.DefectTriage, bug.State);
    }

    [TestMethod]
    public void From_triage_StartFixing_goes_to_Fixing()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.StartFixing);
        Assert.AreEqual(BugState.Fixing, bug.State);
    }

    [TestMethod]
    public void Fixing_ProblemSolved_closes_bug()
    {
        var bug = Fixing();
        bug.Fire(BugTrigger.ProblemSolved);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Full_path_triage_fix_close()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.StartFixing);
        bug.Fire(BugTrigger.ProblemSolved);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Triage_CloseAsNotABug_closes()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.CloseAsNotABug);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Triage_CloseAsWontFix_closes()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.CloseAsWontFix);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Triage_CloseAsDuplicate_closes()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.CloseAsDuplicate);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Fixing_ReturnAfterFailedFix_goes_to_Returned_then_triage()
    {
        var bug = Fixing();
        bug.Fire(BugTrigger.ReturnAfterFailedFix);
        Assert.AreEqual(BugState.Returned, bug.State);
        bug.Fire(BugTrigger.CompleteReturnToTriage);
        Assert.AreEqual(BugState.DefectTriage, bug.State);
    }

    [TestMethod]
    public void Triage_MarkNotReproducible_goes_to_NotReproducible()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.MarkNotReproducible);
        Assert.AreEqual(BugState.NotReproducible, bug.State);
    }

    [TestMethod]
    public void NotReproducible_ConfirmOk_closes()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.MarkNotReproducible);
        bug.Fire(BugTrigger.ConfirmNotReproducibleOk);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void NotReproducible_Reject_goes_to_Returned_then_triage()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.MarkNotReproducible);
        bug.Fire(BugTrigger.RejectNotReproducible);
        Assert.AreEqual(BugState.Returned, bug.State);
        bug.Fire(BugTrigger.CompleteReturnToTriage);
        Assert.AreEqual(BugState.DefectTriage, bug.State);
    }

    [TestMethod]
    public void Fixing_can_move_to_NotReproducible()
    {
        var bug = Fixing();
        bug.Fire(BugTrigger.MarkNotReproducible);
        Assert.AreEqual(BugState.NotReproducible, bug.State);
    }

    [TestMethod]
    public void Fixing_RequestMoreInfo_goes_to_NeedMoreInfo()
    {
        var bug = Fixing();
        bug.Fire(BugTrigger.RequestMoreInfo);
        Assert.AreEqual(BugState.NeedMoreInfo, bug.State);
    }

    [TestMethod]
    public void Describe_returns_Vozvrat_for_Returned()
    {
        Assert.AreEqual("ВОЗВРАТ", Bug.Describe(BugState.Returned));
    }

    [TestMethod]
    public void Returned_invalid_StartFixing_throws_InvalidOperationException()
    {
        var bug = Fixing();
        bug.Fire(BugTrigger.ReturnAfterFailedFix);
        Assert.AreEqual(BugState.Returned, bug.State);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.StartFixing));
    }

    [TestMethod]
    public void DeferNoTime_can_return_to_triage_or_start_fixing()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.DeferNoTime);
        Assert.AreEqual(BugState.DeferredNoTime, bug.State);

        bug.Fire(BugTrigger.ReturnToTriage);
        Assert.AreEqual(BugState.DefectTriage, bug.State);

        bug.Fire(BugTrigger.DeferNoTime);
        bug.Fire(BugTrigger.StartFixing);
        Assert.AreEqual(BugState.Fixing, bug.State);
    }

    [TestMethod]
    public void NeedsSeparateDecision_round_trip()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.RequireSeparateDecision);
        Assert.AreEqual(BugState.NeedsSeparateDecision, bug.State);
        bug.Fire(BugTrigger.ReturnToTriage);
        Assert.AreEqual(BugState.DefectTriage, bug.State);
    }

    [TestMethod]
    public void OtherProductProblem_can_start_fixing()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.MarkOtherProduct);
        bug.Fire(BugTrigger.StartFixing);
        Assert.AreEqual(BugState.Fixing, bug.State);
    }

    [TestMethod]
    public void NeedMoreInfo_StartFixing_is_allowed()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.RequestMoreInfo);
        bug.Fire(BugTrigger.StartFixing);
        Assert.AreEqual(BugState.Fixing, bug.State);
    }

    [TestMethod]
    public void Closed_Reopen_then_EnterTriage_returns_to_triage()
    {
        var bug = Fixing();
        bug.Fire(BugTrigger.ProblemSolved);
        bug.Fire(BugTrigger.Reopen);
        Assert.AreEqual(BugState.Reopened, bug.State);
        bug.Fire(BugTrigger.EnterTriageAfterReopen);
        Assert.AreEqual(BugState.DefectTriage, bug.State);
    }

    [TestMethod]
    public void CanFire_is_false_for_disallowed_trigger()
    {
        var bug = new Bug();
        Assert.IsFalse(bug.CanFire(BugTrigger.StartFixing));
    }

    [TestMethod]
    public void Describe_returns_expected_Russian_label_for_triage()
    {
        Assert.AreEqual("РАЗБОР ДЕФЕКТОВ", Bug.Describe(BugState.DefectTriage));
    }

    [TestMethod]
    public void NewDefect_invalid_fire_throws_InvalidOperationException_from_Stateless()
    {
        var bug = new Bug();
        var ex = Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.ProblemSolved));
        StringAssert.Contains(ex.Message, BugTrigger.ProblemSolved.ToString(), StringComparison.Ordinal);
    }

    [TestMethod]
    public void Closed_invalid_fire_throws_InvalidOperationException_from_Stateless()
    {
        var bug = new Bug(BugState.Closed);
        var ex = Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.StartFixing));
        StringAssert.Contains(ex.Message, BugTrigger.StartFixing.ToString(), StringComparison.Ordinal);
    }

    [TestMethod]
    public void Fixing_SubmitToTriage_throws_InvalidOperationException()
    {
        var bug = Fixing();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.SubmitToTriage));
    }

    [TestMethod]
    public void Reopened_StartFixing_throws_until_triage_is_entered()
    {
        var bug = new Bug(BugState.Closed);
        bug.Fire(BugTrigger.Reopen);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.StartFixing));
    }

    [TestMethod]
    public void NotReproducible_Reopen_throws_InvalidOperationException()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.MarkNotReproducible);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Reopen));
    }

    private static Bug Triage()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        return bug;
    }

    private static Bug Fixing()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.StartFixing);
        return bug;
    }
}
