using System;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
    	var bug = new Bug(Bug.State.Open);
	    Assert.AreEqual(bug.getState(), Bug.State.Open);
    }
    [TestMethod]
    public void TestMethod2()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetUpEnviroment();
        Assert.AreEqual(bug.getState(), Bug.State.SetUpEnviroment);
    }
    [TestMethod]
    public void TestMethod3()
    {
        var bug = new Bug(Bug.State.Open);
        try
        {
            bug.Close();
        }catch(Exception ex)
                {
            Assert.AreNotEqual(bug.getState(), Bug.State.Closed);
        }
    }
    [TestMethod]
    public void TestMethod4()
    {
        var bug = new Bug(Bug.State.Open);
        try
        {
            bug.Assign();
        }catch (Exception ex)
               {
            Assert.AreEqual(bug.getState(), Bug.State.Open);
        }
    }
    [TestMethod]
    public void TestMethod5()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetUpEnviroment();
        bug.Assign();
        bug.InProgress();
        Assert.AreEqual(bug.getState(), Bug.State.InProgress);
    }
    [TestMethod]
    public void TestMethod6()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetUpEnviroment();
        bug.Assign();
        bug.InProgress();
        bug.InProgress();
        Assert.AreEqual(bug.getState(), Bug.State.InProgress);
    }
    [TestMethod]
    public void TestMethod7()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetUpEnviroment();
        bug.Assign();
        bug.InProgress();
        bug.Assign();
        bug.RollUpEnviroment();
        bug.Close();
        Assert.AreNotEqual(bug.getState(), Bug.State.InProgress);
    }
    [TestMethod]
    public void TestMethod8()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetUpEnviroment();
        bug.SetUpEnviroment();
        Assert.AreEqual(bug.getState(), Bug.State.SetUpEnviroment);
    }
    [TestMethod]
    public void TestMethod9()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetUpEnviroment();
        bug.Assign();
        bug.InProgress();
        bug.Defer();
        bug.Assign();
        bug.Syncronizing();
        bug.RollUpEnviroment();
        bug.Close();
        Assert.AreEqual(bug.getState(), Bug.State.Closed);
    }
    [TestMethod]
    public void TestMethod10()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetUpEnviroment();
        Assert.AreEqual(bug.isEnviromentSetUped(), true);
    }
    [TestMethod]
    public void TestMethod11()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetUpEnviroment();
        Assert.AreEqual(bug.isEnviromentRollUped(), false);
    }
    [TestMethod]
    public void TestMethod12()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetUpEnviroment();
        bug.Assign();
        bug.InProgress();
        Assert.AreEqual(bug.isEnviromentRollUped(), true);
    }
    [TestMethod]
    public void TestMethod13()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetUpEnviroment();
        bug.Assign();
        bug.InProgress();
        bug.RollUpEnviroment();
        bug.Close();
        Assert.AreEqual(bug.isEnviromentSetUped(), false);
    }
    [TestMethod]
    public void TestMethod14()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetUpEnviroment();
        bug.Assign();
        try { bug.Close(); }catch(Exception e) { Assert.AreEqual(bug.isEnviromentRollUped(), false); }
           }
    [TestMethod]
    public void TestMethod15()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetUpEnviroment();
        bug.RollUpEnviroment();
        bug.Close();
        Assert.AreEqual(bug.getState(), Bug.State.Closed);
    }
    [TestMethod]
    public void TestMethod16()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetUpEnviroment();
        bug.Assign();
        bug.RollUpEnviroment();
        bug.Close();
        Assert.AreEqual(bug.getState(), Bug.State.Closed);
    }
    [TestMethod]
    public void TestMethod17()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetUpEnviroment();
        bug.Assign();
        bug.InProgress();
        bug.Assign();
        bug.Close();
        try
        {
            bug.Close();
        }catch (Exception e) { Assert.AreEqual(bug.isEnviromentRollUped() , false); }
           }
    [TestMethod]
    public void TestMethod18()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetUpEnviroment();
        bug.Syncronizing();
        bug.RollUpEnviroment();
        bug.Close();
        Assert.AreEqual(bug.getState(), Bug.State.Closed);
    }
    [TestMethod]
    public void TestMethod19()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetUpEnviroment();
        bug.Assign();
        bug.Syncronizing();
        bug.Defer();
        bug.Assign();
        bug.RollUpEnviroment();
        bug.Close();
        Assert.AreEqual(bug.getState(), Bug.State.Closed);
    }
    [TestMethod]
    public void TestMethod20()
    {
        var bug = new Bug(Bug.State.Open);
        bug.SetUpEnviroment();
        bug.Assign();
        bug.Syncronizing();
        bug.Defer();
        bug.Assign();
        bug.RollUpEnviroment();
        bug.Close();
        Assert.AreEqual(bug.getState(), Bug.State.Closed);
    }
}