using UnityEngine;
using System.Collections;

public class MazeGeneration2 : MonoBehaviour {
	
	enum Cell{OPEN, CLOSED, CROSS};
	int [,] maze;
	
	public int columns;
	public int rows;

	//Maze object prefabs
	public GameObject mazeBackground;
	public GameObject mazeWallBlock;

	//Maze objects produced in the game. Use these so that we can quickly delete them after the maze is solved
	public GameObject objBackground;
	public GameObject[,] objSpaces;
	public GameObject[,] objWalls;

	//LevelGenerator object in order to access height-related values/functions for placing the maze onto the screen
	public LevelGenerator scrLevGen;

	
	// Use this for initialization
	void Start () {
		scrLevGen = (LevelGenerator) GetComponent ("LevelGenerator");
	}

	public void startMazeGeneration(){
		produceMaze ();
	}

	void produceMaze(){
		//Rows and columns must be even for the algorithm to work.
			//If they are not even, then it is best to double the amounts, because it will allow for the same number of filled rows and
				// columns, but with the necessary amount of unfilled rows and columns
		if(rows % 2 == 1)
			rows *= 2;
		if(columns % 2 == 1)
			columns *= 2;

		maze = new int[rows,columns];
		objSpaces = new GameObject[rows,columns]
		objWalls = new GameObject[2, rows];

		//Initialize the maze
		for (int i=0; i<rows; ++i) {
			for(int j=0; j<columns; ++j){
				maze[i, j] = (int)Cell.OPEN;
			}
		}

		//Generate the maze
		divideArea (0, columns-1, 0);

		//TODO - Freeze the game

		//Display the maze
		objBackground = (GameObject) Instantiate (mazeBackground, 
		                                       new Vector3(scrLevGen.start.transform.position.x, 
		            							scrLevGen.player.transform.position.y - (float) 0.5, 
		            							scrLevGen.start.transform.position.z),
		                                       Quaternion.identity);

		for(int i=0; i<rows; ++i){
			objWalls[0, i] =  Instantiate(mazeWallBlock, 
			            		new Vector3(objBackground.transform.position.x  - (float) 3.75, 
			            		objBackground.transform.position.y + (float)0.25 * i - (float) 3.5, -1), 
			            		Quaternion.identity);
			objWalls[1, i] =  Instantiate(mazeWallBlock, 
			            		new Vector3(objBackground.transform.position.x  - (float) 3.5 + (float) 0.25 * columns, 
			            		objBackground.transform.position.y + (float)0.25 * i - (float) 3.5, -1), 
			            		Quaternion.identity);

			for(int j=0; j<columns; ++j){
				if(maze[i,j] == (int) Cell.OPEN){
					objSpaces[i,j] = null;
				   /*if(i == 0 && j == 0){
						Instantiate(mazeWallBlock, 
						            new Vector3(mazeObject.transform.position.x + (float)0.30 - (float) 3.5, 
						            mazeObject.transform.position.y + (float)0.30 - (float) 3.5, -1), 
						            Quaternion.identity);}
					else if (i==rows-1 && j == columns - 1){
						Instantiate(mazeWallBlock, 
						            new Vector3(mazeObject.transform.position.x + (float)0.25 * i - (float) 3.5, 
						            mazeObject.transform.position.y + (float)0.25 * j - (float) 3.5, -1), 
						            Quaternion.identity);}*/
				}
				else{
					objSpaces[i,j] = Instantiate(mazeWallBlock, 
					            	new Vector3(objBackground.transform.position.x + (float)0.25 * j - (float) 3.5, 
					            		objBackground.transform.position.y + (float)0.25 * i - (float) 3.5, -1), 
					            		Quaternion.identity);
				}
			}
		}

		printGraph ();
	}

	void deleteMaze(){
		for(int i=0; i<rows; ++i){
			//Delete walls
			//objWalls[0, i].

			for(int j=0; j<columns; ++j){
				//Delete whatever block was at this space
				if(objSpaces[i,j] != null){
				}
			}
		}
	}

