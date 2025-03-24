using UnityEngine;

public class FollowShapeManager : MonoBehaviour
{
    private ShapeManager followShape = null;
    private bool isTouchGround;
    public Color color = new Color(1f, 1f, 1f, .2f);
    
    public void CreateFollowShape(ShapeManager realShape,BoardManager board)
    {
        if (!followShape)
        {
            followShape = Instantiate(realShape, realShape.transform.position, transform.rotation) as ShapeManager;

            followShape.name = "TakipShape";

            SpriteRenderer[] allSprites = followShape.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer sr in allSprites)
            {
                sr.color = color;
            }
        }

        else
        {
            followShape.transform.position = realShape.transform.position;
            followShape.transform.rotation = realShape.transform.rotation;
        }
        isTouchGround = false;

        while (!isTouchGround)
        {
            followShape.MoveDown();
            if(!board.CurrentPosition(followShape))
            {
                followShape.MoveUp();
                isTouchGround=true;
            }
        }
    }
    public void ResetFc()
    {
        Destroy(followShape.gameObject);
    }
}
