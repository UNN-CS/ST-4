// Copyright 2026 NNTU-CS
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using Stateless;
using System;

namespace BugTests {
  [TestClass]
  public class UnitTest1 {
    [TestMethod]
    public void Bug_InitialState_IsOpen() {
      var bug = new Bug("Test");
      Assert.AreEqual(Bug.State.Open, bug.CurrentState);
    }

    [TestMethod]
    public void Bug_Assign_MovesToAssigned() {
      var bug = new Bug("Test");
      bug.Assign();
      Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
    }

    [TestMethod]
    public void Bug_Start_MovesToInProgress() {
      var bug = new Bug("Test");
      bug.Assign();
      bug.Start();
      Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
    }

    [TestMethod]
    public void Bug_Resolve_MovesToResolved() {
      var bug = new Bug("Test");
      bug.Assign();
      bug.Start();
      bug.Resolve();
      Assert.AreEqual(Bug.State.Resolved, bug.CurrentState);
    }

    [TestMethod]
    public void Bug_Close_MovesToClosed() {
      var bug = new Bug("Test");
      bug.Assign();
      bug.Start();
      bug.Resolve();
      bug.Close();
      Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void Bug_Reopen_FromClosed_MovesToReopened() {
      var bug = new Bug("Test");
      bug.Assign();
      bug.Start();
      bug.Resolve();
      bug.Close();
      bug.Reopen();
      Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void Bug_Reopen_FromResolved_MovesToReopened() {
      var bug = new Bug("Test");
      bug.Assign();
      bug.Start();
      bug.Resolve();
      bug.Reopen();
      Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void Bug_Reset_FromAssigned_MovesToOpen() {
      var bug = new Bug("Test");
      bug.Assign();
      bug.Reset();
      Assert.AreEqual(Bug.State.Open, bug.CurrentState);
    }

    [TestMethod]
    public void Bug_Reset_FromInProgress_MovesToOpen() {
      var bug = new Bug("Test");
      bug.Assign();
      bug.Start();
      bug.Reset();
      Assert.AreEqual(Bug.State.Open, bug.CurrentState);
    }

    [TestMethod]
    public void Bug_ReopenedCanAssign() {
      var bug = new Bug("Test");
      bug.Assign();
      bug.Start();
      bug.Resolve();
      bug.Reopen();
      Assert.IsTrue(bug.CanAssign());
    }

    [TestMethod]
    public void Bug_CanAssign_WhenOpen() {
      var bug = new Bug("Test");
      Assert.IsTrue(bug.CanAssign());
    }

    [TestMethod]
    public void Bug_CannotStart_WhenOpen() {
      var bug = new Bug("Test");
      Assert.IsFalse(bug.CanStart());
    }

    [TestMethod]
    public void Bug_CannotClose_WhenOpen() {
      var bug = new Bug("Test");
      Assert.IsFalse(bug.CanClose());
    }

    [TestMethod]
    public void Bug_CannotReopen_WhenOpen() {
      var bug = new Bug("Test");
      Assert.IsFalse(bug.CanReopen());
    }

    [TestMethod]
    public void Bug_CannotResolve_WhenOpen() {
      var bug = new Bug("Test");
      Assert.IsFalse(bug.CanResolve());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Bug_Start_WhenOpen_ThrowsException() {
      var bug = new Bug("Test");
      bug.Start();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Bug_Resolve_WhenOpen_ThrowsException() {
      var bug = new Bug("Test");
      bug.Resolve();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Bug_Close_WhenOpen_ThrowsException() {
      var bug = new Bug("Test");
      bug.Close();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Bug_Reopen_WhenOpen_ThrowsException() {
      var bug = new Bug("Test");
      bug.Reopen();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Bug_Assign_WhenInProgress_ThrowsException() {
      var bug = new Bug("Test");
      bug.Assign();
      bug.Start();
      bug.Assign();
    }

    [TestMethod]
    public void Bug_FullCycle_WorksCorrectly() {
      var bug = new Bug("Test");
      Assert.AreEqual(Bug.State.Open, bug.CurrentState);
      bug.Assign();
      Assert.AreEqual(Bug.State.Assigned, bug.CurrentState);
      bug.Start();
      Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
      bug.Resolve();
      Assert.AreEqual(Bug.State.Resolved, bug.CurrentState);
      bug.Close();
      Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }
  }
}
