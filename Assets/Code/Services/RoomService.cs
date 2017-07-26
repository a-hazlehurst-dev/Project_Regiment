using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomService
{

	private RoomRepository _roomRepository;
	int roomIndex = 1;


	public void Init(){
		_roomRepository = new RoomRepository();
		_roomRepository.Add (new Room ( "outside")); // add the 'outside room'
	}

	public int GetRoomID(Room r){
		return _roomRepository.FindRooms ().IndexOf (r);
	}

	public Room GetRoomFromID(int i ){
		if (i < 0 || i > _roomRepository.FindRooms ().Count - 1) {
			return null;
		}
		return _roomRepository.FindRooms () [i];
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
		room.ResetRoomTilesToOutside ();
        _roomRepository.Delete(room);
	}

    public void AddRoom(Room rm)
    {
        _roomRepository.Add(rm);
    }
}

