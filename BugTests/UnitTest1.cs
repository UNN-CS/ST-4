using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void close_from_open()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
        }

        [TestMethod]
        public void to_deferred()
        {
            var a = new Bug(Bug.State.Assigned);
            a.Defer();
            Assert.AreEqual(Bug.State.Defered, a.getState());
        }

        [TestMethod]
        public void assign_ignored_in_assigned_state()
        {
            var a = new Bug(Bug.State.Assigned);
            a.Assign();
            Assert.AreEqual(Bug.State.Assigned, a.getState());
        }

        [TestMethod]
        public void all_scenaries()
        {
            var a = new Bug(Bug.State.Open);
            a.Assign();
            a.Defer();
            a.Assign();
            a.Close();
            Assert.AreEqual(Bug.State.Closed, a.getState());
        }

        [TestMethod]
        public void open_to_assigned()
        {
            var a = new Bug(Bug.State.Open);
            a.Assign();
            Assert.AreEqual(Bug.State.Assigned, a.getState());
        }

        [TestMethod]
        public void close_from_open_throws_exception()
        {
            var a = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => a.Close());
        }

        [TestMethod]
        public void defer_from_open_throws_exception()
        {
            var a = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => a.Defer());
        }

        [TestMethod]
        public void close_from_deferred_throws_exception()
        {
            var a = new Bug(Bug.State.Defered);
            Assert.ThrowsException<InvalidOperationException>(() => a.Close());
        }

        [TestMethod]
        public void closed_to_assigned()
        {
            var a = new Bug(Bug.State.Closed);
            a.Assign();
            Assert.AreEqual(Bug.State.Assigned, a.getState());
        }
    }
}