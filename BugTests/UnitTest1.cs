using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BugStateMachine;

namespace BugTests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void Assign_From_Open() {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void StartProgress_From_Assigned() {
            var bug = new Bug(Bug.State.Assigned);
            bug.StartProgress();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        public void Close_From_Assigned() {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void Defer_From_Assigned() {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Deferred, bug.GetState());
        }

        [TestMethod]
        public void Assign_From_Deferred() {
            var bug = new Bug(Bug.State.Deferred);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Reopen_From_Closed() {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void Assign_From_Reopened() {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Close_From_InProgress() {
            var bug = new Bug(Bug.State.InProgress);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void Defer_From_InProgress() {
            var bug = new Bug(Bug.State.InProgress);
            bug.Defer();
            Assert.AreEqual(Bug.State.Deferred, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_Close_From_Open() {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_Reopen_From_Open() {
            var bug = new Bug(Bug.State.Open);
            bug.Reopen();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_Defer_From_Open() {
            var bug = new Bug(Bug.State.Open);
            bug.Defer();
        }

        [TestMethod]
        public void Ignore_Assign_From_Assigned() {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_Verify_Without_Transition() {
            var bug = new Bug(Bug.State.Open);
            bug.Verify();
        }

        [TestMethod]
        public void FullCycle_OpenToClosed() {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.StartProgress();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void Cycle_Open_Assigned_Deferred_Assigned() {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Reopen_Then_StartProgress() {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            bug.Assign();
            bug.StartProgress();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_Assign_In_Closed() {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
        }

        [TestMethod]
        public void MultipleDeferCycles() {
            var bug = new Bug(Bug.State.Assigned);
            for (int i = 0; i < 3; i++)
            {
                bug.Defer();
                bug.Assign();
            }
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void ComplexPathThroughAllStates() {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.StartProgress();
            bug.Defer();
            bug.Assign();
            bug.Close();
            bug.Reopen();
            bug.Assign();
            bug.StartProgress();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_Trigger_Order() {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
        }

        [TestMethod]
        public void Defer_From_Assigned_Then_Close_After_Reassignment() {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void Assign_From_Reopened_Then_Close() {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_Verify_From_Closed() {
            var bug = new Bug(Bug.State.Closed);
            bug.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_Trigger_In_Reopened() {
            var bug = new Bug(Bug.State.Reopened);
            bug.Close();
        }
    }
}
