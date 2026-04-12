using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace Tests
{
    
    [TestClass]
    public class BugTests
    {
        private static Bug InAnalysis()
        {
            var bug = new Bug();
            bug.Start();
            return bug;
        }

        private static Bug InNeedInfo()
        {
            var bug = InAnalysis();
            bug.RequestInfo();
            return bug;
        }

        [TestMethod]
        public void New_State_IsNew()
        {
            var bug = new Bug();
            Assert.AreEqual(BugState.New, bug.State);
        }

        [TestMethod]
        public void Start_FromNew_ToAnalysis()
        {
            var bug = new Bug();
            bug.Start();
            Assert.AreEqual(BugState.Analysis, bug.State);
        }

        [TestMethod]
        public void NeedInfo_FromAnalysis()
        {
            var bug = InAnalysis();
            bug.RequestInfo();
            Assert.AreEqual(BugState.MoreInfo, bug.State);
        }

        [TestMethod]
        public void AddInfo_BackToAnalysis()
        {
            var bug = InNeedInfo();
            bug.AddInfo();
            Assert.AreEqual(BugState.Analysis, bug.State);
        }

        [TestMethod]
        public void Fix_FromAnalysis()
        {
            var bug = InAnalysis();
            bug.Fix();
            Assert.AreEqual(BugState.Fixed, bug.State);
        }

        [TestMethod]
        public void NotBug_FromAnalysis()
        {
            var bug = InAnalysis();
            bug.SetNotBug();
            Assert.AreEqual(BugState.NotBug, bug.State);
        }

        [TestMethod]
        public void NoFix_FromAnalysis()
        {
            var bug = InAnalysis();
            bug.SetNoFix();
            Assert.AreEqual(BugState.NoFix, bug.State);
        }

        [TestMethod]
        public void Duplicate_FromAnalysis()
        {
            var bug = InAnalysis();
            bug.SetDuplicate();
            Assert.AreEqual(BugState.Duplicate, bug.State);
        }

        [TestMethod]
        public void NotRepro_FromAnalysis()
        {
            var bug = InAnalysis();
            bug.SetNotRepro();
            Assert.AreEqual(BugState.NotRepro, bug.State);
        }

        [TestMethod]
        public void Separate_FromAnalysis()
        {
            var bug = InAnalysis();
            bug.SetSeparate();
            Assert.AreEqual(BugState.Separate, bug.State);
        }

        [TestMethod]
        public void NoTime_FromAnalysis()
        {
            var bug = InAnalysis();
            bug.SetNoTime();
            Assert.AreEqual(BugState.NoTime, bug.State);
        }

        [TestMethod]
        public void Other_FromAnalysis()
        {
            var bug = InAnalysis();
            bug.SetOther();
            Assert.AreEqual(BugState.Other, bug.State);
        }

        [TestMethod]
        public void AddInfo_FromNew_Throws()
        {
            var bug = new Bug();
            Assert.ThrowsException<InvalidOperationException>(() => bug.AddInfo());
        }

        [TestMethod]
        public void Fix_FromNew_Throws()
        {
            var bug = new Bug();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Fix());
        }

        [TestMethod]
        public void Start_FromAnalysis_Throws()
        {
            var bug = InAnalysis();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Start());
        }

        [TestMethod]
        public void AddInfo_FromAnalysis_Throws()
        {
            var bug = InAnalysis();
            Assert.ThrowsException<InvalidOperationException>(() => bug.AddInfo());
        }

        [TestMethod]
        public void NeedInfo_FromNeedInfo_Throws()
        {
            var bug = InNeedInfo();
            Assert.ThrowsException<InvalidOperationException>(() => bug.RequestInfo());
        }

        [TestMethod]
        public void Duplicate_FromNeedInfo_Throws()
        {
            var bug = InNeedInfo();
            Assert.ThrowsException<InvalidOperationException>(() => bug.SetDuplicate());
        }

        [TestMethod]
        public void Fixed_IsTerminal()
        {
            var bug = InAnalysis();
            bug.Fix();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Start());
        }

        [TestMethod]
        public void NotBug_IsTerminal()
        {
            var bug = InAnalysis();
            bug.SetNotBug();
            Assert.ThrowsException<InvalidOperationException>(() => bug.RequestInfo());
        }

        [TestMethod]
        public void NoFix_IsTerminal()
        {
            var bug = InAnalysis();
            bug.SetNoFix();
            Assert.ThrowsException<InvalidOperationException>(() => bug.AddInfo());
        }

        [TestMethod]
        public void Duplicate_IsTerminal()
        {
            var bug = InAnalysis();
            bug.SetDuplicate();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Fix());
        }

        [TestMethod]
        public void NotRepro_IsTerminal()
        {
            var bug = InAnalysis();
            bug.SetNotRepro();
            Assert.ThrowsException<InvalidOperationException>(() => bug.RequestInfo());
        }

        [TestMethod]
        public void Separate_IsTerminal()
        {
            var bug = InAnalysis();
            bug.SetSeparate();
            Assert.ThrowsException<InvalidOperationException>(() => bug.AddInfo());
        }

        [TestMethod]
        public void NoTime_IsTerminal()
        {
            var bug = InAnalysis();
            bug.SetNoTime();
            Assert.ThrowsException<InvalidOperationException>(() => bug.SetOther());
        }

        [TestMethod]
        public void Other_IsTerminal()
        {
            var bug = InAnalysis();
            bug.SetOther();
            Assert.ThrowsException<InvalidOperationException>(() => bug.Fix());
        }

        [TestMethod]
        public void Flow_Info_Back_ToAnalysis()
        {
            var bug = InAnalysis();
            bug.RequestInfo();
            bug.AddInfo();
            Assert.AreEqual(BugState.Analysis, bug.State);
        }

        [TestMethod]
        public void Flow_Info_To_Fixed()
        {
            var bug = InAnalysis();
            bug.RequestInfo();
            bug.AddInfo();
            bug.Fix();
            Assert.AreEqual(BugState.Fixed, bug.State);
        }
    }
}