using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void ExpectOk_Constructor()
        {
            Bug bug = new Bug(Bug.State.Open);
        }

        [TestMethod]
        public void ExpectOk_GetState()
        {
            Bug bug = new Bug(Bug.State.Open);
	    Assert.AreEqual(Bug.State.Open, bug.getState());
        }

        [TestMethod]
        public void ExpectOk_AssignFromOpen()
        {
            Bug bug = new Bug(Bug.State.Open);
            bug.Assign();
	    Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExpectError_DeferFromOpen()
        {
            Bug bug = new Bug(Bug.State.Open);
            bug.Defer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExpectError_CloseFromOpen()
        {
            Bug bug = new Bug(Bug.State.Open);
            bug.Close();
        }

        [TestMethod]
        public void ExpectOk_AssignFromAssigned()
        {
            Bug bug = new Bug(Bug.State.Assigned);
            bug.Assign();
	    Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void ExpectOk_DeferFromAssigned()
        {
            Bug bug = new Bug(Bug.State.Assigned);
            bug.Defer();
	    Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void ExpectOk_CloseFromAssigned()
        {
            Bug bug = new Bug(Bug.State.Assigned);
            bug.Close();
	    Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void ExpectOk_AssignFromDefered()
        {
            Bug bug = new Bug(Bug.State.Defered);
            bug.Assign();
	    Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExpectError_DeferFromDefered()
        {
            Bug bug = new Bug(Bug.State.Defered);
            bug.Defer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExpectError_CloseFromDefered()
        {
            Bug bug = new Bug(Bug.State.Defered);
            bug.Close();
        }

        [TestMethod]
        public void ExpectOk_AssignFromClosed()
        {
            Bug bug = new Bug(Bug.State.Closed);
            bug.Assign();
	    Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExpectError_DeferFromClosed()
        {
            Bug bug = new Bug(Bug.State.Closed);
            bug.Defer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExpectError_CloseFromClosed()
        {
            Bug bug = new Bug(Bug.State.Closed);
            bug.Close();
        }
    }
}
