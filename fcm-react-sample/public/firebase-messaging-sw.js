importScripts('https://www.gstatic.com/firebasejs/9.23.0/firebase-app-compat.js');
importScripts('https://www.gstatic.com/firebasejs/9.23.0/firebase-messaging-compat.js');

firebase.initializeApp({
    apiKey: "AIzaSyAJWDPAkYmYN7S6_EzZlA7RHzt_Ywv0gqQ",
    authDomain: "quantum-plasma-463302-u9.firebaseapp.com",
    projectId: "quantum-plasma-463302-u9",
    storageBucket: "quantum-plasma-463302-u9.firebasestorage.app",
    messagingSenderId: "48243424737",
    appId: "1:48243424737:web:a57c2a99a838c418bba6a0",
    measurementId: "G-K25BB3ZFXE"
});

const messaging = firebase.messaging();

messaging.onBackgroundMessage((payload) => {
    console.log('[firebase-messaging-sw.js] Received background message ', payload);
    const notificationTitle = payload.notification?.title || 'Background Message';
    const notificationOptions = {
        body: payload.notification?.body || 'You have a new message!',
        icon: '/firebase-logo.png',
    };

    self.registration.showNotification(notificationTitle, notificationOptions);
});