using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void Open_Assign_ToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Assigned_Close_ToClosed()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Open_Close_Throws()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Closed_Assign_Throws()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
        }

        [TestMethod]
        public void Resolved_Test_ToTesting()
        {
            var bug = new Bug(Bug.State.Resolved);
            bug.Test();
            Assert.AreEqual(Bug.State.Testing, bug.GetState());
        }

        [TestMethod]
        public void Testing_Reject_ToRejected()
        {
            var bug = new Bug(Bug.State.Testing);
            bug.Reject();
            Assert.AreEqual(Bug.State.Rejected, bug.GetState());
        }

        [TestMethod]
        public void FullWorkflow_OpenToClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Resolve();
            bug.Test();
            bug.Verify();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void Rejected_Reopen_ToReopened()
        {
            var bug = new Bug(Bug.State.Rejected);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void Defered_Assign_ToAssigned()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        public void Verified_Reopen_ToReopened()
        {
            var bug = new Bug(Bug.State.Verified);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void Closed_Reopen_ToReopened()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        [TestMethod]
        public void Reopened_Close_ToClosed()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void Assigned_Assign_NoChange()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Testing_Close_Throws()
        {
            var bug = new Bug(Bug.State.Testing);
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Rejected_Verify_Throws()
        {
            var bug = new Bug(Bug.State.Rejected);
            bug.Verify();
        }

        [TestMethod]
        public void ComplexWorkflow_WithReject()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Resolve();
            bug.Test();
            bug.Reject();
            bug.Assign();
            bug.Resolve();
            bug.Verify();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        public void InitialState_Open()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.GetState());
        }

        [TestMethod]
        public void InitialState_Closed()
        {
            var bug = new Bug(Bug.State.Closed);
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Defered_Close_Throws()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Reopened_Test_Throws()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Test();
        }

        [TestMethod]
        public void UltimateTest()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.Resolve();
            bug.Test();
            bug.Reject();
            bug.Reopen();
            bug.Assign();
            bug.Resolve();
            bug.Verify();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }
    }
}