using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

[CustomEditor(typeof(Wii))]
[System.Serializable]

public class WiiBuddy : Editor{

	string feedback = "Ready";
	private static bool amAwake;
	bool groupEnabled;
	GUILayoutOption[] mpOptions;
	  	
  	public Wii wii;
  	private static int wiiMoteCount;
    private static int discoveryStatus = 0;
	private static int max;
    private static bool[][] theLEDs;
    private static bool[] rumblings;
    public Wii.State[] currentStates;
    private static Vector3[] theIRs;
    private static float[]   theIRRot;
    private static bool[] showPanel;
    private static bool[] showMPPanel;
    private static bool[] showExpPanel;
    
    private static bool rapidDisplay;

	public void Awake (){
		WakeUp();
	}
	
	public void OnDisable(){
		amAwake = false;
	}
	
	private void WakeUp()
	{
		if(amAwake)//already awake.
			return;
		if(!checkPlugin())
    	{
    	   	initPlugin("doesn't matter");
    	}
    	
		wii              = target as Wii;	
		Wii.WakeUp();
		max = Wii.GetMaximumRemotes();
		showPanel        = new bool[max];
		showMPPanel      = new bool[max];
		showExpPanel     = new bool[max];
		currentStates    = new Wii.State[max];
		theIRs           = new Vector3[max];
		theIRRot         = new float[max];
		theLEDs          = new bool[max][];
		
		for(int x=0;x<max;x++)
		{
			theLEDs[x] = new bool[4];
			currentStates[x] = getStates(x);
			if(currentStates[x].active || Wii.GetIsVirtual(x))
			{
				theIRs[x].x     = getVirtualIR(x,0);
				theIRs[x].y     = getVirtualIR(x,1);
				theIRs[x].z     = getVirtualIR(x,2);
				theIRRot[x]     = getVirtualIR(x,3);
				
				showPanel[x]     = getPanel(x,0);
				showMPPanel[x]   = getPanel(x,1);
				showExpPanel[x]  = getPanel(x,2);
			}
		}
		amAwake=true;
	}
	
