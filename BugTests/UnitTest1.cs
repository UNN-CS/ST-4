namespace BugTests
{
    using BugPro;
    using System.Reflection.PortableExecutable;

    [TestClass]
    public sealed class UnitTest1
    {
        [TestMethod]
        public void New_To_Triage()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            Assert.AreEqual(BugState.Triage, bug.State);
        }

        // Triage ---> Fixing
        [TestMethod]
        public void Triage_To_Fixing()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.StartFix);
            Assert.AreEqual(BugState.Fixing, bug.State);
        }

        // Triage ---> NoTimeNow/NeedSeparateSolution/ExternalProblem
        [TestMethod]
        public void Triage_To_NoTimeNow()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.NoTime);
            Assert.AreEqual(BugState.NoTimeNow, bug.State);
        }

        [TestMethod]
        public void Triage_To_NeedSeparateSolution()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.NeedSolution);
            Assert.AreEqual(BugState.NeedSeparateSolution, bug.State);
        }

        [TestMethod]
        public void Triage_To_ExternalProblem()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.External);
            Assert.AreEqual(BugState.ExternalProblem, bug.State);
        }

        // NoTimeNow/NeedSeparateSolution/ExternalProblem ---> Triage
        [TestMethod]
        public void NoTimeNow_To_Triage()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.NoTime);
            bug.Fire(BugAction.StartTriage);
            Assert.AreEqual(BugState.Triage, bug.State);
        }

        [TestMethod]
        public void NeedSeparateSolution_To_Triage()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.NeedSolution);
            bug.Fire(BugAction.StartTriage);
            Assert.AreEqual(BugState.Triage, bug.State);
        }

        [TestMethod]
        public void ExternalProblem_To_Triage()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.External);
            bug.Fire(BugAction.StartTriage);
            Assert.AreEqual(BugState.Triage, bug.State);
        }


        // Triage ---> NotABug/WontFix/Duplicate
        [TestMethod]
        public void Triage_To_NotABug()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.MarkNotABug);
            Assert.AreEqual(BugState.NotABug, bug.State);
        }

        [TestMethod]
        public void Triage_To_WontFix()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.MarkWontFix);
            Assert.AreEqual(BugState.WontFix, bug.State);
        }

        [TestMethod]
        public void Triage_To_Duplicate()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.MarkDuplicate);
            Assert.AreEqual(BugState.Duplicate, bug.State);
        }

        // NotABug/WontFix/Duplicate final states, no transitions out of them
        [TestMethod]
        public void NotABug_cannot_transit()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.MarkNotABug);

            Assert.IsTrue(bug.IsFinal());
            Assert.Throws<Exception>(() =>
                bug.Fire(BugAction.StartTriage));
        }

        [TestMethod]
        public void WontFix_cannot_transit()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.MarkWontFix);

            Assert.IsTrue(bug.IsFinal());
            Assert.Throws<Exception>(() =>
                bug.Fire(BugAction.StartTriage));
        }

        [TestMethod]
        public void Duplicate_cannot_transit()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.MarkDuplicate);

            Assert.IsTrue(bug.IsFinal());
            Assert.Throws<Exception>(() =>
                bug.Fire(BugAction.StartTriage));
        }

        // Fixing ---> NeedMoreInfo
        [TestMethod]
        public void Fixing_To_NeedMoreInfo()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.StartFix);
            bug.Fire(BugAction.RequestInfo);
            Assert.AreEqual(BugState.NeedMoreInfo, bug.State);
        }

        // NeedMoreInfo ---> Fixing
        [TestMethod]
        public void NeedMoreInfo_To_Fixing()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.StartFix);
            bug.Fire(BugAction.RequestInfo);
            bug.Fire(BugAction.StartFix);
            Assert.AreEqual(BugState.Fixing, bug.State);
        }

        // Fixing ---> ResolutionCheck
        [TestMethod]
        public void Fixing_To_ResolvedCheck()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.StartFix);
            bug.Fire(BugAction.ToResolutionCheck);
            Assert.AreEqual(BugState.ResolutionCheck, bug.State);
        }

        // ResolutionCheck ---> Closed/Returned
        [TestMethod]
        public void ResolutionCheck_Yes_To_Closed()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.StartFix);
            bug.Fire(BugAction.ToResolutionCheck);
            bug.Fire(BugAction.Approve);
            Assert.AreEqual(BugState.Closed, bug.State);
        }

        [TestMethod]
        public void ResolutionCheck_No_To_Returned()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.StartFix);
            bug.Fire(BugAction.ToResolutionCheck);
            bug.Fire(BugAction.Reject);
            Assert.AreEqual(BugState.Returned, bug.State);
        }

        // Fixing ---> CannotReproduce
        [TestMethod]
        public void Fixing_To_CannotReproduce()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.StartFix);
            bug.Fire(BugAction.MarkCannotReproduce);
            Assert.AreEqual(BugState.CannotReproduce, bug.State);
        }

        // CannotReproduce ---> OkCheck
        [TestMethod]
        public void CannotReproduce_To_OkCheck()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.StartFix);
            bug.Fire(BugAction.MarkCannotReproduce);
            bug.Fire(BugAction.ToOkCheck);
            Assert.AreEqual(BugState.OkCheck, bug.State);
        }

        // OkCheck ---> Closed/Returned
        [TestMethod]
        public void OkCheck_Yes_To_Closed()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.StartFix);
            bug.Fire(BugAction.MarkCannotReproduce);
            bug.Fire(BugAction.ToOkCheck);
            bug.Fire(BugAction.Approve);
            Assert.AreEqual(BugState.Closed, bug.State);
        }

        [TestMethod]
        public void OkCheck_No_To_Returned()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.StartFix);
            bug.Fire(BugAction.MarkCannotReproduce);
            bug.Fire(BugAction.ToOkCheck);
            bug.Fire(BugAction.Reject);
            Assert.AreEqual(BugState.Returned, bug.State);
        }


        // Returned ---> Triage
        [TestMethod]
        public void Returned_To_Triage()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.StartFix);
            bug.Fire(BugAction.ToResolutionCheck);
            bug.Fire(BugAction.Reject);
            bug.Fire(BugAction.StartTriage);
            Assert.AreEqual(BugState.Triage, bug.State);
        }

        // Closed ---> Reopened
        [TestMethod]
        public void Closed_To_Reopened()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.StartFix);
            bug.Fire(BugAction.ToResolutionCheck);
            bug.Fire(BugAction.Approve);
            bug.Fire(BugAction.Reopen);
            Assert.AreEqual(BugState.Reopened, bug.State);
        }

        // Reopened ---> Triage
        [TestMethod]
        public void Reopened_To_Triage()
        {
            var bug = new Bug();
            bug.Fire(BugAction.StartTriage);
            bug.Fire(BugAction.StartFix);
            bug.Fire(BugAction.ToResolutionCheck);
            bug.Fire(BugAction.Approve);
            bug.Fire(BugAction.Reopen);
            bug.Fire(BugAction.StartTriage);
            Assert.AreEqual(BugState.Triage, bug.State);
        }
    }
}
