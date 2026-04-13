using Stateless;

namespace BugTests;

[TestClass]
public class UnitTest1
{
    // 1. Начальное состояние — Open
    [TestMethod]
    public void NewBug_InitialStateIsOpen()
    {
        var bug = new Bug("Test bug");
        Assert.AreEqual(State.Open, bug.CurrentState);
    }

    // 2. Open -> Triage после Assign
    [TestMethod]
    public void Assign_FromOpen_MovesToTriage()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        Assert.AreEqual(State.Triage, bug.CurrentState);
    }

    // 3. Triage -> Triage при NeedMoreInfo (self-transition)
    [TestMethod]
    public void NeedMoreInfo_FromTriage_StaysInTriage()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.NeedMoreInfo();
        Assert.AreEqual(State.Triage, bug.CurrentState);
    }

    // 4. NeedMoreInfo несколько раз подряд
    [TestMethod]
    public void NeedMoreInfo_MultipleTimes_StaysInTriage()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.NeedMoreInfo();
        bug.NeedMoreInfo();
        bug.NeedMoreInfo();
        Assert.AreEqual(State.Triage, bug.CurrentState);
    }

    // 5. Triage -> InProgress после StartFix
    [TestMethod]
    public void StartFix_FromTriage_MovesToInProgress()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        Assert.AreEqual(State.InProgress, bug.CurrentState);
    }

    // 6. InProgress -> Triage при NotABug
    [TestMethod]
    public void NotABug_FromInProgress_MovesToTriage()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        bug.NotABug();
        Assert.AreEqual(State.Triage, bug.CurrentState);
    }

    // 7. InProgress -> Triage при NotReproducible
    [TestMethod]
    public void NotReproducible_FromInProgress_MovesToTriage()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        bug.NotReproducible();
        Assert.AreEqual(State.Triage, bug.CurrentState);
    }

    // 8. InProgress -> Triage при Duplicate
    [TestMethod]
    public void Duplicate_FromInProgress_MovesToTriage()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        bug.Duplicate();
        Assert.AreEqual(State.Triage, bug.CurrentState);
    }

    // 9. InProgress -> Triage при WontFix
    [TestMethod]
    public void WontFix_FromInProgress_MovesToTriage()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        bug.WontFix();
        Assert.AreEqual(State.Triage, bug.CurrentState);
    }

    // 10. InProgress -> Resolved при MarkResolved
    [TestMethod]
    public void MarkResolved_FromInProgress_MovesToResolved()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        bug.MarkResolved();
        Assert.AreEqual(State.Resolved, bug.CurrentState);
    }

    // 11. Resolved -> Closed при Approve
    [TestMethod]
    public void Approve_FromResolved_MovesToClosed()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        bug.MarkResolved();
        bug.Approve();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    // 12. Resolved -> Triage при Return (проверка не прошла)
    [TestMethod]
    public void Return_FromResolved_MovesToTriage()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        bug.MarkResolved();
        bug.Return();
        Assert.AreEqual(State.Triage, bug.CurrentState);
    }

    // 13. Closed -> Reopened при Reopen
    [TestMethod]
    public void Reopen_FromClosed_MovesToReopened()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        bug.MarkResolved();
        bug.Approve();
        bug.Reopen();
        Assert.AreEqual(State.Reopened, bug.CurrentState);
    }

    // 14. Reopened -> InProgress при AssignForFix
    [TestMethod]
    public void AssignForFix_FromReopened_MovesToInProgress()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        bug.MarkResolved();
        bug.Approve();
        bug.Reopen();
        bug.AssignForFix();
        Assert.AreEqual(State.InProgress, bug.CurrentState);
    }

    // 15. Полный путь: Open->Triage->InProgress->Resolved->Closed
    [TestMethod]
    public void HappyPath_OpenToClosedWithoutIssues()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        bug.MarkResolved();
        bug.Approve();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    // 16. Полный путь повторного открытия: Closed->Reopened->InProgress->Resolved->Closed
    [TestMethod]
    public void ReopenCycle_ClosedToReopenedToClosedAgain()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        bug.MarkResolved();
        bug.Approve();
        bug.Reopen();
        bug.AssignForFix();
        bug.MarkResolved();
        bug.Approve();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    // 17. Исключение: Fire недопустимого триггера из Open (Stateless бросает InvalidOperationException)
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void FireInvalidTrigger_FromOpen_ThrowsException()
    {
        var bug = new Bug("Test bug");
        bug.StartFix(); // нельзя из Open
    }

    // 18. Исключение: Assign из Triage (уже назначен)
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Assign_FromTriage_ThrowsException()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.Assign(); // повторный Assign недопустим
    }

    // 19. Исключение: MarkResolved из Open
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void MarkResolved_FromOpen_ThrowsException()
    {
        var bug = new Bug("Test bug");
        bug.MarkResolved();
    }

    // 20. Исключение: Approve из InProgress (минуя Resolved)
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Approve_FromInProgress_ThrowsException()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        bug.Approve(); // нельзя, не помечен как Resolved
    }

    // 21. Исключение: Reopen из InProgress (не Closed)
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Reopen_FromInProgress_ThrowsException()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        bug.Reopen();
    }

    // 22. Исключение: Return из InProgress (не Resolved)
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Return_FromInProgress_ThrowsException()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        bug.Return();
    }

    // 23. CanFire возвращает true для допустимого триггера
    [TestMethod]
    public void CanFire_ValidTrigger_ReturnsTrue()
    {
        var bug = new Bug("Test bug");
        Assert.IsTrue(bug.CanFire(Trigger.Assign));
    }

    // 24. CanFire возвращает false для недопустимого триггера
    [TestMethod]
    public void CanFire_InvalidTrigger_ReturnsFalse()
    {
        var bug = new Bug("Test bug");
        Assert.IsFalse(bug.CanFire(Trigger.StartFix));
    }

    // 25. Title сохраняется корректно
    [TestMethod]
    public void BugTitle_IsStoredCorrectly()
    {
        var bug = new Bug("Some important bug");
        Assert.AreEqual("Some important bug", bug.Title);
    }

    // 26. Повторный цикл: после возврата из Resolved можно снова дойти до Closed
    [TestMethod]
    public void ReturnFromResolved_ThenResolveAndApprove_Succeeds()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        bug.MarkResolved();
        bug.Return();
        // вернулись в Triage, начинаем заново
        bug.StartFix();
        bug.MarkResolved();
        bug.Approve();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    // 27. Из Triage после NotABug возврата можно снова уйти в InProgress
    [TestMethod]
    public void AfterNotABug_CanStartFixAgain()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        bug.NotABug();
        bug.StartFix();
        Assert.AreEqual(State.InProgress, bug.CurrentState);
    }

    // 28. AssignForFix недоступен из Closed (нужно сначала Reopen)
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AssignForFix_FromClosed_ThrowsException()
    {
        var bug = new Bug("Test bug");
        bug.Assign();
        bug.StartFix();
        bug.MarkResolved();
        bug.Approve();
        bug.AssignForFix(); // Closed -> нужен Reopen перед AssignForFix
    }
}
