[TestClass]
public sealed class BugTests
{
    [TestMethod]
    public void InitialStateIsSet()
    {
        var bug = new Bug(Bug.State.Open);
        Assert.AreEqual(Bug.State.Open, bug.GetState());
    }

    private static IEnumerable<object[]> BugStateTransitions =>
    [
        [Bug.State.Open, new Action<Bug>((bug) => bug.Assign()), Bug.State.Assigned],
        [Bug.State.Assigned, new Action<Bug>((bug) => bug.Close()), Bug.State.Closed],
        [Bug.State.Closed, new Action<Bug>((bug) => bug.Assign()), Bug.State.Assigned],
        [Bug.State.Assigned, new Action<Bug>((bug) => bug.Defer()), Bug.State.Defered],
        [Bug.State.Defered, new Action<Bug>((bug) => bug.Assign()), Bug.State.Assigned],
        [Bug.State.Assigned, new Action<Bug>((bug) => bug.Select()), Bug.State.Selected],
        [Bug.State.Selected, new Action<Bug>((bug) => bug.Close()), Bug.State.Closed],
        [Bug.State.Defered, new Action<Bug>((bug) => bug.Defer()), Bug.State.Defered],
        [Bug.State.Selected, new Action<Bug>((bug) => bug.Select()), Bug.State.Selected],
        [Bug.State.Closed, new Action<Bug>((bug) => bug.Close()), Bug.State.Closed],
        [Bug.State.Open, new Action<Bug>((bug) => bug.Open()), Bug.State.Open],
    ];

    [DynamicData(nameof(BugStateTransitions))]
    [DataTestMethod]
    public void CanChangeStateFromXToY(Bug.State from, Action<Bug> modify, Bug.State to)
    {
        var bug = new Bug(from);
        modify(bug);
        Assert.AreEqual(to, bug.GetState());
    }

    private static IEnumerable<object[]> BugStateInvalidTransitions =>
    [
        [Bug.State.Open, new Action<Bug>((bug) => bug.Close())],
        [Bug.State.Closed, new Action<Bug>((bug) => bug.Defer())],
        [Bug.State.Defered, new Action<Bug>((bug) => bug.Close())],
        [Bug.State.Selected, new Action<Bug>((bug) => bug.Assign())],
        [Bug.State.Closed, new Action<Bug>((bug) => bug.Open())],
    ];

    [DynamicData(nameof(BugStateInvalidTransitions))]
    [DataTestMethod]
    public void ChangingStateFromXToYThrowsException(Bug.State from, Action<Bug> modify)
    {
        var bug = new Bug(from);
        Assert.ThrowsException<InvalidOperationException>(() => modify(bug));
    }

    private static IEnumerable<object[]> BugStateIdempotentTransitions =>
    [
        [Bug.State.Assigned, new Action<Bug>((bug) => bug.Assign())],
        [Bug.State.Defered, new Action<Bug>((bug) => bug.Defer())],
        [Bug.State.Closed, new Action<Bug>((bug) => bug.Close())],
        [Bug.State.Open, new Action<Bug>((bug) => bug.Open())],
    ];

    [DynamicData(nameof(BugStateIdempotentTransitions))]
    [DataTestMethod]
    public void ChangingStateIsIdempotent(Bug.State from, Action<Bug> modify)
    {
        var bug = new Bug(from);
        modify(bug);
        Assert.AreEqual(from, bug.GetState());
    }
}
