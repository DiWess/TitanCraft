# TitanCraft

## Project Director Entry Point

TitanCraft is a solo, offline-first, Windows-first Godot 4 .NET FPS about an astronaut surviving a hostile crash site, collecting three resource types, crafting a first mechanical arm, defeating one Galaxabrain Scout, recovering a component, and activating a rescue beacon. The current engine/runtime stack is Godot 4 .NET with C# gameplay code, local generated-support tooling in Python, Blender Asset Forge for standalone asset production candidates, and GitHub Actions for repeatable validation and visual artifact bundles.

**Current MVP status:** the locked MVP is **Crash Site**, a short 10-30 minute single-player loop with one small map, FPS player movement/combat, resource pickups, workbench crafting, a mechanical arm, one Galaxabrain Scout encounter, a component retrieval step, a save point, a beacon, victory/defeat/save flow, and Windows offline export target. Passing automated tests, import checks, or artifact generation does **not** mean visual approval; visual work needs PNG evidence and human or visual-reviewer verdicts.

**Current production phase:** governance and pipelines are usable; gameplay MVP work should remain conservative and locked; visual production is still gated because Stage A generated art is not approved and production asset candidates require standalone review artifacts before scene replacement.

**Blocked:** Stage A visual replacement, marketing screenshots, Stage B work, and any claim of art readiness until approved review PNGs and human/visual-reviewer verdicts exist. **Unblocked:** governance/doc tasks, gameplay bug fixes inside the Crash Site MVP scope, CI/build maintenance, Agent Studio routing, Blender Asset Forge standalone asset candidate generation, Art Taste Pack checks, and Visual Artifact Factory review-bundle requests.

**How to start any task:** read this README, read [`AGENTS.md`](AGENTS.md), run `python3 tools/agent_preflight.py "<exact task>"`, follow the routed packet, state forbidden scope, make the smallest safe change, run applicable validation, and report evidence with an approved verdict. Future autonomous project directors should begin with [`PROJECT_DIRECTOR_START_HERE.md`](PROJECT_DIRECTOR_START_HERE.md).

Key operating links: [`PROJECT_DIRECTOR_START_HERE.md`](PROJECT_DIRECTOR_START_HERE.md), [Agent Studio](studio/README.md), [Blender Asset Forge](docs/pipeline/blender-asset-forge.md), [Art Taste Pack / Visual Identity](docs/art/titancraft-visual-identity.md), [Visual Artifact Factory](docs/pipeline/visual-artifact-factory.md), [gameplay validation commands](docs/testing.md), and the [visual review artifact workflow](.github/workflows/visual-artifact-factory.yml). Production status, scope, roadmap, blockers, start procedure, and done criteria live under [`docs/production/`](docs/production/current-status.md).

---


> **Source de vérité**  
> «Le README est la source de vérité du projet. En cas de contradiction entre une suggestion, un ticket, un prompt ou une implémentation et ce document, le README prévaut jusqu’à décision humaine explicite.»

TitanCraft est un nouveau projet de jeu vidéo développé avec une contrainte forte de temps, de simplicité et de clarté. Ce document sert de référence principale pour tous les développeurs humains et agents IA travaillant sur le dépôt.

Les agents IA comme Codex Pro et Claude Code Pro peuvent accélérer l’écriture, l’analyse, la correction et la documentation du code. Ils ne remplacent pas les tests humains de gameplay dans Godot, en particulier pour juger la lisibilité, les sensations de déplacement, le rythme du combat et la compréhension des objectifs.

## 1. Identité du projet

| Élément | Décision |
|---|---|
| Nom du projet | TitanCraft |
| Type de jeu | FPS de survie, exploration et progression mécanique |
| Plateforme initiale | Windows PC |
| Mode initial | Solo intégralement hors ligne |
| Positionnement technique | Offline-first, Windows-first, single-player-first |
| Direction artistique | Science-fiction réaliste simplifiée avec formes polygonales lisibles |
| Public cible | Joueurs habitués aux FPS |
| Contrainte de production | Environ 5 heures de développement par week-end |
| Outils d’assistance | Codex Pro, Claude Code Pro, GitHub, Godot, Git, éventuellement Blender |
| Équipe | Un parent avec son fils |

## 2. Phrase centrale du jeu

> «TitanCraft est un FPS de survie solo dans lequel un astronaute échoué sur une planète hostile chasse des Galaxabrains biomécaniques afin de construire un exosquelette, récupérer des composants et préparer son retour vers la Terre.»

Cette phrase est la définition officielle du projet pour le MVP. Elle est verrouillée pour le MVP. Un agent ne doit pas la modifier sans décision humaine explicite.

## 3. Contexte narratif

Le joueur incarne un astronaute dont le vaisseau s’est écrasé sur une planète inconnue. La planète semble habitable, mais elle est dominée par des créatures biomécaniques appelées les **Galaxabrains**.

Les Galaxabrains combinent des composants organiques et métalliques. Ils attaquent les êtres vivants et cherchent à capturer le joueur.

Le joueur commence sans exosquelette fonctionnel. Il doit progressivement :

- explorer le site du crash ;
- survivre à l’environnement ;
- récupérer des matériaux ;
- fabriquer des équipements ;
- construire progressivement un exosquelette ;
- combattre les Galaxabrains ;
- restaurer des moyens de communication ;
- préparer, à long terme, un véhicule capable de quitter la planète.

Dans le MVP, le joueur ne construit pas encore une fusée complète. La fusée représente l’objectif narratif de la campagne longue, pas celui du premier prototype.

### Règles de propriété créative

