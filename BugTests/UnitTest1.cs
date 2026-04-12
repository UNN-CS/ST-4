using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public sealed class BugStateMachineTests
{
    private static Bug NewBugInAnalysis()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitForTriage);
        return bug;
    }

    [TestMethod]
    public void Initial_state_is_New()
    {
        var bug = new Bug();
        Assert.AreEqual(BugState.New, bug.State);
    }

    [TestMethod]
    public void SubmitForTriage_moves_to_DefectAnalysis()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitForTriage);
        Assert.AreEqual(BugState.DefectAnalysis, bug.State);
    }

    [TestMethod]
    public void Fire_invalid_trigger_from_New_throws_InvalidOperationException()
    {
        var bug = new Bug();
        var ex = Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.AssignToDeveloper));
        Assert.IsFalse(string.IsNullOrEmpty(ex.Message));
    }

    [TestMethod]
    public void Fire_Defer_from_New_throws_InvalidOperationException()
    {
        var bug = new Bug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Defer));
    }

    [TestMethod]
    public void AssignToDeveloper_from_analysis_goes_to_Fixing()
    {
        var bug = NewBugInAnalysis();
        bug.Fire(BugTrigger.AssignToDeveloper);
        Assert.AreEqual(BugState.Fixing, bug.State);
    }

    [TestMethod]
    public void ConfirmProblemSolved_from_Fixing_closes_bug()
    {
        var bug = NewBugInAnalysis();
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.ConfirmProblemSolved);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void ReportProblemUnsolved_from_Fixing_returns_to_analysis()
    {
        var bug = NewBugInAnalysis();
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.ReportProblemUnsolved);
        Assert.AreEqual(BugState.DefectAnalysis, bug.State);
    }

    [TestMethod]
    public void Cannot_ConfirmProblemSolved_from_DefectAnalysis_throws()
    {
        var bug = NewBugInAnalysis();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.ConfirmProblemSolved));
    }

    [TestMethod]
    public void Defer_then_ResumeAnalysis_restores_analysis()
    {
        var bug = NewBugInAnalysis();
        bug.Fire(BugTrigger.Defer);
        Assert.AreEqual(BugState.Deferred, bug.State);
        bug.Fire(BugTrigger.ResumeAnalysis);
        Assert.AreEqual(BugState.DefectAnalysis, bug.State);
    }

    [TestMethod]
    public void Fixing_can_Defer_and_resume()
    {
        var bug = NewBugInAnalysis();
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.Defer);
        Assert.AreEqual(BugState.Deferred, bug.State);
        bug.Fire(BugTrigger.ResumeAnalysis);
        Assert.AreEqual(BugState.DefectAnalysis, bug.State);
    }

    [TestMethod]
    public void RequestMoreInfo_then_InformationProvided_returns_to_analysis()
    {
        var bug = NewBugInAnalysis();
        bug.Fire(BugTrigger.RequestMoreInfo);
        Assert.AreEqual(BugState.NeedMoreInfo, bug.State);
        bug.Fire(BugTrigger.InformationProvided);
        Assert.AreEqual(BugState.DefectAnalysis, bug.State);
    }

    [TestMethod]
    public void MarkNotDefect_TesterApprovesClosure_goes_to_Closed()
    {
        var bug = NewBugInAnalysis();
        bug.Fire(BugTrigger.MarkNotDefect);
        Assert.AreEqual(BugState.PendingClosureNotDefect, bug.State);
        bug.Fire(BugTrigger.TesterApprovesClosure);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void MarkWontFix_TesterRequestsReturn_goes_to_analysis()
    {
        var bug = NewBugInAnalysis();
        bug.Fire(BugTrigger.MarkWontFix);
        bug.Fire(BugTrigger.TesterRequestsReturn);
        Assert.AreEqual(BugState.DefectAnalysis, bug.State);
    }

    [TestMethod]
    public void MarkDuplicate_TesterApprovesClosure_goes_to_Closed()
    {
        var bug = NewBugInAnalysis();
        bug.Fire(BugTrigger.MarkDuplicate);
        bug.Fire(BugTrigger.TesterApprovesClosure);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void MarkNotReproducible_from_analysis_pending_then_close()
    {
        var bug = NewBugInAnalysis();
        bug.Fire(BugTrigger.MarkNotReproducible);
        Assert.AreEqual(BugState.PendingClosureNotReproducible, bug.State);
        bug.Fire(BugTrigger.TesterApprovesClosure);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void ReportNotReproducibleFromFix_moves_to_pending_closure()
    {
        var bug = NewBugInAnalysis();
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.ReportNotReproducibleFromFix);
        Assert.AreEqual(BugState.PendingClosureNotReproducible, bug.State);
    }

    [TestMethod]
    public void ReopenClosed_from_Closed_automatically_returns_to_DefectAnalysis()
    {
        var bug = NewBugInAnalysis();
        bug.Fire(BugTrigger.MarkNotDefect);
        bug.Fire(BugTrigger.TesterApprovesClosure);
        Assert.AreEqual(BugState.Closed, bug.State);
        bug.Fire(BugTrigger.ReopenClosed);
        Assert.AreEqual(BugState.DefectAnalysis, bug.State);
    }

    [TestMethod]
    public void Cannot_ReopenClosed_from_non_Closed_throws()
    {
        var bug = NewBugInAnalysis();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.ReopenClosed));
    }

    [TestMethod]
    public void EscalateBlocked_then_Unblock_returns_to_analysis()
    {
        var bug = NewBugInAnalysis();
        bug.Fire(BugTrigger.EscalateBlocked);
        Assert.AreEqual(BugState.Blocked, bug.State);
        bug.Fire(BugTrigger.Unblock);
        Assert.AreEqual(BugState.DefectAnalysis, bug.State);
    }

    [TestMethod]
    public void Cannot_Unblock_from_DefectAnalysis_throws()
    {
        var bug = NewBugInAnalysis();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Unblock));
    }

    [TestMethod]
    public void TesterApprovesClosure_from_Fixing_throws()
    {
        var bug = NewBugInAnalysis();
        bug.Fire(BugTrigger.AssignToDeveloper);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.TesterApprovesClosure));
    }

    [TestMethod]
    public void CanFire_SubmitForTriage_only_in_New()
    {
        var bug = new Bug();
        Assert.IsTrue(bug.CanFire(BugTrigger.SubmitForTriage));
        bug.Fire(BugTrigger.SubmitForTriage);
        Assert.IsFalse(bug.CanFire(BugTrigger.SubmitForTriage));
    }

    [TestMethod]
    public void PermittedTriggers_in_New_contains_SubmitForTriage()
    {
        var bug = new Bug();
        CollectionAssert.Contains(bug.PermittedTriggers.ToList(), BugTrigger.SubmitForTriage);
    }

    [TestMethod]
    public void DescribeWorkflow_is_non_empty_after_configuration()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitForTriage);
        Assert.IsFalse(string.IsNullOrWhiteSpace(bug.DescribeWorkflow()));
    }

    [TestMethod]
    public void InformationProvided_from_DefectAnalysis_throws()
    {
        var bug = NewBugInAnalysis();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.InformationProvided));
    }

    [TestMethod]
    public void Full_cycle_fix_reject_reopen_close()
    {
        var bug = NewBugInAnalysis();
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.ReportProblemUnsolved);
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.ConfirmProblemSolved);
        Assert.AreEqual(BugState.Closed, bug.State);
        bug.Fire(BugTrigger.ReopenClosed);
        bug.Fire(BugTrigger.MarkDuplicate);
        bug.Fire(BugTrigger.TesterApprovesClosure);
        Assert.AreEqual(BugState.Closed, bug.State);
    }
}
