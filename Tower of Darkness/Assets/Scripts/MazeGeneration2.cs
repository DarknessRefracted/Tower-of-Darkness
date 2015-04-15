using UnityEngine;
using System.Collections;

public class MazeGeneration2 : MonoBehaviour {
	
	enum Cell{OPEN, CLOSED, CROSS};
	int [,] maze;
	
	int columns;
	int rows;
	
	// Use this for initialization
	void Start () {
		
		columns = 15*2;
		rows = 15*2;
		
		maze = new int[rows,columns];
		
		testGeneration ();
	}
	
	void testGeneration(){
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
		int i;
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
			maze [rowForLine, Random.Range (x1, colForLine - 1)] = (int) Cell.OPEN;
		}
		//Right
		if(colForLine != columns - 1){
			maze [rowForLine, Random.Range (colForLine + 1, x2)] = (int) Cell.OPEN;
		}
		//Top
		if(rowForLine != 0){
			maze [Random.Range (y1, rowForLine - 1), colForLine] = (int) Cell.OPEN;
		}
		//Bottom
		if(rowForLine != rows - 1){
			maze [Random.Range (rowForLine + 1, y2), colForLine] = (int) Cell.OPEN;
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
		int i;
		int rowForLine;
		int colForLine = Random.Range (x1, x2);

		//Blocks can only be filled-in in odd rows and columns
		if(colForLine % 2 == 0)
			colForLine += 1;

		//Base case 1: vertical hallway of 0
		if(x1+1 >= x2){
			return;
		}
		
		//Now get the true value for y2 (our "bottom-most" point)
		for(i=y1; i <= y2 && maze[i, colForLine] != (int)Cell.CLOSED; ++i)
			;
		y2 = i - 1;

		//Base case 2: horizontal hallway of 0
		if(y1+1 >= y2){
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
		
		//Remove a random space from three of the walls
		i = Random.Range (0, 4); i = 4;

		//Left and right
		if(i != 0){
			maze [rowForLine, Random.Range (leftMost, colForLine)] = (int) Cell.OPEN;
		}
		if (i != 1)
				if (colForLine + 1 < x2)
						maze [rowForLine, Random.Range (colForLine + 1, rightMost+1)] = (int)Cell.OPEN;
				else
						maze [rowForLine, rightMost] = (int)Cell.OPEN;
		//Above and below
		if(i != 2)
			maze [Random.Range (y1, rowForLine), colForLine] = (int) Cell.OPEN;
		if(i != 3){
			if (rowForLine + 1 < y2)
				maze [Random.Range (rowForLine+1, y2+1), colForLine] = (int) Cell.OPEN;
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