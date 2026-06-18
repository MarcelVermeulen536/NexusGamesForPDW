# NexusGames

Plateforme de vente de jeux vidéo (style Steam / GOG) développée avec **Angular** (frontend) et **ASP.NET Core / .NET** (backend) en **Clean Architecture**, communication avec la base de données **MySQL** via **Dapper**.

## Fonctionnalités

1. **Inscription** d'un compte utilisateur
2. **Connexion** (authentification)
3. **Catalogue de jeux** (liste + détails : développeur, plateforme, genres, promotions)
4. **Panier & achat** (le panier vit côté Angular ; l'achat est enregistré en base)
5. **Bibliothèque** personnelle (jeux possédés par l'utilisateur connecté)

---

## Prérequis — logiciels à installer (une seule fois)

Sur une machine neuve, installez ces 3 logiciels **avant tout** (cliquez « Suivant » / « Next » jusqu'au bout pour chaque installation) :

1. **.NET SDK 10** → https://dotnet.microsoft.com/download/dotnet/10.0
   (téléchargez « SDK x64 » pour Windows, lancez l'installateur.)
2. **Node.js 20 ou plus (version LTS)** → https://nodejs.org
   (npm est inclus automatiquement avec Node.js — rien d'autre à installer.)
3. **XAMPP** (fournit le serveur **MySQL** + l'outil graphique **phpMyAdmin**) → https://www.apachefriends.org

> ⚠️ **Important** : après avoir installé .NET et Node.js, **fermez puis rouvrez** vos fenêtres de terminal (sinon les commandes ne seront pas reconnues).

**Pour vérifier que tout est bien installé**, ouvrez un terminal (touche Windows → tapez `cmd` → Entrée) et tapez ces commandes ; chacune doit afficher un numéro de version :
```bash
dotnet --version
node --version
npm --version
```

Versions utilisées pour ce projet (à titre indicatif) :

| Outil | Version |
|-------|---------|
| .NET SDK | 10 (10.0.203) |
| Node.js / npm | 24.14.1 / 11.11.0 |
| Angular | 21.2 (inclus dans le projet, **rien à installer en plus**) |
| Base de données | MariaDB 10.4.32 via XAMPP (compatible MySQL) |

> L'outil **Angular CLI** n'a **pas besoin** d'être installé globalement : il est déjà inclus dans le projet et utilisé automatiquement par la commande `npm start`.

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

## Lancement pas à pas (depuis zéro)

> Astuce pour ouvrir un terminal dans un dossier : dans l'Explorateur Windows, ouvrez le dossier voulu, cliquez dans la **barre d'adresse**, tapez `cmd` et appuyez sur **Entrée**.

### Étape 1 — Démarrer la base de données

Ouvrez le **XAMPP Control Panel** et cliquez sur **Start** en face de **MySQL** (la ligne devient verte). Laissez XAMPP ouvert.

### Étape 2 — Créer la base de données (uniquement la 1ʳᵉ fois)

Cette étape crée la base `plateforme_jeux`, les tables, le déclencheur (trigger) et les données d'exemple. **La méthode la plus simple : phpMyAdmin.**

1. Dans XAMPP, cliquez sur **Admin** en face de MySQL (ou ouvrez votre navigateur sur **http://localhost/phpmyadmin**).
2. En haut, cliquez sur l'onglet **Importer**.
3. Cliquez sur **Choisir un fichier** et sélectionnez le fichier :
   `backend/Infrastructure/db.sql` (dans le dossier du projet).
4. Descendez en bas et cliquez sur **Importer** (ou **Exécuter**).
5. À gauche, la base **`plateforme_jeux`** doit apparaître avec ses 10 tables. ✅

> *Alternative en ligne de commande (si vous préférez)* : ouvrez un terminal dans le dossier `backend/Infrastructure` et lancez
> `"C:\xampp\mysql\bin\mysql.exe" -u root --default-character-set=utf8mb4 < db.sql`

### Étape 3 — Vérifier le mot de passe de la base (souvent rien à faire)

Le projet est configuré pour MySQL de XAMPP : utilisateur **`root`** **sans mot de passe**, ce qui est le réglage par défaut de XAMPP. **Si c'est votre cas, ne touchez à rien.**

Seulement si votre utilisateur MySQL a un mot de passe différent, ouvrez le fichier `backend/Api/appsettings.json` et adaptez cette ligne (mettez le mot de passe après `Pwd=`) :
```json
"ConnectionStrings": {
  "Default": "Server=localhost;Port=3306;Database=plateforme_jeux;Uid=root;Pwd=;CharSet=utf8mb4;"
}
```

### Étape 4 — Démarrer le Backend (l'API)

Ouvrez un terminal dans le dossier `backend/Api` et tapez :
```bash
dotnet run --launch-profile http
```
La 1ʳᵉ fois, .NET télécharge les dépendances (patientez). C'est prêt quand vous voyez **`Now listening on: http://localhost:5252`**.
👉 **Laissez ce terminal ouvert** (ne le fermez pas tant que vous utilisez l'application).

### Étape 5 — Démarrer le Frontend (l'application Angular)

Ouvrez un **deuxième** terminal (le premier doit rester ouvert) dans le dossier `frontend`.

La toute première fois seulement, installez les dépendances (cela prend quelques minutes) :
```bash
npm install
```
Puis, à chaque lancement :
```bash
npm start
```
C'est prêt quand vous voyez **`Local: http://localhost:4200`**. 👉 Laissez aussi ce terminal ouvert.

### Étape 6 — Ouvrir l'application

Ouvrez votre navigateur sur **http://localhost:4200**.
Connectez-vous avec un compte de test, par exemple **gamer1@email.com** / **Password123!** (voir la section ci-dessous).

> Pour tout arrêter : fermez les deux terminaux, puis cliquez sur **Stop** (MySQL) dans XAMPP.

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
