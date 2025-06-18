import { useEffect } from 'react';
import { toast, ToastContainer } from 'react-toastify';
import type { ToastOptions } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { onForegroundMessage, type NotificationPayload } from '../services/notificationService';

const toastOptions: ToastOptions = {
  closeButton: true,
  position: 'bottom-right',
  autoClose: 7000,
  hideProgressBar: false,
  pauseOnHover: true,
  draggable: true,
  style: {
    background: 'var(--apple-card)',
    color: 'var(--apple-text)',
    borderRadius: 14,
    border: '1px solid var(--apple-border)',
    boxShadow: 'var(--apple-shadow)',
    fontFamily: 'var(--apple-font)',
    fontSize: 16,
    padding: '1.2em 1.5em',
    minWidth: 280,
    maxWidth: 400,
  },
};

const Notification: React.FC = () => {
  useEffect(() => {
    const unsubscribe = onForegroundMessage((payload: NotificationPayload) => {
      const title = payload.notification?.title || 'New Notification';
      const body = payload.notification?.body || 'You have a new message!';
      toast.success(`${title}: ${body}`, toastOptions);
    });

    return () => unsubscribe();
  }, []);

  return <ToastContainer newestOnTop theme="light" />;
};

export default Notification;