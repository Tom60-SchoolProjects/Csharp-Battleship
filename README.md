# Sujet

Le projet sera un projet **console**, de préférence, sous **.NET 6**, ou **.NET Core 3.1** développé en utilisant le langage **C#**.

Le but de ce projet est de réaliser un jeu de société, [la bataille navale](https://fr.wikipedia.org/wiki/Bataille_navale_\(jeu\)), qui permettra de jouer à 2 joueurs. Les règles à implémenter sont les suivantes :

- Chaque joueur dispose de deux grilles carrées de coté N (N étant un entier donné lors de l'appel à une API). Chaque ligne est numérotée de 1 à N et les colonnes de A à Alphabet[N] (Ex. pour N=10, Alphabet[9] -> "J")
- Les joueur dispose d'une flotte composée de bateaux qui vous seront également donnés suite à l'appel à la même API
- Une grille sera la grille de la flotte du joueur, sur laquelle il placera ses bateaux en début de partie
- L'autre grille représentera la grille de l'adverse, où le joueur pourra marquer les "touchés" et les "ratés" à chaque tour de jeu
- Les joueurs jouent chacun leur tour, lors du tour d'un joueur celui-ci annonce une case de la grille (Ex. B5), l'adversaire doit répondre soit "raté" si la case est vide, soit "touché" si le joueur a touché un bateau et enfin "touché coulé" si le joueur a touché la totalité des cases d'un bateau.

**Techniquement**, votre solution utilisera une **API**, créé pour cette occasion, qui vous donnera la taille N de la grille ainsi qu'une liste de bateaux avec leurs tailles et leurs noms. L'URL de cette API est la suivant https://REDACTED/api/GetConfig, cette API est protégée par une clé d'API qui est la suivante **REDACTED**. La clé d'API est à passer en **header** lors de votre requête avec comme nom **x-functions- key**.

Vous organiserez votre solution en plusieurs projets, **au minimum** vous devrez produire :

- Bibliothèque gérant les interactions avec votre API
- Bibliothèque de logique du jeu
- Application console gérant l'affichage du déroulé de la partie.

Vos différents projets devront être le plus modulable possible pour pouvoir fonctionner indépendamment. Exemple :

- Pouvoir utiliser un autre système de récupération des infos de base
- Pouvoir utiliser une autre application que l'application console.

Vous veillerez également à organiser correctement la structure interne de chaque projet (utilisation de différents dossiers/namespaces).

# Bonus

Des points bonus vous seront accordés, si vous n'avez pas la note maximale. Pour obtenir ces points bonus, vous devrez effectuer :

- les tests unitaire des différentes bibliothèques et de l'application
- Ajouter la possibilité d'utiliser une interface graphique (avec **MAUI** ou WPF), **en plus** de la console.