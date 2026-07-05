using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraRoomManager : MonoBehaviour
{
    public static CameraRoomManager Instance { get; private set; }

    [Header("References")]
    public Transform player;

    [Header("Follow")]
    public float followSmoothTime = 0.15f;

    [Header("Room Transition")]
    public float roomTransitionDuration = 0.6f;
    public AnimationCurve transitionEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Camera cam;
    private RoomSizeManager currentRoom;
    private Vector3 followVelocity;
    private bool isTransitioning;

    void Awake()
    {
        Instance = this;
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (isTransitioning || currentRoom == null || player == null)
        {
            return;
        }

        Vector3 target = ClampToRoom(player.position, currentRoom.GetBounds());
        transform.position = Vector3.SmoothDamp(transform.position, target, ref followVelocity, followSmoothTime);
    }

    public void SetInitialRoom(RoomSizeManager room)
    {
        currentRoom = room;

        if (player != null)
        {
            transform.position = ClampToRoom(player.position, room.GetBounds());
        }
    }

    public void TransitionToRoom(RoomSizeManager newRoom)
    {
        if (newRoom == currentRoom || isTransitioning)
        {
            return;
        }

        StartCoroutine(TransitionRoutine(newRoom));
    }

    IEnumerator TransitionRoutine(RoomSizeManager newRoom)
    {
        isTransitioning = true;

        Vector3 startPos = transform.position;
        Bounds targetBounds = newRoom.GetBounds();
        Vector3 endPos = ClampToRoom(player != null ? player.position : targetBounds.center, targetBounds);

        float elapsed = 0f;

        while (elapsed < roomTransitionDuration)
        {
            elapsed += Time.deltaTime;

            float t = transitionEase.Evaluate(Mathf.Clamp01(elapsed / roomTransitionDuration));
            transform.position = Vector3.Lerp(startPos, endPos, t);
            
            yield return null;
        }

        transform.position = endPos;
        currentRoom = newRoom;
        isTransitioning = false;
    }

    Vector3 ClampToRoom(Vector3 targetPos, Bounds roomBounds)
    {
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight*cam.aspect;

        float minX = roomBounds.min.x+halfWidth;
        float maxX = roomBounds.max.x-halfWidth;
        float minY = roomBounds.min.y+halfHeight;
        float maxY = roomBounds.max.y-halfHeight;

        float x = minX <= maxX?Mathf.Clamp(targetPos.x, minX, maxX):roomBounds.center.x;
        float y = minY <= maxY?Mathf.Clamp(targetPos.y, minY, maxY):roomBounds.center.y;

        return new Vector3(x, y, transform.position.z);
    }
}
