# 🎓 UniversityApp : Système de Gestion Universitaire (API)

Bienvenue dans le projet **UniversityApp**, une API Web construite avec ASP.NET Core 8, conçue pour gérer les opérations clés d'un établissement universitaire.

Ce projet utilise une architecture modulaire et suit les principes de la Séparation des Préoccupations (Separation of Concerns) pour garantir la maintenabilité et l'évolutivité.

---
## 🚀 Technologies Utilisées

*   **Backend** : ASP.NET Core 8 Web API
*   **Base de données** : SQL Server (via Entity Framework Core)
*   **ORM** : Entity Framework Core
*   **Authentification** : JWT (JSON Web Tokens)
*   **Logging** : NLog
*   **Mapping** : AutoMapper
## 🏗 Architecture du Projet

Le projet est structuré en plusieurs couches logiques, chacune ayant une responsabilité unique et bien définie. Cette approche, inspirée de l'Architecture en Couches (Layered Architecture), facilite le développement et les tests unitaires.

| Couche | Nom du Projet | Rôle Principal | Dépendances |
| :--- | :--- | :--- | :--- |
| **Présentation** | `UniversityApp` | Point d'entrée de l'API. Contient les **Contrôleurs** (Endpoints), la configuration des middlewares (JWT, CORS) et l'injection de dépendances. | BL, DTOs |
| **Logique Métier** | `BL` (Business Logic) | Contient la logique métier complexe via des **Managers** (ex: `AuthManager`, `CourseManager`). Gère la création de tokens JWT et le mapping des objets. | DAL, DTOs, Entities |
| **Accès aux Données** | `DAL` (Data Access Layer) | Définit les contrats d'accès aux données (Interfaces de **Repository** et **UnitOfWork**). Gère la communication avec la couche DAO. | DAO, Entities |
| **Objet d'Accès aux Données** | `DAO` (Data Access Object) | Implémentation concrète de l'accès aux données via **Entity Framework Core**. Contient le `DbContext` et les Migrations. | Entities |
| **Modèles de Données** | `Entities` | Contient les classes qui représentent les tables de la base de données (ex: `User`, `Course`, `Assignment`). | Aucune |
| **Transfert de Données** | `DTOs` (Data Transfer Objects) | Contient les modèles de données utilisés pour les communications entre l'API et le client (requêtes et réponses). | Aucune |

## 🚀 Mise en Route

Suivez ces étapes pour configurer et exécuter le projet sur votre machine.

### Prérequis

Assurez-vous d'avoir installé :

*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) ou supérieur.
*   Un serveur de base de données compatible avec Entity Framework Core (par défaut, **SQL Server LocalDB** est configuré via la chaîne de connexion).

### 1. Cloner le Répertoire

```bash
git clone [URL_DU_REPERTOIRE]
cd UniversityApp
```

### 2. Configuration de la Base de Données

Le projet utilise Entity Framework Core pour la gestion de la base de données.

1.  **Vérifiez la chaîne de connexion** :
    Ouvrez `UniversityApp/appsettings.json` et assurez-vous que la chaîne de connexion `DefaultConnection` est correcte pour votre environnement. Par défaut, elle pointe vers SQL Server LocalDB :
    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=UniversityDb;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```

2.  **Appliquez les Migrations** :
    Exécutez les commandes suivantes dans le terminal à la racine du projet (`/UniversityApp`) pour créer la base de données et les tables :

    ```bash
    dotnet ef database update --project DAO
    ```

### 3. Exécution du Projet

Démarrez l'API depuis le répertoire racine du projet :

```bash
dotnet run --project UniversityApp/UniversityApp.csproj
```

L'API sera lancée, généralement sur `https://localhost:7000` (vérifiez `Properties/launchSettings.json` pour le port exact).

### 4. Tester l'API avec Swagger

Une fois l'application lancée, ouvrez votre navigateur et accédez à l'interface **Swagger UI** :

```
https://localhost:[PORT]/swagger
```

#### Authentification (JWT)

