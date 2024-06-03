using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests
{
    [TestClass]
    public class BugStateMachineTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CloseFromOpenStateThrowsException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
        }

        [TestMethod]
        public void TransitionToDeferredState()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void AssignIsIgnoredInAssignedState()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestAllTransitions()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void TransitionFromOpenToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void CloseFromOpenStateThrowsException1()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
        }

        [TestMethod]
        public void DeferFromOpenStateThrowsException()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
        }

        [TestMethod]
        public void CloseFromDeferredStateThrowsException()
        {
            var bug = new Bug(Bug.State.Defered);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
        }

        [TestMethod]
        public void TransitionFromClosedToAssigned()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void AssignFromDeferredState()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void DeferFromAssignedState()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }
    }
}
