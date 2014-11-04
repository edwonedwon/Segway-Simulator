
var nunchuk: Transform;
var wiimote: Transform;
static var whichRemote: int;


var buttonC : MeshRenderer;
var buttonZ : MeshRenderer;
var nunchuckStick   : Transform;

var buttonA : MeshRenderer;
var buttonB : MeshRenderer;

var buttonUp : MeshRenderer;
var buttonDown : MeshRenderer;
var buttonLeft : MeshRenderer;
var buttonRight : MeshRenderer;

var buttonMinus : MeshRenderer;
var buttonPlus : MeshRenderer;
var buttonHome : MeshRenderer;

var buttonOne : MeshRenderer;
var buttonTwo : MeshRenderer;


var buttonClassicA : MeshRenderer;
var buttonClassicB : MeshRenderer;
var buttonClassicMinus : MeshRenderer;
var buttonClassicPlus : MeshRenderer;
var buttonClassicHome : MeshRenderer;
var buttonClassicX : MeshRenderer;
var buttonClassicY : MeshRenderer;
var buttonClassicUp : MeshRenderer;
var buttonClassicDown : MeshRenderer;
var buttonClassicLeft : MeshRenderer;
var buttonClassicRight : MeshRenderer;
    
var classic : Transform;
var classicStickLeft : Transform;
var classicStickRight : Transform;
    
var buttonClassicL : MeshRenderer;
var buttonClassicR : MeshRenderer;
    
var buttonClassicZL : MeshRenderer;
var buttonClassicZR : MeshRenderer;

var buttonGuitarGreen:  MeshRenderer;
var buttonGuitarRed:    MeshRenderer;
var buttonGuitarYellow: MeshRenderer;
var buttonGuitarBlue:   MeshRenderer;
var buttonGuitarOrange: MeshRenderer;
var buttonGuitarPlus:   MeshRenderer;
var buttonGuitarMinus:  MeshRenderer;
var guitarBody:   Transform;
var guitarNeck:   Transform;
var guitarStick:  Transform;
var guitarStrum:  Transform;
var guitarWhammy: Transform;

var drumSet: Transform;
var drumMinus: MeshRenderer;
var drumPlus:  MeshRenderer;
var drumStick:  Transform;
var drumGreen:  Transform;
var drumBlue:   Transform;
var drumRed:    Transform;
var drumOrange: Transform;
var drumYellow: Transform;

var hub: Transform;
var dial: Transform;
var slider: Transform;
var tableStick: Transform;
var buttonTablePlus: MeshRenderer;
var buttonTableMinus: MeshRenderer;
var butttonEuphoria: MeshRenderer;
var turnTableLeft: MeshRenderer;
var turnTableRight: MeshRenderer;
var platterLeft: Transform;
var platterRight: Transform;
var tableLeftGreen: MeshRenderer;
var tableLeftRed: MeshRenderer;
var tableLeftBlue: MeshRenderer;
var tableRightGreen: MeshRenderer;
var tableRightRed: MeshRenderer;
var tableRightBlue: MeshRenderer;


var motionPlus:    Transform;

var theIRMain: GUITexture;
var theIR1:    GUITexture;
var theIR2:    GUITexture;
var theIR3:    GUITexture;
var theIR4:    GUITexture;

var balanceBoard       : Transform;
var balanceTopLeft     : Transform;
var balanceTopRight    : Transform;
var balanceBottomLeft  : Transform;
var balanceBottomRight : Transform;


//private var searching = false;
var theText : GUIText;
var theRemoteNumber: GUIText;
var WiiObject: GameObject;
var Wii;

Wii = WiiObject.GetComponent("Wii");

function setVisibility(t : Transform , v : boolean)
{
	if (t.renderer && t.renderer.enabled != v) 
	{
		t.renderer.enabled = v;
	}
	renderers = t.GetComponentsInChildren (Renderer);
		for (var rendy : Renderer in renderers) {
    		rendy.enabled = v;
		}
}

function Start () {
	theText.text = "Ready.";
	setVisibility(wiimote,false);
	setVisibility(nunchuk,false);
	setVisibility(classic,false);
	setVisibility(guitarBody,false);
	setVisibility(motionPlus,false);
	setVisibility(balanceBoard,false);
	setVisibility(hub,false);
	setVisibility(drumSet,false);
}
 var totalRemotes = 0;