	//Function will perform maze generation many times (I've been using 111,000 as the number of tests) to make sure that none of the
		// random maze designs will crash the program.
	void testGeneration(){
		maze = new int[rows,columns];

		for (int s=0; s<111000; ++s){
			for (int i=0; i<rows; ++i) {
				for(int j=0; j<columns; ++j){
					maze[i, j] = (int)Cell.OPEN;
				}
			}
			divideArea (0, columns-1, 0);
		}
		
		printGraph ();
	}
	
	
	//Function will divide up the area into four zones, starting the borders from x1 and y1, respectively, and go until there is a
	// border in the way (or until we have reached the bounds of x2 or y2)
	//The placement of the lines will be randomized based on the size of the area (x1->x2, y1->y2)
	//x2 and y2 are equal to the last cell of that column or row, respectively. That is, if we are on the first call of the function, and
	// the width of the maze is six, then x2 is equal to five, not six.
	void divideArea(int x1, int x2, int y1){
		//y2 is determined as being the bottom of the vertical line. Once that line is drawn, y2 will be initialized
		int y2;//AKA "bottom-most"
		int leftMost, rightMost;
		int i, holeLocation;
		int rowForLine;
		//Prevent the program from picking the very leftmost or rightmost column
		int colForLine = Random.Range (x1+2, x2-2);
		
		//Blocks can only be filled-in in odd rows and columns (remember, first = 0, second = 1; cell [0,0] should be empty)
		if(colForLine % 2 == 0)
			colForLine += 1;

		//Make the vertical line.
		for(i = y1; i < rows && maze[i, colForLine] != (int)Cell.CLOSED; ++i){
			maze[i, colForLine] = (int)Cell.CLOSED;
		}

		//Now get the value for y2 (our "bottom-most" point)
		y2 = i-1;

		//Get the row number
		rowForLine = Random.Range (y1, y2);

		//Blocks can only be filled-in in odd rows and columns
		if(rowForLine % 2 == 0)
			rowForLine += 1;

		//Make the left-side horizontal line
		for(i=colForLine-1; i >= 0 && maze[rowForLine, i] != (int)Cell.CLOSED; -- i){
			maze[rowForLine, i] = (int)Cell.CLOSED;
		}
		leftMost = i+1;

		//Make the right-side horizontal line
		for(i=colForLine+1; i <= x2 && maze[rowForLine, i] != (int)Cell.CLOSED; ++i){
			maze[rowForLine, i] = (int)Cell.CLOSED;
		}
		rightMost = i - 1;
		
		
		//Remove a space from all four walls
		maze [rowForLine, colForLine] = (int) Cell.CROSS;

		//Left
		if(colForLine != 0){
			holeLocation = Random.Range (x1, colForLine - 1);
			
			if(holeLocation % 2 == 1){
				if(holeLocation > x1)
					holeLocation--;
				else
					holeLocation++;
			}

			maze [rowForLine, holeLocation] = (int) Cell.OPEN;
		}
		//Right
		if(colForLine != columns - 1){
			holeLocation = Random.Range (colForLine + 1, x2);
			
			if(holeLocation % 2 == 1){
				if(holeLocation > colForLine + 1)
					holeLocation--;
				else
					holeLocation++;
			}

			maze [rowForLine, holeLocation] = (int) Cell.OPEN;
		}
		//Top
		if(rowForLine != 0){
			
			holeLocation = Random.Range (y1, rowForLine - 1);
			
			if(holeLocation % 2 == 1){
				if(holeLocation > y1)
					holeLocation--;
				else
					holeLocation++;
			}
			
			maze [holeLocation, colForLine] = (int) Cell.OPEN;
		}
		//Bottom
		if(rowForLine != rows - 1){
			holeLocation = Random.Range (rowForLine + 1, y2);

			if(holeLocation % 2 == 1){
				if(holeLocation > rowForLine + 1)
					holeLocation--;
				else
					holeLocation++;
			}

			maze [holeLocation, colForLine] = (int) Cell.OPEN;
		}

		//Top-right and -left
		rec_divideArea (colForLine+1, rightMost, y1, rowForLine-1);
		rec_divideArea (leftMost, colForLine-1, y1, rowForLine-1);
		//Bottom-right and -left
		rec_divideArea (colForLine, rightMost, rowForLine+1, y2);
		rec_divideArea (leftMost, colForLine, rowForLine+1, y2);
	}

