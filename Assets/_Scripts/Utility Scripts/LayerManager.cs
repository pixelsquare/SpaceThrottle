using UnityEngine;
using System.Collections;

public enum Layers { 
Default = 0, 
Player = 8, 
TopCamera = 9, 
BottomCamera = 10 };

public class LayerManager : MonoBehaviour {
	public static Layers layer;
}
