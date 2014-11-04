using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
[ExecuteInEditMode()]

public class Wii : MonoBehaviour
{	

	public static void StartSearch()
    {
		if(!isAwake)
	    	WakeUp();

    	for(int x=0;x<max;x++)
    		if(!wiiRemotesActive[x])
     		{
     			//Debug.Log("I said find it!!");
     			findWiiRemote();
     			searching = true;
     			return;
     		}
     	Debug.Log("Wii remotes at capacity!");
    }

	public static int GetDiscoveryStatus()
	{
		return getDiscoveryStatus();
	}

    public static void StopSearch()
    {
    	//Debug.Log("I said stop it!");
    	searching = false;
        stopSearch();
    }

    public static void DropWiiRemote(int i)
    {
		if(i<0 || i>(max-1))
		{
			Debug.Log("invalid remote number");
			return;
    	}
		if(!isAwake)
	    	WakeUp();
	
		//Debug.Log("I said drop it!");
    	rumbling[i]=false;
       	dropWiiRemote(i);
		currentStates[i].active=false;	
       	wiiRemotesActive[i] = false;
       	isVirtual[i] = false;
    }

	public static int GetRemoteCount()
    {
		if(!isAwake)
	    	WakeUp();

    	int theCount = 0;
    	for(int x=0;x<max;x++)
    	{
    		if(IsActive(x))
    			theCount++;	
    	}
    	return theCount;
    }

	public static bool IsActive(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

    	return wiiRemotesActive[remote];
    }

	public static bool IsSearching()
    {
    	return searching;	
    }

	public static float GetBattery(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return 0.0f;
    	}
		if(!isAwake)
	    	WakeUp();

