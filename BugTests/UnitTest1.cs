using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stateless;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void NewBug_ShouldStartInSpecifiedState()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.getState());
        }

        [TestMethod]
        public void Assign_FromOpen_ChangesToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Close_FromAssigned_ChangesToClosed()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Defer_FromAssigned_ChangesToDefered()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void Reopen_FromClosed_ChangesToReopened()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.getState());
        }

        [TestMethod]
        public void Verify_FromAssigned_ChangesToVerified()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Verify();
            Assert.AreEqual(Bug.State.Verified, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Close_FromOpen_ThrowsException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Verify_FromOpen_ThrowsException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Verify();
        }

        [TestMethod]
        public void Assign_FromReopened_ChangesToAssigned()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Close_FromVerified_ChangesToClosed()
        {
            var bug = new Bug(Bug.State.Verified);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
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
        public void ComplexWorkflow_OpenToClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Verify();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void ComplexWorkflow_WithReopen()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.Close();
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.getState());
        }

        [TestMethod]
        public void Close_FromVerified_WorksCorrectly()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Verify();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Assign_FromDefered_ChangesToAssigned()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Close_FromReopened_ThrowsException()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Close();
        }

        [TestMethod]
        public void Verify_FromReopened_ChangesToVerified()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            bug.Verify();
            Assert.AreEqual(Bug.State.Verified, bug.getState());
        }

        [TestMethod]
        public void MultipleTransitions_EndToEnd()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.Verify();
            bug.Close();
            bug.Reopen();
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Reopen_FromVerified_ThrowsException()
        {
            var bug = new Bug(Bug.State.Verified);
            bug.Reopen();
        }
    }
}