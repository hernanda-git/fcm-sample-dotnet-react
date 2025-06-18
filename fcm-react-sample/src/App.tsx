import { useEffect } from 'react';
import './App.css';
import Notification from './components/notification';
import TopicManager from './components/TopicManager';
import { requestNotificationPermission } from './services/notificationService';

function App() {
  useEffect(() => {
    requestNotificationPermission();
  }, []);

  return (
    <div className="App apple-layout">
      <div className="apple-left-card">
        <h1>FCM React Sample</h1>
        <p>Open the console to see the FCM token and messages.</p>
      </div>
      <div className="apple-right-card">
        <TopicManager />
      </div>
      <Notification />
    </div>
  );
}

export default App;