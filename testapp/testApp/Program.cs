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
				SpaceMouseHID device = new SpaceMouseHID();
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

		public static void Loop(SpaceMouseHID device)
		{
			Console.WriteLine("Press Ctrl+C to stop");
			Console.WriteLine("SpaceMouse data:");

			while (true)
			{
				// break out on Ctrl+C

				Console.WriteLine("                                                                      ");
				Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 1);
				Console.WriteLine($"X: {device.x}, Y: {device.y}, Z: {device.z} - Pitch: {device.pitch}, Yaw: {device.yaw}, Roll: {device.roll}");
				Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 1);


				device.Update();

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
