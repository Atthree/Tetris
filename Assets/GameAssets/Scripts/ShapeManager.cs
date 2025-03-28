using System.Collections;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    [SerializeField] private bool canRotate = true;

    public Sprite shape;
    public void MoveLeft()
    {
        transform.Translate(Vector3.left,Space.World);
    }
    
    public void MoveRight()
    {
        transform.Translate(Vector3.right,Space.World);
    }
    
    public void MoveDown()
    {
        transform.Translate(Vector3.down,Space.World);
    }
    
    public void MoveUp()
    {
        transform.Translate(Vector3.up,Space.World);
    }

    public void TurnLeft()
    {
        if(canRotate)
        {
            transform.Rotate(0,0,-90);
        }
    }
    
    public void TurnRight()
    {
        if(canRotate)
        {
            transform.Rotate(0,0,90);
        }
    }

    public void CanTurnRight(bool rightDir)
    {
        if (rightDir)
        {
            TurnRight();
        }
        else
        {
            TurnLeft();
        }
    }
}
