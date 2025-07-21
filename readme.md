
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


##  Overview

The study explores how **rendering styles**—specifically **photorealism vs. non-photorealism**—affect **visual saliency** and the **sense of presence** in immersive virtual reality (VR) experiences. The research was conducted in the context of **junk food craving**, using head-mounted displays (HMDs) and tracking systems.

We also evaluate the potential of using **head tracking** as a proxy for **eye tracking** to simplify saliency measurement in VR experiments.

Project of the University of Verona between the Departement of Computer Science, Department of Engineering for Innovation Medicine (IntelliGO Labs, GRAIL division) and Department of Diagnostic and Public Health (NeuroPsychopharmacology - NeuroPsi Laboratory)

<div align="center"><img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/parkHQ.png" alt="ParkHQ" width="400"> <img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/coffeeShopHQ.png" alt="CoffeeShopHQ" width="400"></div>

Here is a summary of available environments and their functionalities:

| Environment 				  | Type 		| Rendering quality & styles 	| Interactables | Fully working |
|-----------------------------|-------------|-------------------|---------------|---------------|
| Park 						  | Natural 	| Real (HQ), cartoon (LQ) 	| Burger, fries | ✅ |
| Coffee Shop (external area) | Man-made    | Real (HQ), cartoon (LQ) 	| Burger, fries | ✅ |

Additionally, we included other environments that were not used in the study but are provided here for completeness:

| Environment 				  | Type 		| Rendering quality & styles 	| Interactables | Fully working |
|-----------------------------|-------------|-------------------|---------------|---------------
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

Each environment comes in two different Unity scene file:
* Cafe HR and Cafe LR
* Park HR and Park LR

### ‼️​ Please note ‼️
Due to repository size limitations, the pre-baked lighting assets couldn't be included.  
Please re-bake the lights locally to experience the scenes as intended.

### To test inside Unity Engine

* Connect headset to PC, open SteamVR and make sure headset is visible in SteamVR
	* In SteamVR `Settings - Developer`,  check if SteamVR is the current OpenXR Runtime
* Press Play inside Unity - enjoy!

### Build for Windows

When building the project into an `.exe`, make sure to select the correct **URP (Universal Render Pipeline)** quality preset. This will ensure the scene is rendered with the intended visual fidelity.

#### Change URP Quality Preset (e.g., from High to Low)

1.  Go to:  
    `Edit → Project Settings → Quality`    
2.  Under the **Quality Levels**, check only the boxes corresponding to your target preset:    
    -   For **Low Quality (LR)**, enable only the `Low` option and **disable** all others (e.g., `Ultra`)        
    -   For **High Quality (HR)**, enable only the `Ultra` option
        
    Example screenshots:  
    <img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/quality low.png" alt="quality low">  
    <img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/quality high.png" alt="quality high">

#### Build the Project

*  Open:  
    `File → Build Settings...`    
*  Click **Add Open Scenes**  
    → Make sure your current scene is selected.    
* Set the platform:    
    -   Select Windows, Mac, Linux       
    -   Target Platform: Windows        
    -   Architecture: Intel 64-bit        
    -   Uncheck any other build options        
    -  Compression Method: Default
*  Click Build and choose your destination folder.    


#### Run the Build with VR

*  Connect your VR headset to the PC.
*  Open **SteamVR**, and ensure your headset is visible and tracked.
*  In SteamVR:
    -   Go to `Settings → Developer`
    -   Make sure **SteamVR is set as the current OpenXR Runtime**
* Finally, **run the `.exe` you just built** and enjoy the scene!

## Interactables

For the study, we defined two special, interactable food items: a **burger** and a **tray of fries**. 
These were the only objects tracked for user attention (via gaze and head direction) and could be grabbed as whole items. 
Both were modeled in 3D based on validated images from the FoodCast Research Image Database (FRIDa).

<div align="center"><img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/foodcues.jpg" alt="FoodCues"></div>

> (a)(b)  Calibrated and standardized food-related stimuli from FRIDa
> 
> (c)(d)  Renderings of the corresponding 3D models inserted in our environments

