INTRODUCTION

Hello! Thankyou for buying WiiBuddy for Unity! This product is the result of a student project that helped me realize how unnecessarily difficult it has been to develop a simple prototype using Wii® controls. So I've spent more than a few months putting together everything I knew and everything I could learn about connecting to Wii® controllers via bluetooth into this here plugin. Hopefully, you and many others like you will now be able to easily create prototypes and proof-of-concepts, before having to make the daunting commitment to an official Nintendo dev-kit (and I certainly hope your pojects make it that far). And so I wish you the best of luck in your endeavours. Go forth and conquer!

-Greg

Btw, BitLegit is in no way affiliated with Nintendo.

You can contact us at bitlegit@gmail.com
or check out our website at www.bitlegit.net
where you can find our entire API online.

========================================================================================================

HOW TO CONNECT WIIMOTES 

1. Begin the search by calling the StartSearch() function. The search will last approximately 20 seconds.

2. During that time, press the red sync button located on your wiimote, next to the batteries. With older remotes (bluetooth devices named "Nintendo RVL-CNT-01") you can also press the "1" and "2" simultaneously, but with newer remotes (bluetooth devices named "Nintendo RVL-CNT-01-TR") pressing "1" and "2" together will result in a false connection. Obviously with balance boards, you have to use the red button. Personally, I think the red button is always more reliable anyways.

3. If the connection succeeds, the remote will automatically set its LEDs accordingly and you will be able to access its input through the API. A message will also be broadcast by the Wii prefab if it is in the scene.

- If no remote is found is found before the search ends, the discoveryStatus will become -100 and that discovery error will be broadcast.

- If a remote is found but the connection fails, the discovery status will become -536870195 and that discovery error will be broadcast. This happens from time to time and doesn't necessarily mean you did anything wrong. So just try again. If the problem persists, open up your computer's bluetooth preferences and remove the remote from the list of familiar devices. This pretty much results in a 100% connection rate.

- With the newer remotes, there's still a chance that the remote will disconnect immediately after connecting. This results in a false connection, where WiiBuddy will think the remote is connected for a few seconds before dropping it automatically. These tend to sort themselves out, and again, if the problem persists, just remove the Wii® remote from your computer's list of familiar bluetooth devices.

- During tests the connection rate has been fairly reliable. I plan on making it more so with future updates.

4. When you're done with the remote, call DropWiiRemote(int) to disconnect it from your computer. Whilst using the editor, your remotes will stay connected regardless of whether you start or stop playing the current scene. When closing the standalone app of your project, all remotes will automatically disconnect. This was also the case for closing the Unity editor until I upgraded my OS to Mountain Lion. So I imagine all Mountain Lion users will just have to disconnect their wiimotes either before closing Unity or afterwards, using the power button on the remote.

- A total of 16 remotes are allowed to be connected at once. This is for no reason other than that's how many variations there are for the wiimote's LED lights. If you want to set it to a LOWER number, there is a variable in Wii.cs called "max" that you can change.

========================================================================================================

HOW TO USE WII PREFAB

- The Wii prefab comes with a custom inspector, which is capable of satisfying all of your connecting and disconnecting needs whilst you work within Unity.

- Having the Wii prefab in your scene will also allow it to do things like automatically check for Motion Plus, as well as automatically calibrate Motion Plus.

- The inspector will also display the controllers' input as it comes in. By default, it will only update OnInspectorGUI(), but if you select "rapid update", it will show input in real time.

- You can also command remotes to rumble (just cuz).

- If you want to test your game without connecting a remote, you can also use the inspector to add virtual remotes, which will let you create virtual input. I recommend you make liberal use of the pause-scene button if you want to make quick changes to virtual input.

========================================================================================================

TESTED COMPATIBLE CONTROLLERS
- Wii® Remote ("Nintendo RVL-CNT-01")
- Wii® Remote Plus ("Nintendo RVL-CNT-01")
- Wii® Remote Plus ("Nintendo RVL-CNT-01-TR")
- Motion Plus
- Nunchuck
- Nunchuck + Motion Plus
- Classic Controller
- Classic Controller Pro
- Classic + Motion Plus
- Balance Board
- Guitar Hero® Guitar
- Guitar Hero® Drumset
- DJ Hero® Turntable

========================================================================================================

HOW TO SCRIPT:

JavaScript

	0. Make sure your computer's bluetooth is on.

	1. Place an instance of the "Wii" prefab in your scene.

	2. Access The Object's "Wii.cs" component to utilize its functions.

	3. Children of this object will receive messages broadcast from it (having to do with remote discovery)

	4. Open up your heart.

C#

	0. Make sure your computer's bluetooth is on.

	1. Regardless of whether or not you've placed the "Wii" prefab in your scene, you can access the Wii class's functions (i.e. Wii.FunctionName() )

	2. If you want to receive the Wii class's broadcast message, you'll still need to place the "Wii" prefab into your scene as its children will be the ones receiving the messages.

	3. Let the light shine in.

========================================================================================================

HOW TO DEAL WITH MOTION PLUS

1. Unlike other extensions, Wii Motion Plus® is not automatically detected by the remote.

2. You can either keep the Wii prefab in your scene, which will automatically check for Motion Plus every five seconds, or you can use CheckForMotionPlus(int) to check yourself. 

2.5 If you want the prefab in your scene but don't want it automatically detecting Motion Plus, you can change this with ShouldAutomaticallyCheckForMotionPlus(bool) at Start

3. Once Motion Plus is detected, you can either let the Wii prefab calibrate it automatically, by simply keeping the prefab in your scene and placing the remote face down on a flat surface for a few seconds… or you can use GetRawMotionPlus(int) and CalibrateMotionPlus(int,Vector3) to calibrate it yourself.

3.5 If you want the prefab in your scene but don't want it automatically calibrating Motion Plus, you can change this with ShouldAutomaticallyCalibrateMotionPlus(bool) at Start
