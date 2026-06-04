using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class NameInput : MonoBehaviour
{
    public GameObject namePanel;
    public TMP_InputField inputField;
    public Button confirmButton;

    private System.Action onConfirmed;

    void Start()
    {
        confirmButton.onClick.AddListener(OnConfirm);
        namePanel.SetActive(false);
    }

    void Update()
    {
        if (!namePanel.activeSelf)
        {
            return;
        }

        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame && confirmButton.interactable)
        {
            if (confirmButton.interactable)
            {
                OnConfirm();
            }
            else
            {
                FocusInput();
            }
        }

        bool tabPressed = Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame;
        bool downPressed = Keyboard.current != null && Keyboard.current.downArrowKey.wasPressedThisFrame;
        bool upPressed = Keyboard.current != null && Keyboard.current.upArrowKey.wasPressedThisFrame;
        bool gamepadDown = Gamepad.current != null && Gamepad.current.dpad.down.wasPressedThisFrame;
        bool gamepadUp = Gamepad.current != null && Gamepad.current.dpad.up.wasPressedThisFrame;

        if (tabPressed || downPressed || gamepadDown)
        {
            if (inputField.isFocused && confirmButton.interactable)
            {
                FocusButton();
            }
            else
            {
                FocusInput();
            }
        }

        if (upPressed || gamepadUp)
        {
            if (!inputField.isFocused)
            {
                FocusInput();
            }
        }
    }

    public void Show(System.Action callback)
    {
        onConfirmed = callback;
        namePanel.SetActive(true);
        inputField.text = "";
        confirmButton.interactable = false;
        inputField.onValueChanged.AddListener(OnInputChanged);
        StartCoroutine(FocusInputNextFrame());
    }

    void OnInputChanged(string value)
    {
        confirmButton.interactable = value.Trim().Length > 0;

        if (value.Trim().Length == 0)
        {
            FocusInput();
        }
    }

    void OnConfirm()
    {
        string playerName = inputField.text.Trim();
        
        if (playerName.Length == 0)
        {
            return;
        }

        SaveData data = new SaveData();
        data.playerName = playerName;
        SaveSystem.SaveGame(data);

        // namePanel.SetActive(false);
        inputField.onValueChanged.RemoveListener(OnInputChanged);
        onConfirmed?.Invoke();
    }

    void FocusInput()
    {
        StartCoroutine(FocusInputNextFrame());
    }

    IEnumerator FocusInputNextFrame()
    {
        yield return null;

        inputField.ActivateInputField();
        inputField.Select();

        Cursor.visible = false;
    }

    void FocusButton()
    {
        inputField.DeactivateInputField();
        confirmButton.Select();

        Cursor.visible = false;
    }
}
