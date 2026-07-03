# Revue MVP Crash Site

Cette liste sert à choisir une seule amélioration par tâche future. Elle reste limitée au MVP **Crash Site** défini dans `README.md` et ne remplace pas les tests humains de gameplay.

État de référence : `artifacts/mvp_closure/final_mvp_verdict.json` conclut `GO` avec 15/15 contrôles de playthrough réussis, aucun P0/P1 ouvert, et une chaîne restore/build/tests/import/export/launch validée par les preuves listées. `artifacts/mvp_closure/runtime_playthrough.md` détaille les résultats runtime associés. `artifacts/mvp_closure/20260703_windows_simulated_validation.md` complète la validation simulée repository-owned sans remplacer le playthrough humain Windows.

## Priorités actives restantes

| Priorité | Catégorie | Tâche | Résultat attendu |
|---:|---|---|---|
| 1 | validation humaine | Réaliser un playthrough humain Windows sur matériel cible. | Confirmer les sensations de déplacement/combat, la lisibilité et les performances ressenties hors simulation, automatisation headless et CI. |
| 2 | polish non bloquant | Ajouter des sons temporaires uniquement si nécessaire à la compréhension. | Les actions clés peuvent être mieux comprises sans transformer l'audio en objectif MVP prioritaire. |
| 3 | polish non bloquant | Nettoyer les noms `Placeholder_*` et les primitives d'interactables quand ils ne sont plus nécessaires aux tests. | Les éléments restent compatibles avec les NodePaths et tests existants tout en réduisant les libellés temporaires visibles côté développement. |
| 4 | sauvegarde non bloquante | Étudier la persistance de la direction du regard du joueur. | Le chargement conserve déjà position, santé, inventaire, mission, ennemi et balise ; la direction du regard peut être améliorée sans bloquer le GO MVP. |

## Éléments résolus ou complétés

Ces points ne doivent plus rester dans la liste active sauf régression prouvée par un nouveau test ou un nouveau playthrough.

| Catégorie | Point résolu | Preuve actuelle |
|---|---|---|
| Entrées clavier | Compatibilité QWERTY/WASD et AZERTY/ZQSD, y compris `Q` pour le déplacement gauche sans action critique de sortie. | `runtime_playthrough.md` indique que les déplacements WASD/ZQSD, diagonaux, la souris et le saut sont PASS dans `TestPhysicsAndMovement` et `TestJumpAndCamera`. |
| Tutoriel HUD | Couverture HUD courte pour l'état du bras, les indications obsolètes et l'objectif courant. | `runtime_playthrough.md` indique que le HUD passe l'état du bras à "Online" et que l'indice obsolète de démarrage a été remplacé. |
| Séquence mission | Boucle collecter → fabriquer → vaincre → récupérer → activer → victoire. | `final_mvp_verdict.json` confirme `mission_sequence_verified: true`, `victory_verified: true` et 15/15 contrôles de playthrough réussis. |
| Bras mécanique visible | Visuel du bras après fabrication/chargement. | `final_mvp_verdict.json` confirme `mechanical_arm_visual_verified: true`; `runtime_playthrough.md` marque le correctif P1 comme régressé-testé. |
| Sauvegarde, mort et rechargement | Restauration de l'état de mission, inventaire, ennemi, composant et balise après la mort/recharge. | `final_mvp_verdict.json` confirme `save_death_reload_verified: true`; le playthrough marque les tests de sauvegarde/rechargement PASS. |
| Limites de carte | Sortie de zone et chute hors limites récupérées par le flux de mort standard. | `final_mvp_verdict.json` confirme `collision_boundary_verified: true`; le playthrough marque le correctif P0 comme régressé-testé. |
| Défauts P0/P1 | Aucun P0/P1 ouvert dans l'état de clôture MVP. | `final_mvp_verdict.json` indique `open_p0_count: 0` et `open_p1_count: 0`. |
| Validation simulée Windows | Simulation repository-owned de lancement, boucle complète, sauvegarde/Continue et lisibilité proxy. | `20260703_windows_simulated_validation.md` marque la table simulée PASS tout en gardant le playthrough humain Windows ouvert. |

## Règles de maintien

- Ne rouvrir un point résolu que si une preuve actuelle démontre une régression.
- Garder les priorités actives limitées au périmètre **Crash Site** : une petite carte, un joueur FPS, un Galaxabrain, trois ressources, un bras mécanique, une balise, une mission, sauvegarde locale et menus minimaux.
- Ne pas introduire de nouvelle fonctionnalité MVP pour traiter le polish restant.

## Exclusions explicites du périmètre MVP

Les tâches de cette revue ne doivent pas introduire les éléments hors périmètre listés dans la section 6 du `README.md`, notamment : multijoueur, serveurs, comptes utilisateurs, services cloud, microtransactions, monde infini, génération procédurale complète, système de blocs voxel, destruction totale du terrain, grand mécha, fusée complète, grappin, course murale, véhicules, arbres de compétences, classes de personnages, quêtes secondaires, plusieurs biomes, plusieurs cartes, plusieurs armes, plusieurs types d'ennemis, compagnons IA, IA générative intégrée au jeu, synthèse vocale, reconnaissance vocale, cinématiques complexes, doublage, support console, support mobile, support Linux ou macOS garanti, mods, Steam Workshop, classements, succès en ligne, anti-triche et télémétrie distante.

## Règle d'utilisation

Pour chaque tâche future, sélectionner une seule ligne ou un seul point actif de cette liste, définir le test pertinent avant modification, puis vérifier que l'amélioration ne déborde pas du MVP **Crash Site**.
