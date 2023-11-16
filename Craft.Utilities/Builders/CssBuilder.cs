using System.Text;

namespace Craft.Utilities.Builders;

public readonly struct CssBuilder(string value)
{
    private readonly StringBuilder stringBuffer = new(value);

    public static CssBuilder Default(string value) => new(value);

    public readonly string Build()
        => stringBuffer != null ? stringBuffer.ToString().Trim() : string.Empty;

    public override readonly string ToString() => Build();

    public readonly CssBuilder AddValue(string value)
    {
        stringBuffer.Append(value);
        return this;
    }

    public readonly CssBuilder AddClass(string value) => AddValue(" " + value);

    public readonly CssBuilder AddClass(string value, bool when = true)
        => when ? AddClass(value) : this;

    public readonly CssBuilder AddClass(string value, Func<bool> when = null)
        => AddClass(value, when != null && when());

    public readonly CssBuilder AddClass(Func<string> value, bool when = true)
        => when ? AddClass(value()) : this;

    public readonly CssBuilder AddClass(Func<string> value, Func<bool> when = null)
        => AddClass(value, when != null && when());

    public readonly CssBuilder AddClass(CssBuilder builder, bool when = true)
        => when ? AddClass(builder.Build()) : this;

    public readonly CssBuilder AddClass(CssBuilder builder, Func<bool> when = null)
        => AddClass(builder, when != null && when());

    public readonly CssBuilder AddClassFromAttributes(IReadOnlyDictionary<string, object> additionalAttributes)
        => additionalAttributes == null
        ? this
        : additionalAttributes.TryGetValue("class", out var c)
            ? AddClass(c.ToString())
            : this;
}
