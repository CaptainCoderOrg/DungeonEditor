namespace CaptainCoder.Dungeoneering.Raylib;

using System.Numerics;

using Raylib_cs;

public class DungeonEditorScreen : IScreen
{
    public const int CellSize = 16;
    public Cursor Cursor = new(new Position(0, 0), Facing.West);
    public void Render()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        Cursor.Render();
        Raylib.EndDrawing();
    }

    public void HandleUserInput()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.E))
        {
            Cursor = Cursor.Rotate();
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Q))
        {
            Cursor = Cursor.RotateCounterClockwise();
        }
        if (Raylib.IsKeyPressed(KeyboardKey.W))
        {
            Cursor = Cursor.Move(Facing.North);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.D))
        {
            Cursor = Cursor.Move(Facing.East);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.S))
        {
            Cursor = Cursor.Move(Facing.South);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.A))
        {
            Cursor = Cursor.Move(Facing.West);
        }
    }
}

public static class CursorExtensions
{
    public static void Render(this Cursor cursor)
    {
        float x = (float)(cursor.Position.X * DungeonEditorScreen.CellSize);
        float y = (float)(cursor.Position.Y * DungeonEditorScreen.CellSize);
        Vector2[] points = CursorPoints(cursor.Facing, x, y);
        Raylib.DrawTriangle(points[0], points[1], points[2], Color.Yellow);
    }

    public static Vector2[] CursorPoints(Facing direction, float x, float y)
    {
        const int padding = 5;
        return direction switch
        {
            Facing.North => [
                new Vector2(x + DungeonEditorScreen.CellSize * 0.5f, y + padding),
                new Vector2(x + padding, y + DungeonEditorScreen.CellSize - padding),
                new Vector2(x + DungeonEditorScreen.CellSize - padding, y + DungeonEditorScreen.CellSize - padding)
            ],
            Facing.South => [
                new Vector2(x + padding, y + padding),
                new Vector2(x + DungeonEditorScreen.CellSize * 0.5f, y + DungeonEditorScreen.CellSize - padding),
                new Vector2(x + DungeonEditorScreen.CellSize - padding, y + padding),
            ],
            Facing.East => [
                new Vector2(x + padding, y + padding),
                new Vector2(x + padding, y + DungeonEditorScreen.CellSize - padding),
                new Vector2(x + DungeonEditorScreen.CellSize - padding, y + DungeonEditorScreen.CellSize * 0.5f),
            ],
            Facing.West => [
                new Vector2(x + DungeonEditorScreen.CellSize - padding, y + padding),
                new Vector2(x + padding, y + DungeonEditorScreen.CellSize * 0.5f),
                new Vector2(x + DungeonEditorScreen.CellSize - padding, y + DungeonEditorScreen.CellSize - padding),
            ],
            _ => throw new Exception($"Unexpected facing: {direction}"),
        };
    }
}