using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestInitialState()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.GetState());
        }

        [TestMethod]
        public void TestOpenToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void TestOpenToInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        public void TestAssignedToInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.StartProgress();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        public void TestAssignedToDefered()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.GetState());
        }

        [TestMethod]
        public void TestDeferedToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void TestInProgressToResolved()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.Resolve();
            Assert.AreEqual(Bug.State.Resolved, bug.GetState());
        }

        [TestMethod]
        public void TestResolvedToClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void TestResolvedToReopened()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.Resolve();
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void TestClosedToReopened()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void TestReopenedToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            bug.Reopen();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void TestReopenedToInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            bug.Reopen();
            bug.StartProgress();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        public void TestMultipleTransitions1()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.StartProgress();
            bug.Resolve();
            bug.Reopen();
            bug.Assign();
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void TestIgnoreTriggerAssign()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void TestInvalidTransitionAssignedToResolve()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Resolve();
            });
        }

        [TestMethod]
        public void TestInvalidTransitionAssignedToClose()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Close();
            });
        }

        [TestMethod]
        public void TestInvalidTransitionOpenToReopen()
        {
            var bug = new Bug(Bug.State.Open);

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Reopen();
            });
        }

        [TestMethod]
        public void TestComplexTransitionSequence()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.StartProgress();
            bug.Resolve();
            bug.Reopen();
            bug.StartProgress();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        public void TestInvalidDeferFromInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Defer();
            });
        }

        [TestMethod]
        public void TestMultipleValidTransitions()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.StartProgress();
            bug.Resolve();
            bug.Reopen();
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void TestRepeatIgnoredTrigger()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void TestInvalidReopenFromAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Reopen();
            });
        }

        [TestMethod]
        public void TestReopenedTransitionPaths()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            bug.Reopen();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());

            bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            bug.Reopen();
            bug.StartProgress();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }
    }
}