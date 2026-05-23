using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void New_To_InReview()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.StartReview);

        Assert.AreEqual(Bug.State.InReview, bug.Current);
    }

    [TestMethod]
    public void InReview_To_InProgress()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);
        bug.Fire(Bug.Trigger.StartWork);

        Assert.AreEqual(Bug.State.InProgress, bug.Current);
    }

    [TestMethod]
    public void InProgress_To_Fixed()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);
        bug.Fire(Bug.Trigger.StartWork);
        bug.Fire(Bug.Trigger.Fix);

        Assert.AreEqual(Bug.State.Fixed, bug.Current);
    }

    [TestMethod]
    public void Fixed_To_Closed()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);
        bug.Fire(Bug.Trigger.StartWork);
        bug.Fire(Bug.Trigger.Fix);
        bug.Fire(Bug.Trigger.Close);

        Assert.AreEqual(Bug.State.Closed, bug.Current);
    }

    [TestMethod]
    public void Fixed_To_Reopened()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);
        bug.Fire(Bug.Trigger.StartWork);
        bug.Fire(Bug.Trigger.Fix);
        bug.Fire(Bug.Trigger.Reopen);

        Assert.AreEqual(Bug.State.Reopened, bug.Current);
    }

    [TestMethod]
    public void Reopened_To_InProgress()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);
        bug.Fire(Bug.Trigger.StartWork);
        bug.Fire(Bug.Trigger.Fix);
        bug.Fire(Bug.Trigger.Reopen);
        bug.Fire(Bug.Trigger.StartWork);

        Assert.AreEqual(Bug.State.InProgress, bug.Current);
    }

    [TestMethod]
    public void New_To_Rejected()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.Reject);

        Assert.AreEqual(Bug.State.Rejected, bug.Current);
    }

    [TestMethod]
    public void InReview_To_Rejected()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);
        bug.Fire(Bug.Trigger.Reject);

        Assert.AreEqual(Bug.State.Rejected, bug.Current);
    }

    [TestMethod]
    public void Full_Normal_Flow()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);
        bug.Fire(Bug.Trigger.StartWork);
        bug.Fire(Bug.Trigger.Fix);
        bug.Fire(Bug.Trigger.Close);

        Assert.AreEqual(Bug.State.Closed, bug.Current);
    }

    [TestMethod]
    public void Full_Reopen_Flow()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);
        bug.Fire(Bug.Trigger.StartWork);
        bug.Fire(Bug.Trigger.Fix);
        bug.Fire(Bug.Trigger.Reopen);
        bug.Fire(Bug.Trigger.StartWork);
        bug.Fire(Bug.Trigger.Fix);
        bug.Fire(Bug.Trigger.Close);

        Assert.AreEqual(Bug.State.Closed, bug.Current);
    }

    [TestMethod]
    public void Invalid_Transition_Fix_From_New_Should_Throw()
    {
        var bug = new Bug();

        Assert.Throws<InvalidOperationException>(() =>
        {
            bug.Fire(Bug.Trigger.Fix);
        });
    }

    [TestMethod]
    public void Invalid_Transition_Close_From_New_Should_Throw()
    {
        var bug = new Bug();

        Assert.Throws<InvalidOperationException>(() =>
        {
            bug.Fire(Bug.Trigger.Close);
        });
    }

    [TestMethod]
    public void Invalid_Transition_StartWork_From_New_Should_Throw()
    {
        var bug = new Bug();

        Assert.Throws<InvalidOperationException>(() =>
        {
            bug.Fire(Bug.Trigger.StartWork);
        });
    }

    [TestMethod]
    public void Invalid_Transition_Reopen_From_New_Should_Throw()
    {
        var bug = new Bug();

        Assert.Throws<InvalidOperationException>(() =>
        {
            bug.Fire(Bug.Trigger.Reopen);
        });
    }

    [TestMethod]
    public void Cannot_Close_From_InProgress()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);
        bug.Fire(Bug.Trigger.StartWork);

        Assert.Throws<InvalidOperationException>(() =>
        {
            bug.Fire(Bug.Trigger.Close);
        });
    }

    [TestMethod]
    public void Cannot_Fix_From_InReview()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);

        Assert.Throws<InvalidOperationException>(() =>
        {
            bug.Fire(Bug.Trigger.Fix);
        });
    }

    [TestMethod]
    public void Repeated_StartReview_Should_Throw()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);

        Assert.Throws<InvalidOperationException>(() =>
        {
            bug.Fire(Bug.Trigger.StartReview);
        });
    }

    [TestMethod]
    public void Repeated_Close_Should_Throw()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);
        bug.Fire(Bug.Trigger.StartWork);
        bug.Fire(Bug.Trigger.Fix);
        bug.Fire(Bug.Trigger.Close);

        Assert.Throws<InvalidOperationException>(() =>
        {
            bug.Fire(Bug.Trigger.Close);
        });
    }

    [TestMethod]
    public void Closed_State_Should_Be_Final()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);
        bug.Fire(Bug.Trigger.StartWork);
        bug.Fire(Bug.Trigger.Fix);
        bug.Fire(Bug.Trigger.Close);

        Assert.AreEqual(Bug.State.Closed, bug.Current);
    }

    [TestMethod]
    public void Rejected_State_Should_Be_Final()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.Reject);

        Assert.AreEqual(Bug.State.Rejected, bug.Current);
    }

    [TestMethod]
    public void InReview_To_NeedMoreInfo()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);
        bug.Fire(Bug.Trigger.RequestMoreInfo);

        Assert.AreEqual(Bug.State.NeedMoreInfo, bug.Current);
    }

    [TestMethod]
    public void NeedMoreInfo_To_InReview()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);
        bug.Fire(Bug.Trigger.RequestMoreInfo);
        bug.Fire(Bug.Trigger.ProvideInfo);

        Assert.AreEqual(Bug.State.InReview, bug.Current);
    }

    [TestMethod]
    public void InReview_To_Duplicate()
        {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);
        bug.Fire(Bug.Trigger.MarkDuplicate);

        Assert.AreEqual(Bug.State.Duplicate, bug.Current);
    }

    [TestMethod]
    public void InReview_To_NotABug()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);
        bug.Fire(Bug.Trigger.MarkNotABug);

        Assert.AreEqual(Bug.State.NotABug, bug.Current);
    }

    [TestMethod]
    public void InReview_To_CannotReproduce()
    {
        var bug = new Bug();

        bug.Fire(Bug.Trigger.StartReview);
        bug.Fire(Bug.Trigger.MarkCannotReproduce);

        Assert.AreEqual(Bug.State.CannotReproduce, bug.Current);
    }

    [TestMethod]
    public void Cannot_ProvideInfo_Without_Request()
    {
        var bug = new Bug();

        Assert.Throws<InvalidOperationException>(() =>
        {
            bug.Fire(Bug.Trigger.ProvideInfo);
        });
    }
    
    [TestMethod]
    public void Cannot_MarkDuplicate_From_New()
    {
        var bug = new Bug();

        Assert.Throws<InvalidOperationException>(() =>
        {
            bug.Fire(Bug.Trigger.MarkDuplicate);
        });
    }
}