Ne pas utiliser les noms **Jarvis**, **SpaceX**, **Titanfall**, **Minecraft** ou d’autres marques comme éléments internes du jeu.

Le projet peut être inspiré par les sensations de mobilité, de progression et de construction de certains jeux existants, mais il doit conserver :

- ses propres noms ;
- ses propres personnages ;
- ses propres créatures ;
- ses propres designs ;
- ses propres assets ;
- ses propres mécaniques combinées.

L’assistant embarqué futur de l’exosquelette doit utiliser un nom original. Le nom de travail temporaire est **NOVA** jusqu’à décision contraire.

## 4. Vision long terme

La vision complète de TitanCraft peut inclure plus tard :

- une campagne continue de plusieurs heures ;
- plusieurs régions connectées ;
- différents biomes ;
- plusieurs types de Galaxabrains ;
- un exosquelette évolutif ;
- un grand mécha ou véhicule lourd ;
- une intelligence embarquée ;
- un grappin ;
- la course murale ;
- une base plus développée ;
- plusieurs structures ;
- une économie de ressources avancée ;
- la construction d’un véhicule orbital ;
- une coopération en ligne à deux joueurs ;
- des missions secondaires ;
- des boss ;
- une progression de compétences ;
- des équipements persistants.

Cette section décrit uniquement la vision long terme. Elle ne constitue pas une autorisation de développer ces fonctionnalités dans le MVP.

## 5. Définition exacte du MVP

| Élément | Définition |
|---|---|
| Nom interne du MVP | Crash Site |
| Objectif | Créer une boucle jouable courte dans laquelle le joueur explore une petite zone, collecte des composants, construit un bras mécanique et élimine un Galaxabrain avant d’activer une balise |
| Durée cible d’une session | Entre 10 et 30 minutes |
| Condition de victoire | Construire le bras, vaincre le Galaxabrain, récupérer le composant, activer la balise, afficher un écran de victoire |
| Condition de défaite | Le joueur perd tous ses points de vie |
| Après la mort | Le joueur revient au dernier point de sauvegarde |

### Condition de victoire détaillée

1. Explorer le site du crash.
2. Trouver les ressources nécessaires.
3. Construire le premier module de bras mécanique.
4. Utiliser ce bras pour éliminer un Galaxabrain.
5. Récupérer son composant.
6. Activer la balise de secours.
7. Afficher un écran de victoire.

### Contenu autorisé dans le MVP

Le MVP doit contenir uniquement :

- une petite carte jouable ;
- un personnage en première personne ;
- déplacement ;
- caméra FPS ;
- saut simple ;
- points de vie ;
- une attaque de base ;
- un bras mécanique constructible ;
- un type de Galaxabrain ;
- une IA ennemie simple ;
- trois ressources ;
- un inventaire minimal ;
- une recette de fabrication ;
- un établi ou point de fabrication ;
- un point de sauvegarde ;
- une balise ;
- une mission ;
- une condition de victoire ;
- une condition de défaite ;
- une sauvegarde locale ;
- un menu principal minimal ;
- un menu pause ;
- un build Windows hors ligne.

## 6. Hors périmètre du MVP

> [!WARNING]
> Les fonctionnalités ci-dessous sont explicitement interdites dans le MVP. Une fonctionnalité hors périmètre ne peut être ajoutée qu’après validation humaine et mise à jour de ce README.

- le multijoueur ;
- les serveurs ;
- les comptes utilisateurs ;
- les services cloud ;
- les microtransactions ;
- le monde infini ;
- la génération procédurale complète ;
- le système de blocs voxel ;
- la destruction totale du terrain ;
- le grand mécha ;
- la fusée complète ;
- le grappin ;
- la course murale ;
- les véhicules ;
- les arbres de compétences ;
- les classes de personnages ;
- les quêtes secondaires ;
- plusieurs biomes ;
- plusieurs cartes ;
- plusieurs armes ;
- plusieurs types d’ennemis ;
- les compagnons contrôlés par IA ;
- l’intelligence artificielle générative intégrée au jeu ;
- la synthèse vocale ;
- la reconnaissance vocale ;
- les cinématiques complexes ;
- le doublage ;
- le support console ;
- le support mobile ;
- le support Linux ou macOS garanti ;
- les mods ;
- Steam Workshop ;
- les classements ;
- les succès en ligne ;
- l’anti-triche ;
- la télémétrie distante.

## 7. Boucle de gameplay du MVP

La boucle principale doit rester linéaire et compréhensible :

1. Le joueur apparaît près du vaisseau écrasé.
2. Il reçoit l’objectif de trouver des ressources.
3. Il explore une zone limitée.
4. Il collecte du métal, de la biomasse et des composants électroniques.
5. Il retourne à l’établi.
6. Il construit un bras mécanique.
7. Il rencontre un Galaxabrain.
8. Il utilise le bras mécanique pour le combattre.
9. Il récupère un composant spécifique.
10. Il active la balise.
11. Il termine la mission.

Le joueur doit toujours pouvoir identifier son prochain objectif. Le MVP ne doit pas dépendre d’un tutoriel textuel long.

Utiliser en priorité :

- des objectifs courts ;
- des marqueurs simples ;
- des changements visuels ;
- des messages contextuels ;
- des sons temporaires si nécessaire.

## 8. Ressources du MVP

Le MVP utilise exactement trois catégories principales de ressources.

| Ressource | Utilisation | Sources possibles |
|---|---|---|
| Métal | Fabrication mécanique, réparation, structure du bras mécanique | Débris du vaisseau, morceaux de machines, éléments métalliques présents dans la zone |
| Biomasse | Composants organiques, soins simples, interface entre matière organique et technologie | Plantes, éléments organiques, restes de Galaxabrains |
| Composants électroniques | Circuits, activation du bras, activation de la balise | Épave, machines abandonnées, ennemi vaincu |

