using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestClose()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
        }

        [TestMethod]
        public void TestDeff()
        {
            var a = new Bug(Bug.State.Assigned);
            a.Defer();
            Assert.AreEqual(Bug.State.Defered, a.getState());
        }

        [TestMethod]
        public void TestAssign()
        {
            var a = new Bug(Bug.State.Assigned);
            a.Assign();
            Assert.AreEqual(Bug.State.Assigned, a.getState());
        }

        [TestMethod]
        public void TestAll()
        {
            var a = new Bug(Bug.State.Open);
            a.Assign();
            a.Defer();
            a.Assign();
            a.Close();
            Assert.AreEqual(Bug.State.Closed, a.getState());
        }

        [TestMethod]
        public void TestOpenScene()
        {
            var a = new Bug(Bug.State.Open);
            a.Assign();
            Assert.AreEqual(Bug.State.Assigned, a.getState());
        }

        [TestMethod]
        public void TestCloseThrows()
        {
            var a = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => a.Close());
        }

        [TestMethod]
        public void TestDeffOpen()
        {
            var a = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => a.Defer());
        }

        [TestMethod]
        public void TestCloserDeff()
        {
            var a = new Bug(Bug.State.Defered);
            Assert.ThrowsException<InvalidOperationException>(() => a.Close());
        }

        [TestMethod]
        public void TestClodeAssign()
        {
            var a = new Bug(Bug.State.Closed);
            a.Assign();
            Assert.AreEqual(Bug.State.Assigned, a.getState());
        }
    }
}