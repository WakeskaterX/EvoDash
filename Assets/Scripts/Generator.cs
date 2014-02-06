using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Generator {

	static int numberPaths = 1;
	public static int numberGenerated = 0;
	public static GameObject player;
	public static GameObject spikes;
	public static GameObject ground;
	public static GameObject air;
	public static GameObject ghost;

	public static Vector3 roomLocation 			= new Vector3(0f,0f,0f);

	public static void MakeChunk(PlayerDataCapsule data, GameObject platform, GameObject trigger, GameObject player)
	{

		new BasicGenerator ().GenerateChunk (data, platform, trigger, player);
		numberGenerated++;
	}

	interface ChunkGenerator 
	{
		void GenerateChunk(PlayerDataCapsule data, GameObject platform, GameObject trigger, GameObject player);
	}

 	class BasicGenerator : ChunkGenerator
	{
		public Vector3 startLoc = roomLocation;

		public void GenerateChunk(PlayerDataCapsule data, GameObject platform, GameObject trigger, GameObject player)
		{
			int[,] roomGen = new int[32,32];
			Vector3 platScale = new Vector3(0,0,0);
			int tValFloor = 25;
			int tValSpace = 5;
			
			
			//Create the basic grid room here
			//start at 0 and stay at 0 but go to 31 for J later for entire grid.  Grid starts at 0,0 at startLoc
			for (int j = 0; j < 32; j++){
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
				}}else{
				//create jumping platforms
				
				roomGen[i,j] = 0;
				
				}
			}}
			
			DebugRoomGen(roomGen);

			//Create new spikes
			for (int i = 0; i < 8; i ++){
				GameObject spk = GameObject.Instantiate(spikes, new Vector3(i*2 + startLoc.x,-2 + startLoc.y,0), Quaternion.identity) as GameObject;
				spk.GetComponent<PlatformDestruction>().player = player;
			}
			//Create blocks from grid room
			CreateBlocks(roomGen, platform, player, startLoc);

			//CREATE TEST FLOOR
			//GameObject plat = GameObject.Instantiate(platform,new Vector3(startLoc.x + 8,startLoc.y,0), Quaternion.identity) as GameObject;
			//plat.GetComponent<PlatformDestruction>().player = player;
			//platScale = plat.transform.localScale;
			//plat.transform.localScale = new Vector3(platScale.x * 32, platScale.y,platScale.z);

			//Create new Chunkgenerator
			GameObject trig = GameObject.Instantiate(trigger,new Vector3(roomLocation.x+4f,roomLocation.y,0f),Quaternion.Euler (0,0,90)) as GameObject;
			trig.GetComponent<MakeNextChunk>().player = player;

			//set position to the end of this room
			roomLocation = new Vector3(startLoc.x + 16, startLoc.y, 0);
		
		}
		//take the previous block amounts and determine if there should be a space or not
		bool BuildNoPlat(int i, int j, int[,] roomGen, int tValSpace){
			
			float chance 	= 0f;
			int num 		= 0;
			for (int t = i-1; t >= 0; t--)
			{
				if (roomGen[t,j] == 1){
					num ++;
				} else break;
			} 
			chance = tValSpace * num;
			Debug.Log ("Chance: "+chance);
			
			if (Random.Range(0f,100f) <= chance){
				return true;
			} else return false;
		}
		
		//take the previous spaces and determine if we should build a platform
		bool BuildAPlat(int i, int j, int[,] roomGen, int tValFloor){
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
		}
		
		
		
		void CreateBlocks(int[,] roomGen, GameObject platform, GameObject player, Vector3 startLoc){
			GameObject plat;
			for (int jj = 0; jj < 32; jj++){
			for (int ii = 0; ii < 32; ii++){
				if (roomGen[ii,jj] == 1){
					plat = GameObject.Instantiate(platform,new Vector3(startLoc.x + (.5f * ii), startLoc.y + (.5f * jj), 0),Quaternion.identity) as GameObject;	
					plat.GetComponent<PlatformDestruction>().player = player;
				}
			}}
		}
		
		
		void DebugRoomGen(int[,] roomGen){
			string strDebug = "Debug Int[,]: ";
			for (int yy = 0; yy < 32; yy++){
			    strDebug += "}/r/n Row " + yy + ": { ";
			    
				for (int xx = 0; xx < 32; xx++){
					strDebug += roomGen[xx,yy]+", ";
				}
			}
			
			Debug.Log (strDebug);
		}
	}
	
	
	

}