    	return currentStates[remote].battery;
    }
    
    public static void SetLEDs(int remote, bool LED1, bool LED2, bool LED3, bool LED4)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return;
    	}
		if(!isAwake)
	    	WakeUp();

    	if(LED1!=theLEDs[remote][0] || LED2!=theLEDs[remote][1] || LED3!=theLEDs[remote][2] || LED4!=theLEDs[remote][3])
    	{
    		setLEDs(remote,LED1,LED2,LED3,LED4);
    		theLEDs[remote][0] = LED1;
    		theLEDs[remote][1] = LED2;
    		theLEDs[remote][2] = LED3;
    		theLEDs[remote][3] = LED4;
    	}
    }
    
    public static bool[] GetLEDS(int remote)
    {
    	bool[] leds;
    	leds = new bool[4];
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return leds;
    	}
		if(!isAwake)
	    	WakeUp();

    	leds[0] = theLEDs[remote][0];
    	leds[1] = theLEDs[remote][1];
    	leds[2] = theLEDs[remote][2];
    	leds[3] = theLEDs[remote][3];
    	
    	return leds;
    }
        
    public static void SetRumble(int remote, bool enabled)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return;
    	}
		if(!isAwake)
	    	WakeUp();

    	rumbling[remote] = enabled;
    	if(wiiRemotesActive[remote] && !isVirtual[remote])
    		setForceFeedback(remote,enabled);
    }
    
    public static bool GetRumble(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

    	return rumbling[remote];
    }
    
    public static void ToggleRumble(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return;
    	}
		if(!isAwake)
	    	WakeUp();

    	setForceFeedback(remote,!GetRumble(remote));	
    }
    
    public static bool GetButton(int remote, string s)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

        s.ToUpper();
        s.Trim();
        switch (s)
	    {
	    	case "A":
	    		return currentStates[remote].a;
	    	case "B":
				return currentStates[remote].b;
			case "ONE":
			case "1":
				return currentStates[remote].one;
			case "TWO":
			case "2":
				return currentStates[remote].two;
			case "PLUS":
			case "+":
				return currentStates[remote].plus;
			case "MINUS":
			case "-":
				return currentStates[remote].minus;
			case "HOME":
			case "H":
				return currentStates[remote].home;
			case "UP":
				return currentStates[remote].up;
			case "DOWN":
				return currentStates[remote].down;
			case "LEFT":
				return currentStates[remote].left;
			case "RIGHT":
				return currentStates[remote].right;

			case "C":
				return currentStates[remote].expButton1;
			case "Z":
				return currentStates[remote].expButton2;
	            
			case "CLASSICA":
				return currentStates[remote].expButton1;    
			case "CLASSICB":
				return currentStates[remote].expButton2;
			case "CLASSICMINUS":
				return currentStates[remote].expButton9;
			case "CLASSICPLUS":
				return currentStates[remote].expButton10;
			case "CLASSICHOME":
				return currentStates[remote].expButton11;
			case "CLASSICX":
				return currentStates[remote].expButton3;
			case "CLASSICY":
				return currentStates[remote].expButton4;
			case "CLASSICUP":
				return currentStates[remote].expButton8;
			case "CLASSICDOWN":
				return currentStates[remote].expButton6;
			case "CLASSICLEFT":
				return currentStates[remote].expButton5;
			case "CLASSICRIGHT":
				return currentStates[remote].expButton7;
			case "CLASSICL":
				return currentStates[remote].expButton14;
			case "CLASSICR":
				return currentStates[remote].expButton15;
			case "CLASSICZL":
				return currentStates[remote].expButton12;
			case "CLASSICZR":
				return currentStates[remote].expButton13;
				
			case "GUITARGREEN":
	           	return currentStates[remote].expButton1;    
	        case "GUITARRED":
	           	return currentStates[remote].expButton2;
	        case "GUITARYELLOW":
	          	return currentStates[remote].expButton3;
	        case "GUITARBLUE":
	        	return currentStates[remote].expButton4;
            case "GUITARORANGE":
            	return currentStates[remote].expButton5;
            case "GUITARPLUS":
            	return currentStates[remote].expButton6;
            case "GUITARMINUS":
            	return currentStates[remote].expButton7;
			
			case "DRUMGREEN":
				return currentStates[remote].expButton1;
			case "DRUMRED":
				return currentStates[remote].expButton2;
			case "DRUMYELLOW":
				return currentStates[remote].expButton3;
			case "DRUMBLUE":
				return currentStates[remote].expButton4;
			case "DRUMORANGE":
				return currentStates[remote].expButton5;
			case "DRUMPLUS":
				return currentStates[remote].expButton6;
			case "DRUMMINUS":
				return currentStates[remote].expButton7;
			case "DRUMPEDAL":
				return currentStates[remote].expButton8;
			
			case "TURNTABLEGREENLEFT":
				return currentStates[remote].expButton1;
			case "TURNTABLEREDLEFT":
				return currentStates[remote].expButton2;
			case "TURNTABLEBLUELEFT":
				return currentStates[remote].expButton3;
			case "TURNTABLEGREENRIGHT":
				return currentStates[remote].expButton4;
			case "TURNTABLEREDRIGHT":
				return currentStates[remote].expButton5;
			case "TURNTABLEBLUERIGHT":
				return currentStates[remote].expButton6;
			case "TURNTABLEEUPHORIA":
				return currentStates[remote].expButton7;
			case "TURNTABLEPLUS":
				return currentStates[remote].expButton8;
			case "TURNTABLEMINUS":
				return currentStates[remote].expButton9;
					
			default:
				print("Unsupported Button Type " + s);
				return false;
	    }
    }

    public static bool GetButtonDown(int remote, string s)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

        s.ToUpper();
        s.Trim();
	    switch (s)
	    {
	    	case "A":
	    		return !previousStates[remote].a && currentStates[remote].a;
	    	case "B":
				return !previousStates[remote].b && currentStates[remote].b;
			case "ONE":
			case "1":
				return !previousStates[remote].one && currentStates[remote].one;
			case "TWO":
			case "2":
				return !previousStates[remote].two && currentStates[remote].two;
			case "PLUS":
			case "+":
				return !previousStates[remote].plus && currentStates[remote].plus;
			case "MINUS":
			case "-":
				return !previousStates[remote].minus && currentStates[remote].minus;
			case "HOME":
			case "H":
				return !previousStates[remote].home && currentStates[remote].home;
			case "UP":
				return !previousStates[remote].up && currentStates[remote].up;
			case "DOWN":
				return !previousStates[remote].down && currentStates[remote].down;
			case "LEFT":
				return !previousStates[remote].left && currentStates[remote].left;
			case "RIGHT":
				return !previousStates[remote].right && currentStates[remote].right;

			case "C":
				return !previousStates[remote].expButton1 && currentStates[remote].expButton1;
			case "Z":
				return !previousStates[remote].expButton2 && currentStates[remote].expButton2;
	            
			case "CLASSICA":
				return !previousStates[remote].expButton1 && currentStates[remote].expButton1;    
			case "CLASSICB":
				return !previousStates[remote].expButton2 && currentStates[remote].expButton2;
			case "CLASSICMINUS":
				return !previousStates[remote].expButton9 && currentStates[remote].expButton9;
			case "CLASSICPLUS":
				return !previousStates[remote].expButton10 && currentStates[remote].expButton10;
			case "CLASSICHOME":
				return !previousStates[remote].expButton11 && currentStates[remote].expButton11;
			case "CLASSICX":
				return !previousStates[remote].expButton3 && currentStates[remote].expButton3;
			case "CLASSICY":
				return !previousStates[remote].expButton4 && currentStates[remote].expButton4;
			case "CLASSICUP":
				return !previousStates[remote].expButton8 && currentStates[remote].expButton8;
			case "CLASSICDOWN":
				return !previousStates[remote].expButton6 && currentStates[remote].expButton6;
			case "CLASSICLEFT":
				return !previousStates[remote].expButton5 && currentStates[remote].expButton5;
			case "CLASSICRIGHT":
				return !previousStates[remote].expButton7 && currentStates[remote].expButton7;
			case "CLASSICL":
				return !(previousStates[remote].expButton14) && currentStates[remote].expButton14;
			case "CLASSICR":
				return !(previousStates[remote].expButton15) && currentStates[remote].expButton15;
			case "CLASSICZL":
				return !previousStates[remote].expButton12 && currentStates[remote].expButton12;
			case "CLASSICZR":
				return !previousStates[remote].expButton13 && currentStates[remote].expButton13;

			case "GUITARGREEN":
	           	return !previousStates[remote].expButton1 && currentStates[remote].expButton1;    
	        case "GUITARRED":
	           	return !previousStates[remote].expButton2 && currentStates[remote].expButton2;
	        case "GUITARYELLOW":
	          	return !previousStates[remote].expButton3 && currentStates[remote].expButton3;
	        case "GUITARBLUE":
	        	return !previousStates[remote].expButton4 && currentStates[remote].expButton4;
            case "GUITARORANGE":
            	return !previousStates[remote].expButton5 && currentStates[remote].expButton5;
            case "GUITARPLUS":
            	return !previousStates[remote].expButton6 && currentStates[remote].expButton6;
            case "GUITARMINUS":
            	return !previousStates[remote].expButton7 && currentStates[remote].expButton7;
			
			case "DRUMGREEN":
				return !previousStates[remote].expButton1 && currentStates[remote].expButton1;
			case "DRUMRED":
				return !previousStates[remote].expButton2 && currentStates[remote].expButton2;
			case "DRUMYELLOW":
				return !previousStates[remote].expButton3 && currentStates[remote].expButton3;
			case "DRUMBLUE":
				return !previousStates[remote].expButton4 && currentStates[remote].expButton4;
			case "DRUMORANGE":
				return !previousStates[remote].expButton5 && currentStates[remote].expButton5;
			case "DRUMPLUS":
				return !previousStates[remote].expButton6 && currentStates[remote].expButton6;
			case "DRUMMINUS":
				return !previousStates[remote].expButton7 && currentStates[remote].expButton7;
			case "DRUMPEDAL":
				return !previousStates[remote].expButton8 && currentStates[remote].expButton8;
			
			case "TURNTABLEGREENLEFT":
				return !previousStates[remote].expButton1 && currentStates[remote].expButton1;
			case "TURNTABLEREDLEFT":
				return !previousStates[remote].expButton2 && currentStates[remote].expButton2;
			case "TURNTABLEBLUELEFT":
				return !previousStates[remote].expButton3 && currentStates[remote].expButton3;
			case "TURNTABLEGREENRIGHT":
				return !previousStates[remote].expButton4 && currentStates[remote].expButton4;
			case "TURNTABLEREDRIGHT":
				return !previousStates[remote].expButton5 && currentStates[remote].expButton5;
			case "TURNTABLEBLUERIGHT":
				return !previousStates[remote].expButton6 && currentStates[remote].expButton6;
			case "TURNTABLEEUPHORIA":
				return !previousStates[remote].expButton7 && currentStates[remote].expButton7;
			case "TURNTABLEPLUS":
				return !previousStates[remote].expButton8 && currentStates[remote].expButton8;
			case "TURNTABLEMINUS":
				return !previousStates[remote].expButton9 && currentStates[remote].expButton9;
		
			default:
				print("Unsupported Button Type " + s);
				return false;
		}
    }

    public static bool GetButtonUp(int remote, string s)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

        s.ToUpper();
        s.Trim();
		switch (s)
		{
			case "A":
				return previousStates[remote].a && !currentStates[remote].a;
			case "B":
				return previousStates[remote].b && !currentStates[remote].b;
			case "ONE":
			case "1":
				return previousStates[remote].one && !currentStates[remote].one;
			case "TWO":
			case "2":
				return previousStates[remote].two && !currentStates[remote].two;
			case "PLUS":
			case "+":
				return previousStates[remote].plus && !currentStates[remote].plus;
			case "MINUS":
			case "-":
				return previousStates[remote].minus && !currentStates[remote].minus;
			case "HOME":
			case "H":
				return previousStates[remote].home && !currentStates[remote].home;
			case "UP":
				return previousStates[remote].up && !currentStates[remote].up;
			case "DOWN":
				return previousStates[remote].down && !currentStates[remote].down;
			case "LEFT":
				return previousStates[remote].left && !currentStates[remote].left;
			case "RIGHT":
				return previousStates[remote].right && !currentStates[remote].right;
	
			case "C":
				return previousStates[remote].expButton1 && !currentStates[remote].expButton1;
			case "Z":
				return previousStates[remote].expButton2 && !currentStates[remote].expButton2;
	                
			case "CLASSICA":
				return previousStates[remote].expButton1 && !currentStates[remote].expButton1;    
			case "CLASSICB":
				return previousStates[remote].expButton2 && !currentStates[remote].expButton2;
			case "CLASSICMINUS":
				return previousStates[remote].expButton9 && !currentStates[remote].expButton9;
			case "CLASSICPLUS":
				return previousStates[remote].expButton10 && !currentStates[remote].expButton10;
			case "CLASSICHOME":
				return previousStates[remote].expButton11 && !currentStates[remote].expButton11;
			case "CLASSICX":
				return previousStates[remote].expButton3 && !currentStates[remote].expButton3;
			case "CLASSICY":
				return previousStates[remote].expButton4 && !currentStates[remote].expButton4;
			case "CLASSICUP":
				return previousStates[remote].expButton8 && !currentStates[remote].expButton8;
			case "CLASSICDOWN":
				return previousStates[remote].expButton6 && !currentStates[remote].expButton6;
			case "CLASSICLEFT":
				return previousStates[remote].expButton5 && !currentStates[remote].expButton5;
			case "CLASSICRIGHT":
				return previousStates[remote].expButton7 && !currentStates[remote].expButton7;
			case "CLASSICL":
				return (previousStates[remote].expButton14) && !(currentStates[remote].expButton14);
			case "CLASSICR":
				return (previousStates[remote].expButton15) && !(currentStates[remote].expButton15);
			case "CLASSICZL":
				return previousStates[remote].expButton12 && !currentStates[remote].expButton12;
			case "CLASSICZR":
				return previousStates[remote].expButton13 && !currentStates[remote].expButton13;

			case "GUITARGREEN":
	           	return previousStates[remote].expButton1 && !currentStates[remote].expButton1;    
	        case "GUITARRED":
	           	return previousStates[remote].expButton2 && !currentStates[remote].expButton2;
	        case "GUITARYELLOW":
	          	return previousStates[remote].expButton3 && !currentStates[remote].expButton3;
	        case "GUITARBLUE":
	        	return previousStates[remote].expButton4 && !currentStates[remote].expButton4;
            case "GUITARORANGE":
            	return previousStates[remote].expButton5 && !currentStates[remote].expButton5;
            case "GUITARPLUS":
            	return previousStates[remote].expButton6 && !currentStates[remote].expButton6;
            case "GUITARMINUS":
            	return previousStates[remote].expButton7 && !currentStates[remote].expButton7;
			
			case "DRUMGREEN":
				return previousStates[remote].expButton1 && !currentStates[remote].expButton1;
			case "DRUMRED":
				return previousStates[remote].expButton2 && !currentStates[remote].expButton2;
			case "DRUMYELLOW":
				return previousStates[remote].expButton3 && !currentStates[remote].expButton3;
			case "DRUMBLUE":
				return previousStates[remote].expButton4 && !currentStates[remote].expButton4;
			case "DRUMORANGE":
				return previousStates[remote].expButton5 && !currentStates[remote].expButton5;
			case "DRUMPLUS":
				return previousStates[remote].expButton6 && !currentStates[remote].expButton6;
			case "DRUMMINUS":
				return previousStates[remote].expButton7 && !currentStates[remote].expButton7;
			case "DRUMPEDAL":
				return previousStates[remote].expButton8 && !currentStates[remote].expButton8;
			
			case "TURNTABLEGREENLEFT":
				return previousStates[remote].expButton1 && !currentStates[remote].expButton1;
			case "TURNTABLEREDLEFT":
				return previousStates[remote].expButton2 && !currentStates[remote].expButton2;
			case "TURNTABLEBLUELEFT":
				return previousStates[remote].expButton3 && !currentStates[remote].expButton3;
			case "TURNTABLEGREENRIGHT":
				return previousStates[remote].expButton4 && !currentStates[remote].expButton4;
			case "TURNTABLEREDRIGHT":
				return previousStates[remote].expButton5 && !currentStates[remote].expButton5;
			case "TURNTABLEBLUERIGHT":
				return previousStates[remote].expButton6 && !currentStates[remote].expButton6;
			case "TURNTABLEEUPHORIA":
				return previousStates[remote].expButton7 && !currentStates[remote].expButton7;
			case "TURNTABLEPLUS":
				return previousStates[remote].expButton8 && !currentStates[remote].expButton8;
			case "TURNTABLEMINUS":
				return previousStates[remote].expButton9 && !currentStates[remote].expButton9;

			default:
				print("Unsupported Button Type " + s);
				return false;
	        }
    }

	public static Vector3 GetWiimoteAcceleration(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return Vector3.zero;
    	}
		if(!isAwake)
	    	WakeUp();

    	if(float.IsNaN(currentStates[remote].wiiAccelX)||
		currentStates[remote].wiiAccelX == Mathf.Infinity||
		currentStates[remote].wiiAccelX == Mathf.NegativeInfinity)
		{
			return Vector3.zero;
		}	
    	float theAccelX = currentStates[remote].wiiAccelX;
    	float theAccelY = currentStates[remote].wiiAccelY;
    	float theAccelZ = currentStates[remote].wiiAccelZ;
    	
		return new Vector3(theAccelX,theAccelY,theAccelZ);
    }

	public static float GetIRSensitivity(int remote)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return 50.0f;
    	}
		if(!isAwake)
	    	WakeUp();

		return IRSensitivity[remote];
	}
	
	public static void SetIRSensitivity(int i, float sensitivity)
	{
		if(i<0 || i>(max-1))
		{
			Debug.Log("invalid remote number");
			return;
    	}
		if(!isAwake)
	    	WakeUp();

		if(sensitivity>100)
			sensitivity=100;
		if(sensitivity<1)
			sensitivity=1;
		IRSensitivity[i] = sensitivity;
		setIRSensitivity(i,sensitivity);
	}
	
    public static Vector2 GetIRPosition(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return Vector2.zero;
    	}
		if(!isAwake)
	    	WakeUp();

    	if(isVirtual[remote])
    	{
    		return virtualIR[remote];	
    	}
    	
    	float IRPointerX = -1.0f;
    	float IRPointerY = -1.0f;
    	
    	
    	if (currentStates[remote].rawIR1.y == 1023 && currentStates[remote].rawIR2.y == 1023) {
    		pointsDifference[remote] = Vector2.zero;
    	 	return new Vector2(IRPointerX,IRPointerY);
    	}
    	
    	Vector2 point1;
    	Vector2 point2;
    	
    	float rotation = 0.0f;
		if ((currentStates[remote].wiiAccelZ+previousStates[remote].wiiAccelZ)/2.0f >0) {//right side up
			rotation = Mathf.Atan(currentStates[remote].wiiAccelX/currentStates[remote].wiiAccelZ)/3.0f;
			rotation = (rotation + (Mathf.Atan(previousStates[remote].wiiAccelX/previousStates[remote].wiiAccelZ)/3.0f))/2.0f;
		}
		else if((currentStates[remote].wiiAccelZ+previousStates[remote].wiiAccelZ)/2.0f<0){//upside down
			rotation = (3+Mathf.Atan(currentStates[remote].wiiAccelX/currentStates[remote].wiiAccelZ))/3.0f;
			rotation = (rotation + ((3+Mathf.Atan(previousStates[remote].wiiAccelX/previousStates[remote].wiiAccelZ))/3.0f))/2.0f;
		}
		else {
			if ((currentStates[remote].wiiAccelX+previousStates[remote].wiiAccelX)/2.0f>0) {
				rotation=.5f;
			}
			else {
				rotation=-.5f;
			}
		}
		if(rotation>1.0)
			rotation-=2;
		
		float rotRad = rotation*Mathf.PI;
		
		if(currentStates[remote].rawIR1.y != 1023 && currentStates[remote].rawIR2.y != 1023)
		{
			//CONTAINS BOTH IR POINTS
			point1 = new Vector2(currentStates[remote].rawIR1.x,currentStates[remote].rawIR1.y);
    		point2 = new Vector2(currentStates[remote].rawIR2.x,currentStates[remote].rawIR2.y);
 			
 			switchedIR[remote] = 0;
 			if(point1.x > point2.x)
 			{
 				switchedIR[remote] +=1;
 			}
 			if(point1.y > point2.y)
 			{
 				switchedIR[remote] +=2;
 			}		
 			
    		float hyp = Vector2.Distance(point1,point2);
    		float currentAngle = Mathf.Atan((point2.y-point1.y)/(point2.x-point1.x));
    			
    		pointsDifference[remote].x = Mathf.Cos(currentAngle-rotRad)*hyp;
    		pointsDifference[remote].y = Mathf.Sin(currentAngle-rotRad)*hyp;
   		}
		else
		{
			float tit = Mathf.Cos(rotRad);
			float tat = Mathf.Sin(rotRad);
		    	
    		if(currentStates[remote].rawIR1.y == 1023)
    		{
    			//Debug.Log("IR 1 MISSING");
    			if(pointsDifference[remote] ==Vector2.zero)//entering in with 1 IR
    		 	 	return new Vector2(IRPointerX,IRPointerY);
    		 	 	
    			point2 = new Vector2(currentStates[remote].rawIR2.x,currentStates[remote].rawIR2.y);
    			float virtualX =0;
    			float virtualY =0;
    			if(switchedIR[remote]==0)//nothing switched
    			{
    				virtualX = point2.x - (tit*pointsDifference[remote].x) 
    								    - (tat*pointsDifference[remote].y);
    				virtualY = point2.y - (tit*pointsDifference[remote].y) 
    								    - (tat*pointsDifference[remote].x);				    
    			}
    			else if(switchedIR[remote]==1)//switched x only
    			{
    				virtualX = point2.x + (tit*pointsDifference[remote].x) 
    								    - (tat*pointsDifference[remote].y);
    				virtualY = point2.y - (tit*pointsDifference[remote].y) 
    								    + (tat*pointsDifference[remote].x);
    			}
    			else if(switchedIR[remote]==2)//switched y only
    			{
    				virtualX = point2.x - (tit*pointsDifference[remote].x) 
    								    + (tat*pointsDifference[remote].y);
    				virtualY = point2.y + (tit*pointsDifference[remote].y) 
    								    - (tat*pointsDifference[remote].x);
    			}
    			else if(switchedIR[remote]==3)//switched both
    			{
    				virtualX = point2.x + (tit*pointsDifference[remote].x) 
    								    + (tat*pointsDifference[remote].y);
    				virtualY = point2.y + (tit*pointsDifference[remote].y) 
    								    + (tat*pointsDifference[remote].x);
    			}
    			
    			point1 = new Vector2(virtualX,virtualY);
    		}
    		else if(currentStates[remote].rawIR2.y == 1023)
    		{
    			//Debug.Log("IR 2 MISSING");
    			if(pointsDifference[remote] ==Vector2.zero)//entering in with 1 IR
    		 	 	return new Vector2(IRPointerX,IRPointerY);
    	 	 	
    			point1 = new Vector2(currentStates[remote].rawIR1.x,currentStates[remote].rawIR1.y);
    			float virtualX =0;
    			float virtualY =0;
    			if(switchedIR[remote]==0)//nothing switched
    			{
    				virtualX = point1.x + (tit*pointsDifference[remote].x) 
    								    + (tat*pointsDifference[remote].y);
    				virtualY = point1.y + (tit*pointsDifference[remote].y) 
    								    + (tat*pointsDifference[remote].x);				    
    			}
    			else if(switchedIR[remote]==1)//switched x only
    			{
    				virtualX = point1.x - (tit*pointsDifference[remote].x) 
    								    + (tat*pointsDifference[remote].y);
    				virtualY = point1.y + (tit*pointsDifference[remote].y) 
    								    - (tat*pointsDifference[remote].x);
    			}
    			else if(switchedIR[remote]==2)//switched y only
    			{
    				virtualX = point1.x + (tit*pointsDifference[remote].x) 
    								    - (tat*pointsDifference[remote].y);
    				virtualY = point1.y - (tit*pointsDifference[remote].y) 
    								    + (tat*pointsDifference[remote].x);
    			}
    			else if(switchedIR[remote]==3)//switched both
    			{
    				virtualX = point1.x - (tit*pointsDifference[remote].x) 
    								    - (tat*pointsDifference[remote].y);
    				virtualY = point1.y - (tit*pointsDifference[remote].y) 
    								    - (tat*pointsDifference[remote].x);
    			}
    			
    			point2 = new Vector2(virtualX,virtualY);
    		}
    		
    		else
    		{
    			//NONE ARE VISIBLE	
    			return new Vector2(-1,-1);
    		}
		} 
    	
    	float simpleX = (point1.x + point2.x)/2.0f;
		float simpleY = (point1.y + point2.y)/2.0f;
    	float hypot = Vector2.Distance(point1,point2);
		simpleX = simpleX/1016.0f;
		simpleY = simpleY/760.0f;
						
		if(rotation>0)//on right side
		{
			if (rotation>0.5) {//upside down
				IRPointerX = (float)( ((-2.0*Mathf.Abs(rotation-.5f)+1.0)*simpleY)
									+ ((2.0*Mathf.Abs(rotation-.5f))*(1-simpleX)) );
				IRPointerY = (float)( ((-2.0*Mathf.Abs(rotation-.5f)+1.0)*(1-simpleX))
									+ ((2.0*Mathf.Abs(rotation-.5f))*(1-simpleY)) );
			}
			else {//
				IRPointerX = (float)( ((-2.0*Mathf.Abs(rotation-.5f)+1.0)*simpleY)
									+ ((2.0*Mathf.Abs(rotation-.5f))*simpleX) );
				IRPointerY = (float)( ((-2.0*Mathf.Abs(rotation-.5f)+1.0)*(1-simpleX))
									+ ((2.0*Mathf.Abs(rotation-.5f))*simpleY) );
			}
		}
		else {//on left side
			if (rotation<-0.5) {//upside down
				IRPointerX = (float)( (((-2.0*Mathf.Abs(rotation+.5f))+1.0)*(1.0-simpleY))
									+ ((2.0*Mathf.Abs(rotation+.5f))*(1-simpleX)) );
				IRPointerY = (float)( (((-2.0*Mathf.Abs(rotation+.5f))+1.0)*simpleX)
									+ ((2.0*Mathf.Abs(rotation+.5f))*(1-simpleY)) );
			}
			else {
				IRPointerX = (float)( (((-2.0*Mathf.Abs(rotation+.5f))+1.0)*(1.0-simpleY))
									+ ((2.0*Mathf.Abs(rotation+.5f))*simpleX) );
				IRPointerY = (float)( (((-2.0*Mathf.Abs(rotation+.5f))+1.0)*simpleX)
									+ ((2.0*Mathf.Abs(rotation+.5f))*simpleY) );
			}
		}
		float boost = 1+(hypot/(51400.0f/IRSensitivity[remote]));
		IRPointerX = (IRPointerX-.5f)*boost+.5f;
		IRPointerY = (IRPointerY-.5f)*boost+.5f;
		
		if(float.IsNaN(IRPointerX) || IRPointerX>1 || IRPointerX<0 || IRPointerY>1 || IRPointerY<0)
			return new Vector2(-1,-1);
			
	 	return new Vector2(1-IRPointerX,1-IRPointerY);
    }
    
    public static Vector3 GetIRVector3(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return Vector3.zero;
    	}
		if(!isAwake)
	    	WakeUp();

    	Vector2 theVec2 = GetIRPosition(remote);
    	float theSize;
    	if(currentStates[remote].rawIR1.s !=0 && currentStates[remote].rawIR1.s !=0)
    	{
    		theSize = (currentStates[remote].rawIR1.s + currentStates[remote].rawIR1.s)/2;
    	}
    	else
    		theSize = (currentStates[remote].rawIR1.s + currentStates[remote].rawIR1.s);
    	
    	return new Vector3(theVec2.x,theVec2.y,theSize);
    }
    
    public static float GetIRRotation(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return 0.0f;
    	}
		if(!isAwake)
	    	WakeUp();

    	if(isVirtual[remote])
	        return virtualIRRot[remote];
	   	
	   	float rotation;
		if (currentStates[remote].wiiAccelZ>0) {//right side up
			rotation = Mathf.Atan(currentStates[remote].wiiAccelX/currentStates[remote].wiiAccelZ)/3.0f;
		}
		else if(currentStates[remote].wiiAccelZ<0){//upside down
			rotation = (3.0f+Mathf.Atan(currentStates[remote].wiiAccelX/currentStates[remote].wiiAccelZ))/3.0f;
		}
		else {
			if (currentStates[remote].wiiAccelX>0) {
				rotation=.5f;
			}
			else {
				rotation=-.5f;
			}
		}
		
		if(rotation>1.0)
			rotation-=2.0f;
		
		return rotation*Mathf.PI;
    }
    
    public static Vector3[] GetRawIRData(int remote)
    {
    	//1023 is out
    	//x 0-1016
    	//y 0-760
    	Vector3[] theRaw;
    	theRaw = new Vector3[4];
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return theRaw;
    	}    
		if(!isAwake)
	    	WakeUp();	
	
    	float tempX = (float)currentStates[remote].rawIR1.x;
    	float tempY = (float)currentStates[remote].rawIR1.y;
    	if(tempX != 1023 && tempY !=1023)
    	{
    		tempX /= 1016.0f;
    		tempY/=760.0f;
    		theRaw[0] = new Vector3 (1.0f-tempX,1.0f-tempY,currentStates[remote].rawIR1.s);
    	}
		else
		{
			theRaw[0] = new Vector3 (-1.0f,-1.0f,0.0f);
		}
		
		tempX = (float)currentStates[remote].rawIR2.x;
    	tempY = (float)currentStates[remote].rawIR2.y;
    	if(tempX != 1023 && tempY !=1023)
    	{
    		tempX /= 1016.0f;
    		tempY/=760.0f;
    		theRaw[1] = new Vector3 (1.0f-tempX,1.0f-tempY,currentStates[remote].rawIR2.s);
    	}
		else
		{
			theRaw[1] = new Vector3 (-1.0f,-1.0f,0.0f);
		}
		
		tempX = (float)currentStates[remote].rawIR3.x;
    	tempY = (float)currentStates[remote].rawIR3.y;
    	if(tempX != 1023 && tempY !=1023)
    	{
    		tempX /= 1016.0f;
    		tempY/=760.0f;
    		theRaw[2] = new Vector3 (1.0f-tempX,1.0f-tempY,currentStates[remote].rawIR3.s);
    	}
		else
		{
			theRaw[2] = new Vector3 (-1.0f,-1.0f,0.0f);
		}
		
		tempX = (float)currentStates[remote].rawIR4.x;
    	tempY = (float)currentStates[remote].rawIR4.y;
    	if(tempX != 1023 && tempY !=1023)
    	{
    		tempX /= 1016.0f;
    		tempY/=760.0f;
    		theRaw[3] = new Vector3 (1.0f-tempX,1.0f-tempY,currentStates[remote].rawIR4.s);
    	}
		else
		{
			theRaw[3] = new Vector3 (-1.0f,-1.0f,0.0f);
		}
		
    	return theRaw;
    }

	public static bool HasMotionPlus(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

    	return currentStates[remote].motionPlusAvailable;
    }
	
	public static void CheckForMotionPlus(int remote)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
		}

		checkForMotionPlus(remote);
	}

	public static void DeactivateMotionPlus(int remote)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
		}

		deactivateMotionPlus(remote); 
	}
	
	public static void ShouldAutomaticallyCheckForMotionPlus(bool should)
	{
		automaticallyCheckForMotionPlus = should;
	}

	public static void ShouldAutomaticallyCalibrateMotionPlus(bool should)
	{
		automaticallyCalibrateMotionPlus = should;
	}

	public static bool IsMotionPlusCalibrated(int remote)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		return currentStates[remote].motionPlusCalibrated;
	}	

	public static Vector3 GetMotionPlus(int remote)
	{	
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return Vector3.zero;
    	}
		if(!isAwake)
	    	WakeUp();

		if(!currentStates[remote].motionPlusCalibrated)//uncalibrated
			return Vector3.zero;
		
		float theRoll =(float)currentStates[remote].roll;		
		float theYaw  =(float)currentStates[remote].yaw;
		float thePitch=(float)currentStates[remote].pitch;
		
		theRoll *=Time.deltaTime;
		theYaw  *=Time.deltaTime;
		thePitch*=Time.deltaTime;
		
		if(Mathf.Abs(theRoll) <.05)  theRoll  = 0;
		if(Mathf.Abs(theYaw)  <.05)  theYaw   = 0;
		if(Mathf.Abs(thePitch)<.05)  thePitch = 0;
		
		return new Vector3(theRoll,theYaw,thePitch);
	}

	public static Vector3 GetRawMotionPlus(int remote)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return Vector3.zero;
    	}
		if(!isAwake)
	    	WakeUp();
		
		float theRoll =(float)currentStates[remote].roll;		
		float theYaw  =(float)currentStates[remote].yaw;
		float thePitch=(float)currentStates[remote].pitch;
		
		return new Vector3(theRoll,theYaw,thePitch);
	}
	
	public static bool IsRollFast(int remote)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		return currentStates[remote].rollFast;	
	}
	
	public static bool IsYawFast(int remote)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		return currentStates[remote].yawFast;	
	}
	
	public static bool IsPitchFast(int remote)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		return currentStates[remote].pitchFast;	
	}
	
	public static void CalibrateMotionPlus(int remote, Vector3 calib)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return;
    	}
		setMotionPlusCalibration(remote,calib.x,calib.y,calib.z);
	}

	public static void CalibrateMotionPlus(int remote)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return;
    	}

		setMotionPlusCalibration(remote,currentStates[remote].roll,currentStates[remote].yaw,currentStates[remote].pitch);
	}

	public static void UncalibrateMotionPlus(int remote)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return;
    	}
		setMotionPlusCalibration(remote,0,0,0);
	}    
    
    public static int GetExpType(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return 0;
    	}
		if(!isAwake)
	    	WakeUp();
		
    	return currentStates[remote].expType;
    }

	public static Vector2 GetAnalogStick(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return Vector2.zero;
    	}
		if(!isAwake)
	    	WakeUp();

		switch(currentStates[remote].expType)
		{
			case (char)1:
				return GetAnalogStick(remote,"NUNCHUCK");
			case (char)2:
				return GetAnalogStick(remote,"CLASSICLEFT");
			case (char)4:
				return GetAnalogStick(remote,"GUITAR");
			case (char)5:
				return GetAnalogStick(remote,"DRUMS");
			case (char)6:
				return GetAnalogStick(remote,"TURNTABLE");
			default:
				return Vector2.zero;
			
		}
 	}
    
    public static Vector2 GetAnalogStick(int remote, string s)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return Vector2.zero;
    	}
		if(!isAwake)
	    	WakeUp();

		if(s == "CLASSICRIGHT")
		{
    		if(float.IsNaN(currentStates[remote].expFloat3)||
			currentStates[remote].expFloat3 == Mathf.Infinity||
			currentStates[remote].expFloat3 == Mathf.NegativeInfinity)
			{
				return Vector2.zero;
			}
		}
		else
		{
    		if(float.IsNaN(currentStates[remote].expFloat1)||
			currentStates[remote].expFloat1 == Mathf.Infinity||
			currentStates[remote].expFloat1 == Mathf.NegativeInfinity)
			{
				return Vector2.zero;
			}
		}

		switch(s)
    	{
    		case "NUNCHUCK": 
    	    	return new Vector2(currentStates[remote].expFloat1, currentStates[remote].expFloat2);
    	    case "CLASSICLEFT":
    	    	return new Vector2(currentStates[remote].expFloat1, currentStates[remote].expFloat2);
    	    case "CLASSICRIGHT":
    	    	return new Vector2(currentStates[remote].expFloat3, currentStates[remote].expFloat4);
    	    case "GUITAR":
    	    	//x 5 32 62
    	    	//y 5 32 60
    	    	float gStickX = currentStates[remote].expFloat1;
    	    	float gStickY = currentStates[remote].expFloat2;

				if(isVirtual[remote])
					return new Vector2(gStickX,gStickY);
    	    
            	gStickX = (gStickX-32.0f);
            	if(gStickX<0)
            		gStickX /=(32.0f-5.0f);
            	else
            		gStickX /=(62.0f-32.0f);
            
           	 	gStickY = (gStickY-32.0f);
            	if(gStickY<0)
            		gStickY /=(32.0f-5.0f);
            	else
            		gStickY /=(60.0f-32.0f);
    	    
    	    	return new Vector2(gStickX, gStickY);
    	    case "DRUMS":
    	    	//x 0 32 63
    	    	//y 0 31 63
    	    	float dStickX = currentStates[remote].expFloat1;
    	    	float dStickY = currentStates[remote].expFloat2;

				if(isVirtual[remote])
					return new Vector2(dStickX,dStickY);
    	    
            	dStickX = (dStickX-32.0f);
            	if(dStickX<0)
            		dStickX /=(32.0f);
            	else
            		dStickX /=(63.0f-32.0f);
            
           	 	dStickY = (dStickY-31.0f);
            	if(dStickY<0)
            		dStickY /=(31.0f);
            	else
            		dStickY /=(63.0f-31.0f);
    	    
    	    	return new Vector2(dStickX, dStickY);
    	    case "TURNTABLE":
    	    	//x 0 33 63
    	    	//y 0 32 63
    	    	float tStickX = currentStates[remote].expFloat1;
    	    	float tStickY = currentStates[remote].expFloat2;

				if(isVirtual[remote])
					return new Vector2(tStickX,tStickY);
    	    
            	tStickX = (tStickX-33.0f);
            	if(tStickX<0)
            		tStickX /=(33.0f);
            	else
            		tStickX /=(63.0f-33.0f);
            
           	 	tStickY = (tStickY-32.0f);
            	if(tStickY<0)
            		tStickY /=(32.0f);
            	else
            		tStickY /=(63.0f-32.0f);
    	    	
    	    	return new Vector2(tStickX,tStickY);
    	    default:
    	    	return Vector2.zero;
    	}
    }

	//get nunchuck stick
	public static Vector2 GetNunchuckAnalogStick(int remote)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return Vector2.zero;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==1)
		{
			return GetAnalogStick(remote,"NUNCHUCK");
		}
		return Vector2.zero;
	}

	public static bool  GetNunchuckButton(int remote, string s)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==1)
		{
			switch(s)
			{	
				case "C":
					return currentStates[remote].expButton1;
				case "Z":
					return currentStates[remote].expButton2;
			}
		}
		return false;
	}

	public static bool  GetNunchuckButtonDown(int remote, string s)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==1)
		{
			switch(s)
			{	
				case "C":
					return !previousStates[remote].expButton1 && currentStates[remote].expButton1;
				case "Z":
					return !previousStates[remote].expButton2 && currentStates[remote].expButton2;
			}
		}
		return false;
	}

	public static bool  GetNunchuckButtonUp(int remote, string s)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==1)
		{
			switch(s)
			{	
				case "C":
					return previousStates[remote].expButton1 && !currentStates[remote].expButton1;
				case "Z":
					return previousStates[remote].expButton2 && !currentStates[remote].expButton2;
			}
		}
		return false;
	}

	public static Vector3 GetNunchuckAcceleration(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return Vector3.zero;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType!=1)
		{
			return Vector3.zero;
		}
		float theX = (currentStates[remote].expFloat3);
		float theY = (currentStates[remote].expFloat4);
		float theZ = (currentStates[remote].expFloat5);
		
        return new Vector3(theX,theY,theZ);
    }
    
	//get classic button
	public static bool  GetClassicButton(int remote, string s)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==2)
		{
			switch(s)
			{	
				case "A":
					return currentStates[remote].expButton1;    
				case "B":
					return currentStates[remote].expButton2;
				case "MINUS":
					return currentStates[remote].expButton9;
				case "PLUS":
					return currentStates[remote].expButton10;
				case "HOME":
					return currentStates[remote].expButton11;
				case "X":
					return currentStates[remote].expButton3;
				case "Y":
					return currentStates[remote].expButton4;
				case "UP":
					return currentStates[remote].expButton8;
				case "DOWN":
					return currentStates[remote].expButton6;
				case "LEFT":
					return currentStates[remote].expButton5;
				case "RIGHT":
					return currentStates[remote].expButton7;
				case "L":
					return currentStates[remote].expButton14;
				case "R":
					return currentStates[remote].expButton15;
				case "ZL":
					return currentStates[remote].expButton12;
				case "ZR":
					return currentStates[remote].expButton13;
				default:
					print("Unsupported Button Type " + s);
				return false;
			}
		}
		return false;
	}

	public static bool  GetClassicButtonDown(int remote, string s)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==2)
		{
			switch(s)
			{	
				case "A":
					return !previousStates[remote].expButton1 && currentStates[remote].expButton1;    
				case "B":
					return !previousStates[remote].expButton2 && currentStates[remote].expButton2;
				case "MINUS":
					return !previousStates[remote].expButton9 && currentStates[remote].expButton9;
				case "PLUS":
					return !previousStates[remote].expButton10 && currentStates[remote].expButton10;
				case "HOME":
					return !previousStates[remote].expButton11 && currentStates[remote].expButton11;
				case "X":
					return !previousStates[remote].expButton3 && currentStates[remote].expButton3;
				case "Y":
					return !previousStates[remote].expButton4 && currentStates[remote].expButton4;
				case "UP":
					return !previousStates[remote].expButton8 && currentStates[remote].expButton8;
				case "DOWN":
					return !previousStates[remote].expButton6 && currentStates[remote].expButton6;
				case "LEFT":
					return !previousStates[remote].expButton5 && currentStates[remote].expButton5;
				case "RIGHT":
					return !previousStates[remote].expButton7 && currentStates[remote].expButton7;
				case "L":
					return !previousStates[remote].expButton14 && currentStates[remote].expButton14;
				case "R":
					return !previousStates[remote].expButton15 && currentStates[remote].expButton15;
				case "ZL":
					return !previousStates[remote].expButton12 && currentStates[remote].expButton12;
				case "ZR":
					return !previousStates[remote].expButton13 && currentStates[remote].expButton13;
				default:
					print("Unsupported Button Type " + s);
				return false;
			}
		}
		return false;
	}

	public static bool  GetClasicButtonUp(int remote, string s)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==2)
		{
			switch(s)
			{	
				case "A":
					return previousStates[remote].expButton1 && !currentStates[remote].expButton1;    
				case "B":
					return previousStates[remote].expButton2 && !currentStates[remote].expButton2;
				case "MINUS":
					return previousStates[remote].expButton9 && !currentStates[remote].expButton9;
				case "PLUS":
					return previousStates[remote].expButton10 && !currentStates[remote].expButton10;
				case "HOME":
					return previousStates[remote].expButton11 && !currentStates[remote].expButton11;
				case "X":
					return previousStates[remote].expButton3 && !currentStates[remote].expButton3;
				case "Y":
					return previousStates[remote].expButton4 && !currentStates[remote].expButton4;
				case "UP":
					return previousStates[remote].expButton8 && !currentStates[remote].expButton8;
				case "DOWN":
					return previousStates[remote].expButton6 && !currentStates[remote].expButton6;
				case "LEFT":
					return previousStates[remote].expButton5 && !currentStates[remote].expButton5;
				case "RIGHT":
					return previousStates[remote].expButton7 && !currentStates[remote].expButton7;
				case "L":
					return previousStates[remote].expButton14 && !currentStates[remote].expButton14;
				case "R":
					return previousStates[remote].expButton15 && !currentStates[remote].expButton15;
				case "ZL":
					return previousStates[remote].expButton12 && !currentStates[remote].expButton12;
				case "ZR":
					return previousStates[remote].expButton13 && !currentStates[remote].expButton13;
				default:
					print("Unsupported Button Type " + s);
				return false;
			}
		}
		return false;
	}

	//get classic stick
	public static Vector2 GetClassicAnalogStick(int remote, string s)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return Vector2.zero;
    	}
		if(!isAwake)
	    	WakeUp();
		
    	if(currentStates[remote].expType!=2 || float.IsNaN(currentStates[remote].expFloat1) || float.IsNaN(currentStates[remote].expFloat3))
			return Vector2.zero;
		switch(s)
    	{
    	    case "LEFT":
    	    	return new Vector2(currentStates[remote].expFloat1, currentStates[remote].expFloat2);
    	    case "RIGHT":
    	    	return new Vector2(currentStates[remote].expFloat3, currentStates[remote].expFloat4);
			default:
				print("Unsupported Analog Stick Type " + s);
				return Vector2.zero;
		}
	}

    static float classicAnalogButtonMin = 2.0f;
    static float classicAnalogButtonMax = 31.0f;
    public static float GetAnalogButton(int remote, string s)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return 0.0f;
    	}
		if(!isAwake)
	    	WakeUp();

 		if(currentStates[remote].expType==2)
 		{
 			switch(s)
 			{
 				case "CLASSICL":
					if(isVirtual[remote])
						return currentStates[remote].expFloat5;
 					return (currentStates[remote].expFloat5-classicAnalogButtonMin)/(classicAnalogButtonMax-classicAnalogButtonMin);
 				case "CLASSICR":
 					if(isVirtual[remote])
						return currentStates[remote].expFloat6;
					return (currentStates[remote].expFloat6-classicAnalogButtonMin)/(classicAnalogButtonMax-classicAnalogButtonMin);
 				default:
    	    	return 0.0f;
 			}
 		}
 		else
 			return 0.0f;
    }

	public static Vector4 GetBalanceBoard(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return Vector4.zero;
    	}
		if(!isAwake)
	    	WakeUp();

    	if(currentStates[remote].expType != 3)
		{
    		return Vector4.zero;
    	}	
    	return new Vector4(currentStates[remote].expFloat5,
    		currentStates[remote].expFloat6,
    		currentStates[remote].expFloat7,
    		currentStates[remote].expFloat8);	
    }
    
    public static Vector4 GetRawBalanceBoard(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return Vector4.zero;
    	}
		if(!isAwake)
	    	WakeUp();

    	if(currentStates[remote].expType != 3)
    		return Vector4.zero;
    		
    	return new Vector4(currentStates[remote].expFloat1,
    		currentStates[remote].expFloat2,
    		currentStates[remote].expFloat3,
    		currentStates[remote].expFloat4);	
    }
    
    public static Vector2 GetCenterOfBalance(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return Vector2.zero;
    	}
		if(!isAwake)
	    	WakeUp();

    	if(currentStates[remote].expType !=3)
    		return Vector2.zero;
    	
    	float leftSide   = currentStates[remote].expFloat6+currentStates[remote].expFloat8; 
    	float rightSide  = currentStates[remote].expFloat5+currentStates[remote].expFloat7;
    	float frontSide  = currentStates[remote].expFloat6+currentStates[remote].expFloat5; 
    	float backSide   = currentStates[remote].expFloat8+currentStates[remote].expFloat7;
    	float theX=0.0f;
    	float theY=0.0f;
    	
    	if(leftSide+rightSide != 0)
    		theX = (rightSide-leftSide)/(rightSide+leftSide);
    	if(frontSide+backSide != 0)
    		theY = (frontSide-backSide)/(frontSide+backSide);
    	 
    	 return new Vector2(theX,theY);
    }
    
    public static float GetTotalWeight(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return 0.0f;
    	}
		if(!isAwake)
	    	WakeUp();

    	if(currentStates[remote].expType !=3)
    		return 0.0f;
    	
    	return currentStates[remote].expFloat5+
    		currentStates[remote].expFloat6+
    		currentStates[remote].expFloat7+
    		currentStates[remote].expFloat8;
    }

	//get guitar buttons
	public static bool  GetGuitarButton(int remote, string s)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==4)
		{
			switch(s)
			{	
				case "GREEN":
		           	return currentStates[remote].expButton1;    
		        case "RED":
		           	return currentStates[remote].expButton2;
		        case "YELLOW":
		          	return currentStates[remote].expButton3;
		        case "BLUE":
		        	return currentStates[remote].expButton4;
    	        case "ORANGE":
    	        	return currentStates[remote].expButton5;
        	    case "PLUS":
        	    	return currentStates[remote].expButton6;
        	    case "MINUS":
        	    	return currentStates[remote].expButton7;
				default:
					print("Unsupported Button Type " + s);
				return false;
			}
		}
		return false;
	}

	public static bool  GetGuitarButtonDown(int remote, string s)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==4)
		{
			switch(s)
			{	
				case "GREEN":
		           	return currentStates[remote].expButton1;    
		        case "RED":
		           	return currentStates[remote].expButton2;
		        case "YELLOW":
		          	return currentStates[remote].expButton3;
		        case "BLUE":
		        	return currentStates[remote].expButton4;
    	        case "ORANGE":
    	        	return currentStates[remote].expButton5;
        	    case "PLUS":
        	    	return currentStates[remote].expButton6;
        	    case "MINUS":
        	    	return currentStates[remote].expButton7;
				default:
					print("Unsupported Button Type " + s);
				return false;
			}
		}
		return false;
	}

	public static bool  GetGuitarButtonUp(int remote, string s)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==4)
		{
			switch(s)
			{	
				case "GREEN":
		           	return currentStates[remote].expButton1;    
		        case "RED":
		           	return currentStates[remote].expButton2;
		        case "YELLOW":
		          	return currentStates[remote].expButton3;
		        case "BLUE":
		        	return currentStates[remote].expButton4;
    	        case "ORANGE":
    	        	return currentStates[remote].expButton5;
        	    case "PLUS":
        	    	return currentStates[remote].expButton6;
        	    case "MINUS":
        	    	return currentStates[remote].expButton7;
				default:
					print("Unsupported Button Type " + s);
				return false;
			}
		}
		return false;
	}

	//get guitar stick
	public static Vector2 GetGuitarAnalogStick(int remote)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return Vector2.zero;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==4)
		{
			return GetAnalogStick(remote,"GUITAR");
		}
		return Vector2.zero;
	}

	public static int GetGuitarStrum(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return 0;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType!=4)
		{
			return 0;
		}
    	if(currentStates[remote].expButton8)
    	{
    		return 1;
    	}
    	else
    	{
    		if(currentStates[remote].expButton9)
    		{
    			return -1;
    		}
    		return 0;
    	}
    }
    
    public static float GetGuitarWhammy(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return 0.0f;
    	}
		if(!isAwake)
	    	WakeUp();

    	if(currentStates[remote].expType==4)
    	{
    		float whammy = (currentStates[remote].expFloat4-15.0f);

			if(isVirtual[remote])
				return whammy;

            if(whammy<0)
                whammy = 0.0f;
            else
                whammy /=(26.0f-15.0f);
    		return whammy;
    	}
    	else
    		return 0;
    }

	//get drum stick
	public static Vector2 GetDrumAnalogStick(int remote)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return Vector2.zero;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==5)
		{
			return GetAnalogStick(remote,"DRUMS");
		}
		return Vector2.zero;
	}

	//get drum buttons
	public static bool  GetDrumButton(int remote, string s)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==5)
		{
			switch(s)
			{	
				case "GREEN":
					return currentStates[remote].expButton1;
				case "RED":
					return currentStates[remote].expButton2;
				case "YELLOW":
					return currentStates[remote].expButton3;
				case "BLUE":
					return currentStates[remote].expButton4;
				case "ORANGE":
					return currentStates[remote].expButton5;
				case "PLUS":
					return currentStates[remote].expButton6;
				case "MINUS":
					return currentStates[remote].expButton7;
				case "PEDAL":
					return currentStates[remote].expButton8;
				default:
					print("Unsupported Button Type " + s);
				return false;
			}
		}
		return false;
	}

	public static bool  GetDrumButtonDown(int remote, string s)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==5)
		{
			switch(s)
			{	
				case "GREEN":
					return !previousStates[remote].expButton1 && currentStates[remote].expButton1;
				case "RED":
					return !previousStates[remote].expButton2 && currentStates[remote].expButton2;
				case "YELLOW":
					return !previousStates[remote].expButton3 && currentStates[remote].expButton3;
				case "BLUE":
					return !previousStates[remote].expButton4 && currentStates[remote].expButton4;
				case "ORANGE":
					return !previousStates[remote].expButton5 && currentStates[remote].expButton5;
				case "PLUS":
					return !previousStates[remote].expButton6 && currentStates[remote].expButton6;
				case "MINUS":
					return !previousStates[remote].expButton7 && currentStates[remote].expButton7;
				case "PEDAL":
					return !previousStates[remote].expButton8 && currentStates[remote].expButton8;
				default:
					print("Unsupported Button Type " + s);
				return false;
			}
		}
		return false;
	}

	public static bool  GetDrumButtonUp(int remote, string s)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==5)
		{
			switch(s)
			{	
				case "GREEN":
					return previousStates[remote].expButton1 && !currentStates[remote].expButton1;
				case "RED":
					return previousStates[remote].expButton2 && !currentStates[remote].expButton2;
				case "YELLOW":
					return previousStates[remote].expButton3 && !currentStates[remote].expButton3;
				case "BLUE":
					return previousStates[remote].expButton4 && !currentStates[remote].expButton4;
				case "ORANGE":
					return previousStates[remote].expButton5 && !currentStates[remote].expButton5;
				case "PLUS":
					return previousStates[remote].expButton6 && !currentStates[remote].expButton6;
				case "MINUS":
					return previousStates[remote].expButton7 && !currentStates[remote].expButton7;
				case "PEDAL":
					return previousStates[remote].expButton8 && !currentStates[remote].expButton8;
				default:
					print("Unsupported Button Type " + s);
				return false;
			}
		}
		return false;
	}

	public static float GetDrumVelocity(int remote, string drum)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return 0.0f;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType!=5)
		{
			return 0.0f;
		}

		if(isVirtual[remote])
		{
			switch(drum) 
    		{
    			case "RED":
    				return currentStates[remote].expFloat4;
    			case "BLUE":
    				return currentStates[remote].expFloat6;
    			case "GREEN":
    				return currentStates[remote].expFloat3;
    			case "YELLOW":
    				return currentStates[remote].expFloat5;
    			case "ORANGE":
    				return currentStates[remote].expFloat7;
    			case "PEDAL":
    				return currentStates[remote].expFloat8;	
    			default:
    				return 0.0f;
    		}
		}

    	switch(drum) 
    	{
    		case "RED":
    			return (7.0f-currentStates[remote].expFloat4)/7.0f;
    		case "BLUE":
    			return (7.0f-currentStates[remote].expFloat6)/7.0f;
    		case "GREEN":
    			return (7.0f-currentStates[remote].expFloat3)/7.0f;
    		case "YELLOW":
    			return (7.0f-currentStates[remote].expFloat5)/7.0f;
    		case "ORANGE":
    			return (7.0f-currentStates[remote].expFloat7)/7.0f;
    		case "PEDAL":
    			return (7.0f-currentStates[remote].expFloat8)/7.0f;	
    		default:
    			return 0.0f;
    	}
    }

	//get turntable stick
	public static Vector2 GetTurntableAnalogStick(int remote)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return Vector2.zero;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==6)
		{
			return GetAnalogStick(remote,"TURNTABLE");
		}
		return Vector2.zero;
	}

	//get turntable buttons
	public static bool  GetTurntableButton(int remote, string s)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==6)
		{
			switch(s)
			{	
				case "GREENLEFT":
					return currentStates[remote].expButton1;
				case "REDLEFT":
					return currentStates[remote].expButton2;
				case "BLUELEFT":
					return currentStates[remote].expButton3;
				case "GREENRIGHT":
					return currentStates[remote].expButton4;
				case "REDRIGHT":
					return currentStates[remote].expButton5;
				case "BLUERIGHT":
					return currentStates[remote].expButton6;
				case "EUPHORIA":
					return currentStates[remote].expButton7;
				case "PLUS":
					return currentStates[remote].expButton8;
				case "MINUS":
					return currentStates[remote].expButton9;
				default:
					print("Unsupported Button Type " + s);
				return false;
			}
		}
		return false;
	}

	public static bool  GetTurntableButtonDown(int remote, string s)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==6)
		{
			switch(s)
			{	
				case "GREENLEFT":
					return !previousStates[remote].expButton1 && currentStates[remote].expButton1;
				case "REDLEFT":
					return !previousStates[remote].expButton2 && currentStates[remote].expButton2;
				case "BLUELEFT":
					return !previousStates[remote].expButton3 && currentStates[remote].expButton3;
				case "GREENRIGHT":
					return !previousStates[remote].expButton4 && currentStates[remote].expButton4;
				case "REDRIGHT":
					return !previousStates[remote].expButton5 && currentStates[remote].expButton5;
				case "BLUERIGHT":
					return !previousStates[remote].expButton6 && currentStates[remote].expButton6;
				case "EUPHORIA":
					return !previousStates[remote].expButton7 && currentStates[remote].expButton7;
				case "PLUS":
					return !previousStates[remote].expButton8 && currentStates[remote].expButton8;
				case "MINUS":
					return !previousStates[remote].expButton9 && currentStates[remote].expButton9;
				default:
					print("Unsupported Button Type " + s);
				return false;
			}
		}
		return false;
	}

	public static bool  GetTurntableButtonUp(int remote, string s)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return false;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==6)
		{
			switch(s)
			{	
				case "GREENLEFT":
					return previousStates[remote].expButton1 && !currentStates[remote].expButton1;
				case "REDLEFT":
					return previousStates[remote].expButton2 && !currentStates[remote].expButton2;
				case "BLUELEFT":
					return previousStates[remote].expButton3 && !currentStates[remote].expButton3;
				case "GREENRIGHT":
					return previousStates[remote].expButton4 && !currentStates[remote].expButton4;
				case "REDRIGHT":
					return previousStates[remote].expButton5 && !currentStates[remote].expButton5;
				case "BLUERIGHT":
					return previousStates[remote].expButton6 && !currentStates[remote].expButton6;
				case "EUPHORIA":
					return previousStates[remote].expButton7 && !currentStates[remote].expButton7;
				case "PLUS":
					return previousStates[remote].expButton8 && !currentStates[remote].expButton8;
				case "MINUS":
					return previousStates[remote].expButton9 && !currentStates[remote].expButton9;
				default:
					print("Unsupported Button Type " + s);
				return false;
			}
		}
		return false;
	}

	public static float GetTurntableSpin(int remote, string which)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return 0.0f;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==6)
		{
			if(which=="LEFT")
				return currentStates[remote].expFloat5*39.7f*Time.deltaTime;
			if(which=="RIGHT")
				return currentStates[remote].expFloat6 *39.7f*Time.deltaTime;
		}	
		return 0.0f;
	}
	
	public static float GetTurntableSlider(int remote)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return 0.0f;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==6)
		{
			//0 14 30
			float slider = currentStates[remote].expFloat3;
			if(isVirtual[remote])
				return slider;
			
			slider -= 14.0f;
			if(slider<0)
				slider/=14.0f;
			else
				slider/=(30.0f-14.0f);
			return slider;
		}
		return 0.0f;
	}
	
	public static float GetTurntableDial(int remote)
	{
		if(remote<0 || remote>(max-1))
		{
			Debug.Log("invalid remote number");
			return 0.0f;
    	}
		if(!isAwake)
	    	WakeUp();

		if(currentStates[remote].expType==6)
		{
			//0 31
			float dial = currentStates[remote].expFloat4;
			if(isVirtual[remote])
				return dial;

			if(dial<0)
				dial=0;
			else
				dial/=31.0f;
			return dial;
		}
		return 0.0f;
	}
    
    /////////////////////////////////////////////////////////////////////////////
	////////////////BEHIND THE CURTAIN///////////////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////

	private static int max = 16;//CANNOT EXCEED 16!!! (this is a hard limit within the plugin)
	private static bool automaticallyCalibrateMotionPlus = true;
	private static bool automaticallyCheckForMotionPlus = true;

	private static float timeSinceLastCheck = 0.0f;

	private static bool isAwake;
	
    private static bool searching = false;
    private static bool[][] theLEDs;
    private static bool[] rumbling;
	private static float[] IRSensitivity;// (1-100);
	private static Vector3[] virtualIR;
	private static float[] virtualIRRot;
	private static State[] currentStates;
    private static State[] previousStates;
    private static int highestNumberedPlayer = 0;
    private static bool[] wiiRemotesActive;
    private static bool[] isVirtual;
    
    private static Vector2[] pointsDifference;
	private static int[]switchedIR;
    private static float[][] motionPlusCalib; 
    private static int calibrationBuffer = 20;
    private static int calibrationCounterLimit = 50;
    private static int errorCode = 0;

    private const string pluginName = "WiiBuddy";
    
    [DllImport(pluginName)]
    private static extern bool checkPlugin();
    
    [DllImport(pluginName)]
    private static extern void initPlugin(string theAssemblyPath);

	[DllImport(pluginName)]
    private static extern int test();
    
    [DllImport(pluginName)]
    private static extern void setLEDs(int thisRemote, bool LED1, bool LED2, bool LED3, bool LED4);

	[DllImport(pluginName)]
    private static extern bool getLED(int i ,int led);

    [DllImport(pluginName)]
    private static extern void findWiiRemote();
    
    [DllImport(pluginName)]
    private static extern void addVirtual(int i);

    [DllImport(pluginName)]
    private static extern void stopSearch();

    [DllImport(pluginName)]
    private static extern void dropWiiRemote(int i);

	[DllImport(pluginName)]
    private static extern int getDiscoveryStatus();
    
    [DllImport(pluginName)]
    private static extern void clearDiscoveryStatus();

    [DllImport(pluginName)]
    [return : MarshalAs( UnmanagedType.Struct )]
    private static extern State getStates(int i);
    
    [DllImport(pluginName)]
    private static extern void setIRSensitivity(int i,float sensitivity);
    
    [DllImport(pluginName)]
    private static extern float getIRSensitivity(int i);
    
    [DllImport(pluginName)]
    private static extern float getVirtualIR(int i, int r);
    
    [DllImport(pluginName)]
    [return : MarshalAs( UnmanagedType.Struct )]
    private static extern bool getVirtual(int i);
    
    [DllImport(pluginName)]
    [return : MarshalAs( UnmanagedType.Struct )]
    private static extern BalanceGrid getBalanceBoard(int i);
    
    [DllImport(pluginName)]
    private static extern void setForceFeedback(int thisRemote, bool enabled);

	[DllImport(pluginName)]	private static extern void deactivateMotionPlus(int thisRemote);	
	[DllImport(pluginName)]	private static extern void checkForMotionPlus(int thisRemote);
    
    [DllImport(pluginName)]
    private static extern void setMotionPlusCalibration(int thisRemote,float rollZero,float yawZero, float pitchZero);

    [DllImport(pluginName)]
	private static extern void applicationWillTerminate();

    
    
    
   //Data types in C      bytes
   // bool                 1     
   // int                  4     
   // float                4
   // double               8     
   // IRData               6     
 
    [StructLayout(LayoutKind.Explicit)]
    public struct IRData
    {
        [FieldOffset(0)]public short x;
        [FieldOffset(2)]public short y;
        [FieldOffset(4)]public short s;
        
        public IRData(short X, short Y, short S)
    	{
    		x = X;
    		y = Y;
    		s = S;
    	}
    }
    
    [StructLayout(LayoutKind.Explicit)]
    public struct BalanceGrid
    {
    	[FieldOffset(0)]public float tr;
    	[FieldOffset(4)]public float br;
    	[FieldOffset(8)]public float tl;
    	[FieldOffset(12)]public float bl;
    
    	public BalanceGrid(float TR, float BR, float TL, float BL)
    	{
    		tr = TR;
    		br = BR;
    		tl = TL;
    		bl = BL;
    	}	
    }

	[StructLayout(LayoutKind.Explicit)]
   	public struct State
    {
    	[FieldOffset(0)]public bool active;
		[FieldOffset(1)]public bool a;
		[FieldOffset(2)]public bool b;
		[FieldOffset(3)]public bool up;
		[FieldOffset(4)]public bool down;
		[FieldOffset(5)]public bool left;
		[FieldOffset(6)]public bool right;
		[FieldOffset(7)]public bool one;
		[FieldOffset(8)]public bool two;
		[FieldOffset(9)]public bool plus;
		[FieldOffset(10)]public bool minus;
		[FieldOffset(11)]public bool home;
    	[FieldOffset(12)]public bool expButton1;
    	[FieldOffset(13)]public bool expButton2;
    	[FieldOffset(14)]public bool expButton3;
    	[FieldOffset(15)]public bool expButton4;
    	[FieldOffset(16)]public bool expButton5;
    	[FieldOffset(17)]public bool expButton6;
    	[FieldOffset(18)]public bool expButton7;
    	[FieldOffset(19)]public bool expButton8;
    	[FieldOffset(20)]public bool expButton9;
    	[FieldOffset(21)]public bool expButton10;
    	[FieldOffset(22)]public bool expButton11;
    	[FieldOffset(23)]public bool expButton12;
    	[FieldOffset(24)]public bool expButton13;
		[FieldOffset(25)]public bool expButton14;
		[FieldOffset(26)]public bool expButton15;
    	[FieldOffset(27)]public bool yawFast;
    	[FieldOffset(28)]public bool rollFast;
    	[FieldOffset(29)]public bool pitchFast;

    	[FieldOffset(30)]public bool motionPlusAvailable;
    	[FieldOffset(31)]public bool motionPlusCalibrated;
		
		[FieldOffset(32)]public char expType;
		[FieldOffset(36)]public float battery;
		
		[FieldOffset(40)]public float wiiAccelX;
		[FieldOffset(44)]public float wiiAccelY;
		[FieldOffset(48)]public float wiiAccelZ;
		
    	[FieldOffset(52)]public float expFloat1;
    	[FieldOffset(56)]public float expFloat2;
    	[FieldOffset(60)]public float expFloat3;
    	[FieldOffset(64)]public float expFloat4;
    	[FieldOffset(68)]public float expFloat5;
    	[FieldOffset(72)]public float expFloat6;
    	[FieldOffset(76)]public float expFloat7;
    	[FieldOffset(80)]public float expFloat8;
    	
    	[FieldOffset(84)]public float yaw;
    	[FieldOffset(88)]public float roll;
    	[FieldOffset(92)]public float pitch;
    	
    	[MarshalAs(UnmanagedType.Struct)]
    	[FieldOffset(96)]public IRData rawIR1;
		[MarshalAs(UnmanagedType.Struct)]
		[FieldOffset(102)]public IRData rawIR2;
		[MarshalAs(UnmanagedType.Struct)]
		[FieldOffset(108)]public IRData rawIR3;
		[MarshalAs(UnmanagedType.Struct)]
		[FieldOffset(114)]public IRData rawIR4;
    }
        
    void Awake()
    {
		//gameObject.GetComponent<Transform>().hideFlags = HideFlags.HideInInspector;
    	if(!isAwake)
	    	WakeUp();	
    }
    
    public void OnDisable(){
		isAwake=false;
	}

	public static bool GetIsAwake()
	{
		return isAwake;
	}
        
    public static void WakeUp()
    {
    	currentStates    = new State[max];
    	previousStates   = new State[max];
    	wiiRemotesActive = new  bool[max];
    	isVirtual        = new bool[max];
    	motionPlusCalib  = new float[max][];
    	theLEDs          = new bool[max][];
    	rumbling         = new bool[max];
    	pointsDifference = new Vector2[max];
    	switchedIR       = new int[max];
    	IRSensitivity    = new float[max];
    	virtualIR        = new Vector3[max];
    	virtualIRRot     = new float[max];
    	
    	if(!checkPlugin())
    	{
	    	initPlugin("doesn't matter");
    	}
    	
    	int newHighest = 0;
    	for(int x=0;x<max;x++)
    	{
    		currentStates[x] = getStates(x);
    		isVirtual[x]     = getVirtual(x); 
    		
    		if(currentStates[x].active || isVirtual[x])
    		{
    			wiiRemotesActive[x] = true;
    			if(x>=newHighest)
    				newHighest=(x+1);	
    			
    			virtualIR[x]    = new Vector3(getVirtualIR(x,0),getVirtualIR(x,1),getVirtualIR(x,2));
    			virtualIRRot[x] = getVirtualIR(x,3);
    		}
    		highestNumberedPlayer = newHighest;
    		IRSensitivity[x]   = getIRSensitivity(x);
    		motionPlusCalib[x] = new float[4];
    		theLEDs[x]         = new bool[4];
    		theLEDs[x][0] = getLED(x,0);
    		theLEDs[x][1] = getLED(x,1);
    		theLEDs[x][2] = getLED(x,2);
    		theLEDs[x][3] = getLED(x,3);
    		
    	}
    	//Debug.Log("the struct should be "+test());
    	isAwake = true;
    }

	public static int GetMaximumRemotes()
	{
		return max;
	}
	
	public static void setActive(int remote, bool b)//only meant for use by inspector
    {
    	wiiRemotesActive[remote] = b;
    }

	public static void AddVirtualRemote()
    {
    	int i = 0;
    	while(wiiRemotesActive[i]==true && i<(max-1))
    	{
    		i++;
    	}
    	wiiRemotesActive[i] = true;
        isVirtual[i] = true;
        currentStates[i].active = true;
       	addVirtual(i);
        
        if((i+1)>highestNumberedPlayer)//probably unnecessary
    	{
    		highestNumberedPlayer=(i+1);
    	}	
    }
    
    public static bool GetIsVirtual(int i)
    {
    	return isVirtual[i];
    }
    
    public static void SetVirtualState(int i,State state)
    {
    	previousStates[i]=currentStates[i];
    	currentStates[i]=state;
    }
    public static void SetVirtualIR(int i,Vector3 state)
    {
   		virtualIR[i] = state; 
    }
    
    public static void SetVirtualIRRot(int i,float state)
    {
   		virtualIRRot[i] = state; 
    }
    
    public static State[] GetAllCurrentStates()
    {
    	return currentStates;
    }
    
	public static float[] GetAllIRSensitivity()
	{
		return IRSensitivity;	
	}

     public static bool[][] GetAllLEDs()
    {
    	return theLEDs;
    }
    
    public static bool[] GetAllRumblings()
    {
    	return rumbling;
    }
	
	void OnApplicationQuit()
	{
		if(Application.isEditor==false)
			applicationWillTerminate();
	}

	float totalWiggles = 0.0f;
    void FixedUpdate()
    {	
		if(!isAwake)
    		WakeUp();    	
    		
    	if(searching || Application.isEditor)//(because the extension could also cause new ones to be discovered at any time.)
    	{
    		int status = getDiscoveryStatus();
    		
    		if(status<0)
    		{
    			searching=false;
    			if(errorCode==status)//I ALREADY HEARD
    			{
    				errorCode=0;
    				clearDiscoveryStatus();
    			}
    			else
    			{
    				//otherwise
    				errorCode = status;
    				Debug.Log("spread the word: error"+status);
    				gameObject.BroadcastMessage("OnDiscoveryError", status,SendMessageOptions.DontRequireReceiver);
    				//don't clear status yet. the editor expansion might need to know
    			}
    		}
    		if(status==0)
    		{
    			searching=false;
    			//Debug.Log("NOT SEARCHING");
    		}
    		if(status==1)
    		{
    			//Debug.Log(" SEARCHING");
       		}
    		if(status==2)
    		{
    			//Debug.Log("STARTING CONNECTION");
    		}
    		if(status>99)
    		{
    			int i =  status-100;
    			Debug.Log("remote found"+i);
    			if(wiiRemotesActive[i]==true)//ALREADY FOUND IT
    			{
    				//Debug.Log("clear discovery status found");
    				setLEDs(i,theLEDs[i][0],theLEDs[i][1],theLEDs[i][2],theLEDs[i][3]);
    				clearDiscoveryStatus();
    			}
    			else
    			{
					//otherwise 
    				//Debug.Log("spread the word: found it"+ i);
    				wiiRemotesActive[i] = true;
    				setLEDs(i,theLEDs[i][0],theLEDs[i][1],theLEDs[i][2],theLEDs[i][3]);
    				searching = false;
			
    				if((i+1)>highestNumberedPlayer)
    				{
    					highestNumberedPlayer=(i+1);
    				}	
       				gameObject.BroadcastMessage("OnWiimoteFound",i,SendMessageOptions.DontRequireReceiver);
    				//don't clear status yet. the editor expansion might need to know
    			}
    		}
    	}
    	
    	for(int i=0;i<highestNumberedPlayer;i++)
    	{
    		if(wiiRemotesActive[i] && !isVirtual[i])
    		{
       			previousStates[i] = currentStates[i];
	  	 		currentStates[i] = getStates(i);
	  	 		
	  	 		if(isVirtual[i])
	  	 			return;
	  	 		
	  	 		if(currentStates[i].active == false && previousStates[i].active == true)
    			{
    				Debug.Log("REMOTE: "+i+" DROPPED");
    				wiiRemotesActive[i]=false;
    				if((i+1)==highestNumberedPlayer)
    					highestNumberedPlayer--;
 				   	//Debug.Log("spread the word: disconnected: "+i);
 				   	
    				gameObject.BroadcastMessage("OnWiimoteDropped",i,SendMessageOptions.DontRequireReceiver);
					return;	
    			}
	  	 			
				if(automaticallyCheckForMotionPlus)
				{
					if(i==(highestNumberedPlayer-1))
					{	
						timeSinceLastCheck += Time.deltaTime;
					}
					if(timeSinceLastCheck>5)
					{
						if(i==(highestNumberedPlayer-1))
						{	
							timeSinceLastCheck=0.0f;
						}
						if(!currentStates[i].motionPlusAvailable)// && Input.GetKey("k"))
						{
							checkForMotionPlus(i);
						}
					}
				}
	  	 		if(automaticallyCalibrateMotionPlus && currentStates[i].motionPlusAvailable && !currentStates[i].motionPlusCalibrated)//motionplus needs calibratin'
	  	 		{
	  	 			float wiggles = Mathf.Abs(currentStates[i].wiiAccelX-previousStates[i].wiiAccelX)+
	  	 			Mathf.Abs(currentStates[i].wiiAccelY-previousStates[i].wiiAccelY)+
	  	 			Mathf.Abs(currentStates[i].wiiAccelZ-previousStates[i].wiiAccelZ);
	  	 			//Debug.Log("TURN IT UPSIDE DOWN!!!! "+wiggles);
	  	 			//if stillness and upside down
	  	 			if(currentStates[i].wiiAccelZ<-.8 && wiggles<.1f)
	  	 			{
	  	 				//Debug.Log("HOLD STILL!!!! "+wiggles);
	  	 				motionPlusCalib[i][0] += 1; //add counter
	  	 				
	  	 				if(motionPlusCalib[i][0]>calibrationBuffer)//let it settle before starting
	  	 				{
		  	 				totalWiggles+=wiggles;
		  	 				motionPlusCalib[i][1] += (currentStates[i].roll /(float)calibrationCounterLimit);//rollzero
		  	 				motionPlusCalib[i][2] += (currentStates[i].yaw  /(float)calibrationCounterLimit);//yawZero
	  		 				motionPlusCalib[i][3] += (currentStates[i].pitch/(float)calibrationCounterLimit);//pitchZero
	  	 				}
	  	 				//sample motion plus to calibration information
	  	 				if(motionPlusCalib[i][0] >= calibrationCounterLimit+calibrationBuffer)//taken enough samples
	  	 				{
	  	 					//set calibration informtion
							Debug.Log("motion plus calibrated");
	  	 					//Debug.Log(motionPlusCalib[i][1]+" "+motionPlusCalib[i][2]+" "+motionPlusCalib[i][3]);
	  	 					setMotionPlusCalibration(i,motionPlusCalib[i][1],motionPlusCalib[i][2],motionPlusCalib[i][3]);
	  	 					//announce that motion plus is now calibrated
	  	 				}
	  	 			}
	  	 			else
	  	 			{
	  	 				//calibration incomplete
	  	 				totalWiggles = 0;
	  	 				motionPlusCalib[i][0]=0;
	  	 				motionPlusCalib[i][1]=0.0f;
	  	 				motionPlusCalib[i][2]=0.0f;
	  	 				motionPlusCalib[i][3]=0.0f;
	  	 				//reset calibration information
	  	 			}
    			}
	   		}
    	}
    }
}