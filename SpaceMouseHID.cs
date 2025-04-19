using HidSharp;

using System;
using System.Linq;

namespace FreePIE.SpaceMice
{
	public class SpaceMouseHID
	{

		private HidDevice _device;
		private HidStream _stream;
		protected byte[] data;

		public double x;
		public double y;
		public double z;

		public double pitch;
		public double yaw;
		public double roll;


		public bool Initialize(int vendorId, int productId)
		{
			var deviceList = DeviceList.Local;
			_device = deviceList.GetHidDevices(vendorId, productId).FirstOrDefault();

			if (_device == null)
				return false;

			_device.TryOpen(out _stream);
			_stream.ReadTimeout = 100; // Set a read timeout of 1 second
			data = new byte[_device.GetMaxInputReportLength()];

			return _stream != null;
		}

		public int ReadInput()
		{
			if (_stream == null) return 0;

			int bytesRead;
			try
			{
				bytesRead = _stream.Read(data, 0, data.Length);
				return bytesRead;
			}
			catch (TimeoutException ex)
			{
				// this sucks, but apparently is the way to do it. If the device isn't being moved, it won't send data and reading will timeout.				
				return 0;
			}

		}

		public void Update()
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
						// Handle button presses here
						break;
					}
				}
			}
		}

		public void Close()
		{
			_stream?.Dispose();
			data = null;
		}
	}

}
