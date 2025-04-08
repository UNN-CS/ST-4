[TestClass]
public class BugTests {
                
    // ~ OPEN ~ //

    [TestMethod]
    public void OpenToAssign() {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void OpenToDefer() {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
    }


    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void OpenToWIP() {
        var bug = new Bug(Bug.State.Open);
        bug.WorkInProgress();
        
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void OpenToReview() {
        var bug = new Bug(Bug.State.Open);
        bug.Review();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void OpenToClose() {
        var bug = new Bug(Bug.State.Open);
        bug.Close();
    }

    // ~ ASSIGN ~ //

    [TestMethod]
    public void AssignToWIP() {
        var bug = new Bug(Bug.State.Assigned);
        bug.WorkInProgress();
        Assert.AreEqual(Bug.State.WIP, bug.getState());
    }

    [TestMethod]
    public void AssignToAssign() {
        var bug = new Bug(Bug.State.Assigned);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    public void AssignToDefer() {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AssignToReview() {
        var bug = new Bug(Bug.State.Assigned);
        bug.Review();
    }

    [TestMethod]
    public void AssignToClose() {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    // ~ DEFER ~ //
    
    [TestMethod]
    public void DeferToAssign() {
        var bug = new Bug(Bug.State.Defered);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeferToDefer() {
        var bug = new Bug(Bug.State.Defered);
        bug.Defer();
    }

    [TestMethod]
    public void DeferToWIP() {
        var bug = new Bug(Bug.State.Defered);
        bug.WorkInProgress();
        Assert.AreEqual(Bug.State.WIP, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeferToReview() {
        var bug = new Bug(Bug.State.Defered);
        bug.Review();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeferToClose() {
        var bug = new Bug(Bug.State.Defered);
        bug.Close();
    }

    // ~ WIP ~ //

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void WIPToAssign() {
        var bug = new Bug(Bug.State.WIP);
        bug.Assign();
    }

    [TestMethod]
    public void WIPToDefer() {
        var bug = new Bug(Bug.State.WIP);
        bug.Defer();
        Assert.AreEqual(Bug.State.Defered, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void WIPToWIP() {
        var bug = new Bug(Bug.State.WIP);
        bug.WorkInProgress();
    }

    [TestMethod]
    public void WIPToReview() {
        var bug = new Bug(Bug.State.WIP);
        bug.Review();
        Assert.AreEqual(Bug.State.Reviewed, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void WIPToClose() {
        var bug = new Bug(Bug.State.WIP);
        bug.Close();
    }

    // ~ REVIEW ~ //

    [TestMethod]
    public void ReviewToAssign() {
        var bug = new Bug(Bug.State.Reviewed);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ReviewToDefer() {
        var bug = new Bug(Bug.State.Reviewed);
        bug.Defer();
        
    }

    [TestMethod]
    public void ReviewToWIP() {
        var bug = new Bug(Bug.State.Reviewed);
        bug.WorkInProgress();
        Assert.AreEqual(Bug.State.WIP, bug.getState());
    }

    [TestMethod]
    public void ReviewToReview() {
        var bug = new Bug(Bug.State.Reviewed);
        bug.Review();
        Assert.AreEqual(Bug.State.Reviewed, bug.getState());
    }

    [TestMethod]
    public void ReviewToClose() {
        var bug = new Bug(Bug.State.Reviewed);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    // ~ CLOSE ~ //

    [TestMethod]
    public void CloseToAssign() {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CloseToDefer() {
        var bug = new Bug(Bug.State.Closed);
        bug.Defer();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CloseToWIP() {
        var bug = new Bug(Bug.State.Closed);
        bug.WorkInProgress();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CloseToReview() {
        var bug = new Bug(Bug.State.Closed);
        bug.Review();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CloseToClose() {
        var bug = new Bug(Bug.State.Closed);
        bug.Close();
    }
}
