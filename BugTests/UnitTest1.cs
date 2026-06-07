using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public sealed class BugLifecycleTests
{
    private Bug _bug = null!;

    [TestInitialize]
    public void SetUp()
    {
        _bug = new Bug();
    }

    [TestCleanup]
    public void TearDown()
    {
        _bug = null!;
    }

    #region Старт

    [TestMethod]
    public void OnCreate_StatusIsNew()
    {
        CheckStatus(BugStatus.New);
    }

    [TestMethod]
    public void OnCreate_CanStartAnalysis()
    {
        Assert.IsTrue(_bug.CanStartAnalysis());
    }

    [TestMethod]
    public void OnCreate_CannotFix()
    {
        Assert.IsFalse(_bug.CanFix());
    }

    [TestMethod]
    public void OnCreate_CannotClose()
    {
        Assert.IsFalse(_bug.CanClose());
    }

    [TestMethod]
    public void OnCreate_CannotReopen()
    {
        Assert.IsFalse(_bug.CanReopen());
    }

    #endregion

    #region Анализ и уточнение

    [TestMethod]
    public void AfterStartAnalysis_StatusIsAnalysis()
    {
        _bug.StartAnalysis();
        CheckStatus(BugStatus.Analysis);
    }

    [TestMethod]
    public void AfterRequestInfo_StatusIsNeedMoreInfo()
    {
        MoveToAnalysis();
        _bug.RequestInfo();
        CheckStatus(BugStatus.NeedMoreInfo);
    }

    [TestMethod]
    public void InNeedMoreInfo_CanReturnToAnalysis()
    {
        MoveToNeedMoreInfo();
        Assert.IsTrue(_bug.CanReturnToAnalysis());
    }

    [TestMethod]
    public void AfterReturnFromNeedMoreInfo_StatusIsAnalysis()
    {
        MoveToNeedMoreInfo();
        _bug.ReturnToAnalysis();
        CheckStatus(BugStatus.Analysis);
    }

    [TestMethod]
    public void AfterStartFix_StatusIsInProgress()
    {
        MoveToAnalysis();
        _bug.StartFix();
        CheckStatus(BugStatus.InProgress);
    }

    #endregion

    #region Исправление

    [TestMethod]
    public void InProgress_CanFix()
    {
        MoveToInProgress();
        Assert.IsTrue(_bug.CanFix());
    }

    [TestMethod]
    public void AfterFix_StatusIsFixed()
    {
        MoveToInProgress();
        _bug.Fix();
        CheckStatus(BugStatus.Fixed);
    }

    [TestMethod]
    public void InFixed_CanClose()
    {
        MoveToFixed();
        Assert.IsTrue(_bug.CanClose());
    }

    [TestMethod]
    public void AfterConfirmResolved_StatusIsClosed()
    {
        MoveToFixed();
        _bug.ConfirmResolved();
        CheckStatus(BugStatus.Closed);
    }

    #endregion

    #region Переоткрытие

    [TestMethod]
    public void InClosed_CanReopen()
    {
        MoveToClosed();
        Assert.IsTrue(_bug.CanReopen());
    }

    [TestMethod]
    public void AfterReopen_StatusIsReopened()
    {
        MoveToClosed();
        _bug.Reopen();
        CheckStatus(BugStatus.Reopened);
    }

    [TestMethod]
    public void AfterRejectFix_StatusIsReopened()
    {
        MoveToFixed();
        _bug.RejectFix();
        CheckStatus(BugStatus.Reopened);
    }

    [TestMethod]
    public void FromReopened_ReturnToAnalysis()
    {
        MoveToReopened();
        _bug.ReturnToAnalysis();
        CheckStatus(BugStatus.Analysis);
    }

    [TestMethod]
    public void FromReopened_ResumeFix()
    {
        MoveToReopened();
        _bug.StartFix();
        CheckStatus(BugStatus.InProgress);
    }

    #endregion

    #region Дубликат и не воспроизводится

    [TestMethod]
    public void AfterMarkDuplicate_StatusIsDuplicate()
    {
        MoveToAnalysis();
        _bug.MarkDuplicate();
        CheckStatus(BugStatus.Duplicate);
    }

    [TestMethod]
    public void FromDuplicate_CloseTicket()
    {
        MoveToAnalysis();
        _bug.MarkDuplicate();
        _bug.ConfirmResolved();
        CheckStatus(BugStatus.Closed);
    }

    [TestMethod]
    public void AfterMarkNotReproducible_StatusIsNotReproducible()
    {
        MoveToAnalysis();
        _bug.MarkNotReproducible();
        CheckStatus(BugStatus.NotReproducible);
    }

    [TestMethod]
    public void FromNotReproducible_CloseTicket()
    {
        MoveToAnalysis();
        _bug.MarkNotReproducible();
        _bug.ConfirmResolved();
        CheckStatus(BugStatus.Closed);
    }

    #endregion

    #region Отложен

    [TestMethod]
    public void AfterDefer_StatusIsDeferred()
    {
        _bug.Defer();
        CheckStatus(BugStatus.Deferred);
    }

    [TestMethod]
    public void FromDeferred_ReturnToAnalysis()
    {
        _bug.Defer();
        _bug.ReturnToAnalysis();
        CheckStatus(BugStatus.Analysis);
    }

    #endregion

    #region Запрещённые переходы

    [TestMethod]
    public void FixFromNew_Throws()
    {
        ExpectFailure(() => _bug.Fix());
    }

    [TestMethod]
    public void CloseFromAnalysis_Throws()
    {
        MoveToAnalysis();
        ExpectFailure(() => _bug.ConfirmResolved());
    }

    [TestMethod]
    public void ReopenFromFixed_Throws()
    {
        MoveToFixed();
        ExpectFailure(() => _bug.Reopen());
    }

    [TestMethod]
    public void StartAnalysisFromClosed_Throws()
    {
        MoveToClosed();
        ExpectFailure(() => _bug.StartAnalysis());
    }

    [TestMethod]
    public void ReturnToAnalysisFromNew_Throws()
    {
        ExpectFailure(() => _bug.ReturnToAnalysis());
    }

    #endregion

    private void CheckStatus(BugStatus expected)
    {
        Assert.AreEqual(expected, _bug.State);
    }

    private static void ExpectFailure(Action action)
    {
        Assert.ThrowsException<InvalidOperationException>(action);
    }

    private void MoveToAnalysis()
    {
        _bug.StartAnalysis();
    }

    private void MoveToNeedMoreInfo()
    {
        MoveToAnalysis();
        _bug.RequestInfo();
    }

    private void MoveToInProgress()
    {
        MoveToAnalysis();
        _bug.StartFix();
    }

    private void MoveToFixed()
    {
        MoveToInProgress();
        _bug.Fix();
    }

    private void MoveToClosed()
    {
        MoveToFixed();
        _bug.ConfirmResolved();
    }

    private void MoveToReopened()
    {
        MoveToFixed();
        _bug.RejectFix();
    }
}
