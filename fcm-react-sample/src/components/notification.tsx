import { useEffect } from 'react';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { onForegroundMessage, type NotificationPayload } from '../services/notificationService';

const Notification: React.FC = () => {
  useEffect(() => {
    const unsubscribe = onForegroundMessage((payload: NotificationPayload) => {
      const title = payload.notification?.title || 'New Notification';
      const body = payload.notification?.body || 'You have a new message!';
      toast.info(`${title}: ${body}`, {
        position: 'top-right',
        autoClose: 5000,
      });
    });

    return () => unsubscribe();
  }, []);

  return <ToastContainer />;
};

export default Notification;