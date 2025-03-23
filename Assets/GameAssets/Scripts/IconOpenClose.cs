using UnityEngine;
using UnityEngine.UI;

public class IconOpenClose : MonoBehaviour
{
    [SerializeField] private Sprite openIcon;
    [SerializeField] private Sprite closeIcon;

    private Image iconImage;

    [SerializeField] private bool currentIcon = true;

    private void Start()
    {
        iconImage = GetComponent<Image>();

        iconImage.sprite = currentIcon ? openIcon : closeIcon;
    }

    public void CurrentIcon(bool varsay�lan)
    {
        if(!currentIcon || !openIcon || !closeIcon)
        {
            return;
        }

        iconImage.sprite = varsay�lan ? openIcon : closeIcon;
    }

}
