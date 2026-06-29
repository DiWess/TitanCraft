# Revue MVP Crash Site

Cette liste sert à choisir une seule amélioration par tâche future. Elle reste limitée au MVP **Crash Site** défini dans `README.md` et ne remplace pas les tests humains de gameplay.

## Priorités immédiates

| Priorité | Catégorie | Tâche | Résultat attendu |
|---:|---|---|---|
| 1 | bug bloquant | Corriger et valider la touche gauche/Q. | Le déplacement gauche fonctionne en AZERTY avec `Q` sans déclencher une action critique comme quitter la partie. |
| 2 | lisibilité gameplay | Ajouter un tutoriel HUD court. | Le joueur comprend rapidement les contrôles et l'objectif actuel sans lire un long tutoriel. |
| 3 | lisibilité gameplay | Différencier visuellement les interactables. | Les ressources, l'établi, le point de sauvegarde et la balise sont distinguables à distance des éléments décoratifs. |

## Bug bloquant

- Vérifier que les entrées clavier QWERTY/WASD et AZERTY/ZQSD restent compatibles avec le déplacement minimal du MVP.
- Confirmer que les actions critiques ne réutilisent pas `Q`, car `Q` est réservé au déplacement gauche en AZERTY.
- Valider que le joueur ne peut pas sortir facilement des limites de la petite zone jouable.

## Lisibilité gameplay

- Afficher un message HUD très court pour guider la boucle : collecter les ressources, fabriquer le bras, vaincre le Galaxabrain, activer la balise.
- Rendre les interactables lisibles avec des silhouettes, couleurs ou marqueurs simples cohérents avec la direction artistique.
- Vérifier que l'objectif actuel reste identifiable pendant toute la session MVP.

## Polish non bloquant

- Ajuster les couleurs et matériaux temporaires des placeholders sans remplacer les assets par des contenus définitifs prématurés.
- Ajouter ou ajuster des sons temporaires uniquement si cela améliore la compréhension des actions clés.
- Améliorer les textes courts du HUD, du menu pause et de l'écran de victoire sans étendre la boucle de gameplay.

## Exclusions explicites du périmètre MVP

Les tâches de cette revue ne doivent pas introduire les éléments hors périmètre listés dans la section 6 du `README.md`, notamment : multijoueur, serveurs, comptes utilisateurs, services cloud, microtransactions, monde infini, génération procédurale complète, système de blocs voxel, destruction totale du terrain, grand mécha, fusée complète, grappin, course murale, véhicules, arbres de compétences, classes de personnages, quêtes secondaires, plusieurs biomes, plusieurs cartes, plusieurs armes, plusieurs types d'ennemis, compagnons IA, IA générative intégrée au jeu, synthèse vocale, reconnaissance vocale, cinématiques complexes, doublage, support console, support mobile, support Linux ou macOS garanti, mods, Steam Workshop, classements, succès en ligne, anti-triche et télémétrie distante.

## Règle d'utilisation

Pour chaque tâche future, sélectionner une seule ligne ou un seul point de cette liste, définir le test pertinent avant modification, puis vérifier que l'amélioration ne déborde pas du MVP **Crash Site**.
