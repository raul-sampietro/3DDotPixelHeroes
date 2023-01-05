using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;

public enum RoomInstanceState
{
    UNINSTANTIATED,
    INSTANTIATED,
    HIDDEN
}

public enum RoomProgressState
{
    NOT_STARTED,
    CLOSED,
    BATTLE,
    DONE
}

public struct EnemyStruct
{
    public EnemyStruct(GameObject obj, Vector2 initPos, bool dropLife, bool dropCoin)
    {
        this.Object = obj;
        this.InitPosition = initPos;
        this.DropLife = dropLife;
        this.DropCoin = dropCoin;
    }
    public GameObject Object { get; }
    public Vector2 InitPosition { get; }
    public bool DropLife { get; }
    public bool DropCoin { get; }
    public override string ToString() => $"({Object}, {InitPosition}, {DropLife}, {DropCoin})";
}

public struct DoorStruct
{
    public DoorStruct(GameObject obj, Vector2 initPos)
    {
        this.Object = obj;
        this.InitPosition = initPos;
    }
    public GameObject Object { get; }
    public Vector2 InitPosition { get; }
    public override string ToString() => $"({Object}, {InitPosition})";
}

public struct Room
{
    public Room(int x, int y, GameObject obj, List<EnemyStruct> enemies, List<DoorStruct> doors, RoomInstanceState instanceState, RoomProgressState progressState)
    {
        this.x = x;
        this.y = y;
        this.Enemies = enemies;
        this.Doors = doors;
        this.InstanceState = instanceState;
        this.ProgressState = progressState;
        this.Obj = obj;
    }
    public int x { get; }
    public int y { get; }
    public GameObject Obj { get; set; }
    public List<EnemyStruct> Enemies { get; }
    public List<DoorStruct> Doors { get; }
    public RoomInstanceState InstanceState { get; set; }
    public RoomProgressState ProgressState { get; set; }
    public void AddEnemy(EnemyStruct enemy) { Enemies.Add(enemy); }
    public bool EnemiesAlive()
    {
        foreach (EnemyStruct enemy in Enemies) {
            if (enemy.Object != null) return true;
        }
        return false;
    }
    public void AddDoor(DoorStruct door) { Doors.Add(door); }
    public void SetObj(GameObject obj) { this.Obj = obj; }
    public void SetInstanceState(RoomInstanceState instanceState) { this.InstanceState = instanceState; }
    public void SetProgressState(RoomProgressState progressState) { this.ProgressState = progressState; }
    public override string ToString() => $"({x}, {y}, {Obj}, {Enemies}, {InstanceState}, {ProgressState})";
}


public class LevelGenerator : Singleton<LevelGenerator>
{
    public Array2DInt levelsList;
    public Vector2 bossRoomBottomLeft;
    public Vector2 bossRoomTopRight;
    public Vector2 startingRoom;

    public Texture2D[] levelsMappings;

    public ColorToPrefab[] colorMappings;
    public GameObject floor;

    private Vector2 sizeOfImage = new(16, 12);

    //Room status
    //private RoomInstanceState[,] roomInstanceStates = new RoomInstanceState[10, 10];
    //private RoomProgressState[,] roomProgrssStates = new RoomProgressState[10, 10];
    private Dictionary<int, Dictionary<int, Room>> rooms = new Dictionary<int, Dictionary<int, Room>>();

    private Vector2 currentRoom;

    void Awake()
    {
        //Debug.Log("Awake");
        //Debug.Log("" + bossRoomBottomLeft + bossRoomTopRight);
        GameObject.Find("OverviewCamera")
            .GetComponent<CameraMover>()
            .SetBossRooms(bossRoomBottomLeft, bossRoomTopRight);
    }

    void Start()
    {
        //Debug.Log("Start");
        //SetCurrentRoom((int)startingRoom.x, (int)startingRoom.y);
        for (int i = 0; i < 10; ++i)
            for (int j = 0; j < 10; ++j)
                EnableRoom(i, j);
        

        //InstanciateRoom(3, 3);

        // TODO manage the rooms that have to be "deinstanciated"
    }

    void Update()
    {
        //PlayCurrentRoom();
    }

