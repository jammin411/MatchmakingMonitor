using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace MatchMakingMonitor.SocketIO
{
	public class SocketIoService
	{
		public LiveMatchSocket Hub;

		public IObservable<ConnectionState> StateChanged => _stateChanged.AsObservable();

		private readonly ReplaySubject<ConnectionState> _stateChanged;


		public ConnectionState ConnectionState { get; private set; }

		private readonly Socket _socketConnection;

		public SocketIoService()
		{
			_socketConnection = IO.Socket("http://localhost:4000", new IO.Options() { AutoConnect = false });
			Hub = new LiveMatchSocket(_socketConnection);

			_stateChanged = new ReplaySubject<ConnectionState>();

			_socketConnection.On(Socket.EVENT_CONNECT, () =>
			{
				ConnectionState = ConnectionState.Connected;
				_stateChanged.OnNext(ConnectionState);
			});

			_socketConnection.On(Socket.EVENT_DISCONNECT, () =>
			{
				ConnectionState = ConnectionState.Disconnected;
				_stateChanged.OnNext(ConnectionState);
			});

			_socketConnection.On(Socket.EVENT_RECONNECTING, () =>
			{
				ConnectionState = ConnectionState.Reconnecting;
				_stateChanged.OnNext(ConnectionState);
			});

			_socketConnection.On(Socket.EVENT_RECONNECT, () =>
			{
				ConnectionState = ConnectionState.Connected;
				_stateChanged.OnNext(ConnectionState);
			});
		}

		public void Connect()
		{
			ConnectionState = ConnectionState.Connecting;
			_stateChanged.OnNext(ConnectionState);
			_socketConnection.Connect();
		}

		public void Disconnect()
		{
			_socketConnection.Disconnect();
		}
		
	}

	public enum ConnectionState
	{
		Connecting,
		Reconnecting,
		Disconnected,
		Connected
	}
}
