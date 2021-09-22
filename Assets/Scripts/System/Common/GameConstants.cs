﻿public static class GameConstants
{
    // Common
    public const bool Show = true;
    public const bool Hide = false;

    // Camera
    public const bool CameraToCharacter = true;
    public const bool CameraToLobby = false;
    public const bool CameraOffsetSmooth = true;
    public const bool CameraOffsetNotSmooth = false;

    // UI
    public const bool OpenCharacterSelectionUI = true;
    public const bool CloseCharacterSelectionUI = false;
    public const bool OpenLobbyUI = true;

    // Character
    public const bool CharacterIsControl = true;
    public const bool CharacterIsNotControl = false;
    public const bool TeleportToTrolley = true;
    public const bool TeleportToStartPoint = false;

    // Lights
    public const bool TurnOn = true;
    public const bool TurnOff = false;

    // Sounds
    public const string LobbyBackgroundMusic = "LobbyBackground";
    public const string GameBackgroundMusic = "GameBackground";
    public const string ShotgunSound = "Shotgun";
    public const string LaserSound = "Laser";

    // SceneParams
    public const bool OpenWithLoadingScene = true;
    public const bool OpenWithoutLoadingScene = false;

    // Items
    public const bool Available = true;
    public const bool Unavailable = false;

    // Paths
    public const string CharactersDataPath = "Specifications/Characters";

    // Days
    public const int DayToAdsRewards = 1;
    public const int HoursToDailyReward = 6;

    public const int NumberLevelInGame = 10;
    public const int NumberBiomeInLevel = 5;
}

