using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestToClosed()
        {
            var a = new Bug(Bug.State.Assigned);
            a.Close();
            Assert.AreEqual(Bug.State.Closed, a.getState());
        }

        [TestMethod]
        public void ToDeferred()
        {
            var a = new Bug(Bug.State.Assigned);
            a.Defer();
            Assert.AreEqual(Bug.State.Defered, a.getState());
        }

        [TestMethod]
        public void DeferredToAssigned()
        {
            var a = new Bug(Bug.State.Defered);
            a.Assign();
            Assert.AreEqual(Bug.State.Assigned, a.getState());
        }

        [TestMethod]
        public void AssignIgnoredInAssignedState()
        {
            var a = new Bug(Bug.State.Assigned);
            a.Assign();
            Assert.AreEqual(Bug.State.Assigned, a.getState());
        }

        [TestMethod]
        public void ComplexScenario()
        {
            var a = new Bug(Bug.State.Open);
            a.Assign();
            a.Defer();
            a.Assign();
            a.Close();
            Assert.AreEqual(Bug.State.Closed, a.getState());
        }

        [TestMethod]
        public void OpenToAssigned()
        {
            var a = new Bug(Bug.State.Open);
            a.Assign();
            Assert.AreEqual(Bug.State.Assigned, a.getState());
        }

        [TestMethod]
        public void CloseFromOpenThrowsException()
        {
            var a = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => a.Close());
        }

        [TestMethod]
        public void DeferFromOpenThrowsException()
        {
            var a = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => a.Defer());
        }

        [TestMethod]
        public void CloseFromDeferredThrowsException()
        {
            var a = new Bug(Bug.State.Defered);
            Assert.ThrowsException<InvalidOperationException>(() => a.Close());
        }

        [TestMethod]
        public void ClosedToAssigned()
        {
            var a = new Bug(Bug.State.Closed);
            a.Assign();
            Assert.AreEqual(Bug.State.Assigned, a.getState());
        }
    }
}