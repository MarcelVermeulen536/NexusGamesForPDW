-- ============================================
-- NexusGames - BASE DE DONNÉES (MySQL)
-- Plateforme de vente de jeux vidéo (style Steam / GOG)
-- ============================================
-- Script de création du schéma + données d'amorçage (seed).
-- À exécuter une fois pour préparer la base utilisée par l'API .NET (via Dapper).
--
-- Comptes de test : tous les utilisateurs ci-dessous ont le mot de passe : Password123!
-- (stocké haché en Base64( SHA-256(mot_de_passe) ), comme le fait Base64Utils côté .NET)
-- ============================================

DROP DATABASE IF EXISTS plateforme_jeux;
CREATE DATABASE IF NOT EXISTS plateforme_jeux;
USE plateforme_jeux;

-- ============================================
-- TABLE 1 : GENRES
-- ============================================
CREATE TABLE genres (
    id_genre INT PRIMARY KEY AUTO_INCREMENT,
    nom_genre VARCHAR(100) NOT NULL UNIQUE,
    description TEXT,
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ============================================
-- TABLE 2 : DEVELOPPEURS
-- ============================================
CREATE TABLE developpeurs (
    id_developpeur INT PRIMARY KEY AUTO_INCREMENT,
    nom_developpeur VARCHAR(150) NOT NULL UNIQUE,
    pays VARCHAR(100),
    annee_fondation INT,
    site_web VARCHAR(255)
);

-- ============================================
-- TABLE 3 : PLATEFORMES
-- ============================================
CREATE TABLE plateformes (
    id_plateforme INT PRIMARY KEY AUTO_INCREMENT,
    nom_plateforme VARCHAR(100) NOT NULL UNIQUE,
    entreprise VARCHAR(100),
    annee_sortie INT
);

-- ============================================
-- TABLE 4 : UTILISATEURS
-- ============================================
CREATE TABLE utilisateurs (
    id_utilisateur INT PRIMARY KEY AUTO_INCREMENT,
    pseudo VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(100) NOT NULL UNIQUE,
    mot_de_passe VARCHAR(255) NOT NULL,
    nom VARCHAR(100),
    prenom VARCHAR(100),
    date_inscription TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    solde_compte DECIMAL(10, 2) DEFAULT 0,
    statut ENUM('actif', 'inactif', 'suspendu') DEFAULT 'actif'
);

-- ============================================
-- TABLE 5 : JEUX
-- ============================================
CREATE TABLE jeux (
    id_jeu INT PRIMARY KEY AUTO_INCREMENT,
    titre VARCHAR(200) NOT NULL,
    description TEXT,
    id_developpeur INT NOT NULL,
    id_plateforme INT NOT NULL,
    prix DECIMAL(10, 2) NOT NULL,
    date_sortie DATE,
    score_metascore INT CHECK (score_metascore >= 0 AND score_metascore <= 100),
    nombre_joueurs INT,
    date_ajout TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_developpeur) REFERENCES developpeurs(id_developpeur) ON DELETE RESTRICT,
    FOREIGN KEY (id_plateforme) REFERENCES plateformes(id_plateforme) ON DELETE RESTRICT
);

-- ============================================
-- TABLE 6 : JEUX_GENRES (TABLE INTERMÉDIAIRE - relation N:N)
-- ============================================
CREATE TABLE jeux_genres (
    id_jeu_genre INT PRIMARY KEY AUTO_INCREMENT,
    id_jeu INT NOT NULL,
    id_genre INT NOT NULL,
    date_association TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_jeu) REFERENCES jeux(id_jeu) ON DELETE CASCADE,
    FOREIGN KEY (id_genre) REFERENCES genres(id_genre) ON DELETE CASCADE,
    UNIQUE KEY unique_jeu_genre (id_jeu, id_genre)
);

-- ============================================
-- TABLE 7 : ACHATS
-- ============================================
CREATE TABLE achats (
    id_achat INT PRIMARY KEY AUTO_INCREMENT,
    id_utilisateur INT NOT NULL,
    id_jeu INT NOT NULL,
    prix_paye DECIMAL(10, 2) NOT NULL,
    date_achat TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    methode_paiement ENUM('carte', 'paypal', 'bitcoin', 'portefeuille') NOT NULL,
    statut_achat ENUM('complete', 'en_attente', 'remboursee') DEFAULT 'complete',
    FOREIGN KEY (id_utilisateur) REFERENCES utilisateurs(id_utilisateur) ON DELETE CASCADE,
    FOREIGN KEY (id_jeu) REFERENCES jeux(id_jeu) ON DELETE CASCADE
);