Le système de ressources doit rester minimal :

- pas de poids d’inventaire ;
- pas de rareté complexe ;
- pas de commerce ;
- pas de monnaies multiples.

## 9. Inventaire

L’inventaire du MVP doit être simple. Il doit au minimum stocker :

- la quantité de métal ;
- la quantité de biomasse ;
- la quantité de composants électroniques ;
- l’état de construction du bras mécanique ;
- le composant spécial récupéré sur l’ennemi.

Le joueur ne doit pas déplacer des objets entre des cases. Une interface de type compteur est suffisante.

Le système doit être piloté par les données quand cela reste simple. Éviter une architecture générale d’inventaire conçue pour des centaines d’objets.

## 10. Construction et fabrication

Le joueur ne pose pas des blocs librement. Le système de fabrication utilise des structures ou recettes prédéfinies.

Le MVP possède une seule fabrication centrale : **Bras mécanique Mk I**.

Coût provisoire :

| Ressource | Quantité |
|---|---:|
| Métal | 10 |
| Biomasse | 3 |
| Composants électroniques | 2 |

Le coût doit être configurable dans une ressource ou un fichier de données et ne doit pas être codé dans plusieurs scripts différents.

Le bras mécanique permet :

- d’effectuer une attaque ;
- d’infliger des dégâts au Galaxabrain ;
- de représenter la première étape de l’exosquelette.

Le bras mécanique n’ajoute pas encore :

- de grappin ;
- de propulsion ;
- de course wall-run ;
- de bouclier ;
- de capacité de construction avancée.

Le joueur fabrique le bras à un établi fixe. L’établi ne peut produire qu’une seule recette dans le MVP.

## 11. Système de combat

| Élément | Décision |
|---|---|
| Perspective | Première personne |
| Arme principale | Bras mécanique de l’exosquelette |
| Avant fabrication | Le joueur peut être incapable d’endommager efficacement le Galaxabrain ou disposer d’une attaque faible temporaire |
| Priorité | Lisibilité |

Le système doit inclure :

- une entrée d’attaque ;
- une portée définie ;
- un délai entre les attaques ;
- des dégâts ;
- des points de vie ennemis ;
- une réaction visuelle simple ;
- la mort de l’ennemi ;
- la génération du composant de mission.

À éviter :

- les combos ;
- les attaques chargées ;
- les armes alternatives ;
- les dégâts élémentaires ;
- les statuts ;
- les parades complexes ;
- les démembrements ;
- les arbres d’animation complexes.

Valeurs provisoires configurables :

| Paramètre | Valeur de départ |
|---|---:|
| Points de vie joueur | 100 |
| Points de vie Galaxabrain | 100 |
| Dégâts attaque du bras | 25 |
| Délai entre attaques | 0,8 seconde |

Ces valeurs sont provisoires et doivent être ajustées pendant les tests.

## 12. Galaxabrain du MVP

| Élément | Définition |
|---|---|
| Archétype | Un seul ennemi |
| Nom provisoire | Galaxabrain Scout |
| Description | Créature biomécanique de taille moyenne, mélange de matière organique et de plaques métalliques |
| Rôle | Ennemi de base servant à valider le combat, la navigation et la mission |

Comportements minimums :

- état inactif ;
- détection du joueur ;
- poursuite ;
- attaque à courte portée ;
- réception de dégâts ;
- mort ;
- abandon du composant de mission.

Architecture recommandée :

- machine à états explicite ;
- états limités ;
- transitions lisibles ;
- paramètres configurables.

États possibles :

- `Idle` ;
- `Chase` ;
- `Attack` ;
- `Dead`.

L’IA doit utiliser une logique simple et déterministe. Ne pas intégrer de modèle d’intelligence artificielle externe. Le terme « dopé à l’IA » est narratif dans le MVP.

La navigation peut utiliser le système de navigation 3D du moteur, à condition que la carte reste simple.

L’ennemi ne doit pas :

- grimper ;
- voler ;
- construire ;
- apprendre ;
- coordonner une meute ;
- utiliser des attaques à distance ;
- modifier dynamiquement le terrain.

## 13. Joueur

| Élément | Décision |
|---|---|
| Départ | À pied |
| Perspective | Première personne uniquement |
| Équipement initial | Pas d’exosquelette fonctionnel |

Contrôles minimums :

- avancer ;
- reculer ;
- déplacement latéral ;
- regarder avec la souris ;
- sauter ;
- interagir ;
- attaquer ;
- ouvrir le menu pause.

Le joueur possède :

- une vitesse de marche ;
- une vitesse de course optionnelle simple ;
- une gravité ;
- un saut ;
- des points de vie ;
- un inventaire minimal ;
- un point de réapparition ;
- un bras mécanique après fabrication.

Ne pas ajouter dans le MVP :

- accroupissement ;
- glissade ;
- course murale ;
- grappin ;
- double saut ;
- stamina ;
- faim ;
- soif ;
- température ;
- radiation ;
- membres blessés ;
- poids de l’inventaire.

## 14. Monde et level design

Le MVP utilise une seule zone.

| Élément | Cible |
|---|---|
| Taille indicative | Environ 150 mètres sur 150 mètres |
| Traversée | Rapide |
| Construction | Manuelle, non infinie, non procédurale complète |

La zone doit contenir :

