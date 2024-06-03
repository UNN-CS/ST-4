
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void TestStateClosed()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void TestStateDeffered()
        {

            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void TestStateDefferedToAssigned()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestAssignIgnoreAssign()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestRunAll()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void TestStateOpenToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }
        [TestMethod]
        public void TestCloseToDefferedThrowException()
        {
            var bug = new Bug(Bug.State.Defered);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
        }

        [TestMethod]
        public void TestCloseThrowException()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
        }

        [TestMethod]
        public void TestDefferThrowException()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
        }


        [TestMethod]
        public void TestClosedInAssigned()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestClosedInAssigned2()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }
    }
}