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
        public void TestDefered()
        {
            var prime = new Bug(Bug.State.Assigned);
            prime.Defer();
            Assert.AreEqual(Bug.State.Defered, prime.getState());
        }

        [TestMethod]
        public void TestAssign()
        {
            var prime = new Bug(Bug.State.Assigned);
            prime.Assign();
            Assert.AreEqual(Bug.State.Assigned, prime.getState());
        }

        [TestMethod]
        public void TestAllFunc()
        {
            var prime = new Bug(Bug.State.Open);
            prime.Assign();
            prime.Defer();
            prime.Assign();
            prime.Close();
            Assert.AreEqual(Bug.State.Closed, prime.getState());
        }

        [TestMethod]
        public void TestClosedAss()
        {
            var prime = new Bug(Bug.Condition.Closed);
            prime.Assign();
            Bug.Condition static = prime.getCondition();
            Assert.AreEqual(Bug.Condition.Assigned, static);
        }

        [TestMethod]
        public void TestCloseException()
        {
            var prime = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => prime.Close());
        }

        [TestMethod]
        public void TestDeferThrowEXC()
        {
            var prime = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => prime.Defer());
        }

        [TestMethod]
        public void TestCloseExc2()
        {
            var prime = new Bug(Bug.State.Defered);
            Assert.ThrowsException<InvalidOperationException>(() => prime.Close());
        }

        [TestMethod]
        public void TestClosedAss2()
        {
            var prime = new Bug(Bug.State.Closed);
            prime.Assign();
            Assert.AreEqual(Bug.State.Assigned, prime.getState());
        }
    }
}