-- ============================================
-- TABLE 8 : CRITIQUES
-- ============================================
CREATE TABLE critiques (
    id_critique INT PRIMARY KEY AUTO_INCREMENT,
    id_utilisateur INT NOT NULL,
    id_jeu INT NOT NULL,
    note INT NOT NULL CHECK (note >= 1 AND note <= 10),
    titre_critique VARCHAR(200),
    contenu TEXT NOT NULL,
    date_critique TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    nombre_votes INT DEFAULT 0,
    FOREIGN KEY (id_utilisateur) REFERENCES utilisateurs(id_utilisateur) ON DELETE CASCADE,
    FOREIGN KEY (id_jeu) REFERENCES jeux(id_jeu) ON DELETE CASCADE
);

-- ============================================
-- TABLE 9 : BIBLIOTHEQUE
-- ============================================
CREATE TABLE bibliotheque (
    id_bibliotheque INT PRIMARY KEY AUTO_INCREMENT,
    id_utilisateur INT NOT NULL,
    id_jeu INT NOT NULL,
    date_acquisition DATE,
    temps_jeu_heures INT DEFAULT 0,
    completion_pourcentage INT DEFAULT 0 CHECK (completion_pourcentage >= 0 AND completion_pourcentage <= 100),
    FOREIGN KEY (id_utilisateur) REFERENCES utilisateurs(id_utilisateur) ON DELETE CASCADE,
    FOREIGN KEY (id_jeu) REFERENCES jeux(id_jeu) ON DELETE CASCADE,
    UNIQUE KEY unique_user_jeu (id_utilisateur, id_jeu)
);

-- ============================================
-- TABLE 10 : PROMOTIONS
-- ============================================
CREATE TABLE promotions (
    id_promotion INT PRIMARY KEY AUTO_INCREMENT,
    id_jeu INT NOT NULL,
    pourcentage_reduction INT NOT NULL CHECK (pourcentage_reduction > 0 AND pourcentage_reduction <= 100),
    prix_reduit DECIMAL(10, 2) NOT NULL,
    date_debut DATETIME NOT NULL,
    date_fin DATETIME NOT NULL,
    raison VARCHAR(200),
    FOREIGN KEY (id_jeu) REFERENCES jeux(id_jeu) ON DELETE CASCADE
);

-- ============================================
-- DONNÉES D'EXEMPLE (seed)
-- ============================================

-- 1. Genres
INSERT INTO genres (nom_genre, description) VALUES
('Action', 'Jeux d\'action rapide et intenses'),
('RPG', 'Jeux de rôle avec progression de personnage'),
('Stratégie', 'Jeux nécessitant de la réflexion tactique'),
('Puzzle', 'Jeux de puzzle et d\'énigmes'),
('Aventure', 'Jeux exploratoires et narratifs');

-- 2. Développeurs
INSERT INTO developpeurs (nom_developpeur, pays, annee_fondation) VALUES
('Rockstar Games', 'USA', 1998),
('FromSoftware', 'Japon', 1986),
('Valve Corporation', 'USA', 1996),
('CD Projekt Red', 'Pologne', 2002),
('Nintendo', 'Japon', 1889);

-- 3. Plateformes
INSERT INTO plateformes (nom_plateforme, entreprise, annee_sortie) VALUES
('PC Windows', 'Microsoft', 1985),
('PlayStation 5', 'Sony', 2020),
('Xbox Series X', 'Microsoft', 2020),
('Nintendo Switch', 'Nintendo', 2017),
('Mac', 'Apple', 2001);

