
using FreePIE.Core.Contracts;
using FreePIE.Core.Plugins.Globals;
using FreePIE.SpaceMice;

using System;
using System.Collections.Generic;
using System.Linq;

//using TDx.TDxInput;


[GlobalType(Type = typeof(TDxPluginGlobal), IsIndexed = true)]
public class TDxPlugin : IPlugin
{

	public List<SpaceMiceHID> devices;
	protected bool running;
	public bool Active => running;

	public double x;
	public double y;
	public double z;

	public double rx;
	public double ry;
	public double rz;
	public double ra;

	public double pitch;
	public double yaw;
	public double roll;

	public uint btns;

	public Dictionary<string, (int vid, int pid, bool enable)> supportedDevices = new Dictionary<string, (int vid, int pid, bool enable)>
	{
		// tested
		{ "SpaceNavigator", (0x046D, 0xC626, true)},
		{ "SpaceMouse Pro", (0x046D, 0xC62B, true)},
		
		// later devices (3dx vid)
		{ "CadMouse Pro Wireless Left", (0x256F, 0xC657, false)},
		{ "Universal Receiver", (0x256F, 0xC652, false)},
		{ "SpaceMouse Compact", (0x256F, 0xC635, false)},
		{ "CadMouse Wireless", (0x256F, 0xC651, false)},
		{ "SpaceMouse Pro Wireless Receiver", (0x256F, 0xC632, false)},
		{ "SpaceMouse Pro Wireless (cabled)", (0x256F, 0xC631, false)},
		{ "SpaceMouse Enterprise", (0x256F, 0xC633, true)},
		{ "SpaceMouse Wireless Receiver", (0x256F, 0xC62F, false)},
		{ "SpaceMouse Wireless (cabled)", (0x256F, 0xC62E, false)},

		// legacy devices (logitech vid)
		{ "SpaceNavigator Notebook", (0x046D, 0xC628, false)},
		{ "SpacePilot Pro", (0x046D, 0xC629, false)},
		{ "SpacePilot", (0x046D, 0xC625, false)},
		{ "SpaceExplorer", (0x046D, 0xC627, false)},
		{ "Spacemouse", (0x046D, 0xC606, false)},
		{ "Spaceball 5000", (0x046D, 0x621, false)},
		{ "SpaceTraveller", (0x046D, 0xC623, false)},
		{ "Spacemouse Plus XT", (0x046D, 0x603, false)},
		{ "CADMan", (0x046D, 0x605, false)},

	};


	public object CreateGlobal()
	{
		// try creating all devices, the ones we don't have should fail silently 
		devices = new List<SpaceMiceHID>(supportedDevices.Keys.Count);
		foreach (var device in supportedDevices)
		{
			if (!device.Value.enable) continue;

			var d = new SpaceMiceHID()
			{
				DeviceName = device.Key,
				VendorId = device.Value.vid,
				ProductId = device.Value.pid,
			};

			if (!d.TryGetDevice(out var _))
			{
				Console.WriteLine($"Failed to find the {device.Key}.");
				continue;
			}
			devices.Add(d);
		}


		return new GlobalIndexer<SpaceMiceHID, int, string>
		(
			intIndex => devices.ElementAtOrDefault(intIndex),
			(strIndex, idx) =>
			{
				var device = devices.FirstOrDefault(d => string.Equals(d.DeviceName, strIndex, StringComparison.InvariantCultureIgnoreCase));

				if (device != null) return device;
				else return devices.ElementAtOrDefault(idx);
			}
		);
	}

	public Action Start()
	{

		foreach (var device in devices)
		{
			if (!device.Initialize())
				Console.WriteLine($"Failed to initialize the {device.DeviceName}.");
		}

		return null;
	}

	public void Stop()
	{
		foreach (var device in devices)
		{
			if (device.active)
				device.Close();
		}
		GC.Collect();
	}

	public event EventHandler Started;

	public string FriendlyName
	{
		get { return "3DConnexion HID"; }
	}

	public bool GetProperty(int index, IPluginProperty property)
	{
		return false;
	}

	public bool SetProperties(Dictionary<string, object> properties)
	{
		return false;
	}

	public void DoBeforeNextExecute()
	{
	}


}



[Global(Name = "spacemice")]
public class TDxPluginGlobal
{
	private readonly SpaceMiceHID tdx;


	public TDxPluginGlobal(SpaceMiceHID tdx)
	{
		this.tdx = tdx;
	}

	public string DeviceName => tdx.DeviceName;

	public double x => tdx.x;
	public double y => tdx.y;
	public double z => tdx.z;


	public double pitch => tdx.pitch;
	public double yaw => tdx.yaw;
	public double roll => tdx.roll;

	public bool getButton(int btn) => tdx.getButton(btn);

}
