namespace BugTests;

[TestClass]
public class DefectWorkflowTests
{
    private static Bug CreateTicketInTriage()
    {
        var ticket = new Bug();
        ticket.Fire(BugTrigger.Report);
        return ticket;
    }

    private static Bug CreateTicketInFixing()
    {
        var ticket = CreateTicketInTriage();
        ticket.Fire(BugTrigger.AssignToDeveloper);
        return ticket;
    }

    private static Bug CreateTicketInClosing()
    {
        var ticket = CreateTicketInFixing();
        ticket.Fire(BugTrigger.SubmitFixResolved);
        ticket.Fire(BugTrigger.ApproveReview);
        return ticket;
    }

    private static Bug CreateArchivedTicket()
    {
        var ticket = CreateTicketInClosing();
        ticket.Fire(BugTrigger.FinalClose);
        return ticket;
    }

    [TestMethod]
    public void Test01_FreshTicket_StartsAsNewDefect()
    {
        var ticket = new Bug();
        Assert.AreEqual(BugState.NewDefect, ticket.State);
    }

    [TestMethod]
    public void Test02_RegisterTicket_EntersTriageQueue()
    {
        var ticket = new Bug();
        ticket.Fire(BugTrigger.Report);
        Assert.AreEqual(BugState.Triage, ticket.State);
    }

    [TestMethod]
    public void Test03_Triage_AssignDeveloper_SwitchesToFixing()
    {
        var ticket = CreateTicketInTriage();
        ticket.Fire(BugTrigger.AssignToDeveloper);
        Assert.AreEqual(BugState.Fixing, ticket.State);
    }

    [TestMethod]
    public void Test04_Triage_RejectAsInvalid_BecomesNotABug()
    {
        var ticket = CreateTicketInTriage();
        ticket.Fire(BugTrigger.MarkNotABug);
        Assert.AreEqual(BugState.NotABug, ticket.State);
    }

    [TestMethod]
    public void Test05_Triage_DeclineFix_BecomesWontFix()
    {
        var ticket = CreateTicketInTriage();
        ticket.Fire(BugTrigger.MarkWontFix);
        Assert.AreEqual(BugState.WontFix, ticket.State);
    }

    [TestMethod]
    public void Test06_Triage_LinkDuplicate_BecomesDuplicate()
    {
        var ticket = CreateTicketInTriage();
        ticket.Fire(BugTrigger.MarkDuplicate);
        Assert.AreEqual(BugState.Duplicate, ticket.State);
    }

    [TestMethod]
    public void Test07_Fixing_NoReproCase_MovesToNotReproducible()
    {
        var ticket = CreateTicketInFixing();
        ticket.Fire(BugTrigger.CannotReproduce);
        Assert.AreEqual(BugState.NotReproducible, ticket.State);
    }

    [TestMethod]
    public void Test08_Fixing_ResolvedPatch_GoesToInReview()
    {
        var ticket = CreateTicketInFixing();
        ticket.Fire(BugTrigger.SubmitFixResolved);
        Assert.AreEqual(BugState.InReview, ticket.State);
    }

    [TestMethod]
    public void Test09_InReview_QAApproved_ReachesClosing()
    {
        var ticket = CreateTicketInFixing();
        ticket.Fire(BugTrigger.SubmitFixResolved);
        ticket.Fire(BugTrigger.ApproveReview);
        Assert.AreEqual(BugState.Closing, ticket.State);
    }

    [TestMethod]
    public void Test10_InReview_QARejected_ReturnsToTriage()
    {
        var ticket = CreateTicketInFixing();
        ticket.Fire(BugTrigger.SubmitFixResolved);
        ticket.Fire(BugTrigger.RejectReview);
        Assert.AreEqual(BugState.Triage, ticket.State);
    }

    [TestMethod]
    public void Test11_Closing_ConfirmArchive_StateIsClosed()
    {
        var ticket = CreateTicketInClosing();
        ticket.Fire(BugTrigger.FinalClose);
        Assert.AreEqual(BugState.Closed, ticket.State);
    }

    [TestMethod]
    public void Test12_Closed_ReactivateTicket_EntersReopening()
    {
        var ticket = CreateArchivedTicket();
        ticket.Fire(BugTrigger.Reopen);
        Assert.AreEqual(BugState.Reopening, ticket.State);
    }

    [TestMethod]
    public void Test13_Reopening_FinishCycle_ReturnsToTriage()
    {
        var ticket = CreateArchivedTicket();
        ticket.Fire(BugTrigger.Reopen);
        ticket.Fire(BugTrigger.CompleteReopen);
        Assert.AreEqual(BugState.Triage, ticket.State);
    }

    [TestMethod]
    public void Test14_NotReproducible_AcceptClosure_GoesToClosing()
    {
        var ticket = CreateTicketInFixing();
        ticket.Fire(BugTrigger.CannotReproduce);
        ticket.Fire(BugTrigger.ConfirmNotReproOk);
        Assert.AreEqual(BugState.Closing, ticket.State);
    }

