using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Bug_InitialState_ShouldBeOpen()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.getState());
        }

        [TestMethod]
        public void Bug_CanAssignFromOpen()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Bug_CanCloseFromAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Bug_CanDeferFromAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Deferred, bug.getState());
        }

        [TestMethod]
        public void Bug_CanReopenFromClosed()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Open, bug.getState()); // ѕосле повторного открыти€, состо€ние должно быть "Open"
        }

        [TestMethod]
        public void Bug_CanAssignFromReopened()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Bug_CanCloseFromReopened()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Bug_CanCloseDirectlyFromOpen()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Bug_AssignDoesNothingInClosed()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Bug_CanCloseFromDeferred()
        {
            var bug = new Bug(Bug.State.Deferred);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Bug_CanAssignFromDeferred()
        {
            var bug = new Bug(Bug.State.Deferred);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }
    }
}
