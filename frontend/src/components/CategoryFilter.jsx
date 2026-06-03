import React from 'react';

function CategoryFilter({ categories, activeCategory, onChange }) {
  return (
    <div className="category-filter" aria-label="Category filter">
      {categories.map((category) => (
        <button
          className={category === activeCategory ? 'chip active' : 'chip'}
          key={category}
          onClick={() => onChange(category)}
          type="button"
        >
          {category}
        </button>
      ))}
    </div>
  );
}

export default CategoryFilter;
