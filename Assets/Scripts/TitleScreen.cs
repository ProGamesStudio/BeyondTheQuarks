using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class TitleScreen : MonoBehaviour
{
    public Button firstButton;
    public Button resumeButton;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;

        UpdateResumeButton();
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

    void UpdateResumeButton()
    {
        if (resumeButton == null)
        {
            return;
        }

        if (!SaveSystem.SaveExists())
        {
            resumeButton.interactable = false;
        }
    }
}