    private void PlayCurrentRoom()
    {
        int i = (int)currentRoom.x;
        int j = (int)currentRoom.y;
        //Debug.Log("Play (" + i + "," + j + ")");
        // Close doors
        // Instanciate enemies
        switch (rooms[i][j].ProgressState)
        {
            case RoomProgressState.NOT_STARTED:
                //List<DoorStruct> doors = rooms[i][j].Doors;
                //foreach (DoorStruct door in doors)
                //{
                //    Instantiate(door.Object, door.InitPosition, Quaternion.identity);
                //}
                //rooms[i][j] = new Room(i, j, rooms[i][j].Obj, rooms[i][j].Enemies, rooms[i][j].Doors, rooms[i][j].InstanceState, RoomProgressState.CLOSED);
                break;

            case RoomProgressState.CLOSED:
                // Instantiate enemies
                break;

            case RoomProgressState.BATTLE:
                if (!rooms[i][j].EnemiesAlive())
                {
                    // Open doors

                    rooms[i][j] = new Room(i, j, rooms[i][j].Obj, rooms[i][j].Enemies, rooms[i][j].Doors, rooms[i][j].InstanceState, RoomProgressState.DONE);
                }
                break;

            case RoomProgressState.DONE:
                // chests enabled
                break;

            default:
                break;
        }
    }

    public void SetCurrentRoom(int x, int z)
    {
        currentRoom = new Vector3(x, z);
        //Debug.Log("Current Room: " + currentRoom);
        //// Start room
        // Move camera
        GameObject.Find("OverviewCamera")
            .GetComponent<CameraMover>()
            .ChangeCurrentRoom(currentRoom);
        // Instanciate next rooms if needed
        //EnableRoomAndNeighbours(x, z);
        // Disappear innecessary rooms
        //DisableUselessRooms();
    }

    // Entry point to manage the rooms that have to be instanciated
    private void EnableRoomAndNeighbours(int i, int j)
    {
        // Enable this room and those around it
        for (int x = i - 1; x <= i + 1; ++x)
            for (int y = j - 1; y <= j + 1; ++y)
                EnableRoom(x, y);
    }

