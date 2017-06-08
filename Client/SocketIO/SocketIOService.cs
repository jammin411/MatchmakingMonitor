using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Quobject.SocketIoClientDotNet.Client;

namespace MatchMakingMonitor.SocketIO
{
	public class SocketIOService
	{
		private readonly Socket _socketConnection;

		private readonly ReplaySubject<ConnectionState> _stateChanged;
		public LiveMatchSocket Hub;

		public SocketIOService()
		{
			_socketConnection = IO.Socket("http://localhost:4000", new IO.Options {AutoConnect = false});
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

		public IObservable<ConnectionState> StateChanged => _stateChanged.AsObservable();


		public ConnectionState ConnectionState { get; private set; }

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