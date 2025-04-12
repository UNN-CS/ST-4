using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using System;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void InitialState_IsCorrect()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.GetState());
        }

        [TestMethod]
        public void Assign_FromOpen_TransitionsToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Close_FromAssigned_TransitionsToClosed()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void Assign_FromClosed_TransitionsToAssigned()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Defer_FromAssigned_TransitionsToDefered()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.GetState());
        }

        [TestMethod]
        public void Assign_FromDefered_TransitionsToAssigned()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Test_FromAssigned_TransitionsToTesting()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Test();
            Assert.AreEqual(Bug.State.Testing, bug.GetState());
        }

        [TestMethod]
        public void Verify_FromTesting_TransitionsToClosed()
        {
            var bug = new Bug(Bug.State.Testing);
            bug.Verify();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void Reopen_FromTesting_TransitionsToReopened()
        {
            var bug = new Bug(Bug.State.Testing);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void Assign_FromReopened_TransitionsToAssigned()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Reopen_FromClosed_TransitionsToReopened()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void Close_FromOpen_ThrowsException()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
        }

        [TestMethod]
        public void Defer_FromClosed_ThrowsException()
        {
            var bug = new Bug(Bug.State.Closed);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
        }

        [TestMethod]
        public void Test_FromOpen_ThrowsException()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Test());
        }

        [TestMethod]
        public void Verify_FromAssigned_ThrowsException()
        {
            var bug = new Bug(Bug.State.Assigned);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Verify());
        }

        [TestMethod]
        public void Reopen_FromAssigned_ThrowsException()
        {
            var bug = new Bug(Bug.State.Assigned);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Reopen());
        }

        [TestMethod]
        public void Assign_FromAssigned_Ignored()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Test_FromDefered_ThrowsException()
        {
            var bug = new Bug(Bug.State.Defered);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Test());
        }

        [TestMethod]
        public void Verify_FromReopened_ThrowsException()
        {
            var bug = new Bug(Bug.State.Reopened);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Verify());
        }

        [TestMethod]
        public void Reopen_FromDefered_ThrowsException()
        {
            var bug = new Bug(Bug.State.Defered);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Reopen());
        }

        [TestMethod]
        public void Assign_FromTesting_ThrowsException()
        {
            var bug = new Bug(Bug.State.Testing);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Assign());
        }
    }
}