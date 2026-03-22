namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Reserved to be used by the compiler for tracking metadata about the IsExternalInit type.
    /// This polyfill allows using `record` types in projects targeting .NET Framework or older .NET versions.
    /// In .NET 5+, this type is provided by the runtime.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class IsExternalInit
    {
    }
}
