import { messaging } from '../firebase/config';
import { getToken, onMessage } from 'firebase/messaging';

export interface NotificationPayload {
  notification?: {
    title?: string;
    body?: string;
    image?: string;
  };
  data?: Record<string, string>;
}

export const requestNotificationPermission = async (): Promise<string | null> => {
  try {
    if (!('Notification' in window)) {
      console.log('This browser does not support notifications.');
      return null;
    }

    const permission = await Notification.requestPermission();
    if (permission !== 'granted') {
      console.log('Notification permission denied.');
      return null;
    }

    const serviceWorkerRegistration = await navigator.serviceWorker.register('/firebase-messaging-sw.js');
    
    const token = await getToken(messaging, {
      vapidKey: import.meta.env.VITE_PUBLIC_VAPID_KEY,
      serviceWorkerRegistration,
    });

    console.log('FCM Token:', token);
    return token;
  } catch (error) {
    console.error('Error getting FCM token:', error);
    return null;
  }
};

// Handle foreground messages
export const onForegroundMessage = (
  callback: (payload: NotificationPayload) => void
): (() => void) => {
  const unsubscribe = onMessage(messaging, (payload) => {
    console.log('Foreground message received:', payload);
    callback(payload);
  });
  return unsubscribe;
};

const API_BASE = import.meta.env.VITE_API_BASE;

export const subscribeToTopic = async (token: string, topic: string): Promise<boolean> => {
  try {
    const response = await fetch(`${API_BASE}/subscribe-topic`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ tokens: [token], topic }),
    });
    return response.ok;
  } catch (error) {
    console.error('Error subscribing to topic:', error);
    return false;
  }
};

export const unsubscribeFromTopic = async (token: string, topic: string): Promise<boolean> => {
  try {
    const response = await fetch(`${API_BASE}/unsubscribe-topic`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ tokens: [token], topic }),
    });
    return response.ok;
  } catch (error) {
    console.error('Error unsubscribing from topic:', error);
    return false;
  }
};