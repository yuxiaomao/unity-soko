using System;

/// <summary>
/// https://weblogs.asp.net/stefansedich/enum-with-string-values-in-c
/// <para></para>
/// This attribute is used to represent a string value for a value in an enum.
/// <code>public enum Test : int {
/// [StringValue("a")]
/// Foo = 1,
/// [StringValue("b")]
/// Something = 2
/// } </code>
/// <code>Test.Foo.GetStringValue()</code>
/// </summary>
public class StringValueAttribute : Attribute
{
    public string StringValue { get; protected set; }

    public StringValueAttribute(string value)
    {
        this.StringValue = value;
    }
}
