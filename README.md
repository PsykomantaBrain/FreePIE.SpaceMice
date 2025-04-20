# FreePIE.SpaceMice
A [FreePIE plugin](https://github.com/AndersMalmgren/FreePIE) for reading the 3DConnexion SpaceMouse device.



Currently the plugin targets the SpaceMouse Pro device specifically `V: 0x046D P: 0xC62B`. 
I have tested the original SpaceNavigator as well `V: 0x046D P:0xC626`, which also worked fine.

Support currently includes polling the six motion axes, and up to 32 buttons.

Polling the device via HID bypasses the default 3DxWare driver completely, so configuring the device there won't have an effect on the reported values in freepie. This is necessary because the 3Dx driver only sends input to the window that is currently focused, so naturally that won't do for a FreePIE plug-in. 

Mind however, that buttons will likely be configured in the driver to do something by default, and the Plugin will read them regardless of what the driver has assigned to them. To avoid conflicts, you can either assign all buttons as `Disabled`  in the target application's (not FreePIE) 3dx profile, then use FreePIE to do your own button binding; or just bind them using the original driver itself.

# Installation

No easy-to-install method yet. You'll need an IDE to build the solution (Rider, VS or VSCode are all usable, pick your poison)

The FreePIE.SpaceMice.dll (out of /bin after building) file needs to be placed in FreePIE's /Plugins folder (in Program Files/FreePIE/plugins)
Additionally, the HIDSharp.dll needs to be placed next to FreePIe.exe. (this is part of the HIDSharp nuget package)

After doing this, FreePIE should contain a `spacemouse` class in the code. 

# Usage

![image](https://github.com/user-attachments/assets/3199b3f4-95c8-4e2e-8fd5-b95ade654485)

You can read any of the spacemouse's 6 axes as shown. You can then use those for whatever your script needs. 

All values range from 0 to +/-350. 

For driving a vJoy device, you can scale the values like this: 

![image](https://github.com/user-attachments/assets/a852a57e-52a0-4782-8c5c-9e2419380b16)



# Other 3Dx Devices

For other 3D connection devices, you should be able to adapt the SpaceMouse HID vendor/product IDs (Plugin.cs, line 54). Change them for whatever your device values are. You can find those in the windows device manager, on the driver details page.

I have not tested any other devices than the SpaceMouse Pro and original Space Navigator (wired) at the moment, but I am hoping the data coming out of these things is still formatted the same on the newer models. 

Here are the VID/PID values I've been able to verify so far, as well as some others I've found (but can't validate as I don't own the devices):
```
// tested --------------------------------
SpaceMouse Pro        V: 0x046D P: 0xC62B
SpaceNavigator        V: 0x046D P: 0xC626

// untested ------------------------------
SpacePilot            V: 0x046D P: 0xC62D
SpaceMouse Wireless   V: 0x046D P: 0xC62A
SpaceMouse Compact:   V: 0x046D P: 0xC62C

```

If you can test and validate any other devices, let me know and I'll happily add them to the list. 
(and add the option when I get around to implementing multiple device support)

