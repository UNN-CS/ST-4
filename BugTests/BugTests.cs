using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BugPro;
using Stateless;

namespace BugTests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void Test_Open_To_Assigned() {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Test_Assigned_To_Closed() {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Test_Assigned_To_Defered() {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void Test_Closed_To_Assigned() {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Test_Defered_To_Assigned() {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Test_Assign_Ignored_In_Assigned() {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState()); // should remain Assigned
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_Close_From_Open_Throws() {
            var bug = new Bug(Bug.State.Open);
            bug.Close(); // Not allowed
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_Defer_From_Open_Throws() {
            var bug = new Bug(Bug.State.Open);
            bug.Defer(); // Not allowed
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_Close_From_Defered_Throws() {
            var bug = new Bug(Bug.State.Defered);
            bug.Close(); // Not allowed
        }

        [TestMethod]
        public void Test_Multiple_Transitions() {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();    // Assigned
            bug.Defer();     // Defered
            bug.Assign();    // Assigned
            bug.Close();     // Closed
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Test_Initial_State() {
            var bug = new Bug(Bug.State.Defered);
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void Test_Defer_From_Assigned_And_Then_Assign() {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Test_Close_Then_Assign() {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Test_Assign_Multiple_Times_From_Closed() {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            bug.Assign(); // ignored
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_Defer_From_Closed_Throws() {
            var bug = new Bug(Bug.State.Closed);
            bug.Defer(); // Not configured
        }

        [TestMethod]
        public void Test_Valid_Chain() {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.Close();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Test_Assign_From_Defered() {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_Invalid_Transition_Throws() {
            var bug = new Bug(Bug.State.Open);
            bug.Defer(); // Invalid
        }

        [TestMethod]
        public void Test_Close_After_Assign() {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Test_Reopen_Bug() {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }
    }
}