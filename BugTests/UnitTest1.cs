using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests
    {
        [TestClass]
        public class Tests
        {
            [TestMethod]
            public void TestAll()
            {
                var a = new Bug(Bug.State.Open);
                a.Assign();
                a.Defer();
                a.Assign();
                a.Close();
                Assert.AreEqual(Bug.State.Closed, a.getState());
            }

            [TestMethod]
            public void TestDef()
            {
                var a = new Bug(Bug.State.Assigned);
                a.Defer();
                Assert.AreEqual(Bug.State.Defered, a.getState());
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void TestStateOpen()
            {
                var bug = new Bug(Bug.State.Open);
                bug.Close();
            }

            [TestMethod]
            public void TestIgnoreAssign()
            {
                var a = new Bug(Bug.State.Assigned);
                a.Assign();
                Assert.AreEqual(Bug.State.Assigned, a.getState());
            }



            [TestMethod]
            public void TestCloseToAssign()
            {
                var a = new Bug(Bug.State.Closed);
                a.Assign();
                Assert.AreEqual(Bug.State.Assigned, a.getState());
            }


            [TestMethod]
            public void TestOpenToAssign()
            {
                var a = new Bug(Bug.State.Open);
                a.Assign();
                Assert.AreEqual(Bug.State.Assigned, a.getState());
            }

            [TestMethod]
            public void TestDeferThrowExc()
            {
                var a = new Bug(Bug.State.Open);
                Assert.ThrowsException<InvalidOperationException>(() => a.Defer());
            }

            [TestMethod]
            public void TestCloseThrowExc()
            {
                var a = new Bug(Bug.State.Open);
                Assert.ThrowsException<InvalidOperationException>(() => a.Close());
            }
            [TestMethod]
            public void TestCloseThrowExc2()
            {
                var a = new Bug(Bug.State.Open);
                Assert.ThrowsException<InvalidOperationException>(() => a.Close());
            }

            [TestMethod]
            public void TestCloseDeferThrowExc()
            {
                var a = new Bug(Bug.State.Defered);
                Assert.ThrowsException<InvalidOperationException>(() => a.Close());
            }
            [TestMethod]
            public void TestCloseDeferThrowExc2()
            {
                var a = new Bug(Bug.State.Defered);
                Assert.ThrowsException<InvalidOperationException>(() => a.Close());
            }

        }
    }
