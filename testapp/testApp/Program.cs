using FreePIE.SpaceMice;

namespace testApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("SpaceMouse Test Application");


			// Create an instance of the SpaceMouseHID class
			try
			{
				//space navigator: 0x046D, 0xC626
				//space mouse pro: 0x046D, 0xC62B

				// copilot might have been making these up. I don't have these devices to test with.
				//space pilot: 0x046D, 0xC62D
				//space mouse wireless: 0x046D, 0xC62A
				//space mouse compact: 0x046D, 0xC62C


				SpaceMiceHID device = new SpaceMiceHID();
				if (!device.Initialize(0x046D, 0xC62B))
				{
					Console.WriteLine("Failed to initialize the SpaceMouse.");
					return;
				}

				Console.WriteLine("SpaceMouse initialized successfully.");

				Loop(device);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error initializing SpaceMouse: {ex.Message}");
				return;
			}

		}

		public static void Loop(SpaceMiceHID device)
		{
			Console.WriteLine("Press Ctrl+C to stop");
			Console.WriteLine("SpaceMouse data:");

			while (true)
			{
				// break out on Ctrl+C

				Console.WriteLine(string.Empty.PadRight(Console.BufferWidth, ' '));
				Console.WriteLine(string.Empty.PadRight(Console.BufferWidth, ' '));
				Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 2);
				Console.WriteLine($"X: {device.x}, Y: {device.y}, Z: {device.z} - Pitch: {device.pitch}, Yaw: {device.yaw}, Roll: {device.roll}");
				Console.WriteLine($"Btns: {(Convert.ToString(device.btns, 2).PadLeft(32, '0'))}");
				Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 2);


				//device.Update();

				// Print the values to the console
				//Console.WriteLine($"X: {device.x}, Y: {device.y}, Z: {device.z}");
				//Console.WriteLine($"RX: {device.rx}, RY: {device.ry}, RZ: {device.rz}");
				//Console.WriteLine($"Pitch: {device.pitch}, Yaw: {device.yaw}, Roll: {device.roll}");

				// Add a delay to avoid flooding the console
				System.Threading.Thread.Sleep(1);
			}
		}
	}
}
