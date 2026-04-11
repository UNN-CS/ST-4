using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using System;

namespace BugTests;

[TestClass]
public class BugStateMachineTests {
  private Bug _bug;

  [TestInitialize]
  public void Setup() { _bug = new Bug(); }

  [TestMethod]
  public void Test01_InitialState_ShouldBeNew() {
    Assert.AreEqual(State.New, _bug.CurrentState);
  }

  [TestMethod]
  public void Test02_New_StartTriage_TransitionsToTriage() {
    _bug.Fire(Trigger.StartTriage);

    Assert.AreEqual(State.Triage, _bug.CurrentState);
  }

  [TestMethod]
  public void Test03_New_InvalidTrigger_ThrowsInvalidOperationException() {
    Assert.ThrowsException<InvalidOperationException>(
        () => _bug.Fire(Trigger.SendToDev));
  }

  [TestMethod]
  public void Test04_Triage_SendToDev_TransitionsToInDevelopment() {
    _bug.Fire(Trigger.StartTriage);
    _bug.Fire(Trigger.SendToDev);

    Assert.AreEqual(State.InDevelopment, _bug.CurrentState);
  }

  [TestMethod]
  public void Test05_Triage_MarkNotADefect_TransitionsToNotADefect() {
    _bug.Fire(Trigger.StartTriage);
    _bug.Fire(Trigger.MarkNotADefect);

    Assert.AreEqual(State.NotADefect, _bug.CurrentState);
  }

  [TestMethod]
  public void Test06_Triage_MarkWontFix_TransitionsToWontFix() {
    _bug.Fire(Trigger.StartTriage);
    _bug.Fire(Trigger.MarkWontFix);

    Assert.AreEqual(State.WontFix, _bug.CurrentState);
  }

  [TestMethod]
  public void Test07_Triage_MarkDuplicate_TransitionsToDuplicate() {
    _bug.Fire(Trigger.StartTriage);
    _bug.Fire(Trigger.MarkDuplicate);

    Assert.AreEqual(State.Duplicate, _bug.CurrentState);
  }

  [TestMethod]
  public void Test08_InDevelopment_ProvideFix_TransitionsToReadyForTesting() {
    NavigateToDevState();
    _bug.Fire(Trigger.ProvideFix);

    Assert.AreEqual(State.ReadyForTesting, _bug.CurrentState);
  }

  [TestMethod]
  public void
  Test09_InDevelopment_MarkCannotReproduce_TransitionsToCannotReproduce() {
    NavigateToDevState();
    _bug.Fire(Trigger.MarkCannotReproduce);

    Assert.AreEqual(State.CannotReproduce, _bug.CurrentState);
  }

  [TestMethod]
  public void Test10_InDevelopment_DeferNoTime_ReturnsToTriage() {
    NavigateToDevState();
    _bug.Fire(Trigger.DeferNoTime);

    Assert.AreEqual(State.Triage, _bug.CurrentState);
  }

  [TestMethod]
  public void Test11_InDevelopment_DeferSeparateDecision_ReturnsToTriage() {
    NavigateToDevState();
    _bug.Fire(Trigger.DeferSeparateDecision);

    Assert.AreEqual(State.Triage, _bug.CurrentState);
  }

  [TestMethod]
  public void Test12_InDevelopment_DeferOtherProduct_ReturnsToTriage() {
    NavigateToDevState();
    _bug.Fire(Trigger.DeferOtherProduct);

    Assert.AreEqual(State.Triage, _bug.CurrentState);
  }

  [TestMethod]
  public void Test13_InDevelopment_RequestMoreInfo_ReturnsToTriage() {
    NavigateToDevState();
    _bug.Fire(Trigger.RequestMoreInfo);

    Assert.AreEqual(State.Triage, _bug.CurrentState);
  }

  [TestMethod]
  public void Test14_ReadyForTesting_VerifyOk_TransitionsToClosed() {
    NavigateToReadyForTestingState();
    _bug.Fire(Trigger.VerifyOk);

    Assert.AreEqual(State.Closed, _bug.CurrentState);
  }

  [TestMethod]
  public void Test15_ReadyForTesting_VerifyFailed_ReturnsToTriage() {
    NavigateToReadyForTestingState();
    _bug.Fire(Trigger.VerifyFailed);

    Assert.AreEqual(State.Triage, _bug.CurrentState);
  }

  [TestMethod]
  public void Test16_CannotReproduce_VerifyOk_TransitionsToClosed() {
    NavigateToCannotReproduceState();
    _bug.Fire(Trigger.VerifyOk);

    Assert.AreEqual(State.Closed, _bug.CurrentState);
  }

  [TestMethod]
  public void Test17_CannotReproduce_VerifyFailed_ReturnsToTriage() {
    NavigateToCannotReproduceState();
    _bug.Fire(Trigger.VerifyFailed);

    Assert.AreEqual(State.Triage, _bug.CurrentState);
  }

  [TestMethod]
  public void Test18_Closed_Reopen_ReturnsToTriage() {
    NavigateToReadyForTestingState();
    _bug.Fire(Trigger.VerifyOk);

    _bug.Fire(Trigger.Reopen);

    Assert.AreEqual(State.Triage, _bug.CurrentState);
  }

  [TestMethod]
  public void Test19_Closed_Archive_TransitionsToArchived() {
    NavigateToReadyForTestingState();
    _bug.Fire(Trigger.VerifyOk);

    _bug.Fire(Trigger.Archive);

    Assert.AreEqual(State.Archived, _bug.CurrentState);
  }

  [TestMethod]
  public void Test20_Closed_InvalidTrigger_ThrowsException() {
    NavigateToReadyForTestingState();
    _bug.Fire(Trigger.VerifyOk);

    Assert.ThrowsException<InvalidOperationException>(
        () => _bug.Fire(Trigger.ProvideFix));
  }

  [TestMethod]
  public void Test21_Archived_IgnoreArchiveTrigger_StaysArchived() {
    NavigateToReadyForTestingState();
    _bug.Fire(Trigger.VerifyOk);
    _bug.Fire(Trigger.Archive);

    _bug.Fire(Trigger.Archive);

    Assert.AreEqual(State.Archived, _bug.CurrentState);
  }

  [TestMethod]
  public void Test22_Archived_InvalidTrigger_ThrowsException() {
    NavigateToReadyForTestingState();
    _bug.Fire(Trigger.VerifyOk);
    _bug.Fire(Trigger.Archive);

    Assert.ThrowsException<InvalidOperationException>(
        () => _bug.Fire(Trigger.Reopen));
  }

  private void NavigateToDevState() {
    _bug.Fire(Trigger.StartTriage);
    _bug.Fire(Trigger.SendToDev);
  }

  private void NavigateToReadyForTestingState() {
    NavigateToDevState();
    _bug.Fire(Trigger.ProvideFix);
  }

  private void NavigateToCannotReproduceState() {
    NavigateToDevState();
    _bug.Fire(Trigger.MarkCannotReproduce);
  }
}