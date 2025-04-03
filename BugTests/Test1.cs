using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Stateless;
using static Bug;

namespace BugTests {
    [TestClass]
    public class BugStateMachineTests {

        [TestMethod]
        public void assignFromOpenGoesToAssigned() {
            var bug = new Bug(State.Open);
            bug.Assign();
            Assert.AreEqual(State.Assigned, bug.getState());
        }

        [TestMethod]
        public void cancelFromOpenGoesToCanceled() {
            var bug = new Bug(State.Open);
            bug.Cancel();
            Assert.AreEqual(State.Canceled, bug.getState());
        }

        [TestMethod]
        public void deferFromAssignedGoesToDeferred() {
            var bug = new Bug(State.Assigned);
            bug.Defer();
            Assert.AreEqual(State.Defered, bug.getState());
        }

        [TestMethod]
        public void verifyFromAssignedGoesToVerified() {
            var bug = new Bug(State.Assigned);
            bug.Verify();
            Assert.AreEqual(State.Verified, bug.getState());
        }

        [TestMethod]
        public void rejectFromAssignedGoesToRejected() {
            var bug = new Bug(State.Assigned);
            bug.Reject();
            Assert.AreEqual(State.Rejected, bug.getState());
        }

        [TestMethod]
        public void assignFromRejectedReturnsToAssigned() {
            var bug = new Bug(State.Rejected);
            bug.Assign();
            Assert.AreEqual(State.Assigned, bug.getState());
        }

        [TestMethod]
        public void closeFromVerifiedGoesToClosed() {
            var bug = new Bug(State.Verified);
            bug.Close();
            Assert.AreEqual(State.Closed, bug.getState());
        }

        [TestMethod]
        public void assignFromClosedGoesToAssigned() {
            var bug = new Bug(State.Closed);
            bug.Assign();
            Assert.AreEqual(State.Assigned, bug.getState());
        }

        [TestMethod]
        public void assignFromDeferedGoesToAssigned() {
            var bug = new Bug(State.Defered);
            bug.Assign();
            Assert.AreEqual(State.Assigned, bug.getState());
        }

        [TestMethod]
        public void getStateReturnsCorrectState() {
            var bug = new Bug(State.Assigned);
            Assert.AreEqual(State.Assigned, bug.getState());
        }

        // Ошибочные переходы

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void deferFromOpenThrowsException() {
            var bug = new Bug(State.Open);
            bug.Defer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void closeFromOpenThrowsException() {
            var bug = new Bug(State.Open);
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void verifyFromOpenThrowsException() {
            var bug = new Bug(State.Open);
            bug.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void rejectFromOpenThrowsException() {
            var bug = new Bug(State.Open);
            bug.Reject();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void assignFromCanceledThrowsException() {
            var bug = new Bug(State.Canceled);
            bug.Assign();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void rejectFromClosedThrowsException() {
            var bug = new Bug(State.Closed);
            bug.Reject();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void resolveFromAssignedThrowsException() {
            var bug = new Bug(State.Assigned);
            bug.Close(); // allowed
            bug.Defer(); // not allowed from Closed
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void cancelFromAssignedThrowsException() {
            var bug = new Bug(State.Assigned);
            bug.Cancel();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void closeFromRejectedThrowsException() {
            var bug = new Bug(State.Rejected);
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void closeFromDeferredThrowsException() {
            var bug = new Bug(State.Defered);
            bug.Close();
        }

        // Сценарии

        [TestMethod]
        public void fullFlowToClosed() {
            var bug = new Bug(State.Open);
            bug.Assign();
            bug.Verify();
            bug.Close();
            Assert.AreEqual(State.Closed, bug.getState());
        }

        [TestMethod]
        public void deferAssignVerifyCloseFlow() {
            var bug = new Bug(State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.Verify();
            bug.Close();
            Assert.AreEqual(State.Closed, bug.getState());
        }

        [TestMethod]
        public void rejectAndReturn() {
            var bug = new Bug(State.Open);
            bug.Assign();
            bug.Reject();
            bug.Assign();
            Assert.AreEqual(State.Assigned, bug.getState());
        }

        [TestMethod]
        public void cancelImmediately() {
            var bug = new Bug(State.Open);
            bug.Cancel();
            Assert.AreEqual(State.Canceled, bug.getState());
        }

        [TestMethod]
        public void rejectThenCancelFails() {
            var bug = new Bug(State.Open);
            bug.Assign();
            bug.Reject();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Cancel());
        }

        [TestMethod]
        public void verifyThenReassignAndDefer() {
            var bug = new Bug(State.Open);
            bug.Assign();
            bug.Verify();
            bug.Assign();
            bug.Defer();
            Assert.AreEqual(State.Defered, bug.getState());
        }

        [TestMethod]
        public void assignTwiceFromAssignedIgnored() {
            var bug = new Bug(State.Assigned);
            bug.Assign();
            Assert.AreEqual(State.Assigned, bug.getState());
        }

        [TestMethod]
        public void verifyCloseThenReassignAgain() {
            var bug = new Bug(State.Open);
            bug.Assign();
            bug.Verify();
            bug.Close();
            bug.Assign();
            Assert.AreEqual(State.Assigned, bug.getState());
        }

        [TestMethod]
        public void rejectThenAssignAndVerifyThenClose() {
            var bug = new Bug(State.Open);
            bug.Assign();
            bug.Reject();
            bug.Assign();
            bug.Verify();
            bug.Close();
            Assert.AreEqual(State.Closed, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void cancelFromVerifiedThrows() {
            var bug = new Bug(State.Verified);
            bug.Cancel();
        }
    }
}
