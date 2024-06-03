using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests
{
    [TestClass]
    public class OpenStateTests {
	private Bug bug;

        [TestInitialize]
	public void Initialize() {
	    bug = new Bug(Bug.State.Open);
	}

	[TestMethod]
	public void TestAssign() {
            bug.Assign();
	    Assert.AreEqual(Bug.State.Assigned, bug.getState());
	}

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDeferShouldThrow()
        {
            bug.Defer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
	public void TestClose()
	{
            bug.Close();
	}
    }

    [TestClass]
    public class AssignedStateTests {
	private Bug bug;

        [TestInitialize]
	public void Initialize() {
	    bug = new Bug(Bug.State.Assigned);
	}

        [TestMethod]
        public void TestAssign()
        {
            bug.Assign();
	    Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestDefer()
        {
            bug.Defer();
	    Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void TestClose()
        {
            bug.Close();
	    Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

    }

    [TestClass]
    public class DeferedStateTests {
	private Bug bug;

        [TestInitialize]
	public void Initialize() {
	    bug = new Bug(Bug.State.Defered);
	}


        [TestMethod]
        public void TestAssign()
        {
            bug.Assign();
	    Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDeferShouldThrow()
        {
            bug.Defer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestCloseShouldThrow()
        {
            bug.Close();
        }

    }

    [TestClass]
    public class ClosedStateTests
    {
	private Bug bug;

        [TestInitialize]
	public void Initialize() {
	    bug = new Bug(Bug.State.Closed);
	}

        [TestMethod]
        public void TestAssign()
        {
            bug.Assign();
	    Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDeferShouldThrow()
        {
            bug.Defer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestClose()
        {
            bug.Close();
        }
    }
}
