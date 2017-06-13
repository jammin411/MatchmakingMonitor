using System;
using Quobject.SocketIoClientDotNet.Client;

namespace TestSocketApp
{
	internal class Program
	{
		private static void Main()
		{
			var socket = IO.Socket("http://localhost:4000", new IO.Options { AutoConnect = false });

			socket.On(Socket.EVENT_CONNECT, () =>
			{
				Console.WriteLine("Connected");
				socket.Emit("setToken", "fa0531ba-4223-4149-9d5c-039c5f293619");
			});

			socket.On("tokenChanged", () => { Console.WriteLine("tokenChanged"); });

			socket.On("statsReceived", stats => { Console.WriteLine("statsReceived"); });

			socket.On("settingsReceived", stats => { Console.WriteLine("settingsReceived"); });

			socket.Connect();

			var closed = false;
			while (!closed)
			{
				var key = Console.ReadLine();
				if (key == "c")
					closed = true;

				if (key == "1")
					socket.Emit("requestStats");

				if (key == "2")
					socket.Emit("requestSettings");
			}
		}
	}
}