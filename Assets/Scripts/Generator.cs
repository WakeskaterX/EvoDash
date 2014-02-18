using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Generator {

	public static int numberGenerated = 0;
	public static GameObject player;
	public static GameObject spikes;
	public static GameObject ground;
	public static GameObject air;
	public static GameObject smallSpike;

	public static Vector3 roomLocation 			= new Vector3(0f,0f,0f);

	public static void MakeChunk(PlayerDataCapsule data, GameObject platform, GameObject ramp, GameObject trigger, GameObject player)
	{

		new BasicGenerator ().GenerateChunk (data, platform, ramp, trigger, player);
		numberGenerated++;
	}

	interface ChunkGenerator 
	{
		void GenerateChunk(PlayerDataCapsule data, GameObject platform, GameObject ramp, GameObject trigger, GameObject player);
	}

 	class BasicGenerator : ChunkGenerator
	{
		public Vector3 startLoc = roomLocation;

		public void GenerateChunk(PlayerDataCapsule data, GameObject platform, GameObject ramp, GameObject trigger, GameObject player)
		{
			int[,] roomGen 		= new int[32,32];

			int tValFloor 		= Mathf.Max (35 - numberGenerated, 15);
			int tValSpace 		= Mathf.Min (5 + Mathf.FloorToInt (numberGenerated/2 + ((data.curDashLen - .2f) * 5)),25);
			int floorRange		= 3;
			int floorChance 	= Mathf.Min (20 + numberGenerated, 35);
			int prevFloor 		= 0;
			bool createFloor 	= false;
			
			int tValJump 		= Mathf.Max (30 - numberGenerated, 10);
			int tValJumpSpace	= Mathf.Min (5 + Mathf.FloorToInt (numberGenerated/2),45);
			int maxVert			= 3 + Mathf.FloorToInt(data.curJumpHeight - 5);
			int vertChance		= Mathf.Min (Mathf.FloorToInt (10+(numberGenerated/2)),30);
			
			int tValCeil		= Mathf.Max (85 - numberGenerated, 45);
			int tValCeilSpace	= Mathf.Min (2 + Mathf.FloorToInt (numberGenerated/2),20);
			
			int tFlyTopSpawn	= Mathf.Min (3 + Mathf.FloorToInt (numberGenerated/2),33);
			int tSpikeTopSpawn	= Mathf.Min	(5 + numberGenerated,50);
			
			//Create the basic grid room here
			//start at 0 and stay at 0 but go to 31 for J later for entire grid.  Grid starts at 0,0 at startLoc
			for (int j = 0; j < 32; j++){
			
				//CREATE setup for horizontal floor heights
				if (!createFloor){
					floorRange = Random.Range (3, 3 + Mathf.CeilToInt ((data.curJumpHeight - 4)/1.5f));
					//Debug.Log (floorRange);
					if (j - prevFloor >= floorRange){
						if (Random.Range (1,101) < floorChance){
							createFloor = true;
						}
					}
				}
			for (int i = 0; i < 32; i++){
				//create base row
				if (j == 0){
				if (i < 2){
					roomGen[i,j] = 1;
				}
				else{
					if (roomGen[i-1,j] == 1)
					{
						if (BuildNoPlat(i,j,roomGen, tValSpace)){
							roomGen[i,j] = 0;
						} else {
							roomGen[i,j] = 1;
						}
					} else {
						if (BuildAPlat(i,j,roomGen,tValFloor)){
							roomGen[i,j] = 1;
						} else {
							roomGen[i,j] = 0;
						}
					}
					}}else	if (j < 31){   //create jumping platforms
					if (createFloor){
						if (i > 0)
						{
							if (roomGen[i-1,j] == 1)
							{
								if (BuildNoPlat(i,j,roomGen, tValJumpSpace)){
									roomGen[i,j] = 0;
								} else {
									roomGen[i,j] = 1;
								}
							} else {
								if (BuildAPlat(i,j,roomGen,tValJump)){
									roomGen[i,j] = 1;
								} else {
									roomGen[i,j] = 0;
								}
							}	
						}
						} else roomGen[i,j] = 0;
				//create horizontal jumping layers.  Jump height influences height.  Jumping areas range between 2-4 blocks between the previous set
				}
				else{
				//Create Ceiling
						if (i < 2){
							roomGen[i,j] = 1;
						}
						else{
							if (roomGen[i-1,j] == 1)
							{
								if (BuildNoPlat(i,j,roomGen, tValCeilSpace)){
									roomGen[i,j] = 0;
								} else {
									roomGen[i,j] = 1;
								}
							} else {
								if (BuildAPlat(i,j,roomGen,tValCeil)){
									roomGen[i,j] = 1;
								} else {
									roomGen[i,j] = 0;
								}
							}
						}
						
						CreateTopFlyer(roomGen,i,j,tFlyTopSpawn, startLoc);
						CreateTopSpikes(roomGen, i,j,tSpikeTopSpawn,startLoc);
				}
			}
			
			if (createFloor){
				createFloor = false;
				prevFloor = j;
			}
			}
			
			//NEXT go through and create vertical sections
			for (int j = 1; j < 32; j++){
			for (int i = 0; i < 32; i++){
				if (roomGen[i,j-1] == 1){   									//if the block below the current slot is filled
					if (Random.Range(0,100) < vertChance){						//we have a chance of extending a line up from each platform
						BuildPillar(roomGen,i,j, Random.Range (1,maxVert+1));
						i = GoToNextPlatform(roomGen,i,j);
					}
				}
			
			}
			}

			//Create new spikes
			for (int i = 0; i < 8; i ++){
				GameObject spk = GameObject.Instantiate(spikes, new Vector3(i*2 + startLoc.x,-2 + startLoc.y,0), Quaternion.identity) as GameObject;
				spk.GetComponent<PlatformDestruction>().player = player;
			}
			
			//Create blocks from grid room
			CreateBlocks(roomGen, platform, ramp, player, startLoc);

			//Create new Chunkgenerator
			GameObject trig = GameObject.Instantiate(trigger,new Vector3(roomLocation.x+4f,roomLocation.y,0f),Quaternion.Euler (0,0,90)) as GameObject;
			trig.GetComponent<MakeNextChunk>().player = player;

			//set position to the end of this room
			roomLocation = new Vector3(startLoc.x + 16, startLoc.y, 0);
		
		} // END OF CHUNK GENERATOR
	} //End of Basic Generator
	
	/************************************************************************/
	/* 		Generator.CS Private Static Methods for Gen Array Manipulation 	*/
	/************************************************************************/
	
	//take the previous block amounts and determine if there should be a space or not
	private static bool BuildNoPlat(int i, int j, int[,] roomGen, int tValSpace){
		
		float chance 	= 0f;
		int num 		= 0;
		for (int t = i-1; t >= 0; t--)
		{
			if (roomGen[t,j] == 1){
				num ++;
			} else break;
		} 
		chance = tValSpace * num;
		//Debug.Log ("Chance: "+chance);
		
		if (Random.Range(0f,100f) <= chance){
			return true;
		} else return false;
	}//end of BuildNoPlat()
	
	
	//take the previous spaces and determine if we should build a platform
	private static bool BuildAPlat(int i, int j, int[,] roomGen, int tValFloor){
		float chance 	= 0f;
		int num 		= 0;
		for (int t = i-1; t >= 0; t--)
		{
			if (roomGen[t,j] == 0){
				num ++;
			} else break;
		} 
		chance = tValFloor * num;
		
		if (Random.Range(0f,100f) <= chance){
			return true;
		} else return false;
	}//end of BuildAPlat()
	
	
	//build a pillar at i,j up until it hits a platform
	private static void BuildPillar(int[,] roomGen, int i, int j, int maxVert){
		//Debug.Log ("Building a Pillar at: " + i + ", " + j);
		if (Random.Range (0,10) <= 8){
			for (int p = 0; p < maxVert; p ++){
				if (j+p < 32){
					if (roomGen[i,j+p] != 1){
						roomGen[i,j+p] = 1;
					} else break;
				} else break;
			}
		} else{
			roomGen[i,j] = 2;
		}
	}//end of BuildPillar()
	
	
	//Go to the next platform on this row
	private static int GoToNextPlatform(int[,] roomGen, int i, int j){
		for (int h = 0; h < 32-i; h ++){
			if (roomGen[i+h,j-1] == 0){
				return i+h;
				//Debug.Log("Returned Correctly");
			}
		} 
		//Debug.Log ("Returned Incorrectly");
		return i+1;
	}//end of GoToNextPlatform()
	
	//Create Blocks from RoomGenArray
	private static void CreateBlocks(int[,] roomGen, GameObject platform, GameObject ramp, GameObject player, Vector3 startLoc){
		GameObject plat;
		for (int jj = 0; jj < 32; jj++){
			for (int ii = 0; ii < 32; ii++){
				if (roomGen[ii,jj] == 1){
					plat = GameObject.Instantiate(platform,new Vector3(startLoc.x + (.5f * ii), startLoc.y + (.5f * jj), 0),Quaternion.identity) as GameObject;	
					plat.GetComponent<PlatformDestruction>().player = player;
				}
				
				if (roomGen[ii,jj] == 2){
					plat = GameObject.Instantiate(ramp,new Vector3(startLoc.x + (.5f * ii), startLoc.y + (.5f * jj), 0),Quaternion.identity) as GameObject;	
					plat.GetComponent<PlatformDestruction>().player = player;
				}
			}}
	}//end of CreateBlocks()
	
	private static void CreateTopFlyer(int[,] roomGen, int i, int j, int tFlyTop, Vector3 startLoc){
		int chance = Random.Range (0,100);
		if (chance <= tFlyTop){
			float randFloat 		= Random.Range (1.5f,5f);
			Vector3 spawn			= new Vector3(startLoc.x + (.5f * i) ,startLoc.y + (.5f * j) + randFloat,startLoc.z);
			GameObject.Instantiate (air,spawn,Quaternion.identity);
		}
	}//end of CreateTopFlyer()
	
	private static void CreateTopSpikes(int[,] roomGen, int i, int j, int tSpikeTop, Vector3 startLoc){
		int chance = Random.Range (0,100);
		if (roomGen[i,j] == 1 && chance <= tSpikeTop){
			Vector3 spawn 			= new Vector3(startLoc.x + (.5f * i) ,startLoc.y + (.5f * j) + .5f, startLoc.z);
			GameObject.Instantiate(smallSpike,spawn,Quaternion.identity);			
		}
	}
	
	//Debug the RoomGen Array to a string
	private static void DebugRoomGen(int[,] roomGen){
		string strDebug = "Debug Int[,]: ";
		for (int yy = 0; yy < 32; yy++){
			strDebug += "}/r/n Row " + yy + ": { ";
			
			for (int xx = 0; xx < 32; xx++){
				strDebug += roomGen[xx,yy]+", ";
			}
		}
		
		Debug.Log (strDebug);
	}//End of DebugRoomGen();	
	
}
