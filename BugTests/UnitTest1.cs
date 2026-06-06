using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void Initial_State_Is_New()
    {
        var bug = new Bug();
        Assert.AreEqual(Bug.State.New, bug.CurrentState);
    }

    [TestMethod]
    public void New_To_Analysis()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.StartAnalysis);

        Assert.AreEqual(Bug.State.Analysis, bug.CurrentState);
    }

    [TestMethod]
    public void Analysis_To_NeedInfo()
    {
        var bug = CreateAnalyzedBug();

        bug.Fire(Bug.Trigger.RequestInfo);

        Assert.AreEqual(Bug.State.NeedInfo, bug.CurrentState);
    }

    [TestMethod]
    public void NeedInfo_Back_To_Analysis()
    {
        var bug = CreateAnalyzedBug();

        bug.Fire(Bug.Trigger.RequestInfo);
        bug.Fire(Bug.Trigger.ReceiveInfo);

        Assert.AreEqual(Bug.State.Analysis, bug.CurrentState);
    }

    [TestMethod]
    public void Analysis_To_Fixing()
    {
        var bug = CreateAnalyzedBug();

        bug.Fire(Bug.Trigger.StartFix);

        Assert.AreEqual(Bug.State.Fixing, bug.CurrentState);
    }

    [TestMethod]
    public void Fixing_To_Resolved()
    {
        var bug = CreateFixingBug();

        bug.Fire(Bug.Trigger.Resolve);

        Assert.AreEqual(Bug.State.Resolved, bug.CurrentState);
    }

    [TestMethod]
    public void Resolved_To_Closed()
    {
        var bug = CreateResolvedBug();

        bug.Fire(Bug.Trigger.VerifySuccess);

        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void Resolved_To_Reopened()
    {
        var bug = CreateResolvedBug();

        bug.Fire(Bug.Trigger.VerifyFail);

        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void Reopened_To_Analysis()
    {
        var bug = CreateResolvedBug();

        bug.Fire(Bug.Trigger.VerifyFail);
        bug.Fire(Bug.Trigger.StartAnalysis);

        Assert.AreEqual(Bug.State.Analysis, bug.CurrentState);
    }

    [TestMethod]
    public void Analysis_To_NotABug()
    {
        var bug = CreateAnalyzedBug();

        bug.Fire(Bug.Trigger.MarkNotABug);

        Assert.AreEqual(Bug.State.NotABug, bug.CurrentState);
    }

    [TestMethod]
    public void Analysis_To_Duplicate()
    {
        var bug = CreateAnalyzedBug();

        bug.Fire(Bug.Trigger.MarkDuplicate);

        Assert.AreEqual(Bug.State.Duplicate, bug.CurrentState);
    }

    [TestMethod]
    public void Analysis_To_CannotReproduce()
    {
        var bug = CreateAnalyzedBug();

        bug.Fire(Bug.Trigger.MarkCannotReproduce);

        Assert.AreEqual(Bug.State.CannotReproduce, bug.CurrentState);
    }

    [TestMethod]
    public void Closed_Can_Reopen()
    {
        var bug = CreateResolvedBug();

        bug.Fire(Bug.Trigger.VerifySuccess);
        bug.Fire(Bug.Trigger.Reopen);

        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void Duplicate_Can_Reopen()
    {
        var bug = CreateAnalyzedBug();

        bug.Fire(Bug.Trigger.MarkDuplicate);
        bug.Fire(Bug.Trigger.Reopen);

        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void NotABug_Can_Reopen()
    {
        var bug = CreateAnalyzedBug();

        bug.Fire(Bug.Trigger.MarkNotABug);
        bug.Fire(Bug.Trigger.Reopen);

        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void CannotReproduce_Can_Reopen()
    {
        var bug = CreateAnalyzedBug();

        bug.Fire(Bug.Trigger.MarkCannotReproduce);
        bug.Fire(Bug.Trigger.Reopen);

        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void Invalid_Transition_From_New()
    {
        var bug = new Bug();

        Assert.ThrowsException<InvalidOperationException>(
            () => bug.Fire(Bug.Trigger.Resolve));
    }

    [TestMethod]
    public void Invalid_Transition_From_Closed()
    {
        var bug = CreateResolvedBug();

        bug.Fire(Bug.Trigger.VerifySuccess);

        Assert.ThrowsException<InvalidOperationException>(
            () => bug.Fire(Bug.Trigger.Resolve));
    }

    [TestMethod]
    public void Invalid_Transition_From_Duplicate()
    {
        var bug = CreateAnalyzedBug();

        bug.Fire(Bug.Trigger.MarkDuplicate);

        Assert.ThrowsException<InvalidOperationException>(
            () => bug.Fire(Bug.Trigger.Resolve));
    }

    [TestMethod]
    public void Invalid_Transition_From_NotABug()
    {
        var bug = CreateAnalyzedBug();

        bug.Fire(Bug.Trigger.MarkNotABug);

        Assert.ThrowsException<InvalidOperationException>(
            () => bug.Fire(Bug.Trigger.StartFix));
    }

    private Bug CreateAnalyzedBug()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.StartAnalysis);
        return bug;
    }

    private Bug CreateFixingBug()
    {
        var bug = CreateAnalyzedBug();
        bug.Fire(Bug.Trigger.StartFix);
        return bug;
    }

    private Bug CreateResolvedBug()
    {
        var bug = CreateFixingBug();
        bug.Fire(Bug.Trigger.Resolve);
        return bug;
    }
}
