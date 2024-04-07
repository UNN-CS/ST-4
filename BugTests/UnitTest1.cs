namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestDeclineFixStateFromCreatedFixes()
        {
            var bug = new Bug(Bug.State.CreatedFixes);
            bug.DeclineFix();
            Assert.AreEqual(bug.getState(), Bug.State.DeclinedFixes);
        }

        [TestMethod]
        public void TestCloseStateFromAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Close();
            Assert.AreEqual(bug.getState(), Bug.State.Closed);
        }

        [TestMethod]
        public void TestCloseStateFromAcceptedFixes()
        {
            var bug = new Bug(Bug.State.AcceptedFixes);
            bug.Close();
            Assert.AreEqual(bug.getState(), Bug.State.Closed);
        }

        [TestMethod]
        public void TestAssignStateFromOpen()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(bug.getState(), Bug.State.Assigned);
        }

        [TestMethod]
        public void TestAssignStateFromClosed()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.Assign();
            Assert.AreEqual(bug.getState(), Bug.State.Assigned);
        }

        [TestMethod]
        public void TestAssignStateFromDefered()
        {
            var bug = new Bug(Bug.State.Defered);
            bug.Assign();
            Assert.AreEqual(bug.getState(), Bug.State.Assigned);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestAcceptFixStateFromClosed()
        {
            var bug = new Bug(Bug.State.Closed);
            bug.AcceptFix();
        }
        [TestMethod]
        public void TestDeferStateFromAssigned()
        {
            var bug = new Bug(Bug.State.Assigned);
            bug.Defer();
            Assert.AreEqual(bug.getState(), Bug.State.Defered);
        }

        [TestMethod]
        public void TestCreateFixStateFromDeclinedFixes()
        {
            var bug = new Bug(Bug.State.DeclinedFixes);
            bug.CreateFix();
            Assert.AreEqual(bug.getState(), Bug.State.CreatedFixes);
        }

        [TestMethod]
        public void TestAcceptFixStateFromCreatedFixes()
        {
            var bug = new Bug(Bug.State.CreatedFixes);
            bug.AcceptFix();
            Assert.AreEqual(bug.getState(), Bug.State.AcceptedFixes);
        }

    }
}
