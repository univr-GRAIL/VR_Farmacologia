
<div align="center">
<h1>Attentive Saliency and Photorealism in Immersive Virtual Environments</h1>

[**Deborah Pintani**](https://debbyx3.github.io)<sup>1*;</sup> · [**Giulia Benvegnù**](https://www.ddsp.univr.it/?ent=persona&id=39360&lang=en)<sup>2;</sup> · [**Federico Maria Lorusso**](mailto:federicomaria.lorusso@studenti.unipd.it)<sup>3</sup> · [**Cristiano Chiamulera**](https://www.ddsp.univr.it/?ent=persona&id=564)<sup>2</sup> · [**Andrea Giachetti**](https://www.dimi.univr.it/?ent=persona&id=4028)<sup>4</sup> · [**Ariel Caputo**](https://www.dimi.univr.it/?ent=persona&id=67990)<sup>4</sup> 

<sup>1</sup>Department of Computer Science, University of Verona, Italy&emsp;&emsp;&emsp;&emsp;<sup>2</sup>Department of Diagnostic and Public Health, University of Verona, Italy&emsp;&emsp;&emsp;&emsp;<sup>3</sup>University of Padua, Italy&emsp;&emsp;&emsp;&emsp; <sup>4</sup>Department of Engineering for Innovation Medicine, University of Verona, Italy&emsp;&emsp;&emsp;&emsp;

*corresponding author

[CHItaly2025](https://doi.org/10.1145/3750069.3750321)
</div>

<center><img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/teaser.jpg" alt="teaser"></center>

This repository contains the code and assets used in the study presented in the paper:
**[Attentive Saliency and Photorealism in Immersive Virtual Environments](https://doi.org/10.1145/3750069.3750321)**


## 🧠 Overview

The study explores how **rendering styles**—specifically **photorealism vs. non-photorealism**—affect **visual saliency** and the **sense of presence** in immersive virtual reality (VR) experiences. The research was conducted in the context of **junk food craving**, using head-mounted displays (HMDs) and tracking systems.

We also evaluate the potential of using **head tracking** as a proxy for **eye tracking** to simplify saliency measurement in VR experiments.

Project of the University of Verona between the Departement of Computer Science, Department of Engineering for Innovation Medicine (IntelliGO Labs, GRAIL division) and Department of Diagnostic and Public Health (NeuroPsychopharmacology - NeuroPsi Laboratory)

<center><img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/parkHQ.png" alt="ParkHQ" width="350"> <img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/coffeeShopHQ.png" alt="CoffeeShopHQ" width="350"></center>

Here is a summary of available environments and their functionalities

| Environment 				  | Type 		| Rendering quality & styles 	| Interactables | Fully working |
|-----------------------------|-------------|-------------------|---------------|---------------|
| Park 						  | Natural 	| Real (HQ), cartoon (LQ) 	| Burger, fries | ✅ |
| Coffee Shop (external area) | Man-made    | Real (HQ), cartoon (LQ) 	| Burger, fries | ✅ |
| Porch / Home garden 		  | Natural     | Real 				        | Cigarettes, pack of cigarettes | ❌ Fix teleport & eye tracking |
| Library 					  | Man-made    | Real 				        | Book, pen | ❌ Fix teleport & eye tracking |
| Church 					  | Man-made    | Real 				        | Breviary | ❌ Fix teleport & eye tracking |
| Clothing store 			  | Man-made    | Real 				        | Shopping basket with clothes | ❌ Fix teleport & eye tracking |

## Pre-requisites
* Windows 10/11
* Virtual Reality Headset HTC Vive Pro Eye
	* Vive Pro (not Eye) should be fine as well, but please note the eye tracking functionality will not be available
* Unity 2022.3.0f1 ([download here](https://unity.com/releases/editor/whats-new/2022.3.0#installs) the version)
	* Other versions are fine, as long as they are 2022.3.x
* Packages <ins>already installed</ins> in the project (here just for reference):
	* OpenVR XR Plugin for Unity
	* TobiiXR SDK
* [Blender](https://www.blender.org) 
	* Latest version is better 
* Steam and [SteamVR](https://store.steampowered.com/app/250820/SteamVR/)
* SRanipal (for eye tracking)
	* Not easy to install as a Standalone software
	* We recommend to install it via [Vive Console on Steam](https://store.steampowered.com/app/1635730/VIVE_Console_for_SteamVR/)
	* If you are encountering issues, please follow [this complete guide](https://docs.vrcft.io/docs/hardware/vr/vive/sranipal) 

## Get started  

Make sure to have all the pre-requisites listed above.

* Open the project with Unity Engine
* Open a scene in `Assets - Scenes`

### To test inside Unity Engine

* Connect headset to PC, open SteamVR and make sure headset is visible in SteamVR
	* In SteamVR `Settings - Developer`,  check if SteamVR is the current OpenXR Runtime
* Press Play inside Unity - enjoy!

### Build for Windows

[TODO, remember to explain URP quality]

## Navigation and Locomotion

Use [Vive Pro Controllers](https://www.vive.com/us/support/vive/category_howto/about-the-controllers.html)

### Teleporting

Point the right contoller towards the direction you want to teleport to, and hold down the Trackpad. 
A green circle and arc appear if the location allows teleporting. 
Release the Trackpad button to teleport there. 
If the location does not allow teleporting, the circle and arc will be red.

#### Cancel teleport

Squeeze the Grip button to cancel an initiated teleport action.

### Grab interactables

To grab an interactable, approach it and hold down the Trigger button. 
Release the Trigger to drop the item.
A white circle appears on top of the controller when the Trigger button is pushed.

## Logs

User actions are logged in log files every 0.2 seconds, that includes:
* Name of the scene
* User absolute starting position
* If the user teleported, where and their previous position
* User position relative to the starting one
* User absolute position
* Head orientation in degrees
* Head quaternion
* Right controller position
* Right controller rotation
* Right controller quaternion
* If the head is pointing toward an interactable
* If the user is gazing an interactable
* If the user is grabbing an interactable

## Rendering styles & quality

[TODO explain]
