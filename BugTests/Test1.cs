using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public class UnitTest1
{
    // ── Constructor ──────────────────────────────────────────────────────────

    [TestMethod]
    public void NewBug_HasOpenState()
    {
        var bug = new Bug("Sample bug");
        Assert.AreEqual(BugState.Open, bug.State);
    }

    [TestMethod]
    public void NewBug_StoresTitleCorrectly()
    {
        const string title = "UI glitch on dashboard";
        var bug = new Bug(title);
        Assert.AreEqual(title, bug.Title);
    }

    [TestMethod]
    public void NewBug_EmptyTitle_ThrowsArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => new Bug(""));
    }

    [TestMethod]
    public void NewBug_WhitespaceTitle_ThrowsArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => new Bug("   "));
    }

    [TestMethod]
    public void NewBug_NullTitle_ThrowsArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => new Bug(null!));
    }

    // ── Open state ───────────────────────────────────────────────────────────

    [TestMethod]
    public void OpenState_CanTriggerTriage()
    {
        var bug = new Bug("Bug A");
        Assert.IsTrue(bug.CanFire(BugTrigger.Triage));
    }

    [TestMethod]
    public void OpenState_CannotTriggerClose()
    {
        var bug = new Bug("Bug B");
        Assert.IsFalse(bug.CanFire(BugTrigger.Close));
    }

    [TestMethod]
    public void Triage_FromOpen_MovesToTriaged()
    {
        var bug = new Bug("Bug C");
        bug.Fire(BugTrigger.Triage);
        Assert.AreEqual(BugState.Triaged, bug.State);
    }

    [TestMethod]
    public void Reject_FromOpen_MovesToRejected()
    {
        var bug = new Bug("Bug D");
        bug.Fire(BugTrigger.Reject);
        Assert.AreEqual(BugState.Rejected, bug.State);
    }

    [TestMethod]
    public void MarkDuplicate_FromOpen_MovesToDuplicate()
    {
        var bug = new Bug("Bug E");
        bug.Fire(BugTrigger.MarkDuplicate);
        Assert.AreEqual(BugState.Duplicate, bug.State);
    }

    // ── Triaged state ────────────────────────────────────────────────────────

    [TestMethod]
    public void StartWork_FromTriaged_MovesToInProgress()
    {
        var bug = new Bug("Bug F");
        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.StartWork);
        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void RequestInfo_FromTriaged_MovesToWaitingForInfo()
    {
        var bug = new Bug("Bug G");
        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.RequestInfo);
        Assert.AreEqual(BugState.WaitingForInfo, bug.State);
    }

    [TestMethod]
    public void MarkCannotReproduce_FromTriaged_MovesToCannotReproduce()
    {
        var bug = new Bug("Bug H");
        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.MarkCannotReproduce);
        Assert.AreEqual(BugState.CannotReproduce, bug.State);
    }

    // ── InProgress state ─────────────────────────────────────────────────────

    [TestMethod]
    public void Fix_FromInProgress_MovesToResolved()
    {
        var bug = new Bug("Bug I");
        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.StartWork);
        bug.Fire(BugTrigger.Fix);
        Assert.AreEqual(BugState.Resolved, bug.State);
    }

    [TestMethod]
    public void RequestInfo_FromInProgress_MovesToWaitingForInfo()
    {
        var bug = new Bug("Bug J");
        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.StartWork);
        bug.Fire(BugTrigger.RequestInfo);
        Assert.AreEqual(BugState.WaitingForInfo, bug.State);
    }

    // ── WaitingForInfo state ─────────────────────────────────────────────────

    [TestMethod]
    public void ProvideInfo_FromWaitingForInfo_MovesToTriaged()
    {
        var bug = new Bug("Bug K");
        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.RequestInfo);
        bug.Fire(BugTrigger.ProvideInfo);
        Assert.AreEqual(BugState.Triaged, bug.State);
    }

    // ── Resolved state ───────────────────────────────────────────────────────

    [TestMethod]
    public void Close_FromResolved_MovesToClosed()
    {
        var bug = new Bug("Bug L");
        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.StartWork);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Close);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void SendBack_FromResolved_MovesToReopened()
    {
        var bug = new Bug("Bug M");
        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.StartWork);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.SendBack);
        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    // ── Closed state ─────────────────────────────────────────────────────────

    [TestMethod]
    public void Reopen_FromClosed_MovesToReopened()
    {
        var bug = new Bug("Bug N");
        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.StartWork);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Close);
        bug.Fire(BugTrigger.Reopen);
        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    // ── Reopened state ───────────────────────────────────────────────────────

    [TestMethod]
    public void Triage_FromReopened_MovesToTriaged()
    {
        var bug = new Bug("Bug O");
        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.StartWork);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Close);
        bug.Fire(BugTrigger.Reopen);
        bug.Fire(BugTrigger.Triage);
        Assert.AreEqual(BugState.Triaged, bug.State);
    }

    // ── Invalid transitions → Stateless exceptions ───────────────────────────

    [TestMethod]
    public void Close_FromOpen_ThrowsInvalidOperationException()
    {
        var bug = new Bug("Bug P");
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Close));
    }

    [TestMethod]
    public void Fix_FromTriaged_ThrowsInvalidOperationException()
    {
        var bug = new Bug("Bug Q");
        bug.Fire(BugTrigger.Triage);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Fix));
    }

    [TestMethod]
    public void StartWork_FromRejected_ThrowsInvalidOperationException()
    {
        var bug = new Bug("Bug R");
        bug.Fire(BugTrigger.Reject);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.StartWork));
    }

    [TestMethod]
    public void Reopen_FromDuplicate_ThrowsInvalidOperationException()
    {
        var bug = new Bug("Bug S");
        bug.Fire(BugTrigger.MarkDuplicate);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Reopen));
    }

    [TestMethod]
    public void Close_FromCannotReproduce_ThrowsInvalidOperationException()
    {
        var bug = new Bug("Bug T");
        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.MarkCannotReproduce);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Close));
    }

    [TestMethod]
    public void Triage_FromInProgress_ThrowsInvalidOperationException()
    {
        var bug = new Bug("Bug U");
        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.StartWork);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Triage));
    }

    // ── History tracking ─────────────────────────────────────────────────────

    [TestMethod]
    public void History_IsEmptyBeforeAnyTransition()
    {
        var bug = new Bug("Bug V");
        // OnEntry fires for initial state too — history should already have one entry
        Assert.IsNotNull(bug.History);
    }

    [TestMethod]
    public void History_GrowsWithEachTransition()
    {
        var bug = new Bug("Bug W");
        int before = bug.History.Count;
        bug.Fire(BugTrigger.Triage);
        Assert.IsTrue(bug.History.Count > before);
    }

    // ── Full workflow ─────────────────────────────────────────────────────────

    [TestMethod]
    public void FullResolutionWorkflow_EndStateIsClosed()
    {
        var bug = new Bug("Complete workflow bug");
        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.StartWork);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Close);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void WorkflowWithInfoRequest_EndStateIsClosed()
    {
        var bug = new Bug("Needs info bug");
        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.RequestInfo);
        bug.Fire(BugTrigger.ProvideInfo);
        bug.Fire(BugTrigger.StartWork);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Close);
        Assert.AreEqual(BugState.Closed, bug.State);
    }
}
