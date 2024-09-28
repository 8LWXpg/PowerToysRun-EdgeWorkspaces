using Community.PowerToys.Run.Plugin.EdgeWorkspaces.Helpers;
using Community.PowerToys.Run.Plugin.EdgeWorkspaces.Properties;
using ManagedCommon;
using System.Windows.Controls;
using System.Windows.Input;
using Wox.Infrastructure;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.EdgeWorkspaces;

public class Main : IPlugin, IPluginI18n, IContextMenu, IReloadable, IDisposable
{
	private PluginInitContext? _context;
	private string? _iconFolder;
	private bool _disposed;
	public string Name => Resources.plugin_name;
	public string Description => Resources.plugin_description;
	public static string PluginID => "83fc8324db384db4978d41f183ed5b30";

	public List<Result> Query(Query query)
	{
		ArgumentNullException.ThrowIfNull(query);

		var result = new WorkspacesProvider().WorkspaceEntries.SelectMany(pf => pf.Workspaces.Select(ws =>
		{
			MatchResult matchResult = StringMatcher.FuzzySearch(query.Search, ws.Name);
			return new Result
			{
				Title = $"{ws.Name} ({pf.AccountType.ToLocalizedString()}: {pf.ProfileName})",
				TitleHighlightData = matchResult.MatchData,
				SubTitle = ws.MenuSubtitle,
				ToolTipData = new ToolTipData(Resources.launch_workspace, ""),
				Score = matchResult.Score,
				IcoPath = ws.Color.GetIconPath(_iconFolder!),
				ContextData = pf.ProfileDir,
				Action = _ =>
				{
					EdgeHelpers.OpenEdgeProfile(pf.ProfileDir, ws.Id);
					return true;
				},
			};
		})).ToList();

		if (!string.IsNullOrEmpty(query.Search))
		{
			_ = result.RemoveAll(r => r.Score <= 0);
		}

		return result;
	}

	public List<ContextMenuResult> LoadContextMenus(Result selectedResult)
	{
		ArgumentNullException.ThrowIfNull(selectedResult);

		return
		[
			new ()
			{
				Title = Resources.open_profile,
				FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
				Glyph = "\xE7EE",
				AcceleratorKey = Key.P,
				AcceleratorModifiers = ModifierKeys.Control|ModifierKeys.Shift,
				Action = _ =>
				{
					EdgeHelpers.OpenEdgeProfile((string)selectedResult.ContextData);
					return true;
				}
			},
		];
	}

	public void Init(PluginInitContext context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
		_context.API.ThemeChanged += OnThemeChanged;
		UpdateIconPath(_context.API.GetCurrentTheme());
	}

	public string GetTranslatedPluginTitle() => Resources.plugin_name;

	public string GetTranslatedPluginDescription() => Resources.plugin_description;

	private void OnThemeChanged(Theme oldTheme, Theme newTheme) => UpdateIconPath(newTheme);

	private void UpdateIconPath(Theme theme)
	{
		_iconFolder = theme is Theme.Light or Theme.HighContrastWhite
			? "Images/Light"
			: "Images/Dark";
	}

	public Control CreateSettingPanel() => throw new NotImplementedException();

	public void ReloadData()
	{
		if (_context is null)
		{
			return;
		}

		UpdateIconPath(_context.API.GetCurrentTheme());
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed && disposing)
		{
			if (_context != null && _context.API != null)
			{
				_context.API.ThemeChanged -= OnThemeChanged;
			}

			_disposed = true;
		}
	}
}
