using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestBugClosed()
        {
            var a = new Bug(Bug.State.Assigned);
            a.Close();
            Assert.AreEqual(Bug.State.Closed, a.getState());
        }

        [TestMethod]
        public void TestBugDefered()
        {
            var a = new Bug(Bug.State.Assigned);
            a.Defer();
            Assert.AreEqual(Bug.State.Defered, a.getState());
        }

        [TestMethod]
        public void TestBugDeferedToAssigned()
        {
            var a = new Bug(Bug.State.Defered);
            a.Assign();
            Assert.AreEqual(Bug.State.Assigned, a.getState());
        }

        [TestMethod]
        public void TestBugAssignIgnoredInAssignedState()
        {
            var a = new Bug(Bug.State.Assigned);
            a.Assign();
            Assert.AreEqual(Bug.State.Assigned, a.getState());
        }

        [TestMethod]
        public void TestBugComplexScenario()
        {
            var a = new Bug(Bug.State.Open);
            a.Assign();
            a.Defer();
            a.Assign();
            a.Close();
            Assert.AreEqual(Bug.State.Closed, a.getState());
        }

        [TestMethod]
        public void TestBugOpenToAssigned()
        {
            var a = new Bug(Bug.State.Open);
            a.Assign();
            Assert.AreEqual(Bug.State.Assigned, a.getState());
        }

        [TestMethod]
        public void TestBugCloseFromOpenThrowsException()
        {
            var a = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => a.Close());
        }

        [TestMethod]
        public void TestBugDeferFromOpenThrowsException()
        {
            var a = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => a.Defer());
        }

        [TestMethod]
        public void TestBugCloseFromDeferredThrowsException()
        {
            var a = new Bug(Bug.State.Defered);
            Assert.ThrowsException<InvalidOperationException>(() => a.Close());
        }

        [TestMethod]
        public void TestBugClosedAndAssigned()
        {
            var a = new Bug(Bug.State.Closed);
            a.Assign();
            Assert.AreEqual(Bug.State.Assigned, a.getState());
        }
    }
}