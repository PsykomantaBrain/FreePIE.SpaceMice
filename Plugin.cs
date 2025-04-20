
using FreePIE.Core.Contracts;
using FreePIE.SpaceMice;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

//using TDx.TDxInput;


[GlobalType(Type = typeof(TDxPluginGlobal))]
public class TDxPlugin : IPlugin
{

	public SpaceMouseHID device;
	protected bool running;

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


	protected TDxPluginGlobal tdx;
	public object CreateGlobal()
	{
		tdx = new TDxPluginGlobal(this);
		device = new SpaceMouseHID();
		return tdx;
	}

	public Action Start()
	{
		Connect();


		x = y = z = 0;
		pitch = yaw = roll = 0;

		return null;
	}

	private void Connect()
	{
		if (!device.Initialize(0x046D, 0xC62B))
		{
			Console.WriteLine("Failed to initialize the SpaceMouse.");
			return;
		}
		running = true;
	}


	public void Disconnect()
	{
		if (device != null)
		{
			device.Close();
			device = null;
		}
		running = false;
	}

	public void Stop()
	{
		x = y = z = 0;
		pitch = yaw = roll = 0;

		try
		{
			Disconnect();
			GC.Collect();
		}
		catch (COMException ex)
		{
			Console.WriteLine(ex.ToString());
		}
	}

	public event EventHandler Started;

	public string FriendlyName
	{
		get { return "3DConnexion SpaceMouse"; }
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
		if (device != null & running)
		{
			//device.Update();
			x = device.x;
			y = device.y;
			z = device.z;

			pitch = device.pitch;
			yaw = device.yaw;
			roll = device.roll;
		}
		else
		{
			x = y = z = 0;
			pitch = yaw = roll = 0;
		}
	}

	internal void OnDispose()
	{
		running = false;
		GC.SuppressFinalize(this);
	}

}



[Global(Name = "spacemouse")]
public class TDxPluginGlobal : IDisposable
{
	private readonly TDxPlugin tdx;


	public TDxPluginGlobal(TDxPlugin tdx)
	{
		this.tdx = tdx;
	}


	public double x => tdx.x;
	public double y => tdx.y;
	public double z => tdx.z;


	public double pitch => tdx.pitch;
	public double yaw => tdx.yaw;
	public double roll => tdx.roll;


	void IDisposable.Dispose()
	{
		tdx.OnDispose();
	}
}
