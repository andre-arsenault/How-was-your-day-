
 You will find two extra scripts

- The Cursor.js script is a very simple script for the mouse cursor. When you want to define an area on the screen that the cursor should change when it hovers and initiate interaction, just add an empty game object (even a simple cube) on the scene. I think it's simpler than doing Raycasting or defining the areas with pixel coordinates. The only disadvantage is to attach it to all interactable objects. 

 You will find an example Toy_object on the scene. Hover the mouse above it and the cursor will change ( I used a random texture from the internet


- I finally managed to find the #@Q$#$@#Q$ texture for the dialogue background now it's black on all boxes.


- We added 3 options for dialogue choices. Whether this choice is a) plot-point b) a good ending c) and the aspect this plot point is related to. 

 A little more in detail...

 We realized that the player will traverse the dialogue tree and at some point he will have to make some choices which will affect the ending of the subplot. We named these choices "plot points" ( we altered the DialogueInstance.js code a little bit). So every time you click on a diaologue option there is a check on whether this option was a plot point . If it is ,it checks on whether the choice was a good ending and updates the score of the related aspect which is stored at score.js.

 I've added an dialogue example to demonstrate how this works. I've also added comments on the code itself. 

=========

Adrian:

I've transcribed the graph into a file called "abuse" under /Resources/DialogGraphs

Feel free to correct, replace etc as required.

I'll probably be back around noon since I need some sleep :3

Don't forget the game profile


========

Dear Adrian:

GET SOME SLEEP. IT'S 8.00 IN THE MORNING.