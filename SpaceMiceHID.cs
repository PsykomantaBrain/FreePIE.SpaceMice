using HidSharp;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FreePIE.SpaceMice
{
	public class SpaceMiceHID : IDisposable
	{
		public int VendorId;
		public int ProductId;
		public string DeviceName;
		public bool active;

		private HidDevice _device;
		private HidStream _stream;
		protected byte[] data;

		public double x;
		public double y;
		public double z;

		public double pitch;
		public double yaw;
		public double roll;

		// there are up to 4 bytes of buttons on the button report. We pack them into a single uint 32 here
		public uint btns;

		public bool TryGetDevice(out HidDevice device)
		{
			var deviceList = DeviceList.Local;
			device = deviceList.GetHidDevices(VendorId, ProductId).FirstOrDefault();

			return device != null;
		}

		public bool Initialize()
		{
			if (!TryGetDevice(out _device))
			{
				active = false;
				return false;
			}

			if (_device.TryOpen(out _stream))
			{
				data = new byte[_device.GetMaxInputReportLength()];

				// launch an async task to read the data in a loop
				Task.Run(() =>
				{
					_stream.ReadTimeout = Timeout.Infinite;
					while (_stream != null)
					{
						Update();

						Task.Yield(); // Yield to avoid busy waiting
					}
				});
			}

			active = _stream != null;
			return _stream != null;
		}

		private int ReadInput()
		{
			if (_stream == null) return 0;

			int bytesRead;
			try
			{
				bytesRead = _stream.Read(data, 0, data.Length);
				return bytesRead;
			}
			catch (Exception ex)
			{
				// this sucks, but apparently is the way to do it. If the device isn't being moved, it won't send data and reading will timeout.				
				return 0;
			}

		}

		private void Update()
		{
			if (_stream == null) return;

			if (!_stream.CanRead) return;

			int bytesRead = ReadInput();
			if (bytesRead > 0)
			{
				switch (data[0]) // Translation axes
				{
					case 0x01: // translation
					{
						short xTrans = (short)(data[1] | (data[2] << 8));
						short yTrans = (short)(data[3] | (data[4] << 8));
						short zTrans = (short)(data[5] | (data[6] << 8));

						x = xTrans;
						y = yTrans;
						z = zTrans;
						break;
					}

					case 0x02: // rotation
					{
						short pitch = (short)(data[1] | (data[2] << 8));
						short roll = (short)(data[3] | (data[4] << 8));
						short yaw = (short)(data[5] | (data[6] << 8));

						this.pitch = pitch;
						this.roll = roll;
						this.yaw = yaw;
						break;
					}

					case 0x03: // buttons
					{
						// bitmask 1
						btns = (uint)(data[1] | (data[2] << 8) | (data[3] << 16) | (data[4] << 24));

						break;
					}
				}
			}
		}

		public bool getButton(int btn)
		{
			return (btns & (1 << btn)) != 0;
		}

		public void Close()
		{
			_stream?.Dispose();
			_stream = null;
			data = null;

			active = false;
		}

		void IDisposable.Dispose()
		{
			Close();
		}


	}

}
