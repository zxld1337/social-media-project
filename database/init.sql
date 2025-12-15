-- MySQL 8.0 Compatible Init Script
-- Database: socialdb

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";

-- Create database if it doesn't exist (optional, since docker-compose creates it)
CREATE DATABASE IF NOT EXISTS `socialdb` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_hungarian_ci;
USE `socialdb`;

-- --------------------------------------------------------

--
-- Table structure for table `accounts`
--

CREATE TABLE IF NOT EXISTS `accounts` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(25) NOT NULL,
  `password` varchar(72) NOT NULL,
  `full_name` varchar(40) DEFAULT NULL,
  `email` varchar(40) NOT NULL,
  `phone_number` varchar(11) DEFAULT NULL,
  `date_of_birth` varchar(20) DEFAULT NULL,
  `date_of_create` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `profile_picture` blob DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `username` (`username`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Table structure for table `posts`
--

CREATE TABLE IF NOT EXISTS `posts` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `image` blob DEFAULT NULL,
  `text` text DEFAULT NULL,
  `date_of_post` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `user_id_fk` (`user_id`),
  CONSTRAINT `user_id_fk` FOREIGN KEY (`user_id`) REFERENCES `accounts` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Table structure for table `comments`
--

CREATE TABLE IF NOT EXISTS `comments` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `post_id` int(11) NOT NULL,
  `text` text NOT NULL,
  `date_of_comment` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `comments_user_id_fk` (`user_id`),
  KEY `comments_post_id_fk` (`post_id`),
  CONSTRAINT `comments_post_id_fk` FOREIGN KEY (`post_id`) REFERENCES `posts` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `comments_user_id_fk` FOREIGN KEY (`user_id`) REFERENCES `accounts` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Table structure for table `liked_posts`
--

CREATE TABLE IF NOT EXISTS `liked_posts` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `post_id` int(11) NOT NULL,
  `date_of_like` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `liked_user_id_fk` (`user_id`),
  KEY `liked_post_id_fk` (`post_id`),
  CONSTRAINT `liked_post_id_fk` FOREIGN KEY (`post_id`) REFERENCES `posts` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `liked_user_id_fk` FOREIGN KEY (`user_id`) REFERENCES `accounts` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Table structure for table `follows`
--

CREATE TABLE IF NOT EXISTS `follows` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `following_id` int(11) NOT NULL,
  `follower_id` int(11) NOT NULL,
  `date_of_follow` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `unique_follow` (`following_id`, `follower_id`),
  KEY `following_id_fk` (`following_id`),
  KEY `follower_id_fk` (`follower_id`),
  CONSTRAINT `following_id_fk` FOREIGN KEY (`following_id`) REFERENCES `accounts`(`id`) ON DELETE CASCADE,
  CONSTRAINT `follower_id_fk` FOREIGN KEY (`follower_id`) REFERENCES `accounts`(`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;



-- Add unique constraint to prevent duplicate likes
ALTER TABLE `liked_posts`
  ADD UNIQUE KEY `unique_user_post_like` (`user_id`, `post_id`);


-- -- Dumping data for table `accounts`
INSERT INTO `accounts` (`id`, `username`, `password`, `full_name`, `email`, `phone_number`, `date_of_birth`, `date_of_create`, `profile_picture`) VALUES
(1, 'Ferenc', '$2a$12$IINjlJez5tcJ9217gqBRG.EFZIeeA6Lx5WvXXDtuBEcR893YvgvUq', NULL, 'f@gmail.com', NULL, NULL, '2025-12-14 14:46:00', NULL),
(2, 'Kerepesi', '$2a$12$FKhMFzgaDswzYNeO03dmKOiI6qs3eCbPL8zxD8o9DggW4cBTCKinC', NULL, 'k@gmail.com', NULL, NULL, '2025-12-14 14:48:12', NULL),
(3, 'Levente', '$2a$12$ufuxxAGNw1le3KTLbNIW2Ou77rSLpwHa5DRmpCXfKTwepejh3TNG6', NULL, 'l@gmail.com', NULL, NULL, '2025-12-14 14:52:00', NULL);


COMMIT;