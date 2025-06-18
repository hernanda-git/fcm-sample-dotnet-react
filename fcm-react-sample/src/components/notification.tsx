import { useEffect } from 'react';
import { toast, ToastContainer, ToastOptions } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { onForegroundMessage, type NotificationPayload } from '../services/notificationService';
import '../glass-toast.css';

const toastOptions: ToastOptions = {
  className: 'glass-toast',
  closeButton: true,
  position: 'bottom-right',
  autoClose: 5000,
  hideProgressBar: false,
  pauseOnHover: true,
  draggable: true,
};

const Notification: React.FC = () => {
  useEffect(() => {
    const unsubscribe = onForegroundMessage((payload: NotificationPayload) => {
      const title = payload.notification?.title || 'New Notification';
      const body = payload.notification?.body || 'You have a new message!';
      toast.info(`${title}: ${body}`, toastOptions);
    });

    return () => unsubscribe();
  }, []);

  return <ToastContainer newestOnTop theme="colored" />;
};

export default Notification;