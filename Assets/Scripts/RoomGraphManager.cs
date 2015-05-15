using UnityEngine;
using System.Collections.Generic;
using System;
public class RoomGraphManager : MonoBehaviour
{
	//private float _infiniteCost = 1000.0f;
	private class Node
	{
		public Room room;
		public List<Edge> edges;
		//public float tentativeDistance;
		public Node(Room room)
		{
			this.room=room;
			edges = new List<Edge>();
		}
	}
	private class Edge
	{
		public Node node_1, node_2;
		public Interactable door;
		public Door doorComponent;
		//public float cost;
		public Edge(Door d, RoomGraphManager manager)
		{
			node_1 = manager.GetNode(d.room_1);
			node_2 = manager.GetNode(d.room_2);
			door = d.door;
			doorComponent = d;
		}
		public bool Contains(Node node)
		{
			bool result = node_1 == node || node_2 == node;
			return result;
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
		Edge edge = new Edge (door, this);
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
		Room[] rooms = UnityEngine.Object.FindObjectsOfType<Room> ();
		Door[] doors = UnityEngine.Object.FindObjectsOfType<Door> ();
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
	public bool CanSeeAdjacentRoom(Room from, Room to)
	{
		bool result = false;
		if(from!=null && to!=null)
		{
			Node fromNode = GetNode (from);
			Node toNode = GetNode (to);

			foreach(Edge edge in fromNode.edges)
			{
				if(edge.Contains(toNode))
				{
					result = !edge.door.bObstructsVision;
					break;
				}
			}
		}
		return result;
	}
	public Room GetContainingRoom(Collider2D collider)
	{
		Room result = null;
		foreach(Node node in _nodes)
		{
			if(node.room._volume.IsTouching(collider))
			{
				result = node.room;
				break;
			}
		}
		return result;
	}
	/*private float FindCost_Distance(Edge edge)
	{
		float result = (edge.node_1.room.GetCenter () - edge.node_2.room.GetCenter ()).magnitude;
		return result;
	}
	public Room FindRoom_Dijkstra(Room start, Room end)
	{
		Node startNode = GetNode (start);
		Node endNode = GetNode (end);
		Room result = FindRoom_Dijkstra (startNode, endNode, null, null);
		return result;
	}
	private Room FindRoom_Dijkstra(Node startNode, Node endNode, List<Node> openList, List<Node> closedList)
	{
		if(openList == null || closedList == null)
		{
			openList = new List<Node>();
			closedList = new List<Node>();

			foreach(Node node in _nodes)
			{
				node.tentativeDistance = Single.PositiveInfinity;
				openList.Add(node);
			}
			foreach(Edge edge in _edges)
			{
				edge.cost = FindCost_Distance(edge);
			}
			startNode.tentativeDistance = 0;
		}
		openList.Remove (startNode);

		Edge closestEdge = GetShortestEdge (startNode.edges, closedList);
	}
	private Edge GetShortestEdge(List<Edge> edges, List<Node> visitedNodes)
	{
		Edge result = null;
		if(edges.Count>0)
		{
			foreach(Edge edge in edges)
			{
				if(!visitedNodes.Contains(edge.node_1)
				   &&!visitedNodes.Contains(edge.node_2))
				{
					if(edge.cost<result.cost)
					{
						result = edge;
					}
				}
			}
		}
		return result;
	}*/
}