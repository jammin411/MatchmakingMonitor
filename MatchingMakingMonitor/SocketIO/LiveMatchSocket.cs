using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;
using MatchMakingMonitor.Models;
using System.Collections.Generic;

namespace MatchMakingMonitor.SocketIO
{
	public class LiveMatchSocket
	{
		private readonly Socket _socket;

#pragma warning disable 169
		private ReplaySubject<object> _onTokenChanged;
#pragma warning restore 169
		private readonly ReplaySubject<object> _onStatsRequested;

		public LiveMatchSocket(Socket socket)
		{
			_socket = socket ?? throw new ArgumentNullException(nameof(socket));

			_onStatsRequested = new ReplaySubject<object>();
			_socket.On("statsRequested", () =>
			{
				_onStatsRequested.OnNext(null);
			});
		}

		public void SetToken(string token)
		{
			_socket.Emit("setToken", token);
		}

		public void SendStats(List<PlayerShip> stats)
		{
			_socket.Emit("sendStats", JsonConvert.SerializeObject(stats));
		}

		public IObservable<object> OnStatsRequested => _onStatsRequested.AsObservable();
	}
}
