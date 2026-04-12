using BugPro;

namespace BugTests
{
    [TestClass]
    public class BugStateTransitionsTests
    {
        private const string SampleTitle = "Test bug";

        [TestMethod]
        public void Constructor_WithTitle_SetsInitialStateAndProperties()
        {
            var bug = new Bug(SampleTitle, "Description");

            Assert.AreEqual(SampleTitle, bug.Title);
            Assert.AreEqual("Description", bug.Description);
            Assert.AreEqual(BugState.New, bug.CurrentState);
            Assert.AreEqual(0, bug.TransitionCount);
            Assert.IsNotNull(bug.CreatedAt);
        }

        [TestMethod]
        public void Submit_FromNew_MovesToTriage()
        {
            var bug = new Bug(SampleTitle);
            bug.Fire(BugTrigger.Submit);

            Assert.AreEqual(BugState.Triage, bug.CurrentState);
            Assert.AreEqual(1, bug.TransitionCount);
            Assert.AreEqual(1, bug.History.Count);
        }

        [TestMethod]
        public void Cancel_FromNew_ClosesBug()
        {
            var bug = new Bug(SampleTitle);
            bug.Fire(BugTrigger.Cancel);

            Assert.AreEqual(BugState.Closed, bug.CurrentState);
            Assert.IsNotNull(bug.ClosedAt);
            Assert.AreEqual("Cancelled", bug.ClosedReason);
        }

        [TestMethod]
        public void AskForInfo_FromTriage_MovesToAwaitingInfo()
        {
            var bug = new Bug(SampleTitle);
            bug.Fire(BugTrigger.Submit);
            bug.Fire(BugTrigger.AskForInfo);

            Assert.AreEqual(BugState.AwaitingInfo, bug.CurrentState);
        }

        [TestMethod]
        public void ProvideInfo_FromAwaitingInfo_ReturnsToTriage()
        {
            var bug = new Bug(SampleTitle);
            bug.Fire(BugTrigger.Submit);
            bug.Fire(BugTrigger.AskForInfo);
            bug.Fire(BugTrigger.ProvideInfo);

            Assert.AreEqual(BugState.Triage, bug.CurrentState);
        }

        [TestMethod]
        public void MarkOtherProduct_FromTriage_MovesToOtherProduct()
        {
            var bug = new Bug(SampleTitle);
            bug.Fire(BugTrigger.Submit);
            bug.Fire(BugTrigger.MarkOtherProduct);

            Assert.AreEqual(BugState.OtherProduct, bug.CurrentState);
        }

        [TestMethod]
        public void MarkNotBug_FromTriage_TransitionsToNotABug()
        {
            var bug = new Bug(SampleTitle);
            bug.Fire(BugTrigger.Submit);
            bug.Fire(BugTrigger.MarkNotBug);

            Assert.AreEqual(BugState.NotABug, bug.CurrentState);
        }

        [TestMethod]
        public void CancelFromNotABug_ClosesWithCancelledReason()
        {
            var bug = new Bug(SampleTitle);
            bug.Fire(BugTrigger.Submit);
            bug.Fire(BugTrigger.MarkNotBug);
            bug.Fire(BugTrigger.Cancel);

            Assert.AreEqual(BugState.Closed, bug.CurrentState);
            Assert.AreEqual("Cancelled", bug.ClosedReason);
        }

        [TestMethod]
        public void Assign_FromTriage_MovesToInWork()
        {
            var bug = new Bug(SampleTitle);
            bug.Fire(BugTrigger.Submit);
            bug.Fire(BugTrigger.Assign);

            Assert.AreEqual(BugState.InWork, bug.CurrentState);
        }

        [TestMethod]
        public void CompleteWork_FromInWork_MovesToReview()
        {
            var bug = new Bug(SampleTitle);
            bug.Fire(BugTrigger.Submit);
            bug.Fire(BugTrigger.Assign);
            bug.Fire(BugTrigger.CompleteWork);

            Assert.AreEqual(BugState.Review, bug.CurrentState);
        }

        [TestMethod]
        public void Accept_FromReview_ClosesBugWithReason()
        {
            var bug = new Bug(SampleTitle);
            bug.Fire(BugTrigger.Submit);
            bug.Fire(BugTrigger.Assign);
            bug.Fire(BugTrigger.CompleteWork);
            bug.Fire(BugTrigger.Accept);

            Assert.AreEqual(BugState.Closed, bug.CurrentState);
            Assert.AreEqual("Fixed and accepted", bug.ClosedReason);
        }

