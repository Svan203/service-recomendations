import React from 'react';

function SearchBar({ value, onChange }) {
  return (
    <label className="search-bar">
      <span>Search</span>
      <input
        type="search"
        placeholder="Search by title..."
        value={value}
        onChange={(event) => onChange(event.target.value)}
      />
    </label>
  );
}

export default SearchBar;
