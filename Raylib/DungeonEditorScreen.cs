namespace CaptainCoder.Dungeoneering.Raylib;

using Raylib_cs;

public class DungeonEditorScreen : IScreen
{
    public const int CellSize = 16;
    public Cursor Cursor = new(new Position(0, 0), Facing.West);
    public WallMap WallMap = new();

    public void Render()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        WallMap.Render();
        Cursor.Render();
        Raylib.EndDrawing();
    }

    public void HandleUserInput()
    {
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