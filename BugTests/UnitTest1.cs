[TestClass]
public class BugTests {
    [TestMethod]
    public void OpenToAssigned() {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void OpenToDeferred() {
        var bug = new Bug(Bug.State.Open);
        bug.Defer();
    }


    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void OpenToInProgress() {
        var bug = new Bug(Bug.State.Open);
        bug.InProgress();
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

    [TestMethod]
    public void AssignedToDeferred() {
        var bug = new Bug(Bug.State.Assigned);
        bug.Defer();
        Assert.AreEqual(Bug.State.Deferred, bug.getState());
    }

    [TestMethod]
    public void AssignedToInProgress() {
        var bug = new Bug(Bug.State.Assigned);
        bug.InProgress();
        Assert.AreEqual(Bug.State.InProgress, bug.getState());
    }

    [TestMethod]
    public void AssignedToAssigned() {
        var bug = new Bug(Bug.State.Assigned);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AssignedToReviewed() {
        var bug = new Bug(Bug.State.Assigned);
        bug.Review();
    }

    [TestMethod]
    public void AssignedToClosed() {
        var bug = new Bug(Bug.State.Assigned);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void DeferredToInProgress() {
        var bug = new Bug(Bug.State.Deferred);
        bug.InProgress();
        Assert.AreEqual(Bug.State.InProgress, bug.getState());
    }

    [TestMethod]
    public void DeferredToAssigned() {
        var bug = new Bug(Bug.State.Deferred);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeferredToDeferred() {
        var bug = new Bug(Bug.State.Deferred);
        bug.Defer();
    }


    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeferredToReviewed() {
        var bug = new Bug(Bug.State.Deferred);
        bug.Review();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DeferredToClosed() {
        var bug = new Bug(Bug.State.Deferred);
        bug.Close();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void InProgressToAssigned() {
        var bug = new Bug(Bug.State.InProgress);
        bug.Assign();
    }

    [TestMethod]
    public void InProgressToReviewed() {
        var bug = new Bug(Bug.State.InProgress);
        bug.Review();
        Assert.AreEqual(Bug.State.Reviewed, bug.getState());
    }

    [TestMethod]
    public void InProgressToDeferred() {
        var bug = new Bug(Bug.State.InProgress);
        bug.Defer();
        Assert.AreEqual(Bug.State.Deferred, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void InProgressToInProgress() {
        var bug = new Bug(Bug.State.InProgress);
        bug.InProgress();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void InProgressToCLosed() {
        var bug = new Bug(Bug.State.InProgress);
        bug.Close();
    }

    [TestMethod]
    public void ReviewedToAssigned() {
        var bug = new Bug(Bug.State.Reviewed);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ReviewedToDeferred() {
        var bug = new Bug(Bug.State.Reviewed);
        bug.Defer();
    }

    [TestMethod]
    public void ReviewedToInProgress() {
        var bug = new Bug(Bug.State.Reviewed);
        bug.InProgress();
        Assert.AreEqual(Bug.State.InProgress, bug.getState());
    }

    [TestMethod]
    public void ReviewedToReviewed() {
        var bug = new Bug(Bug.State.Reviewed);
        bug.Review();
        Assert.AreEqual(Bug.State.Reviewed, bug.getState());
    }

    [TestMethod]
    public void ReviewedToClosed() {
        var bug = new Bug(Bug.State.Reviewed);
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.getState());
    }

    [TestMethod]
    public void ClosedToAssigned() {
        var bug = new Bug(Bug.State.Closed);
        bug.Assign();
        Assert.AreEqual(Bug.State.Assigned, bug.getState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClosedToDeferred() {
        var bug = new Bug(Bug.State.Closed);
        bug.Defer();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClosedToInProgress() {
        var bug = new Bug(Bug.State.Closed);
        bug.InProgress();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClosedToReviewed() {
        var bug = new Bug(Bug.State.Closed);
        bug.Review();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClosedToClosed() {
        var bug = new Bug(Bug.State.Closed);
        bug.Close();
    }
}
