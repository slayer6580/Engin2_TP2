--------------------------------------------------------------------------------------------------------------
----- Character ----------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------

Scripts
-------
Character Spawn Point: Ce script sert a initialiser le personnage a une position d'un StartPoint
et appeller le changement de couleur.

Character Color: Ce script sert a changer la couleur du personnage a partir de l'index de son startPoint.
J'utilise le Resources.Load pour aller chercher mes matérials, si ya un souci de compréhension, demandez moi ca va faire plaisir.


--------------------------------------------------------------------------------------------------------------
----- StartPoint ---------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------

Description
-----------
J'ai mit dans la scene 3 StartPoint qui représente la position possible du début de chaque joueur.

Scripts
-------
StartPoint: Ce script sert juste a rendre invisible le meshRenderer lors du lancement du jeu.
Le meshRenderer es juste bon pour la création du jeu dans la scene, pour nous pauvre dévelopeur.


--------------------------------------------------------------------------------------------------------------
----- StartPointManager --------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------

Scripts
-------
StartPointManager: Ce script Singleton sert a réserver un startPoint par personnage lors d'une demande du personnage.
Elle a une fonction qui active et qui donne la position du startPoint au personnage,
elle va aussi donner l'index du startPoint au personnage pour déterminer sa couleur par la suite.


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
