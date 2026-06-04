using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
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
            OnConfirm();
        }
    }

    public void Show(System.Action callback)
    {
        onConfirmed = callback;
        namePanel.SetActive(true);
        inputField.text = "";
        inputField.ActivateInputField();
        confirmButton.interactable = false;
        inputField.onValueChanged.AddListener(OnInputChanged);
    }

    void OnInputChanged(string value)
    {
        confirmButton.interactable = value.Trim().Length > 0;
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

        namePanel.SetActive(false);
        inputField.onValueChanged.RemoveListener(OnInputChanged);
        onConfirmed?.Invoke();
    }
}
