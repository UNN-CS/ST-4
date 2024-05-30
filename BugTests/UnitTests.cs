namespace BugTests;

[TestClass]
public class UnitTests
{
    [TestMethod]
    public void Transition_ClosedToAssigned()
    {
        // Arrange
        var bug = new Bug(Bug.State.Closed);

        // Act
        bug.Assign();

        // Assert
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void Transition_DeferedToAssigned()
    {
        // Arrange
        var bug = new Bug(Bug.State.Defered);

        // Act
        bug.Assign();

        // Assert
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void Transition_OpenToAssigned()
    {
        // Arrange
        var bug = new Bug(Bug.State.Open);

        // Act
        bug.Assign();

        // Assert
        Assert.AreEqual(Bug.State.Assigned, bug.GetState());
    }

    [TestMethod]
    public void Transition_CreatedFixesToAcceptedFixes()
    {
        // Arrange
        var bug = new Bug(Bug.State.CreatedFixes);

        // Act
        bug.AcceptFix();

        // Assert
        Assert.AreEqual(Bug.State.AcceptedFixes, bug.GetState());
    }

    [TestMethod]
    public void Transition_CreatedFixesToDeclinedFixes()
    {
        // Arrange
        var bug = new Bug(Bug.State.CreatedFixes);

        // Act
        bug.DeclineFix();

        // Assert
        Assert.AreEqual(Bug.State.DeclinedFixes, bug.GetState());
    }

    [TestMethod]
    public void Transition_AssignedToClosed()
    {
        // Arrange
        var bug = new Bug(Bug.State.Assigned);

        // Act
        bug.Close();

        // Assert
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void Transition_AssignedToDefered()
    {
        // Arrange
        var bug = new Bug(Bug.State.Assigned);

        // Act
        bug.Defer();

        // Assert
        Assert.AreEqual(Bug.State.Defered, bug.GetState());
    }

    [TestMethod]
    public void Transition_AcceptedFixesToClosed()
    {
        // Arrange
        var bug = new Bug(Bug.State.AcceptedFixes);

        // Act
        bug.Close();

        // Assert
        Assert.AreEqual(Bug.State.Closed, bug.GetState());
    }

    [TestMethod]
    public void Transition_DeclinedFixesToCreatedFixes()
    {
        // Arrange
        var bug = new Bug(Bug.State.DeclinedFixes);

        // Act
        bug.CreateFix();

        // Assert
        Assert.AreEqual(Bug.State.CreatedFixes, bug.GetState());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void InvalidTransition_AcceptFixFromClosed()
    {
        // Arrange
        var bug = new Bug(Bug.State.Closed);

        // Act
        bug.AcceptFix();
    }
}
