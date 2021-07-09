# Queezie : Un Quizz avec dotnet core / Nodejs / mysql db

## Introduction
La solution se compose de :
- Un front-end avec ASP.Net Core razor pages
- Une api nodejs
- Une base de données Mysql

## Récupérez les sources

`git clone https://github.com/Cyrilchc/Queezie`

## Entrez dans le projet

`cd Queezie`

## Déployez avec Docker Compose
`docker-compose up -d`

Vérifiez que les trois conteneurs ont démarrés :

`docker compose ls`
 
## Utilisez l'application
`http://localhost`

## Librairies notables / Divers

### CSS / JS
 - Framework css : [Bootstrap 4.3](https://getbootstrap.com/docs/4.3/getting-started/introduction/)
 - Police d'écriture : [Marianne](https://www.gouvernement.fr/charte/charte-graphique-les-fondamentaux/la-typographie)
 - Transitions de page : [Swup](https://swup.js.org/)
 - [JQuery](https://jquery.com/)

### Données 
- [Mysql](https://hub.docker.com/_/mysql/)
- ORM : [Dapper](https://github.com/DapperLib/Dapper) -(old)

### Markdown
- [Convertisseur Markdown](https://github.com/RickStrahl/Westwind.AspNetCore.Markdown)

