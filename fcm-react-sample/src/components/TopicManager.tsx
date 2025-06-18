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
    }
  };

  const handleUnsubscribe = async (t: string) => {
    if (!token) return;
    setLoading(true);
    const ok = await unsubscribeFromTopic(token, t);
    setLoading(false);
    if (ok) {
      setTopics((prev) => prev.filter((x) => x !== t));
    }
  };

  return (
    <section>
      <div style={{ marginBottom: 28 }}>
        <h2 style={{ color: '#18181a', fontWeight: 700, fontSize: 22, marginBottom: 10 }}>Manage Topics</h2>
        <div style={{ display: 'flex', gap: 10, alignItems: 'center' }}>
          <input
            type="text"
            value={topic}
            onChange={e => setTopic(e.target.value)}
            placeholder="Enter topic name"
            disabled={loading}
            style={{ flex: 1 }}
          />
          <button onClick={handleSubscribe} disabled={!topic || loading}>
            Subscribe
          </button>
        </div>
      </div>
      <div>
        <h3 style={{ color: '#6e6e73', fontSize: 15, margin: '0 0 0.7em 0', fontWeight: 600 }}>Subscribed Topics</h3>
        <div style={{ overflowX: 'auto' }}>
          <table style={{ width: '100%', borderCollapse: 'collapse', background: 'transparent' }}>
            <thead>
              <tr style={{ background: '#f5f6fa' }}>
                <th style={{ textAlign: 'left', color: '#18181a', fontWeight: 600, fontSize: 15, padding: '0.6em 0.8em' }}>Topic</th>
                <th style={{ textAlign: 'right', color: '#18181a', fontWeight: 600, fontSize: 15, padding: '0.6em 0.8em' }}>Action</th>
              </tr>
            </thead>
            <tbody>
              {topics.length === 0 ? (
                <tr>
                  <td colSpan={2} style={{ color: '#b0b0b5', fontSize: 15, textAlign: 'center', padding: '1.2em 0' }}>No topics subscribed.</td>
                </tr>
              ) : (
                topics.map((t) => (
                  <tr key={t} style={{ borderBottom: '1.5px solid #f0f0f0' }}>
                    <td style={{ color: '#18181a', fontWeight: 500, fontSize: 16, padding: '0.7em 0.8em' }}>{t}</td>
                    <td style={{ textAlign: 'right', padding: '0.7em 0.8em' }}>
                      <button
                        onClick={() => handleUnsubscribe(t)}
                        disabled={loading}
                        style={{ background: '#f2f2f7', color: '#d70015', border: '1px solid #eee', borderRadius: 8, fontSize: 14, fontWeight: 600, padding: '0.3em 1.2em' }}
                      >
                        Remove
                      </button>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>
    </section>
  );
};

export default TopicManager;
