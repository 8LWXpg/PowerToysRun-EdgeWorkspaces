# EdgeWorkspaces for PowerToys Run

A [PowerToys Run](https://aka.ms/PowerToysOverview_PowerToysRun) plugin to open Edge Workspaces.

Derive from [quachpas/PowerToys-Run-EdgeWorkspaces](https://github.com/quachpas/PowerToys-Run-EdgeWorkspaces), removed runtime icon drawing and rewrote from scratch. Mainly developed for self use, but bug report and feature request is welcomed!

Checkout the [Template](https://github.com/8LWXpg/PowerToysRun-PluginTemplate) for a starting point to create your own plugin.

## Features

### Open Edge Workspaces

![Open Edge Workspaces](./assets/demo.png)

## Installation

### Manual

1. Download the latest release of the from the releases page.
2. Extract the zip file's contents to `%LocalAppData%\Microsoft\PowerToys\PowerToys Run\Plugins`
3. Restart PowerToys.

### Via [ptr](https://github.com/8LWXpg/ptr)

```shell
ptr add EdgeWorkspaces 8LWXpg/PowerToysRun-EdgeWorkspaces
```

## Usage

1. Open PowerToys Run (default shortcut is <kbd>Alt+Space</kbd>).
2. Type `}` and search for Edge workspaces.

## Building

1. Clone the repository and the dependencies in `/lib` with `EdgeWorkspaces/copyLib.ps1`.
2. run `dotnet build -c Release`.

<details>
<summary>Generating images</summary>

#### Prerequisites

- [oxipng](https://github.com/shssoichiro/oxipng)
- [Inkscape](https://inkscape.org)

Both can be installed with `winget`

```shell
winget install oxipng inkscape
```

After that, you can run the `generateImages.ps1` script (Powershell 7) to generate the images.

</details>

## Debugging

1. Clone the repository and the dependencies in `/lib` with `EdgeWorkspaces/copyLib.ps1`.
2. Build the project in `Debug` configuration.
3. Make sure you have [gsudo](https://github.com/gerardog/gsudo) installed in the path.
4. Run `debug.ps1` (change `$ptPath` if you have PowerToys installed in a different location).
5. Attach to the `PowerToys.PowerLauncher` process in Visual Studio.

## Contributing

### Localization

If you want to help localize this plugin, please check the [localization guide](./Localizing.md)
