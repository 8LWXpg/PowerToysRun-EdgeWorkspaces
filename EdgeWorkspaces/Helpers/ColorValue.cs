using System.Text.Json;
using System.Text.Json.Serialization;

namespace Community.PowerToys.Run.Plugin.EdgeWorkspaces.Helpers;

public class ColorValue(int value)
{
	public int Value { get; } = value;

	/// <summary>
	/// Get the icon path for the color value
	/// </summary>
	/// <param name="iconPath">Path to theme based image folder</param>
	/// <remarks>This does not check if <c>value &lt; 0</c> or <c>value &gt; 13</c></remarks>
	/// <returns>Path to icon</returns>
	public string GetIconPath(string iconPath) => $"{iconPath}\\{Value}.png";
}

internal class ColorValueConverter : JsonConverter<ColorValue>
{
	public override ColorValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return reader.TokenType != JsonTokenType.Number || !reader.TryGetInt32(out var value)
			? throw new JsonException("Invalid value for ColorValue")
			: new ColorValue(value);
	}
	public override void Write(Utf8JsonWriter writer, ColorValue value, JsonSerializerOptions options)
		=> writer.WriteNumberValue(value.Value);
}
