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
    public GameObject chandelier;   // Something to light up hallways
                                    // Placed 50 away from center
    public GameObject floor;        // The basic floor for each level
                                    // Placed at 0,0,0
    public GameObject ceiling;      // The particles that you see in the sky
                                    // Placed at 0,50,0
    public GameObject descender;    // The tunnel you enter before going into a cave
                                    // Placed 150 from center
    public GameObject exit;         // The exit door used to leave the dungeon

    [System.Serializable]
    public class DifficultyArrays {
        public ProbabililtyItem[] difficulty1;
        public ProbabililtyItem[] difficulty2;
        public ProbabililtyItem[] difficulty3;

        public ProbabililtyItem[] get(int i) {
            if (i == 1) {
                return difficulty1;
            } else if (i == 2) {
                return difficulty2;
            } else if (i == 3) {
                return difficulty3;
            }
            return null;
        }
    }
    public DifficultyArrays difficultyArrays;

    public int difficulty;                      // Which array of the above should you use

    public GameObject dungeonLevel; // The prefab of the dungeon level, used to initiate any dungeon level

    public ProbabililtyItem[] junkToSpawn;// A set of items that we spawn randomly on each floor

    public int seed;                // Random seed used for generation of past tunnels (set by playerprefs)

    public int depth;               // The number of levels that we are going to go (set by playerprefs)

    public int[] directionOverride; // Leave empty if you want the directions randomly generated

    private readonly float downChance = 0.3f; // Chance of going downwards

    public DungeonLevel[] dungeons;

    private readonly float distanceBetween = 100f;
    private readonly float spawnRadius = 16f;

    private readonly Vector2 junkCountRangeFirst = new Vector2(25, 40);
    private readonly Vector2 junkCountRange = new Vector2(10, 20);

    private bool cleared = false; // If the dungeon is cleared, only occasionally spawn enemies. Not too often.

    private readonly float enemySpawnRate_Cleared = 0.2f; // What % of the time should enemies spawn when the dungeon is cleared

    private ArrayList junkLottery;
    private ArrayList enemyLottery;


    public static Vector3[] wallPositions = {
        new Vector3(25, 0, 0),
        new Vector3(0, 0, 25),
        new Vector3(-25, 0, 0),
        new Vector3(0, 0, -25)
    };

    public static Vector3[] wallRotations = {
        new Vector3(0, 180, 0),
        new Vector3(0, 90, 0),
        new Vector3(0, 0, 0),
        new Vector3(0, -90, 0)
    };

    public static Vector3[] doorPositions = {
        new Vector3(26, 0, 0),
        new Vector3(0, 0, 26),
        new Vector3(-26, 0, 0),
        new Vector3(0, 0, -26)
    };

    public static Vector3[] descenderPositions = {
        new Vector3(75, 0, 0),
        new Vector3(0, 0, 75),
        new Vector3(-75, 0, 0),
        new Vector3(0, 0, -75)
    };
    
    public static Vector3[] directions = {
        Vector3.right,
        Vector3.forward,
        Vector3.left,
        Vector3.back
    };
    // Door rotations are same as wall rotations

    /**
     * Returns a value based off of the old random state, and then resets the current state
     This won't work since oldState is just an instance, and is not reset
    private static float trueRandomRange(Random.State oldState, float start, float end) {
        Random.State currentState = Random.state;
        Random.state = oldState;
        float result = Random.Range(start, end);
        oldState = Random.state;
        Random.state = currentState;
        return result;
    }
    private static int trueRandomRange(Random.State oldState, int start, int end) {
        Random.State currentState = Random.state;
        Random.state = oldState;
        int result = Random.Range(start, end);
        oldState = Random.state;
        Random.state = currentState;
        return result;
    }
         */

    void Start () {
        Random.Range(0.0f, 1.0f); // Use random so we don't get the same each time
        Random.State oldState = Random.state;
        if (seed == 0) {
            seed = PlayerPrefs.GetInt("DungeonSeed");
            Random.InitState(seed);
        }
        cleared = 1 == PlayerPrefs.GetInt(seed.ToString());
        if (cleared) {
            //Debug.Log("Dungeon is cleared, not doing much for the things");
        }
        difficulty = PlayerPrefs.GetInt("DungeonDifficulty");
        depth = PlayerPrefs.GetInt("DungeonDepth");
        // Since we want an extra room in the first level to include random items, we increment depth
        depth++;

        // Create a lottery for possible junk to be spawned 
        // Each integer added is a reference to the junkToSpawn array
        junkLottery = new ArrayList();
        for (int i = 0; i < junkToSpawn.Length; i++) {
            for (int k = 0; k < junkToSpawn[i].chance; k++) {
                junkLottery.Add(i);
            }
        }

        enemyLottery = new ArrayList();
        for (int i = 0; i < difficultyArrays.get(difficulty).Length; i++) {
            for (int k = 0; k < difficultyArrays.get(difficulty)[i].chance; k++) {
                enemyLottery.Add(i);
            }
        }

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

            // Toss in some random junk each stage, more if the first one
            for (int junkCount = 0; junkCount < (currentDepth == 0 ? Random.Range(junkCountRangeFirst.x, junkCountRangeFirst.y) : Random.Range(junkCountRange.x, junkCountRange.y)); junkCount++) {
                Vector2 v2Position = Random.insideUnitCircle * spawnRadius * 1f;
                Vector3 v3Position = new Vector3(v2Position.x, 1, v2Position.y) + dungeonPosition; //Put them slightly above so they don't fall through
                int randomIndex = Random.Range(0, junkLottery.Count - 1);
                bool keepDesiredRotation = Random.Range(0.0f, 1.0f) < 0.1f;
                //print(keepDesiredRotation);
                GameObject newJunk = GameObject.Instantiate(junkToSpawn[(int)junkLottery[randomIndex]].prefab, v3Position, keepDesiredRotation ? Quaternion.identity : Quaternion.Euler(360 * Random.insideUnitSphere));
                newJunk.transform.parent = newDungeon.transform;
                ItemStats it;
                if (it = newJunk.GetComponent<ItemStats>()) {
                    // Somewhere a bit below max condition to min condition
                    Random.State currentState = Random.state;
                    Random.state = oldState;
                    it.condition = Random.Range( it.minCondition, it.maxCondition - (0.5f * (it.maxCondition - it.minCondition)));
                    oldState = Random.state;
                    Random.state = currentState;
                }
            }

            // toss in some enemies, tell the dungeon level that those are their required enemies
            ArrayList enemyList = new ArrayList();
            // Add none if the first room
            int enemyCount = currentDepth == 0 ? 0 : Random.Range(1, 5);
            int enemyType = Random.Range(0, enemyLottery.Count);
            // We preserve enemy type for each room when coming back from being cleared. 
            for (int enemies = 0; enemies < enemyCount; enemies++) {
                Random.State currentState = Random.state;
                Random.state = oldState;
                float result = Random.Range(0.0f, 1.0f);
                print("no enemy result: " + result);
                if (cleared && enemySpawnRate_Cleared < result) {
                    // If we are cleared, use the change and continue if we want some enemies not to be spawned
                    oldState = Random.state;
                    Random.state = currentState;
                    continue;
                }
                Vector2 v2Position = Random.insideUnitCircle * spawnRadius;
                Vector3 v3Position = new Vector3(v2Position.x, 1, v2Position.y) + dungeonPosition; //Put them slightly above so they don't fall through
              
                GameObject newEnemy = GameObject.Instantiate(difficultyArrays.get(difficulty)[(int)enemyLottery[enemyType]].prefab, v3Position, Quaternion.identity);

                newEnemy.transform.parent = newDungeon.transform;
                enemyList.Add(newEnemy);

                oldState = Random.state;
                Random.state = currentState;
            }
            newDungeon.enemies = enemyList.ToArray(typeof(GameObject)) as GameObject[];

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
                        if (dungeonPosition + directions[option] * distanceBetween == dungeons[i].transform.position) {
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
             
                nextPosition = directions[directionNum] * distanceBetween + dungeonPosition;


                // Add a door at that place
                GameObject doorObject = GameObject.Instantiate(door, dungeonPosition + doorPositions[directionNum], Quaternion.Euler(wallRotations[directionNum]));
                doorObject.transform.parent = newDungeon.transform;
                newDungeon.door = doorObject.GetComponent<Animator>();

             


                // Add a rock to all other door places
                ArrayList rockDirectionList = new ArrayList(new int[] { 0, 1, 2, 3 });
                rockDirectionList.Remove(directionNum);
                // Don't add a rock if we just went down
                if (lastDungeon != null && lastDungeon.transform.position.y == dungeonPosition.y) rockDirectionList.Remove((lastDungeon.direction + 2) % 4);
              
                foreach (int item in rockDirectionList) {
                    GameObject rockSpawn = GameObject.Instantiate(rocks[Random.Range(0, rocks.Length)], dungeonPosition + doorPositions[item], Quaternion.Euler(wallRotations[item]));
                    rockSpawn.transform.parent = newDungeon.transform;
                }

                // Add a chandelier for in-betweens, they are twice the length as walls and 4 units high
                GameObject chandelierSpawn = GameObject.Instantiate(chandelier, dungeonPosition + wallPositions[directionNum] * 2 + (Vector3.up * 4f), Quaternion.identity);
                chandelierSpawn.transform.parent = newDungeon.transform;

                // if the next position has nowhere to go, force to go down 
                bool forceDownwards = true;
                // For every possible direction next time
                foreach (int option in optionsList) {
                    bool failed = false;
                    // Check to see if something is in the way by looking at past dungeons
                    for (int i = 0; i < currentDepth + 1; i++) {
                        if (nextPosition + directions[option] * distanceBetween == dungeons[i].transform.position) {
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
                    nextPosition += Vector3.down * 25;
                    // Add a descender and trap door 75 away from center
                    GameObject descenderObject = GameObject.Instantiate(descender, dungeonPosition + descenderPositions[directionNum], Quaternion.Euler(wallRotations[(directionNum + 2) % 4]));
                    descenderObject.transform.parent = newDungeon.transform;

                }
                newDungeon.direction = directionNum;
                dungeonPosition = nextPosition;
            } else {
                // Rock in all door places
                ArrayList rockDirectionList = new ArrayList(new int[] { 0, 1, 2, 3 });
                // If this isn't the only, and we didn't just go downwards
                if (lastDungeon != null && lastDungeon.transform.position.y == dungeonPosition.y) rockDirectionList.Remove((lastDungeon.direction + 2) % 4);
                foreach (int item in rockDirectionList) {
                    GameObject rockSpawn = GameObject.Instantiate(rocks[Random.Range(0, rocks.Length)], dungeonPosition + doorPositions[item], Quaternion.Euler(wallRotations[item]));
                    rockSpawn.transform.parent = newDungeon.transform;

                }
            }
            if (currentDepth != 0) {
                newDungeon.gameObject.SetActive(false);
            }

            lastDungeon = newDungeon;

        }

        Random.state = oldState;

    }


	
}
