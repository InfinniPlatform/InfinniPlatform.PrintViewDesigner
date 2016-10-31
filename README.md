# InfinniPlatform.PrintViewDesigner

PrintViewDesigner is WYSIWYG editor for printed forms of [InfinniPlatform](https://github.com/InfinniPlatform/InfinniPlatform).

![](.files/Example.png)

# NuGet Source

http://nuget.infinnity.ru/packages/InfinniPlatform.PrintViewDesigner/

# Installation

Download a Windows installation script InfinniPlatform.PrintViewDesigner [here](https://raw.githubusercontent.com/InfinniPlatform/InfinniPlatform.PrintViewDesigner/master/InfinniPlatform.PrintViewDesigner/Install.bat).

The latest version is installed by default:

```bash
Install.bat # installs the latest version of InfinniPlatform.PrintViewDesigner
```

You can install any version of editor from the repository:

```bash
Install.bat <version> # installs specified version of InfinniPlatform.PrintViewDesigner
```

When script finishes InfinniPlatform.PrintViewDesigner will be placed into the folder `InfinniPlatform.PrintViewDesigner.X` (where `X` - version number) in the same folder where script was run. Change folder as in example below:

```bash
InfinniPlatform.PrintViewDesigner.X
```

Run editor:

```bash
InfinniPlatform.PrintViewDesigner.exe
```

# Requirements

To preview a document in PDF format you need to install [wkhtmltopdf 0.12.2.4](http://wkhtmltopdf.org/downloads.html) tool.
