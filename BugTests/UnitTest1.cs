using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stateless;
using System;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void TestInitialState()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.getState());
        }

        [TestMethod]
        public void TestOpenToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestOpenToBlocked()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Block();
            Assert.AreEqual(Bug.State.Blocked, bug.getState());
        }

        [TestMethod]
        public void TestAssignedToInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Start();
            Assert.AreEqual(Bug.State.InProgress, bug.getState());
        }

        [TestMethod]
        public void TestAssignedToDefered()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void TestAssignedToBlocked()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Block();
            Assert.AreEqual(Bug.State.Blocked, bug.getState());
        }

        [TestMethod]
        public void TestInProgressToResolved()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Start();
            bug.Resolve();
            Assert.AreEqual(Bug.State.Resolved, bug.getState());
        }

        [TestMethod]
        public void TestBlockedToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Block();
            bug.Unblock();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestResolvedToClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Start();
            bug.Resolve();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void TestDeferedToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void TestInvalidTransitionFromOpenToClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void TestInvalidTransitionFromOpenToResolved()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Resolve();
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void TestInvalidTransitionFromClosedToInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Start();
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void TestInvalidTransitionFromResolvedToInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Start();
            bug.Resolve();
            bug.Start();
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void TestInvalidTransitionFromBlockedToClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Block();
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void TestInvalidTransitionFromDeferedToResolved()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Resolve();
        }

        [TestMethod]
        public void TestMultipleAssignments()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Assign(); // Игнорируется
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestFullWorkflow()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.getState());
            
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
            
            bug.Start();
            Assert.AreEqual(Bug.State.InProgress, bug.getState());
            
            bug.Block();
            Assert.AreEqual(Bug.State.Blocked, bug.getState());
            
            bug.Unblock();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
            
            bug.Start();
            Assert.AreEqual(Bug.State.InProgress, bug.getState());
            
            bug.Resolve();
            Assert.AreEqual(Bug.State.Resolved, bug.getState());
            
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void TestAlternativeWorkflow()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.getState());
            
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
            
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
            
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
            
            bug.Start();
            Assert.AreEqual(Bug.State.InProgress, bug.getState());
            
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void TestInProgressToDefered()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Start();
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }
    }
} 