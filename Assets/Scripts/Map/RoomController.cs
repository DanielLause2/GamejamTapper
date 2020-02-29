using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<GameObject> nextRooms;
    public Direction entrance;
    public List<DoorController> doors;
   
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
        doors.ForEach(x => { x.Open = true; x.NextRoom = nextRooms[0]; });
    }

    void RoomLoockDown()
    {
        if (doors == null)
        {
            
            doors.ForEach(x => x.Open = false);
            doors[(int)entrance].Open = false;
        }
    }

    public void LeaveRoom(Direction pathOrientation)
    {
        entrance = (Direction)(((int)pathOrientation + 2)%4);
        
    }
}
