using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class ScreenCollider : CollisionPolygon2D
{
	private Vector2 _size = new Vector2(10f, 20f);
    private float _width = 2f;

    [Export]
    public Vector2 Size
    {
        get => _size;
        set
        {
            _size = value;
            UpdatePolygons();
        }
    }

    [Export]
    public float Width
    {
        get => _width;
        set
        {
            _width = value;
            UpdatePolygons();
        }
    }

    public override void _Ready()
    {
        BuildMode = BuildModeEnum.Solids;
        UpdatePolygons();
    }

    private void UpdatePolygons()
    {
        var points = GetRectanglePoints(_size);
        Polygon = points.ToArray();
    }

    private List<Vector2> GetRectanglePoints(Vector2 size)
    {
        float halfWidth = _width / 2f;

        var innerRect = GetRectangleCorners(size - new Vector2(halfWidth, halfWidth));
        var outerRect = GetRectangleCorners(size + new Vector2(halfWidth, halfWidth));

        innerRect.Reverse(); // inverti per creare il buco

        var points = new List<Vector2>();
        points.AddRange(outerRect);
        points.Add(outerRect[0]); // chiudi il loop esterno
        points.AddRange(innerRect);
        points.Add(innerRect[0]); // chiudi il loop interno

        return points;
    }

    private List<Vector2> GetRectangleCorners(Vector2 size)
    {
        Vector2 halfSize = size / 2f;

        return new List<Vector2>
        {
            new Vector2(-halfSize.X, -halfSize.Y),
            new Vector2( halfSize.X, -halfSize.Y),
            new Vector2( halfSize.X,  halfSize.Y),
            new Vector2(-halfSize.X,  halfSize.Y)
        };
    }
}
