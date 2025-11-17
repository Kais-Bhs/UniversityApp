🎓 UniversityApp : Système de Gestion Universitaire (API)

Bienvenue dans le projet UniversityApp, une API Web construite avec ASP.NET Core 8, conçue pour gérer les opérations clés d'un établissement universitaire.

Ce projet utilise une architecture modulaire et suit les principes de la Séparation des Préoccupations (Separation of Concerns) pour garantir la maintenabilité et l'évolutivité.




🏗 Architecture du Projet

Le projet est structuré en plusieurs couches logiques, chacune ayant une responsabilité unique et bien définie. Cette approche, inspirée de l'Architecture en Couches (Layered Architecture), facilite le développement et les tests unitaires.

Couche
Nom du Projet
Rôle Principal
Dépendances
Présentation
UniversityApp
Point d'entrée de l'API. Contient les Contrôleurs (Endpoints), la configuration des middlewares (JWT, CORS) et l'injection de dépendances.
BL, DTOs
Logique Métier
BL (Business Logic)
Contient la logique métier complexe via des Managers (ex: AuthManager, CourseManager). Gère la création de tokens JWT et le mapping des objets.
DAL, DTOs, Entities
Accès aux Données
DAL (Data Access Layer)
Définit les contrats d'accès aux données (Interfaces de Repository et UnitOfWork). Gère la communication avec la couche DAO.
DAO, Entities
Objet d'Accès aux Données
DAO (Data Access Object)
Implémentation concrète de l'accès aux données via Entity Framework Core. Contient le DbContext et les Migrations.
Entities
Modèles de Données
Entities
Contient les classes qui représentent les tables de la base de données (ex: User, Course, Assignment).
Aucune
Transfert de Données
DTOs (Data Transfer Objects)
Contient les modèles de données utilisés pour les communications entre l'API et le client (requêtes et réponses).
Aucune


🚀 Mise en Route

Suivez ces étapes pour configurer et exécuter le projet sur votre machine.

Prérequis

Assurez-vous d'avoir installé :

•
.NET 8 SDK ou supérieur.

•
Un serveur de base de données compatible avec Entity Framework Core (par défaut, SQL Server LocalDB est configuré via la chaîne de connexion).

1. Cloner le Répertoire

Bash


git clone [URL_DU_REPERTOIRE]
cd UniversityApp


2. Configuration de la Base de Données

Le projet utilise Entity Framework Core pour la gestion de la base de données.

1.
Vérifiez la chaîne de connexion : Ouvrez UniversityApp/appsettings.json et assurez-vous que la chaîne de connexion DefaultConnection est correcte pour votre environnement. Par défaut, elle pointe vers SQL Server LocalDB :

2.
Appliquez les Migrations : Exécutez les commandes suivantes dans le terminal à la racine du projet (/UniversityApp) pour créer la base de données et les tables :

3. Exécution du Projet

Démarrez l'API depuis le répertoire racine du projet :

Bash


dotnet run --project UniversityApp/UniversityApp.csproj


L'API sera lancée, généralement sur https://localhost:7000 (vérifiez Properties/launchSettings.json pour le port exact ).

4. Tester l'API avec Swagger

Une fois l'application lancée, ouvrez votre navigateur et accédez à l'interface Swagger UI :

Plain Text


https://localhost:[PORT]/swagger


Authentification (JWT )

1.
Utilisez l'endpoint POST /api/auth/register ou POST /api/auth/login pour obtenir un token JWT.

2.
Cliquez sur le bouton Authorize en haut à droite de l'interface Swagger.

3.
Entrez le token au format Bearer [votre_token] (par exemple : Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...).

4.
Vous pouvez maintenant tester les endpoints protégés (ceux avec l'icône de cadenas).