- le vaisseau écrasé ;
- le point d’apparition ;
- un point de sauvegarde ;
- un établi ;
- plusieurs sources de ressources ;
- une zone de rencontre avec le Galaxabrain ;
- une balise ;
- des limites naturelles ou artificielles ;
- quelques éléments verticaux simples ;
- une direction visuelle claire.

Le monde n’est pas infini. Le monde n’est pas généré procéduralement. Le monde n’est pas intégralement destructible.

Le terrain peut être construit avec :

- meshes simples ;
- primitives ;
- assets temporaires ;
- modèles low-poly ;
- matériaux simples.

Les collisions doivent être propres et prévisibles. Les éléments décoratifs ne doivent pas bloquer la progression. Le joueur ne doit pas pouvoir sortir facilement des limites.

## 15. Direction artistique

Style : **science-fiction réaliste simplifiée**.

Principes visuels :

- formes polygonales lisibles ;
- silhouettes fortes ;
- matériaux simples ;
- palette cohérente ;
- contraste clair entre éléments interactifs et décor ;
- interfaces fonctionnelles ;
- priorité à la lisibilité plutôt qu’au réalisme.

Les premiers prototypes peuvent utiliser :

- cubes ;
- capsules ;
- primitives ;
- matériaux unis ;
- assets temporaires clairement identifiés.

Les placeholders sont autorisés. Ils doivent être marqués dans la documentation ou dans leurs noms.

Exemples :

- `Placeholder_PlayerArms` ;
- `Placeholder_GalaxabrainScout` ;
- `Placeholder_Workbench`.

Ne pas retarder le gameplay pour créer des assets définitifs.

## 16. Audio

L’audio n’est pas prioritaire dans les premières étapes.

Le MVP peut utiliser des sons temporaires pour :

- collecte ;
- attaque ;
- dégâts ;
- mort ennemie ;
- fabrication ;
- activation de la balise ;
- victoire.

Aucune musique originale n’est obligatoire. Ne pas intégrer d’assets audio dont la licence est inconnue.

Toute ressource externe doit être accompagnée de sa licence et de sa source dans un fichier dédié, par exemple [`THIRD_PARTY_ASSETS.md`](THIRD_PARTY_ASSETS.md).

## 17. Interface utilisateur

Le MVP doit posséder une interface minimale.

### HUD

- points de vie ;
- objectif actuel ;
- quantités des trois ressources ;
- indication d’interaction ;
- état du bras mécanique.

### Menu principal

- Nouvelle partie ;
- Continuer, uniquement si une sauvegarde existe ;
- Paramètres minimaux ;
- Quitter.

### Menu pause

- Reprendre ;
- Sauvegarder si nécessaire ;
- Retour au menu principal ;
- Quitter.

### Écran de victoire

- message de mission réussie ;
- résumé simple ;
- possibilité de retourner au menu.

### Écran de défaite

- message de mort ;
- rechargement du dernier point de sauvegarde.

Ne pas construire un système complexe d’interface ou de navigation.

## 18. Sauvegarde locale

Le jeu est offline-first. La sauvegarde est locale. Aucun compte, serveur ou accès Internet n’est requis.

Le système de sauvegarde du MVP doit enregistrer au minimum :

- position ou point de réapparition ;
- points de vie si pertinent ;
- quantités des ressources ;
- état de fabrication du bras ;
- état de l’ennemi ;
- composant de mission récupéré ou non ;
- activation de la balise ;
- progression de la mission.

La sauvegarde doit utiliser un format versionné.

Exemple conceptuel :

```json
{
  "save_version": 1,
  "checkpoint_id": "crash_site",
  "resources": {
    "metal": 0,
    "biomass": 0,
    "electronics": 0
  },
  "mechanical_arm_built": false,
  "galaxabrain_defeated": false,
  "mission_component_collected": false,
  "beacon_activated": false
}
```

Ne pas ajouter de chiffrement complexe.

Le jeu doit gérer proprement :

- l’absence de sauvegarde ;
- une sauvegarde partiellement invalide ;
- une version de sauvegarde inconnue ;
- le redémarrage d’une nouvelle partie.

## 19. Moteur et langage

| Élément | Décision |
|---|---|
| Moteur recommandé | Godot 4 avec prise en charge .NET |
| Langage principal | C# |
| Version exacte | À consigner après installation dans la section Versions validées |

Le projet doit utiliser une version stable de Godot 4 .NET compatible avec l’environnement local. Ce README ne fixe pas de numéro de version tant qu’il n’a pas été validé sur la machine réelle.

### Versions validées

| Outil | Version validée | Notes |
|---|---|---|
| Godot | À renseigner après installation | Utiliser Godot 4 .NET stable |
| .NET SDK | À renseigner après installation | Doit être compatible avec la version Godot utilisée |
| OS de développement | Windows | À préciser avec la version exacte |

### Raisons du choix

- moteur léger ;
- adapté à un petit projet ;
- export Windows ;
- C# fortement typé ;
- fichiers de scènes relativement lisibles ;
- bonne compatibilité avec Git ;
- architecture accessible aux agents de code ;
- absence de dépendance obligatoire à un service en ligne.

Le projet ne doit pas utiliser simultanément C# et GDScript sans justification explicite. C# est le langage principal. GDScript ne doit être introduit que si une raison technique forte est documentée.

## 20. Architecture technique

L’architecture doit rester simple et non surdimensionnée.

Principes :

- composants courts ;
- responsabilités explicites ;
- dépendances limitées ;
- données configurables ;
- scènes réutilisables ;
- signaux ou événements pour réduire le couplage ;
- pas de framework interne complexe ;
- pas d’abstraction créée uniquement pour anticiper des fonctionnalités futures ;
- utiliser les fonctionnalités natives du moteur avant de créer un système personnalisé.

