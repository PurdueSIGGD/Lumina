using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {

    public GameObject[] walls;      // All wall variations that are seen
                                    // Placed 50 away from center
    public GameObject door;         // A single door
                                    // Placed 52 away from center
    public GameObject[] rocks;         // A rock placed to  block up a door entrance
                                    // Placed same place as doors
    public GameObject floor;        // The basic floor for each level
                                    // Placed at 0,0,0
    public GameObject ceiling;      // The particles that you see in the sky
                                    // Placed at 0,50,0
    public GameObject descender;    // The tunnel you enter before going into a cave
                                    // Placed 150 from center
    public GameObject[] enemyTypes;   // Every enemy you can possibly fight

    public GameObject dungeonLevel; // The prefab of the dungeon level, used to initiate any dungeon level

    public int seed;                // Random seed used for generation of past tunnels (set by playerprefs)

    public int depth;               // The number of levels that we are going to go (set by playerprefs)

    public int[] directionOverride; // Leave empty if you want the directions randomly generated

    private readonly float downChance = 0.3f; // Chance of going downwards

    public DungeonLevel[] dungeons;


    public static Vector3[] wallPositions = {
        new Vector3(50, 0, 0),
        new Vector3(0, 0, 50),
        new Vector3(-50, 0, 0),
        new Vector3(0, 0, -50)
    };

    public static Vector3[] wallRotations = {
        new Vector3(0, 180, 0),
        new Vector3(0, 90, 0),
        new Vector3(0, 0, 0),
        new Vector3(0, -90, 0)
    };

    public static Vector3[] doorPositions = {
        new Vector3(52, 0, 0),
        new Vector3(0, 0, 52),
        new Vector3(-52, 0, 0),
        new Vector3(0, 0, -52)
    };

    public static Vector3[] descenderPositions = {
        new Vector3(150, 0, 0),
        new Vector3(0, 0, 150),
        new Vector3(-150, 0, 0),
        new Vector3(0, 0, -150)
    };
    
    public static Vector3[] directions = {
        Vector3.right,
        Vector3.forward,
        Vector3.left,
        Vector3.back
    };
    // Door rotations are same as wall rotations
  

    void Start () {
        dungeons = new DungeonLevel[depth];
        Vector3 dungeonPosition = Vector3.zero;
        DungeonLevel lastDungeon = null;
        for (int currentDepth = 0; currentDepth < depth; currentDepth++) {
            
            // Create a dungeon level at position
            DungeonLevel newDungeon = GameObject.Instantiate(dungeonLevel, dungeonPosition, Quaternion.identity).GetComponent<DungeonLevel>();

            // Keep track of where we've been
            dungeons[currentDepth] = newDungeon;
            newDungeon.pastLevel = lastDungeon;
            if (lastDungeon) lastDungeon.nextLevel = newDungeon;
            newDungeon.generator = this;

            // the floor and the ceiling are already added
            
            // Add 4 random walls
            for (int i = 0; i < 4; i++) {
                GameObject wall = GameObject.Instantiate(walls[Random.Range(0, walls.Length - 1)], dungeonPosition + wallPositions[i], Quaternion.Euler(wallRotations[i]));
                wall.transform.parent = newDungeon.transform;
            }

            // toss in some enemies, tell the dungeon level that those are their required enemies
            //TODO
            newDungeon.enemies = null;

            // If not the last one
            if (currentDepth < depth - 1) {
                // 0 = right, 1 = forward, 2 = left, 3 = backwards

                int directionNum = 0;

                //Choose a next direction that isn't occupied (N, E, S, W), use pastPositions, and is 200 away from the current one
                // make a list of all possible futures, iterate through past positions, remove found items from list, then randomly grab
                ArrayList optionsList = new ArrayList{ 0, 1, 2, 3 };
                // Remove the entrance
                ArrayList toRemove = new ArrayList();
                ArrayList finalOptionsList = new ArrayList();

                // Remove any blocked areas by previous dungeon levels
                foreach (int option in optionsList) {
                    bool failed = false;
                    for (int i = 0; i < currentDepth; i++) {
                    //if (currentDepth == 7) print(dungeons[i].transform.position);
                    // Gather invalid directions
                        if (dungeonPosition + directions[option] * 200 == dungeons[i].transform.position) {
                            //print(currentDepth + " can't go " + option);
                            failed = true;
                            break;
                        }
                    }
                    if (!failed) {
                        finalOptionsList.Add(option);
                    }
                }
              
                

                Vector3 nextPosition;
                // Use the custom override, if provided
                // Otherwise, randomly generate
                if (directionOverride.Length == 0) {
                    directionNum = (int)finalOptionsList[Random.Range(0, finalOptionsList.Count - 1)];
                } else {
                    directionNum = directionOverride[currentDepth];
                }
             
                nextPosition = directions[directionNum] * 200 + dungeonPosition;


                // Add a door at that place
                GameObject doorObject = GameObject.Instantiate(door, dungeonPosition + doorPositions[directionNum], Quaternion.Euler(wallRotations[directionNum]));
                doorObject.transform.parent = newDungeon.transform;


                // Add a rock to all other door places
                ArrayList rockDirectionList = new ArrayList(new int[] { 0, 1, 2, 3 });
                rockDirectionList.Remove(directionNum);
                if (lastDungeon != null) rockDirectionList.Remove((lastDungeon.direction + 2) % 4);
                foreach (int item in rockDirectionList) {
                    GameObject rockSpawn = GameObject.Instantiate(rocks[Random.Range(0, rocks.Length)], dungeonPosition + doorPositions[item], Quaternion.Euler(wallRotations[item]));
                    rockSpawn.transform.parent = newDungeon.transform;
                }

                // if the next position has nowhere to go, force to go down 
                bool forceDownwards = true;
                // For every possible direction next time
                foreach (int option in optionsList) {
                    bool failed = false;
                    // Check to see if something is in the way by looking at past dungeons
                    for (int i = 0; i < currentDepth + 1; i++) {
                        if (nextPosition + directions[option] * 200 == dungeons[i].transform.position) {
                            // Hit a wall
                            failed = true;
                            break;
                        }
                    }
                    if (!failed) {
                        // We didn't hit any of the past walls, there's an opening
                        forceDownwards = false;
                        break;
                    }
                }


                if (forceDownwards || Random.Range(0.0f, 1.0f) < downChance) {
                    // Go downwards
                    nextPosition += Vector3.down * 50;
                    // Add a descender and trap door 150 away from center
                    GameObject descenderObject = GameObject.Instantiate(descender, dungeonPosition + descenderPositions[directionNum], Quaternion.Euler(wallRotations[(directionNum + 2) % 4]));
                    descenderObject.transform.parent = newDungeon.transform;

                }
                newDungeon.direction = directionNum;
                dungeonPosition = nextPosition;
            } else {
                // Rock in all door places
                ArrayList rockDirectionList = new ArrayList(new int[] { 0, 1, 2, 3 });
                // If this isn't the only, and we didn't just go downwards
                if (lastDungeon != null && lastDungeon.transform.position.y <= dungeonPosition.y) rockDirectionList.Remove((lastDungeon.direction + 2) % 4);
                foreach (int item in rockDirectionList) {
                    GameObject rockSpawn = GameObject.Instantiate(rocks[Random.Range(0, rocks.Length)], dungeonPosition + doorPositions[item], Quaternion.Euler(wallRotations[item]));
                    rockSpawn.transform.parent = newDungeon.transform;

                }
            }

            lastDungeon = newDungeon;

        }

    }
	
}
