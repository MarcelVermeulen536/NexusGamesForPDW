# NexusGames

Plateforme de vente de jeux vidéo (style Steam / GOG) développée avec **Angular** (frontend) et **ASP.NET Core / .NET** (backend) en **Clean Architecture**, communication avec la base de données **MySQL** via **Dapper**.

## Fonctionnalités

1. **Inscription** d'un compte utilisateur
2. **Connexion** (authentification)
3. **Catalogue de jeux** (liste + détails : développeur, plateforme, genres, promotions)
4. **Panier & achat** (le panier vit côté Angular ; l'achat est enregistré en base)
5. **Bibliothèque** personnelle (jeux possédés par l'utilisateur connecté)

---

## Prérequis

| Outil | Version utilisée | Remarque |
|-------|------------------|----------|
| .NET SDK | **10** (10.0.203) | https://dotnet.microsoft.com/download |
| Node.js | **24.14.1** | https://nodejs.org |
| npm | **11.11.0** | installé avec Node.js |
| Angular CLI | **21.2.6** | `npm install -g @angular/cli` |
| SGBD | **MySQL / MariaDB** (testé sur MariaDB 10.4.32 via XAMPP) | compatible MySQL |

---

## Architecture

```
Projet de developpement web-NEXUS/
├── backend/                 # API REST .NET (Clean Architecture)
│   ├── Api.sln
│   ├── Api/                 # Couche présentation : EndPoints (Minimal API), Middleware, Program.cs
│   ├── Core/                # Couche domaine : Models, IGateways (interfaces), UseCases
│   └── Infrastructure/      # Couche données : Repositories (Dapper), Gateways, db.sql, Utils
└── frontend/                # Application SPA Angular (standalone, signals)
```

Flux d'une donnée :
`Composant Angular → Service Angular (HttpClient) → EndPoint (Api) → UseCase (Core) → Gateway → Repository (Dapper) → MySQL`

---

## Installation

### 1. Base de données

Démarrer le serveur MySQL (par exemple via **XAMPP** → bouton *Start* sur MySQL, ou MySQL Server).

Puis exécuter le script de création de la base. Il crée la base `plateforme_jeux`, les tables, le trigger et les données d'exemple.

**Option A — en ligne de commande :**
```bash
mysql -u root -p --default-character-set=utf8mb4 < backend/Infrastructure/db.sql
```
(Avec XAMPP, le client se trouve dans `C:\xampp\mysql\bin\mysql.exe` et root n'a pas de mot de passe par défaut.)

**Option B — via MySQL Workbench :** ouvrir `backend/Infrastructure/db.sql` et l'exécuter.

### 2. Chaîne de connexion

Vérifier / adapter la chaîne de connexion dans `backend/Api/appsettings.json` selon votre configuration MySQL :
```json
"ConnectionStrings": {
  "Default": "Server=localhost;Port=3306;Database=plateforme_jeux;Uid=root;Pwd=;CharSet=utf8mb4;"
}
```
Modifier `Uid` et `Pwd` si votre utilisateur MySQL diffère (par défaut XAMPP : `Uid=root` et mot de passe vide).

### 3. Lancement du Backend

```bash
cd backend/Api
dotnet restore
dotnet run --launch-profile http
```
L'API démarre sur **http://localhost:5252**.

### 4. Lancement du Frontend

Dans un autre terminal :
```bash
cd frontend
npm install
npm start
```
L'application démarre sur **http://localhost:4200** (ouvrir cette adresse dans le navigateur).

---

## Comptes de test

Tous les comptes d'exemple ont le **même mot de passe** : `Password123!`

| Email | Mot de passe |
|-------|--------------|
| gamer1@email.com | Password123! |
| ninja@email.com | Password123! |
| sky@email.com | Password123! |
| phantom@email.com | Password123! |
| echo@email.com | Password123! |

Vous pouvez aussi créer votre propre compte via la page **Inscription**.

---

## Endpoints de l'API

| Méthode | Route | Description |
|---------|-------|-------------|
| POST | `/api/users/register` | Inscription |
| POST | `/api/users/login` | Connexion |
| GET | `/api/games` | Liste des jeux |
| GET | `/api/games/{id}` | Détail d'un jeu |
| POST | `/api/cart/checkout/{userId}` | Achat du panier |
| GET | `/api/cart/library/{userId}` | Bibliothèque de l'utilisateur |

---

## Notes techniques

- **Dapper** uniquement pour l'accès aux données (pas d'Entity Framework).
- Gestion de l'état côté Angular **exclusivement via des services** (singleton + signals), pas de NgRx/Redux.
- Mots de passe stockés hachés (**SHA-256 + Base64**).
- **CORS** configuré pour autoriser `http://localhost:4200`.
- Le trigger SQL `after_achat_add_to_library` ajoute automatiquement un jeu acheté à la bibliothèque.
