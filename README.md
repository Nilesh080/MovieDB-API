✅ Features Implemented
📦 Repository Layer & Data Access
🔁 Built a Generic BaseRepository<T> to support reusable CRUD operations across all entities.

💡 Used Dapper ORM for fast, efficient, and lightweight database querying.

✅ All SQL queries follow best practices and proper formatting conventions.

🧩 Stored Procedures
🛠 Implemented and integrated usp_AddMovie and usp_UpdateMovie stored procedures to:

Insert/update movie details with related entities.

Map many-to-many relationships: Movie-Actor and Movie-Genre using intermediate tables (ActorMovie, GenreMovie).

Used SCOPE_IDENTITY() to get the inserted MovieId and STRING_SPLIT() to handle actor/genre ID lists.

🎬 Entity-wise API & Controller Coverage
🎥 Movies
Full CRUD operations.

Add/update with actor/genre mapping using stored procedures.

Poster upload functionality using Supabase.

🧑‍🎤 Actors
Add, update, delete, fetch all, fetch by ID.

Connected to movies via mapping table.

🎭 Genres
CRUD APIs to manage genre types.

Used for categorizing movies.

🎬 Producers
Manage producer profiles.

CRUD support with date of birth and bio.

💬 Reviews
Add or fetch reviews related to a movie.

Enables user feedback/comment system.

👤 Users
Signup and login endpoints with password hashing.

JWT-based authentication.

Role-based protection for authorized endpoints.

🖼 Poster Image Upload
☁️ Integrated Supabase Storage to manage movie poster images securely.

📤 Created a dedicated API endpoint for uploading and storing image URLs.

🧾 Implementation follows the official Supabase C# SDK.

🔐 Authentication & Security
🔐 Implemented JWT authentication for secure access.

🔑 Users must log in to get a token for accessing protected routes.

✅ Login and signup functionality with token generation and validation.

🧪 Robust Backend Design
📁 Followed clean architecture with separation of concerns: Controllers, Repositories, Services, DTOs, and Models.

⚙️ Clean and extensible Web API structure, ready for Swagger UI or frontend integration.

📌 Consistent naming conventions and folder structure.

🧠 Proper use of async/await and efficient connection management.
