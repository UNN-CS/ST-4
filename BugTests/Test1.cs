using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using System;

namespace BugTests
{
    [TestClass]
    public class BugStateMachineTests
    {
        private Bug _bug = null!;

        [TestInitialize]
        public void Setup()
        {
            _bug = new Bug("Test Bug");
        }

        private void AssertThrowsInvalidOp(Action action)
        {
            try
            {
                action();

                Assert.Fail(
                    "Ожидалось исключение InvalidOperationException.");
            }
            catch (InvalidOperationException)
            {
                
            }
        }

        [TestMethod]
        public void InitialState_ShouldBeNew()
        {
            Assert.AreEqual(State.New, _bug.CurrentState);
        }

        [TestMethod]
        public void Triage_ShouldMoveToTriaged()
        {
            _bug.Triage();

            Assert.AreEqual(
                State.Triaged,
                _bug.CurrentState);
        }

        [TestMethod]
        public void Assign_ShouldMoveToAssigned()
        {
            _bug.Triage();

            _bug.Assign("Dev1");

            Assert.AreEqual(
                State.Assigned,
                _bug.CurrentState);
        }

        [TestMethod]
        public void Assign_ShouldSetDeveloper()
        {
            _bug.Triage();

            _bug.Assign("Dev1");

            Assert.AreEqual(
                "Dev1",
                _bug.Developer);
        }

        [TestMethod]
        public void StartWork_ShouldMoveToInProgress()
        {
            _bug.Triage();

            _bug.Assign("Dev1");

            _bug.StartWork();

            Assert.AreEqual(
                State.InProgress,
                _bug.CurrentState);
        }

        [TestMethod]
        public void SendToTesting_ShouldMoveToTesting()
        {
            _bug.Triage();

            _bug.Assign("Dev1");

            _bug.StartWork();

            _bug.SendToTesting();

            Assert.AreEqual(
                State.Testing,
                _bug.CurrentState);
        }

        [TestMethod]
        public void Fix_ShouldMoveToFixed()
        {
            _bug.Triage();

            _bug.Assign("Dev1");

            _bug.StartWork();

            _bug.SendToTesting();

            _bug.Fix();

            Assert.AreEqual(
                State.Fixed,
                _bug.CurrentState);
        }

        [TestMethod]
        public void Close_ShouldMoveToClosed()
        {
            _bug.Triage();

            _bug.Assign("Dev1");

            _bug.StartWork();

            _bug.SendToTesting();

            _bug.Fix();

            _bug.Close();

            Assert.AreEqual(
                State.Closed,
                _bug.CurrentState);
        }

        [TestMethod]
        public void Reopen_ShouldMoveToReopened()
        {
            _bug.Triage();

            _bug.Assign("Dev1");

            _bug.StartWork();

            _bug.SendToTesting();

            _bug.Fix();

            _bug.Close();

            _bug.Reopen();

            Assert.AreEqual(
                State.Reopened,
                _bug.CurrentState);
        }

        [TestMethod]
        public void Defer_ShouldMoveToDeferred()
        {
            _bug.Triage();

            _bug.Defer();

            Assert.AreEqual(
                State.Deferred,
                _bug.CurrentState);
        }

        [TestMethod]
        public void CanFire_TriageFromNew_ShouldBeTrue()
        {
            Assert.IsTrue(
                _bug.CanFire(Trigger.Triage));
        }

        [TestMethod]
        public void CanFire_CloseFromNew_ShouldBeFalse()
        {
            Assert.IsFalse(
                _bug.CanFire(Trigger.Close));
        }

        [TestMethod]
        public void Close_FromNew_ShouldThrow()
        {
            AssertThrowsInvalidOp(
                () => _bug.Close());
        }

        [TestMethod]
        public void Fix_FromAssigned_ShouldThrow()
        {
            _bug.Triage();

            _bug.Assign("Dev1");

            AssertThrowsInvalidOp(
                () => _bug.Fix());
        }

        [TestMethod]
        public void Cannot_Close_From_New()
        {
            Assert.ThrowsException<InvalidOperationException>(() => _bug.Close());
        }

        [TestMethod]
        public void Cannot_Fix_From_New()
        {
            Assert.ThrowsException<InvalidOperationException>(() => _bug.Fix());
        }

        [TestMethod]
        public void Cannot_StartWork_From_New()
        {
            Assert.ThrowsException<InvalidOperationException>(() => _bug.StartWork());
        }

        [TestMethod]
        public void Cannot_SendToTesting_Without_Work()
        {
            _bug.Triage();
            Assert.ThrowsException<InvalidOperationException>(() => _bug.SendToTesting());
        }

        [TestMethod]
        public void Reopened_Assign_ShouldMoveToAssigned()
        {
            _bug.Triage();
            _bug.Assign("Dev1");
            _bug.StartWork();
            _bug.SendToTesting();
            _bug.Fix();
            _bug.Close();
            _bug.Reopen();

            _bug.Assign("Dev2");

            Assert.AreEqual(State.Assigned, _bug.CurrentState);
            Assert.AreEqual("Dev2", _bug.Developer);
        }

        [TestMethod]
        public void Deferred_CanFire_Assign_ShouldBeTrue()
        {
            _bug.Triage();
            _bug.Defer();

            Assert.IsTrue(_bug.CanFire(Trigger.Assign));
        }

        [TestMethod]
        public void Reject_FromTesting_ShouldMoveToReopened()
        {
            _bug.Triage();

            _bug.Assign("Dev1");

            _bug.StartWork();

            _bug.SendToTesting();

            _bug.Reject();

            Assert.AreEqual(
                State.Reopened,
                _bug.CurrentState);
        }
    }
}