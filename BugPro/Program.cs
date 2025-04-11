using Stateless;
using System;

namespace BugStateMachine {
    public class Bug {
        public enum State {
            Open,
            Assigned,
            InProgress,
            InReview,
            Verified,
            Deferred,
            Closed,
            Reopened,
            Rejected
        }

        private enum Trigger {
            Assign,
            StartProgress,
            SubmitForReview,
            ApproveFix,
            RejectFix,
            Verify,
            Defer,
            Close,
            Reopen,
            Reject
        }

        private StateMachine<State, Trigger> sm;

        public Bug(State initialState)
        {
            sm = new StateMachine<State, Trigger>(initialState);

            sm.Configure(State.Open)
                .Permit(Trigger.Assign, State.Assigned)
                .Permit(Trigger.Reject, State.Rejected);

            sm.Configure(State.Assigned)
                .Permit(Trigger.StartProgress, State.InProgress)
                .Permit(Trigger.Defer, State.Deferred)
                .Permit(Trigger.Close, State.Closed)
                .Ignore(Trigger.Assign);

            sm.Configure(State.InProgress)
                .Permit(Trigger.SubmitForReview, State.InReview)
                .Permit(Trigger.Defer, State.Deferred)
                .Permit(Trigger.Close, State.Closed);

            sm.Configure(State.InReview)
                .Permit(Trigger.ApproveFix, State.Verified)
                .Permit(Trigger.RejectFix, State.Assigned);

            sm.Configure(State.Verified)
                .Permit(Trigger.Close, State.Closed);

            sm.Configure(State.Deferred)
                .Permit(Trigger.Assign, State.Assigned);

            sm.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Reopened);

            sm.Configure(State.Reopened)
                .Permit(Trigger.Assign, State.Assigned);

            sm.Configure(State.Rejected)
                .Permit(Trigger.Reopen, State.Reopened);
        }

        public void Assign() => sm.Fire(Trigger.Assign);
        public void StartProgress() => sm.Fire(Trigger.StartProgress);
        public void SubmitForReview() => sm.Fire(Trigger.SubmitForReview);
        public void ApproveFix() => sm.Fire(Trigger.ApproveFix);
        public void RejectFix() => sm.Fire(Trigger.RejectFix);
        public void Verify() => sm.Fire(Trigger.Verify);
        public void Defer() => sm.Fire(Trigger.Defer);
        public void Close() => sm.Fire(Trigger.Close);
        public void Reopen() => sm.Fire(Trigger.Reopen);
        public void Reject() => sm.Fire(Trigger.Reject);

        public State GetState() => sm.State;
    }

    public class Program {
        public static void Main(string[] args) {
            var bug = new Bug(Bug.State.Open);

            bug.Assign();
            bug.StartProgress();
            bug.SubmitForReview();
            bug.ApproveFix();
            bug.Close();  // Close the bug once
            bug.Reopen();
            bug.Assign();
            bug.Defer();
            bug.Assign();
            bug.StartProgress();
            bug.SubmitForReview();
            bug.RejectFix();
            bug.Close();  // Close the bug once more after fixing the issue

            Console.WriteLine($"Current bug state: {bug.GetState()}");
        }
    }
}
