using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void Verify_InitialState_Is_NewDefect()
        {
            var bug = new Bug();
            Assert.AreEqual(BugState.NewDefect, bug.State);
        }

        [TestMethod]
        public void Transition_Start_MovesTo_Analysis()
        {
            var bug = new Bug();
            bug.Start();
            Assert.AreEqual(BugState.Analysis, bug.State);
        }

        [TestMethod]
        public void Transition_RequestInfo_MovesTo_MoreInfo()
        {
            var bug = new Bug();
            bug.Start();

            bug.RequestInfo();
            Assert.AreEqual(BugState.MoreInfo, bug.State);
        }

        [TestMethod]
        public void Transition_AddInfo_ReturnsTo_Analysis()
        {
            var bug = new Bug();
            bug.Start();
            bug.RequestInfo();

            bug.AddInfo();
            Assert.AreEqual(BugState.Analysis, bug.State);
        }

        [TestMethod]
        public void Transition_Fix_MovesTo_Fixed()
        {
            var bug = new Bug();
            bug.Start();

            bug.Fix();
            Assert.AreEqual(BugState.Fixed, bug.State);
        }

        [TestMethod]
        public void Transition_SetNotBug_MovesTo_NoDefect()
        {
            var bug = new Bug();
            bug.Start();

            bug.SetNotBug();
            Assert.AreEqual(BugState.NoDefect, bug.State);
        }

        [TestMethod]
        public void Transition_SetNoFix_MovesTo_NoFix()
        {
            var bug = new Bug();
            bug.Start();

            bug.SetNoFix();
            Assert.AreEqual(BugState.NoFix, bug.State);
        }

        [TestMethod]
        public void Transition_SetDuplicate_MovesTo_Duplicate()
        {
            var bug = new Bug();
            bug.Start();

            bug.SetDuplicate();
            Assert.AreEqual(BugState.Duplicate, bug.State);
        }

        [TestMethod]
        public void Transition_SetNotRepro_MovesTo_NoReproducible()
        {
            var bug = new Bug();
            bug.Start();

            bug.SetNotRepro();
            Assert.AreEqual(BugState.NoReproducible, bug.State);
        }

        [TestMethod]
        public void Transition_SetSeparate_MovesTo_SeparateSolution()
        {
            var bug = new Bug();
            bug.Start();

            bug.SetSeparate();
            Assert.AreEqual(BugState.SeparateSolution, bug.State);
        }

        [TestMethod]
        public void Transition_SetNoTime_MovesTo_NoTime()
        {
            var bug = new Bug();
            bug.Start();

            bug.SetNoTime();
            Assert.AreEqual(BugState.NoTime, bug.State);
        }

        [TestMethod]
        public void Transition_SetOther_MovesTo_OtherProduct()
        {
            var bug = new Bug();
            bug.Start();

            bug.SetOther();
            Assert.AreEqual(BugState.OtherProduct, bug.State);
        }

        [TestMethod]
        public void Exception_AddInfo_From_NewDefect_Throws()
        {
            var bug = new Bug();
            Assert.Throws<InvalidOperationException>(() => bug.AddInfo());
        }

        [TestMethod]
        public void Exception_Fix_From_NewDefect_Throws()
        {
            var bug = new Bug();
            Assert.Throws<InvalidOperationException>(() => bug.Fix());
        }

        [TestMethod]
        public void Exception_Start_When_Already_InAnalysis_Throws()
        {
            var bug = new Bug();
            bug.Start();

            Assert.Throws<InvalidOperationException>(() => bug.Start());
        }

        [TestMethod]
        public void Exception_AddInfo_When_InAnalysis_Throws()
        {
            var bug = new Bug();
            bug.Start();

            Assert.Throws<InvalidOperationException>(() => bug.AddInfo());
        }

        [TestMethod]
        public void Exception_RequestInfo_When_Already_InMoreInfo_Throws()
        {
            var bug = new Bug();
            bug.Start();
            bug.RequestInfo();

            Assert.Throws<InvalidOperationException>(() => bug.RequestInfo());
        }

        [TestMethod]
        public void Exception_Fix_From_MoreInfo_Throws()
        {
            var bug = new Bug();
            bug.Start();
            bug.RequestInfo();

            Assert.Throws<InvalidOperationException>(() => bug.Fix());
        }

        [TestMethod]
        public void TerminalState_Fixed_ThrowsOnStart()
        {
            var bug = new Bug();
            bug.Start();
            bug.Fix();

            Assert.Throws<InvalidOperationException>(() => bug.Start());
        }

        [TestMethod]
        public void TerminalState_NoDefect_ThrowsOnRequestInfo()
        {
            var bug = new Bug();
            bug.Start();
            bug.SetNotBug();

            Assert.Throws<InvalidOperationException>(() => bug.RequestInfo());
        }

        [TestMethod]
        public void TerminalState_Duplicate_ThrowsOnFix()
        {
            var bug = new Bug();
            bug.Start();
            bug.SetDuplicate();

            Assert.Throws<InvalidOperationException>(() => bug.Fix());
        }

        [TestMethod]
        public void TerminalState_NoTime_ThrowsOnOther()
        {
            var bug = new Bug();
            bug.Start();
            bug.SetNoTime();

            Assert.Throws<InvalidOperationException>(() => bug.SetOther());
        }

        [TestMethod]
        public void Flow_RequestInfo_Then_ProvideInfo_ReturnsToAnalysis()
        {
            var bug = new Bug();
            bug.Start();
            bug.RequestInfo();

            bug.AddInfo();
            Assert.AreEqual(BugState.Analysis, bug.State);
        }

        [TestMethod]
        public void Flow_Complex_Cycle_And_Fix()
        {
            var bug = new Bug();
            bug.Start();
            bug.RequestInfo();
            bug.AddInfo();

            bug.Fix();
            Assert.AreEqual(BugState.Fixed, bug.State);
        }
    }
}