using Godot;
using System.Collections.Generic;

public class Navigation : Navigation2D
{

    [Export]
    private float characterSpeed = 350;
    private List<Vector2> path;

    public override void _Ready()
    {
        path = new List<Vector2>();
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (!inputEvent.IsActionPressed("click"))
        {
            return;
        }
        Sprite character = GetNode("Character") as Sprite;
        UpdateNavigationPath(character.GetPosition(), GetLocalMousePosition());
    }

    public override void _Process(float delta)
    {
        float walkDistance = delta * characterSpeed;
        MoveAlongPath(walkDistance);
    }

    private void UpdateNavigationPath(Vector2 startPosition, Vector2 endPosition)
    {
        path.AddRange(GetSimplePath(startPosition, endPosition, true));
        SetProcess(true);
    }

    private void MoveAlongPath(float walkDistance)
    {
        Sprite character = GetNode("Character") as Sprite;
        Vector2 lastPosition = character.GetPosition();

        while (path.Count != 0)
        {
            float distanceBetweenPoints = lastPosition.DistanceTo(path[0]);

            if (walkDistance < distanceBetweenPoints)
            {
                character.SetPosition(lastPosition.LinearInterpolate(path[0], walkDistance / distanceBetweenPoints));
                return;
            }

            walkDistance -= distanceBetweenPoints;
            lastPosition = path[0];
            path.Remove(lastPosition);
        }

        character.SetPosition(lastPosition);
        SetProcess(false);
    }

}