    private void EnableRoom(int i, int j)
    {
        Vector2Int boundaries = levelsList.GridSize;
        // Check that we are not outside the grid range
        if (!(i >= 0 && j >= 0 && i < boundaries.x && j < boundaries.y)) return;
        //Debug.Log("Boundaries checked");
        int roomNumber = levelsList.GetCell(i, boundaries.y - 1 - j) - 1;
        // Check that we are not outside the array range or have a 0 (no room)
        //Debug.Log("Room (" + i + "," + j + ") number: " + roomNumber);
        if (roomNumber >= levelsMappings.Length || roomNumber < 0) return;
        //Debug.Log("Room Number checked");
        Texture2D level = levelsMappings[roomNumber];

        //Debug.Log("Enable Room (" + i + "," + j + ")");

        // Check if the room already existed, otherwise create it.
        if (!rooms.ContainsKey(i))
        {
            rooms.Add(i, new Dictionary<int, Room>());
        }
        if (!rooms[i].ContainsKey(j))
        {
            rooms[i].Add(j, new Room(i, j, null, new List<EnemyStruct>(), new List<DoorStruct>(), RoomInstanceState.UNINSTANTIATED, RoomProgressState.NOT_STARTED));
        }

        switch (rooms[i][j].InstanceState)
        {
            case RoomInstanceState.UNINSTANTIATED:
                //Debug.Log("Instanciate room(" + i + ", " + j + ")");
                // Instantiate
                GameObject levelObject = new GameObject();
                levelObject.name = "Room" + i + j;
                // Traverse the Texture2D room map
                for (int x = 0; x < level.width; ++x)
                {
                    for (int z = 0; z < level.height; ++z)
                    {
                        Color pixelColor = level.GetPixel(x, z);
                        //Debug.Log("Pixel (" + x + "," + z + "): (" + pixelColor.r * 255 + "," + pixelColor.g * 255 + "," + pixelColor.b * 255 + "," + pixelColor.a + ")");

                        // Calcualte relative position
                        Vector3 worldOffset = new(8, 0, 8);
                        Vector3 offset = new(sizeOfImage.x * i * sizeOfImage.x, 0, sizeOfImage.x * j * sizeOfImage.y);
                        Vector3 position = new(x * sizeOfImage.x, 0, z * sizeOfImage.x);
                        position += offset;
                        position += worldOffset;

                        if (pixelColor.a > 0) // Pixel not transparent
                        {
                            foreach (ColorToPrefab colorPrefab in colorMappings)
                            {
                                //Debug.Log("colorPrefab: " + "(" + colorPrefab.color.r * 255 + "," + colorPrefab.color.g * 255 + "," + colorPrefab.color.b * 255 + "," + colorPrefab.color.a + ")");
                                //Debug.Log("X: " + x);
                                //Debug.Log("Z: " + z);

                                // Color matches and we are not in the corners
                                if (Mathf.Approximately(colorPrefab.color.r, pixelColor.r) &&
                                    Mathf.Approximately(colorPrefab.color.g, pixelColor.g) &&
                                    Mathf.Approximately(colorPrefab.color.b, pixelColor.b) &&
                                    Mathf.Approximately(colorPrefab.color.a, pixelColor.a) &&
                                    !(x == 0 && z == 0) && // Bottom-left corner
                                    !(x == 0 && z == level.height - 1) && // Top-left corner
                                    !(x == level.width - 1 && z == 0) && // Bottom-right corner
                                    !(x == level.width - 1 && z == level.height - 1)) // Top-right corner
                                {
                                    //if (colorPrefab.prefab.layer == 8) // layer 8 == Enemy
                                    //{
                                    //    // If it is an enemy, save data into the Room and instantiate later
                                    //    rooms[i][j].AddEnemy(new EnemyStruct(colorPrefab.prefab, position, false, false));
                                    //    break;
                                    //}
                                    //else if (colorPrefab.prefab.tag == "Door")
                                    //{
                                    //    Debug.Log("Door");
                                    //    rooms[i][j].AddDoor(new DoorStruct(colorPrefab.prefab, position));
                                    //    break;
                                    //}
                                    //if (colorPrefab.prefab.tag == "Door") break;
                                    GameObject obj = Instantiate(colorPrefab.prefab, position, Quaternion.identity, transform);

                                    // Scale, rotate and move the asset
                                    switch (colorPrefab.prefab.tag)
                                    {
                                        case "Wall":

                                            Vector3 rotationX = new(0, 0, 0);
                                            Vector3 torchOffset = new(0, 15, 0);
                                            // Rotate according to position X in the level
                                            if (x == 0) // Left side
                                            {
                                                rotationX = new(0.0f, 90.0f, 0.0f);
                                                torchOffset += new Vector3(13, 0, 0);
                                            }
                                            else if (x == level.width - 1) // Right side
                                            {
                                                rotationX = new(0.0f, -90.0f, 0.0f);
                                                torchOffset += new Vector3(-13, 0, 0);
                                            }
                                            obj.transform.Rotate(rotationX, Space.World);

                                            Vector3 rotationZ = new(0, 0, 0);
                                            // Rotate according to position Y in the level
                                            if (z == 0) // Bottom side
                                            {
                                                rotationZ = new(0.0f, 0.0f, 0.0f);
                                                torchOffset += new Vector3(0, 0, 13);
                                            }
                                            else if (z == level.height - 1) // Top side
                                            {
                                                rotationZ = new(0.0f, -180.0f, 0.0f);
                                                torchOffset += new Vector3(0, 0, -13);
                                            }
                                            obj.transform.Rotate(rotationZ, Space.World);

                                            // Check if a torch has to be added, if so, apply the transformations
                                            if (colorPrefab.torch != null)
                                            {
                                                GameObject torch = Instantiate(colorPrefab.torch, position + torchOffset, Quaternion.identity, transform);
                                                torch.transform.Rotate(rotationX, Space.World);
                                                torch.transform.Rotate(rotationZ, Space.World);

                                                // Set parent object
                                                torch.transform.parent = levelObject.transform;
                                            }
                                            break;

                                        case "SlideBox":
                                            //obj.transform.GetComponent<Rigidbody>().isKinematic = false;
                                            break;
                                        case "Door":
                                        case "DoorKey":
                                        case "DoorBoss":
                                            // Rotate according to position X in the level
                                            if (x == 0) // Left side
                                            {
                                                obj.transform.Rotate(Vector3.up, 90);
                                            }
                                            else if (x == level.width - 1) // Right side
                                            {
                                                obj.transform.Rotate(Vector3.up, -90);
                                            }

                                            // Rotate according to position Y in the level
                                            if (z == 0) // Bottom side
                                            {

                                            }
                                            else if (z == level.height - 1) // Top side
                                            {
                                                obj.transform.Rotate(Vector3.up, 180);
                                            }
                                            obj.transform.Translate(-8, 0, 0);
                                            break;

                                case "batFlying":
                                    transform.Translate(new Vector3(0, 7, 0), Space.World);
                                    break;

                                        case "Chest":
                                        case "ChestBoss":
                                            obj.transform.Rotate(Vector3.up, 180);
                                            break;
                                        default:
                                            break;
                                    }

                                    // Check for special options
                                    if (colorPrefab.dropCoin)
                                    {
                                        // Assing this property to the gameObject

                                    }



                                    if (colorPrefab.dropLife)
                                    {
                                        // Assing this property to the gameObject

                                    }

                            if (colorPrefab.movementPattern != null)
                            {
                                // Assing this property to the gameObject depending on its type
                                // The gameObject movement script should implement the method
                                // TODO establecer capas y comprobar que esta en la capa que toca (enemies, obstacles, etc...)
                                switch (obj.tag)
                                {
                                    case "Anubis":
                                    case "Skeleton":
                                    case "Bat":
                                    case "Scorpion":
                                    case "Golem":
                                        obj.BroadcastMessage("SetMovementPattern", colorPrefab.movementPattern);
                                        break;

                                            default:
                                                break;
                                        }
                                    }

                                    // Set parent object
                                    obj.transform.parent = levelObject.transform;

                                    if (colorPrefab.prefab.tag != "Wall" &
                                        colorPrefab.prefab.tag != "BigBox")
                                    {
                                        // Instanciate the floor
                                        GameObject floorObject = Instantiate(floor, position, Quaternion.identity, transform);
                                        floorObject.transform.parent = levelObject.transform;
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            // Instanciate the floor wherever there is a blank pixel
                            GameObject floorObject = Instantiate(floor, position, Quaternion.identity, transform);
                            floorObject.transform.parent = levelObject.transform;
                        }
                    }
                }
                rooms[i][j] = new Room(i, j, levelObject, rooms[i][j].Enemies, rooms[i][j].Doors, RoomInstanceState.INSTANTIATED, rooms[i][j].ProgressState);
                //Debug.Log(rooms[i][j]);
                break;

            case RoomInstanceState.HIDDEN:
                //Debug.Log("Unhide room (" + i + "," + j + ")");
                // Activate
                GameObject levelObj = rooms[i][j].Obj;
                levelObj.SetActive(true);
                rooms[i][j] = new Room(i, j, levelObj, rooms[i][j].Enemies, rooms[i][j].Doors, RoomInstanceState.INSTANTIATED, rooms[i][j].ProgressState);
                break;

            default:
                break;
        }
    }

    //private void DisableUselessRooms()
    //{
    //    foreach (KeyValuePair<int, Dictionary<int, Room>> entry1 in rooms)
    //    {
    //        int i = entry1.Key;
    //        foreach (KeyValuePair<int, Room> entry2 in entry1.Value)
    //        {
    //            int j = entry2.Key;
    //            if (rooms[i][j].InstanceState == RoomInstanceState.INSTANTIATED &&
    //                (i < currentRoom.x - 1 || i > currentRoom.x + 1) ||
    //                (j < currentRoom.y - 1 || j > currentRoom.y + 1))
    //            {
    //                DisableRoom(i, j);
    //            }
    //        }
    //    }
    //}

    //private void DisableRoom(int i, int j)
    //{
    //    if (rooms[i][j].InstanceState == RoomInstanceState.INSTANTIATED)
    //    {
    //        Debug.Log("Hide room (" + i + "," + j + ")");
    //        // Deactivate
    //        GameObject levelObj = rooms[i][j].Obj;
    //        levelObj.SetActive(false);
    //        rooms[i][j] = new Room(i, j, levelObj, rooms[i][j].Enemies, rooms[i][j].Doors, RoomInstanceState.HIDDEN, rooms[i][j].ProgressState);
    //    }
    //}
}
