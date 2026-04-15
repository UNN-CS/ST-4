using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stateless;
using BugPro;

namespace Tests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void Constructor_ShouldSetInitialState_New()
        {
            var bug = new Bug();
            Assert.AreEqual(BugState.New, bug.State);
        }

        [TestMethod]
        public void Start_FromNew_ShouldMoveToAnalysis()
        {
            var bug = new Bug();

            bug.Start();

            Assert.AreEqual(BugState.Analysis, bug.State);
        }

        [TestMethod]
        public void RequestInfo_FromAnalysis_ShouldMoveToMoreInfo()
        {
            var bug = new Bug();
            bug.Start();

            bug.RequestInfo();

            Assert.AreEqual(BugState.MoreInfo, bug.State);
        }

        [TestMethod]
        public void AddInfo_FromMoreInfo_ShouldMoveToAnalysis()
        {
            var bug = new Bug();
            bug.Start();
            bug.RequestInfo();

            bug.AddInfo();

            Assert.AreEqual(BugState.Analysis, bug.State);
        }

        [TestMethod]
        public void Fix_FromAnalysis_ShouldMoveToFixed()
        {
            var bug = new Bug();
            bug.Start();

            bug.Fix();

            Assert.AreEqual(BugState.Fixed, bug.State);
        }

        [TestMethod]
        public void SetNotBug_FromAnalysis_ShouldMoveToNotBug()
        {
            var bug = new Bug();
            bug.Start();

            bug.SetNotBug();

            Assert.AreEqual(BugState.NotBug, bug.State);
        }

        [TestMethod]
        public void SetNoFix_FromAnalysis_ShouldMoveToNoFix()
        {
            var bug = new Bug();
            bug.Start();

            bug.SetNoFix();

            Assert.AreEqual(BugState.NoFix, bug.State);
        }

        [TestMethod]
        public void SetDuplicate_FromAnalysis_ShouldMoveToDuplicate()
        {
            var bug = new Bug();
            bug.Start();

            bug.SetDuplicate();

            Assert.AreEqual(BugState.Duplicate, bug.State);
        }

        [TestMethod]
        public void SetNotRepro_FromAnalysis_ShouldMoveToNotRepro()
        {
            var bug = new Bug();
            bug.Start();

            bug.SetNotRepro();

            Assert.AreEqual(BugState.NotRepro, bug.State);
        }

        [TestMethod]
        public void SetSeparate_FromAnalysis_ShouldMoveToSeparate()
        {
            var bug = new Bug();
            bug.Start();

            bug.SetSeparate();

            Assert.AreEqual(BugState.Separate, bug.State);
        }

        [TestMethod]
        public void SetNoTime_FromAnalysis_ShouldMoveToNoTime()
        {
            var bug = new Bug();
            bug.Start();

            bug.SetNoTime();

            Assert.AreEqual(BugState.NoTime, bug.State);
        }

        [TestMethod]
        public void SetOther_FromAnalysis_ShouldMoveToOther()
        {
            var bug = new Bug();
            bug.Start();

            bug.SetOther();

            Assert.AreEqual(BugState.Other, bug.State);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Fix_FromNew_ShouldThrowException()
        {
            var bug = new Bug();
            bug.Fix();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddInfo_FromNew_ShouldThrowException()
        {
            var bug = new Bug();
            bug.AddInfo();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Start_FromAnalysis_ShouldThrowException()
        {
            var bug = new Bug();
            bug.Start();

            bug.Start();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RequestInfo_FromMoreInfo_ShouldThrowException()
        {
            var bug = new Bug();
            bug.Start();
            bug.RequestInfo();

            bug.RequestInfo();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetDuplicate_FromFixed_ShouldThrowException()
        {
            var bug = new Bug();
            bug.Start();
            bug.Fix();

            bug.SetDuplicate();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Fix_FromFixed_ShouldThrowException()
        {
            var bug = new Bug();
            bug.Start();
            bug.Fix();

            bug.Fix();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddInfo_FromAnalysis_ShouldThrowException()
        {
            var bug = new Bug();
            bug.Start();

            bug.AddInfo();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RequestInfo_FromNew_ShouldThrowException()
        {
            var bug = new Bug();

            bug.RequestInfo();
        }

        [TestMethod]
        public void FullScenario_RequestInfo_AddInfo_Fix()
        {
            var bug = new Bug();

            bug.Start();
            bug.RequestInfo();
            bug.AddInfo();
            bug.Fix();

            Assert.AreEqual(BugState.Fixed, bug.State);
        }

        [TestMethod]
        public void FullScenario_Start_SetNoFix()
        {
            var bug = new Bug();

            bug.Start();
            bug.SetNoFix();

            Assert.AreEqual(BugState.NoFix, bug.State);
        }
    }
}