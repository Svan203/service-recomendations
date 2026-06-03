import React from 'react';

function Header() {
  return (
    <header className="hero">
      <nav className="topbar">
        <span className="brand-mark">CRS</span>
        <span className="nav-note">Coursework demo</span>
      </nav>

      <div className="hero-content">
        <p className="eyebrow">Personalized learning and media discovery</p>
        <h1>Content Recommendation Service</h1>
        <p className="hero-description">
          The system analyzes user views, likes, and ratings, then recommends similar content.
        </p>
        <div className="banner-block">
          Choose what interests you - the system will find similar materials for you.
        </div>
      </div>
    </header>
  );
}

export default Header;
