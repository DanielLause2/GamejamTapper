using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Direction orientation;
    private bool roomConnected = false;
    private bool open;
    private GameObject connectedRoom;
    private List<Vector3> connectedRoomPos;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (open && other.gameObject.tag == "Player")
        {
            if (!roomConnected)
            {
                roomConnected = true;
                GameObject room = Instantiate(connectedRoom, connectedRoomPos[(int)orientation], Quaternion.identity);
                RoomController roomController = room.GetComponent<RoomController>();
                roomController.LeaveRoom(orientation);
            }
        }
    }

    public void OpenDoor(GameObject v_NextRoom, List<Vector3> v_nextRoomsPositions)
    {
        open = true;
        connectedRoom = v_NextRoom;
        connectedRoomPos = v_nextRoomsPositions;
    }

    public void CloseDoor()
    {
        open = false;
        connectedRoom = null;
        connectedRoomPos = null;
    }
}

  public enum Direction
{
    NORTH = 0,
    EAST,
    SOUTH,
    WEST
}
