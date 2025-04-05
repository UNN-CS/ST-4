namespace BugTests;
using BugLib;
[TestClass]
public sealed class Test1
{
    [TestMethod]
        public void InitialState_Open_IsCorrect()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.getState());
        }

        [TestMethod]
        public void Assign_FromOpen_TransitionsToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Close_FromAssigned_TransitionsToClosed()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Defer_FromAssigned_TransitionsToDefered()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void Assign_FromDefered_TransitionsToAssigned()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Assign_FromClosed_TransitionsToAssigned()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Review_FromAssigned_TransitionsToUnderReview()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Review();
            Assert.AreEqual(Bug.State.UnderReview, bug.getState());
        }

        [TestMethod]
        public void Resolve_FromUnderReview_TransitionsToResolved()
        {
            var bug = new Bug(Bug.State.UnderReview);
            bug.Resolve();
            Assert.AreEqual(Bug.State.Resolved, bug.getState());
        }

        [TestMethod]
        public void Close_FromResolved_TransitionsToClosed()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Assign_FromResolved_TransitionsToAssigned()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
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
        public void Review_FromDefered_ThrowsException()
        {
            var bug = new Bug(Bug.State.Defered);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Review());
        }

        [TestMethod]
        public void Resolve_FromAssigned_ThrowsException()
        {
            var bug = new Bug(Bug.State.Assigned);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Resolve());
        }

        [TestMethod]
        public void Assign_FromUnderReview_TransitionsToAssigned()
        {
            var bug = new Bug(Bug.State.UnderReview);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void MultipleTransitions_EndsInCorrectState()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Review();
            bug.Resolve();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void Assign_FromAssigned_Ignored()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign(); // Should be ignored
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void Defer_FromResolved_ThrowsException()
        {
            var bug = new Bug(Bug.State.Resolved);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
        }

        [TestMethod]
        public void Resolve_FromClosed_ThrowsException()
        {
            var bug = new Bug(Bug.State.Closed);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Resolve());
        }

        [TestMethod]
        public void Review_FromResolved_ThrowsException()
        {
            var bug = new Bug(Bug.State.Resolved);
            Assert.ThrowsException<InvalidOperationException>(() => bug.Review());
        }
}
