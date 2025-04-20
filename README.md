# FreePIE.SpaceMice
A [FreePIE plugin](https://github.com/AndersMalmgren/FreePIE) for reading the 3DConnexion devices via HID.



Currently the plugin targets the SpaceMouse Pro and SpaceNavigator devices specifically `V: 0x046D P: 0xC62B` and `V: 0x046D P:0xC626`. 
I have enabled the Spacemouse Enterprise as well, but it is currently untested.

Supports polling the six motion axes, and up to 32 buttons.

Polling the device via HID bypasses the default 3DxWare driver completely, so configuring the device there won't have an effect on the reported values in freepie. This is necessary because the 3Dx driver only sends input to the window that is currently focused, so naturally that won't do for a FreePIE plug-in. 

Mind however, that buttons will likely be configured in the driver to do something by default, and the Plugin will read them regardless of what the driver has assigned to them. To avoid conflicts, you can either assign all buttons as `Disabled`  in the target application's (not FreePIE) 3dx profile, then use FreePIE to do your own button binding; or just bind them using the original driver itself.

# Installation

No easy-to-install method yet. You'll need an IDE to build the solution (Rider, VS or VSCode are all usable, pick your poison)

The FreePIE.SpaceMice.dll (out of /bin after building) file needs to be placed in FreePIE's /Plugins folder (in Program Files/FreePIE/plugins)
Additionally, the HIDSharp.dll needs to be placed next to FreePIe.exe. (this is part of the HIDSharp nuget package, but the HIDSharp.dll file is included in the repo now, for convenience) 

After doing this, FreePIE should contain a `spacemouse` class in the code. 

### Install Troubleshooting

If you get an error in FreePIE saying it could not find FreePIE.Core.Plugins.dll, try copying this dll file from FreePIE's install/plugins folder one level up to the root install (next to freepie.exe). 

# Usage

![image](https://github.com/user-attachments/assets/1b765ed2-0d45-4d85-9e12-a73a200e1747)

You can read any of the devices as shown. You can then use those for whatever your script needs. 

All values range from 0 to +/-350. 

For driving a vJoy device, you can scale the values like this: 
![image](https://github.com/user-attachments/assets/6b59ceb1-6690-4228-9896-d28d18ffc452)




# Other 3Dx Devices

For other 3D connection devices, I have added a list of all known 3DConnexion devices to Plugin.cs. It lists all device hardware IDs by name, along with an enable flag. 

![image](https://github.com/user-attachments/assets/ffbcb05b-4401-4cbb-9a10-e14f99e01270)

The plugin will attempt to access all of the enabled devices on the list. To enable an existing device, simply set the enabled flag to true. To add a new device, add a new entry for it in the list. The name on the entry is also the string key for the scripts. (script key lookup is case-insensitive)

I have not tested any other devices than the SpaceMouse Pro and original Space Navigator (wired) at the moment, but I am hoping the data coming out of these things is still formatted the same on the newer models. 


# Still pending

The last remaining feature I have pending for this plugin is setting up a FreePIE properties page to configure the devices. For now, you'll need to compile the plugin with the devices you want enabled in the code.

Lastly, support for multiple identical devices is not implemented. This is probably a very unlikely use case (even I'm not crazy enough to own multiple of the same spacemouse model), but it is not technically _impossible_ to support. The device search will grab the first HID device that matches the id, but it should be possible to add them all to the polling list. 


