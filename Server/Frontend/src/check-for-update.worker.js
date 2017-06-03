self.addEventListener('install', event => {
	console.log('Update checker installed');
	event.waitUntil(self.skipWaiting());
});

// The activate handler takes care of cleaning up old caches.
self.addEventListener('activate', event => {
	console.log('Update checker activated');
	event.waitUntil(self.clients.claim());
});

self.addEventListener('notificationclick', function (event) {
	event.waitUntil(
		self.clients.matchAll().then(function (clientList) {
			event.notification.close();
			if (clientList.length > 0) {
				return clientList[0].focus();
			}
			return self.clients.openWindow('/download/latest');
		})
	);
});

check();
setInterval(() => {
	check();
}, 300000);

function check() {
	fetch('./api/version/latest').then(resp => {
		resp.json().then(version => {
			try {
				var dbRequest = indexedDB.open('versionDb', 1);
				dbRequest.onsuccess = () => {
					var db = dbRequest.result;
					var store = db.transaction('version', 'readwrite').objectStore('version');
					var dbVersionR = store.get('current');
					dbVersionR.onsuccess = () => {
						var dbVersion = dbVersionR.result;
						if (dbVersion != version) {
							store.put(version, 'current');
							showNotification(version);
						}
					}
					db.close();
				};
			} catch (e) {
				console.log('Error when checking for new version');
			}
		});
	}).catch(console.log);
}

function showNotification(version) {
	var title = 'MatchmakingMonitor';
	var options = {
		body: 'New version released (' + version + ')',
		icon: 'favicon.ico'
	};
	self.registration.showNotification(title, options);
}
