const API_URL = 'http://localhost:5088';

async function request(path, options = {}) {
  const response = await fetch(`${API_URL}${path}`, {
    headers: {
      'Content-Type': 'application/json',
      ...options.headers,
    },
    ...options,
  });

  if (!response.ok) {
    throw new Error(`Request failed: ${response.status}`);
  }

  return response.json();
}

export function getContent() {
  return request('/api/content');
}

export function getRecommendations() {
  return request('/api/recommendations');
}

export function getStatistics() {
  return request('/api/statistics');
}

export function saveView(contentId) {
  return request(`/api/actions/view/${contentId}`, { method: 'POST' });
}

export function saveLike(contentId) {
  return request(`/api/actions/like/${contentId}`, { method: 'POST' });
}

export function saveRating(contentId, rating) {
  return request(`/api/actions/rate/${contentId}`, {
    method: 'POST',
    body: JSON.stringify({ rating }),
  });
}

