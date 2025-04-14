using ST_4;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_New_Bug()
        {
            var bug = new Bug();
            Assert.AreEqual(bug.getState(), Bug.State.Open);
        }
        
        [TestMethod]
        public void Test_Open_Assign()
        {
            var bug = new Bug();
            bug.Assign();
            Assert.AreEqual(bug.getState(), Bug.State.Assigned);
        }

        [TestMethod]
        public void Test_Cloded_Assign()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Close();
            bug.Assign();
            Assert.AreEqual(bug.getState(), Bug.State.Assigned);
        }

        [TestMethod]
        public void Test_Solved_Assign()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Solve();
            bug.Assign();
            Assert.AreEqual(bug.getState(), Bug.State.Assigned);
        }

        [TestMethod]
        public void Test_NSolved_Assign()
        {
            var bug = new Bug();
            bug.Assign();
            bug.N_Solve();
            bug.Assign();
            Assert.AreEqual(bug.getState(), Bug.State.Assigned);
        }

        [TestMethod]
        public void Test_Deffred_Assign()
        {
            var bug = new Bug();
            bug.Assign();
            bug.N_Solve();
            bug.Defer();
            bug.Assign();
            Assert.AreEqual(bug.getState(), Bug.State.Assigned);
        }

        [TestMethod]
        public void Test_NSolved_Deffred()
        {
            var bug = new Bug();
            bug.Assign();
            bug.N_Solve();
            bug.Defer();
            Assert.AreEqual(bug.getState(), Bug.State.Defered);
        }

        [TestMethod]
        public void Test_Assigned_NSolved()
        {
            var bug = new Bug();
            bug.Assign();
            bug.N_Solve();
            Assert.AreEqual(bug.getState(), Bug.State.N_solved);
        }

        [TestMethod]
        public void Test_Assigned_Solved()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Solve();
            Assert.AreEqual(bug.getState(), Bug.State.Solved);
        }

        [TestMethod]
        public void Test_Deffered_Solved()
        {
            var bug = new Bug();
            bug.Assign();
            bug.N_Solve();
            bug.Defer();
            bug.Solve();
            Assert.AreEqual(bug.getState(), Bug.State.Solved);
        }

        [TestMethod]
        public void Test_Assigned_Closed()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Close();
            Assert.AreEqual(bug.getState(), Bug.State.Closed);
        }

        [TestMethod]
        public void Test_Solved_Closed()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Solve();
            bug.Close();
            Assert.AreEqual(bug.getState(), Bug.State.Closed);
        }

        [TestMethod]
        public void Test_NSolved_Closed()
        {
            var bug = new Bug();
            bug.Assign();
            bug.N_Solve();
            bug.Close();
            Assert.AreEqual(bug.getState(), Bug.State.Closed);
        }

        [TestMethod]
        public void Throws_Open_Close()
        {
            var bug = new Bug();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
        }

        [TestMethod]
        public void Throws_Open_Solve()
        {
            var bug = new Bug();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Solve());
        }


        [TestMethod]
        public void Throws_Open_NSolve()
        {
            var bug = new Bug();
            Assert.ThrowsException<InvalidOperationException>(() => bug.N_Solve());
        }

        [TestMethod]
        public void Throws_Open_Deffred()
        {
            var bug = new Bug();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
        }

        [TestMethod]
        public void Throws_Close_Solve()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Close();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Solve());
        }

        [TestMethod]
        public void Throws_Close_NSolve()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Close();
            Assert.ThrowsException<InvalidOperationException>(() => bug.N_Solve());
        }

        [TestMethod]
        public void Throws_Close_Deffre()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Close();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
        }

        [TestMethod]
        public void Throws_Solved_NSolve()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Solve();
            Assert.ThrowsException<InvalidOperationException>(() => bug.N_Solve());
        }

        [TestMethod]
        public void Throws_Solved_Deffere()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Solve();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
        }

        [TestMethod]
        public void Throws_NSolved_Solved()
        {
            var bug = new Bug();
            bug.Assign();
            bug.N_Solve();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Solve());
        }

        [TestMethod]
        public void Throws_Close_Close()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Close();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
        }

        [TestMethod]
        public void Throws_Assigned_Assigned()
        {
            var bug = new Bug();
            bug.Assign();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Assign());
        }
    }
}