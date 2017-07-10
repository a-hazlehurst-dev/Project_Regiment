using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomService
{

	private RoomRepository _roomRepository;
	int roomIndex = 1;


	public void Init(){
		_roomRepository = new RoomRepository();
		_roomRepository.Add (new Room (roomIndex, "outside")); // add the 'outside room'
	}

	public void Create(string name = "Room"){
		roomIndex++;
		if (name == "Room") {
			name += roomIndex.ToString ();
		}
		var room = new Room (roomIndex, name);
	}

	public List<Room> FindRooms(){
		return _roomRepository.FindRooms ();
	}

	public Room Get(string name){
		var room = _roomRepository.Get (name);
		if (room == null) {
			Debug.LogError ("Attempting to get room that does not exist. cant find " + name);
		}
		return room;
	}


	public void Delete(Room room){
        _roomRepository.Delete(room);
        room.ResetRoomTilesToOutside ();
		
	}

    public void AddRoom(Room rm)
    {
        _roomRepository.Add(rm);
    }
}

