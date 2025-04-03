using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using Stateless;

namespace BugTests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void assignFromOpenShouldGoToAssigned() {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void closeFromAssignedShouldGoToClosed() {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void deferFromAssignedShouldGoToDeferred() {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void assignFromDeferredShouldGoToAssigned() {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void assignFromClosedShouldGoToAssigned() {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void repeatedAssignInAssignedShouldStayAssigned() {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign(); // ignored
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void closeFromOpenShouldThrowException() {
            var bug = new Bug(Bug.State.Open);
            bug.Close(); // not allowed
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void deferFromOpenShouldThrowException() {
            var bug = new Bug(Bug.State.Open);
            bug.Defer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void closeFromDeferredShouldThrowException() {
            var bug = new Bug(Bug.State.Defered);
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void deferFromDeferredShouldThrowException() {
            var bug = new Bug(Bug.State.Defered);
            bug.Defer();
        }

        [TestMethod]
        public void multipleAssignCloseAssignTransitions() {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void multipleAssignDeferAssignTransitions() {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void assignDeferAssignCloseSequence() {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void getStateShouldReturnCorrectState() {
            var bug = new Bug(Bug.State.Assigned);
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void assignTwiceShouldStayAssigned() {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Assign(); // Ignored
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void closeAfterAssignFromOpenShouldBeClosed() {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void deferAfterAssignFromOpenShouldBeDeferred() {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void assignAfterCloseShouldBeAssigned() {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void assignAfterDeferShouldBeAssigned() {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void closeAfterAssignAfterDeferAfterAssign() {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void reopenFromClosedShouldGoToReopened() {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.getState());
        }

        [TestMethod]
        public void assignAfterReopenShouldGoToAssigned() {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void reopenFromOpenShouldThrowException() {
            var bug = new Bug(Bug.State.Open);
            bug.Reopen(); // Не допускается
        }
    }
}