        [TestMethod]
        public void Reject_FromReview_MovesToReturned()
        {
            var bug = new Bug(SampleTitle);
            bug.Fire(BugTrigger.Submit);
            bug.Fire(BugTrigger.Assign);
            bug.Fire(BugTrigger.CompleteWork);
            bug.Fire(BugTrigger.Reject);

            Assert.AreEqual(BugState.Returned, bug.CurrentState);
        }

        [TestMethod]
        public void Defer_FromTriage_MovesToDeferred()
        {
            var bug = new Bug(SampleTitle);
            bug.Fire(BugTrigger.Submit);
            bug.Fire(BugTrigger.Defer);

            Assert.AreEqual(BugState.Deferred, bug.CurrentState);
        }

        [TestMethod]
        public void Resume_FromDeferred_ReturnsToTriage()
        {
            var bug = new Bug(SampleTitle);
            bug.Fire(BugTrigger.Submit);
            bug.Fire(BugTrigger.Defer);
            bug.Fire(BugTrigger.Resume);

            Assert.AreEqual(BugState.Triage, bug.CurrentState);
        }

        [TestMethod]
        public void Reopen_FromClosed_MovesToTriage()
        {
            var bug = new Bug(SampleTitle);
            bug.Fire(BugTrigger.Submit);
            bug.Fire(BugTrigger.Reject);
            bug.Fire(BugTrigger.Reopen);

            Assert.AreEqual(BugState.Triage, bug.CurrentState);
        }

        [TestMethod]
        public void Return_FromNotABug_MovesToTriage()
        {
            var bug = new Bug(SampleTitle);
            bug.Fire(BugTrigger.Submit);
            bug.Fire(BugTrigger.MarkNotBug);
            bug.Fire(BugTrigger.Return);

            Assert.AreEqual(BugState.Triage, bug.CurrentState);
        }

        [TestMethod]
        public void History_TracksAllTransitions()
        {
            var bug = new Bug(SampleTitle);
            bug.Fire(BugTrigger.Submit);
            bug.Fire(BugTrigger.Assign);
            bug.Fire(BugTrigger.Defer);

            Assert.AreEqual(3, bug.History.Count);
            Assert.IsTrue(bug.History[0].Contains("New -> Triage"));
            Assert.IsTrue(bug.History[1].Contains("Triage -> InWork"));
            Assert.IsTrue(bug.History[2].Contains("InWork -> Deferred"));
        }

        [TestMethod]
        public void CanFire_ReturnsCorrectPermissions()
        {
            var bug = new Bug(SampleTitle);

            Assert.IsTrue(bug.CanFire(BugTrigger.Submit));
            Assert.IsTrue(bug.CanFire(BugTrigger.Cancel));
            Assert.IsFalse(bug.CanFire(BugTrigger.Assign));

            bug.Fire(BugTrigger.Submit);

            Assert.IsTrue(bug.CanFire(BugTrigger.Assign));
            Assert.IsTrue(bug.CanFire(BugTrigger.AskForInfo));
            Assert.IsTrue(bug.CanFire(BugTrigger.MarkNotBug));
        }

        [TestMethod]
        public void InvalidTransition_ThrowsException()
        {
            var bug = new Bug(SampleTitle);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Assign));
        }

        [TestMethod]
        public void FullHappyPath_CompletesSuccessfully()
        {
            var bug = new Bug(SampleTitle);

            bug.Fire(BugTrigger.Submit);
            bug.Fire(BugTrigger.Assign);
            bug.Fire(BugTrigger.CompleteWork);
            bug.Fire(BugTrigger.Accept);

            Assert.AreEqual(BugState.Closed, bug.CurrentState);
            Assert.AreEqual("Fixed and accepted", bug.ClosedReason);
            Assert.IsNotNull(bug.ClosedAt);
            Assert.AreEqual(4, bug.TransitionCount);
        }

        [TestMethod]
        public void GetFullReport_ReturnsInformativeString()
        {
            var bug = new Bug(SampleTitle, "Test desc");
            bug.Fire(BugTrigger.Submit);

            var report = bug.GetFullReport();

            Assert.IsTrue(report.Contains(SampleTitle));
            Assert.IsTrue(report.Contains("Test desc"));
            Assert.IsTrue(report.Contains(nameof(BugState.Triage)));
            Assert.IsTrue(report.Contains("History"));
        }
    }
}