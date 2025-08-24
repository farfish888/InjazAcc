namespace InjazAcc.Shared;

public record Result(bool Success, string Message)
{
    public static Result Ok(string m = "OK") => new(true, m);
    public static Result Fail(string m) => new(false, m);
}
