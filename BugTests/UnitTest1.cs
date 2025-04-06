using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro; // Пространство имен должно быть доступно
using Stateless;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestInitialState()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.getState());
        }

        [TestMethod]
        public void TestAssignFromOpen()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestCloseFromAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void TestDeferFromAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void TestReopenFromClosed()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.getState());
        }

        [TestMethod]
        public void TestAssignFromDefered()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestResolveFromAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Resolve();
            Assert.AreEqual(Bug.State.Resolved, bug.getState());
        }

        [TestMethod]
        public void TestReopenFromResolved()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void TestInvalidTransitionCloseFromOpen()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void TestInvalidTransitionDeferFromOpen()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Defer();
        }

        [TestMethod]
        public void TestMultipleTransitions()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Reopen();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestReopenAfterResolve()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.getState());
        }

        [TestMethod]
        public void TestAssignAfterReopen()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestDeferAfterReopen()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void TestCloseAfterReopen()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void TestResolveAfterReopen()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Resolve();
            Assert.AreEqual(Bug.State.Resolved, bug.getState());
        }

        [TestMethod]
        public void TestAssignAfterResolve()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestDeferAfterResolve()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void TestCloseAfterResolve()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void TestReopenAfterDefered()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.getState());
        }

        [TestMethod]
        public void TestReopenAfterClosed()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.getState());
        }

        [TestMethod]
        public void TestDeferAfterAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void TestFullCycle()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Resolve();
            bug.Reopen();
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }
    }
}
