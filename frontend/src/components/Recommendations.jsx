import React from 'react';

function Recommendations({ recommendations }) {
  return (
    <section className="section-block recommendations-section">
      <div className="section-heading compact">
        <div>
          <p className="eyebrow">For you</p>
          <h2>Personal Recommendations</h2>
        </div>
      </div>

      {recommendations.length === 0 ? (
        <p className="empty-text">
          No recommendations yet. Click view, like, or rate a few materials to get personalized recommendations.
        </p>
      ) : (
        <div className="recommendation-list">
          {recommendations.map((item) => (
            <article className="recommendation-card" key={item.id}>
              <div>
                <span className="category-pill">{item.category}</span>
                <h3>{item.title}</h3>
                <p>{item.description}</p>
                <div className="tag-list">
                  {item.tags.map((tag) => (
                    <span key={tag}>#{tag}</span>
                  ))}
                </div>
              </div>
              <div className="score-box">
                <strong>{item.score}</strong>
                <span>score</span>
              </div>
            </article>
          ))}
        </div>
      )}
    </section>
  );
}

export default Recommendations;
