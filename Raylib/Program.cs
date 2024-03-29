﻿namespace CaptainCoder.Dungeoneering.Raylib;

using System.Text.Json;

using Raylib_cs;

public class Program
{
    public const string ConfigFile = "dungeoneering.config";
    public static IScreen Screen { get; set; } = new DungeonEditorScreen();
    public static ScreenConfig Config { get; set; } = InitScreenConfig();
    private static bool s_isRunning = true;

    public static void Main()
    {
        InitWindow();
        Raylib.InitAudioDevice();

        Raylib.SetTargetFPS(60);
        Raylib.SetExitKey(0);
        // Main game loop
        while (!Raylib.WindowShouldClose() && s_isRunning)
        {
            Screen.HandleUserInput();
            Raylib.BeginDrawing();
            Screen.Render();
            Raylib.EndDrawing();
        }
        SaveScreenConfig(new ScreenConfig(Raylib.GetCurrentMonitor()));
        Raylib.CloseWindow();

    }

    public static void Exit() => s_isRunning = false;

    private static void SaveScreenConfig(ScreenConfig config)
    {
        try
        {
            string json = JsonSerializer.Serialize(config);
            File.WriteAllText(ConfigFile, json);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Failed to save {ConfigFile}. Stack Trace below:");
            Console.Error.WriteLine(e.StackTrace);
        }
    }

    private static ScreenConfig InitScreenConfig()
    {
        if (File.Exists(ConfigFile))
        {
            try
            {
                string json = File.ReadAllText(ConfigFile);
                return JsonSerializer.Deserialize<ScreenConfig>(json);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Failed to load {ConfigFile}. Stack trace below:");
                Console.Error.WriteLine(e.StackTrace);
            }
        }
        return GameConstants.Default;
    }

    /// <summary>
    /// Initializes the game window on to the specified monitor and centers
    /// it on the screen
    /// </summary>
    private static void InitWindow()
    {
        Raylib.SetWindowState(ConfigFlags.HiddenWindow);
        Raylib.InitWindow(GameConstants.MinScreenWidth, GameConstants.MinScreenHeight, "Dungeon Editor | Main Window");
        Raylib.SetWindowState(ConfigFlags.ResizableWindow);
        Raylib.SetWindowMinSize(GameConstants.MinScreenWidth, GameConstants.MinScreenHeight);
        CenterWindow();

        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        Raylib.ClearWindowState(ConfigFlags.HiddenWindow);
        Raylib.EndDrawing();
    }

    private static void CenterWindow()
    {
        (int mWidth, int mHeight) = (Raylib.GetMonitorWidth(Config.Monitor), Raylib.GetMonitorHeight(Config.Monitor));
        (int wWidth, int wHeight) = (Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

        Raylib.SetWindowPosition((mWidth - wWidth) / 2, (mHeight - wHeight) / 2);
        Raylib.SetWindowMonitor(Config.Monitor);
    }

}