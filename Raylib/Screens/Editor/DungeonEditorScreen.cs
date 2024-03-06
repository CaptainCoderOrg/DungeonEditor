namespace CaptainCoder.Dungeoneering.Raylib;

using CaptainCoder.Dungeoneering.Model.IO;

using Raylib_cs;

public class DungeonEditorScreen : IScreen
{
    public const int CellSize = 16;
    public Cursor Cursor { get; private set; } = new(new Position(0, 0), Facing.West);
    public WallMap WallMap { get; private set; } = new();
    public IScreen EditorMenu => new ModalMenuScreen(
        this,
        new MenuScreen("Menu",
        [
            new StaticEntry("Save", () => File.WriteAllText("map.json", WallMap.ToJson())),
            new StaticEntry("Load", () => LoadMap("map.json")),
            new StaticEntry("Return to Editor", () => Program.Screen = this),
            new StaticEntry("Exit Editor", Program.Exit),
        ]
    ));

    public void LoadMap(string path)
    {
        string json = File.ReadAllText(path);
        WallMap = JsonExtensions.LoadModel<WallMap>(json);
    }

    public void Render()
    {
        Raylib.ClearBackground(Color.Black);
        WallMap.Render();
        Cursor.Render();
    }

    public void HandleUserInput()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            Program.Screen = EditorMenu;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            if (WallMap.TryGetWall(Cursor.Position, Cursor.Facing, out _))
            {
                WallMap.RemoveWall(Cursor.Position, Cursor.Facing);
            }
            else
            {
                WallMap.SetWall(Cursor.Position, Cursor.Facing, WallType.Solid);
            }
        }
        HandleCursorMovement();
    }

    private void HandleCursorMovement()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.W))
        {
            Cursor = MoveOrRotate(Facing.North);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.D))
        {
            Cursor = MoveOrRotate(Facing.East);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.S))
        {
            Cursor = MoveOrRotate(Facing.South);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.A))
        {
            Cursor = MoveOrRotate(Facing.West);
        }
    }

    private Cursor MoveOrRotate(Facing facing)
    {
        if (Cursor.Facing == facing)
        {
            return Cursor.Move(facing);
        }
        else
        {
            return Cursor.Turn(facing);
        }
    }
}