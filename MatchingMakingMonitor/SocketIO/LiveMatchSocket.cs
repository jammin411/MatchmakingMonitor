using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MatchMakingMonitor.Models;
using Newtonsoft.Json;
using Quobject.SocketIoClientDotNet.Client;

namespace MatchMakingMonitor.SocketIO
{
	public class LiveMatchSocket
	{
		private readonly ReplaySubject<object> _onStatsRequested;
		private readonly Socket _socket;

#pragma warning disable 169
		private ReplaySubject<object> _onTokenChanged;
#pragma warning restore 169

		public LiveMatchSocket(Socket socket)
		{
			_socket = socket ?? throw new ArgumentNullException(nameof(socket));

			_onStatsRequested = new ReplaySubject<object>();
			_socket.On("statsRequested", () => { _onStatsRequested.OnNext(null); });
		}

		public IObservable<object> OnStatsRequested => _onStatsRequested.AsObservable();

		public void SetToken(string token)
		{
			_socket.Emit("setToken", token);
		}

		public void SendStats(List<PlayerShip> stats)
		{
			_socket.Emit("sendStats", JsonConvert.SerializeObject(stats));
		}
	}
}