using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomGraphVisualiser : MonoBehaviour
{
	public Color roomColor = Color.green;
	public Color seenRoomColor = Color.cyan;
	public Color unseenRoomColor = Color.magenta;
	public Color openRoomLinkColor = Color.blue;
	public Color blockedRoomLinkColor = Color.red;
	public float roomBoxWidth = 1;
	public float roomLinkWidth = 1;
	public RoomGraphManager graphManager;
	public static Texture2D _lineTex;
	new public Camera camera;
	public Collider2D playerCollider;
	//test variables
	public Vector2 _begin = new Vector2 (300, 100);
	public Vector2 _end = new Vector2 (500, 100);
	public Vector2 _offset = new Vector2 (10, 0);
	void Awake()
	{
		if(!_lineTex)
		{
			_lineTex = new Texture2D(1, 1);
			_lineTex.SetPixel(0, 0, Color.white);
			_lineTex.Apply();
		}
	}
	// Update is called once per frame
	void OnGUI ()
	{

		//DrawLine (_begin, _end, openRoomLinkColor, roomLinkWidth);
		//DrawLine (_begin, _begin + _offset, blockedRoomLinkColor, 10);
		//DrawLine (_end, _end + _offset, blockedRoomLinkColor, 10);
		Room[] rooms = graphManager.GetAllRooms ();
		Door[] doors = graphManager.GetAllDoors ();
		foreach(Room room in rooms)
		{
			Vector2 min, max;
			Color color = GetRoomColor(room);
			room.GetBounds(out min, out max);
			DrawBox(min, max, color, roomBoxWidth);
		}
		foreach(Door door in doors)
		{
			Color color = door.door.bObstructsPassage ? blockedRoomLinkColor : openRoomLinkColor;
			DrawLineInWorld(door.room_1.GetCenter(), door.room_2.GetCenter(), color, roomLinkWidth);
		}
	}
	private Color GetRoomColor(Room room)
	{
		Room playerRoom = graphManager.GetContainingRoom (playerCollider);
		Color result;
		if(playerRoom==room)
		{
			result=roomColor;
		}
		else if(graphManager.CanSeeAdjacentRoom(playerRoom, room))
		{
			result = seenRoomColor;
		}
		else
		{
			result = unseenRoomColor;
		}
		return result;
	}
	public void DrawLineInWorld(Vector2 begin, Vector2 end, Color color, float width)
	{
		Vector2 newBegin, newEnd;
		newBegin = WorldToScreen (begin);
		newEnd = WorldToScreen (end);
		DrawLine (newBegin, newEnd, color, width);
	}
	public Vector2 WorldToScreen(Vector2 v2)
	{
		Vector3 v3 = new Vector3 (v2.x, v2.y, 0);
		Vector3 result = camera.WorldToScreenPoint (v3);
		result.y = camera.pixelHeight - result.y;
		return result;
	}
	//taken from http://wiki.unity3d.com/index.php?title=DrawLine
	public static void DrawLine(Vector2 begin, Vector2 end, Color color, float width)
	{
		Color lastGuiColor = GUI.color;
		Matrix4x4 lastGuiMatrix = GUI.matrix;

		GUI.color = color;
		float angle = Vector3.Angle(end-begin, Vector2.right);
		
		// Vector3.Angle always returns a positive number.
		// If pointB is above pointA, then angle needs to be negative.
		if (begin.y > end.y) { angle = -angle; }
		
		// Use ScaleAroundPivot to adjust the size of the line.
		// We could do this when we draw the texture, but by scaling it here we can use
		//  non-integer values for the width and length (such as sub 1 pixel widths).
		// Note that the pivot point is at +.5 from pointA.y, this is so that the width of the line
		//  is centered on the origin at pointA.
		//GUIUtility.ScaleAroundPivot(new Vector2((end-begin).magnitude, width), new Vector2(begin.x, begin.y + 0.5f));
		
		// Set the rotation for the line.
		//  The angle was calculated with pointA as the origin.
		GUIUtility.RotateAroundPivot(angle, begin);
		// Finally, draw the actual line.
		// We're really only drawing a 1x1 texture from pointA.
		// The matrix operations done with ScaleAroundPivot and RotateAroundPivot will make this
		//  render with the proper width, length, and angle.
		//begin -= new Vector2 (0.5f, 0.5f);
		Rect rect = new Rect (begin.x, begin.y, (end-begin).magnitude, width);
		GUI.DrawTexture(rect, _lineTex);
		
		// We're done.  Restore the GUI matrix and GUI color to whatever they were before.
		GUI.color = lastGuiColor;
		GUI.matrix = lastGuiMatrix;
	}
	public void DrawBox(Vector2 min, Vector2 max, Color color, float width)
	{
		Vector2[] points = new Vector2[]
		{
			min,
			new Vector2 (min.x, max.y),
			max,
			new Vector2 (max.x, min.y),
			min
		};
		for(int i=1;i<points.Length;i++)
		{
			DrawLineInWorld(points[i-1], points[i], color, width);
		}
	}
}