using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class UsableTile : EmptyTile
{
    public int Owner;
    public enum Effect { Stop, Kill, Reflect, Passthrough}
    public enum Direction { Up, Right, Down, Left }

    [SerializeField] private Direction rotation;
    [SerializeField] private Effect?[,] effectGrid;
    [SerializeField] private Direction baseDirection;

    public void Init(bool isOffset)
    {
        effectGrid = new Effect?[3,3];
        baseDirection = Direction.Up;
        rotation = baseDirection;
    }


    /*
     * For Passthrough
     * O O X
     * O X O    = Forward, Right
     * X O O
     * 
     * X O X
     * O X O    = Left, Forward, Right
     * X O X 
     * 
     * O X O
     * X O X    = Left, Right -> Changed to Reflect
     * O X O
     */

    public List<Direction> GetLaserEffect(Direction comeFrom)
    {
        int offset = (int)rotation;
        Direction calcDir = RotateDirBy(comeFrom, 4 - offset);
        List<Direction> outputDirections;
        

        switch (calcDir) 
        { 
            case Direction.Up:
                {
                    outputDirections = EffectWithPointsAs(new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(1, 2));
                }
                break;
            case Direction.Left:
                {
                    outputDirections = EffectWithPointsAs(new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(1, 2));
                    offset += 1;
                }
                break;
        }

        //For all items in list rotate by offset
    }

    private List<Direction> EffectWithPointsAs(Point leftBottom, Point middleBottom, Point rightBottom, Point leftMiddle, Point middle, Point rightMiddle, Point middleTop)
    {
        List<Direction> outputDirections = new List<Direction>();

        // Straightforward
        if (effectGrid[middleBottom.X, middleBottom.Y] == null && effectGrid[middle.X, middle.Y] == null && effectGrid[middleTop.X, middleTop.Y] == null)
        {
            outputDirections.Add(Direction.Up);
        }

        // Stop
        else if (effectGrid[middleBottom.X, middleBottom.Y] == Effect.Stop)
        {
            // Return Empty
        }

        // Kill
        else if (effectGrid[middleBottom.X, middleBottom.Y] == Effect.Kill)
        {
            Die();
            // Return Empty
        }

        // Reflect
        else if (effectGrid[middleBottom.X, middleBottom.Y] == Effect.Reflect)
        {
            if (effectGrid[leftMiddle.X, leftMiddle.Y] == Effect.Reflect)
                outputDirections.Add(Direction.Left);
            if (effectGrid[rightMiddle.X, rightMiddle.Y] == Effect.Reflect)
                outputDirections.Add(Direction.Right);
        }

        else if (effectGrid[middle.X, middle.Y] == Effect.Reflect)
        {
            if (effectGrid[leftBottom.X, leftBottom.Y] == Effect.Reflect)
                outputDirections.Add(Direction.Right);
            else if (effectGrid[rightBottom.X, rightBottom.Y] == Effect.Reflect)
                outputDirections.Add(Direction.Left);
            else
                throw new System.Exception("The effects matrix is incorrect for Reflect");
        }

        // Passthrough
        else if (effectGrid[middle.X, middle.Y] == Effect.Passthrough)
        {
            if (effectGrid[leftBottom.X, leftBottom.Y] == Effect.Passthrough)
            {
                outputDirections.Add(Direction.Right);
            }
            if (effectGrid[rightBottom.X, rightBottom.Y] == Effect.Passthrough)
            {
                outputDirections.Add(Direction.Left);
            }
            if (outputDirections.Count > 0)
            {
                outputDirections.Add(Direction.Up);
            }
            else
            {
                throw new System.Exception("Effects matrix is incorrect for Passthrough");
            }
        }

        return outputDirections;
    }


    private Direction RotateDirBy (Direction comeFrom, int offset)
    {
        return (Direction)(int) comeFrom + offset % 4;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
