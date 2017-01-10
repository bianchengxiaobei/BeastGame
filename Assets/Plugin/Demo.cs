using UnityEngine;
using System.Collections;

public class Demo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.LogError("start===================>>>>>>>>>>");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void echo(string str) {
		Debug.LogError(str);
	}

	public static void instance() {
		GameObject prefab = Resources.Load("Sphere") as GameObject;
		GameObject go = Instantiate(prefab) as GameObject;
	}
}
