using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests;

[TestClass]
public class BugWorkflowTests
{
    private Bug _bug = null!;

    [TestInitialize]
    public void Setup()
    {
        _bug = new Bug();
    }

    // === ТЕСТЫ НАЧАЛЬНОГО СОСТОЯНИЯ ===
    
    [TestMethod]
    public void Bug_InitialState_ShouldBeNewDefect()
    {
        Assert.AreEqual(State.NewDefect, _bug.CurrentState);
    }

    // === ТЕСТЫ НОРМАЛЬНЫХ ПЕРЕХОДОВ ===
    
    [TestMethod]
    public void NewDefect_CanBeAnalyzed()
    {
        _bug.Analyze();
        Assert.AreEqual(State.Analysis, _bug.CurrentState);
    }

    [TestMethod]
    public void Analysis_CanBeRejected()
    {
        _bug.Analyze();
        _bug.Reject();
        Assert.AreEqual(State.Returned, _bug.CurrentState);
    }

    [TestMethod]
    public void Analysis_CanAskForInfo()
    {
        _bug.Analyze();
        _bug.AskInfo();
        Assert.AreEqual(State.NeedMoreInfo, _bug.CurrentState);
    }

    [TestMethod]
    public void Analysis_CanStartFix()
    {
        _bug.Analyze();
        _bug.StartFix();
        Assert.AreEqual(State.Resolution, _bug.CurrentState);
    }

    [TestMethod]
    public void NeedMoreInfo_CanProvideInfo()
    {
        _bug.Analyze();
        _bug.AskInfo();
        _bug.ProvideInfo();
        Assert.AreEqual(State.Analysis, _bug.CurrentState);
    }

    [TestMethod]
    public void NeedMoreInfo_CanContinueFix()
    {
        _bug.Analyze();
        _bug.AskInfo();
        _bug.ContinueFix();
        Assert.AreEqual(State.Resolution, _bug.CurrentState);
    }

    [TestMethod]
    public void Resolution_VerifySuccess_ClosesBug()
    {
        _bug.Analyze();
        _bug.StartFix();
        _bug.VerifySuccess();
        Assert.AreEqual(State.Closed, _bug.CurrentState);
    }

    [TestMethod]
    public void Resolution_VerifyFailure_ReturnsBug()
    {
        _bug.Analyze();
        _bug.StartFix();
        _bug.VerifyFailure();
        Assert.AreEqual(State.Returned, _bug.CurrentState);
    }

    [TestMethod]
    public void Resolution_ReportCannotReproduce_GoesToReview()
    {
        _bug.Analyze();
        _bug.StartFix();
        _bug.ReportCannotReproduce();
        Assert.AreEqual(State.Review, _bug.CurrentState);
    }

    [TestMethod]
    public void Resolution_ReturnForInfo_GoesToNeedMoreInfo()
    {
        _bug.Analyze();
        _bug.StartFix();
        _bug.ReturnForInfo();
        Assert.AreEqual(State.NeedMoreInfo, _bug.CurrentState);
    }

    [TestMethod]
    public void Review_ConfirmNotRepro_ClosesBug()
    {
        _bug.Analyze();
        _bug.StartFix();
        _bug.ReportCannotReproduce();
        _bug.ConfirmNotRepro();
        Assert.AreEqual(State.Closed, _bug.CurrentState);
    }

    [TestMethod]
    public void Review_ConfirmBugExists_ReturnsBug()
    {
        _bug.Analyze();
        _bug.StartFix();
        _bug.ReportCannotReproduce();
        _bug.ConfirmBugExists();
        Assert.AreEqual(State.Returned, _bug.CurrentState);
    }

    [TestMethod]
    public void ClosedBug_CanBeReopened()
    {
        _bug.Analyze();
        _bug.StartFix();
        _bug.VerifySuccess();
        _bug.Reopen();
        Assert.AreEqual(State.Reopened, _bug.CurrentState);
    }

    [TestMethod]
    public void ReopenedBug_CanBeAnalyzedAgain()
    {
        _bug.Analyze();
        _bug.StartFix();
        _bug.VerifySuccess();
        _bug.Reopen();
        _bug.AnalyzeAgain();
        Assert.AreEqual(State.Analysis, _bug.CurrentState);
    }

    // === ТЕСТЫ НЕДОПУСТИМЫХ ПЕРЕХОДОВ (ИСКЛЮЧЕНИЯ) ===
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void NewDefect_CannotStartFixWithoutAnalysis()
    {
        _bug.StartFix();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void NewDefect_CannotRejectWithoutAnalysis()
    {
        _bug.Reject();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Analysis_CannotVerifySuccessWithoutFix()
    {
        _bug.Analyze();
        _bug.VerifySuccess();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClosedBug_CannotBeAnalyzed()
    {
        _bug.Analyze();
        _bug.StartFix();
        _bug.VerifySuccess();
        _bug.Analyze();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void NeedMoreInfo_CannotReject()
    {
        _bug.Analyze();
        _bug.AskInfo();
        _bug.Reject();
    }

    // === ТЕСТЫ КОМПЛЕКСНЫХ СЦЕНАРИЕВ ===
    
    [TestMethod]
    public void Bug_CanCompleteFullWorkflow()
    {
        _bug.Analyze();
        _bug.StartFix();
        _bug.VerifySuccess();
        Assert.AreEqual(State.Closed, _bug.CurrentState);
    }

    [TestMethod]
    public void Bug_CanGoThroughInfoLoopAndComplete()
    {
        _bug.Analyze();
        _bug.AskInfo();
        _bug.ProvideInfo();
        _bug.StartFix();
        _bug.VerifySuccess();
        Assert.AreEqual(State.Closed, _bug.CurrentState);
    }

    [TestMethod]
    public void Bug_CanBeReopenedAndFixedAgain()
    {
        _bug.Analyze();
        _bug.StartFix();
        _bug.VerifySuccess();
        _bug.Reopen();
        _bug.AnalyzeAgain();
        _bug.StartFix();
        _bug.VerifySuccess();
        Assert.AreEqual(State.Closed, _bug.CurrentState);
    }
}