	public override void OnInspectorGUI (){
		if(!Wii.GetIsAwake())
		{
			Wii.WakeUp(); 
		}
		if(!amAwake)
		{
			WakeUp();
		}
		//gather variables from the scene
		discoveryStatus = getDiscoveryStatus();	 
		theLEDs       = Wii.GetAllLEDs();
    	rumblings     = Wii.GetAllRumblings();
		
			//Find me more Motes
			EditorGUILayout.BeginHorizontal();
			if (discoveryStatus==1) {
				feedback = "searching";
				Repaint();
	
				if (GUILayout.Button ("Stop", GUILayout.MaxWidth (40))) {
					feedback = "cancelled";
					Wii.StopSearch ();
				}
			}
			else
			{
				if (GUILayout.Button ("Find", GUILayout.MaxWidth (40)))
					Wii.StartSearch ();
					
				if(discoveryStatus>99)
				{
					int i =  discoveryStatus-100;
					feedback = "found #"+(i);
	   				//Debug.Log("I think I've found"+i);
	   				if(Wii.IsActive(i)==true)//ALREADY FOUND IT
	   				{
	   					//Debug.Log("clear discovery status found");
	   					setLEDs(i,theLEDs[i][0],theLEDs[i][1],theLEDs[i][2],theLEDs[i][3]);//do it again, just in case
	   					clearDiscoveryStatus();//happens in LateUpdate so extension has time to notice.
	   				}
	   				else
	   				{
						//otherwise 
	   					//Debug.Log("spread the word: found it"+ i);
	   					Wii.setActive(i,true);
	   					setLEDs(i,theLEDs[i][0],theLEDs[i][1],theLEDs[i][2],theLEDs[i][3]);
	    	  				//don't clear status yet. Wii.cs might still need to know
	   				}
				}
				else if(discoveryStatus<0)
				{
					feedback = "error:"+discoveryStatus+" try again";
				}
			}
			
			GUILayout.Label (feedback, GUILayout.MaxWidth (200));		
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			if(GUILayout.Button("Add Virtual",GUILayout.MaxWidth(80)))
				Wii.AddVirtualRemote();
			EditorGUILayout.EndHorizontal ();
			rapidDisplay = GUILayout.Toggle (rapidDisplay,"rapid update", GUILayout.MaxWidth (100));

		
		wiiMoteCount=0;
		for(int i=0;i<max;i++)
		{	
			bool isVirtual = Wii.GetIsVirtual(i);
			if(!isVirtual)
				currentStates[i] = getStates(i);

			Wii.State oldState = currentStates[i];
			
			if(currentStates[i].active || isVirtual)
			{
				wiiMoteCount++;
				//Begin WiiMote Info
				EditorGUILayout.BeginHorizontal ();
					
					bool temp = GUILayout.Toggle (theLEDs[i][0], "", GUILayout.MaxWidth (10));
					if(temp != theLEDs[i][0])
					{
						Wii.SetLEDs(i,temp,theLEDs[i][1],theLEDs[i][2],theLEDs[i][3]);
					}
						
					temp = GUILayout.Toggle (theLEDs[i][1], "", GUILayout.MaxWidth (10));
					if(temp != theLEDs[i][1])
					{
						Wii.SetLEDs(i,theLEDs[i][0],temp,theLEDs[i][2],theLEDs[i][3]);
					}
						
					temp = GUILayout.Toggle (theLEDs[i][2], "", GUILayout.MaxWidth (10));
					if(temp != theLEDs[i][2])
					{
						Wii.SetLEDs(i,theLEDs[i][0],theLEDs[i][1],temp,theLEDs[i][3]);
					}
						
					temp = GUILayout.Toggle (theLEDs[i][3], "", GUILayout.MaxWidth (10));
					if(temp != theLEDs[i][3])
					{
						Wii.SetLEDs(i,theLEDs[i][0],theLEDs[i][1],theLEDs[i][2],temp);
					}
					
					bool panel;
					if(isVirtual)
						panel  = EditorGUILayout.Foldout (showPanel[i], "virtual "+i); 
					else
						panel  = EditorGUILayout.Foldout (showPanel[i], "player "+i); 
					if(panel != showPanel[i])
					{
						Undo.RegisterUndo(this,"panel");
						showPanel[i]=panel;
						setPanel(i,0,panel);
					}
					if(rumblings[i])
					{
						if (GUILayout.Button ("Be Still", GUILayout.MaxWidth (60))) {
							Wii.SetRumble(i,false);
						}
					}
					else
					{
						if (GUILayout.Button ("Rumble", GUILayout.MaxWidth (60))) {
							Wii.SetRumble(i,true);
						}
					}
					if (GUILayout.Button ("Drop", GUILayout.MaxWidth (60))) {
						Wii.DropWiiRemote (i);
						showPanel[i] =false;
						showMPPanel[i] =false;
						showExpPanel[i] =false;	
					}
				EditorGUILayout.EndHorizontal ();
				
				if(currentStates[i].expType == 3)//balance board
				{
					if(showPanel[i])
					{
						if(rapidDisplay && discoveryStatus<100)//don't repaint if wii.cs needs to be notified of a discovery
							Repaint();

						EditorGUILayout.BeginHorizontal();	
						
						Vector4 balance = new Vector4(currentStates[i].expFloat5,currentStates[i].expFloat6,
														currentStates[i].expFloat7,currentStates[i].expFloat8);
						Vector4 newBalance = EditorGUILayout.Vector4Field("Balance",balance,GUILayout.MaxWidth(300));
						if(isVirtual && newBalance != balance)
						{
							currentStates[i].expFloat5 = Mathf.Max(0,newBalance.x);
							currentStates[i].expFloat6 = Mathf.Max(0,newBalance.y);
							currentStates[i].expFloat7 = Mathf.Max(0,newBalance.z);
							currentStates[i].expFloat8 = Mathf.Max(0,newBalance.w);
						}
						
						EditorGUILayout.EndHorizontal();
						if(isVirtual)
						{
							string[] options = new string[7];
							options[0]="none";
							options[1]="nunchuk";
							options[2]="classic";
							options[3]="balance board";
							options[4]="guitar";
							options[5]="drums";
							options[6]="turn table";
							char newExp = (char)EditorGUILayout.Popup("expansion",currentStates[i].expType,options);
							if(newExp != currentStates[i].expType)
							{
								currentStates[i].expType = newExp;
								currentStates[i].expFloat1 = 0.0f;
								currentStates[i].expFloat2 = 0.0f;
								if(newExp != 5)
								{
									currentStates[i].expFloat3 = 0.0f;
									currentStates[i].expFloat4 = 0.0f;
									currentStates[i].expFloat5 = 0.0f;
									currentStates[i].expFloat6 = 0.0f;
									currentStates[i].expFloat7 = 0.0f;
									currentStates[i].expFloat8 = 0.0f;
								}
								else
								{
									currentStates[i].expFloat3 = 1.0f;
									currentStates[i].expFloat4 = 1.0f;
									currentStates[i].expFloat5 = 1.0f;
									currentStates[i].expFloat6 = 1.0f;
									currentStates[i].expFloat7 = 1.0f;
									currentStates[i].expFloat8 = 1.0f;
								}	
								currentStates[i].expButton1 = false;
								currentStates[i].expButton2 = false;
								currentStates[i].expButton3 = false;
								currentStates[i].expButton4 = false;
								currentStates[i].expButton5 = false;
								currentStates[i].expButton6 = false;
								currentStates[i].expButton7 = false;
								currentStates[i].expButton8 = false;
								currentStates[i].expButton9 = false;
								currentStates[i].expButton10 = false;
								currentStates[i].expButton11 = false;
								currentStates[i].expButton12 = false;
								currentStates[i].expButton13 = false;
							}
						}
					}
				}
				else//wiimote
				{
					if (showPanel[i])
					{	
						if(rapidDisplay)
							Repaint();
												
						currentStates[i].battery = EditorGUILayout.FloatField("Battery ",currentStates[i].battery,GUILayout.MaxWidth(150));
						currentStates[i].battery = Mathf.Clamp(currentStates[i].battery,0,1);

						//WiiMote Basic Data Layout
						GUILayout.Label("Buttons",EditorStyles.boldLabel,GUILayout.MaxWidth(55));
						EditorGUILayout.BeginHorizontal();	
							currentStates[i].up = GUILayout.Toggle(currentStates[i].up,"Up",GUILayout.MaxWidth(35));
							currentStates[i].down = GUILayout.Toggle(currentStates[i].down,"Down",GUILayout.MaxWidth(55));
							currentStates[i].left = GUILayout.Toggle(currentStates[i].left,"Left",GUILayout.MaxWidth(45));
							currentStates[i].right = GUILayout.Toggle(currentStates[i].right,"Right",GUILayout.MaxWidth(55));
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.BeginHorizontal();
							currentStates[i].a     = GUILayout.Toggle(currentStates[i].a,"A",GUILayout.MaxWidth(25));
							currentStates[i].b     = GUILayout.Toggle(currentStates[i].b,"B",GUILayout.MaxWidth(25));
							currentStates[i].plus  = GUILayout.Toggle(currentStates[i].plus,"+",GUILayout.MaxWidth(25));
							currentStates[i].minus = GUILayout.Toggle(currentStates[i].minus,"-",GUILayout.MaxWidth(25));
							currentStates[i].one   = GUILayout.Toggle(currentStates[i].one,"1",GUILayout.MaxWidth(25));
							currentStates[i].two   = GUILayout.Toggle(currentStates[i].two,"2",GUILayout.MaxWidth(25));
							currentStates[i].home  = GUILayout.Toggle(currentStates[i].home,"H",GUILayout.MaxWidth(25));
						EditorGUILayout.EndHorizontal();
						Vector3 accel = new Vector3(currentStates[i].wiiAccelX,currentStates[i].wiiAccelY,currentStates[i].wiiAccelZ);
						accel = EditorGUILayout.Vector3Field("Accel",accel,GUILayout.MaxWidth(300));

						if(accel != new Vector3(currentStates[i].wiiAccelX,currentStates[i].wiiAccelY,currentStates[i].wiiAccelZ))
						{
							currentStates[i].wiiAccelX = accel.x;
							currentStates[i].wiiAccelY = accel.y;
							currentStates[i].wiiAccelZ = accel.z;
						}
						
						Wii.SetIRSensitivity(i,EditorGUILayout.FloatField("IR Sensitivity",Wii.GetIRSensitivity(i),GUILayout.MaxWidth(150)));
						
						if(!isVirtual)
						{
							 theIRs[i] =Wii.GetIRVector3(i);
						}
						Vector3 thisIR = EditorGUILayout.Vector3Field("IR",theIRs[i],GUILayout.MaxWidth(300));
						if(isVirtual && thisIR !=theIRs[i])
						{	
							thisIR.x = Mathf.Clamp(thisIR.x,0,1);
							thisIR.y = Mathf.Clamp(thisIR.y,0,1);
							
							theIRs[i]=thisIR;
							Wii.SetVirtualIR(i,theIRs[i]);
							setVirtualIR(i,0,theIRs[i].x);
							setVirtualIR(i,1,theIRs[i].y);
							setVirtualIR(i,2,theIRs[i].z);
						}
						
						//Motion Plus Data Layout
						if(!currentStates[i].motionPlusAvailable && !isVirtual && GUILayout.Button ("Check For Motion Plus", GUILayout.MaxWidth (140)))
						{
							Wii.CheckForMotionPlus(i);
						}

						if((currentStates[i].motionPlusAvailable || isVirtual) && (currentStates[i].expType==0 || currentStates[i].expType==1 || currentStates[i].expType==2)){
							panel = EditorGUILayout.Foldout (showMPPanel[i], "Motion +");
							if(panel != showMPPanel[i])
							{
								showMPPanel[i]=panel;
								setPanel(i,1,panel);
								if(isVirtual)
								{
									if(panel)//motionplus available
									{
										currentStates[i].motionPlusAvailable = true;
									}
									else
									{
										currentStates[i].motionPlusAvailable  = false;
										currentStates[i].motionPlusCalibrated = false;										
									}
								}
							}
							if(showMPPanel[i]){
								if(currentStates[i].motionPlusCalibrated)
								{
									if (GUILayout.Button ("Uncalibrate", GUILayout.MaxWidth (80))) {
										if(isVirtual)
										{
											currentStates[i].motionPlusCalibrated = false;
										}
										else
										{
											Wii.UncalibrateMotionPlus(i);
											currentStates[i] = getStates(i);
										}
									}

									Vector3 motionPlusData = new Vector3(currentStates[i].roll,currentStates[i].yaw,currentStates[i].pitch);
									Vector3 newPlus = EditorGUILayout.Vector3Field("Motion Plus Data",motionPlusData,GUILayout.MaxWidth(300));
									if(motionPlusData != newPlus)
									{
										currentStates[i].roll = newPlus.x;
										currentStates[i].yaw  = newPlus.y;
										currentStates[i].pitch=	newPlus.z;
									}
									EditorGUILayout.BeginHorizontal();
										mpOptions = new GUILayoutOption[2];
										mpOptions[0] = GUILayout.MinWidth(55);
										mpOptions[1] = GUILayout.MaxWidth(55);
										currentStates[i].rollFast = GUILayout.Toggle(currentStates[i].rollFast,"Fast X",mpOptions);
										GUILayout.Space(40);
										currentStates[i].yawFast = GUILayout.Toggle(currentStates[i].yawFast,"Fast Y",mpOptions);
										GUILayout.Space(40);
										currentStates[i].pitchFast = GUILayout.Toggle(currentStates[i].pitchFast,"Fast Z",mpOptions);
									EditorGUILayout.EndHorizontal();
								}
								else
								{
									if (GUILayout.Button ("Calibrate", GUILayout.MaxWidth (60))) {
										if(isVirtual)
										{
											currentStates[i].motionPlusCalibrated = true;
										}
										else
										{
											currentStates[i] = getStates(i);
											Vector3 calibrations = new Vector3(currentStates[i].roll,currentStates[i].yaw,currentStates[i].pitch);
											Wii.CalibrateMotionPlus(i,calibrations);
										}
									}
									GUILayout.Label ("Motion Plus must first be calibrated.", GUILayout.MaxWidth (300));
									Vector3 motionPlusData = new Vector3(currentStates[i].roll,currentStates[i].yaw,currentStates[i].pitch);
									Vector3 newPlus = EditorGUILayout.Vector3Field("Uncalibrated Motion Plus Data",motionPlusData,GUILayout.MaxWidth(300));
									if(motionPlusData != newPlus)
									{
										currentStates[i].roll = newPlus.x;
										currentStates[i].yaw  = newPlus.y;
										currentStates[i].pitch=	newPlus.z;
									}
									EditorGUILayout.BeginHorizontal();
										mpOptions = new GUILayoutOption[2];
										mpOptions[0] = GUILayout.MinWidth(55);
										mpOptions[1] = GUILayout.MaxWidth(55);
										currentStates[i].rollFast = GUILayout.Toggle(currentStates[i].rollFast,"Fast X",mpOptions);
										GUILayout.Space(40);
										currentStates[i].yawFast = GUILayout.Toggle(currentStates[i].yawFast,"Fast Y",mpOptions);
										GUILayout.Space(40);
										currentStates[i].pitchFast = GUILayout.Toggle(currentStates[i].pitchFast,"Fast Z",mpOptions);
									EditorGUILayout.EndHorizontal();
								}
							}
						}
							
						if(isVirtual)
						{
							string[] options = new string[7];
							options[0]="none";
							options[1]="nunchuk";
							options[2]="classic";
							options[3]="balance board";
							options[4]="guitar";
							options[5]="drums";
							options[6]="turn table";
							char newExp = (char)EditorGUILayout.Popup("expansion",currentStates[i].expType,options);
							if(newExp != currentStates[i].expType)
							{
								currentStates[i].expType = newExp;
								currentStates[i].expFloat1 = 0.0f;
								currentStates[i].expFloat2 = 0.0f;
								if(newExp != 5)
								{
									currentStates[i].expFloat3 = 0.0f;
									currentStates[i].expFloat4 = 0.0f;
									currentStates[i].expFloat5 = 0.0f;
									currentStates[i].expFloat6 = 0.0f;
									currentStates[i].expFloat7 = 0.0f;
									currentStates[i].expFloat8 = 0.0f;
								}
								else
								{
									currentStates[i].expFloat3 = 1.0f;
									currentStates[i].expFloat4 = 1.0f;
									currentStates[i].expFloat5 = 1.0f;
									currentStates[i].expFloat6 = 1.0f;
									currentStates[i].expFloat7 = 1.0f;
									currentStates[i].expFloat8 = 1.0f;
								}
								
								currentStates[i].expButton1 = false;
								currentStates[i].expButton2 = false;
								currentStates[i].expButton3 = false;
								currentStates[i].expButton4 = false;
								currentStates[i].expButton5 = false;
								currentStates[i].expButton6 = false;
								currentStates[i].expButton7 = false;
								currentStates[i].expButton8 = false;
								currentStates[i].expButton9 = false;
								currentStates[i].expButton10 = false;
								currentStates[i].expButton11 = false;
								currentStates[i].expButton12 = false;
								currentStates[i].expButton13 = false;
							}						}
						
						//NunChuk Data Layout
						if(currentStates[i].expType==1){
							panel = EditorGUILayout.Foldout (showExpPanel[i], "NunChuk");
							if(panel != showExpPanel[i])
							{
								showExpPanel[i]=panel;
								setPanel(i,2,panel);
							}
							if(showExpPanel[i]){
								Vector3 nunchukAccel = new Vector3(currentStates[i].expFloat3,currentStates[i].expFloat4,currentStates[i].expFloat5);
								Vector3 newAccel = EditorGUILayout.Vector3Field("NunChuk Accel",nunchukAccel,GUILayout.MaxWidth(300));
								if(isVirtual && newAccel != nunchukAccel)
								{
									currentStates[i].expFloat3 = newAccel.x; 
									currentStates[i].expFloat4 = newAccel.y;
									currentStates[i].expFloat5 = newAccel.z;
								}
								
								Vector2 nunchukStick = new Vector2(currentStates[i].expFloat1,currentStates[i].expFloat2);
								Vector2 newStick = EditorGUILayout.Vector2Field("NunChuk Stick",nunchukStick,GUILayout.MaxWidth(300));
								if(newStick != nunchukStick)
								{
									newStick.x = Mathf.Clamp(newStick.x,-1,1);
									newStick.y = Mathf.Clamp(newStick.y,-1,1);
									currentStates[i].expFloat1 = newStick.x; 
									currentStates[i].expFloat2 = newStick.y;
								}
								
								EditorGUILayout.BeginHorizontal();
								GUILayout.Space(50);
								currentStates[i].expButton1 = GUILayout.Toggle(currentStates[i].expButton1,"C",GUILayout.MaxWidth(25));
								currentStates[i].expButton2 = GUILayout.Toggle(currentStates[i].expButton2,"Z",GUILayout.MaxWidth(25));
								EditorGUILayout.EndHorizontal();
							}
						}
					
						//Classic Data Layout
						if(currentStates[i].expType==2)
						{
							panel = EditorGUILayout.Foldout(showExpPanel[i], "Classic");
							if(panel != showExpPanel[i])
							{
								showExpPanel[i]=panel;
								setPanel(i,2,panel);
							}
							if(showExpPanel[i])
							{
								Vector2 classicStickL = new Vector2(currentStates[i].expFloat1,currentStates[i].expFloat2);
								Vector2 classicStickR = new Vector2(currentStates[i].expFloat3,currentStates[i].expFloat4);

								Vector2 newStick = EditorGUILayout.Vector2Field("Classic Stick Left",classicStickL,GUILayout.MaxWidth(300));
								if(isVirtual && newStick != classicStickL)
								{
									newStick.x = Mathf.Clamp(newStick.x,-1,1);
									newStick.y = Mathf.Clamp(newStick.y,-1,1);
									currentStates[i].expFloat1 = newStick.x; 
									currentStates[i].expFloat2 = newStick.y;
								}
								
								newStick = EditorGUILayout.Vector2Field("Classic Stick Right",classicStickR,GUILayout.MaxWidth(300));
								if(isVirtual && newStick != classicStickR)
								{
									newStick.x = Mathf.Clamp(newStick.x,-1,1);
									newStick.y = Mathf.Clamp(newStick.y,-1,1);
									currentStates[i].expFloat3 = newStick.x; 
									currentStates[i].expFloat4 = newStick.y;
								}	
									
								EditorGUILayout.BeginHorizontal();
									currentStates[i].expButton8 = GUILayout.Toggle(currentStates[i].expButton8,
										"Up",GUILayout.MaxWidth(32));
									currentStates[i].expButton6 = GUILayout.Toggle(currentStates[i].expButton6,
										"Down",GUILayout.MaxWidth(50));
									currentStates[i].expButton5 = GUILayout.Toggle(currentStates[i].expButton5,
										"Left",GUILayout.MaxWidth(40));			
									currentStates[i].expButton7 = GUILayout.Toggle(currentStates[i].expButton7,
										"Right",GUILayout.MaxWidth(50));	
									currentStates[i].expButton7 = GUILayout.Toggle(currentStates[i].expButton11,
										"Home",GUILayout.MaxWidth(50));	
								EditorGUILayout.EndHorizontal();
							
								EditorGUILayout.BeginHorizontal();
									currentStates[i].expButton1 = GUILayout.Toggle(currentStates[i].expButton1,
										"A",GUILayout.MaxWidth(25));
									currentStates[i].expButton2 = GUILayout.Toggle(currentStates[i].expButton2,
										"B",GUILayout.MaxWidth(25));
									currentStates[i].expButton9 = GUILayout.Toggle(currentStates[i].expButton9,
										"-",GUILayout.MaxWidth(25));
									currentStates[i].expButton10 = GUILayout.Toggle(currentStates[i].expButton10,
										"+",GUILayout.MaxWidth(25));
									currentStates[i].expButton3 = GUILayout.Toggle(currentStates[i].expButton3,
										"X",GUILayout.MaxWidth(25));
									currentStates[i].expButton4 = GUILayout.Toggle(currentStates[i].expButton4,
										"Y",GUILayout.MaxWidth(25));
									currentStates[i].expButton12 = GUILayout.Toggle(currentStates[i].expButton12,
										"ZL",GUILayout.MaxWidth(35));
									currentStates[i].expButton13 = GUILayout.Toggle(currentStates[i].expButton13,
										"ZR",GUILayout.MaxWidth(35));
								EditorGUILayout.EndHorizontal();
							
								EditorGUILayout.BeginHorizontal();	
									float classicAnalog = currentStates[i].expFloat5;
									if(!isVirtual)
										classicAnalog = (classicAnalog-2.0f)/29.0f;
									currentStates[i].expFloat5 = GUILayout.HorizontalSlider(classicAnalog, 0, 1, GUILayout.MaxWidth(275));
									if(isVirtual)
									{
										if(currentStates[i].expFloat5<=.7f)
											currentStates[i].expButton14 = false;
										if(currentStates[i].expFloat5>=.9f)
											currentStates[i].expButton14 = true;
										
									}
									bool newButton = GUILayout.Toggle(currentStates[i].expButton14,"L",GUILayout.MaxWidth(25));
									if(isVirtual && newButton != currentStates[i].expButton14)
									{
										currentStates[i].expButton14 = newButton;
										if(newButton && currentStates[i].expFloat5<.9f)
											currentStates[i].expFloat5=.9f;
										else
											if(currentStates[i].expFloat5>.7f)
												currentStates[i].expFloat5=.7f;
									}
								EditorGUILayout.EndHorizontal();
								
								EditorGUILayout.BeginHorizontal();	
									classicAnalog = currentStates[i].expFloat6;
									if(!isVirtual)
										classicAnalog = (classicAnalog-2.0f)/29.0f;
									currentStates[i].expFloat6 = GUILayout.HorizontalSlider(classicAnalog, 0, 1, GUILayout.MaxWidth(275));
									if(isVirtual)
									{
										if(currentStates[i].expFloat6<=.7f)
											currentStates[i].expButton15 = false;
										if(currentStates[i].expFloat6>=.9f)
											currentStates[i].expButton15 = true;
										
									}
									newButton = GUILayout.Toggle(currentStates[i].expButton15,"R",GUILayout.MaxWidth(25));
									if(isVirtual && newButton != currentStates[i].expButton15)
									{
										currentStates[i].expButton15 = newButton;
										if(newButton && currentStates[i].expFloat6<.9f)
											currentStates[i].expFloat6=.9f;
										else
											if(currentStates[i].expFloat6>.7f)
												currentStates[i].expFloat6=.7f;
									}
								EditorGUILayout.EndHorizontal();
							}
						}
						if(currentStates[i].expType==4)//guitar
						{
							Vector2 stick = new Vector2(currentStates[i].expFloat1,currentStates[i].expFloat2);
							if(!isVirtual)
							{
								stick.x -= 32.0f;
        				    	if(stick.x<0)
            						stick.x /=(32.0f-5.0f);
            					else
            						stick.x /=(62.0f-32.0f);
            
           					 	stick.y -= 32.0f;
            					if(stick.y<0)
           					 		stick.y /=(32.0f-5.0f);
            					else
            						stick.y /=(60.0f-32.0f);
							}
							Vector2 newStick = EditorGUILayout.Vector2Field("Guitar Stick",stick,GUILayout.MaxWidth(300));
							if(isVirtual && newStick != stick)
							{
								newStick.x = Mathf.Clamp(newStick.x,-1,1);
								newStick.y = Mathf.Clamp(newStick.y,-1,1);
								currentStates[i].expFloat1 = newStick.x; 
								currentStates[i].expFloat2 = newStick.y;
							}
							EditorGUILayout.BeginHorizontal();
									currentStates[i].expButton1 = GUILayout.Toggle(currentStates[i].expButton1,
										"Green",GUILayout.MaxWidth(45));
									currentStates[i].expButton2 = GUILayout.Toggle(currentStates[i].expButton2,
										"Red",GUILayout.MaxWidth(45));
									currentStates[i].expButton3 = GUILayout.Toggle(currentStates[i].expButton3,
										"Yellow",GUILayout.MaxWidth(55));
									currentStates[i].expButton4 = GUILayout.Toggle(currentStates[i].expButton4,
										"Blue",GUILayout.MaxWidth(45));
									currentStates[i].expButton5 = GUILayout.Toggle(currentStates[i].expButton5,
										"Orange",GUILayout.MaxWidth(55));
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.BeginHorizontal();
									GUILayout.Label ("Strum:", GUILayout.MaxWidth (45));		
									currentStates[i].expButton8 = GUILayout.Toggle(!currentStates[i].expButton9 && currentStates[i].expButton8,
															"Up",GUILayout.MaxWidth(45));
									currentStates[i].expButton9 = GUILayout.Toggle(!currentStates[i].expButton8 && currentStates[i].expButton9,
															" Down",GUILayout.MaxWidth(105));
									currentStates[i].expButton7 = GUILayout.Toggle(currentStates[i].expButton7,
										"-",GUILayout.MaxWidth(25));
									currentStates[i].expButton6 = GUILayout.Toggle(currentStates[i].expButton6,
										"+",GUILayout.MaxWidth(25));
							EditorGUILayout.EndHorizontal();
							//whammy
							float theWhammy = currentStates[i].expFloat4;
							if(!isVirtual)
							{
								theWhammy = (theWhammy-15.0f)/(26.0f-15.0f);
							}
							currentStates[i].expFloat4 = EditorGUILayout.FloatField("Whammy ",theWhammy,GUILayout.MaxWidth(150));
							currentStates[i].expFloat4 = Mathf.Clamp(currentStates[i].expFloat4,0,1);
								
							//touch thing
							//currentStates[i].expFloat3 = EditorGUILayout.FloatField("Slider ",currentStates[i].expFloat3,GUILayout.MaxWidth(150));
							//currentStates[i].expFloat3 = Mathf.Clamp(currentStates[i].expFloat3,0,1);


						}
						if(currentStates[i].expType==5)//drumset
						{
							//stick
							Vector2 stick = new Vector2(currentStates[i].expFloat1,currentStates[i].expFloat2);
							if(!isVirtual)
							{
								stick.x -= 32.0f;
        				    	if(stick.x<0)
            						stick.x /=32.0f;
            					else
            						stick.x /=(63.0f-32.0f);
            
           					 	stick.y -= 31.0f;
            					if(stick.y<0)
           					 		stick.y /=31.0f;
            					else
            						stick.y /=(63.0f-31.0f);
							}
							Vector2 newStick = EditorGUILayout.Vector2Field("Drum Stick",stick,GUILayout.MaxWidth(300));
							if(isVirtual && newStick != stick)
							{
								newStick.x = Mathf.Clamp(newStick.x,-1,1);
								newStick.y = Mathf.Clamp(newStick.y,-1,1);
								currentStates[i].expFloat1 = newStick.x; 
								currentStates[i].expFloat2 = newStick.y;
							}
							
							EditorGUILayout.BeginHorizontal();
							currentStates[i].expButton7 = GUILayout.Toggle(currentStates[i].expButton7,
								"-",GUILayout.MaxWidth(25));
							currentStates[i].expButton6 = GUILayout.Toggle(currentStates[i].expButton6,
								"+",GUILayout.MaxWidth(25));
							EditorGUILayout.EndHorizontal();
							//
							EditorGUILayout.BeginHorizontal();
							float theHit = currentStates[i].expFloat3;
							if(!isVirtual)
							{
								theHit = (7.0f-theHit)/7.0f;
							}
							float newHit = EditorGUILayout.FloatField("Green  ",theHit,GUILayout.MaxWidth(140));
							if(isVirtual && newHit != theHit)
							{
								currentStates[i].expFloat3 = Mathf.Clamp(newHit,0.143f,1);
							}
							currentStates[i].expButton1 = GUILayout.Toggle(currentStates[i].expButton1,"",GUILayout.MaxWidth(25));
							EditorGUILayout.EndHorizontal();
							//
							EditorGUILayout.BeginHorizontal();
							theHit = currentStates[i].expFloat4;
							if(!isVirtual)
							{
								theHit = (7.0f-theHit)/7.0f;
							}
							newHit = EditorGUILayout.FloatField("Red    ",theHit,GUILayout.MaxWidth(140));
							if(isVirtual && newHit != theHit)
							{
								currentStates[i].expFloat4 = Mathf.Clamp(newHit,0.143f,1);
							}
							currentStates[i].expButton2 = GUILayout.Toggle(currentStates[i].expButton2,"",GUILayout.MaxWidth(25));
							EditorGUILayout.EndHorizontal();
							//
							EditorGUILayout.BeginHorizontal();
							theHit = currentStates[i].expFloat5;
							if(!isVirtual)
							{
								theHit = (7.0f-theHit)/7.0f;
							}
							newHit = EditorGUILayout.FloatField("Yellow ",theHit,GUILayout.MaxWidth(140));
							if(isVirtual && newHit != theHit)
							{
								currentStates[i].expFloat5 = Mathf.Clamp(newHit,0.143f,1);
							}
							currentStates[i].expButton3 = GUILayout.Toggle(currentStates[i].expButton3,"",GUILayout.MaxWidth(25));
							EditorGUILayout.EndHorizontal();
							//
							EditorGUILayout.BeginHorizontal();
							theHit = currentStates[i].expFloat6;
							if(!isVirtual)
							{
								theHit = (7.0f-theHit)/7.0f;
							}
							newHit = EditorGUILayout.FloatField("Blue   ",theHit,GUILayout.MaxWidth(140));
							if(isVirtual && newHit != theHit)
							{
								currentStates[i].expFloat6 = Mathf.Clamp(newHit,0.143f,1);
							}
							currentStates[i].expButton4 = GUILayout.Toggle(currentStates[i].expButton4,"",GUILayout.MaxWidth(25));
							EditorGUILayout.EndHorizontal();
							//
							EditorGUILayout.BeginHorizontal();
							theHit = currentStates[i].expFloat7;
							if(!isVirtual)
							{
								theHit = (7.0f-theHit)/7.0f;
							}
							newHit = EditorGUILayout.FloatField("Orange ",theHit,GUILayout.MaxWidth(140));
							if(isVirtual && newHit != theHit)
							{
								currentStates[i].expFloat7 = Mathf.Clamp(newHit,0.143f,1);
							}
							currentStates[i].expButton5 = GUILayout.Toggle(currentStates[i].expButton5,"",GUILayout.MaxWidth(25));
							EditorGUILayout.EndHorizontal();
							//
							EditorGUILayout.BeginHorizontal();
							theHit = currentStates[i].expFloat8;
							if(!isVirtual)
							{
								theHit = (7.0f-theHit)/7.0f;
							}
							newHit = EditorGUILayout.FloatField("Pedal  ",theHit,GUILayout.MaxWidth(140));
							if(isVirtual && newHit != theHit)
							{
								currentStates[i].expFloat8 = Mathf.Clamp(newHit,0.143f,1);
							}
							currentStates[i].expButton8 = GUILayout.Toggle(currentStates[i].expButton8,"",GUILayout.MaxWidth(25));
							EditorGUILayout.EndHorizontal();	
						}
						if(currentStates[i].expType==6)//turntable
						{
							//stick
							Vector2 stick = new Vector2(currentStates[i].expFloat1,currentStates[i].expFloat2);
							if(!isVirtual)
							{
								stick.x -= 33.0f;
        				    	if(stick.x<0)
            						stick.x /=33.0f;
            					else
            						stick.x /=(63.0f-33.0f);
            
           					 	stick.y -= 32.0f;
            					if(stick.y<0)
           					 		stick.y /=32.0f;
            					else
            						stick.y /=(63.0f-32.0f);
							}
							Vector2 newStick = EditorGUILayout.Vector2Field("Turntable Stick",stick,GUILayout.MaxWidth(300));
							if(isVirtual && newStick != stick)
							{
								newStick.x = Mathf.Clamp(newStick.x,-1,1);
								newStick.y = Mathf.Clamp(newStick.y,-1,1);
								currentStates[i].expFloat1 = newStick.x; 
								currentStates[i].expFloat2 = newStick.y;
							}
							//dial
							float theFloat = currentStates[i].expFloat4;
							if(!isVirtual)
							{
								theFloat/=31.0f;
							}
							float newFloat = EditorGUILayout.FloatField("Dial",theFloat,GUILayout.MaxWidth(150));
							if(isVirtual && newFloat != theFloat)
							{
								currentStates[i].expFloat4 = Mathf.Clamp(newFloat,0,1);
							}
							//slider
							theFloat = currentStates[i].expFloat3;
							if(!isVirtual)
							{
								theFloat -= 14.0f;
								if(theFloat<0)
									theFloat/=14.0f;
								else
									theFloat/=(30.0f-14.0f);
							}
							newFloat = EditorGUILayout.FloatField("Slider",theFloat,GUILayout.MaxWidth(150));
							if(isVirtual && theFloat != newFloat)
							{
								currentStates[i].expFloat3 = Mathf.Clamp(newFloat,-1  ,1);
							}
							EditorGUILayout.BeginHorizontal();
							currentStates[i].expButton7 = GUILayout.Toggle(currentStates[i].expButton7,
								"Euphoria",GUILayout.MaxWidth(95));
							currentStates[i].expButton8 = GUILayout.Toggle(currentStates[i].expButton8,
								"-",GUILayout.MaxWidth(25));
							currentStates[i].expButton9 = GUILayout.Toggle(currentStates[i].expButton9,
								"+",GUILayout.MaxWidth(25));
							EditorGUILayout.EndHorizontal();
							if(currentStates[i].expButton10 || isVirtual)
							{
								EditorGUILayout.BeginHorizontal();
								//left platter
								currentStates[i].expFloat5 = EditorGUILayout.FloatField("Left Platter  (raw)",currentStates[i].expFloat5,GUILayout.MaxWidth(140));
								currentStates[i].expFloat5 = Mathf.Clamp(currentStates[i].expFloat5,-16,16);

								currentStates[i].expButton3 = GUILayout.Toggle(currentStates[i].expButton3,
									"Blu",GUILayout.MaxWidth(50));
								currentStates[i].expButton2 = GUILayout.Toggle(currentStates[i].expButton2,
									"Red",GUILayout.MaxWidth(50));
								currentStates[i].expButton1 = GUILayout.Toggle(currentStates[i].expButton1,
									"Gre",GUILayout.MaxWidth(50));
								EditorGUILayout.EndHorizontal();
							}		
							//right platter
							if(currentStates[i].expButton11 || isVirtual)
							{
								EditorGUILayout.BeginHorizontal();
								currentStates[i].expFloat6 = EditorGUILayout.FloatField("Right Platter(raw)",currentStates[i].expFloat6,GUILayout.MaxWidth(140));
								currentStates[i].expFloat6 = Mathf.Clamp(currentStates[i].expFloat6,-32,32);
								currentStates[i].expButton4 = GUILayout.Toggle(currentStates[i].expButton4,
										"Gre",GUILayout.MaxWidth(50));
								currentStates[i].expButton5 = GUILayout.Toggle(currentStates[i].expButton5,
										"Red",GUILayout.MaxWidth(50));
								currentStates[i].expButton6 = GUILayout.Toggle(currentStates[i].expButton6,
										"Blu",GUILayout.MaxWidth(50));
								EditorGUILayout.EndHorizontal();
							}
						}
					}		
				}	
			}
			if(isVirtual && !oldState.Equals(currentStates[i]))//and mouse is released or wantsMouseMove?
			{	
				Wii.SetVirtualState(i,currentStates[i]);
				setVirtualStates(i,currentStates[i]);
			}	
		}
	}
	
