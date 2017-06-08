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
		private readonly ReplaySubject<object> _onPlayersRequested;
		private readonly ReplaySubject<object> _onSettingsRequested;
		private readonly Socket _socket;

#pragma warning disable 169
		private ReplaySubject<object> _onTokenChanged;
#pragma warning restore 169

		public LiveMatchSocket(Socket socket)
		{
			_socket = socket ?? throw new ArgumentNullException(nameof(socket));

			_onPlayersRequested = new ReplaySubject<object>();
			_onSettingsRequested = new ReplaySubject<object>();

			_socket.On("playersRequested", () => { _onPlayersRequested.OnNext(null); });
			_socket.On("settingsRequested", () => { _onSettingsRequested.OnNext(null); });
		}

		public IObservable<object> OnPlayersRequested => _onPlayersRequested.AsObservable();
		public IObservable<object> OnSettingsRequested => _onSettingsRequested.AsObservable();

		public void SetToken(string token)
		{
			_socket.Emit("setToken", token);
		}

		public void SendPlayers(List<MobilePlayerStats> players)
		{
			_socket.Emit("sendPlayers", JsonConvert.SerializeObject(players));
		}

		public void SendColorKeys(List<MobileColorKeys> colorKeys)
		{
			_socket.Emit("sendColorKeys", JsonConvert.SerializeObject(colorKeys));
		}

		public void SendSettings(Dictionary<string, object> settings)
		{
			_socket.Emit("sendSettings", JsonConvert.SerializeObject(settings));
		}
	}
}