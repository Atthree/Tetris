using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private ShapeManager[] allShapes;

    [SerializeField] private Image[] shapeImg = new Image[2];

    private ShapeManager[] nextShapes = new ShapeManager[2];


    private void Awake()
    {
        ResetAllReferences();
    }
    public ShapeManager CreateShape()
    {
        ShapeManager shape = null;

        shape = GetNextShape();
        shape.gameObject.SetActive(true);
        shape.transform.position = transform.position;

        if (shape != null)
        {
            return shape;
        }
        else
        {
            return null;
        }
    }
    ShapeManager CreateRandomShape()
    {
        int randomShape = Random.Range(0,allShapes.Length);

        if (allShapes[randomShape])
        {
            return allShapes[randomShape];
        }
        else
        {
            return null;
        }
    }
    void ResetAllReferences()
    {
        for (int i = 0; i < nextShapes.Length; i++)
        {
            nextShapes[i] = null;
        }
        FillRow();
    }

    void FillRow()
    {
        for (int i = 0; i < nextShapes.Length; i++)
        {
            if (!nextShapes[i])
            {
                nextShapes[i] = Instantiate(CreateRandomShape(), transform.position, Quaternion.identity) as ShapeManager;
                nextShapes[i].gameObject.SetActive(false);
                shapeImg[i].sprite = nextShapes[i].shape;
            }
        }
        StartCoroutine(ShapeImgRoutine());
    }

    IEnumerator ShapeImgRoutine()
    {
        for (int i = 0; i < shapeImg.Length; i++)
        {
            shapeImg[i].GetComponent<CanvasGroup>().alpha = 0;
            shapeImg[i].GetComponent<RectTransform>().localScale = Vector3.zero;
        }
        yield return new WaitForSeconds(.1f);

        int counter = 0;
        while (counter < shapeImg.Length)
        {
            shapeImg[counter].GetComponent<CanvasGroup>().DOFade(1, .6f);
            shapeImg[counter].GetComponent<RectTransform>().DOScale(1, .6f).SetEase(Ease.OutBack);

            counter++;
            yield return new WaitForSeconds(.4f);
        }
        
    }

    ShapeManager GetNextShape()
    {
        ShapeManager nextShape = null;

        if (nextShapes[0])
        {
            nextShape = nextShapes[0];
        }

        for (int i = 1; i < nextShapes.Length; i++)
        {
            nextShapes[i - 1] = nextShapes[i];
            shapeImg[i - 1].sprite = shapeImg[i].sprite;
        }

        nextShapes[nextShapes.Length - 1] = null;

        FillRow();
        return nextShape;
    }

    public ShapeManager CreateOtherShape()
    {
        ShapeManager eldekiSekil = null;
        eldekiSekil = Instantiate(CreateRandomShape(), transform.position, Quaternion.identity) as ShapeManager;
        eldekiSekil.transform.position = transform.position;
        return eldekiSekil;
    }
}
