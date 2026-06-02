using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleButtonsEffect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Image backgroundImage;

    public Color normalColor = new Color(0xB4, 0xB4, 0xB4, 0.956f);
    public Color selectedColor = new Color(0xFF, 0x8E, 0x53, 1f);

    public Vector3 normalScale = Vector3.one;
    public Vector3 selectedScale = new Vector3(1.1f, 1.1f, 1f);

    void Start()
    {
        Set(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Set(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Set(false);
    }

    void Set(bool selected)
    {
        backgroundImage.color = selected ? selectedColor : normalColor;
        transform.localScale = selected ? selectedScale : normalScale;
    }
}
