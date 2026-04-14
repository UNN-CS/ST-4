using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BugTests;

[TestClass]
public class BugWorkflowTests
{
    // Тест 1: Начальное состояние
    [TestMethod]
    public void Test01_InitialState_ShouldBeNewDefect()
    {
        var bug = new Bug();
        Assert.AreEqual(State.NewDefect, bug.CurrentState);
    }

    // Тест 2: Анализ бага
    [TestMethod]
    public void Test02_Analyze_TransitionsToAnalysis()
    {
        var bug = new Bug();
        bug.Analyze();
        Assert.AreEqual(State.Analysis, bug.CurrentState);
    }

    // Тест 3: Отклонение бага
    [TestMethod]
    public void Test03_Reject_TransitionsToReturned()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.Reject();
        Assert.AreEqual(State.Returned, bug.CurrentState);
    }

    // Тест 4: Запрос информации
    [TestMethod]
    public void Test04_AskInfo_TransitionsToNeedMoreInfo()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.AskInfo();
        Assert.AreEqual(State.NeedMoreInfo, bug.CurrentState);
    }

    // Тест 5: Предоставление информации
    [TestMethod]
    public void Test05_ProvideInfo_TransitionsToAnalysis()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.AskInfo();
        bug.ProvideInfo();
        Assert.AreEqual(State.Analysis, bug.CurrentState);
    }

    // Тест 6: Начало исправления
    [TestMethod]
    public void Test06_StartFix_TransitionsToResolution()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        Assert.AreEqual(State.Resolution, bug.CurrentState);
    }

    // Тест 7: Успешная проверка
    [TestMethod]
    public void Test07_VerifySuccess_TransitionsToClosed()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.VerifySuccess();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    // Тест 8: Неуспешная проверка
    [TestMethod]
    public void Test08_VerifyFailure_TransitionsToReturned()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.VerifyFailure();
        Assert.AreEqual(State.Returned, bug.CurrentState);
    }

    // Тест 9: Сообщение о невоспроизводимости
    [TestMethod]
    public void Test09_ReportCannotReproduce_TransitionsToReview()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.ReportCannotReproduce();
        Assert.AreEqual(State.Review, bug.CurrentState);
    }

    // Тест 10: Подтверждение невоспроизводимости
    [TestMethod]
    public void Test10_ConfirmNotRepro_TransitionsToClosed()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.ReportCannotReproduce();
        bug.ConfirmNotRepro();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    // Тест 11: Подтверждение существования бага
    [TestMethod]
    public void Test11_ConfirmBugExists_TransitionsToReturned()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.ReportCannotReproduce();
        bug.ConfirmBugExists();
        Assert.AreEqual(State.Returned, bug.CurrentState);
    }

    // Тест 12: Возврат за информацией
    [TestMethod]
    public void Test12_ReturnForInfo_TransitionsToNeedMoreInfo()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.ReturnForInfo();
        Assert.AreEqual(State.NeedMoreInfo, bug.CurrentState);
    }

    // Тест 13: Продолжение исправления после получения информации
    [TestMethod]
    public void Test13_ContinueFix_TransitionsToResolution()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.ReturnForInfo();
        bug.ContinueFix();
        Assert.AreEqual(State.Resolution, bug.CurrentState);
    }

    // Тест 14: Переоткрытие закрытого бага
    [TestMethod]
    public void Test14_Reopen_TransitionsToReopened()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.VerifySuccess();
        bug.Reopen();
        Assert.AreEqual(State.Reopened, bug.CurrentState);
    }

    // Тест 15: Повторный анализ переоткрытого бага
    [TestMethod]
    public void Test15_AnalyzeAgain_TransitionsToAnalysis()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.VerifySuccess();
        bug.Reopen();
        bug.AnalyzeAgain();
        Assert.AreEqual(State.Analysis, bug.CurrentState);
    }

    // Тест 16: Полный позитивный сценарий
    [TestMethod]
    public void Test16_PositiveScenario_CompletesSuccessfully()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.VerifySuccess();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    // Тест 17: Сценарий с запросом информации
    [TestMethod]
    public void Test17_InfoRequestScenario_CompletesSuccessfully()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.AskInfo();
        bug.ProvideInfo();
        bug.StartFix();
        bug.VerifySuccess();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    // Тест 18: Сценарий с возвратом за информацией
    [TestMethod]
    public void Test18_ReturnForInfoScenario_CompletesSuccessfully()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.ReturnForInfo();
        bug.ContinueFix();
        bug.VerifySuccess();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    // Тест 19: Сценарий с невоспроизводимым багом
    [TestMethod]
    public void Test19_NotReproducibleScenario_ClosesSuccessfully()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.ReportCannotReproduce();
        bug.ConfirmNotRepro();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    // Тест 20: Сценарий с переоткрытием
    [TestMethod]
    public void Test20_ReopenScenario_CompletesSuccessfully()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.VerifySuccess();
        bug.Reopen();
        bug.AnalyzeAgain();
        bug.StartFix();
        bug.VerifySuccess();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }

    // Тест 21: Исключение - нельзя начать исправление без анализа
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Test21_StartFixFromNewDefect_ThrowsException()
    {
        var bug = new Bug();
        bug.StartFix();
    }

    // Тест 22: Исключение - нельзя проверить без исправления
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Test22_VerifySuccessFromAnalysis_ThrowsException()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.VerifySuccess();
    }

    // Тест 23: Исключение - нельзя переоткрыть незакрытый баг
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Test23_ReopenFromResolution_ThrowsException()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.Reopen();
    }
}