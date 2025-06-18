import { initializeApp } from 'firebase/app';
import { getMessaging, type Messaging } from 'firebase/messaging';

// Your Firebase configuration object from Firebase Console
export const firebaseConfig = {
  apiKey: "AIzaSyAJWDPAkYmYN7S6_EzZlA7RHzt_Ywv0gqQ",
  authDomain: "quantum-plasma-463302-u9.firebaseapp.com",
  projectId: "quantum-plasma-463302-u9",
  storageBucket: "quantum-plasma-463302-u9.firebasestorage.app",
  messagingSenderId: "48243424737",
  appId: "1:48243424737:web:a57c2a99a838c418bba6a0",
  measurementId: "G-K25BB3ZFXE"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);

// Initialize Firebase Cloud Messaging
export const messaging: Messaging = getMessaging(app);