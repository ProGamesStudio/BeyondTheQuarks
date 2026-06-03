using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

[System.Serializable]
public class StorySlide
{
    public Sprite image;

    [TextArea(3, 6)]
    public string text;
}

public class StoryScene : MonoBehaviour
{
    [Header("UI")]
    public Image storyImage;
    public TextMeshProUGUI storyText;
    public Image fadePanel;

    [Header("Slides")]
    public StorySlide[] slides;

    [Header("Settings")]
    public float typingSpeed = 0.04f;
    public float fadeDuration = 0.5f;
    public string nextScene = "TutorialScene";

    private int currentIndex = 0;
    private bool isTyping = false;
    private bool isTransitioning = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        StartCoroutine(StartSequence());
    }

    void Update()
    {
        if (isTransitioning)
        {
            return;
        }

        bool pressed = Keyboard.current != null && (
            Keyboard.current.spaceKey.wasPressedThisFrame ||
            Keyboard.current.enterKey.wasPressedThisFrame ||
            Keyboard.current.numpadEnterKey.wasPressedThisFrame
        ) || Gamepad.current != null && (
            Gamepad.current.buttonSouth.wasPressedThisFrame ||
            Gamepad.current.startButton.wasPressedThisFrame
        );

        if (!pressed)
        {
            return;
        }

        if (isTyping)
        {
            StopCoroutine(typingCoroutine);

            storyText.text = slides[currentIndex].text;
            isTyping = false;
        }
        else
        {
            currentIndex++;

            if (currentIndex >= slides.Length)
            {
                StartCoroutine(EndStory());
            }
            else
            {
                StartCoroutine(ShowSlide(currentIndex, false));
            }
        }
    }

    IEnumerator StartSequence()
    {
        storyImage.sprite = slides[0].image;
        storyText.text = "";

        yield return StartCoroutine(SceneTransition.Instance.FadeIn());

        isTransitioning = false;

        typingCoroutine = StartCoroutine(TypeText(slides[0].text));
    }

    IEnumerator ShowSlide(int index, bool first)
    {
        isTransitioning = true;

        if (!first)
        {
            yield return StartCoroutine(Fade(0f, 1f));
        }

        storyImage.sprite = slides[index].image;
        storyText.text = "";

        yield return StartCoroutine(Fade(1f, 0f));

        isTransitioning = false;

        typingCoroutine = StartCoroutine(TypeText(slides[index].text));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        storyText.text = "";

        foreach (char c in text)
        {
            storyText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        Color c = fadePanel.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, elapsed / fadeDuration);
            fadePanel.color = c;
            yield return null;
        }

        c.a = to;
        fadePanel.color = c;
    }

    IEnumerator EndStory()
    {
        isTransitioning = true;
        yield return StartCoroutine(Fade(0f, 1f));

        SceneTransition.Instance.LoadScene(nextScene);
    }
}
