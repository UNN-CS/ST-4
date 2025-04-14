using ST_4;

public class Program
{
    public static void Main(string[] args)
    {
        var bug = new Bug();
        bug.Assign();
        bug.Close();
        bug.Assign();
        bug.N_Solve();
        bug.Defer();
        bug.Assign();
        Console.WriteLine(bug.getState());
    }
}