### Organisation conceptuelle recommandée

| Domaine | Responsabilités |
|---|---|
| Core | Gestion du jeu, changement de scène, sauvegarde, progression de mission, configuration |
| Player | Mouvement, santé, interaction, attaque, inventaire, module de bras mécanique |
| Enemies | Santé de base, machine à états, détection, poursuite, attaque, mort, butin |
| Resources | Définition des ressources, objets collectables, compteurs |
| Crafting | Recette, validation du coût, construction du bras, interface de fabrication |
| Missions | Objectifs, progression, condition de victoire |
| UI | HUD, menus, écrans de victoire et de défaite |
| World | Site du crash, balise, établi, point de sauvegarde, zones de collecte |

## 21. Arborescence actuelle

Arborescence officielle du projet :

```text
TitanCraft/
├── README.md
├── AGENTS.md
├── CLAUDE.md
├── PROJECT_DIRECTOR_START_HERE.md
├── THIRD_PARTY_ASSETS.md
├── THIRD_PARTY_DEPENDENCIES.md
├── project.godot
├── export_presets.cfg
├── TitanCraft.csproj
├── TitanCraft.sln
├── .github/
│   ├── pull_request_template.md
│   └── workflows/
├── .gitignore
├── art/
│   └── blender/
├── artifacts/
│   ├── asset-review/
│   ├── mvp_closure/
│   ├── review/
│   ├── source-archives/
│   ├── source-files/
│   ├── runtime-contract-report.json
│   └── terrain-generation-report.json
├── docs/
│   ├── architecture/
│   ├── art/
│   ├── debug/
│   ├── implementation/
│   ├── pipeline/
│   ├── production/
│   ├── release/
│   ├── review/
│   ├── testing/
│   ├── testing.md
│   ├── material-library.md
│   ├── asset-policy.md
│   ├── performance-baseline.md
│   └── [various review and planning documents]
├── src/
│   ├── Core/
│   ├── Player/
│   ├── Enemies/
│   ├── Resources/
│   ├── Crafting/
│   ├── Missions/
│   ├── SaveSystem/
│   ├── UI/
│   ├── World/
│   └── Tools/
├── scenes/
│   ├── Main/
│   ├── Player/
│   ├── Enemies/
│   ├── World/
│   ├── Environment/
│   ├── Props/
│   │   ├── Wreckage/
│   │   ├── Human/
│   │   └── Environment/
│   ├── Resources/
│   ├── UI/
│   ├── Debug/
│   │   └── VisualReview/
│   └── Proof/
├── assets/
│   ├── Production/
│   │   ├── Custom/
│   │   │   └── StageA/
│   │   ├── Generated/
│   │   ├── Player/
│   │   │   └── MechanicalArm/
│   │   ├── Environment/
│   │   │   └── CrashSite/
│   │   └── Enemies/
│   │       └── Galaxabrain/
│   └── ThirdParty/
│       ├── Kenney/
│       │   ├── NatureKit/
│       │   └── ModularSpaceKit/
│       └── KayKit/
│           └── SpaceBaseBits/
├── data/
│   └── Recipes/
├── tests/
│   ├── Unit/
│   └── Integration/
├── tools/
│   ├── agent_preflight.py
│   ├── agent_task_router.py
│   ├── validate_agent_studio.py
│   ├── blender/
│   └── [validation and CI scripts]
└── studio/
    ├── README.md
    ├── agents/
    ├── memory/
    ├── skills/
    ├── decisions/
    ├── prompts/
    ├── checklists/
    ├── templates/
    ├── rehearsals/
    └── indexes/
```

