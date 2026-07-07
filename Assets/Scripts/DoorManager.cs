using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RoomTrigger : MonoBehaviour
{
    public RoomSizeManager nextRoom;
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
        if (!other.CompareTag(playerTag) || nextRoom == null)
        {
            return;
        }

        CameraRoomManager.Instance?.TransitionToRoom(nextRoom);
    }
}
