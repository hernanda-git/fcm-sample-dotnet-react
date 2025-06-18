import React, { useState, useEffect } from 'react';
import { requestNotificationPermission, subscribeToTopic, unsubscribeFromTopic } from '../services/notificationService';

const TopicManager: React.FC = () => {
  const [topic, setTopic] = useState('');
  const [topics, setTopics] = useState<string[]>(() => {
    const saved = localStorage.getItem('subscribedTopics');
    return saved ? JSON.parse(saved) : [];
  });
  const [token, setToken] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    requestNotificationPermission().then(setToken);
  }, []);

  useEffect(() => {
    localStorage.setItem('subscribedTopics', JSON.stringify(topics));
  }, [topics]);

  const handleSubscribe = async () => {
    if (!token || !topic) return;
    setLoading(true);
    const ok = await subscribeToTopic(token, topic);
    setLoading(false);
    if (ok) {
      setTopics((prev) => prev.includes(topic) ? prev : [...prev, topic]);
      setTopic('');
      alert(`Subscribed to ${topic}`);
    } else {
      alert('Failed to subscribe');
    }
  };

  const handleUnsubscribe = async (t: string) => {
    if (!token) return;
    setLoading(true);
    const ok = await unsubscribeFromTopic(token, t);
    setLoading(false);
    if (ok) {
      setTopics((prev) => prev.filter((x) => x !== t));
      alert(`Unsubscribed from ${t}`);
    } else {
      alert('Failed to unsubscribe');
    }
  };

  return (
    <div style={{ margin: '2rem 0' }}>
      <h2>Topic Subscription</h2>
      <input
        type="text"
        value={topic}
        onChange={e => setTopic(e.target.value)}
        placeholder="Enter topic name"
        disabled={loading}
      />
      <button onClick={handleSubscribe} disabled={!topic || loading} style={{ marginLeft: 8 }}>
        Subscribe
      </button>
      <ul>
        {topics.map(t => (
          <li key={t}>
            {t}
            <button onClick={() => handleUnsubscribe(t)} disabled={loading} style={{ marginLeft: 8 }}>
              Unsubscribe
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default TopicManager;
