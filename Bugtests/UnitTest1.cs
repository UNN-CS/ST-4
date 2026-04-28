using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BugTests;

[TestClass]
public class BugStateMachineTests
{
    private Bug _bug;

    [TestInitialize]
    public void Setup()
    {
        _bug = new Bug("Тестовый баг", "Тестовое описание");
    }

    // 1. Тест начального состояния
    [TestMethod]
    public void Bug_InitialState_ShouldBeOpen()
    {
        Assert.AreEqual(BugState.Open, _bug.State);
    }

    // 2. Тест нормального потока работы
    [TestMethod]
    public void Bug_CompleteWorkflow_ShouldEndInClosed()
    {
        _bug.Assign();
        _bug.StartWork();
        _bug.Fix();
        _bug.Verify();
        _bug.Close();
        
        Assert.AreEqual(BugState.Closed, _bug.State);
    }

    // 3. Тест назначения бага
    [TestMethod]
    public void Bug_Assign_ShouldMoveToAssigned()
    {
        _bug.Assign();
        Assert.AreEqual(BugState.Assigned, _bug.State);
    }

    // 4. Тест начала работы
    [TestMethod]
    public void Bug_StartWork_ShouldMoveToInProgress()
    {
        _bug.Assign();
        _bug.StartWork();
        Assert.AreEqual(BugState.InProgress, _bug.State);
    }

    // 5. Тест исправления
    [TestMethod]
    public void Bug_Fix_ShouldMoveToFixed()
    {
        _bug.Assign();
        _bug.StartWork();
        _bug.Fix();
        Assert.AreEqual(BugState.Fixed, _bug.State);
    }

    // 6. Тест верификации
    [TestMethod]
    public void Bug_Verify_ShouldMoveToVerified()
    {
        _bug.Assign();
        _bug.StartWork();
        _bug.Fix();
        _bug.Verify();
        Assert.AreEqual(BugState.Verified, _bug.State);
    }

    // 7. Тест закрытия
    [TestMethod]
    public void Bug_Close_ShouldMoveToClosed()
    {
        _bug.Assign();
        _bug.StartWork();
        _bug.Fix();
        _bug.Verify();
        _bug.Close();
        Assert.AreEqual(BugState.Closed, _bug.State);
    }

    // 8. Тест переоткрытия из Fixed
    [TestMethod]
    public void Bug_ReopenFromFixed_ShouldMoveToReopened()
    {
        _bug.Assign();
        _bug.StartWork();
        _bug.Fix();
        _bug.Reopen();
        Assert.AreEqual(BugState.Reopened, _bug.State);
    }

    // 9. Тест переоткрытия из Verified
    [TestMethod]
    public void Bug_ReopenFromVerified_ShouldMoveToReopened()
    {
        _bug.Assign();
        _bug.StartWork();
        _bug.Fix();
        _bug.Verify();
        _bug.Reopen();
        Assert.AreEqual(BugState.Reopened, _bug.State);
    }

    // 10. Тест отклонения бага
    [TestMethod]
    public void Bug_Reject_ShouldMoveToRejected()
    {
        _bug.Reject();
        Assert.AreEqual(BugState.Rejected, _bug.State);
    }

    // 11. Тест запроса информации
    [TestMethod]
    public void Bug_RequestInfo_ShouldMoveToNeedMoreInfo()
    {
        _bug.RequestInfo();
        Assert.AreEqual(BugState.NeedMoreInfo, _bug.State);
    }

    // 12. Тест предоставления информации
    [TestMethod]
    public void Bug_ProvideInfo_ShouldMoveToOpen()
    {
        _bug.RequestInfo();
        _bug.ProvideInfo();
        Assert.AreEqual(BugState.Open, _bug.State);
    }

    // 13. Тест повторного назначения из Reopened
    [TestMethod]
    public void Bug_AssignFromReopened_ShouldMoveToAssigned()
    {
        _bug.Assign();
        _bug.StartWork();
        _bug.Fix();
        _bug.Reopen();
        _bug.Assign();
        Assert.AreEqual(BugState.Assigned, _bug.State);
    }

    // 14. Тест исключения - нельзя закрыть без проверки
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Bug_CloseFromVerifiedWithoutVerify_ShouldThrowException()
    {
        _bug.Assign();
        _bug.StartWork();
        _bug.Fix();
        // Пропускаем Verify
        _bug.Close(); // Должно выбросить исключение
    }

    // 15. Тест исключения - нельзя верифицировать без исправления
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Bug_VerifyWithoutFix_ShouldThrowException()
    {
        _bug.Assign();
        _bug.StartWork();
        _bug.Verify(); // Должно выбросить исключение
    }

    // 16. Тест исключения - нельзя исправить без начала работы
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Bug_FixWithoutStartWork_ShouldThrowException()
    {
        _bug.Assign();
        _bug.Fix(); // Должно выбросить исключение
    }

    // 17. Тест отклонить после фикса
    [TestMethod]
    public void Bug_RejectFromFixed_ShouldMoveToRejected()
    {
        _bug.Assign();
        _bug.StartWork();
        _bug.Fix();
        _bug.Reject();
        Assert.AreEqual(BugState.Rejected, _bug.State);
    }

    // 18. Тест переоткрытия из Rejected
    [TestMethod]
    public void Bug_ReopenFromRejected_ShouldMoveToReopened()
    {
        _bug.Reject();
        _bug.Reopen();
        Assert.AreEqual(BugState.Reopened, _bug.State);
    }

    // 19. Тест назначения из Rejected
    [TestMethod]
    public void Bug_AssignFromRejected_ShouldMoveToAssigned()
    {
        _bug.Reject();
        _bug.Assign();
        Assert.AreEqual(BugState.Assigned, _bug.State);
    }

    // 20. Тест создания бага с заголовком
    [TestMethod]
    public void Bug_Constructor_ShouldSetTitleAndDescription()
    {
        var title = "Тестовый заголовок";
        var description = "Тестовое описание";
        var bug = new Bug(title, description);
        
        Assert.AreEqual(title, bug.Title);
        Assert.AreEqual(description, bug.Description);
    }

    // 21. Тест нескольких переходов подряд
    [TestMethod]
    public void Bug_MultipleTransitions_ShouldFollowCorrectPath()
    {
        _bug.Assign();
        _bug.StartWork();
        _bug.Fix();
        _bug.Verify();
        _bug.Close();
        
        Assert.AreEqual(BugState.Closed, _bug.State);
    }

    // 22. Тест запроса информации на разных этапах
    [TestMethod]
    public void Bug_RequestInfoFromAssigned_ShouldWork()
    {
        _bug.Assign();
        _bug.RequestInfo();
        Assert.AreEqual(BugState.NeedMoreInfo, _bug.State);
    }

    // 23. Дополнительный тест на исключение - нельзя назначить дважды
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Bug_DoubleAssign_ShouldThrowException()
    {
        _bug.Assign();
        _bug.Assign();
    }
}
