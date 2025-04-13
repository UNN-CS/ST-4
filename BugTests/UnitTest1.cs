using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void InitialBugCorrect()
        {
            var bug = new Bug(Bug.State.Defered);
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }
      
        [TestMethod]
        public void AssignBug()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void CloseBug()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void AssignBugFirst()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CloseBugFirst()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
        }

        [TestMethod]
        public void ReopenBug()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeferBug()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Defer();
        }

        [TestMethod]
        public void AssignBugSecend()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CloseBugSecend()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Close();
        }

        [TestMethod]
        public void Assign_Closed_Bug()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeferBug()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Defer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ReopenBug()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Reopen();
        }

        [TestMethod]
        public void Close_Reopened_Bug()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Defer_Reopened_Bug()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Defer();
        }

        [TestMethod]
        public void TestingBugAC()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestingBugR()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Reopen();
        }

        [TestMethod]
        public void TestingBugAC_Valid()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void TestingBugADACR()
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
        public void TestingBugAD()
        {
            var bug = new Bug(Bug.State.Reopened);
            bug.Assign();
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestingBugC()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Close();
        }
    }
}
