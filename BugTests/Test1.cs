using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using System;


namespace BugTests
{
    [TestClass]
    public class BugWorkflowTests
    {
        [TestMethod]
        public void Test1()
        {
            var bug = new Bug();
            Assert.AreEqual(State.New, bug.CurrentState);
        }

        [TestMethod]
        public void Test2()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            Assert.AreEqual(State.Analysis, bug.CurrentState);
        }

        [TestMethod]
        public void Test3()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.Reject);
            Assert.AreEqual(State.Rejected, bug.CurrentState);
        }

        [TestMethod]
        public void Test4()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.NeedInfo);
            Assert.AreEqual(State.MoreInfoRequired, bug.CurrentState);
        }

        [TestMethod]
        public void Test5()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.NeedInfo);
            bug.Fire(Trigger.ReturnToAnalysis);
            Assert.AreEqual(State.Analysis, bug.CurrentState);
        }

        [TestMethod]
        public void Test6()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            Assert.AreEqual(State.Fix, bug.CurrentState);
        }

        [TestMethod]
        public void Test7()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.Resolve);
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void Test8()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.ReturnToAnalysis);
            Assert.AreEqual(State.Analysis, bug.CurrentState);
        }

        [TestMethod]
        public void Test9()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.Resolve);
            bug.Fire(Trigger.Reopen);
            Assert.AreEqual(State.Analysis, bug.CurrentState);
        }

        [TestMethod]
        public void Test10()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.NeedInfo);
            bug.Fire(Trigger.ReturnToAnalysis);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.Resolve);
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test11_Fail()
        {
            var bug = new Bug();
            bug.Fire(Trigger.StartFix);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test12_Fail()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Resolve);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test13_Fail()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.Reject);
            bug.Fire(Trigger.Resolve);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test14_Fail()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.Resolve);
            bug.Fire(Trigger.StartFix);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test15_Fail()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.NeedInfo);
            bug.Fire(Trigger.StartFix);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test16_Fail()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.Reopen);
        }

        [TestMethod]
        public void Test17()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.Resolve);
            bug.Fire(Trigger.Reopen);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.Resolve);
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test18_Fail()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.Assign);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test19_Fail()
        {
            var bug = new Bug();
            bug.Fire(Trigger.ReturnToAnalysis);
        }

        [TestMethod]
        public void Test20()
        {
            var bug = new Bug();
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.Reject);
            Assert.AreEqual(State.Rejected, bug.CurrentState);
        }
    }
}