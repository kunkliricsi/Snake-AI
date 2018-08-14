# Snake-AI

This is a project that was required for a job interview. A basic graphical program was given, which would control multiple snakes based on their AI's decisions. My job was to write one of these AIs that could beat all the others. The base program I got is not included in this repository for obvious reasons.

### Requirements
 - Implement the given interface, with which the game determines your snake's decision. The interface has one function, which should return the direction you think your snake should move in.
 - Kill all the other snakes, eat more apples than them, or survive the longest. (Also try your best not to crash the game :P)  
 
 
 ### Description of the base game
 There's given:
 - A x\*y sized map, which contains map elements (Empty, Apple, Wall or Snake's body). Let's call each of these elements nodes. All of this information about the map, and the type of nodes on it is made accessible via public getters.
 - A class that contains all the possible directions.
 - A class that represents all the node possibilites.
 - The interface, which every AI must implement.  
 
 
 ### Solution Description
 So basically there are three types of objects:
  - Path finders, the purpose of which is to return a set of nodes (from start to finish), which represent an empty path between two map nodes.
  - Strategies, that implement the AI interface, which by using different path finders decide on the best direction.
  - Template classes (Abstracts and Interfaces) that provide a base for the different path finders, strategies and so on.  
  
Also, there's a Utilities class, the job of which is to provide basic functions that most of the other classes use.

### Path Finders
As of today, there are two types of path finders implemented. A shortest and a longest path finder, the former uses the A*, while the latter uses the *\<insert-not-yet-implemented-algorithm\>* algorithm.

### Strategies
#### Alternative~
Goes through all the possible directions, returns the one in which its snake could go without dying.
#### FollowTail~
Finds the shortest path from the snake's head to its tail, then returns the direction in which this path goes.
#### GoToMiddle~
Finds the closest available point from the middle of the map to the snake's head, then finds the shortest path between these two points. It then returns the direction the path goes, unless the snake is already *around* the middle.
#### Greedy~
Finds the shortest path between the apple and the snake's head. Moves the snake in that direction... I mean, returns the direction the evaluated path goes blah blah.
#### OpponentConsideration~
It must be given another strategy when creating the object.
Estimates the distance it would take for every snake to reach the apple. It returns nothing if other snakes would reach the food faster than our snake. Otherwise it uses the given strategy to calculate the direction it has to return.
#### OpponentKilling~
*NOT YET IMPLEMENTED*, but u get the idea :D.
