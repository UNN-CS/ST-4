using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void Assign_FromOpen_ChangesStateToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Close_FromAssigned_ChangesStateToClosed()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Assign_FromAssigned_StateRemainsAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Close_FromOpen_ThrowsException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
        }

        [TestMethod]
        public void Reopen_FromClosed_ChangesStateToReopened()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Defer_FromOpen_ThrowsException()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Defer();
        }

        [TestMethod]
        public void Assign_FromDefered_ChangesStateToAssigned()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Close_FromDefered_ThrowsException()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Close();
        }

        [TestMethod]
        public void Assign_FromClosed_ChangesStateToAssigned()
        {
            var bug = new Bug(Bug.State.Closed);
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
        public void Reopen_FromReopened_ThrowsException()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Reopen();
        }

        [TestMethod]
        public void Close_FromReopened_ChangesStateToClosed()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Defer_FromReopened_ThrowsException()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Defer();
        }

        [TestMethod]
        public void InitialState_WhenCreatedWithDefered_IsCorrect()
        {
            var bug = new Bug(Bug.State.Defered);
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void MultipleTransitions_FromReopened_ToClosed()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
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
        public void Assign_FromClosedToAssignedAndCloseAgain_Valid()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void ComplexFlow_Open_Assigned_Defered_Assigned_Closed_Reopened()
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
        public void Reopened_CanTransitionThroughAssignedToDefered()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Close_FromClosed_ThrowsException()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Close();
        }
    }
}