    [TestMethod]
    public void Test15_NotReproducible_Dispute_ReturnsToTriage()
    {
        var ticket = CreateTicketInFixing();
        ticket.Fire(BugTrigger.CannotReproduce);
        ticket.Fire(BugTrigger.ConfirmNotReproReject);
        Assert.AreEqual(BugState.Triage, ticket.State);
    }

    [TestMethod]
    public void Test16_Fixing_FailedFix_ReturnsToTriage()
    {
        var ticket = CreateTicketInFixing();
        ticket.Fire(BugTrigger.SubmitFixNotResolved);
        Assert.AreEqual(BugState.Triage, ticket.State);
    }

    [TestMethod]
    public void Test17_Fixing_DeferNoCapacity_ReturnsToTriage()
    {
        var ticket = CreateTicketInFixing();
        ticket.Fire(BugTrigger.DeferNoTime);
        Assert.AreEqual(BugState.Triage, ticket.State);
    }

    [TestMethod]
    public void Test18_Fixing_DeferNeedInfo_ReturnsToTriage()
    {
        var ticket = CreateTicketInFixing();
        ticket.Fire(BugTrigger.DeferNeedInfo);
        Assert.AreEqual(BugState.Triage, ticket.State);
    }

    [TestMethod]
    public void Test19_Fixing_DeferSeparateSolution_ReturnsToTriage()
    {
        var ticket = CreateTicketInFixing();
        ticket.Fire(BugTrigger.DeferSeparateSolution);
        Assert.AreEqual(BugState.Triage, ticket.State);
    }

    [TestMethod]
    public void Test20_Fixing_DeferOtherProduct_ReturnsToTriage()
    {
        var ticket = CreateTicketInFixing();
        ticket.Fire(BugTrigger.DeferOtherProduct);
        Assert.AreEqual(BugState.Triage, ticket.State);
    }

    [TestMethod]
    public void Test21_Lifecycle_FullHappyPath_EndsClosed()
    {
        var ticket = CreateArchivedTicket();
        Assert.AreEqual(BugState.Closed, ticket.State);
    }

    [TestMethod]
    public void Test22_Illegal_NewDefect_AssignWithoutReport_Throws()
    {
        var ticket = new Bug();
        Assert.ThrowsException<InvalidOperationException>(
            () => ticket.Fire(BugTrigger.AssignToDeveloper));
    }

    [TestMethod]
    public void Test23_Illegal_Closed_ReportAgain_Throws()
    {
        var ticket = CreateArchivedTicket();
        Assert.ThrowsException<InvalidOperationException>(
            () => ticket.Fire(BugTrigger.Report));
    }

    [TestMethod]
    public void Test24_Illegal_NotABug_AnyTransition_Throws()
    {
        var ticket = CreateTicketInTriage();
        ticket.Fire(BugTrigger.MarkNotABug);
        Assert.ThrowsException<InvalidOperationException>(
            () => ticket.Fire(BugTrigger.AssignToDeveloper));
    }

    [TestMethod]
    public void Test25_Illegal_Fixing_ApproveWithoutReview_Throws()
    {
        var ticket = CreateTicketInFixing();
        Assert.ThrowsException<InvalidOperationException>(
            () => ticket.Fire(BugTrigger.ApproveReview));
    }

    [TestMethod]
    public void Test26_Illegal_Triage_SubmitFixFromQueue_Throws()
    {
        var ticket = CreateTicketInTriage();
        Assert.ThrowsException<InvalidOperationException>(
            () => ticket.Fire(BugTrigger.SubmitFixResolved));
    }

    [TestMethod]
    public void Test27_Illegal_WontFix_ReopenFromTerminal_Throws()
    {
        var ticket = CreateTicketInTriage();
        ticket.Fire(BugTrigger.MarkWontFix);
        Assert.ThrowsException<InvalidOperationException>(
            () => ticket.Fire(BugTrigger.Reopen));
    }

    [TestMethod]
    public void Test28_Illegal_Reopening_SkipToFinalClose_Throws()
    {
        var ticket = CreateArchivedTicket();
        ticket.Fire(BugTrigger.Reopen);
        Assert.ThrowsException<InvalidOperationException>(
            () => ticket.Fire(BugTrigger.FinalClose));
    }

    [TestMethod]
    public void Test29_CanFire_BlockedAction_ReturnsFalse()
    {
        var ticket = new Bug();
        Assert.IsFalse(ticket.CanFire(BugTrigger.FinalClose));
        ticket.Fire(BugTrigger.Report);
        Assert.IsFalse(ticket.CanFire(BugTrigger.CannotReproduce));
    }

    [TestMethod]
    public void Test30_CanFire_PermittedAction_ReturnsTrue()
    {
        var ticket = CreateTicketInTriage();
        Assert.IsTrue(ticket.CanFire(BugTrigger.AssignToDeveloper));
        Assert.IsTrue(ticket.CanFire(BugTrigger.MarkDuplicate));
        Assert.IsFalse(ticket.CanFire(BugTrigger.FinalClose));
    }
}
