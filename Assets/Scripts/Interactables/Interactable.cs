using UnityEngine;
using System.Collections.Generic;

public class Interactable : MonoBehaviour
{
	public List<BasicAction> actions = new List<BasicAction>();
	public bool bObstructsPassage;
	public bool bObstructsVision;
	public void Awake()
	{
	}
}
