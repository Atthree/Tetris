using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private ShapeManager[] allShapes;

    
    public ShapeManager CreateShape()
    {
        int randomShape = Random.Range(0, allShapes.Length);
        ShapeManager shape = Instantiate(allShapes[randomShape], transform.position, Quaternion.identity) as ShapeManager;

        if (shape != null)
        {
            return shape;
        }
        else
        {
            return null;
        }
    }
}
