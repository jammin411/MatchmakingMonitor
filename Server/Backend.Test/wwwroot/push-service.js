self.addEventListener('install',
	() => {
		//self.skipWaiting();
	});

// The activate handler takes care of cleaning up old caches.
self.addEventListener('activate',
	() => {
		//self.skipWaiting();
	});

// ReSharper disable once InconsistentNaming
var mode = '';
self.addEventListener('push',
	event => {
		event.waitUntil(self.skipWaiting());
		console.log('[Service Worker] Push Received.');
		ShowNotification(event.data.json());
	});

self.addEventListener('notificationclick',
	function(event) {
		event.waitUntil(
			self.clients.matchAll().then(function(clientList) {
				event.notification.close();
				if (clientList.length > 0) {
					return clientList[0].focus();
				}
				if (mode === 'newVersion') {
					return self.clients.openWindow('/download/latest');
				} else {
					return self.clients.openWindow('/');
				}
			})
		);
	});

self.addEventListener('pushsubscriptionchange',
	function(event) {
		console.log('[Service Worker]: \'pushsubscriptionchange\' event fired.');
		var applicationServerKey =
			UrlB64ToUint8Array('BHPsziID1JZJtR8zHEgMWiaogmV9xT_U0grDDQUAB06MzaKrL1nBB9P2ifOu1qsNNThgk19l3K5x8Wh3doB3A44');
		event.waitUntil(
			self.registration.pushManager.subscribe({
				userVisibleOnly: true,
				applicationServerKey: applicationServerKey
			})
			.then(function(newSubscription) {
				var postData = {};
				var rawKey = newSubscription.getKey ? newSubscription.getKey('p256dh') : '';
				postData['key'] = rawKey ? btoa(String.fromCharCode.apply(null, new Uint8Array(rawKey))) : '';
				var rawAuthSecret = newSubscription.getKey ? newSubscription.getKey('auth') : '';
				postData['authSecret'] = rawAuthSecret ? btoa(String.fromCharCode.apply(null, new Uint8Array(rawAuthSecret))) : '';

				postData['endpoint'] = newSubscription.endpoint;

				fetch('/api/push/register',
					{
						method: 'post',
						headers: {
							'Content-type': 'application/json'
						},
						body: JSON.stringify(postData)
					});

				console.log('[Service Worker] New subscription: ', newSubscription);
			})
		);
	});

function ShowNotification(data) {
	mode = data.type;
	var options = {
		body: data.body,
		icon: 'favicon.ico'
	};
	self.registration.showNotification(data.title, options);
}

function UrlB64ToUint8Array(base64String) {
	var padding = '='.repeat((4 - base64String.length % 4) % 4);
	var base64 = (base64String + padding)
		.replace(/\-/g, '+')
		.replace(/_/g, '/');

	var rawData = window.atob(base64);
	var outputArray = new Uint8Array(rawData.length);

	for (let i = 0; i < rawData.length; ++i) {
		outputArray[i] = rawData.charCodeAt(i);
	}
	return outputArray;
}