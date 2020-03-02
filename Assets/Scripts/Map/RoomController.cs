using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<GameObject> nextRooms;
    public Direction entrance;
    public List<DoorController> doors;
    public List<Vector3> nextRoomsPositions; 
   
    public bool cleared;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (cleared)
        {
            RoomClear();
        }
        else 
        {
            RoomLoockDown();
        }
    }

    void RoomClear()
    {
        List<Vector3> connectedRooms = null;// this.transform.position
        nextRoomsPositions.ForEach(x => connectedRooms.Add(new Vector3(x.x + this.transform.position.x, x.y + this.transform.position.y)));
        doors.ForEach(x => { x.OpenDoor(nextRooms[0], connectedRooms); });
    }

    void RoomLoockDown()
    {
        if (doors == null)
        {
            
            doors.ForEach(x => x.CloseDoor());
        }
    }

    public void LeaveRoom(Direction pathOrientation)
    {
        entrance = (Direction)(((int)pathOrientation + 2)%4);
        
    }
}
