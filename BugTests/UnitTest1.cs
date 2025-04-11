
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using Stateless;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void Open_To_Assigned_Should_Succeed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Assigned_To_InProgress_Should_Succeed()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Start();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        [TestMethod]
        public void InProgress_To_Resolved_Should_Succeed()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Resolve();
            Assert.AreEqual(Bug.State.Resolved, bug.GetState());
        }

        [TestMethod]
        public void Resolved_To_Closed_Should_Succeed()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void Closed_To_Open_Should_Succeed()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Open, bug.GetState());
        }

        [TestMethod]
        public void Assigned_To_Defered_Should_Succeed()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.GetState());
        }

        [TestMethod]
        public void Defered_To_Assigned_Should_Succeed()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void InProgress_To_OnHold_Should_Succeed()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Hold();
            Assert.AreEqual(Bug.State.OnHold, bug.GetState());
        }

        [TestMethod]
        public void OnHold_To_Assigned_Should_Succeed()
        {
            var bug = new Bug(Bug.State.OnHold);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Open_To_Rejected_Should_Succeed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.GetState());
        }

        [TestMethod]
        public void Rejected_To_Open_Should_Succeed()
        {
            var bug = new Bug(Bug.State.Rejected);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Open, bug.GetState());
        }

        [TestMethod]
        public void Open_To_Closed_Should_Succeed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void Assigned_To_OnHold_Should_Succeed()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Hold();
            Assert.AreEqual(Bug.State.OnHold, bug.GetState());
        }

        [TestMethod]
        public void Resolved_To_Rejected_Should_Succeed()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.GetState());
        }

        [TestMethod]
        public void Defered_To_Closed_Should_Succeed()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Start_From_Open_Should_Throw_Exception()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Start();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Resolve_From_Assigned_Should_Throw_Exception()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Resolve();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Close_From_InProgress_Should_Throw_Exception()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Reopen_From_Assigned_Should_Throw_Exception()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Reopen();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Hold_From_Open_Should_Throw_Exception()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Hold();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Reject_From_InProgress_Should_Throw_Exception()
        {
            var bug = new Bug(Bug.State.InProgress);
            bug.Reject();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Assign_From_Closed_Should_Throw_Exception()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Defer_From_Resolved_Should_Throw_Exception()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Defer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Resolve_From_Defered_Should_Throw_Exception()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Resolve();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Hold_From_Rejected_Should_Throw_Exception()
        {
            var bug = new Bug(Bug.State.Rejected);
            bug.Hold();
        }
    }
}