using BugPro;

namespace BugTests;

[TestClass]
public sealed class UnitTest1
{
    [TestMethod]
    public void New_bug_starts_as_draft()
    {
        Assert.AreEqual(BugState.Draft, new Bug().State);
    }

    [TestMethod]
    public void Submit_moves_draft_to_triage()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Submit);
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Assign_moves_triage_to_assigned()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.Assign);
        Assert.AreEqual(BugState.Assigned, bug.State);
    }

    [TestMethod]
    public void Developer_can_start_work_after_assignment()
    {
        var bug = Assigned();
        bug.Fire(BugTrigger.StartWork);
        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void Work_can_be_sent_to_review()
    {
        var bug = InProgress();
        bug.Fire(BugTrigger.SendToReview);
        Assert.AreEqual(BugState.CodeReview, bug.State);
    }

    [TestMethod]
    public void Review_approval_makes_bug_ready_for_test()
    {
        var bug = CodeReview();
        bug.Fire(BugTrigger.ApproveReview);
        Assert.AreEqual(BugState.ReadyForTest, bug.State);
    }

    [TestMethod]
    public void Ready_bug_can_be_sent_to_testing()
    {
        var bug = ReadyForTest();
        bug.Fire(BugTrigger.SendToTesting);
        Assert.AreEqual(BugState.Testing, bug.State);
    }

    [TestMethod]
    public void Passing_tests_resolves_bug()
    {
        var bug = Testing();
        bug.Fire(BugTrigger.PassTesting);
        Assert.AreEqual(BugState.Resolved, bug.State);
    }

    [TestMethod]
    public void Resolved_bug_can_be_closed()
    {
        var bug = Resolved();
        bug.Fire(BugTrigger.Close);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Failed_testing_returns_to_development()
    {
        var bug = Testing();
        bug.Fire(BugTrigger.FailTesting);
        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void Failed_review_returns_to_development()
    {
        var bug = CodeReview();
        bug.Fire(BugTrigger.FailTesting);
        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void Triage_can_request_more_information()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.RequestInfo);
        Assert.AreEqual(BugState.WaitingForInfo, bug.State);
    }

    [TestMethod]
    public void Provided_information_returns_to_triage()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.RequestInfo);
        bug.Fire(BugTrigger.ProvideInfo);
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Assigned_bug_can_request_more_information()
    {
        var bug = Assigned();
        bug.Fire(BugTrigger.RequestInfo);
        Assert.AreEqual(BugState.WaitingForInfo, bug.State);
    }

    [TestMethod]
    public void In_progress_bug_can_request_more_information()
    {
        var bug = InProgress();
        bug.Fire(BugTrigger.RequestInfo);
        Assert.AreEqual(BugState.WaitingForInfo, bug.State);
    }

    [TestMethod]
    public void Triage_can_postpone_bug()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.Defer);
        Assert.AreEqual(BugState.Postponed, bug.State);
    }

    [TestMethod]
    public void Postponed_bug_can_resume_to_triage()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.Defer);
        bug.Fire(BugTrigger.Resume);
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Waiting_bug_can_be_deferred()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.RequestInfo);
        bug.Fire(BugTrigger.Defer);
        Assert.AreEqual(BugState.Postponed, bug.State);
    }

    [TestMethod]
    public void Triage_can_mark_duplicate()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.MarkDuplicate);
        Assert.AreEqual(BugState.Duplicate, bug.State);
    }

    [TestMethod]
    public void Duplicate_can_be_closed()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.MarkDuplicate);
        bug.Fire(BugTrigger.Close);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Triage_can_reject_bug()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.Reject);
        Assert.AreEqual(BugState.Rejected, bug.State);
    }

    [TestMethod]
    public void Rejected_bug_can_be_reopened()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.Reject);
        bug.Fire(BugTrigger.Reopen);
        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void Reopened_bug_submits_to_triage()
    {
        var bug = Resolved();
        bug.Fire(BugTrigger.Reopen);
        bug.Fire(BugTrigger.Submit);
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void CanFire_reports_allowed_trigger()
    {
        Assert.IsTrue(new Bug().CanFire(BugTrigger.Submit));
    }

    [TestMethod]
    public void CanFire_reports_rejected_trigger()
    {
        Assert.IsFalse(new Bug().CanFire(BugTrigger.Assign));
    }

    [TestMethod]
    public void Description_returns_human_text()
    {
        Assert.AreEqual("Testing", Bug.Describe(BugState.Testing));
    }

    [TestMethod]
    public void Invalid_fire_from_draft_throws_stateless_exception()
    {
        var bug = new Bug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Close));
    }

    [TestMethod]
    public void Invalid_fire_from_closed_throws_stateless_exception()
    {
        var bug = Resolved();
        bug.Fire(BugTrigger.Close);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Assign));
    }

    [TestMethod]
    public void Constructor_can_start_from_closed_state()
    {
        var bug = new Bug(BugState.Closed);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    private static Bug Triage()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Submit);
        return bug;
    }

    private static Bug Assigned()
    {
        var bug = Triage();
        bug.Fire(BugTrigger.Assign);
        return bug;
    }

    private static Bug InProgress()
    {
        var bug = Assigned();
        bug.Fire(BugTrigger.StartWork);
        return bug;
    }

    private static Bug CodeReview()
    {
        var bug = InProgress();
        bug.Fire(BugTrigger.SendToReview);
        return bug;
    }

    private static Bug ReadyForTest()
    {
        var bug = CodeReview();
        bug.Fire(BugTrigger.ApproveReview);
        return bug;
    }

    private static Bug Testing()
    {
        var bug = ReadyForTest();
        bug.Fire(BugTrigger.SendToTesting);
        return bug;
    }

    private static Bug Resolved()
    {
        var bug = Testing();
        bug.Fire(BugTrigger.PassTesting);
        return bug;
    }
}
