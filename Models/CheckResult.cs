namespace Win11Inspector.Models;

public enum CheckState
{
    Pass,
    Warning,
    Fail
}

public sealed record CheckResult(
    string Name,
    string Value,
    CheckState State,
    string Details,
    bool BypassPossible = false);