using UnityEngine;
using System.Collections;

public class WiiBalanceBoardControl : MonoBehaviour {

	public GameObject wiiObject;
	private Wii wii; 
	static int whichRemote;
	public float speed;

	void Start () {
		 wii = wiiObject.GetComponent("Wii") as Wii;
	}
	
	void FixedUpdate () {
		if(Wii.IsActive(whichRemote)) //remote is on
		{		
			if(Wii.GetExpType(whichRemote)==3) //balance board is being used
			{
				Vector4 theBalanceBoard = Wii.GetBalanceBoard(whichRemote); 
				Vector2 theCenter = Wii.GetCenterOfBalance(whichRemote);
				Debug.Log(theBalanceBoard+" "+theCenter);
				Debug.Log("the total weight is: k" + Wii.GetTotalWeight(whichRemote));

				// move self according the center of gravity
				rigidbody.AddForce(new Vector3(theCenter.x*speed,0,theCenter.y*speed));
			}
		}
	}

	void OnWiimoteFound (int thisRemote) {
		Debug.Log("found this one: "+thisRemote);
		if(!Wii.IsActive(whichRemote))
			whichRemote = thisRemote;
	}
}
