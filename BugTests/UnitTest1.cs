using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        // ���� �������� ���������� ���������
        [TestMethod]
        public void TestInitialState()
        {
            var bug = new Bug(Bug.State.Open);
            Assert.AreEqual(Bug.State.Open, bug.GetState());
        }

        // ���������� �������: Open -> Assigned
        [TestMethod]
        public void TestOpenToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        // ���������� �������: Open -> InProgress
        [TestMethod]
        public void TestOpenToInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        // ���������� �������: Assigned -> InProgress
        [TestMethod]
        public void TestAssignedToInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();         // Open -> Assigned
            bug.StartProgress();  // Assigned -> InProgress
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        // ���������� �������: Assigned -> Defered
        [TestMethod]
        public void TestAssignedToDefered()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();  // Open -> Assigned
            bug.Defer();   // Assigned -> Defered
            Assert.AreEqual(Bug.State.Defered, bug.GetState());
        }

        // ���������� �������: Defered -> Assigned
        [TestMethod]
        public void TestDeferedToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();  // Open -> Assigned
            bug.Defer();   // Assigned -> Defered
            bug.Assign();  // Defered -> Assigned
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        // ���������� �������: InProgress -> Resolved
        [TestMethod]
        public void TestInProgressToResolved()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();  // Open -> InProgress
            bug.Resolve();        // InProgress -> Resolved
            Assert.AreEqual(Bug.State.Resolved, bug.GetState());
        }

        // ���������� �������: Resolved -> Closed
        [TestMethod]
        public void TestResolvedToClosed()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();  // Open -> InProgress
            bug.Resolve();        // InProgress -> Resolved
            bug.Close();          // Resolved -> Closed
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        // ���������� �������: Resolved -> Reopened
        [TestMethod]
        public void TestResolvedToReopened()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();  // Open -> InProgress
            bug.Resolve();        // InProgress -> Resolved
            bug.Reopen();         // Resolved -> Reopened
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        // ���������� �������: Closed -> Reopened
        [TestMethod]
        public void TestClosedToReopened()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();  // Open -> InProgress
            bug.Resolve();        // InProgress -> Resolved
            bug.Close();          // Resolved -> Closed
            bug.Reopen();         // Closed -> Reopened
            Assert.AreEqual(Bug.State.Reopened, bug.GetState());
        }

        // ���������� �������: Reopened -> Assigned
        [TestMethod]
        public void TestReopenedToAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            bug.Reopen();         // Closed -> Reopened
            bug.Assign();         // Reopened -> Assigned
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        // ���������� �������: Reopened -> InProgress
        [TestMethod]
        public void TestReopenedToInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            bug.Reopen();         // Closed -> Reopened
            bug.StartProgress();  // Reopened -> InProgress
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        // �������� ���������������� ���������
        [TestMethod]
        public void TestMultipleTransitions1()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();           // Open -> Assigned
            bug.StartProgress();    // Assigned -> InProgress
            bug.Resolve();          // InProgress -> Resolved
            bug.Reopen();           // Resolved -> Reopened
            bug.Assign();           // Reopened -> Assigned
            bug.StartProgress();    // Assigned -> InProgress
            bug.Resolve();          // InProgress -> Resolved
            bug.Close();            // Resolved -> Closed
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        // ����: ��������� ����� Assign � ��������� Assigned (trigger ������������)
        [TestMethod]
        public void TestIgnoreTriggerAssign()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();   // Open -> Assigned
            bug.Assign();   // � ��������� Assigned trigger Assign ������������
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        // ������� ���������� ������������� ��������: ����� Resolve �� ��������� Assigned
        [TestMethod]
        public void TestInvalidTransitionAssignedToResolve()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();  // Open -> Assigned

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Resolve();  // ��� Assigned ������� Resolve �� ��������
            });
        }

        // ������� ���������� ������������� ��������: ����� Close �� ��������� Assigned
        [TestMethod]
        public void TestInvalidTransitionAssignedToClose()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();  // Open -> Assigned

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Close();  // ��� Assigned ������� Close �� ��������
            });
        }

        // ������� ���������� ������������� ��������: ����� Reopen �� ��������� Open
        [TestMethod]
        public void TestInvalidTransitionOpenToReopen()
        {
            var bug = new Bug(Bug.State.Open);

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Reopen(); // � ��������� Open ��� �������� Reopen
            });
        }

        // ���� ������������������: Open -> Assigned -> Defered -> Assigned -> InProgress -> Resolved -> Reopen -> InProgress
        [TestMethod]
        public void TestComplexTransitionSequence()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();           // Open -> Assigned
            bug.Defer();            // Assigned -> Defered
            bug.Assign();           // Defered -> Assigned
            bug.StartProgress();    // Assigned -> InProgress
            bug.Resolve();          // InProgress -> Resolved
            bug.Reopen();           // Resolved -> Reopened
            bug.StartProgress();    // Reopened -> InProgress
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }

        // ����: ������� ��������� Defer � ������������ ��������� (��������, � ��������� InProgress)
        [TestMethod]
        public void TestInvalidDeferFromInProgress()
        {
            var bug = new Bug(Bug.State.Open);
            bug.StartProgress();  // Open -> InProgress

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Defer();    // ��� InProgress ������� Defer �� ��������
            });
        }

        // ����: ���������� ���������� ���������� ��������� ��������������� ��� ������
        [TestMethod]
        public void TestMultipleValidTransitions()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();         // Open -> Assigned
            bug.StartProgress();  // Assigned -> InProgress
            bug.Resolve();        // InProgress -> Resolved
            bug.Reopen();         // Resolved -> Reopened
            bug.StartProgress();  // Reopened -> InProgress
            bug.Resolve();        // InProgress -> Resolved
            bug.Close();          // Resolved -> Closed
            Assert.AreEqual(Bug.State.Closed, bug.GetState());
        }

        // ����: ��������, ��� ��������� ����� Trigger, �� ���������� � ��������, �� ������ ���������
        [TestMethod]
        public void TestRepeatIgnoredTrigger()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign(); // Open -> Assigned
            // ����� Assign ������ (������ ������������)
            bug.Assign();
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());
        }

        // ����: ������������ ������� ��������� ������ ����������� ���������� (��������, Attempt Reopen �� Assigned)
        [TestMethod]
        public void TestInvalidReopenFromAssigned()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign(); // Open -> Assigned

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                bug.Reopen(); // � ��������� Assigned ��� �������� Reopen
            });
        }

        // ����: ������������������ ��� ������ ��� ��������� �� Reopened
        [TestMethod]
        public void TestReopenedTransitionPaths()
        {
            var bug = new Bug(Bug.State.Open);
            bug.Assign();         // Open -> Assigned
            bug.Defer();          // Assigned -> Defered
            bug.Assign();         // Defered -> Assigned
            bug.StartProgress();  // Assigned -> InProgress
            bug.Resolve();        // InProgress -> Resolved
            bug.Close();          // Resolved -> Closed
            bug.Reopen();         // Closed -> Reopened

            // �� Reopened �������� ��� ��������. ��������� ���.
            bug.Assign();         // Reopened -> Assigned
            Assert.AreEqual(Bug.State.Assigned, bug.GetState());

            // �������� ����� ��� � ������� ����� Reopened -> InProgress
            bug = new Bug(Bug.State.Open);
            bug.Assign();
            bug.StartProgress();
            bug.Resolve();
            bug.Close();
            bug.Reopen();
            bug.StartProgress();  // Reopened -> InProgress
            Assert.AreEqual(Bug.State.InProgress, bug.GetState());
        }
    }
}
