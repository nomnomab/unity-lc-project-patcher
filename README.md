<div align="center">
  <h1>Lethal Company Project Patcher</h1>

  <img src="Assets~/header.jpg" />

  <p>
    A game wrapper that generates a Unity project from Lethal Company's build that can be playable in-editor
  </p>
</div>

<div align="center">
<!-- Badges -->

<span></span>
<a href="https://github.com/nomnomab/unity-project-patcher">Unity Project Patcher</a>
<span> · </span>
<a href="https://github.com/nomnomab/unity-project-patcher/issues/">Report Bug</a>
<span> · </span>
<a href="https://github.com/nomnomab/unity-project-patcher/issues/">Request Feature</a>
</h4>

</div>

<br />

<!-- Table of Contents -->
# Table of Contents

- [About the Project](#about-the-project)
- [Getting Started](#getting-started)
    * [Prerequisites](#prerequisites)
    * [Installation](#installation)
- [Usage](#usage)
- [FAQ](#faq)

<!-- About the Project -->
## About the Project
This tool is a game wrapper on top of the [Unity Project Patcher](https://github.com/nomnomab/unity-project-patcher).

This takes a build of Lethal Company, extracts its assets/scripts/etc, and then generates a project for usage in the Unity editor.

> [!IMPORTANT]  
> This tool does not distribute game files. It simply works off of your copy of the game!
>
> Also, this tool is for **personal** use only. Do not re-distrubute game files to others.

<!-- Getting Started -->
## Getting Started

<!-- Prerequisites -->
### Prerequisites

You will have to make sure you have the following before using the tool in any way:

- [Git](https://git-scm.com/download/win)
- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
    - To run Asset Ripper
- [Unity Project Patcher](https://github.com/nomnomab/unity-project-patcher)
- [Unity Project Patcher BepInEx](https://github.com/nomnomab/unity-project-patcher-bepinex)
  - [Can be disabled](#disabling-bepinex-usage)

<!-- Installation -->
## Installation

### Unity Project

- Requires [Unity 2022.3.9f1](https://unity.com/releases/editor/whats-new/2022.3.9)
- Unity HDRP pipeline (High Definition 3D)

Create a new Unity project with the above requirements before getting started.

> [!IMPORTANT]  
> These options require [git](https://git-scm.com/download/win) to be installed!

### Installing the Unity Project Patcher core

Install with the package manager:

1. Open the Package Manager from `Window > Package Manager`
2. Click the '+' button in the top-left of the window
3. Click 'Add package from git URL'
4. Provide the URL of the this git repository: `https://github.com/nomnomab/unity-project-patcher.git`
    - If you are using a specific version, you can append it to the end of the git URL, such as `#v1.2.3`
5. Click the 'add' button

Install with the manifest.json:

1. Open the manifest at `[PROJECT_NAME]\Packages\manifest.json`
2. Insert the following as an entry:

```json
"com.nomnom.unity-project-patcher": "https://github.com/nomnomab/unity-project-patcher.git"
```

- If you are using a specific version, you can append it to the end of the git URL, such as `#v1.2.3`

### Installing this Game Wrapper

The same steps as previously, just with `https://github.com/nomnomab/unity-lc-project-patcher.git`

### Installing the BepInEx Wrapper

Open the tool window `Tools > Unity Project Patcher > Open Window` and press the `Install BepInEx` button.

Otherwise, follow the steps at https://github.com/nomnomab/unity-project-patcher-bepinex

#### Disabling BepInEx Usage

If you don't want to use plugins, then follow the steps at https://github.com/nomnomab/unity-project-patcher-bepinex#disabling-this-package

<!-- Usage -->
## Usage

The tool window can be opened via `Tools > Unity Project Patcher > Open Window`

> [!IMPORTANT]  
> This tool mostly supports patching an already patched project, although this can lead to broken assets.
> So make sure you back up your project beforehand.

Estimated patch durations:

- Fresh patch: 5 - 10 minutes
- Already patched: 8 - 15 minutes

These can vary wildly depending on system speed and project size.

> [!IMPORTANT]  
> In the HDRP wizard it may say that the Quality asset isn't assigned. It is lying, just ignore it.

## Plugin Settings

> [!NOTE]  
> Can be made via `Create > Unity Project Patcher > Lethal Company Plugin Settings`
> 
> One will be made for you upon entering the game if one doesn't exist.

![](Assets~/LethalCompanyPluginSettings.png)

There are various provided patches for usage via a `LethalCompanyPluginSettings` asset. 

These can be useful for debugging or quickly entering the game.

## FAQ

The core project's FAQ can be found here: https://github.com/nomnomab/unity-project-patcher#faq

#### Q: The patcher keeps crashing due to texture/GPU/driver issues.

Try switching to DX11 with these steps: https://docs.unity3d.com/Manual/UsingDX11GL3Features.html

<br/>

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/B0B6R2Z9U)
