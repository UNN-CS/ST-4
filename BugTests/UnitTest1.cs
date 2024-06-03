namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestInitializeStateIsOpen()
        {
            Assert.AreEqual(Bug.State.Open, (new Bug(Bug.State.Open)).getState());
        }

        [TestMethod]
        public void TestInitializeStateIsAssigned()
        {
            Assert.AreEqual(Bug.State.Assigned, (new Bug(Bug.State.Assigned)).getState());
        }

        [TestMethod]
        public void TestInitializeStateIsDefered()
        {
            Assert.AreEqual(Bug.State.Defered, (new Bug(Bug.State.Defered)).getState());
        }

        [TestMethod]
        public void TestInitializeStateIsClosed()
        {
            Assert.AreEqual(Bug.State.Closed, (new Bug(Bug.State.Closed)).getState());
        }

        [TestMethod]
        public void TestAssignOpenedBug()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestAssignDefereddBug()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestAssignAssignedBug()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestAssignClosedBug()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
        }

        [TestMethod]
        public void TestDeferAssignedBug()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

        [TestMethod]
        public void TestCloseAssignedBug()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        public void TestFullLifeBug()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.getState());
            bug.Close();
            Assert.AreEqual(Bug.State.Closed, bug.getState());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestCannotDeferOpenedBug()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Defer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestCannotDeferClosedBug()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Defer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestCannotCloseDeferedBug()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestCannotCloseOpenedBug()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Close();
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestCannotCloseClosedBug()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Close();
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDeferDeferedBug()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Defer();
            Assert.AreEqual(Bug.State.Defered, bug.getState());
        }

    }
}