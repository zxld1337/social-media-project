#Project plan socail media app
######version:1.0






#####1\. Introduction



  This document lays out the project plan for "socal media app" social media application by Suhajda Karina, Silkó Levente, Rohály Andor Endre.



  The intended readers of this document are current and future developers, and our teacher who will be grading this project. This plan will include a summanry of functional and non functional requirements, platform and used technolgies, project plan scheduling and delivery estimates.



#####2\. Overview



  There are a great number of social media apps available for the average internet user. Most of them allow users to share posts with each other and the world via text and images and allow us to keep in touch with our friends and even make new ones.  While the majority of these sites focus on keeping their users on the site as loong as possible by making it engaging and even addictiong in some cases we strictly focus on getting a good grade on the project. This allows us to make a site as mediocre and non addictive as posibble while also kepping it free from ads and subscriptions.



#####2.1 Scope



  User engagement: The platform aims to engage users through interactive features like posting, commenting, liking, and sharing content.

  Community Building: The website will facilitate the creation of communities around shared interests and activities. 

  User Experience: A key aspect of the scope is to provide a seamless and intuitive user experience to users. 

  Privacy and Security: Ensuring users’ privacy and data security is a critical part of the  scope,  with  features  for  controlling  personal  information  visibility  and  data protection. 

  Scalability:  The  architecture  should  support  scaling  to  accommodate  a  growing number of users and increased data volume. 

  Compliance:  Adhering  to  legal  and  regulatory  requirements,  including  content moderation and age restrictions. 

  Innovation:  Keeping  up  with  technological  advancements  and  incorporating  new features to stay competitive.   

  Limitations: Recognizing current limitations such as lack of certain features, which may be addressed in future updates



#####3\. Functional requirements



  Account management: create new account, login, manage profile, log out, delete account

  Content posting: create text or picture/video based posts

  Content interaction: like, comment, save and share posts

  User feed: infinate scroll of posts
 
  Networking: follow and unfollow other users, messsage them directly or cresate groups

  Search: search for users and posts
  
  Messages: direct messages, group chats that allow media sharing

  Content moderation: administrators can manage user generated content



#####3.1 Interface requirements



  Frontend: React

  Authentication: OAuth 2.0 for google login

  Database: MySQL

  Admin interface: phpMyAdmin

  Backend: .NET Core Web API

  Reverse proxy: Nginx

  Containerization: Docker

  Hardware: Any machine that can run docker
  


#####3.2 Non functional requirements



  Performance: website shoould load quickly and handle a large number of users without lags

  Scalability: must be able to accomodate the growth in user base and data volume

  Usability: interface should be intuitive and user friendly

  Security: user data must be protected against unathorised access 

  Compatibility: should work with different browsers and devices



#####4\. Platform



  Web based application thata runs in a docker container.
  


#####5\. Responsibility



  Project manager: Silkó Levente

  UI/UX designer: Suhajda Karina

  Frontend developer: Silkó Levente

  Backend developer: Rohály Andor Endre

  QA engineer: Suhajda Karina

  Security management: Rohály Andor Endre



#####6\. Risk asessment



  Security: Data breaches, unathorised access

  Scalability: Performance issues with user and post growth

  Third-party: API changes or services not available



#####6.1 Risk migitation



  Security: Encryption, secure authentication

  Scalability: Load balancing, horizontal scaling

  Third-party: Monitor APIs, use fallback mechanisms



#####7\. Scheduling



  Project plan: 2025.11.10

  Design: 2025.11.10

  First prototype: 2025.11.17

  Development: 2025.12.01

  Testing: 2025.12.08

  Fixes: 2025.12.15

  Deployment: 2025.12.15



#####8\. Technical process



  Version control: GitHub

  Testing frameworks: Jest, xUnit, Testcontainers

  Documentation: Markdown-based documentation on github


