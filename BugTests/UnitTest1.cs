using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void test_initialState()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.GetState());
        }

        [TestMethod]
        public void test_openToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void test_openToInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        public void test_assignedToInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.StartProgress();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        public void test_assignedToDefered()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.GetState());
        }

        [TestMethod]
        public void test_deferedToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void test_progressToResolved()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.Resolve();
            Assert.AreEqual(Bug.State.Resolved, bug.GetState());
        }

        [TestMethod]
        public void test_resolvedToClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void test_resolvedToReopened()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.Resolve();
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void test_closedToReopened()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void test_reopenedToAssigned()
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
        public void test_reopenedToInProgress()
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
        public void test_ignoreTriggerAssign()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void test_invalidTransitionAssignedToResolve()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Resolve();
            });
        }

        [TestMethod]
        public void test_invalidTransitionAssignedToClose()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Close();
            });
        }

        [TestMethod]
        public void test_invalidTransitionOpenToReopen()
        {
            var bug = new Bug(Bug.State.Open);

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Reopen();
            });
        }

        [TestMethod]
        public void test_invalidTransitionToDeferFromResolved()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.Resolve();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Defer();
            });
        }

        [TestMethod]
        public void test_complexTransitionWithReopen()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void test_complexTransitionSequence()
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
        public void test_invalidDeferFromInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Defer();
            });
        }

        [TestMethod]
        public void test_multipleValidTransitions()
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
        public void test_repeatIgnoredTrigger()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void test_invalidReopenFromAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Reopen();
            });
        }

        [TestMethod]
        public void test_reopenedTransitionPaths()
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