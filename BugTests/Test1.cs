using BugPro;

namespace BugTests
{
    [TestClass]
    public sealed class BugTests
    {
        [TestMethod]
        public void NewBug_DefaultState_IsOpen()
        {
            var bug = new Bug();
            Assert.AreEqual(Bug.State.Open, bug.CurrentState);
        }

        [TestMethod]
        public void NewBug_WithExplicitState_UsesThatState()
        {
            var bug = new Bug(Bug.State.Assigned);
            Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
        }

        [TestMethod]
        public void Open_Assign_GoesToAssigned()
        {
            var bug = new Bug();
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
        }

        [TestMethod]
        public void Assigned_StartFix_GoesToInProgress()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.StartFix();
            Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
        }

        [TestMethod]
        public void InProgress_Resolve_GoesToResolved()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Resolve();
            Assert.AreEqual(Bug.State.Resolved, bug.CurrentState);
        }

        [TestMethod]
        public void Resolved_Verify_GoesToClosed()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Verify();
            Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void FullHappyPath_OpenToClosed()
        {
            var bug = new Bug();
            bug.Assign();
            bug.StartFix();
            bug.Resolve();
            bug.Verify();
            Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void Assigned_Defer_GoesToDeferred()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Deferred, bug.CurrentState);
        }

        [TestMethod]
        public void Deferred_Resume_GoesBackToAssigned()
        {
            var bug = new Bug(Bug.State.Deferred);
            bug.Resume();
            Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
        }

        [TestMethod]
        public void Assigned_Reject_GoesToRejected()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.CurrentState);
        }

        [TestMethod]
        public void Rejected_Reopen_GoesBackToAssigned()
        {
            var bug = new Bug(Bug.State.Rejected);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
        }

        [TestMethod]
        public void Rejected_Verify_GoesToClosed()
        {
            var bug = new Bug(Bug.State.Rejected);
            bug.Verify();
            Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void Assigned_CannotReproduce_GoesToNotReproducible()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.CannotReproduce();
            Assert.AreEqual(Bug.State.NotReproducible, bug.CurrentState);
        }

        [TestMethod]
        public void NotReproducible_Confirm_GoesToClosed()
        {
            var bug = new Bug(Bug.State.NotReproducible);
            bug.Confirm();
            Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void NotReproducible_Deny_GoesBackToAssigned()
        {
            var bug = new Bug(Bug.State.NotReproducible);
            bug.Deny();
            Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
        }

        [TestMethod]
        public void Resolved_Reopen_GoesToReopened()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
        }

        [TestMethod]
        public void Reopened_ReturnToTriage_GoesToAssigned()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.ReturnToTriage();
            Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
        }

        [TestMethod]
        public void Closed_Reopen_GoesToReopened()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
        }

        [TestMethod]
        public void ReopenedBug_CanBeFixedAndClosedAgain()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            bug.ReturnToTriage();
            bug.StartFix();
            bug.Resolve();
            bug.Verify();
            Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void CanFire_Open_AllowsAssign()
        {
            var bug = new Bug();
            Assert.IsTrue(bug.CanFire(Bug.Trigger.Assign));
        }

        [TestMethod]
        public void CanFire_Open_DoesNotAllowResolve()
        {
            var bug = new Bug();
            Assert.IsFalse(bug.CanFire(Bug.Trigger.Resolve));
        }

        [TestMethod]
        public void CanFire_Closed_AllowsReopenOnly()
        {
            var bug = new Bug(Bug.State.Closed);
            Assert.IsTrue(bug.CanFire(Bug.Trigger.Reopen));
            Assert.IsFalse(bug.CanFire(Bug.Trigger.Verify));
        }

        [TestMethod]
        public void Open_Resolve_Throws()
        {
            var bug = new Bug();
            Assert.ThrowsExactly<InvalidOperationException>(() => bug.Resolve());
        }

        [TestMethod]
        public void Open_StartFix_Throws()
        {
            var bug = new Bug();
            Assert.ThrowsExactly<InvalidOperationException>(() => bug.StartFix());
        }

        [TestMethod]
        public void Closed_Verify_Throws()
        {
            var bug = new Bug(Bug.State.Closed);
            Assert.ThrowsExactly<InvalidOperationException>(() => bug.Verify());
        }

        [TestMethod]
        public void Assigned_Resolve_Throws()
        {
            var bug = new Bug(Bug.State.Assigned);
            Assert.ThrowsExactly<InvalidOperationException>(() => bug.Resolve());
        }

        [TestMethod]
        public void InProgress_Verify_Throws()
        {
            var bug = new Bug(Bug.State.InProgress);
            Assert.ThrowsExactly<InvalidOperationException>(() => bug.Verify());
        }

        [TestMethod]
        public void Deferred_StartFix_Throws()
        {
            var bug = new Bug(Bug.State.Deferred);
            Assert.ThrowsExactly<InvalidOperationException>(() => bug.StartFix());
        }

        [TestMethod]
        public void Fire_InvalidTrigger_ExceptionMessageMentionsState()
        {
            var bug = new Bug();
            var ex = Assert.ThrowsExactly<InvalidOperationException>(() => bug.Resolve());
            StringAssert.Contains(ex.Message, "Open");
        }
    }
}
