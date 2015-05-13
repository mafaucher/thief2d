using UnityEngine;
using System.Collections.Generic;

public class RoomGraphManager : MonoBehaviour
{
	private class Node
	{
		public Room room;
		public List<Edge> edges;
		public Node(Room room)
		{
			this.room=room;
			edges = new List<Edge>();
		}
	}
	private class Edge
	{
		public Room room_1, room_2;
		public Interactable door;
		public Door doorComponent;
		public Edge(Door d)
		{
			room_1 = d.room_1;
			room_2 = d.room_2;
			door = d.door;
			doorComponent = d;
		}
	}
	private List<Node> _nodes;
	private List<Edge> _edges;
	private Node GetNode(Room room)
	{
		Node result = null;
		foreach(Node node in _nodes)
		{
			if(node.room == room)
			{
				result = node;
				break;
			}
		}
		return result;
	}
	private void AddRoom(Room room)
	{
		Node node = new Node (room);
		_nodes.Add (node);
	}
	private void AddDoor(Door door)
	{
		Edge edge = new Edge (door);
		_edges.Add (edge);

		AddEdge (door.room_1, edge);
		AddEdge (door.room_2, edge);

	}
	private void AddEdge(Room room, Edge edge)
	{
		Node node = GetNode (room);
		if(node != null)
		{
			node.edges.Add(edge);
		}
		else
		{
			Debug.LogError("Room found in edges not added to nodes.");
		}
	}
	public void Awake()
	{
		_nodes = new List<Node> ();
		_edges = new List<Edge> ();
		Room[] rooms = Object.FindObjectsOfType<Room> ();
		Door[] doors = Object.FindObjectsOfType<Door> ();
		foreach(Room room in rooms)
		{
			AddRoom(room);
		}
		foreach(Door door in doors)
		{
			AddDoor(door);
		}
	}
	public Room[] GetAllRooms()
	{
		Room[] result = new Room[_nodes.Count];
		int i = 0;
		foreach(Node node in _nodes)
		{
			result[i] = node.room;
			i++;
		}
		return result;
	}
	public Door[] GetAllDoors()
	{
		Door[] result = new Door[_edges.Count];
		int i = 0;
		foreach(Edge edge in _edges)
		{
			result[i] = edge.doorComponent;
			i++;
		}
		return result;
	}
}