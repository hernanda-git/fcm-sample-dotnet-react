import { useEffect } from 'react';
import './App.css';
import Notification from './components/notification';
import TopicManager from './components/TopicManager';
import { requestNotificationPermission } from './services/notificationService';

function App() {
  useEffect(() => {
    // Request notification permission on app load
    requestNotificationPermission();
  }, []);

  return (
    <div className="App">
      <h1>FCM React Sample</h1>
      <p>Open the console to see the FCM token and messages.</p>
      <Notification />
      <TopicManager />
    </div>
  );
}

export default App;