function OnGUI () {
	totalRemotes = Wii.GetRemoteCount();
	
	theRemoteNumber.text = "Remote on Display:"+(whichRemote+1).ToString();
	
	for(var x=0;x<16;x++)
	{
		if(Wii.IsActive(x))
		{
			if (GUI.Button (Rect ((x*70),0, 70, 50), ("remote "+(x+1)) )) 
			{
				whichRemote = x;
				if(!Wii.IsActive(whichRemote))
				{
					if(Wii.IsSearching())
						theText.text = ("I'm looking.");
					else
						theText.text = ("remote "+whichRemote+" is not active.");
				}
			}
		}
	}
	
	if (Wii.IsActive(whichRemote))
	{
		if(GUI.Button (Rect (150,50, 50, 30), "drop")) {
			Wii.DropWiiRemote(whichRemote);   
			theText.text = ("Dropped "+whichRemote);
		}

		if(Wii.HasMotionPlus(whichRemote))
		{
			if(GUI.Button (Rect (170,80, 160, 30), "Deactivate Motion Plus")) 
			{
				Wii.DeactivateMotionPlus(whichRemote);
			}
			if(Wii.IsMotionPlusCalibrated(whichRemote))
			{
				if(GUI.Button (Rect (190,110, 180, 30), "Uncalibrate Motion Plus")) 
				{
					Wii.UncalibrateMotionPlus(whichRemote);
				}
			}
			else
			{
				if(GUI.Button (Rect (190,110, 160, 30), "Calibrate Motion Plus")) 
				{
					Wii.CalibrateMotionPlus(whichRemote);
				}
			}   
		}
		else
		{
			if(GUI.Button (Rect (170,80, 160, 30), "Check For Motion Plus")) 
			{
				Wii.CheckForMotionPlus(whichRemote);
			}
		}
	}
	
	if(Wii.IsSearching())
	{
		if (GUI.Button (Rect (0,60, 128, 58), "Cancel")) {
			//searching = false;
			Wii.StopSearch();   
			theText.text = "Cancelled.";
		}	
	}
	else
	{
		if (!Wii.IsSearching() && (totalRemotes<16) && GUI.Button (Rect (0,50, 128, 58), "Find")) {
			searching = true;
			Wii.StartSearch();   
			theText.text = "I'm looking.";
			Time.timeScale = 1.0;
		}
	}	
}

function OnDiscoveryError(i : int) {
	theText.text = "Error:"+i+". Try Again.";
	//searching = false;
}

function OnWiimoteFound (thisRemote: int) {
	Debug.Log("found this one: "+thisRemote);
	if(!Wii.IsActive(whichRemote))
		whichRemote = thisRemote;
}

function OnWiimoteDisconnected (whichRemote: int) {
	Debug.Log("lost this one: "+ whichRemote);	
}

