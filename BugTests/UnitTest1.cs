using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests
{
    [TestClass]
    public class BugWorkflowTests
    {
        private Bug _bug;

        [TestInitialize]
        public void SetUp()
        {
            _bug = new Bug(1, "Test Bug");
        }

        [TestMethod]
        public void Bug_InitialState_IsNewDefect()
        {
            Assert.AreEqual(BugState.NewDefect, _bug.State);
        }

        [TestMethod]
        public void Bug_StartAnalysis_GoesToAnalysis()
        {
            _bug.StartAnalysis();
            Assert.AreEqual(BugState.Analysis, _bug.State);
        }

        [TestMethod]
        public void Bug_SendToFix_FromAnalysis_GoesToInProgress()
        {
            _bug.StartAnalysis();
            _bug.SendToFix();
            Assert.AreEqual(BugState.InProgress, _bug.State);
        }

        [TestMethod]
        public void Bug_NeedMoreInfo_FromAnalysis_StaysInAnalysis()
        {
            _bug.StartAnalysis();
            _bug.NeedMoreInfo();
            Assert.AreEqual(BugState.Analysis, _bug.State);
        }

        [TestMethod]
        public void Bug_NoTime_FromAnalysis_StaysInAnalysis()
        {
            _bug.StartAnalysis();
            _bug.NoTime();
            Assert.AreEqual(BugState.Analysis, _bug.State);
        }

        [TestMethod]
        public void Bug_SeparateSolution_FromAnalysis_StaysInAnalysis()
        {
            _bug.StartAnalysis();
            _bug.SeparateSolution();
            Assert.AreEqual(BugState.Analysis, _bug.State);
        }

        [TestMethod]
        public void Bug_OtherProduct_FromAnalysis_StaysInAnalysis()
        {
            _bug.StartAnalysis();
            _bug.OtherProduct();
            Assert.AreEqual(BugState.Analysis, _bug.State);
        }

        [TestMethod]
        public void Bug_SetNotDefect_FromAnalysis_GoesToClosed()
        {
            _bug.StartAnalysis();
            _bug.SetNotDefect();
            Assert.AreEqual(BugState.Closed, _bug.State);
        }

        [TestMethod]
        public void Bug_SetNoFix_FromAnalysis_GoesToClosed()
        {
            _bug.StartAnalysis();
            _bug.SetNoFix();
            Assert.AreEqual(BugState.Closed, _bug.State);
        }

        [TestMethod]
        public void Bug_SetDuplicate_FromAnalysis_GoesToClosed()
        {
            _bug.StartAnalysis();
            _bug.SetDuplicate();
            Assert.AreEqual(BugState.Closed, _bug.State);
        }

        [TestMethod]
        public void Bug_SetNotReproducible_FromAnalysis_GoesToClosed()
        {
            _bug.StartAnalysis();
            _bug.SetNotReproducible();
            Assert.AreEqual(BugState.Closed, _bug.State);
        }

        [TestMethod]
        public void Bug_Fix_FromInProgress_GoesToAnalysis()
        {
            _bug.StartAnalysis();
            _bug.SendToFix();
            _bug.Fix();
            Assert.AreEqual(BugState.Analysis, _bug.State);
        }

        [TestMethod]
        public void Bug_VerifyOK_FromInProgress_GoesToClosed()
        {
            _bug.StartAnalysis();
            _bug.SendToFix();
            _bug.VerifyOK();
            Assert.AreEqual(BugState.Closed, _bug.State);
        }

        [TestMethod]
        public void Bug_VerifyFail_FromInProgress_GoesToAnalysis()
        {
            _bug.StartAnalysis();
            _bug.SendToFix();
            _bug.VerifyFail();
            Assert.AreEqual(BugState.Analysis, _bug.State);
        }

        [TestMethod]
        public void Bug_Reopen_FromClosed_GoesToReopened()
        {
            _bug.StartAnalysis();
            _bug.SetNotDefect();
            _bug.Reopen();
            Assert.AreEqual(BugState.Reopened, _bug.State);
        }

        [TestMethod]
        public void Bug_StartAnalysis_FromReopened_GoesToAnalysis()
        {
            _bug.StartAnalysis();
            _bug.SetNotDefect();
            _bug.Reopen();
            _bug.StartAnalysis();
            Assert.AreEqual(BugState.Analysis, _bug.State);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Bug_SendToFix_FromNewDefect_ThrowsException()
        {
            _bug.SendToFix();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Bug_VerifyOK_FromNewDefect_ThrowsException()
        {
            _bug.VerifyOK();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Bug_Reopen_FromNewDefect_ThrowsException()
        {
            _bug.Reopen();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Bug_Fix_FromNewDefect_ThrowsException()
        {
            _bug.Fix();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Bug_StartAnalysis_FromInProgress_ThrowsException()
        {
            _bug.StartAnalysis();
            _bug.SendToFix();
            _bug.StartAnalysis();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Bug_SetDuplicate_FromClosed_ThrowsException()
        {
            _bug.StartAnalysis();
            _bug.SetNotDefect();
            _bug.SetDuplicate();
        }

        [TestMethod]
        public void Bug_FullLifecycle_SuccessfulFix()
        {
            _bug.StartAnalysis();
            _bug.SendToFix();
            _bug.VerifyOK();
            Assert.AreEqual(BugState.Closed, _bug.State);
        }

        [TestMethod]
        public void Bug_FullLifecycle_WithReopen()
        {
            _bug.StartAnalysis();
            _bug.SendToFix();
            _bug.VerifyOK();
            _bug.Reopen();
            _bug.StartAnalysis();
            _bug.SendToFix();
            _bug.VerifyOK();
            Assert.AreEqual(BugState.Closed, _bug.State);
        }
    }
}
