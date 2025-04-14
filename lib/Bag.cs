using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stateless;

namespace ST_4
{
    public class Bug
    {
        public enum State { Open, Assigned, Defered, Closed, Solved, N_solved }
        private enum Trigger { Assign, Defer, Close, Solve, N_solve }
        private StateMachine<State, Trigger> sm;

        public Bug()
        {
            sm = new StateMachine<State, Trigger>(State.Open);
            sm.Configure(State.Open)
                  .Permit(Trigger.Assign, State.Assigned);
            sm.Configure(State.Assigned)
                  .Permit(Trigger.Close, State.Closed)
                  .Permit(Trigger.Solve, State.Solved)
                  .Permit(Trigger.N_solve, State.N_solved);
            sm.Configure(State.Closed)
                  .Permit(Trigger.Assign, State.Assigned);
            sm.Configure(State.Solved)
                  .Permit(Trigger.Assign, State.Assigned)
                  .Permit(Trigger.Close, State.Closed);
            sm.Configure(State.N_solved)
                  .Permit(Trigger.Close, State.Closed)
                  .Permit(Trigger.Assign, State.Assigned)
                  .Permit(Trigger.Defer, State.Defered);
            sm.Configure(State.Defered)
                  .Permit(Trigger.Assign, State.Assigned)
                  .Permit(Trigger.Solve, State.Solved);
        }
        public void Close()
        {
            sm.Fire(Trigger.Close);
            Console.WriteLine("Close");
        }
        public void Assign()
        {
            sm.Fire(Trigger.Assign);
            Console.WriteLine("Assign");
        }
        public void Defer()
        {
            sm.Fire(Trigger.Defer);
            Console.WriteLine("Defer");
        }
        public void Solve()
        {
            sm.Fire(Trigger.Solve);
            Console.WriteLine("Solve");
        }
        public void N_Solve()
        {
            sm.Fire(Trigger.N_solve);
            Console.WriteLine("N_Solve");
        }
        public State getState()
        {
            return sm.State;
        }
    }
}
