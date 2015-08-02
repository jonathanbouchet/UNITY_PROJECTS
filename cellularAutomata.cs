using UnityEngine;
using System.Collections;
//using System.Text;
using System.Collections.Generic;
using System.Globalization;

/* 
	game of life based on Conway's rules :
  1. Any live cell with fewer than two live neighbours dies, as if caused by under-population.
  2. Any live cell with two or three live neighbours lives on to the next generation.
  3. Any live cell with more than three live neighbours dies, as if by overcrowding.
  4. Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
*/

public class cellularAutomata : MonoBehaviour {

    public float interval =0.25f; // time between 2 evolutions
	public int width;             // width of the map
	public int height;            // height of the map
	
	[Range(0,100)]                // define the range of randomFillPercent variable
	public int randomFillPercent; // percentage of empty/filed tiles
	
	int[,] map;                   //0 = dead cell ;1 = alive cell
	
	public string seed;
	private bool useRandomSeed = true;
	
	private float elapsedTime = 0;
	
	void Start()
	{
		GenerateMap ();
	}
	
	void Update()
	{
	//update the status of each cells every second
		if(elapsedTime >= interval)
			{ 
				elapsedTime = 0;  //reset it zero again
				Rules ();
			}
		else
			{
				elapsedTime += Time.deltaTime;
			}
	}
	
	void GenerateMap()
	{
		map = new int[width, height];
		RandomFillMap ();
	}
	
	void RandomFillMap()
	{
		if (useRandomSeed) 
			{
				int test=Random.Range(0,12345);
				//seed = Time.time.ToString();
				seed = test.ToString();
				Debug.Log("in randomSeed , seed = " + seed);
			}
		
		//System.Random prng = new System.Random (seed.GetHashCode());
		
		for (int x=0; x<width; x++) 
			{
				for (int y=0; y<height; y++) 
					{
						if (x == 0 || x == (width - 1) || y == 0 || y == (height - 1)) 
							{
								map [x, y] = 0;
							}  
						else 
							{		
							//map [x, y] = (prng.Next (0, 100) < randomFillPercent) ? 1 : 0;
							map [x, y] = (Random.Range (0, 100) < randomFillPercent) ? 1 : 0;
							}
					}
			}
	}
	
	void SmoothMap(){
		for (int x=0; x<width; x++) {
			for (int y=0; y<height; y++) {
				int neighbourWallTiles = GetSurroundingWallCount(x,y); 
				
				if(neighbourWallTiles>4)
					map[x,y]=1;
				else if(neighbourWallTiles<4)
					map[x,y]=0;
			}
		}
	}

	void Rules(){
		int status; //0 = dead, 1 = live
		int adjacent =0;
			for (int x=0; x<width; x++) {
				for (int y=0; y<height; y++) {
					status = map [x,y];
					int neighbourWallTiles = GetSurroundingWallCount (x,y); 
					if (status == 1 && neighbourWallTiles < 2) map [x, y] = 0;
					if (status == 1 && (neighbourWallTiles == 2 || neighbourWallTiles == 3)) map [x, y] = 1;
					if (status == 1 && neighbourWallTiles > 3) map [x, y] = 0;
					if (status == 0 && neighbourWallTiles == 3){
					if ((x != 0 && x != (width - 1)) && (y != 0 && y != (height - 1))) map[x,y]=1;
						//{
						//if (GetAdjacentCount(x,y)==1) map[x,y] = 1;
						//else map[x, y] = 0;
						//}
					}
				}
		  }
	}

	int GetSurroundingWallCount(int gridX, int gridY){
		int wallCount = 0;
		for (int neightbourX=gridX-1; neightbourX<=gridX+1; neightbourX++) {
			for (int neightbourY=gridY-1; neightbourY<=gridY+1; neightbourY++) {
				if (neightbourX >= 0 && neightbourX < width && neightbourY >= 0 && neightbourY < height) { 
					if (neightbourX != gridX || neightbourY != gridY) {
						wallCount += map [neightbourX, neightbourY];
					}
				}  
				else {
					wallCount++;
				}
			}
		}
		return wallCount;
	}		

	int GetAdjacentCount(int gridX, int gridY){
		int counter = 0;
		string test = "";
		//List<int> test = new List<int>();
		//char[8] test;
		//char tempo;

		// x= gridX -1
		//Debug.Log ("x :"+ gridX);
		//Debug.Log ("y :"+ gridY);
		for (int neightbourY=gridY-1; neightbourY<=gridY+1; neightbourY++) {
			if (map[gridX-1,neightbourY] == 1) test+="L";//.Insert(counter, "L"); 
				else test+="D";
				counter++;
				}

		// x= gridX ; y= gridY+1
		if (map [gridX, gridY+1] == 1) test+="L";
			else test+="D";
		counter++;

		// x = gridX +1
		if (map [gridX + 1, gridY + 1] == 1)test+="L";
			else test+="D";
		counter++;

		if (map [gridX + 1, gridY] == 1)test+="L";
			else test+="D";
		counter++;

		if (map [gridX + 1, gridY - 1] == 1)test+="L";
			else test+="D";
		counter++;

		// x = gridX ; 
		if (map[gridX,gridY-1]==1) test+="L";
		else test+="D";

		int ierr;
		if((test.StartsWith("LLL")==true) ||  test == "LDDDDDLL"){ierr =1;Debug.Log ("string :" + test);}
		//if(test == "LLLDDDDD" || test == "DDLLLDDD" || test == "DDDDLLLD" || test == "LDDDDDLL") {ierr =1;}
		   else ierr =0;
		return ierr;
	}
	
	void OnDrawGizmos()
	{
		if (map != null) 
		{
			for(int x=0;x < width;x++)
			{
				for(int y=0;y<height;y++)
				{
					Gizmos.color = (map[x,y]==1)? Color.black : Color.white;
					Vector3 pos = new Vector3(-width/2 + x + 0.5f, 0, -height/2 + y + 0.5f);
					Gizmos.DrawCube(pos,Vector3.one);
				}
			}
		}
	}
}
