using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void TestToClosed()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void ToDeferred()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void DeferredToAssigned()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void AssignIgnoredInAssignedState()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void ComplexScenario()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void OpenToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void CloseFromOpenThrowsException()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
        }

        [TestMethod]
        public void DeferFromOpenThrowsException()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
        }

        [TestMethod]
        public void CloseFromDeferredThrowsException()
        {
            var bug = new Bug(Bug.State.Defered);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
        }

        [TestMethod]
        public void ClosedToAssigned()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void ClosedToAssigned2()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }
    }
}
