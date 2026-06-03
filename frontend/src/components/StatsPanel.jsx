import React from 'react';

function StatsPanel({ statistics }) {
  const categories = statistics?.popularCategories ?? [];
  const tags = statistics?.popularTags ?? [];

  return (
    <section className="stats-panel">
      <div className="stat-card">
        <span>Viewed</span>
        <strong>{statistics?.viewed ?? 0}</strong>
      </div>
      <div className="stat-card">
        <span>Likes</span>
        <strong>{statistics?.likes ?? 0}</strong>
      </div>
      <div className="stat-card">
        <span>Ratings</span>
        <strong>{statistics?.ratings ?? 0}</strong>
      </div>
      <div className="interests-card">
        <span>Your Interests</span>
        <div className="interest-tags">
          {[...categories, ...tags].length === 0 ? (
            <em>No interests yet</em>
          ) : (
            [...categories, ...tags].slice(0, 8).map((interest) => (
              <b key={`${interest.name}-${interest.count}`}>{interest.name}</b>
            ))
          )}
        </div>
      </div>
    </section>
  );
}

export default StatsPanel;
