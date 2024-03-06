namespace CaptainCoder.Dungeoneering.Raylib;

using CaptainCoder.Dungeoneering.Model.IO;

using Raylib_cs;

public class DungeonEditorScreen : IScreen
{
    public const int CellSize = 16;
    public Cursor Cursor { get; private set; } = new(new Position(0, 0), Facing.West);
    public WallMap WallMap { get; private set; } = new();
    private string? _filename = null;
    public IScreen EditorMenu => new ModalMenuScreen(
        this,
        new MenuScreen("Menu",
        [
            new DynamicEntry(
                () => "Save " + (_filename ?? string.Empty),
                _filename is null ? SaveAs : Save
            ),
            new StaticEntry("Save As", SaveAs),
            new StaticEntry("Load", LoadMap),
            new StaticEntry("Return to Editor", () => Program.Screen = this),
            new StaticEntry("Exit Editor", Program.Exit),
        ]
    ));

    public void Save()
    {
        if (_filename is null)
        {
            SaveAs();
            return;
        }
        File.WriteAllText(_filename, WallMap.ToJson());
    }

    public void SaveAs()
    {
        Program.Screen = new PromptScreen("Save As", this, OnFinished);
        void OnFinished(string filename)
        {
            _filename = $"{filename}.json";
            Save();
        }
    }

    public void LoadMap()
    {
        Program.Screen = new PromptScreen("Load File", this, OnFinish);
        void OnFinish(string filename)
        {
            filename = $"{filename}.json";
            if (!File.Exists(filename))
            {
                Console.Error.WriteLine($"File not found: {filename}");
                return;
            }
            _filename = filename;
            string json = File.ReadAllText(filename);
            WallMap = JsonExtensions.LoadModel<WallMap>(json);
        }
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

    private bool IsShiftDown => Raylib.IsKeyDown(KeyboardKey.LeftShift) || Raylib.IsKeyDown(KeyboardKey.RightShift);

    private void HandleCursorMovement()
    {
        if (IsShiftDown && Raylib.IsKeyPressed(KeyboardKey.W))
        {
            Cursor = Cursor.Move(Facing.North);
        }
        else if (IsShiftDown && Raylib.IsKeyPressed(KeyboardKey.S))
        {
            Cursor = Cursor.Move(Facing.South);
        }
        else if (IsShiftDown && Raylib.IsKeyPressed(KeyboardKey.D))
        {
            Cursor = Cursor.Move(Facing.East);
        }
        else if (IsShiftDown && Raylib.IsKeyPressed(KeyboardKey.A))
        {
            Cursor = Cursor.Move(Facing.West);
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.W))
        {
            Cursor = MoveOrRotate(Facing.North);
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.D))
        {
            Cursor = MoveOrRotate(Facing.East);
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.S))
        {
            Cursor = MoveOrRotate(Facing.South);
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.A))
        {
            Cursor = MoveOrRotate(Facing.West);
        }
    }

    private Cursor MoveOrRotate(Facing facing)
    {
        if (Cursor.Facing == facing)
        {
            return Cursor.MoveAndRotate(facing);
        }
        else
        {
            return Cursor.Turn(facing);
        }
    }
}