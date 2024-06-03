using BugPro;
using System.Collections.Concurrent;
using NUnit.Framework;

namespace BugTests;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void BugInitialStateIsOpen()
        {
            Bug bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.getState());
        }

        [TestMethod]
        public void BugAssignmentChangesStateToAssigned()
        {
            Bug bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void BugCanBeClosedFromAssignedState()
        {
            Bug bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void BugAssignmentIgnoredFromAssignedState()
        {
            Bug bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void BugCanBeDeferedFromAssignedState()
        {
            Bug bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void BugCanBeAssignedAgainAfterDeferral()
        {
            Bug bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void BugCanTransitionToClosedFromDeferedState()
        {
            Bug bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void BugCanTransitionToAssignedFromClosedState()
        {
            Bug bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void BugTransitionToAssignedFromClosedStateIsIgnored()
        {
            Bug bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            bug.Assign();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void BugTransitionToAssignedFromOpenStateIsIgnored()
        {
            Bug bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }
    }

