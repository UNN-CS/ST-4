namespace BugPro;

public enum BugState
{
    New,
    Triage,
    AwaitingInfo,
    OtherProduct,
    NeedDecisionLater,
    NotABug,
    WontFix,
    Duplicate,
    NotReproducible,
    InWork,
    Review,
    Returned,
    Deferred,
    Closed
}