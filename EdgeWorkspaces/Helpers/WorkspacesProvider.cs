using Community.PowerToys.Run.Plugin.EdgeWorkspaces.Properties;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.EdgeWorkspaces.Helpers;

public partial class WorkspacesProvider
{
	public readonly IEnumerable<EdgeProfileWorkspace> WorkspaceEntries;

	public IEnumerator<EdgeProfileWorkspace> GetEnumerator() => WorkspaceEntries.GetEnumerator();

	public WorkspacesProvider()
	{
		var basePath = Path.Combine(Environment.GetFolderPath(
			Environment.SpecialFolder.LocalApplicationData),
			"Microsoft", "Edge", "User Data");
		IEnumerable<string> profileDirectories = Directory.GetDirectories(basePath)
			.Where(static d => WorkspaceProfileRegex().IsMatch(Path.GetFileName(d)) && File.Exists(GetWorkspacesCache(d)));
		WorkspaceEntries = profileDirectories.Select(static d =>
		{
			try
			{
				using FileStream workspaceCacheStream = File.OpenRead(GetWorkspacesCache(d));
				using FileStream preferencesStream = File.OpenRead(GetPreferences(d));
				WorkspacesCache workspaceCache = JsonSerializer.Deserialize<WorkspacesCache>(workspaceCacheStream)!;
				Preferences preferences = JsonSerializer.Deserialize<Preferences>(preferencesStream)!;
				return new EdgeProfileWorkspace
				{
					ProfileDir = Path.GetFileName(d),
					ProfileName = preferences.Profile.Name,
					AccountType = preferences.Sync.AccountType,
					Workspaces = workspaceCache.Workspaces,
				};
			}
			catch (Exception e)
			{
				Log.Exception($"Failed to load workspaces for profile {d}", e, typeof(WorkspacesProvider));
				throw;
			}
		});
	}

	private static string GetWorkspacesCache(string profilePath) => Path.Combine(profilePath, "Workspaces", "WorkspacesCache");
	private static string GetPreferences(string profilePath) => Path.Combine(profilePath, "Preferences");

	[GeneratedRegex(@"(Default|Profile \d+)$")]
	private static partial Regex WorkspaceProfileRegex();
}

public record EdgeProfileWorkspace : WorkspacesCache
{
	public required string ProfileDir { get; init; }
	public required string ProfileName { get; init; }
	public required AccountType AccountType { get; init; }
}

internal record Preferences
{
	[JsonPropertyName("profile")]
	public required Profile Profile { get; init; }

	[JsonPropertyName("sync")]
	public required Sync Sync { get; init; }
}

internal record Profile
{
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}

internal record Sync
{
	[JsonPropertyName("edge_account_type")]
	public required AccountType AccountType { get; init; }
}

public enum AccountType
{
	Local = 0,
	Personal = 1,
	Work = 2,
}

internal static class AccountTypeExtensions
{
	public static string ToLocalizedString(this AccountType accountType)
		=> accountType switch
		{
			AccountType.Local => Resources.account_type_local,
			AccountType.Personal => Resources.account_type_personal,
			AccountType.Work => Resources.account_type_work,
			_ => throw new NotImplementedException(),
		};
}

public record WorkspacesCache
{
	[JsonPropertyName("workspaces")]
	public required IEnumerable<Workspaces> Workspaces { get; init; }
}

public record Workspaces
{
	[JsonPropertyName("color")]
	[JsonConverter(typeof(ColorValueConverter))]
	public required ColorValue Color { get; init; }

	[JsonPropertyName("id")]
	public required string Id { get; init; }

	[JsonPropertyName("menuSubtitle")]
	public required string MenuSubtitle { get; init; }

	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
