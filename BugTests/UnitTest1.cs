using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        private Bug _bug;

        [TestInitialize]
        public void Setup() => _bug = new Bug("TEST-001", "Unit test bug");

        [TestMethod]
        public void NewBug_DefaultStateIsNew()
        {
            Assert.AreEqual(Bug.State.New, _bug.TekushayaState);
        }

        [TestMethod]
        public void StartSort_NewToSort()
        {
            _bug.StartSort();
            Assert.AreEqual(Bug.State.Sort, _bug.TekushayaState);
        }

        [TestMethod]
        public void AssignDev_SortToInProgress()
        {
            _bug.StartSort();
            _bug.AssignDev();
            Assert.AreEqual(Bug.State.InProgress, _bug.TekushayaState);
        }

        [TestMethod]
        public void MarkFixed_InProgressToVerified()
        {
            _bug.StartSort();
            _bug.AssignDev();
            _bug.MarkFixed();
            Assert.AreEqual(Bug.State.Verified, _bug.TekushayaState);
        }

        [TestMethod]
        public void VerifyPass_VerifiedToReadyForRelease()
        {
            ArrangeVerifiedBug();
            _bug.VerifyPass();
            Assert.AreEqual(Bug.State.ReadyForRelease, _bug.TekushayaState);
        }

        [TestMethod]
        public void Release_ReadyForReleaseToClosed()
        {
            ArrangeReadyForReleaseBug();
            _bug.Release();
            Assert.AreEqual(Bug.State.Closed, _bug.TekushayaState);
        }

        [TestMethod]
        public void RequestInfo_InProgressToAwaitingInfo()
        {
            _bug.StartSort();
            _bug.AssignDev();
            _bug.RequestInfo();
            Assert.AreEqual(Bug.State.AwaitingInfo, _bug.TekushayaState);
        }

        [TestMethod]
        public void ProvideInfo_AwaitingInfoToInProgress()
        {
            _bug.StartSort();
            _bug.AssignDev();
            _bug.RequestInfo();
            _bug.ProvideInfo();
            Assert.AreEqual(Bug.State.InProgress, _bug.TekushayaState);
        }

        [TestMethod]
        public void Defer_InProgressToDeferred()
        {
            _bug.StartSort();
            _bug.AssignDev();
            _bug.Defer();
            Assert.AreEqual(Bug.State.Deferred, _bug.TekushayaState);
        }

        [TestMethod]
        public void Go_DeferredToInProgress()
        {
            _bug.StartSort();
            _bug.AssignDev();
            _bug.Defer();
            _bug.Go();
            Assert.AreEqual(Bug.State.InProgress, _bug.TekushayaState);
        }

        [TestMethod]
        public void VerifyFail_VerifiedToInProgress()
        {
            ArrangeVerifiedBug();
            _bug.VerifyFail();
            Assert.AreEqual(Bug.State.InProgress, _bug.TekushayaState);
        }

        [TestMethod]
        public void StartSort_ReopenedToSort()
        {
            ArrangeClosedBug();
            _bug.Reopen();
            _bug.StartSort();
            Assert.AreEqual(Bug.State.Sort, _bug.TekushayaState);
        }

        [TestMethod]
        public void Ignore_RequestInfo_InSort_DoesNotChangeState()
        {
            _bug.StartSort();
            _bug.RequestInfo();
            Assert.AreEqual(Bug.State.Sort, _bug.TekushayaState);
        }

        [TestMethod]
        public void InvalidTransition_MarkFixed_FromNew_ThrowsStatelessException()
        {
            bool thrown = false;
            try { _bug.MarkFixed(); }
            catch (InvalidOperationException) { thrown = true; }
            Assert.IsTrue(thrown);
        }

        [TestMethod]
        public void InvalidTransition_VerifyPass_FromSort_ThrowsStatelessException()
        {
            _bug.StartSort();
            bool thrown = false;
            try { _bug.VerifyPass(); }
            catch (InvalidOperationException) { thrown = true; }
            Assert.IsTrue(thrown);
        }

        [TestMethod]
        public void InvalidTransition_Release_FromNew_ThrowsStatelessException()
        {
            bool thrown = false;
            try { _bug.Release(); }
            catch (InvalidOperationException) { thrown = true; }
            Assert.IsTrue(thrown);
        }

        [TestMethod]
        public void InvalidTransition_Reopen_FromSort_ThrowsStatelessException()
        {
            _bug.StartSort();
            bool thrown = false;
            try { _bug.Reopen(); }
            catch (InvalidOperationException) { thrown = true; }
            Assert.IsTrue(thrown);
        }

        [TestMethod]
        public void InvalidTransition_Go_FromNew_ThrowsStatelessException()
        {
            bool thrown = false;
            try { _bug.Go(); }
            catch (InvalidOperationException) { thrown = true; }
            Assert.IsTrue(thrown);
        }

        [TestMethod]
        public void InvalidTransition_ProvideInfo_FromNew_ThrowsStatelessException()
        {
            bool thrown = false;
            try { _bug.ProvideInfo(); }
            catch (InvalidOperationException) { thrown = true; }
            Assert.IsTrue(thrown);
        }

        [TestMethod]
        public void CanFire_ValidTrigger_ReturnsTrue()
        {
            Assert.IsTrue(_bug.CanFire(Bug.Trigger.StartSort));
        }

        [TestMethod]
        public void CanFire_InvalidTrigger_ReturnsFalse()
        {
            Assert.IsFalse(_bug.CanFire(Bug.Trigger.MarkFixed));
        }

        [TestMethod]
        public void GetPermittedTriggers_InitialState_ContainsStartSort()
        {
            var triggers = _bug.GetPermittedTriggers().ToList();
            Assert.IsTrue(triggers.Contains(Bug.Trigger.StartSort));
        }

        [TestMethod]
        public void History_TracksInitialState()
        {
            Assert.IsTrue(_bug.History.Any(h => h.Contains("Created")));
        }

        [TestMethod]
        public void TryMarkFixed_Valid_ReturnsTrueAndChangesState()
        {
            _bug.StartSort();
            _bug.AssignDev();
            var result = _bug.TryMarkFixed();
            Assert.IsTrue(result);
            Assert.AreEqual(Bug.State.Verified, _bug.TekushayaState);
        }

        [TestMethod]
        public void TryMarkFixed_Invalid_ReturnsFalseAndKeepsState()
        {
            var result = _bug.TryMarkFixed();
            Assert.IsFalse(result);
            Assert.AreEqual(Bug.State.New, _bug.TekushayaState);
        }

        private void ArrangeVerifiedBug()
        {
            _bug.StartSort();
            _bug.AssignDev();
            _bug.MarkFixed();
            Assert.AreEqual(Bug.State.Verified, _bug.TekushayaState);
        }

        private void ArrangeReadyForReleaseBug()
        {
            ArrangeVerifiedBug();
            _bug.VerifyPass();
            Assert.AreEqual(Bug.State.ReadyForRelease, _bug.TekushayaState);
        }

        private void ArrangeClosedBug()
        {
            ArrangeReadyForReleaseBug();
            _bug.Release();
            Assert.AreEqual(Bug.State.Closed, _bug.TekushayaState);
        }
    }
}