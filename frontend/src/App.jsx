import React, { useEffect, useMemo, useState } from 'react';
import { getContent, getRecommendations, getStatistics, saveLike, saveRating, saveView } from './api.js';
import CategoryFilter from './components/CategoryFilter.jsx';
import ContentCard from './components/ContentCard.jsx';
import Header from './components/Header.jsx';
import Recommendations from './components/Recommendations.jsx';
import SearchBar from './components/SearchBar.jsx';
import StatsPanel from './components/StatsPanel.jsx';

const categories = ['All', 'Movies', 'Series', 'Articles', 'Videos', 'Courses'];

function App() {
  const [content, setContent] = useState([]);
  const [recommendations, setRecommendations] = useState([]);
  const [statistics, setStatistics] = useState(null);
  const [category, setCategory] = useState('All');
  const [search, setSearch] = useState('');
  const [loading, setLoading] = useState(true);
  const [notice, setNotice] = useState('');
  const [error, setError] = useState('');

  async function loadData() {
    const [contentData, recommendationData, statisticsData] = await Promise.all([
      getContent(),
      getRecommendations(),
      getStatistics(),
    ]);

    setContent(contentData);
    setRecommendations(recommendationData);
    setStatistics(statisticsData);
  }

  useEffect(() => {
    loadData()
      .catch(() => setError('Backend is not available. Start the API and refresh the page.'))
      .finally(() => setLoading(false));
  }, []);

  async function handleAction(action) {
    setError('');
    await action();
    await loadData();
    setNotice('Action saved. Recommendations updated.');
    window.setTimeout(() => setNotice(''), 2200);
  }

  const filteredContent = useMemo(() => {
    return content.filter((item) => {
      const matchesCategory = category === 'All' || item.category === category;
      const matchesSearch = item.title.toLowerCase().includes(search.toLowerCase().trim());
      return matchesCategory && matchesSearch;
    });
  }, [content, category, search]);

  function hasInterest(item) {
    const categoryMatch = statistics?.popularCategories?.some((interest) => interest.name === item.category);
    const tagMatch = item.tags?.some((tag) => statistics?.popularTags?.some((interest) => interest.name.toLowerCase() === tag.toLowerCase()));
    return Boolean(categoryMatch || tagMatch);
  }

  return (
    <div className="app-shell">
      <Header />

      <main className="main-layout">
        {notice && <div className="toast">{notice}</div>}
        {error && <div className="error-box">{error}</div>}

        <StatsPanel statistics={statistics} />

        <section className="section-block">
          <div className="section-heading">
            <div>
              <p className="eyebrow">Library</p>
              <h2>All Content</h2>
            </div>
            <SearchBar value={search} onChange={setSearch} />
          </div>

          <CategoryFilter categories={categories} activeCategory={category} onChange={setCategory} />

          {loading ? (
            <p className="empty-text">Loading content...</p>
          ) : (
            <div className="content-grid">
              {filteredContent.map((item) => (
                <ContentCard
                  key={item.id}
                  item={item}
                  badges={{
                    popular: item.views >= 300,
                    highRating: item.rating >= 4.6,
                    similar: hasInterest(item),
                  }}
                  onView={() => handleAction(() => saveView(item.id))}
                  onLike={() => handleAction(() => saveLike(item.id))}
                  onRate={(rating) => handleAction(() => saveRating(item.id, rating))}
                />
              ))}
            </div>
          )}
        </section>

        <Recommendations recommendations={recommendations} />
      </main>
    </div>
  );
}

export default App;
