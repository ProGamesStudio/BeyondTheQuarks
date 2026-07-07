using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBoxManager : MonoBehaviour
{
    [Tooltip("The aspect ratio of the rooms")]
    public float targetAspect = 16f / 9f;

    private Camera cam;
    private int lastScreenWidth;
    private int lastScreenHeight;

    void Awake()
    {
        cam = GetComponent<Camera>();
        ApplyBoxManager();
    }

    void Update()
    {
        // Fix on resize
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            ApplyBoxManager();
        }
    }

    void ApplyBoxManager()
    {
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        float windowAspect = (float)Screen.width/Screen.height;
        float scaleHeight = windowAspect/targetAspect;

        Rect rect = cam.rect;

        if (scaleHeight < 1f)
        {
            // Screen is taller (bars top/bottom)
            rect.width = 1f;
            rect.height = scaleHeight;
            rect.x = 0f;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            // Screen is wider (bars left/right)
            float scaleWidth = 1f / scaleHeight;

            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) / 2f;
            rect.y = 0f;
        }

        cam.rect = rect;
    }

    public Vector2 GetReferenceViewSize()
    {
        float height = cam.orthographicSize * 2f;
        float width = height * targetAspect;
        
        return new Vector2(width, height);
    }
}