    private const string pluginName = "WiiBuddy";
    
    [DllImport(pluginName)]
    private static extern bool checkPlugin();
    
    [DllImport(pluginName)]
    private static extern void initPlugin(string theAssemblyPath);
    
    [DllImport(pluginName)]
    private static extern void setLEDs(int thisRemote, bool LED1, bool LED2, bool LED3, bool LED4);

    [DllImport(pluginName)]
    private static extern void findWiiRemote();

    [DllImport(pluginName)]
    private static extern void stopSearch();

    [DllImport(pluginName)]
    private static extern void dropWiiRemote(int i);

	[DllImport(pluginName)]
    private static extern int getDiscoveryStatus();

	[DllImport(pluginName)]
    private static extern int getPairDiscoveryStatus();
    
    [DllImport(pluginName)]
    private static extern void clearDiscoveryStatus();
    
    [DllImport(pluginName)]
    private static extern void setVirtualStates(int i, Wii.State state);
    
    [DllImport(pluginName)]
    private static extern void addVirtual(int i);
    
    [DllImport(pluginName)]
    private static extern void setVirtualIR(int i, int r, float value);
    
    [DllImport(pluginName)]
    private static extern float getVirtualIR(int i, int r);
    
    [DllImport(pluginName)]
    [return : MarshalAs( UnmanagedType.Struct )]
    private static extern Wii.State getStates(int i);
    
    [DllImport(pluginName)]
    private static extern bool getPanel(int i, int p);
        
    [DllImport(pluginName)]
    private static extern void setPanel(int i, int p, bool o);
    
    [DllImport(pluginName)]
    private static extern void setForceFeedback(int thisRemote, bool enabled);
}