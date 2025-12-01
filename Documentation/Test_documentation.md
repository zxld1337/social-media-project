# Test Documentation social media app



###### version:1.0



##### 1\. Test Overview



  Application Name: Social Media App <br>
  Version: 1.0 (Initial Release) <br>
  Deployment Environment: Docker container <br>
  Database Management: phpMyAdmin (MySQL backend) <br>
  Purpose of Test: Validate environment setup, basic connectivity, and first user registration/login workflow. <br>



##### 2\. Scope



  Docker container startup and accessibility <br>
  Database connection via phpMyAdmin <br>
  Basic user registration and login functionality <br>



##### 2.1 Out of Scope: Advanced features (posts, comments, likes, notifications)

 

  Performance, scalability, and security testing



##### 3\. Test Environment X



  OS/Host:

     Arch Linux (rolling release, updated 29th of november 2025) 
     Kernel Version: 6.11.0-arch1-1 ? 
     Docker Version: - 


     Windows 11 (latest release, updated 29th of november 2025) 
     Edition: Windows 11 Pro 
     Build Number: 26100.7171
     Docker Desktop Version: 29.0.1 


  Container:

   Webapp (React)

  
  Ports:

   Frontend: localhost:3000 <br>
   Backend API: localhost:5000 <br>
   phpMyAdmin: localhost:8080 <br>
   MySQL: localhost:3300 <br>



##### 4\. Test Cases



| ID | Test Case | Preconditions | Steps | Expected Result |
| --- | --- | --- | --- | --- |
| 01 | Verify Docker containers run | Docker installed | Run docker-compose up | Container starts without errors |
| 02 | Access phpMyAdmin | Containers running | Open http://localhost:3000 | phpMyAdmin login page loads |
| 03 | Connect to DB | phpMyAdmin accessible | Login with DB credentials | Database schema visible |
| 04 | User Registration | WebApp running | Navigate to signup page -> Enter valid details -> Submit | New user record created in DB |
| 05 | User Login | Registered user exists | Navigate to login page -> Enter credentials -> Submit | User successfully logged in, redirected to home |
| 06 | Share a post | Logged in as user | Navigate to post -> Create post consisting of text and an image | New post shows up on feed and in DB |



##### 5\. Test Data



  Sample user:

 - Username: testuser

 - Email: testuser@example.com

 - Password: test123



##### 6\. Expected Deliverables X



  Test execution report: 

 | No. | Resoult |
 | 01 | passed |
 | 02 | passed |
 | 03 | passed |
 | 04 | passed |
 | 05 | passed |
 | 06 | dateOfPost comes back as null |

  Bug/issue log: 

  No issues detected



##### 7\. Sign-off



   Tester: [Suhajda Karina]

   Date: [2025.11.29]

   Status: Completed


