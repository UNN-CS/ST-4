using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using System;

namespace BugTests
{
    [TestClass]
    public class TestsBugMachine
    {
        [TestMethod]
        public void InitialStateIsNew()
        {
            var bug = new Bug();
            Assert.AreEqual(State.New, bug.CurrentState);
        }

        [TestMethod]
        public void NewToAnalysis()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            Assert.AreEqual(State.Analysis, bug.CurrentState);
        }

        [TestMethod]
        public void AnalysisToFixing()
        {
            var bug = new Bug(State.Analysis);
            bug.Fire(Trigger.StartFixing);
            Assert.AreEqual(State.Fixing, bug.CurrentState);
        }

        [TestMethod]
        public void AnalysisToRejected()
        {
            var bug = new Bug(State.Analysis);
            bug.Fire(Trigger.Reject);
            Assert.AreEqual(State.Rejected, bug.CurrentState);
        }

        [TestMethod]
        public void AnalysisToDeferred()
        {
            var bug = new Bug(State.Analysis);
            bug.Fire(Trigger.Defer);
            Assert.AreEqual(State.Deferred, bug.CurrentState);
        }

        [TestMethod]
        public void FixingToClosed()
        {
            var bug = new Bug(State.Fixing);
            bug.Fire(Trigger.Resolve);
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void FixingToReturned()
        {
            var bug = new Bug(State.Fixing);
            bug.Fire(Trigger.VerifyFail);
            Assert.AreEqual(State.Returned, bug.CurrentState);
        }

        [TestMethod]
        public void FixingToUnreproducible()
        {
            var bug = new Bug(State.Fixing);
            bug.Fire(Trigger.CannotReproduce);
            Assert.AreEqual(State.Unreproducible, bug.CurrentState);
        }

        [TestMethod]
        public void UnreproducibleToClosed()
        {
            var bug = new Bug(State.Unreproducible);
            bug.Fire(Trigger.VerifySuccess);
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void ClosedToAnalysis()
        {
            var bug = new Bug(State.Closed);
            bug.Fire(Trigger.Reopen);
            Assert.AreEqual(State.Analysis, bug.CurrentState);
        }

        [TestMethod]
        public void DeferredToAnalysis()
        {
            var bug = new Bug(State.Deferred);
            bug.Fire(Trigger.ReturnToAnalysis);
            Assert.AreEqual(State.Analysis, bug.CurrentState);
        }

        [TestMethod]
        public void UnreproducibleToReturned()
        {
            var bug = new Bug(State.Unreproducible);
            bug.Fire(Trigger.VerifyFail);
            Assert.AreEqual(State.Returned, bug.CurrentState);
        }

        [TestMethod]
        public void ReturnedToAnalysis()
        {
            var bug = new Bug(State.Returned);
            bug.Fire(Trigger.ReturnToAnalysis);
            Assert.AreEqual(State.Analysis, bug.CurrentState);
        }

        [TestMethod]
        public void Error_FixingFromNew()
        {
            var bug = new Bug(State.New);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Trigger.StartFixing));
        }

        [TestMethod]
        public void Error_ReopenFromAnalysis()
        {
            var bug = new Bug(State.Analysis);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Trigger.Reopen));
        }

        [TestMethod]
        public void Error_ResolveFromDeferred()
        {
            var bug = new Bug(State.Deferred);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Trigger.Resolve));
        }

        [TestMethod]
        public void Error_AssignFromClosed()
        {
            var bug = new Bug(State.Closed);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Trigger.Assign));
        }

        [TestMethod]
        public void Error_RejectFromFixing()
        {
            var bug = new Bug(State.Fixing);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Trigger.Reject));
        }

        [TestMethod]
        public void Error_VerifySuccessFromNew()
        {
            var bug = new Bug(State.New);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Trigger.VerifySuccess));
        }

        [TestMethod]
        public void Error_ReturnToAnalysisFromClosed()
        {
            var bug = new Bug(State.Closed);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Trigger.ReturnToAnalysis));
        }

        [TestMethod]
        public void Error_CannotReproduceFromRejected()
        {
            var bug = new Bug(State.Rejected);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Trigger.CannotReproduce));
        }

        [TestMethod]
        public void Workflow_HappyPath()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFixing);
            bug.Fire(Trigger.CannotReproduce);
            bug.Fire(Trigger.VerifySuccess);
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void Workflow_FullReturn()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFixing);
            bug.Fire(Trigger.VerifyFail);
            bug.Fire(Trigger.ReturnToAnalysis);
            bug.Fire(Trigger.Reject);
            Assert.AreEqual(State.Rejected, bug.CurrentState);
        }
    }
}