using BugPro;

namespace BugTests;

[TestClass]
public sealed class UnitTest1
{
    [TestMethod]
    public void Constructor_SetsInitialStateToNew()
    {
        var bug = CreateBug();

        Assert.AreEqual(BugState.New, bug.State);
    }

    [TestMethod]
    public void NewBug_OnlyAllowsStartTriage()
    {
        var bug = CreateBug();

        CollectionAssert.AreEquivalent(
            new[] { BugTrigger.StartTriage },
            bug.PermittedTriggers.ToArray());
    }

    [TestMethod]
    public void StartTriage_MovesBugToTriage()
    {
        var bug = CreateBug();

        bug.StartTriage();

        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void RequestMoreInfo_MovesBugToNeedMoreInfo()
    {
        var bug = CreateBugAtTriage();

        bug.RequestMoreInfo("Нужны дополнительные шаги.");

        Assert.AreEqual(BugState.NeedMoreInfo, bug.State);
    }

    [TestMethod]
    public void ProvideMoreInfo_ReturnsBugToTriage()
    {
        var bug = CreateBugAtTriage();
        bug.RequestMoreInfo("Нужны дополнительные шаги.");

        bug.ProvideMoreInfo("Добавлены шаги воспроизведения.");

        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void StartFixing_MovesBugToFixing()
    {
        var bug = CreateBugAtTriage();

        bug.StartFixing("Подтвержденный дефект.");

        Assert.AreEqual(BugState.Fixing, bug.State);
    }

    [TestMethod]
    public void MarkAsNotABug_MovesBugToNotABug()
    {
        var bug = CreateBugAtTriage();

        bug.MarkAsNotABug("Это ожидаемое поведение.");

        Assert.AreEqual(BugState.NotABug, bug.State);
    }

    [TestMethod]
    public void MarkAsWontFix_MovesBugToWontFix()
    {
        var bug = CreateBugAtTriage();

        bug.MarkAsWontFix("Исправление не требуется в этой версии.");

        Assert.AreEqual(BugState.WontFix, bug.State);
    }

    [TestMethod]
    public void MarkAsDuplicate_MovesBugToDuplicate()
    {
        var bug = CreateBugAtTriage();

        bug.MarkAsDuplicate("Дубликат BUG-12.");

        Assert.AreEqual(BugState.Duplicate, bug.State);
    }

    [TestMethod]
    public void MarkAsNotReproducible_MovesBugToNotReproducible()
    {
        var bug = CreateBugAtTriage();

        bug.MarkAsNotReproducible("На текущей сборке не воспроизводится.");

        Assert.AreEqual(BugState.NotReproducible, bug.State);
    }

    [TestMethod]
    public void NotABug_CanReturnToTriage()
    {
        var bug = CreateBugAtTriage();
        bug.MarkAsNotABug("Это ожидаемое поведение.");

        bug.ReturnToTriage("Поступили новые требования.");

        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void WontFix_CanReturnToTriage()
    {
        var bug = CreateBugAtTriage();
        bug.MarkAsWontFix("Откладываем решение.");

        bug.ReturnToTriage("Нужно повторно оценить приоритет.");

        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Duplicate_CanReturnToTriage()
    {
        var bug = CreateBugAtTriage();
        bug.MarkAsDuplicate("Совпадает с уже известным багом.");

        bug.ReturnToTriage("Оказалось, что это отдельный дефект.");

        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void NotReproducible_CanReturnToTriage()
    {
        var bug = CreateBugAtTriage();
        bug.MarkAsNotReproducible("Не удалось повторить проблему.");

        bug.ReturnToTriage("Появились свежие логи.");

        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void ReturnFromFix_MovesBugToReturnedAndStoresReason()
    {
        var bug = CreateBugAtFixing();

        bug.ReturnFromFix(ReturnReason.NoTimeRightNow, "Нет времени сейчас.");

        Assert.AreEqual(BugState.Returned, bug.State);
        Assert.AreEqual(ReturnReason.NoTimeRightNow, bug.LastReturnReason);
    }

    [TestMethod]
    public void Returned_CanReturnToTriage()
    {
        var bug = CreateBugAtFixing();
        bug.ReturnFromFix(ReturnReason.AnotherProductIssue, "Проблема другого продукта.");

        bug.ReturnToTriage("Нужно отдельное решение.");

        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void MarkFixed_MovesBugToReadyForVerification()
    {
        var bug = CreateBugAtFixing();

        bug.MarkFixed();

        Assert.AreEqual(BugState.ReadyForVerification, bug.State);
    }

    [TestMethod]
    public void AcceptFix_ClosesBug()
    {
        var bug = CreateBugReadyForVerification();

        bug.AcceptFix();

        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void RejectFix_ReturnsBugToReturned()
    {
        var bug = CreateBugReadyForVerification();

        bug.RejectFix("Исправление не сработало.");

        Assert.AreEqual(BugState.Returned, bug.State);
        Assert.AreEqual(ReturnReason.FailedVerification, bug.LastReturnReason);
    }

    [TestMethod]
    public void ClosedBug_CanBeReopened()
    {
        var bug = CreateClosedBug();

        bug.Reopen("Проблема снова воспроизвелась.");

        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void ReopenedBug_CanReturnToTriage()
    {
        var bug = CreateClosedBug();
        bug.Reopen("Проблема снова воспроизвелась.");

        bug.ReturnToTriage("Нужен повторный разбор.");

        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void InvalidTransitionFromNew_ThrowsInvalidOperationException()
    {
        var bug = CreateBug();

        Assert.ThrowsException<InvalidOperationException>(() => bug.MarkFixed());
    }

    [TestMethod]
    public void InvalidTransitionFromNeedMoreInfo_ThrowsInvalidOperationException()
    {
        var bug = CreateBugAtTriage();
        bug.RequestMoreInfo("Нужны логи.");

        Assert.ThrowsException<InvalidOperationException>(() => bug.AcceptFix());
    }

    [TestMethod]
    public void InvalidTransitionFromClosed_ThrowsInvalidOperationException()
    {
        var bug = CreateClosedBug();

        Assert.ThrowsException<InvalidOperationException>(() => bug.AcceptFix());
    }

    [TestMethod]
    public void Timeline_ContainsExecutedTransitions()
    {
        var bug = CreateBug();
        bug.StartTriage();
        bug.StartFixing("Подтверждено.");

        Assert.AreEqual(3, bug.Timeline.Count);
        StringAssert.Contains(bug.Timeline[1], "StartTriage");
        StringAssert.Contains(bug.Timeline[2], "StartFixing");
    }

    [TestMethod]
    public void Metadata_TracksLastActorAndComment()
    {
        var bug = CreateBugAtTriage();

        bug.MarkAsDuplicate("Дубликат BUG-12.", actor: "Продуктовая команда");

        Assert.AreEqual("Продуктовая команда", bug.LastActor);
        Assert.AreEqual("Дубликат BUG-12.", bug.LastActionComment);
        Assert.AreEqual(TriageDecision.Duplicate, bug.LastTriageDecision);
    }

    [TestMethod]
    public void PermittedTriggersInTriage_AreExpected()
    {
        var bug = CreateBugAtTriage();

        CollectionAssert.AreEquivalent(
            new[]
            {
                BugTrigger.RequestMoreInfo,
                BugTrigger.StartFixing,
                BugTrigger.MarkAsNotABug,
                BugTrigger.MarkAsWontFix,
                BugTrigger.MarkAsDuplicate,
                BugTrigger.MarkAsNotReproducible
            },
            bug.PermittedTriggers.ToArray());
    }

    private static Bug CreateBug() => new("BUG-1", "Test bug");

    private static Bug CreateBugAtTriage()
    {
        var bug = CreateBug();
        bug.StartTriage();
        return bug;
    }

    private static Bug CreateBugAtFixing()
    {
        var bug = CreateBugAtTriage();
        bug.StartFixing("Дефект подтвержден.");
        return bug;
    }

    private static Bug CreateBugReadyForVerification()
    {
        var bug = CreateBugAtFixing();
        bug.MarkFixed();
        return bug;
    }

    private static Bug CreateClosedBug()
    {
        var bug = CreateBugReadyForVerification();
        bug.AcceptFix();
        return bug;
    }
}
