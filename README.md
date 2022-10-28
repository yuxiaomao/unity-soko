# Unity-Soko: Unity based sokoban

Learning project

# Dependencies
- Unity Editor Version: `2021.3.12f1`
- `Assets/Audio/BGM/`: 魔王魂 MaouDamashii
  - `maou_bgm_8bit01.mp3`: [8bit01](https://maou.audio/bgm_8bit01/)
- `Assets/Audio/SE/`: 小森平 Taira Komori
  - `move.mp3`: [select01](https://taira-komori.jpn.org/game01.html)
  - `error.mp3`: [blip02](https://taira-komori.jpn.org/game01.html)
  - `win.mp3`: [crrect_answer2](https://taira-komori.jpn.org/game01.html)

# Implemented features
- Gameplay
  - Support multiples player objects in level
  - Reset level with `R`, undo level with `Z`
- Menu: main menu, pause menu
  - Buttons generated by script (voluntary to facilitate adaptation), navigation with mouse/keyboard
- Audio: Music and sound effets
- Editor
  - Level: store/load level data from/to scene with custom editor button (LevelManager)
- Internal implementation
  - Object: each object (player, box, target, ...) works independently, and does not know the global state of the game (voluntary to make difference with the pico-8 version)
  - User input: with Unity Input System package
  - Scenes: menu and levels are in separate Unity scenes (voluntary to experiment cross-scene functionalities)
  - Dynamic asset load from script: with Unity Addressable package

# Development tools
- Unity on Windows
- Visual Studio for Unity
- Visual Studio Code
- git for Windows