function Update () {
	if(Wii.IsActive(whichRemote))
	{		
		theRemoteNumber.enabled=true;
		var inputDisplay = "";
		inputDisplay = inputDisplay + "Remote #"+whichRemote.ToString();
		inputDisplay = inputDisplay + "\nbattery "+Wii.GetBattery(whichRemote).ToString();
		
		if(Wii.GetExpType(whichRemote)==3)//balance board is in is in
		{
			setVisibility(balanceBoard,true);
			setVisibility(wiimote,false);
			var theBalanceBoard = Wii.GetBalanceBoard(whichRemote); 
			var theCenter = Wii.GetCenterOfBalance(whichRemote);
			//Debug.Log(theBalanceBoard+" "+theCenter);
			balanceTopLeft.localScale.y     = 1-(.01*theBalanceBoard.y); 
			balanceTopRight.localScale.y    = 1-(.01*theBalanceBoard.x);
			balanceBottomLeft.localScale.y  = 1-(.01*theBalanceBoard.w);
			balanceBottomRight.localScale.y = 1-(.01*theBalanceBoard.z);

			theIR1.pixelInset = Rect(Screen.width/2-(Screen.width/4),Screen.height/2+(Screen.height/4),10.0,10.0);
			theIR2.pixelInset = Rect(Screen.width/2+(Screen.width/4),Screen.height/2+(Screen.height/4),10.0,10.0);
			theIR3.pixelInset = Rect(Screen.width/2-(Screen.width/4),Screen.height/2-(Screen.height/4),10.0,10.0);
			theIR4.pixelInset = Rect(Screen.width/2+(Screen.width/4),Screen.height/2-(Screen.height/4),10.0,10.0);
			theIR1.pixelInset.x -=theIR1.pixelInset.width/2;
			theIR1.pixelInset.y -=theIR1.pixelInset.height/2;
			theIR2.pixelInset.x -=theIR2.pixelInset.width/2;
			theIR2.pixelInset.y -=theIR2.pixelInset.height/2;
			theIR3.pixelInset.x -=theIR3.pixelInset.width/2;
			theIR3.pixelInset.y -=theIR3.pixelInset.height/2;
			theIR4.pixelInset.x -=theIR4.pixelInset.width/2;
			theIR4.pixelInset.y -=theIR4.pixelInset.height/2;			
			theIRMain.pixelInset = Rect((Screen.width/2)+(theCenter.x*(Screen.width/4)),(Screen.height/2)+(theCenter.y*Screen.height/4),50.0,50.0);
			theIRMain.pixelInset.x -=theIRMain.pixelInset.width/2;
			theIRMain.pixelInset.y -=theIRMain.pixelInset.height/2;
			
			inputDisplay = inputDisplay + "\nBALANCE BOARD";
			inputDisplay = inputDisplay + "\ntotal Weight "+Wii.GetTotalWeight(whichRemote)+"kg";
			inputDisplay = inputDisplay + "\ntopRight     "+theBalanceBoard.x+"kg";
			inputDisplay = inputDisplay + "\ntopLeft      "+theBalanceBoard.y+"kg";
			inputDisplay = inputDisplay + "\nbottomRight  "+theBalanceBoard.z+"kg";
			inputDisplay = inputDisplay + "\nbottomLeft   "+theBalanceBoard.w+"kg";
		}
		else
		{
			///WIIREMOTE
			setVisibility(wiimote,true);
			pointerArray = Wii.GetRawIRData(whichRemote);		
			mainPointer = Wii.GetIRPosition(whichRemote);
			wiiAccel = Wii.GetWiimoteAcceleration(whichRemote);
			
			theIRMain.pixelInset = Rect(mainPointer.x*Screen.width-25.0f,mainPointer.y*Screen.height-25.0f,50.0,50.0);
			var sizeScale = 5.0f;
			
			theIR1.pixelInset = Rect (pointerArray[0].x*Screen.width-(pointerArray[0].z*sizeScale/2.0f),
			 							pointerArray[0].y*Screen.height-(pointerArray[0].z*sizeScale/2.0f),
			 							pointerArray[0].z*sizeScale*10, pointerArray[0].z*sizeScale*10);
			 							
			theIR2.pixelInset = Rect (pointerArray[1].x*Screen.width-(pointerArray[1].z*sizeScale/2.0f),
										pointerArray[1].y*Screen.height-(pointerArray[1].z*sizeScale/2.0f),
										pointerArray[1].z*sizeScale*10, pointerArray[1].z*sizeScale*10);
										
			theIR3.pixelInset = Rect (pointerArray[2].x*Screen.width-(pointerArray[2].z*sizeScale/2.0f),
										pointerArray[2].y*Screen.height-(pointerArray[2].z*sizeScale/2.0f),
										pointerArray[2].z*sizeScale*10, pointerArray[2].z*sizeScale*10);
										
			theIR4.pixelInset = Rect (pointerArray[3].x*Screen.width-(pointerArray[3].z*sizeScale/2.0f),
										pointerArray[3].y*Screen.height-(pointerArray[3].z*sizeScale/2.0f),
										pointerArray[3].z*sizeScale*10, pointerArray[3].z*sizeScale*10);
		
			wiimote.localRotation = Quaternion.Slerp(transform.localRotation,
				Quaternion.Euler(wiiAccel.y*90.0, 0.0,wiiAccel.x*-90.0),5.0);
				
			if(Wii.GetButton(whichRemote, "A"))
				buttonA.enabled = true;
			else
				buttonA.enabled = false;
			if(Wii.GetButton(whichRemote, "B"))
				buttonB.enabled = true;
			else
				buttonB.enabled = false;
			if(Wii.GetButton(whichRemote, "UP"))
				buttonUp.enabled = true;
			else
				buttonUp.enabled = false;
			if(Wii.GetButton(whichRemote, "DOWN"))
				buttonDown.enabled = true;
			else
				buttonDown.enabled = false;
			if(Wii.GetButton(whichRemote, "LEFT"))
				buttonLeft.enabled = true;
			else
				buttonLeft.enabled = false;
			if(Wii.GetButton(whichRemote, "RIGHT"))
				buttonRight.enabled = true;
			else
				buttonRight.enabled = false;
			if(Wii.GetButton(whichRemote, "MINUS"))
				buttonMinus.enabled = true;
			else
				buttonMinus.enabled = false;
			if(Wii.GetButton(whichRemote, "PLUS"))
				buttonPlus.enabled = true;
			else
				buttonPlus.enabled = false;
			if(Wii.GetButton(whichRemote, "HOME"))
				buttonHome.enabled = true;
			else
				buttonHome.enabled = false;
			if(Wii.GetButton(whichRemote, "ONE"))
				buttonOne.enabled = true;
			else
				buttonOne.enabled = false;		
			if(Wii.GetButton(whichRemote, "TWO"))
				buttonTwo.enabled = true;
			else
				buttonTwo.enabled = false;
				
			inputDisplay = inputDisplay + "\nIR      "+Wii.GetIRPosition(whichRemote).ToString("#.0000");
			inputDisplay = inputDisplay + "\nIR rot  "+Wii.GetIRRotation(whichRemote).ToString();
			inputDisplay = inputDisplay + "\nA       "+Wii.GetButton(whichRemote,"A").ToString();
			inputDisplay = inputDisplay + "\nB       "+Wii.GetButton(whichRemote,"B").ToString();
			inputDisplay = inputDisplay + "\n1       "+Wii.GetButton(whichRemote,"1").ToString();
			inputDisplay = inputDisplay + "\n2       "+Wii.GetButton(whichRemote,"2").ToString();
			inputDisplay = inputDisplay + "\nUp      "+Wii.GetButton(whichRemote,"UP").ToString();
			inputDisplay = inputDisplay + "\nDown    "+Wii.GetButton(whichRemote,"DOWN").ToString();
			inputDisplay = inputDisplay + "\nLeft    "+Wii.GetButton(whichRemote,"LEFT").ToString();
			inputDisplay = inputDisplay + "\nRight   "+Wii.GetButton(whichRemote,"RIGHT").ToString();
			inputDisplay = inputDisplay + "\n-       "+Wii.GetButton(whichRemote,"-").ToString();
			inputDisplay = inputDisplay + "\n+       "+Wii.GetButton(whichRemote,"+").ToString();
			inputDisplay = inputDisplay + "\nHome    "+Wii.GetButton(whichRemote,"HOME").ToString();
			inputDisplay = inputDisplay + "\nAccel   "+Wii.GetWiimoteAcceleration(whichRemote).ToString("#.0000");
			
			if(Wii.HasMotionPlus(whichRemote))
			{
				setVisibility(motionPlus,true);
				motion = Wii.GetMotionPlus(whichRemote);
				if(Input.GetKeyDown("space") || Wii.GetButtonDown(whichRemote,"HOME"))
				{
					motionPlus.localRotation = Quaternion.identity;
				}
				motionPlus.RotateAround(motionPlus.position,motionPlus.right,motion.x);
				motionPlus.RotateAround(motionPlus.position,motionPlus.up,-motion.y);
				motionPlus.RotateAround(motionPlus.position,motionPlus.forward,motion.z);
				
				inputDisplay = inputDisplay + "\nMotion+ "+motion.ToString("#.0000");
				inputDisplay = inputDisplay + "\nYAW FAST "+Wii.IsYawFast(whichRemote);
				inputDisplay = inputDisplay + "\nROLL FAST "+Wii.IsRollFast(whichRemote);				inputDisplay = inputDisplay + "\nPITCH FAST "+Wii.IsPitchFast(whichRemote);				

			}
			else
			{
				setVisibility(motionPlus,false);
			}
			
			if(Wii.GetExpType(whichRemote)==1)//nunchuck is in
			{
				setVisibility(nunchuk,true);
				nunchuk.localRotation = Quaternion.Slerp(transform.localRotation,
					Quaternion.Euler(Wii.GetNunchuckAcceleration(whichRemote).y * 90.0,
						0.0, 
						Wii.GetNunchuckAcceleration(whichRemote).x*-90),
						5.0);
						
				nunchuckStick.rotation = nunchuk.rotation;
				nunchuckStick.RotateAround(nunchuckStick.position,nunchuckStick.right,Wii.GetAnalogStick(whichRemote).y*30.0);
				nunchuckStick.RotateAround(nunchuckStick.position,nunchuckStick.forward,Wii.GetAnalogStick(whichRemote).x*-30.0);
				
				if(Wii.GetButton(whichRemote, "C"))
					buttonC.enabled = true;
				else
					buttonC.enabled = false;
				if(Wii.GetButton(whichRemote,"Z"))
					buttonZ.enabled = true;
				else
					buttonZ.enabled = false;
				
				inputDisplay = inputDisplay + "\nNUNCHUCK";
				inputDisplay = inputDisplay + "\nC       "+Wii.GetButton(whichRemote,"C").ToString();
				inputDisplay = inputDisplay + "\nZ       "+Wii.GetButton(whichRemote,"Z").ToString();
				inputDisplay = inputDisplay + "\nnunchuckStick N "+Wii.GetAnalogStick(whichRemote).ToString("#.0000");
				inputDisplay = inputDisplay + "\nAccel N "+Wii.GetNunchuckAcceleration(whichRemote).ToString("#.0000");
			}
			else if(Wii.GetExpType(whichRemote)==2)//classic controller is in
			{
				setVisibility(classic,true);
				var theStickLeft  = Wii.GetAnalogStick(whichRemote,"CLASSICLEFT");
				var theStickRight = Wii.GetAnalogStick(whichRemote,"CLASSICRIGHT");
				
				classicStickLeft.rotation = classic.rotation;
				classicStickLeft.RotateAround(classicStickLeft.position,transform.right,
					theStickLeft.y* 30.0);
				classicStickLeft.RotateAround(classicStickLeft.position,transform.forward,
					theStickLeft.x*-30.0); 
				
				classicStickRight.rotation = classic.rotation;
				classicStickRight.RotateAround(classicStickRight.position,transform.right,
					theStickRight.y* 30.0);
				classicStickRight.RotateAround(classicStickRight.position,transform.forward,
					theStickRight.x*-30.0); 
					
				buttonClassicL.transform.localScale.x = 4.0 * Wii.GetAnalogButton(whichRemote, "CLASSICL");
				buttonClassicR.transform.localScale.x = 4.0 * Wii.GetAnalogButton(whichRemote, "CLASSICR");
				
				buttonClassicA.enabled     = Wii.GetButton(whichRemote,"CLASSICA");
				buttonClassicB.enabled     = Wii.GetButton(whichRemote,"CLASSICB");
				buttonClassicMinus.enabled = Wii.GetButton(whichRemote,"CLASSICMINUS");
				buttonClassicPlus.enabled  = Wii.GetButton(whichRemote,"CLASSICPLUS");
				buttonClassicHome.enabled  = Wii.GetButton(whichRemote,"CLASSICHOME");
				buttonClassicX.enabled     = Wii.GetButton(whichRemote,"CLASSICX");
				buttonClassicY.enabled     = Wii.GetButton(whichRemote,"CLASSICY");
				buttonClassicUp.enabled    = Wii.GetButton(whichRemote,"CLASSICUP");
				buttonClassicDown.enabled  = Wii.GetButton(whichRemote,"CLASSICDOWN");
				buttonClassicLeft.enabled  = Wii.GetButton(whichRemote,"CLASSICLEFT");
				buttonClassicRight.enabled = Wii.GetButton(whichRemote,"CLASSICRIGHT");
				buttonClassicL.enabled     = Wii.GetButton(whichRemote,"CLASSICL");
				buttonClassicR.enabled     = Wii.GetButton(whichRemote,"CLASSICR");
				buttonClassicZL.enabled    = Wii.GetButton(whichRemote,"CLASSICZL");
				buttonClassicZR.enabled    = Wii.GetButton(whichRemote,"CLASSICZR");
									
				inputDisplay = inputDisplay + "\n CLASSIC";
				inputDisplay = inputDisplay + "\na       "+Wii.GetButton(whichRemote,"CLASSICA").ToString();    
	            inputDisplay = inputDisplay + "\nb       "+Wii.GetButton(whichRemote,"CLASSICB").ToString();
	            inputDisplay = inputDisplay + "\n-       "+Wii.GetButton(whichRemote,"CLASSICMINUS").ToString();
	            inputDisplay = inputDisplay + "\n+       "+Wii.GetButton(whichRemote,"CLASSICPLUS").ToString();
	            inputDisplay = inputDisplay + "\nhome    "+Wii.GetButton(whichRemote,"CLASSICHOME").ToString();
	            inputDisplay = inputDisplay + "\nx       "+Wii.GetButton(whichRemote,"CLASSICX").ToString();
	            inputDisplay = inputDisplay + "\ny       "+Wii.GetButton(whichRemote,"CLASSICY").ToString();
	            inputDisplay = inputDisplay + "\nup      "+Wii.GetButton(whichRemote,"CLASSICUP").ToString();
	            inputDisplay = inputDisplay + "\ndown    "+Wii.GetButton(whichRemote,"CLASSICDOWN").ToString();
	            inputDisplay = inputDisplay + "\nleft    "+Wii.GetButton(whichRemote,"CLASSICLEFT").ToString();
	            inputDisplay = inputDisplay + "\nright   "+Wii.GetButton(whichRemote,"CLASSICRIGHT").ToString();
	            inputDisplay = inputDisplay + "\nL       "+Wii.GetButton(whichRemote,"CLASSICL").ToString();
	            inputDisplay = inputDisplay + " "      +Wii.GetAnalogButton(whichRemote,"CLASSICL").ToString("#.000");
	            inputDisplay = inputDisplay + "\nR       "+Wii.GetButton(whichRemote,"CLASSICR").ToString();
	            inputDisplay = inputDisplay + " "      +Wii.GetAnalogButton(whichRemote,"CLASSICR").ToString("#.000");
	            inputDisplay = inputDisplay + "\nZL      "+Wii.GetButton(whichRemote,"CLASSICZL").ToString();
	            inputDisplay = inputDisplay + "\nZR      "+Wii.GetButton(whichRemote,"CLASSICZR").ToString();
	            inputDisplay = inputDisplay + "\nStick L "+theStickLeft.ToString("#.0000");
				inputDisplay = inputDisplay + "\nStick R "+theStickRight.ToString("#.0000");	       
			}
			else if(Wii.GetExpType(whichRemote)==4)//guitar is in
			{
				setVisibility(guitarBody,true);
				var theStick  = Wii.GetAnalogStick(whichRemote,"GUITAR");
				var theWhammy = Wii.GetGuitarWhammy(whichRemote);
				var theStrum  = Wii.GetGuitarStrum(whichRemote);
				
				
				buttonGuitarGreen.enabled = Wii.GetButton(whichRemote,"GUITARGREEN");
				buttonGuitarRed.enabled = Wii.GetButton(whichRemote,"GUITARRED");
				buttonGuitarYellow.enabled = Wii.GetButton(whichRemote,"GUITARYELLOW");
				buttonGuitarBlue.enabled = Wii.GetButton(whichRemote,"GUITARBLUE");
				buttonGuitarOrange.enabled = Wii.GetButton(whichRemote,"GUITARORANGE");
				buttonGuitarPlus.enabled = Wii.GetButton(whichRemote,"GUITARPLUS");
				buttonGuitarMinus.enabled = Wii.GetButton(whichRemote,"GUITARMINUS");
				
				guitarBody.localRotation = Quaternion.Euler(0,0,0);
				guitarBody.RotateAround(guitarBody.position,guitarBody.forward,wiiAccel.x*90.0);
				
				theStick = Wii.GetAnalogStick(whichRemote,"GUITAR");
				guitarStick.rotation = guitarBody.rotation;
				guitarStick.RotateAround(guitarStick.position, guitarStick.up,  theStick.y* 30.0);
			   	guitarStick.RotateAround(guitarStick.position, guitarStick.right,theStick.x*-30.0);
				
				
				guitarStrum.rotation = guitarBody.rotation;
				guitarStrum.RotateAround(guitarStrum.position,guitarStrum.up,20.0*Wii.GetGuitarStrum(whichRemote));
				guitarWhammy.rotation = guitarBody.rotation;
				guitarWhammy.RotateAround(guitarWhammy.position,guitarWhammy.right,20.0*Wii.GetGuitarWhammy(whichRemote));
				
				inputDisplay = inputDisplay + "\n GUITAR";
				inputDisplay = inputDisplay + "\nGreen  "+Wii.GetButton(whichRemote,"GUITARGREEN").ToString();
				inputDisplay = inputDisplay + "\nRed    "+Wii.GetButton(whichRemote,"GUITARRED").ToString();
				inputDisplay = inputDisplay + "\nYellow "+Wii.GetButton(whichRemote,"GUITARYELLOW").ToString();
				inputDisplay = inputDisplay + "\nBlue   "+Wii.GetButton(whichRemote,"GUITARBLUE").ToString();
				inputDisplay = inputDisplay + "\nOrange "+Wii.GetButton(whichRemote,"GUITARORANGE").ToString();
				inputDisplay = inputDisplay + "\nPlus   "+Wii.GetButton(whichRemote,"GUITARPLUS").ToString();
				inputDisplay = inputDisplay + "\nMinus  "+Wii.GetButton(whichRemote,"GUITARMINUS").ToString();
				inputDisplay = inputDisplay + "\nStick  "+theStick.ToString("#.0000");
				inputDisplay = inputDisplay + "\nWhammy "+theWhammy.ToString("#.0000");
				inputDisplay = inputDisplay + "\nStrum  "+theStrum.ToString();

			}
			else if(Wii.GetExpType(whichRemote)==5)//drums are in
			{
				setVisibility(drumSet,true);
				theStick = Wii.GetAnalogStick(whichRemote,"DRUMS");
				drumStick.rotation = drumSet.rotation;
				drumStick.RotateAround(drumStick.position,  drumStick.right,
					theStick.y* 30.0);
				drumStick.RotateAround(drumStick.position,drumStick.forward,
					theStick.x*-30.0);

				drumPlus.enabled = Wii.GetButton(whichRemote,"DRUMPLUS");
				drumMinus.enabled = Wii.GetButton(whichRemote,"DRUMMINUS");

				if(Wii.GetButton(whichRemote,"DRUMGREEN"))
				{
					drumGreen.localScale.x = 1.5/Wii.GetDrumVelocity(whichRemote,"GREEN");
					drumGreen.localScale.z = drumGreen.localScale.x;
				}
				else
				{
					drumGreen.localScale.x = Mathf.Lerp(drumGreen.localScale.x,1.5,.1);
					drumGreen.localScale.z = drumGreen.localScale.x;
				}

				if(Wii.GetButton(whichRemote,"DRUMBLUE"))
				{
					drumBlue.localScale.x = 1.5/Wii.GetDrumVelocity(whichRemote,"BLUE");
					drumBlue.localScale.z = drumBlue.localScale.x;
				}
				else
				{
					drumBlue.localScale.x = Mathf.Lerp(drumBlue.localScale.x,1.5,.1);
					drumBlue.localScale.z = drumBlue.localScale.x;
				}

				if(Wii.GetButton(whichRemote,"DRUMRED"))
				{
					drumRed.localScale.x = 1.5/Wii.GetDrumVelocity(whichRemote,"RED");
					drumRed.localScale.z = drumRed.localScale.x;
				}
				else
				{
					drumRed.localScale.x = Mathf.Lerp(drumRed.localScale.x,1.5,.1);
					drumRed.localScale.z = drumRed.localScale.x;
				}

				if(Wii.GetButton(whichRemote,"DRUMORANGE"))
				{
					drumOrange.localScale.x = 1.5/Wii.GetDrumVelocity(whichRemote,"ORANGE");
					drumOrange.localScale.z = drumOrange.localScale.x;
				}
				else
				{
					drumOrange.localScale.x = Mathf.Lerp(drumOrange.localScale.x,1.5,.1);
					drumGreen.localScale.z = drumOrange.localScale.x;
				}

				if(Wii.GetButton(whichRemote,"DRUMYELLOW"))
				{
					drumYellow.localScale.x = 1.5/Wii.GetDrumVelocity(whichRemote,"YELLOW");
					drumYellow.localScale.z = drumYellow.localScale.x;
				}
				else
				{
					drumYellow.localScale.x = Mathf.Lerp(drumYellow.localScale.x,1.5,.1);
					drumYellow.localScale.z = drumYellow.localScale.x;
				}

				inputDisplay = inputDisplay + "\n  DRUMS";
				inputDisplay = inputDisplay + "\nGreen  "+Wii.GetButton(whichRemote,"DRUMGREEN").ToString();
				inputDisplay = inputDisplay + "\nRed    "+Wii.GetButton(whichRemote,"DRUMRED").ToString();
				inputDisplay = inputDisplay + "\nYellow "+Wii.GetButton(whichRemote,"DRUMYELLOW").ToString();
				inputDisplay = inputDisplay + "\nBlue   "+Wii.GetButton(whichRemote,"DRUMBLUE").ToString();
				inputDisplay = inputDisplay + "\nOrange "+Wii.GetButton(whichRemote,"DRUMORANGE").ToString();
				inputDisplay = inputDisplay + "\nPlus   "+Wii.GetButton(whichRemote,"DRUMPLUS").ToString();
				inputDisplay = inputDisplay + "\nMinus  "+Wii.GetButton(whichRemote,"DRUMMINUS").ToString();
				inputDisplay = inputDisplay + "\nPedal  "+Wii.GetButton(whichRemote,"DRUMPEDAL").ToString();
				inputDisplay = inputDisplay + "\nStick  "+theStick.ToString("#.0000");

			}
			else if(Wii.GetExpType(whichRemote)==6)//turntable is in
			{
				theStick = Wii.GetAnalogStick(whichRemote,"TURNTABLE");
				setVisibility(hub,true);

				dial.rotation = hub.rotation;
				dial.RotateAround(dial.position,dial.up, 360.0f * Wii.GetTurntableDial(whichRemote));
				
				slider.localPosition.x = .3 * Wii.GetTurntableSlider(whichRemote);
				
				
				tableStick.rotation = hub.rotation;
				tableStick.RotateAround(tableStick.position,  tableStick.right,
					theStick.y* 30.0);
				tableStick.RotateAround(tableStick.position,tableStick.forward,
					theStick.x*-30.0);

				buttonTablePlus.enabled  = Wii.GetButton(whichRemote,"TURNTABLEPLUS");
				buttonTableMinus.enabled = Wii.GetButton(whichRemote,"TURNTABLEMINUS");
				butttonEuphoria.enabled  = Wii.GetButton(whichRemote,"TURNTABLEEUPHORIA");
				
				platterLeft.RotateAround( platterLeft.position, platterLeft.up, Wii.GetTurntableSpin(whichRemote,"LEFT") );
				platterRight.RotateAround(platterRight.position,platterRight.up,Wii.GetTurntableSpin(whichRemote,"RIGHT"));
				
				tableLeftGreen.enabled  = Wii.GetButton(whichRemote,"TURNTABLEGREENLEFT");
				tableLeftRed.enabled    = Wii.GetButton(whichRemote,"TURNTABLEREDLEFT");
				tableLeftBlue.enabled   = Wii.GetButton(whichRemote,"TURNTABLEBLUELEFT");
				tableRightGreen.enabled = Wii.GetButton(whichRemote,"TURNTABLEGREENRIGHT");
				tableRightRed.enabled   = Wii.GetButton(whichRemote,"TURNTABLEREDRIGHT");
				tableRightBlue.enabled  = Wii.GetButton(whichRemote,"TURNTABLEBLUERIGHT");
				
				inputDisplay = inputDisplay + "\nTURN TABLE";
				inputDisplay = inputDisplay + "\nLeft Green  "+Wii.GetButton(whichRemote,"TURNTABLEGREENLEFT").ToString();
				inputDisplay = inputDisplay + "\nLeft Red    "+Wii.GetButton(whichRemote,"TURNTABLEREDLEFT").ToString();
				inputDisplay = inputDisplay + "\nLeft Blue   "+Wii.GetButton(whichRemote,"TURNTABLEBLUELEFT").ToString();
				inputDisplay = inputDisplay + "\nRight Green   "+Wii.GetButton(whichRemote,"TURNTABLEGREENRIGHT").ToString();
				inputDisplay = inputDisplay + "\nRight Red     "+Wii.GetButton(whichRemote,"TURNTABLEREDRIGHT").ToString();
				inputDisplay = inputDisplay + "\nRight Blue    "+Wii.GetButton(whichRemote,"TURNTABLEBLUERIGHT").ToString();
				inputDisplay = inputDisplay + "\nEuphoria    "+Wii.GetButton(whichRemote,"TURNTABLEEUPHORIA").ToString();																inputDisplay = inputDisplay + "\nPlus        "+Wii.GetButton(whichRemote,"TURNTABLEPLUS").ToString();
				inputDisplay = inputDisplay + "\nMinus       "+Wii.GetButton(whichRemote,"TURNTABLEMINUS").ToString();
				inputDisplay = inputDisplay + "\nStick       "+theStick.ToString("#.0000");
				inputDisplay = inputDisplay + "\nSlider      "+Wii.GetTurntableSlider(whichRemote);
				inputDisplay = inputDisplay + "\nDial        "+Wii.GetTurntableDial(whichRemote);
				inputDisplay = inputDisplay + "\nLeft Table  "+Wii.GetTurntableSpin(whichRemote,"LEFT");
				inputDisplay = inputDisplay + "\nRight Table "+Wii.GetTurntableSpin(whichRemote,"RIGHT");;
			}
			else if(Wii.GetExpType(whichRemote)==0)
			{
				setVisibility(nunchuk,false);
				setVisibility(balanceBoard,false);
				setVisibility(classic,false);
				setVisibility(guitarBody,false);	
				setVisibility(hub,false);
				setVisibility(drumSet,false);
			}
		}
		
		theText.text=inputDisplay;		
	}
	else
	{
		setVisibility(wiimote,false);
		setVisibility(nunchuk,false);
		setVisibility(balanceBoard,false);
		setVisibility(classic,false);
		setVisibility(guitarBody,false);	
		setVisibility(hub,false);
		setVisibility(drumSet,false);
		setVisibility(motionPlus,false);
		theRemoteNumber.enabled=false;
		theText.text = "Ready.";
	}
}