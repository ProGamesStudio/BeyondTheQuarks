using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class TitleScreen : MonoBehaviour
{
    public Button firstButton;
    public Button resumeButton;
    public GameObject confirmPanel;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        confirmPanel.SetActive(false);

        UpdateResumeButton();
    }

    void OnEnable()
    {
        StartCoroutine(SelectFirstButton());
    }

    void Update()
    {
        if (confirmPanel != null && confirmPanel.activeSelf)
        {
            return; // Stop processing input if the confirm panel is active
        }
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