	void rec_divideArea(int x1, int x2, int y1, int y2){
		//y2 is determined as being the bottom of the vertical line. Once that line is drawn, y2 will be initialized
		int leftMost, rightMost;
		int i, holeLocation;
		int rowForLine;
		int colForLine = Random.Range (x1, x2-2);

		//Blocks can only be filled-in in odd rows and columns
		if(colForLine % 2 == 0)
			colForLine += 1;

		//Base case 1: vertical hallway of 0
		if(x1+2 >= x2){
			return;
		}
		
		//Now get the true value for y2 (our "bottom-most" point)
		for(i=y1; i <= y2 && maze[i, colForLine] != (int)Cell.CLOSED; ++i)
			;
		y2 = i - 1;

		//Base case 2: horizontal hallway of 0
		if(y1+2 >= y2){
			return;
		}


		//Make the vertical line.
		for(i = y1; i <= y2; ++i){
			maze[i, colForLine] = (int)Cell.CLOSED;
		}
		
		//Get the row number
		rowForLine = Random.Range (y1, y2);

		//Blocks can only be filled-in in odd rows and columns
		if(rowForLine % 2 == 0)
			rowForLine += 1;

		maze [rowForLine, colForLine] = (int) Cell.CROSS;

		//Make the left-side horizontal line
		for(i=colForLine-1; i >= x1 && maze[rowForLine, i] != (int)Cell.CLOSED; -- i){
			maze[rowForLine, i] = (int)Cell.CLOSED;
		}
		leftMost = i + 1;
		
		//Make the right-side horizontal line
		for(i=colForLine+1; i <= x2 && maze[rowForLine, i] != (int)Cell.CLOSED; ++i){
			maze[rowForLine, i] = (int)Cell.CLOSED;
		}
		rightMost = i-1;
		
		//Remove a random space from three of the walls, but only in even spaces
		i = Random.Range (0, 4);i = 4;

		//Left and right
		if(i != 0){
			holeLocation = Random.Range (leftMost, colForLine);

			if(holeLocation % 2 == 1){
				if(holeLocation > leftMost)
					holeLocation--;
				else
					holeLocation++;
			}

			maze [rowForLine, holeLocation] = (int) Cell.OPEN;
		}
		if (i != 1){
			if (colForLine + 1 < x2){
				holeLocation = Random.Range (colForLine + 1, rightMost+1);
				
				if(holeLocation % 2 == 1){
					if(holeLocation > colForLine + 1)
						holeLocation--;
					else
						holeLocation++;
				}

				maze [rowForLine, holeLocation] = (int)Cell.OPEN;
			}
			else
				maze [rowForLine, rightMost] = (int)Cell.OPEN;
		}

		//Above and below
		if(i != 2){
			holeLocation = Random.Range (y1, rowForLine);
			
			if(holeLocation % 2 == 1){
				if(holeLocation > y1)
					holeLocation--;
				else
					holeLocation++;
			}

			maze [holeLocation, colForLine] = (int) Cell.OPEN;
		}
		if(i != 3){
			if (rowForLine + 1 < y2){
				holeLocation = Random.Range (rowForLine+1, y2+1);
				
				if(holeLocation % 2 == 1){
					if(holeLocation > rowForLine+1)
						holeLocation--;
					else
						holeLocation++;
				}

				maze [holeLocation, colForLine] = (int) Cell.OPEN;
			}
		}

		//Top-right and -left
		rec_divideArea (leftMost, colForLine-1, y1, rowForLine-1);
		rec_divideArea (colForLine+1, rightMost, y1, rowForLine-1);
		//Bottom-right and -left
		rec_divideArea (leftMost, colForLine-1, rowForLine+1, y2);
		rec_divideArea (colForLine+1, rightMost, rowForLine+1, y2);
	}
	
	void printGraph(){
		string testMessage = "";
		
		for (int i=0; i<rows; ++i) {
			for(int j=0; j<columns; ++j){
				//Print with pictures! :D
				switch(maze[i,j]){
				case 0:
					testMessage += " ";
					break;
				case 1:
					testMessage += "#";
					break;
				default:
					testMessage += "X";
					break;
				}
			}
			testMessage += "6\n";
		}
		System.IO.File.WriteAllText("C:\\Users\\Kevin Strileckis\\Documents\\Tower-of-Darkness\\Tower of Darkness\\Assets\\maze.txt", testMessage);
		Debug.Log (testMessage);
	}

}