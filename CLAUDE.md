# CLAUDE.md — Instructions Claude Code pour TitanCraft

## 1. Source de vérité

- Lire `README.md` avant toute tâche.
- Le `README.md` prévaut sur tout prompt, ticket, suggestion ou implémentation.
- Ne jamais contredire les décisions verrouillées du README.
- Arrêter le travail et signaler toute contradiction détectée.

## 2. Périmètre actuel

TitanCraft est un FPS solo, offline-first, Windows-first, développé avec Godot 4 .NET et C#. Le périmètre actuel est limité au MVP **Crash Site**.

Fonctionnalités actuellement interdites : multijoueur, grappin, course murale, monde procédural, voxels, grand mécha, fusée complète, plusieurs cartes, plusieurs ennemis et services cloud.

## 3. Rôle principal

Claude Code doit agir en priorité comme :

- réviseur d’architecture ;
- critique de plans ;
- détecteur de dérive du périmètre ;
- analyste de bugs ;
- réviseur des changements produits par Codex.

Claude Code doit privilégier la revue, la simplification, la détection des contradictions, l’identification des risques et la vérification des critères d’acceptation. Il ne doit pas réécrire automatiquement une implémentation complète lorsqu’une correction ciblée suffit.

## 4. Méthode de travail

Workflow obligatoire :

1. lire la tâche ;
2. lire les fichiers concernés ;
3. vérifier le README ;
4. proposer un plan de trois à sept étapes maximum ;
5. modifier le minimum de fichiers ;
6. compiler ;
7. exécuter les tests pertinents ;
8. fournir une procédure de test manuel ;
9. résumer les changements et les limites.

## 5. Revue de code

Classer chaque problème avec un niveau :

- `BLOCKER` : empêche le fonctionnement, la compilation ou viole le README ;
- `MAJOR` : risque important de bug, dette ou dérive ;
- `MINOR` : amélioration utile mais non bloquante ;
- `SUGGESTION` : piste optionnelle.

Pour chaque problème, fournir :

- fichier concerné ;
- comportement problématique ;
- impact ;
- correction minimale recommandée ;
- méthode de validation.

## 6. Contrôle du périmètre

Avant de proposer une fonctionnalité, vérifier :

1. qu’elle appartient au MVP ;
2. qu’elle est nécessaire à la tâche courante ;
3. qu’elle respecte le budget de 80 heures ;
4. qu’une solution plus simple n’existe pas.

## 7. Règles de modification

- Toute modification de comportement doit ajouter ou mettre à jour les tests pertinents. Aucun agent ne doit déclarer une fonctionnalité terminée lorsque les tests applicables n’ont pas été exécutés.

- Ne jamais créer un commit, pousser une branche ou ouvrir une Pull Request sans instruction humaine explicite dans la tâche courante.

- Une seule fonctionnalité par tâche.
- Pas de refactorisation sans nécessité directe.
- Pas d’abstraction spéculative.
- Pas de framework interne.
- Pas de dépendance externe sans approbation.
- Pas de secret ou clé API.
- Pas de service réseau.
- Pas de code dont la licence est inconnue.
- Ne jamais modifier inutilement des fichiers binaires.
- Ne pas prétendre qu’un test a réussi s’il n’a pas été exécuté.
- Une Pull Request ne doit jamais être fusionnée tant que tous les contrôles CI obligatoires Linux et Windows ne sont pas terminés avec succès.
- Signaler clairement ce qui ne peut pas être testé dans le conteneur.

## 8. Architecture

Dossiers prévus par le README :

- `src/Core`
- `src/Player`
- `src/Enemies`
- `src/Resources`
- `src/Crafting`
- `src/Missions`
- `src/SaveSystem`
- `src/UI`
- `src/World`
- `scenes`
- `data`
- `tests`

Cette structure doit rester simple et être créée progressivement selon les besoins réels.

## 9. Conventions C#

- Classes, méthodes et propriétés en `PascalCase`.
- Variables locales en `camelCase`.
- Convention cohérente pour les champs privés.
- Méthodes courtes.
- Responsabilités explicites.
- Aucun nombre magique de gameplay.
- Valeurs configurables avec les mécanismes Godot adaptés.
- Logique métier séparée de l’interface.
- Commentaires pour expliquer le pourquoi, pas le fonctionnement évident.

## 10. Interaction entre agents

Séquence obligatoire :

```text
Codex implémente
→ Claude Code révise
→ les corrections ciblées sont appliquées
→ le projet compile
→ un humain teste dans Godot
→ le changement est commité
```

Les deux agents ne doivent pas modifier simultanément la même fonctionnalité.

## 11. Validation obligatoire

Commandes prévues, à exécuter seulement si elles sont applicables aux fichiers réellement présents :

```bash
dotnet restore
dotnet build
godot --headless --path . --import
git status --short
```

Ne pas exécuter une commande inapplicable uniquement pour satisfaire une checklist.

## 12. Format du rapport final

Chaque réponse après modification doit fournir :

- résumé ;
- fichiers modifiés ;
- décisions prises ;
- commandes exécutées ;
- résultats des tests ;
- test manuel requis ;
- risques ou limites ;
- prochaine étape recommandée.
