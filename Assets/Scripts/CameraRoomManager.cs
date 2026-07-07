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

        Vector3 target = GetTargetPosition(currentRoom.GetBounds());
        transform.position = Vector3.SmoothDamp(transform.position, target, ref followVelocity, followSmoothTime);
    }

    public void SetInitialRoom(RoomSizeManager room)
    {
        currentRoom = room;

        transform.position = GetTargetPosition(room.GetBounds());
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
        Vector3 endPos = GetTargetPosition(targetBounds);

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

    Vector3 GetTargetPosition(Bounds roomBounds)
    {
        float viewHeight = cam.orthographicSize * 2f;
        float viewWidth = viewHeight * cam.aspect;
 
        bool roomFitsInView = (roomBounds.size.x<=viewWidth)&&(roomBounds.size.y<=viewHeight);
 
        Vector3 target = roomFitsInView?roomBounds.center:(player != null?player.position:roomBounds.center);
 
        return new Vector3(target.x, target.y, transform.position.z);
    }
}
