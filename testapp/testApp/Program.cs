using FreePIE.SpaceMice;

namespace testApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("SpaceMice Test Application");


			// Create an instance of the SpaceMouseHID class
			try
			{
				//space navigator: 0x046D, 0xC626
				//space mouse pro: 0x046D, 0xC62B

				SpaceMiceHID device = new SpaceMiceHID()
				{
					VendorId = 0x046D,
					ProductId = 0xC62B,
					DeviceName = "SpaceMouse Pro"
				};
				if (!device.Initialize())
				{
					Console.WriteLine($"Failed to initialize the {device.DeviceName}.");
					return;
				}

				Console.WriteLine($"{device.DeviceName} initialized successfully.");

				Loop(device);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error initializing device: {ex.Message}");
				return;
			}

		}

		public static void Loop(SpaceMiceHID device)
		{
			Console.WriteLine("Press Ctrl+C to stop");
			Console.WriteLine($"{device.DeviceName} data:");

			while (true)
			{
				// break out on Ctrl+C

				Console.WriteLine(string.Empty.PadRight(Console.BufferWidth, ' '));
				Console.WriteLine(string.Empty.PadRight(Console.BufferWidth, ' '));
				Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 2);
				Console.WriteLine($"X: {device.x}, Y: {device.y}, Z: {device.z} - Pitch: {device.pitch}, Yaw: {device.yaw}, Roll: {device.roll}");
				Console.WriteLine($"Btns: {(Convert.ToString(device.btns, 2).PadLeft(32, '0'))}");
				Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 2);

				System.Threading.Thread.Sleep(1);
			}
		}
	}
}
