namespace CaptainCoder.Dungeoneering.Raylib;

using CaptainCoder.Dungeoneering.Model.IO;

using Raylib_cs;

public class DungeonEditorScreen : IScreen
{
    public const int CellSize = 16;
    public Cursor Cursor { get; private set; } = new(new Position(0, 0), Facing.West);
    public WallMap WallMap { get; private set; } = new();
    private string? _filename = null;
    private readonly InfoOverLayScreen _overlay = new();
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
        _overlay.AddMessage($"File saved: {_filename}!", Color.Green);
        Program.Screen = this;
    }

    public void SaveAs()
    {
        Program.Screen = new PromptScreen("Save As", this, OnFinished);
        void OnFinished(string filename)
        {
            _filename = Path.Combine(".save-data", $"{filename}.json");
            Save();
        }
    }

    public void LoadMap()
    {
        string[] filenames = Directory.GetFiles(Path.Combine(".save-data"));
        Program.Screen = new ModalMenuScreen(
            this,
            new MenuScreen(
                "Load Map",
                filenames.Select((file, ix) => new StaticEntry(file, () => LoadMap(file)))
            )
        );

        void LoadMap(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.Error.WriteLine($"File not found: {filename}");
                _overlay.AddMessage($"File not found: {filename}", Color.Red);
                return;
            }
            _overlay.AddMessage($"File loaded!", Color.Green);

            _filename = filename;
            string json = File.ReadAllText(filename);
            WallMap = JsonExtensions.LoadModel<WallMap>(json);
            Program.Screen = this;
        }
    }

    public void Render()
    {
        Raylib.ClearBackground(Color.Black);
        WallMap.Render();
        Cursor.Render();
        _overlay.Render();
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
    private bool IsWestKeyPressed => Raylib.IsKeyPressed(KeyboardKey.A) || Raylib.IsKeyPressed(KeyboardKey.Left);
    private bool IsNorthKeyPressed => Raylib.IsKeyPressed(KeyboardKey.W) || Raylib.IsKeyPressed(KeyboardKey.Up);
    private bool IsSouthKeyPressed => Raylib.IsKeyPressed(KeyboardKey.S) || Raylib.IsKeyPressed(KeyboardKey.Down);
    private bool IsEastKeyPressed => Raylib.IsKeyPressed(KeyboardKey.D) || Raylib.IsKeyPressed(KeyboardKey.Right);

    private void HandleCursorMovement()
    {
        if (IsShiftDown && IsNorthKeyPressed)
        {
            Cursor = Cursor.Move(Facing.North);
        }
        else if (IsShiftDown && IsSouthKeyPressed)
        {
            Cursor = Cursor.Move(Facing.South);
        }
        else if (IsShiftDown && IsEastKeyPressed)
        {
            Cursor = Cursor.Move(Facing.East);
        }
        else if (IsShiftDown && IsWestKeyPressed)
        {
            Cursor = Cursor.Move(Facing.West);
        }
        else if (IsNorthKeyPressed)
        {
            Cursor = MoveOrRotate(Facing.North);
        }
        else if (IsEastKeyPressed)
        {
            Cursor = MoveOrRotate(Facing.East);
        }
        else if (IsSouthKeyPressed)
        {
            Cursor = MoveOrRotate(Facing.South);
        }
        else if (IsWestKeyPressed)
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