La structure reflète les besoins du projet :
- **assets/** est organisée par statut (Production vs ThirdParty) et rôle (Player, Environment, Enemies) plutôt que par type.
- **data/** contient actuellement les Recipes ; Items, Enemies, Missions peuvent être ajoutés selon les besoins.
- **docs/** contient les documents de production organisés par domaine (art, pipeline, production, release, etc.).
- **src/Tools** contient les scripts utilitaires C# pour la production.
- **studio/** est le système opérationnel local Agent Studio pour la gouvernance et le routage.
- **tests/** contient les tests Unit et Integration (les tests manuels sont documentés dans docs/testing/).
- **artifacts/** stocke les résultats de génération d’assets et les rapports de validation.
- **tools/** contient les scripts Python pour l’automatisation, la validation et le routage des tâches.

Tout changement important doit être documenté dans [`studio/decisions/`](studio/decisions/) ou [`docs/architecture/`](docs/architecture/).

## 22. Conventions C#

Utiliser :

- noms de classes en `PascalCase` ;
- méthodes en `PascalCase` ;
- propriétés en `PascalCase` ;
- variables locales en `camelCase` ;
- champs privés avec une convention unique ;
- namespaces cohérents ;
- nullable reference types si compatibles avec le projet ;
- types explicites quand ils améliorent la compréhension ;
- commentaires uniquement pour expliquer le pourquoi ;
- méthodes courtes ;
- classes à responsabilité unique ;
- paramètres exportés pour les valeurs de gameplay ;
- logs utiles mais limités.

Éviter :

- les classes géantes ;
- les singletons excessifs ;
- les dépendances cachées ;
- les nombres magiques ;
- les chemins de nœuds répétés dans plusieurs scripts ;
- la logique de gameplay dans l’interface ;
- les appels réseau ;
- les bibliothèques non nécessaires ;
- les patterns complexes sans besoin réel.

Toute nouvelle dépendance externe doit être approuvée et documentée.

## 23. Règles de travail pour les agents IA

Règles obligatoires :

1. Lire `README.md` avant toute modification.
2. Lire `AGENTS.md` ou `CLAUDE.md` selon l’agent utilisé.
3. Ne pas ajouter de fonctionnalité hors MVP.
4. Ne pas modifier le concept central.
5. Ne pas créer plus d’abstraction que nécessaire.
6. Ne pas réécrire plusieurs systèmes pour une petite correction.
7. Travailler sur une seule tâche claire à la fois.
8. Compiler après chaque modification significative.
9. Signaler les hypothèses.
10. Documenter les décisions structurelles.
11. Ajouter ou mettre à jour les tests pertinents.
12. Ne jamais considérer une tâche comme terminée uniquement parce que le code compile.
13. Fournir une procédure de test manuel.
14. Ne pas modifier des assets binaires inutilement.
15. Ne pas introduire de services en ligne.
16. Ne pas ajouter de télémétrie.
17. Ne pas ajouter de dépendance sans justification.
18. Ne pas utiliser de code ou d’assets dont la licence est inconnue.
19. Garder les commits petits et compréhensibles.
20. Arrêter et signaler toute contradiction avec le README.

Workflow attendu :

```text
Lire la tâche
→ inspecter le code existant
→ proposer un plan court
→ modifier le minimum de fichiers
→ compiler
→ exécuter les tests
→ fournir un test manuel
→ résumer les changements
```

Quand deux agents sont utilisés :

```text
Un agent implémente
→ le second agent révise
→ le projet compile
→ un humain teste dans Godot
→ les changements sont commités
```

Les deux agents ne doivent pas modifier simultanément la même fonctionnalité.

## 24. Git et GitHub

Recommandations :

- dépôt Git privé au début ;
- branche principale stable ;
- branches courtes par fonctionnalité ;
- commits atomiques ;
- Pull Requests pour les changements importants ;
- revue par un second agent ou un humain ;
- Git LFS uniquement si de gros fichiers binaires le rendent nécessaire.

Conventions de branches possibles :

```text
feature/player-movement
feature/resource-collection
feature/mechanical-arm
fix/enemy-navigation
docs/update-readme
```

Conventions de commits possibles :

```text
feat: add basic player movement
fix: prevent resource pickup duplication
docs: define MVP acceptance criteria
test: add save data validation
refactor: simplify enemy state transitions
```

Ne pas commiter :

- builds temporaires ;
- caches Godot ;
- fichiers générés inutiles ;
- secrets ;
- tokens ;
- clés API ;
- dossiers IDE non nécessaires.

Un `.gitignore` adapté à Godot, C#, .NET et aux IDE utilisés doit être présent.

## 25. Installation de l’environnement Windows

> Les commandes finales doivent être testées dans le dépôt réel avant d’être considérées comme officielles. Ne pas inventer de commande spécifique à un outil lorsque la commande dépend de la version installée.

### Étapes recommandées

1. Installer Git depuis la source officielle adaptée à Windows.
2. Installer Godot 4 .NET en version stable.
3. Installer le SDK .NET compatible avec la version Godot utilisée.
4. Installer Visual Studio Code ou un IDE C# approprié.
5. Installer Codex selon l’environnement de l’utilisateur.
6. Installer Claude Code selon l’environnement de l’utilisateur.
7. Cloner le dépôt.
8. Ouvrir le projet dans Godot.
9. Restaurer ou compiler le projet C#.
10. Lancer la scène principale.
11. Exécuter les tests.
12. Exporter Windows.

### Commandes de base à valider

```bash
git clone <REPOSITORY_URL>
cd TitanCraft
dotnet restore
dotnet build
```

Si ces commandes changent selon la version de Godot ou l’organisation finale du projet, mettre à jour cette section et [`docs/testing.md`](docs/testing.md).

## 26. Lancer le projet

Procédure attendue une fois le projet Godot créé :

1. Ouvrir Godot 4 .NET.
2. Importer ou ouvrir le dossier `TitanCraft/`.
3. Vérifier que `project.godot` est reconnu.
4. Restaurer les dépendances C# si Godot le demande.
5. Compiler le projet C#.
6. Lancer la scène principale.
7. Vérifier que le joueur apparaît dans la scène du MVP.

Commandes indicatives à valider :

```bash
dotnet restore
dotnet build
```

## 27. Export Windows

Le MVP doit produire une version Windows jouable hors ligne.

Critères :

- lancement sans IDE ;
- aucune connexion Internet nécessaire ;
- aucune clé API ;
- aucune authentification ;
- sauvegarde locale fonctionnelle ;
- fermeture propre ;
- reprise depuis une sauvegarde ;
- dossier de build identifiable ;
- instructions d’installation simples.

Répertoire de sortie recommandé lors de l'export Godot :

```text
builds/Windows/TitanCraft-MVP/
```

Le répertoire `builds/` n'existe que localement après export et ne doit pas être commité dans Git. Un `.gitignore` approprié exclut ce répertoire.

### Checklist d’export Windows

- [ ] Les presets d’export Godot sont configurés.
- [ ] L’export Windows cible `builds/Windows/TitanCraft-MVP/`.
- [ ] Le projet compile avant export.
- [ ] L’exécutable se lance sans Godot.
- [ ] Le jeu fonctionne sans Internet.
- [ ] Aucun compte utilisateur n’est demandé.
- [ ] La sauvegarde locale fonctionne dans le build.
- [ ] Quitter le jeu ferme proprement l’application.
- [ ] Reprendre une partie fonctionne après relance.
- [ ] Le dossier de build n’est pas commité par défaut.

## 28. Critères de performance

| Élément | Cible |
|---|---|
| Machine cible | PC Windows moyen |
| Objectif | 60 images par seconde dans la scène du MVP sur la machine de test principale |
| Minimum tolérable provisoire | 30 images par seconde stables |
| Ennemis actifs | 1 ennemi obligatoire |

Une extension à 2 ou 3 ennemis n’est permise qu’après validation du premier ennemi.

Le projet doit éviter :

- trop de lumières dynamiques ;
- trop d’ombres ;
- textures surdimensionnées ;
- meshes inutilement complexes ;
- logique exécutée chaque frame sans besoin ;
- trop d’ennemis ;
- effets visuels coûteux.

## 29. Tests

### Tests de compilation

- le projet C# compile ;
- aucune erreur bloquante ;
- les scènes principales chargent.

### Tests automatisés

Priorité aux systèmes purs :

- calcul des ressources ;
- validation d’une recette ;
- consommation des ressources ;
- sérialisation de sauvegarde ;
- validation des données ;
- progression de mission.

### Tests manuels

Checklist minimale :

- [ ] Nouvelle partie.
- [ ] Déplacement.
- [ ] Caméra.
- [ ] Saut.
- [ ] Collecte de métal.
- [ ] Collecte de biomasse.
- [ ] Collecte de composants électroniques.
- [ ] Inventaire.
- [ ] Fabrication du bras.
- [ ] Attaque.
- [ ] Détection de l’ennemi.
- [ ] Poursuite.
- [ ] Dégâts.
- [ ] Mort ennemie.
- [ ] Récupération du composant.
- [ ] Activation de la balise.
- [ ] Victoire.
- [ ] Mort du joueur.
- [ ] Rechargement.
- [ ] Sauvegarde.
- [ ] Fermeture.
- [ ] Reprise.
- [ ] Export Windows.

Chaque fonctionnalité doit fournir :

- son comportement attendu ;
- sa méthode de test ;
- son critère de réussite ;
- ses cas d’échec principaux.

## 30. Critères d’acceptation globaux du MVP

Le MVP est terminé uniquement lorsque :

- [ ] le projet se lance depuis Godot ;
- [ ] le projet C# compile sans erreur ;
- [ ] le joueur peut marcher ;
- [ ] le joueur peut regarder autour de lui ;
- [ ] le joueur peut sauter ;
- [ ] le joueur peut collecter trois types de ressources ;
- [ ] les quantités sont affichées ;
- [ ] le joueur peut utiliser l’établi ;
- [ ] le joueur peut construire le bras mécanique ;
- [ ] les ressources sont correctement consommées ;
- [ ] le Galaxabrain détecte le joueur ;
- [ ] le Galaxabrain poursuit le joueur ;
- [ ] le Galaxabrain attaque ;
- [ ] le joueur peut subir des dégâts ;
- [ ] le joueur peut attaquer avec le bras ;
- [ ] le Galaxabrain peut mourir ;
- [ ] le composant de mission apparaît ;
- [ ] le joueur peut le récupérer ;
- [ ] la balise peut être activée ;
- [ ] l’écran de victoire apparaît ;
- [ ] le joueur réapparaît après sa mort ;
- [ ] la sauvegarde locale fonctionne ;
- [ ] la partie peut être reprise ;
- [ ] le jeu fonctionne sans connexion Internet ;
- [ ] un build Windows peut être lancé en dehors de Godot ;
- [ ] la boucle complète peut être terminée en moins de 30 minutes ;
- [ ] aucun bug bloquant connu n’empêche de terminer la mission.

## 31. Budget de production

| Élément | Cible |
|---|---|
| Temps disponible | Environ 5 heures par week-end |
| Budget cible initial | 80 heures |
| Nombre de cycles cible | 16 week-ends |

Plan indicatif :

| Week-ends | Focus | Résultat attendu |
|---|---|---|
| 1 à 2 | Capsule technique | Projet créé, dépôt configuré, personnage, caméra, saut, build Windows minimal |
| 3 à 5 | Combat | Santé, attaque, ennemi, dégâts, mort |
| 6 à 8 | Collecte | Ressources, objets ramassables, compteurs, inventaire minimal |
| 9 à 11 | Fabrication | Établi, recette, consommation des ressources, bras mécanique |
| 12 à 14 | Mission | Objectifs, composant spécial, balise, sauvegarde, victoire |
| 15 à 16 | Stabilisation | Correction des bugs, optimisation, tests, export Windows, documentation |

Règle de contrôle :

> «Toute fonctionnalité qui demande plus de deux week-ends doit être réduite, divisée ou supprimée.»

## 32. Définition de « terminé »

Une tâche n’est pas terminée parce que le code existe.

Une tâche est terminée lorsque :

- son code est présent ;
- le projet compile ;
- la fonctionnalité fonctionne dans Godot ;
- les erreurs principales sont gérées ;
- le test manuel est documenté ;
- les tests automatisés pertinents passent ;
- aucune fonctionnalité existante n’est cassée ;
- la documentation est mise à jour ;
- le changement respecte le périmètre du MVP.

## 33. Risques principaux

| Risque | Impact | Probabilité | Stratégie de réduction |
|---|---|---|---|
| Périmètre trop large | MVP jamais terminé | Élevée | Respecter strictement le hors périmètre et réduire toute tâche trop grande |
| Ajout prématuré du multijoueur | Complexité technique majeure | Moyenne | Interdire multijoueur, serveurs et comptes dans le MVP |
| Architecture excessive | Perte de temps, code difficile à modifier | Élevée | Préférer composants courts, scènes simples et données configurables |
| Assets trop ambitieux | Retard du gameplay | Moyenne | Utiliser placeholders nommés clairement |
| IA ennemie instable | Combat injouable | Moyenne | Machine à états déterministe avec peu d’états |
| Bugs de sauvegarde | Perte de progression | Moyenne | Format versionné, tests de sérialisation, gestion des sauvegardes invalides |
| Agents IA modifiant trop de fichiers | Régressions et confusion | Élevée | Une tâche claire, minimum de fichiers, revue humaine ou second agent |
| Dépendances incompatibles | Blocage compilation/export | Moyenne | Approuver et documenter chaque dépendance |
| Baisse de performance | Expérience FPS dégradée | Moyenne | Limiter lumières, ennemis, meshes et logique par frame |
| Absence de tests humains | Gameplay techniquement correct mais mauvais | Élevée | Tester dans Godot à chaque fonctionnalité importante |
| Perte de motivation | Abandon du projet | Moyenne | Week-ends courts, objectifs visibles, victoires rapides |
| Objectifs de week-end trop grands | Frustration et dette technique | Élevée | Diviser, réduire ou supprimer toute fonctionnalité dépassant deux week-ends |

## 34. Décisions verrouillées

| Sujet | Décision |
|---|---|
| Nom | TitanCraft |
| Genre | FPS de survie et exploration |
| Plateforme initiale | Windows |
| Mode du MVP | Solo hors ligne |
| Perspective | Première personne |
| Moteur | Godot 4 .NET |
| Langage | C# |
| Direction artistique | Science-fiction réaliste simplifiée |
| Monde | Une petite zone créée manuellement |
| Personnage | Astronaute à pied |
| Équipement principal | Bras mécanique évoluant vers un exosquelette |
| Ennemi du MVP | Un Galaxabrain Scout |
| Ressources | Métal, biomasse, composants électroniques |
| Construction | Recettes et structures prédéfinies |
| Mort | Retour au dernier point de sauvegarde |
| Objectif du MVP | Construire le bras, vaincre l’ennemi et activer la balise |
| Connexion Internet | Non requise |
| Multijoueur | Hors périmètre |
| Temps disponible | 5 heures par week-end |
| Budget MVP | Environ 80 heures |

## 35. Décisions ouvertes

Ces décisions ne sont pas encore verrouillées. Ne pas inventer de réponse dans le code ou la documentation. Ne pas bloquer le démarrage du prototype sur ces décisions lorsqu’elles ne sont pas nécessaires immédiatement.

- âge du fils ;
- niveau de programmation des membres de l’équipe ;
- configuration exacte du PC principal ;
- modèles 3D créés en interne ou assets existants ;
- licence finale du projet ;
- nom définitif de l’assistant embarqué ;
- palette visuelle ;
- paramètres définitifs de combat ;
- structure exacte des fichiers de sauvegarde ;
- outil de tests automatisés ;
- méthode finale de distribution du build Windows.

## 36. Première tâche de développement

Créer une capsule technique Godot 4 .NET contenant :

- une scène principale ;
- un sol ;
- une caméra FPS ;
- un joueur contrôlable ;
- marche ;
- rotation à la souris ;
- saut ;
- collision ;
- une lumière ;
- un bouton ou raccourci pour quitter ;
- un export Windows minimal.

Critères d’acceptation :

- [ ] le projet compile ;
- [ ] le joueur peut se déplacer ;
- [ ] le joueur ne traverse pas le sol ;
- [ ] la souris contrôle la caméra ;
- [ ] le saut fonctionne ;
- [ ] le jeu peut être quitté proprement ;
- [ ] un exécutable Windows peut être généré ;
- [ ] le build fonctionne sans Internet.

Ne pas commencer l’ennemi, les ressources ou la fabrication avant validation de cette capsule technique.

## 37. Documentation principale

Documentation organisée par domaine :

- [`docs/architecture/`](docs/architecture/) : décisions techniques et organisation du projet ;
- [`docs/testing.md`](docs/testing.md) : procédures de test automatisé et manuel ;
- [`docs/testing/`](docs/testing/) : résultats et rapports de tests ;
- [`docs/production/`](docs/production/) : statut de production, roadmap, blockers, définition de terminé ;
- [`docs/art/`](docs/art/) : direction artistique, briefs, statut, guides d'exécution ;
- [`docs/pipeline/`](docs/pipeline/) : Blender Asset Forge, Visual Artifact Factory, processus d'assets ;
- [`docs/release/`](docs/release/) : préparation de la démo publique, checklist de release ;
- [`docs/review/`](docs/review/) : résultats et diagnostics des reviews visuelles ;
- [`studio/decisions/`](studio/decisions/) : Architecture Decision Records et décisions structurantes ;
- [`THIRD_PARTY_ASSETS.md`](THIRD_PARTY_ASSETS.md) : licences et sources des assets externes ;
- [`THIRD_PARTY_DEPENDENCIES.md`](THIRD_PARTY_DEPENDENCIES.md) : licences des dépendances logicielles.

## Checklist avant toute Pull Request

- [ ] J’ai lu le README.
- [ ] La modification appartient au MVP.
- [ ] Je n’ai pas ajouté de fonctionnalité hors périmètre.
- [ ] Le projet compile.
- [ ] Les tests pertinents passent.
- [ ] J’ai testé manuellement la fonctionnalité.
- [ ] J’ai documenté les hypothèses.
- [ ] J’ai mis à jour la documentation nécessaire.
- [ ] Je n’ai ajouté aucun secret.
- [ ] Je n’ai ajouté aucune dépendance injustifiée.
- [ ] Le build reste jouable hors ligne.
- [ ] Les critères d’acceptation sont satisfaits.