1.  Utilisez l'endpoint `POST /api/auth/register` ou `POST /api/auth/login` pour obtenir un token JWT.
2.  Cliquez sur le bouton **Authorize** en haut à droite de l'interface Swagger.
3.  Entrez le token au format `Bearer [votre_token]` (par exemple : `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`).
4.  Vous pouvez maintenant tester les endpoints protégés (ceux avec l'icône de cadenas).
## 🎥 Démonstration

[▶️ Vous le trouverez dans le dossier Docs https://github.com/Kais-Bhs/UniversityApp/blob/main/Docs/Execution%20Demo.mp4.]

---
## 💡 Fonctionnalités Bonus Implémentées

Conformément aux exigences d'amélioration, les fonctionnalités suivantes ont été ajoutées ou renforcées :

*   **Gestion d'Erreur Améliorée** : Implémentation d'un *middleware* global pour la gestion centralisée des exceptions et le retour de réponses JSON standardisées.
*   **Logging (NLog)** : Intégration de NLog pour une journalisation complète des événements et des erreurs.
*   **Notifications Email** : Envoi automatique d'emails aux étudiants lorsque :
    *   Une soumission est notée (Grade Notification).
    *   L'étudiant est assigné à une nouvelle classe (New Class Enrollment Notification).
*   **API d'Upload de Fichiers (IFormFile)** : Ajout d'un endpoint pour la soumission de devoirs via upload de fichiers (`IFormFile`), avec validation du type et de la taille du fichier.
*   **Caching en Mémoire** : Utilisation de `IMemoryCache` pour mettre en cache les listes de départements et de cours.
*   **Pagination et Filtrage** : Déjà présent pour `Class` et `Assignment`, étendu pour inclure la récupération des utilisateurs (`User`).

## 🌐 Aperçu des Endpoints (Swagger)

L'API utilise Swagger pour la documentation interactive.

### Authentification (`/api/auth`)

| Endpoint | Description |
| :--- | :--- |
| `POST /api/auth/register` | Enregistre un nouvel utilisateur. |
| `POST /api/auth/login` | Connecte l'utilisateur et retourne le JWT. |
| `POST /api/auth/refresh-token` | Rafraîchit le JWT. |

![Endpoints d'Authentification](https://private-us-east-1.manuscdn.com/sessionFile/gE5EZQphkU4VsC42mi4igy/sandbox/qGnB3ykjTSFLKILhNhRqsf-images_1763340892601_na1fn_L2hvbWUvdWJ1bnR1L1VuaXZlcnNpdHlBcHAvVW5pdmVyc2l0eUFwcC9kb2NzL2ltYWdlcy9zd2FnZ2VyX2F1dGg.png?Policy=eyJTdGF0ZW1lbnQiOlt7IlJlc291cmNlIjoiaHR0cHM6Ly9wcml2YXRlLXVzLWVhc3QtMS5tYW51c2Nkbi5jb20vc2Vzc2lvbkZpbGUvZ0U1RVpRcGhrVTRWc0M0Mm1pNGlneS9zYW5kYm94L3FHbkIzeWtqVFNGTEtJTGhOaFJxc2YtaW1hZ2VzXzE3NjMzNDA4OTI2MDFfbmExZm5fTDJodmJXVXZkV0oxYm5SMUwxVnVhWFpsY25OcGRIbEJjSEF2Vlc1cGRtVnljMmwwZVVGd2NDOWtiMk56TDJsdFlXZGxjeTl6ZDJGbloyVnlYMkYxZEdnLnBuZyIsIkNvbmRpdGlvbiI6eyJEYXRlTGVzc1RoYW4iOnsiQVdTOkVwb2NoVGltZSI6MTc5ODc2MTYwMH19fV19&Key-Pair-Id=K2HSFNDJXOU9YS&Signature=ZhJ1nea~6liv4-CKnebhrzjwONNeiZr7lIfjlrD9mmHOCyWX-8bfQt0hOYGlzK8~jXsn4OxgXlXOEYv3n908~qfl-CcfWPhoC3F4uuJ9PJEkG9x6QF28BP0Cl1UzGPwRGXWLGJRIwB7DwdctIaYBaqlpNA4T1TILCsPhyzD3kpWXiB4BpZUwgudL1TEHqG4fy4qhMEiANLqydZNo11PuwfnX3y0faQugHJ5sf-0PdU7pbIBDxqS4hIm6sBgq2jIpLJt2NdmCf2XdumvFB3ang2prynC1JYBfR-AlFzJ~9BMBa4KuFe7NROLB0Ge3KvpwIMUokasgtTbUSP1Mh6eEsg__)

### Administration (`/api/admin`)

Ces endpoints nécessitent le rôle **Admin**.

| Endpoint | Description |
| :--- | :--- |
| `GET /api/admin/departments` | CRUD pour les départements. |
| `GET /api/admin/courses` | CRUD pour les cours. |

![Endpoints d'Administration](https://private-us-east-1.manuscdn.com/sessionFile/gE5EZQphkU4VsC42mi4igy/sandbox/qGnB3ykjTSFLKILhNhRqsf-images_1763340892602_na1fn_L2hvbWUvdWJ1bnR1L1VuaXZlcnNpdHlBcHAvVW5pdmVyc2l0eUFwcC9kb2NzL2ltYWdlcy9zd2FnZ2VyX2FkbWlu.png?Policy=eyJTdGF0ZW1lbnQiOlt7IlJlc291cmNlIjoiaHR0cHM6Ly9wcml2YXRlLXVzLWVhc3QtMS5tYW51c2Nkbi5jb20vc2Vzc2lvbkZpbGUvZ0U1RVpRcGhrVTRWc0M0Mm1pNGlneS9zYW5kYm94L3FHbkIzeWtqVFNGTEtJTGhOaFJxc2YtaW1hZ2VzXzE3NjMzNDA4OTI2MDJfbmExZm5fTDJodmJXVXZkV0oxYm5SMUwxVnVhWFpsY25OcGRIbEJjSEF2Vlc1cGRtVnljMmwwZVVGd2NDOWtiMk56TDJsdFlXZGxjeTl6ZDJGbloyVnlYMkZrYldsdS5wbmciLCJDb25kaXRpb24iOnsiRGF0ZUxlc3NUaGFuIjp7IkFXUzpFcG9jaFRpbWUiOjE3OTg3NjE2MDB9fX1dfQ__&Key-Pair-Id=K2HSFNDJXOU9YS&Signature=cXFgB5aH0viE8Tkj6PVRj6xax~J-UpvD8Qf~WG-k-3gPd4gD1TUVAVKHx7YnmPKshSLICWP1BZScz~yB6rOswrgE0z1~ec2tYqaqYtPlYPn6kHZd0FDMZzxNVjHjscS-J0fx2rEZICoQhTbpwPbu6rfiZzlmjsztJDGnvcVE2fwpnOck6rVbCpMS57CF~G2H9i8h6eJ-E8HViwXJG77jOgwt0jVxHFlsg2PiBcCgiKiRYXrTTani3FPgSNESqF6gWWY5bJwMu9diKKllhMTi5R0nM4d10~pRGITrgDe6mEMchCGl-3ZKv8Q2eS9z3YYmvn7Uz6Gc6Xsm9OyCfdkqxw__)

### Enseignant (`/api/teacher`)

Ces endpoints nécessitent le rôle **Teacher**.

| Endpoint | Description |
| :--- | :--- |
| `GET /api/teacher/classes` | Gestion des classes (CRUD, assignation d'étudiants). |
| `POST /api/teacher/attendance` | Gestion des présences. |
| `POST /api/teacher/assignments` | Gestion des devoirs et des notes. |

![Endpoints Enseignant](https://private-us-east-1.manuscdn.com/sessionFile/gE5EZQphkU4VsC42mi4igy/sandbox/qGnB3ykjTSFLKILhNhRqsf-images_1763340892603_na1fn_L2hvbWUvdWJ1bnR1L1VuaXZlcnNpdHlBcHAvVW5pdmVyc2l0eUFwcC9kb2NzL2ltYWdlcy9zd2FnZ2VyX3RlYWNoZXI.png?Policy=eyJTdGF0ZW1lbnQiOlt7IlJlc291cmNlIjoiaHR0cHM6Ly9wcml2YXRlLXVzLWVhc3QtMS5tYW51c2Nkbi5jb20vc2Vzc2lvbkZpbGUvZ0U1RVpRcGhrVTRWc0M0Mm1pNGlneS9zYW5kYm94L3FHbkIzeWtqVFNGTEtJTGhOaFJxc2YtaW1hZ2VzXzE3NjMzNDA4OTI2MDNfbmExZm5fTDJodmJXVXZkV0oxYm5SMUwxVnVhWFpsY25OcGRIbEJjSEF2Vlc1cGRtVnljMmwwZVVGd2NDOWtiMk56TDJsdFlXZGxjeTl6ZDJGbloyVnlYM1JsWVdOb1pYSS5wbmciLCJDb25kaXRpb24iOnsiRGF0ZUxlc3NUaGFuIjp7IkFXUzpFcG9jaFRpbWUiOjE3OTg3NjE2MDB9fX1dfQ__&Key-Pair-Id=K2HSFNDJXOU9YS&Signature=ZPNRhXxW0kKpev6iicKICLrIlM1xmOAo-Am74x3p7i6VRoXY6VwW9cZQJGVE5arKBSMV0KkO-4VRKJJnT74aUW9lttVWYm3VwnBv2Y78s4bMLN7a~tE247XU~QSz8g3gIWJnKlIbHrLKUkB4wj5T2JUO-xW7T6RGw~HeCY0qPusrw8z4R5mOoYu1e2YzDN-L2sLyzWdctSkbaHTVFZTL6qNWD-FppUE-VP4MYcAcjpA7DOm12Q4q2HU2TdFSM6mQG~8z5xwU-wcH-fPZ-gNN9Hdytzp~7Uhq3CBk8bEUNCRHTdEB-y7ny2nEsRE-eM4i3CUcWjwKGYcvDURxWN2O3w__)

### Étudiant (`/api/student`)

Ces endpoints nécessitent le rôle **Student**.

| Endpoint | Description |
| :--- | :--- |
| `GET /api/student/classes` | Consultation des classes et des présences. |
| `POST /api/student/assignments/{id}/submit` | Soumission des devoirs. |
| `GET /api/student/grades` | Consultation des notes. |

![Endpoints Étudiant](https://private-us-east-1.manuscdn.com/sessionFile/gE5EZQphkU4VsC42mi4igy/sandbox/qGnB3ykjTSFLKILhNhRqsf-images_1763340892603_na1fn_L2hvbWUvdWJ1bnR1L1VuaXZlcnNpdHlBcHAvVW5pdmVyc2l0eUFwcC9kb2NzL2ltYWdlcy9zd2FnZ2VyX3N0dWRlbnQ.png?Policy=eyJTdGF0ZW1lbnQiOlt7IlJlc291cmNlIjoiaHR0cHM6Ly9wcml2YXRlLXVzLWVhc3QtMS5tYW51c2Nkbi5jb20vc2Vzc2lvbkZpbGUvZ0U1RVpRcGhrVTRWc0M0Mm1pNGlneS9zYW5kYm94L3FHbkIzeWtqVFNGTEtJTGhOaFJxc2YtaW1hZ2VzXzE3NjMzNDA4OTI2MDNfbmExZm5fTDJodmJXVXZkV0oxYm5SMUwxVnVhWFpsY25OcGRIbEJjSEF2Vlc1cGRtVnljMmwwZVVGd2NDOWtiMk56TDJsdFlXZGxjeTl6ZDJGbloyVnlYM04wZFdSbGJuUS5wbmciLCJDb25kaXRpb24iOnsiRGF0ZUxlc3NUaGFuIjp7IkFXUzpFcG9jaFRpbWUiOjE3OTg3NjE2MDB9fX1dfQ__&Key-Pair-Id=K2HSFNDJXOU9YS&Signature=gqNruB-011vXG6eCkHhh51MmQNveXx55EUxvc7-rTBgawQh03EawFjscBQJ52hZKlVZD6zjce1-sIN06RnrdZV8HpvpJuyq-9JfYoQ2MHjEk3tLLJWfsweFa9RtW9HXfeLS-D0ETC52cmL7Kou22cwDAUEGym0GVc15WEbigO~7SMkqXJtof-v2R9uj9xijXQod9GpWpWD0N0Qv5bF9n4mK6nvYYo5pI8N46Whdk6Q-5ETZNkXtXAzh3HFFqfQZbYnj-aJcHACsJQnQ9UgBloXz7xEk-wNAofYwUaWOAbNB4aUWNt0GcMpoBwvOvA6AJ4uXUXhl-EofHIS7VZOVhxw__)

