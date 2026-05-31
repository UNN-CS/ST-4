using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_InitialState_IsNew()
        {
            var bug = new Bug();
            Assert.AreEqual(State.New, bug.CurrentState);
        }

        [TestMethod]
        public void Test_New_To_Open()
        {
            var bug = new Bug();
            bug.Assign();
            Assert.AreEqual(State.Open, bug.CurrentState);
        }

        [TestMethod]
        public void Test_New_To_Rejected()
        {
            var bug = new Bug();
            bug.Reject();
            Assert.AreEqual(State.Rejected, bug.CurrentState);
        }

        [TestMethod]
        public void Test_Open_To_InProgress()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Start();
            Assert.AreEqual(State.InProgress, bug.CurrentState);
        }

        [TestMethod]
        public void Test_Open_To_Closed()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Close();
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void Test_Open_To_Rejected()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Reject();
            Assert.AreEqual(State.Rejected, bug.CurrentState);
        }

        [TestMethod]
        public void Test_InProgress_To_Resolved()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Start();
            bug.Resolve();
            Assert.AreEqual(State.Resolved, bug.CurrentState);
        }

        [TestMethod]
        public void Test_InProgress_To_Closed()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Start();
            bug.Close();
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void Test_Resolved_To_Closed()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Start();
            bug.Resolve();
            bug.Close();
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void Test_Resolved_To_Reopened()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Start();
            bug.Resolve();
            bug.Reopen();
            Assert.AreEqual(State.Reopened, bug.CurrentState);
        }

        [TestMethod]
        public void Test_Closed_To_Reopened()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Close();
            bug.Reopen();
            Assert.AreEqual(State.Reopened, bug.CurrentState);
        }

        [TestMethod]
        public void Test_Reopened_To_Open()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Start();
            bug.Resolve();
            bug.Reopen();
            bug.Assign();
            Assert.AreEqual(State.Open, bug.CurrentState);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_Invalid_New_To_InProgress()
        {
            var bug = new Bug();
            bug.Start();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_Invalid_Closed_To_Resolved()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Close();
            bug.Resolve();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_Invalid_Rejected_To_Open()
        {
            var bug = new Bug();
            bug.Reject();
            bug.Assign();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_Invalid_New_To_Close()
        {
            var bug = new Bug();
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_Invalid_InProgress_To_Open()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Start();
            bug.Assign();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_Invalid_Reopened_To_Resolved()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Start();
            bug.Resolve();
            bug.Reopen();
            bug.Resolve();
        }

        [TestMethod]
        public void Test_Multiple_Transitions()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Start();
            bug.Resolve();
            bug.Close();
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void Test_Reopen_Cycle()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Start();
            bug.Resolve();
            bug.Reopen();
            bug.Assign();
            bug.Start();
            bug.Resolve();
            Assert.AreEqual(State.Resolved, bug.CurrentState);
        }
    }
}
