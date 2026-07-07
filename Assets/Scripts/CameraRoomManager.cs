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
    public RoomSizeManager initialRoom;

    private Camera cam;
    private RoomSizeManager currentRoom;
    private RoomSizeManager targetRoom;
    private Coroutine transitionCoroutine;
    private Vector3 followVelocity;
    private bool isTransitioning;

    void Awake()
    {
        Instance = this;
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        if (initialRoom != null)
        {
            SetInitialRoom(initialRoom);
        }
    }

    void LateUpdate()
    {
        if (isTransitioning || currentRoom == null || player == null)
        {
            return;
        }

        Vector3 target = GetTargetPosition(currentRoom);
        transform.position = Vector3.SmoothDamp(transform.position, target, ref followVelocity, followSmoothTime);
    }

    public void SetInitialRoom(RoomSizeManager room)
    {
        currentRoom = room;
        targetRoom = room;
        followVelocity = Vector3.zero;
        transform.position = GetTargetPosition(room);
    }

    public void TransitionToRoom(RoomSizeManager newRoom)
    {
        if (newRoom == targetRoom)
        {
            return;
        }

        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }

        transitionCoroutine = StartCoroutine(TransitionRoutine(newRoom));
    }

    IEnumerator TransitionRoutine(RoomSizeManager newRoom)
    {
        isTransitioning = true;
        targetRoom = newRoom;
        followVelocity = Vector3.zero;

        Vector3 startPos = transform.position;
        Vector3 endPos = GetTargetPosition(newRoom);

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
        followVelocity = Vector3.zero;
        isTransitioning = false;
        transitionCoroutine = null;
    }

    Vector3 GetTargetPosition(RoomSizeManager room)
    {
        Vector3 target = room.cameraFollowsPlayer && player != null ? player.position : room.GetBounds().center;
 
        return new Vector3(target.x, target.y, transform.position.z);
    }
}
