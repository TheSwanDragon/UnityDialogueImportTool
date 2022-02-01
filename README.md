# UnityDialogueImportTool
A straightforward tool that takes a parsed CSV file using an Active Dialogue Format

This tool is designed to be used with, and requires the download of, a free Unity Asset Store asset called `CSV Serialize`:
    https://assetstore.unity.com/packages/tools/integration/csv-serialize-135763
    
## To Set Up:
- Import the `CSV Serialize` package. You should only need the `CSVSerializer.cs` file from it.
    - This can be done through the [Unity Asset Store](https://assetstore.unity.com/packages/tools/integration/csv-serialize-135763), by clicking `Add to My Assets` and then `Open In Unity`
    - Alternatively, if you've already added it to your assets in the Asset Store: `Window -> Package Manger`, selecting `Packages: My Assets`. Find `CSV Serialize` from the list, then click `Import` in the bottom right
- Add TextMeshPro to your Unity Project
    - This can be done by going to `GameObject -> UI -> "Text - TextMeshPro"`, then clicking `Import` on the popup that appears
- Import the `DialogueLineImporter.unitypackage`. You _**do not**_ need to download any other files from this repository—the source code is included here for convenience.

## To Use:
- Save your dialogue spreadsheet as `"<FileName>AF.csv"`. The spreadsheet must be using the same column format as the included example.
- Import the CSV file into Unity wherever you would like the Dialogue asset to be.
    - If you want to update the dialogue, the files must stay in the same folder, and they cannot be renamed unless both are renamed.

## Editor Use:
- **Option 1:** Use the `DialogueLineHolder` component on an object that also has a `TextMeshPro` component
- **Option 2:** Use a `DialogueLineSelector` object, or array of them, in one of your scripts.
