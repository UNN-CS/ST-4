using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stateless;
using System;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void Assign_FromOpen_ToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Close_FromAssigned_ToClosed()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Close_FromDefered_ThrowsException()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Close();
        }

        [TestMethod]
        public void Defer_FromAssigned_ToDefered()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void Assign_FromDefered_ToAssigned()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Assign_FromClosed_ToAssigned()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Reopen_FromClosed_ToReopened()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.getState());
        }

        [TestMethod]
        public void Assign_FromReopened_ToAssigned()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Close_FromReopened_ToClosed()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Verify_FromAssigned_ToVerified()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Verify();
            Assert.AreEqual(Bug.State.Verified, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Verify_FromOpen_ThrowsException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Verify();
        }

        [TestMethod]
        public void Close_FromVerified_ToClosed()
        {
            var bug = new Bug(Bug.State.Verified);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Defer_FromVerified_ToDefered()
        {
            var bug = new Bug(Bug.State.Verified);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Reopen_FromOpen_ThrowsException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Reopen();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Verify_FromDefered_ThrowsException()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Verify();
        }

        [TestMethod]
        public void ComplexFlow_OpenToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Verify();
            bug.Close();
            bug.Reopen();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Assign_FromAssigned_Ignored()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Defer_FromClosed_ThrowsException()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Defer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Verify_FromClosed_ThrowsException()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Verify();
        }

        [TestMethod]
        public void FullCycle_OpenToClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }
    }
}
