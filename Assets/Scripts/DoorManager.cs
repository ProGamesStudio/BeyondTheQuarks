using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DoorManager : MonoBehaviour
{
    public RoomSizeManager thisRoom;
    public RoomSizeManager nextRoom;

    public BoxCollider2D thisDoorCollider;
    public BoxCollider2D nextDoorCollider;

    public string playerTag = "Player";

    void Reset()
    {
        Collider2D col = GetComponent<Collider2D>();

        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        CalcDirection(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        CalcDirection(other);
    }

    void CalcDirection(Collider2D other)
    {
        if (!other.CompareTag(playerTag) || thisDoorCollider == null || nextDoorCollider == null)
        {
            return;
        }

        Vector2 centerOfThisDoor = thisDoorCollider.bounds.center;
        Vector2 centerOfNextDoor = nextDoorCollider.bounds.center;
        Vector2 midpoint = (centerOfThisDoor + centerOfNextDoor) * 0.5f;
        Vector2 doorwayDirection = centerOfNextDoor - centerOfThisDoor;

        float side = Vector2.Dot((Vector2)other.transform.position - midpoint, doorwayDirection.normalized);
        RoomSizeManager target = side >= 0f ? nextRoom : thisRoom;

        CameraRoomManager.Instance?.TransitionToRoom(target);
    }
}
