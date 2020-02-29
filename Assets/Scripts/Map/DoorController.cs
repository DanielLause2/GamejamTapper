using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    private bool RoomConnected = false;
    public bool Open;
    public Direction Orientation;
    public GameObject NextRoom;

    

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
        if (Open && other.gameObject.tag == "Player")
        {
            if (!RoomConnected)
            {
                RoomConnected = true;
                GameObject room=null;
                
                switch (Orientation)
                {
                    case Direction.NORTH: 
                        room = Instantiate(NextRoom, new Vector3(this.transform.position.x, this.transform.position.y + (this.NextRoom.transform.localScale.y/2)), Quaternion.identity);
                        break;
                    case Direction.EAST:
                        room= Instantiate(NextRoom, new Vector3(this.transform.position.x+ (this.NextRoom.transform.localScale.x / 2), this.transform.position.y ), Quaternion.identity);
                        break;
                    case Direction.SOUTH:
                       room= Instantiate(NextRoom, new Vector3(this.transform.position.x, this.transform.position.y - (this.NextRoom.transform.localScale.y / 2)), Quaternion.identity);
                        break;
                    case Direction.WEST: 
                        room=Instantiate(NextRoom, new Vector3(this.transform.position.x - (this.NextRoom.transform.localScale.x / 2), this.transform.position.y), Quaternion.identity);
                        break;
                }
                RoomController roomController = room.GetComponent<RoomController>();
                roomController.LeaveRoom(Orientation);
            }
                 
             
        }
    }
    
}


  public enum Direction { 
        NORTH=0,
        EAST,
        SOUTH,
        WEST
    }
