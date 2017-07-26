using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomRepository
{
	public List<Room> _rooms;

	public RoomRepository(){
		_rooms = new List<Room> ();
	}

	public void Add(Room room){
		if(_rooms.Contains(room)){
			return;
		}
		_rooms.Add (room);
	}

	public List<Room> FindRooms(){
		return _rooms;
	}

	public Room Get(string name){
		return _rooms.SingleOrDefault (x => x.Name == name);
	}
	public void Delete(Room room){
        Debug.Log("deleying room");
		_rooms.Remove (room);
	}



}