-- 4. Utilisateurs
-- Mot de passe en clair pour TOUS les comptes de test : Password123!
-- Stocké haché : Base64( SHA-256("Password123!") ) = oQnjaUetVt4dyhzEnw74rJrZp7GqDfQfs8TLc8H/Aeo=
INSERT INTO utilisateurs (pseudo, email, mot_de_passe, nom, prenom) VALUES
('GamerPro123', 'gamer1@email.com', 'oQnjaUetVt4dyhzEnw74rJrZp7GqDfQfs8TLc8H/Aeo=', 'Dupont', 'Jean'),
('NinjaGamer', 'ninja@email.com', 'oQnjaUetVt4dyhzEnw74rJrZp7GqDfQfs8TLc8H/Aeo=', 'Martin', 'Pierre'),
('SkyWalker99', 'sky@email.com', 'oQnjaUetVt4dyhzEnw74rJrZp7GqDfQfs8TLc8H/Aeo=', 'Moreau', 'Sophie'),
('PhantomX', 'phantom@email.com', 'oQnjaUetVt4dyhzEnw74rJrZp7GqDfQfs8TLc8H/Aeo=', 'Bernard', 'Michel'),
('EchoKnight', 'echo@email.com', 'oQnjaUetVt4dyhzEnw74rJrZp7GqDfQfs8TLc8H/Aeo=', 'Roux', 'Isabelle');

-- 5. Jeux
INSERT INTO jeux (titre, description, id_developpeur, id_plateforme, prix, date_sortie, score_metascore) VALUES
('Cyberpunk 2077', 'Jeu futuriste immersif dans un univers cyberpunk', 4, 1, 59.99, '2020-12-10', 77),
('Elden Ring', 'RPG d\'action dark fantasy', 2, 2, 69.99, '2022-02-25', 96),
('The Witcher 3', 'Jeu de rôle épique avec histoires complexes', 4, 1, 49.99, '2015-05-19', 92),
('Half-Life 2', 'Jeu de tir à la première personne révolutionnaire', 3, 1, 19.99, '2004-11-16', 96),
('The Legend of Zelda', 'Jeu d\'aventure fantastique', 5, 4, 59.99, '2023-05-12', 98);

-- 6. Association Jeux-Genres
INSERT INTO jeux_genres (id_jeu, id_genre) VALUES
(1, 1), (1, 2),
(2, 1), (2, 2),
(3, 2),
(4, 1),
(5, 4), (5, 5);

-- 7. Achats
INSERT INTO achats (id_utilisateur, id_jeu, prix_paye, methode_paiement) VALUES
(1, 1, 59.99, 'carte'),
(1, 4, 19.99, 'carte'),
(2, 2, 69.99, 'paypal'),
(3, 3, 49.99, 'carte'),
(4, 5, 59.99, 'portefeuille');

-- 8. Critiques
INSERT INTO critiques (id_utilisateur, id_jeu, note, titre_critique, contenu) VALUES
(1, 1, 8, 'Excellent RPG futuriste', 'Jeu immersif avec une bonne histoire'),
(2, 2, 10, 'Chef-d\'oeuvre du jeu vidéo', 'Elden Ring est un masterpiece'),
(3, 3, 9, 'The Witcher 3 merveilleux', 'Histoire captivante et graphismes splendides');

-- 9. Bibliothèque (données historiques avec heures de jeu)
-- Insérées ici car le trigger n'existe pas encore.
INSERT INTO bibliotheque (id_utilisateur, id_jeu, temps_jeu_heures, completion_pourcentage) VALUES
(1, 1, 45, 75),
(1, 4, 20, 100),
(2, 2, 120, 100),
(3, 3, 95, 85),
(4, 5, 60, 100);

-- 10. Promotions (dates rendues actives pour la démonstration du catalogue)
INSERT INTO promotions (id_jeu, pourcentage_reduction, prix_reduit, date_debut, date_fin, raison) VALUES
(1, 20, 47.99, '2026-01-01 00:00:00', '2026-12-31 23:59:59', 'Promotion de lancement NexusGames'),
(3, 15, 42.49, '2026-01-01 00:00:00', '2026-12-31 23:59:59', 'Promotion de lancement NexusGames');

-- ============================================
-- TRIGGER : ajoute automatiquement le jeu à la bibliothèque après un achat « complete »
-- (créé en dernier, une fois les données historiques en place)
-- ============================================
DELIMITER //

DROP TRIGGER IF EXISTS after_achat_add_to_library //

CREATE TRIGGER after_achat_add_to_library
AFTER INSERT ON achats
FOR EACH ROW
BEGIN
    IF NEW.statut_achat = 'complete' THEN
        INSERT IGNORE INTO bibliotheque (id_utilisateur, id_jeu, date_acquisition, temps_jeu_heures, completion_pourcentage)
        VALUES (NEW.id_utilisateur, NEW.id_jeu, CURRENT_DATE, 0, 0);
    END IF;
END //

DELIMITER ;
