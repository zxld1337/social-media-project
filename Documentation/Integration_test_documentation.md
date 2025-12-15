## Integration\_test\_Documentation



version: 1.1 (Integration Testing Edition)



1\. Test Overview



Application Name: Social Media App Version: 1.1 (Integration Test Release) Deployment Environment: Docker container (multi‑service environment) Database Management: phpMyAdmin (MySQL backend) Purpose of Test: Validate integration between frontend, backend, and database, including API communication, data persistence, and cross‑module behavior (authentication, posts, comments, likes).

2\. Scope

Included



&nbsp;   Docker container startup and inter‑service communication



&nbsp;   Frontend ↔ Backend API integration



&nbsp;   Backend API ↔ MySQL database integration



&nbsp;   Authentication workflow (register → login → token validation)



&nbsp;   Post creation and DB persistence



&nbsp;   Comment and like interactions (API + DB)



&nbsp;   Data flow validation across modules



2.1 Out of Scope



&nbsp;   UI/UX testing



&nbsp;   Performance, scalability, and load testing



&nbsp;   Security/pentesting



&nbsp;   Notification system



&nbsp;   Advanced social features (chat, real‑time updates)



3\. Test Environment

OS / Host Systems



Arch Linux (rolling release, updated 29 November 2025)



&nbsp;   Kernel Version: 6.11.0‑arch1‑1



&nbsp;   Docker Version: not specified



Windows 11 (latest release, updated 29 November 2025)



&nbsp;   Edition: Windows 11 Pro



&nbsp;   Build Number: 26100.7171



&nbsp;   Docker Desktop Version: 29.0.1



Containerized Services

Service	Technology	Port

Frontend	React	localhost:3000

Backend API	.NET Web API	localhost:5000

phpMyAdmin	Web UI	localhost:8080

MySQL	Database	localhost:3300

4\. Test Cases

ID	Test Case	Preconditions	Steps	Expected Result

01	Verify Docker containers run	Docker installed	Run docker-compose up	All containers start without errors; services reachable

02	Access phpMyAdmin	Containers running	Open http://localhost:8080	phpMyAdmin login page loads

03	Connect to DB	phpMyAdmin accessible	Login with DB credentials	Database schema visible; tables load correctly

04	User Registration (Integration)	WebApp + API + DB running	Signup → enter valid details → submit	API returns 200; new user inserted into DB

05	User Login (Integration)	Registered user exists	Login → submit credentials	Backend returns JWT; UI redirects to home

06	Share a Post (Integration)	Logged in user	Create post (text + image)	Post appears in feed; DB entry created

07	Add Comment (Integration)	Existing post	Add comment → submit	Comment appears under post; DB row created

08	Like a Post (Integration)	Existing post	Click like button	Like count increases; DB relation created

09	Delete Post Cascade	Post with comments/likes	Delete post	Post removed; related comments/likes removed

10	Token Validation	Logged in user	Refresh page → access protected route	Token validated; user stays logged in

5\. Test Data

Sample User

Code

