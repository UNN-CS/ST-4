using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests
{
    [TestClass]
    public class BugTests
    {
        [TestMethod]
        public void InitialState_ShouldBeNewDefect()
        {
            var bug = new Bug();
            Assert.AreEqual(BugState.NewDefect, bug.CurrentState);
        }

        [TestMethod]
        public void StartAnalysis_FromNewDefect_GoesToAnalysis()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            Assert.AreEqual(BugState.Analysis, bug.CurrentState);
        }

        [TestMethod]
        public void FromAnalysis_MarkAsFixed_GoesToFixed()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.MarkAsFixed();
            Assert.AreEqual(BugState.Fixed, bug.CurrentState);
        }

        [TestMethod]
        public void FromAnalysis_MarkAsNotDefect_GoesToNotDefect()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.MarkAsNotDefect();
            Assert.AreEqual(BugState.NotDefect, bug.CurrentState);
        }

        [TestMethod]
        public void FromAnalysis_MarkAsWonTFix_GoesToWonTFix()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.MarkAsWonTFix();
            Assert.AreEqual(BugState.WonTFix, bug.CurrentState);
        }

        [TestMethod]
        public void FromAnalysis_MarkAsDuplicate_GoesToDuplicate()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.MarkAsDuplicate();
            Assert.AreEqual(BugState.Duplicate, bug.CurrentState);
        }

        [TestMethod]
        public void FromAnalysis_MarkAsNotReproducible_GoesToNotReproducible()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.MarkAsNotReproducible();
            Assert.AreEqual(BugState.NotReproducible, bug.CurrentState);
        }

        [TestMethod]
        public void FromAnalysis_NeedMoreInfo_GoesToNeedMoreInfo()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.NeedMoreInfo();
            Assert.AreEqual(BugState.NeedMoreInfo, bug.CurrentState);
        }

        [TestMethod]
        public void FromAnalysis_MarkAsSeparateSolution_GoesToSeparateSolution()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.MarkAsSeparateSolution();
            Assert.AreEqual(BugState.SeparateSolution, bug.CurrentState);
        }

        [TestMethod]
        public void FromAnalysis_MarkAsNoTimeNow_GoesToNoTimeNow()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.MarkAsNoTimeNow();
            Assert.AreEqual(BugState.NoTimeNow, bug.CurrentState);
        }

        [TestMethod]
        public void FromAnalysis_MarkAsOtherProduct_GoesToOtherProduct()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.MarkAsOtherProduct();
            Assert.AreEqual(BugState.OtherProduct, bug.CurrentState);
        }

        [TestMethod]
        public void FromNeedMoreInfo_ProvideInfo_ReturnsToAnalysis()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.NeedMoreInfo();
            bug.ProvideInfo();
            Assert.AreEqual(BugState.Analysis, bug.CurrentState);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FromNewDefect_MarkAsFixed_Throws()
        {
            var bug = new Bug();
            bug.MarkAsFixed();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FromNewDefect_MarkAsDuplicate_Throws()
        {
            var bug = new Bug();
            bug.MarkAsDuplicate();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FromFixed_StartAnalysis_Throws()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.MarkAsFixed();
            bug.StartAnalysis();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FromFixed_MarkAsFixed_Throws()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.MarkAsFixed();
            bug.MarkAsFixed();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FromAnalysis_ProvideInfo_Throws()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.ProvideInfo();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FromNeedMoreInfo_MarkAsFixed_Throws()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.NeedMoreInfo();
            bug.MarkAsFixed();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FromNotDefect_StartAnalysis_Throws()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.MarkAsNotDefect();
            bug.StartAnalysis();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FromSeparateSolution_StartAnalysis_Throws()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.MarkAsSeparateSolution();
            bug.StartAnalysis();
        }

        [TestMethod]
        public void AfterInvalidTransition_StateRemainsUnchanged()
        {
            var bug = new Bug();
            try
            {
                bug.MarkAsFixed();
            }
            catch (InvalidOperationException) { }
            Assert.AreEqual(BugState.NewDefect, bug.CurrentState);
        }

        [TestMethod]
        public void FullValidFlow_NewDefect_Analysis_Fixed()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.MarkAsFixed();
            Assert.AreEqual(BugState.Fixed, bug.CurrentState);
        }

        [TestMethod]
        public void FullValidFlow_NewDefect_Analysis_NeedMoreInfo_ProvideInfo_Fixed()
        {
            var bug = new Bug();
            bug.StartAnalysis();
            bug.NeedMoreInfo();
            bug.ProvideInfo();
            bug.MarkAsFixed();
            Assert.AreEqual(BugState.Fixed, bug.CurrentState);
        }
    }
}
