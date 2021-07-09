-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1:3306
-- Généré le :  jeu. 29 avr. 2021 à 20:59
-- Version du serveur :  5.7.26
-- Version de PHP :  7.2.18

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données :  `queezie`
--
CREATE DATABASE IF NOT EXISTS `queezie` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `queezie`;

-- --------------------------------------------------------

--
-- Structure de la table `answer`
--

DROP TABLE IF EXISTS `answer`;
CREATE TABLE IF NOT EXISTS `answer` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `answer` text NOT NULL,
  `type` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=latin1;

--
-- Déchargement des données de la table `answer`
--

INSERT INTO `answer` (`id`, `answer`, `type`) VALUES
(4, 'C#', 1),
(5, 'JavaScript', 0),
(6, 'C++', 0),
(7, 'Rust', 0),
(8, 'La mer', 1),
(9, 'L\'océan', 1),
(10, 'Un lac', 0),
(11, 'Une rivière', 0),
(12, 'Oui', 0),
(14, 'Non', 1);

-- --------------------------------------------------------

--
-- Structure de la table `domain`
--

DROP TABLE IF EXISTS `domain`;
CREATE TABLE IF NOT EXISTS `domain` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `domain` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=latin1;

--
-- Déchargement des données de la table `domain`
--

INSERT INTO `domain` (`id`, `domain`) VALUES
(4, 'Nature'),
(5, 'Histoire'),
(7, 'Informatique'),
(9, 'Cuisine');

-- --------------------------------------------------------

--
-- Structure de la table `linkquestionanswer`
--

DROP TABLE IF EXISTS `linkquestionanswer`;
CREATE TABLE IF NOT EXISTS `linkquestionanswer` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `questionId` int(11) NOT NULL,
  `answerId` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `questionId` (`questionId`),
  KEY `answerId` (`answerId`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=latin1;

--
-- Déchargement des données de la table `linkquestionanswer`
--

INSERT INTO `linkquestionanswer` (`id`, `questionId`, `answerId`) VALUES
(1, 1, 4),
(2, 1, 5),
(3, 1, 6),
(4, 1, 7),
(5, 3, 8),
(6, 3, 9),
(7, 3, 10),
(8, 3, 11),
(9, 4, 12),
(11, 4, 14);

-- --------------------------------------------------------

--
-- Structure de la table `linkquizquestion`
--

DROP TABLE IF EXISTS `linkquizquestion`;
CREATE TABLE IF NOT EXISTS `linkquizquestion` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `quizId` int(11) NOT NULL,
  `questionId` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `quizId` (`quizId`),
  KEY `questionId` (`questionId`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=latin1;

--
-- Déchargement des données de la table `linkquizquestion`
--

INSERT INTO `linkquizquestion` (`id`, `quizId`, `questionId`) VALUES
(11, 1, 1),
(13, 1, 4),
(14, 4, 3);

-- --------------------------------------------------------

--
-- Structure de la table `question`
--

DROP TABLE IF EXISTS `question`;
CREATE TABLE IF NOT EXISTS `question` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `question` text NOT NULL,
  `questionTypeId` int(11) NOT NULL,
  `domainId` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `questionTypeId` (`questionTypeId`),
  KEY `domainId` (`domainId`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

--
-- Déchargement des données de la table `question`
--

INSERT INTO `question` (`id`, `question`, `questionTypeId`, `domainId`) VALUES
(1, 'Quel est le meilleur langage informatique ?', 1, 7),
(3, 'Où peut-on trouver de l\'eau de mer ?', 2, 4),
(4, 'HTML est un langage de programmation', 3, 7);

-- --------------------------------------------------------

--
-- Structure de la table `questiontype`
--

DROP TABLE IF EXISTS `questiontype`;
CREATE TABLE IF NOT EXISTS `questiontype` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `questionType` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Déchargement des données de la table `questiontype`
--

INSERT INTO `questiontype` (`id`, `questionType`) VALUES
(1, 'Simple'),
(2, 'Multiple'),
(3, 'Boolean');

-- --------------------------------------------------------

--
-- Structure de la table `quiz`
--

DROP TABLE IF EXISTS `quiz`;
CREATE TABLE IF NOT EXISTS `quiz` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `quiz` text NOT NULL,
  `duration` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

--
-- Déchargement des données de la table `quiz`
--

INSERT INTO `quiz` (`id`, `quiz`, `duration`) VALUES
(1, 'Quiz Informatique', 10),
(4, 'Quiz Nature', 10);

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `linkquestionanswer`
--
ALTER TABLE `linkquestionanswer`
  ADD CONSTRAINT `linkquestionanswer_ibfk_1` FOREIGN KEY (`questionId`) REFERENCES `question` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `linkquestionanswer_ibfk_2` FOREIGN KEY (`answerId`) REFERENCES `answer` (`id`) ON DELETE CASCADE;

--
-- Contraintes pour la table `linkquizquestion`
--
ALTER TABLE `linkquizquestion`
  ADD CONSTRAINT `linkquizquestion_ibfk_1` FOREIGN KEY (`quizId`) REFERENCES `quiz` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `linkquizquestion_ibfk_2` FOREIGN KEY (`questionId`) REFERENCES `question` (`id`) ON DELETE CASCADE;

--
-- Contraintes pour la table `question`
--
ALTER TABLE `question`
  ADD CONSTRAINT `question_ibfk_1` FOREIGN KEY (`questionTypeId`) REFERENCES `questiontype` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `question_ibfk_2` FOREIGN KEY (`domainId`) REFERENCES `domain` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
