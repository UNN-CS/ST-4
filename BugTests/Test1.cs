using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using Stateless;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void Open_CanAssign_ShouldTransitionToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Open_CanClose_ShouldTransitionToClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Open_CannotDefer_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Defer();
        }

        [TestMethod]
        public void Assigned_CanStartProgress_ShouldTransitionToInProgress()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.StartProgress();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        public void Assigned_CanDefer_ShouldTransitionToDefered()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.GetState());
        }

        [TestMethod]
        public void Assigned_CanClose_ShouldTransitionToClosed()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Assigned_CannotResolve_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Resolve();
        }

        [TestMethod]
        public void InProgress_CanResolve_ShouldTransitionToResolved()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Resolve();
            Assert.AreEqual(Bug.State.Resolved, bug.GetState());
        }

        [TestMethod]
        public void InProgress_CanDefer_ShouldTransitionToDefered()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.GetState());
        }

        [TestMethod]
        public void InProgress_CanReassign_ShouldTransitionToAssigned()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Reassign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InProgress_CannotClose_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Close();
        }

        [TestMethod]
        public void Defered_CanAssign_ShouldTransitionToAssigned()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Defered_CanClose_ShouldTransitionToClosed()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Defered_CannotResolve_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Resolve();
        }

        [TestMethod]
        public void Resolved_CanClose_ShouldTransitionToClosed()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void Resolved_CanReopen_ShouldTransitionToReopened()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Resolved_CannotAssign_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Assign();
        }

        [TestMethod]
        public void Closed_CanReopen_ShouldTransitionToReopened()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Closed_CannotDefer_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Defer();
        }

        [TestMethod]
        public void Reopened_CanAssign_ShouldTransitionToAssigned()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Reopened_CanClose_ShouldTransitionToClosed()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Reopened_CannotResolve_ShouldThrowException()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Resolve();
        }
    }
}