import React from 'react';

function ContentCard({ item, badges, onView, onLike, onRate }) {
  return (
    <article className="content-card">
      <div className="card-topline">
        <span className="category-pill">{item.category}</span>
        <span className="rating-value">Star {item.rating.toFixed(1)}</span>
      </div>

      <h3>{item.title}</h3>
      <p className="card-description">{item.description}</p>

      <div className="badges">
        {badges.popular && <span>Popular</span>}
        {badges.highRating && <span>High rating</span>}
        {badges.similar && <span>Similar to your interests</span>}
      </div>

      <div className="tag-list">
        {item.tags.map((tag) => (
          <span key={tag}>#{tag}</span>
        ))}
      </div>

      <div className="card-metrics">
        <span>{item.views} views</span>
        <span>{item.category}</span>
      </div>

      <div className="card-actions">
        <button type="button" onClick={onView}>View</button>
        <button type="button" onClick={onLike}>Like</button>
      </div>

      <div className="rating-buttons" aria-label={`Rate ${item.title}`}>
        {[1, 2, 3, 4, 5].map((rating) => (
          <button key={rating} type="button" onClick={() => onRate(rating)}>
            {rating}
          </button>
        ))}
      </div>
    </article>
  );
}

export default ContentCard;
