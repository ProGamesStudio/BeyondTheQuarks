using UnityEngine;

public class RoomSizeManager : MonoBehaviour
{
    [Tooltip("Half-size of the room")]
    public Vector2 size = new Vector2(8.9f, 5f);

    public Bounds GetBounds()
    {
        return new Bounds(transform.position, new Vector3(size.x * 2f, size.y * 2f, 0f));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x * 2f, size.y * 2f, 0f));
    }
}
