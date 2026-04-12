using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using System;

namespace BugTests;

[TestClass]
public class BugStateMachineTests
{
    private Bug bug = new Bug();
    
    [TestInitialize]
    public void Setup() { bug = new Bug(); }
    
    [TestMethod]
    public void Test_Initial_New()
    {
        Assert.AreEqual(BugState.New, bug.State);
    }

    [TestMethod]
    public void Test_New_Triage()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Test_Triage_Fixing()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.AssignRole(Role.Developer);
        bug.StartFix();
        Assert.AreEqual(BugState.Fixing, bug.State);
    }

    [TestMethod]
    public void Test_Triage_NotBug()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.NotBug();
        Assert.AreEqual(BugState.NotBug, bug.State);
        bug.AssignRole(Role.Tester);
        bug.Approve();
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Test_Triage_DontFix()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.DontFix();
        bug.AssignRole(Role.Tester);
        bug.Reject();
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Test_Triage_Duplicate()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.Duplicate();
        bug.AssignRole(Role.Tester);
        bug.Approve();
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Test_Triage_NotReproducible()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.AssignRole(Role.Developer);
        bug.NotReproducible();
        Assert.AreEqual(BugState.NotReproducible, bug.State);
    }

    [TestMethod]
    public void Test_NotReproducible_Approve()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.AssignRole(Role.Developer);
        bug.NotReproducible();
        bug.AssignRole(Role.Tester);
        bug.Approve(); 
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Test_NotReproducible_Reject()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.AssignRole(Role.Developer);
        bug.NotReproducible();
        bug.AssignRole(Role.Tester);
        bug.Reject(); 
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Test_Fixing_NoTime()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.AssignRole(Role.Developer);
        bug.StartFix();
        bug.NoTime();
        Assert.AreEqual(BugState.NoTime, bug.State);
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Test_Fixing_NeedSeparateSolution()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.AssignRole(Role.Developer);
        bug.StartFix();
        bug.NeedSeparateSolution();
        Assert.AreEqual(BugState.NeedSeparateSolution, bug.State);
        bug.AssignRole(Role.Developer);
        bug.StartFix();
        Assert.AreEqual(BugState.Fixing, bug.State);
    }

    [TestMethod]
    public void Test_Fixing_OtherProductProblem()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.AssignRole(Role.Developer);
        bug.StartFix();
        bug.OtherProductProblem();
        Assert.AreEqual(BugState.OtherProductProblem, bug.State);
        bug.AssignRole(Role.Developer);
        bug.StartFix();
        Assert.AreEqual(BugState.Fixing, bug.State);
    }

    [TestMethod]
    public void Test_Fixing_RequestMoreInfo()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.AssignRole(Role.Developer);
        bug.StartFix();
        bug.AssignRole(Role.Tester);
        bug.RequestMoreInfo();
        Assert.AreEqual(BugState.RequestMoreInfo, bug.State);
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Test_EndFix_Approve()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.AssignRole(Role.Developer);
        bug.StartFix();
        bug.EndFix();
        bug.AssignRole(Role.Tester);
        bug.Approve();
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Test_EndFix_Reject()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.AssignRole(Role.Developer);
        bug.StartFix();
        bug.EndFix();
        bug.AssignRole(Role.Tester);
        bug.Reject();
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Test_Approve_Reopen()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.Duplicate();
        bug.AssignRole(Role.Tester);
        bug.Approve();
        bug.Reopen();
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Test_Tester_StartFix()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.AssignRole(Role.Tester);
        Assert.ThrowsException<InvalidOperationException>(() => bug.StartFix());
    }

    [TestMethod]
    public void Test_Developer_Approve()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.AssignRole(Role.ProductTeam);
        bug.NotBug();
        bug.AssignRole(Role.Developer);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Approve());
    }

    [TestMethod]
    public void Test_ProductTeam_EndFix()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.AssignRole(Role.Developer);
        bug.StartFix();
        bug.AssignRole(Role.ProductTeam);
        Assert.ThrowsException<InvalidOperationException>(() => bug.EndFix());
    }

    [TestMethod]
    public void Test_Tester_NotReproducible()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.AssignRole(Role.Tester);
        Assert.ThrowsException<InvalidOperationException>(() => bug.NotReproducible());
    }

    [TestMethod]
    public void Test_Developer_StartTriage()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.AssignRole(Role.Developer);
        bug.StartFix();
        bug.NoTime();
        bug.AssignRole(Role.Developer);
        Assert.ThrowsException<InvalidOperationException>(() => bug.StartTriage());
    }
    
    [TestMethod]
    public void TestFull()
    {
        bug.AssignRole(Role.ProductTeam);
        bug.StartTriage();
        bug.AssignRole(Role.Developer);
        bug.StartFix();
        bug.EndFix();
        bug.AssignRole(Role.Tester);
        bug.Reject();
        bug.AssignRole(Role.Developer);
        bug.StartFix();
        bug.EndFix();
        bug.AssignRole(Role.Tester);
        bug.Approve();
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Test_Initial_Custom()
    {
        var localBug = new Bug(BugState.Fixing);
        Assert.AreEqual(BugState.Fixing, localBug.State);
    }
}