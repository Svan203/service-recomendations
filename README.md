# Content Recommendation Service

Educational coursework project: a content recommendation service that analyzes user views, likes, and ratings, then recommends similar content.

## Project Goal

The goal is to demonstrate a simple but real recommendation system with a separate backend API and frontend application. Users can browse content, interact with materials, and immediately see personal recommendations based on their interests.

## Technologies

- Frontend: React, Vite
- Backend: ASP.NET Core Web API, C# (.NET 10)
- Database: In-memory storage for easy local launch
- API communication: HTTP requests from frontend to backend

## Main Functionality

- Browse all content materials as cards
- Search content by title
- Filter content by category
- Save user actions: view, like, rating
- Show personal recommendations
- Show user statistics
- Display top interests by category and tag
- Show content badges such as Popular, High rating, and Similar to your interests

## Recommendation Logic

The backend stores all user actions and calculates recommendations every time `/api/recommendations` is requested.

Scoring rules:

- View: +1 point
- Like: +3 points
- Rating 4: +4 points
- Rating 5: +5 points
- Matching category: +2 points
- Each shared tag: +1 point

The system does not recommend content that the user has already interacted with. Recommended materials are sorted by the highest score.

## Run Backend

```bash
cd backend
dotnet restore
dotnet run
```

Backend URL:

```text
http://localhost:5088
```

## Run Frontend

Open another terminal:

```bash
cd frontend
npm install
npm run dev
```

Frontend URL:

```text
http://localhost:5173
```

## API Endpoints

- `GET /api/content`
- `GET /api/recommendations`
- `POST /api/actions/view/{contentId}`
- `POST /api/actions/like/{contentId}`
- `POST /api/actions/rate/{contentId}`
- `GET /api/statistics`

Rating request body:

```json
{
  "rating": 5
}
```
