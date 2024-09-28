namespace Community.PowerToys.Run.Plugin.EdgeWorkspaces.Helpers;

internal static class EdgeHelpers
{
	public static void OpenEdgeProfile(string profileDir, string id)
		=> OpenEdgeWithArgs($"--profile-directory=\"{profileDir}\" --launch-workspace=\"{id}\"");

	public static void OpenEdgeProfile(string profileDir)
		=> OpenEdgeWithArgs($"--profile-directory=\"{profileDir}\"");

	private static void OpenEdgeWithArgs(string args)
		=> Wox.Infrastructure.Helper.OpenInShell(@"shell:AppsFolder\Microsoft.MicrosoftEdge.Stable_8wekyb3d8bbwe!App", args);
}
