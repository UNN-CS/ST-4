using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void TestAssign()
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
            Assert.AreEqual(Bug.State.Deferred, bug.getState());
        }

        [TestMethod]
        public void TestNothingFromClosed()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void TestReassignFromDeferred()
        {
            var bug = new Bug(Bug.State.Deferred);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestIgnoreAssignInAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestInvalidDeferFromOpen()
        {
            var bug = new Bug(Bug.State.Open);
            try
            {
                bug.Defer();
            }
            catch (InvalidOperationException)
            {
                Assert.AreEqual(Bug.State.Open, bug.getState());
            }
        }

        [TestMethod]
        public void TestInvalidCloseFromOpen()
        {
            var bug = new Bug(Bug.State.Open);
            try
            {
                bug.Close();
            }
            catch (InvalidOperationException)
            {
                Assert.AreEqual(Bug.State.Open, bug.getState());
            }
        }

        [TestMethod]
        public void TestMultipleTransitions()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }
    }
}