## Navigation and Locomotion

<div align="center"><img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/vive_controller_lineart_color.jpg" alt="ControllerLineart" width="200"></div>

Use [Vive Pro Controllers](https://www.vive.com/us/support/vive/category_howto/about-the-controllers.html)

### Teleporting

Point the right contoller towards the direction you want to teleport to, and hold down the Trackpad (blue in figure). 
A green circle and arc appear if the location allows teleporting. 
Release the Trackpad button to teleport there. 
If the location does not allow teleporting, the circle and arc will be red.

#### Cancel teleport

Squeeze the Grip button (green in figure) to cancel an initiated teleport action.

### Grab interactables

To grab an interactable, approach it and hold down the Trigger button (red in figure). 
Release the Trigger to drop the item.
A white circle appears on top of the controller when the Trigger button is pushed.

## Rendering styles & quality

Each VR scene is available in two versions:

-   **High Realism (HR)**: photorealistic rendering    
-   **Low Realism (LR)**: stylized, cartoon-like rendering
    
Both versions are built with **Unity's Universal Render Pipeline (URP)**. Rather than tweaking individual parameters, we defined two consistent visual styles to compare their effect on user attention.

<div align="center"><img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/HRhighlights.jpg" alt="HR highlights" width="400"> <img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/LRhighlights.jpg" alt="LR highlights" width="400"></div>

> 1st image: High Realism Cafè scene 
> 
> 2nd image: Low Realism Cafè scene

### High Realism (HR)

The HR version uses:

-   2K textures    
-   High-resolution soft shadows for natural lighting    
-   Post-processing with [ReShade](https://github.com/crosire/reshade):    
    -   Simulated HDR (tone mapping for natural light range)        
    -   Bloom effects for glowing highlights        

These choices aim to create a believable and immersive environment.

### Low Realism (LR)

The LR version simplifies the rendering:
-   Textures at 1/8 resolution    
-   Only hard shadows    
-   Stylized post-processing with [ReShade](https://github.com/crosire/reshade):    
    -   Black outlines        
    -   Surface blur        
    -   +20% saturation        
    -   Color quantization (16 brightness levels)        

This results in a flatter, more cartoonish look.

Both versions include baked lightmaps, ambient occlusion, and light probes near interactive elements to ensure consistent lighting.

<div align="center"><img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/barHQScreen1.jpg" alt="Cafè high realism" width="400"> <img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/barLQScreen1.jpg" alt="Cafè low realism" width="400"></div>

> 1st image: Cafè High Realism 
> 
> 2nd image: Cafè Low Realism

<div align="center"><img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/parkHQScreen1.jpg" alt="Park high realism" width="400"> <img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/parkLQScreen1.jpg" alt="Park low realism" width="400"></div>

> 1st image: Park High Realism 
> 
> 2nd image: Park Low Realism

## Quality settings

### High Realism 

#### Lights
<div align="center"><img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/HR light cafe.png" alt="hr light cafe" width="400"> <img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/HR light park.png" alt="hr light park" width="400"></div>

> 1st image: HR Cafè
> 
> 2nd image: HR Park

#### URP Settings

<div align="center"><img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/HR urp 1.jpg" alt="hr urp 1" width="400"> <img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/HR urp 2.jpg" alt="hr urp 2" width="400"></div>

### Low Realism 

#### Lights
<div align="center"><img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/LR light cafe.jpg" alt="lr light cafe" width="400"> <img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/LR light park.jpg" alt="lr light park" width="400"></div>

> 1st image: LR Cafè
> 
> 2nd image: LR Park

#### URP Settings

<div align="center"><img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/LR urp 1.jpg" alt="lr urp 1" width="400"> <img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/LR urp 2.jpg" alt="lr urp 2" width="400"></div>

### Light baking
As a reference, here are baking settings used for both HR and LR scenes:

<div align="center"><img src="https://github.com/univr-GRAIL/VR_Farmacologia/blob/main/Assets/Screenshots/bake farma.jpg" alt="lights baking settings" width="400"></div>

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
