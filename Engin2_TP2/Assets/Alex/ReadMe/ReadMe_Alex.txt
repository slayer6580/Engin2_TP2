--------------------------------------------------------------------------------------------------------------
----- Character ----------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------

Scripts
-------
Character Spawn Point: Ce script sert a garder la position du spawn en mémoire pour le respawn.

Character Color: Ce script sert a changer la couleur du personnage a partir de l'index de son startPoint.
J'utilise le Resources.Load pour aller chercher mes matérials, si ya un souci de compréhension, demandez moi ca va faire plaisir.

CharacterControlBasic: Script temporaire de controle pour tester le réseau.

PlayerSetup: (Pour le réseau) Ca va désactiver les component des autres clients que tu pourrais y avoir accès sinon.
Tu veux pas que quand tu bouge, ca fait bouger les autres aussi en meme temps.
Il sert aussi a donner un nom au personnage dans la scene selon son networkId.


--------------------------------------------------------------------------------------------------------------
----- StartPoint X3 ---------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------

Description
-----------
J'ai mit dans la scene 3 StartPoint qui représente la position possible du début de chaque joueur. 
C'est pour ca que dans le networkManager, le playerSpawnMethod es a Round Robin.


--------------------------------------------------------------------------------------------------------------
----- StartPointManager --------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------

Scripts
-------
StartPointManager: Ce script Singleton sert a donner l'index des spawnsPoints pour le changement de couleur.
Il sert aussi a placer le joueur sous l'onglet -----Players----- dans la hierarchie.
Pas grave si c'est un singleton, c'est juste pour avoir accés rapide sur une méthode polyvalente.

--------------------------------------------------------------------------------------------------------------
----- FinishLine ---------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------

Description
-----------
La ligne d'arrivé des joueurs au sol.

Scripts
-------
FinishLine: Ce script sert a détecter le triggerEnter avec une character pour le retourner à 
son spawnPoint d'origine.
