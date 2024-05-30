using Stateless;

public class Bug
{
    public enum Condition { Open, Assigned, Def, Closed }
    private enum Catalyst { Assign, Defer, Close }
    private StateMachine<Condition, Catalyst> sm;

    public Bug(Condition state)
    {
        sm = new StateMachine<Condition, Catalyst>(state);
        sm.Configure(Condition.Open)
              .Permit(Catalyst.Assign, Condition.Assigned);
        sm.Configure(Condition.Assigned)
              .Permit(Catalyst.Close, Condition.Closed)
              .Permit(Catalyst.Defer, Condition.Def)
              .Ignore(Catalyst.Assign);
        sm.Configure(Condition.Closed)
              .Permit(Catalyst.Assign, Condition.Assigned);
        sm.Configure(Condition.Def)
              .Permit(Catalyst.Assign, Condition.Assigned);
    }
    public void Close()
    {
        sm.Fire(Catalyst.Close);
        Console.WriteLine("Close");
    }
    public void Assign()
    {
        sm.Fire(Catalyst.Assign);
        Console.WriteLine("Assign");
    }
    public void Defer()
    {
        sm.Fire(Catalyst.Defer);
        Console.WriteLine("Defer");
    }
    public Condition getCondition()
    {
        return sm.State;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var bug = new Bug(Bug.Condition.Open);
        bug.Assign();
        bug.Close();
        bug.Assign();
        bug.Defer();
        bug.Assign();
        Console.WriteLine(bug.getCondition());
    }
}
