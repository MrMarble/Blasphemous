# Blasphemous Extractor ![Blasphemous Version](https://img.shields.io/badge/Blasphemous-3.0.32-blue)

DLL to extract internal game data.

## Why?

I started playing the game and realized how bad I am so wanted to cheese it (in a cool way).

## How to use

You'll need a C# Mono injector. Like [SharpMonoInjector](https://github.com/warbler/SharpMonoInjector/releases/tag/v2.2) (the console version, gui just doesn't want to start)

1. Open Blasphemous and load a savegame
2. Inject the DLL
   - If using SharpMonoInjector: 
    ```bash
    smi.exe inject -p Blasphemous -a <DLL_PATH> -n BlasphemousExtractor -c Loader -m Init
    ```
3. Back to the game, `DLL INJECTED` should appear in the middle of the screen
4. Click on the top left button to save the extracted data to the clipboard or the lower left one to enable the debug console
5. Profit

## Data extracted

Currently the DLL generates a JSON object with the following structure

```JSON5
{
    //...
    "53_41": { // Coordinates on map (X_Y)
        "ZoneId": "D08_Z01_S01", // District, Zone, Scene
        "Type": "Normal", // Normal room or special (checkpoint, shop...)
        "Sprite": "_W_W" // Structure of the cell from left, top, right and bottom (_ => Empty, W => Wall, D => Door)
    },
    //...
}
```