# Flipside-game
Unity Project for ACML9
# Gameplay
In this project, you should implement a simple game called Flip-Side . The game consists of two
opposite platforms that are on top of each other. The platforms are parallel to each other and
are arranged such that the bottom platform is facing up and the upper platform is facing down.
Both platforms are infinite (in the forward direction) and include obstacles as well as collectibles.
There are three types of obstacles and two types of collectibles. Collectibles can have different
colors.
The player controls a sphere that is automatically moving forward on one of the two platforms.
The sphere starts the game on the bottom platform. The player can switch platforms during the
game as well as move the sphere left and right. The player starts with three health points and
loses one point each time they hit an obstacle. The game is over whenever the player hits an
obstacle while their health points are 0.
The sphere can have different colors and switches between them automatically. The game has
two modes depending on the platform that the sphere is currently on; Normal mode and Flipped
mode. In the normal mode, the player’s score increases whenever they collect a collectible
whose color matches the sphere’s color and loses score whenever they collect one that doesn’t.
In the flipped mode, it’s the exact opposite; the player’s score decreases whenever they collect
a collectible whose color matches the sphere’s color and gains score points whenever they
collect one that doesn’t. The normal mode is active on the bottom platform while the flipped
mode on the top platform.
