--------------------------------------------------------------------------------------------------------------
----- Character ----------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------

Scripts
-------
Character Spawn Point: Ce script sert a garder la position du spawn en m�moire pour le respawn.

Character Color: Ce script sert a changer la couleur du personnage a partir de l'index de son startPoint.
J'utilise le Resources.Load pour aller chercher mes mat�rials, si ya un souci de compr�hension, demandez moi ca va faire plaisir.

CharacterControlBasic: Script temporaire de controle pour tester le r�seau.

PlayerSetup: (Pour le r�seau) Ca va d�sactiver les component des autres clients que tu pourrais y avoir acc�s sinon.
Tu veux pas que quand tu bouge, ca fait bouger les autres aussi en meme temps.
Il sert aussi a donner un nom au personnage dans la scene selon son networkId.


--------------------------------------------------------------------------------------------------------------
----- StartPoint X3 ---------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------

Description
-----------
J'ai mit dans la scene 3 StartPoint qui repr�sente la position possible du d�but de chaque joueur. 
C'est pour ca que dans le networkManager, le playerSpawnMethod es a Round Robin.


--------------------------------------------------------------------------------------------------------------
----- StartPointManager --------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------

Scripts
-------
StartPointManager: Ce script Singleton sert a donner l'index des spawnsPoints pour le changement de couleur.
Il sert aussi a placer le joueur sous l'onglet -----Players----- dans la hierarchie.
Pas grave si c'est un singleton, c'est juste pour avoir acc�s rapide sur une m�thode polyvalente.

--------------------------------------------------------------------------------------------------------------
----- FinishLine ---------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------

Description
-----------
La ligne d'arriv� des joueurs au sol.

Scripts
-------
FinishLine: Ce script sert a d�tecter le triggerEnter avec une character pour le retourner � 
son spawnPoint d'origine.
