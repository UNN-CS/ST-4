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
        public void Setup()
        {
            _bug = new Bug();
        }

        [TestMethod]
        public void InitialState_ShouldBeNewDefect()
        {
            Assert.AreEqual(Bug.State.NewDefect, _bug.CurrentState);
        }

        [TestMethod]
        public void StartTriage_FromNewDefect_ShouldTransitionToTriage()
        {
            _bug.StartTriage();
            Assert.AreEqual(Bug.State.Triage, _bug.CurrentState);
        }

        [TestMethod]
        public void NeedSeparateSolution_FromTriage_ShouldTransitionToFix()
        {
            _bug.StartTriage();
            _bug.NeedSeparateSolution();
            Assert.AreEqual(Bug.State.Fix, _bug.CurrentState);
        }

        [TestMethod]
        public void NoTime_FromTriage_ShouldTransitionToFix()
        {
            _bug.StartTriage();
            _bug.NoTime();
            Assert.AreEqual(Bug.State.Fix, _bug.CurrentState);
        }

        [TestMethod]
        public void FixDone_FromFix_ShouldTransitionToClosed()
        {
            _bug.StartTriage();
            _bug.NeedSeparateSolution();
            _bug.FixDone();
            Assert.AreEqual(Bug.State.Closed, _bug.CurrentState);
        }

        [TestMethod]
        public void NotDefect_FromTriage_ShouldTransitionToClosed()
        {
            _bug.StartTriage();
            _bug.NotDefect();
            Assert.AreEqual(Bug.State.Closed, _bug.CurrentState);
        }

        [TestMethod]
        public void DoNotFix_FromTriage_ShouldTransitionToClosed()
        {
            _bug.StartTriage();
            _bug.DoNotFix();
            Assert.AreEqual(Bug.State.Closed, _bug.CurrentState);
        }

        [TestMethod]
        public void Duplicate_FromTriage_ShouldTransitionToClosed()
        {
            _bug.StartTriage();
            _bug.Duplicate();
            Assert.AreEqual(Bug.State.Closed, _bug.CurrentState);
        }

        [TestMethod]
        public void OtherProduct_FromTriage_ShouldTransitionToClosed()
        {
            _bug.StartTriage();
            _bug.OtherProduct();
            Assert.AreEqual(Bug.State.Closed, _bug.CurrentState);
        }

        [TestMethod]
        public void NotReproducible_FromTriage_ShouldTransitionToClosed()
        {
            _bug.StartTriage();
            _bug.NotReproducible();
            Assert.AreEqual(Bug.State.Closed, _bug.CurrentState);
        }

        [TestMethod]
        public void Reopen_FromClosed_ShouldTransitionToReopened()
        {
            ArrangeClosedBug();
            _bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, _bug.CurrentState);
        }

        [TestMethod]
        public void StartTriage_FromReopened_ShouldTransitionToTriage()
        {
            ArrangeClosedBug();
            _bug.Reopen();
            _bug.StartTriage();
            Assert.AreEqual(Bug.State.Triage, _bug.CurrentState);
        }

        [TestMethod]
        public void FixDone_FromReopened_ShouldTransitionToClosed()
        {
            ArrangeClosedBug();
            _bug.Reopen();
            _bug.FixDone();
            Assert.AreEqual(Bug.State.Closed, _bug.CurrentState);
        }

        [TestMethod]
        public void NeedMoreInfo_InTriage_ShouldNotChangeState()
        {
            _bug.StartTriage();
            _bug.NeedMoreInfo();
            Assert.AreEqual(Bug.State.Triage, _bug.CurrentState);
        }

        [TestMethod]
        public void StartTriage_InTriage_ShouldNotChangeState()
        {
            _bug.StartTriage();
            _bug.StartTriage();
            Assert.AreEqual(Bug.State.Triage, _bug.CurrentState);
        }

        [TestMethod]
        public void Reopen_InReopened_ShouldNotChangeState()
        {
            ArrangeClosedBug();
            _bug.Reopen();
            _bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, _bug.CurrentState);
        }

        [TestMethod]
        public void FixDone_FromNewDefect_ShouldThrowException()
        {
            bool isThrown = false;
            try { _bug.FixDone(); }
            catch (InvalidOperationException) { isThrown = true; }
            Assert.IsTrue(isThrown, "InvalidOperationException not thrown.");
        }

        [TestMethod]
        public void Reopen_FromNewDefect_ShouldThrowException()
        {
            bool isThrown = false;
            try { _bug.Reopen(); }
            catch (InvalidOperationException) { isThrown = true; }
            Assert.IsTrue(isThrown, "InvalidOperationException not thrown.");
        }

        [TestMethod]
        public void StartTriage_FromClosed_ShouldThrowException()
        {
            ArrangeClosedBug();
            bool isThrown = false;
            try { _bug.StartTriage(); }
            catch (InvalidOperationException) { isThrown = true; }
            Assert.IsTrue(isThrown, "InvalidOperationException not thrown.");
        }

        [TestMethod]
        public void NeedSeparateSolution_FromClosed_ShouldThrowException()
        {
            ArrangeClosedBug();
            bool isThrown = false;
            try { _bug.NeedSeparateSolution(); }
            catch (InvalidOperationException) { isThrown = true; }
            Assert.IsTrue(isThrown, "InvalidOperationException not thrown.");
        }

        [TestMethod]
        public void Duplicate_FromFix_ShouldTransitionToClosed()
        {
            _bug.StartTriage();
            _bug.NeedSeparateSolution();
            _bug.Duplicate();
            Assert.AreEqual(Bug.State.Closed, _bug.CurrentState);
        }

        [TestMethod]
        public void CanFire_ValidTrigger_ShouldReturnTrue()
        {
            Assert.IsTrue(_bug.CanFire(Bug.Trigger.StartTriage));
        }

        [TestMethod]
        public void CanFire_InvalidTrigger_ShouldReturnFalse()
        {
            Assert.IsFalse(_bug.CanFire(Bug.Trigger.FixDone));
        }

        [TestMethod]
        public void GetPermittedTriggers_ShouldContainStartTriage()
        {
            var triggers = _bug.GetPermittedTriggers().ToList();
            Assert.IsTrue(triggers.Contains(Bug.Trigger.StartTriage));
        }

        [TestMethod]
        public void GetPermittedTriggers_ShouldNotContainFixDone()
        {
            var triggers = _bug.GetPermittedTriggers().ToList();
            Assert.IsFalse(triggers.Contains(Bug.Trigger.FixDone));
        }

        private void ArrangeClosedBug()
        {
            _bug.StartTriage();
            _bug.NeedSeparateSolution();
            _bug.FixDone();
            Assert.AreEqual(Bug.State.Closed, _bug.CurrentState);
        }
    }
}