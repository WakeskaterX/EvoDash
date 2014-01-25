﻿using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {
	private int numberJumps, numberDashes;
	private int curJumpHeight, curDashLen, curRunSpeed;
	private float runningTime, airTime, maxJumpHeight;

	PlayerData()
	{
		numberJumps = 0;
		numberDashes = 0;
		curJumpHeight = 0;
		curDashLen = 0;
		curRunSpeed = 0;
		runningTime = 0;
		airTime = 0;
		maxJumpHeight = 0;
	}

	public void PlayerJumped() { numberJumps++; }
	public void PlayerDashed() { numberDashes++; }
	public void PlayerCurrentStats(float jump, float dash, float run)
	{
		curJumpHeight = jump;
		curDashLen = dash;
		curRunSpeed = run;
	}
	public void AddRunningTime(float t)
	{
		if (t > 0)
			runningTime += t;
	}
	public void AddAirTime(float t)
	{
		if (t > 0)
			airTime += t;
	}
	public void ChangeMaxJumpHeight(float height)
	{
		if (height > maxJumpHeight)
			maxJumpHeight = height;
	}

	public PlayerDataCapsule giveData()
	{
		PlayerDataCapsule data;
		data.numberJumps = numberJumps;
		data.numberDashes = numberDashes;
		data.curDashLen = curDashLen;
		data.curJumpHeight = curJumpHeight;
		data.curRunSpeed = curRunSpeed;
		data.runningTime = runningTime;
		data.airTime = airTime;
		data.maxJumpHeight = maxJumpHeight;
		return data;
	}
}

public struct PlayerDataCapsule {
	
	public int numberJumps, numberDashes;
	public int curJumpHeight, curDashLen, curRunSpeed;
	public float runningTime, airTime, maxJumpHeight;

}