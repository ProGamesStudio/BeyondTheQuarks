using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class TitleScreen : MonoBehaviour
{
    public Button firstButton;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
    }

    void OnEnable()
    {
        StartCoroutine(SelectFirstButton());
    }

    IEnumerator SelectFirstButton()
    {
        yield return null;

        if (EventSystem.current == null || firstButton == null)
        {
            yield break;
        }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }
}
