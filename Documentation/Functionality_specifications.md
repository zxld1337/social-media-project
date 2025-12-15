# Functionality specifications



###### Glossary



  Post: User-generated content

  Feed: Aggregation of posts

  Follow: Subscription to a user's content



##### 1\. User roles



  User: Can crreate posers, like commetn, follow message, edit own profile

  Private user: Same us user but content is only visible to people the follow

  Moderator: Full platofrm controls, user manaagement, configuraítion toggles



##### 2\. Functional requirements



  Describes what the product will do exactly, specific behaviours and features. The testin crieria will be based on these.



##### 2.1 Registrationa and authenticatio



  Email sign up: Create an account with email, password and username. The email and username must be unique, all users must be logged in before posting and browsing. 

  Authentication: ??



##### 2.2 Profiles



  Edit profile: Update avatar, display name, email address. Image size <= 5 MB in PNG format. Changes persisct accross devices and for other users, invalid input shows error message. 



##### 2.3 Posting



  Create post: Compose text and opétionally pictures and publish. Text <= 500 and pictures <= 1, size must be <= 5 MB. Succesful post appears in authors profile and visible based on privacy settings. Faliure to post returns error message

  Edit and delete posts: Posts can be edited or deleted aíny time, edit marks the post as "edited" delete removes it from everywhere. 



##### 2.4 Feed and discovery



  Home feed: Show a feed in cronological order with followers' posts. Pull-to-refresh updates new or chaged items, no duplicates

  Search users and posts:



##### 3\. Data models



  Account: id, username, password, full_name, email, phone_number, date_of_birth, date_of_create, profile_picture

  Post: id, user_id, image, text, date_of_post

  Media

  Follow: ??

  Comment: id, user_id, post_id, text, date_of_comment

  Liked  post: id, user_id, post_id, date_of_like



##### 3.1 Relationships



  one-to-many: user -> post, post -> comment, post -> like

  many-to-many: user <-> user 



##### 4\. Validation rules and errors



  User: unique username 

  Password: >= 8 caharacters, lower case, upper case, number

  Post: cannot be empty, image <= 5MB

  Comment: cannot be empty or > 200 characters



##### 5\. Non-functional requirements



##### 6\. Acceptance criteria







##### 7\. Dependencies



##### 8\. Change log
