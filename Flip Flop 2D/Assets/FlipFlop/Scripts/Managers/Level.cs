using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    private const float OBSTACLE_WIDTH = 1f;
    private const float CAMERA_ORTHO_SIZE = 10f;
    private const int OBSTACLE_MOVE_SPEED = 20;
    private const float OBSTACLE_DESTROY_X_POS = -22f;
    private const int OBSTACLE_SPAWN_X_POS = +22; 
    private const float GROUND_DESTROY_X_POS = -62f;
    private const float IGOR_X_POSITION = 0f;

    private float obstacleSpawnTimer;
    private float obstacleSpawntimerMax;

    private float crystallPosX;

    private int obstacleSpawned;

    public int scoreCount;

    public int crystallCount;

    public Dictionary<string, Queue<GameObject>> poolDict;

    public List<Pool> pools;

    public List<Transform> crystallList;

    private List<Obstacle> obstacleList;

    private List<Transform> backgroundList;

    private State state;

    public SoundManager soundManager;

    public AudioSource musicSource;

    public List<AudioClip> clips = new List<AudioClip>();

    public static Level instance;


    private enum Difficulty 
    {
        Easy,
        Medium,
        Hard,
        Impossible,
    }

    public enum State 
    {
        WaitingToStart,
        Playing,
        Dead,
    }
    

    private void Awake() 
    {
        if (Application.isMobilePlatform) 
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }


        if(instance == null) 
        {
            instance = this;
        }
        HandleBackGroundSpawn();
        crystallList = new List<Transform>();
        obstacleList = new List<Obstacle>();
        poolDict = new Dictionary<string, Queue<GameObject>>();
        LoadPoolObstacles();
        obstacleSpawntimerMax = 1f;
        SetDifficulty(Difficulty.Easy);
        state = State.WaitingToStart;
    }

    private void Start() 
    {
        PlayerController.instance.OnDied += Player_OnDied;
        PlayerController.instance.OnStartedPlaying += Player_OnStartedPlaying;

    }

    private void Player_OnDied(object sender, System.EventArgs e) 
    {
        state = State.Dead;
        
    }

    private void Player_OnStartedPlaying(object sender, System.EventArgs e) 
    {
        state = State.Playing;
    }




    private void Update() 
    {
        if(state == State.Playing) 
        {
            HandleObstacleSpawning();
            HandleBackGroundMove();
            HandleCrysstallMove();
            HandleObstacleMovement();
            if (!musicSource.isPlaying) 
            {
                PlayMusic();
            }
        }

    }
    private void HandleCrysstallMove() 
    {
        if(crystallList.Count != 0) 
        {
            foreach (Transform t in crystallList.ToArray())
            {
                if (t.transform.position.x > -22f)
                {
                    t.position += new Vector3(-1, 0, 0) * OBSTACLE_MOVE_SPEED * Time.deltaTime;
                    
                }
                else 
                {
                    crystallList.Remove(t);
                    Destroy(t.gameObject);
                }
            }
        }
        
    }
    private void HandleBackGroundSpawn() 
    {
        backgroundList = new List<Transform>();
        Transform ground;
        float backGroundY = 1.74f;
        float backGroundX = 22;
        ground = Instantiate(GameAssets.GetInstance().backGround, new Vector3(backGroundX, backGroundY, 0f), Quaternion.identity);
        backgroundList.Add(ground);
        ground = Instantiate(GameAssets.GetInstance().backGround, new Vector3(backGroundX * 4.9f, backGroundY, 0f), Quaternion.identity);
        backgroundList.Add(ground); 
        
    }
    
    private void HandleBackGroundMove() 
    {
        foreach(Transform gT in backgroundList) 
        {
            gT.Translate(Vector3.left * 10 * Time.deltaTime);
            
            if(gT.position.x < GROUND_DESTROY_X_POS) 
            {
                float rightMostPos = -22f;
                for(int i = 0; i < backgroundList.Count; i++) 
                {
                    if(backgroundList[i].position.x > rightMostPos) 
                    {
                        rightMostPos = backgroundList[i].position.x;
                    }
                }
                
                gT.position = new Vector3(107f, gT.position.y, gT.position.z);
            }
        }
        


    }

    private void LoadPoolObstacles() 
    { 
        foreach(Pool pool in pools) 
        {
            Queue<GameObject> objPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++) 
            {
                GameObject obj = Instantiate(pool.prefab);
                Obstacle obs = new Obstacle(obj);
                obstacleList.Add(obs);
                obj.SetActive(false);
                objPool.Enqueue(obj);

            }

            poolDict.Add(pool.tag, objPool);
        }
    }

    private GameObject SpawnPoolObstacles(string tag) 
    {
        if (!poolDict.ContainsKey(tag)) 
        {
            Debug.Log("Pool does not exist");
            return null;
        }

        GameObject objToSpawn = poolDict[tag].Dequeue();
       
        objToSpawn.SetActive(true);
        poolDict[tag].Enqueue(objToSpawn);

        return objToSpawn;
    } 
    

    private void HandleObstacleSpawning() 
    {
        obstacleSpawnTimer -= Time.deltaTime;
        if(obstacleSpawnTimer < 0) 
        {
            bool pointToSpawn = Random.Range(0f, 10f) > 5f;
            int randomHeight = Random.Range(3, 10);
            obstacleSpawnTimer += obstacleSpawntimerMax;
            SetObstacle(randomHeight, OBSTACLE_SPAWN_X_POS, pointToSpawn);
            int rand = Random.Range(0, 10);
            if(rand > 8) 
            {
                CreateCrystall(pointToSpawn);
            }
        }
    }

    private void HandleObstacleMovement() 
    {
        foreach(Obstacle obs in obstacleList) 
        {
           
            bool isToTheRightOfPlayer = obs.GetXPos() > IGOR_X_POSITION;
            obs.Move();
            if (isToTheRightOfPlayer && obs.GetXPos() <= IGOR_X_POSITION)
            {
                scoreCount++;
                if(Input.GetMouseButtonDown(0))
                    soundManager.PlaySound(SoundManager.Sounds.GoThrow);
            }
            if (obs.GetXPos() < OBSTACLE_DESTROY_X_POS)
            {
                poolDict["Obstacle"].Enqueue(obs.obstacleTransform);
                obs.obstacleTransform.gameObject.SetActive(false);
               
            }
        }
    }

    private void SetObstacle(float height, float xPos, bool createOnBottom) 
    {
        Obstacle obstacle = new Obstacle(SpawnPoolObstacles("Obstacle"));
        
        float obstaclePosY;
        if (createOnBottom)
        {
            obstaclePosY = -CAMERA_ORTHO_SIZE + 1.11f;
            obstacle.obstacleTransform.transform.localScale = new Vector3(1, 1, 1);
        }
        else 
        {
            obstaclePosY = +CAMERA_ORTHO_SIZE - 1.02f;
            obstacle.obstacleTransform.transform.localScale = new Vector3(1, -1, 1);
        }
        obstacle.obstacleTransform.transform.position = new Vector3(xPos, obstaclePosY);
       

        SpriteRenderer obstacleRenderer = obstacle.obstacleTransform.GetComponent<SpriteRenderer>();
        obstacleRenderer.size = new Vector2(OBSTACLE_WIDTH, height);

        BoxCollider2D obsCol = obstacle.obstacleTransform.GetComponent<BoxCollider2D>();
        obsCol.size = new Vector2(OBSTACLE_WIDTH, height);
        obsCol.offset = new Vector2(0f, height * 0.5f);

        
        obstacleSpawned++;
        SetDifficulty(GetDifficulty());
    }

    private void CreateCrystall(bool createOnBottom) 
    {
        Transform crystall = Instantiate(GameAssets.GetInstance().crystall);

        float crystallPosY;
        if (createOnBottom)
        {
            crystallPosY = -CAMERA_ORTHO_SIZE + 5;
        }
        else
        {
            crystallPosY = +CAMERA_ORTHO_SIZE - 5;
        }

        crystall.transform.position = new Vector3(crystallPosX, crystallPosY);
        crystallList.Add(crystall);
    }

    private void SetDifficulty(Difficulty difficulty) 
    {
        switch (difficulty) 
        {
            case Difficulty.Easy:
                obstacleSpawntimerMax = 1f;
                crystallPosX = OBSTACLE_SPAWN_X_POS + 7f;
                musicSource.clip = clips[0];
                
                break;
            case Difficulty.Medium:
                obstacleSpawntimerMax = 0.7f;
                crystallPosX = OBSTACLE_SPAWN_X_POS + 5f;
                musicSource.clip = clips[1];
                break;
            case Difficulty.Hard:
                obstacleSpawntimerMax = 0.5f;
                crystallPosX = OBSTACLE_SPAWN_X_POS + 3.5f;
                musicSource.clip = clips[1];
                break;
            case Difficulty.Impossible:
                obstacleSpawntimerMax = 0.38f;
                crystallPosX = OBSTACLE_SPAWN_X_POS + 2.5f;
                musicSource.clip = clips[1];
                break;

        }
    }

    private Difficulty GetDifficulty() 
    {
        if (obstacleSpawned >= 70) return Difficulty.Impossible;
        if (obstacleSpawned >= 40) return Difficulty.Hard;
        if (obstacleSpawned >= 10) return Difficulty.Medium;
        return Difficulty.Easy;
    }

    public int GetObstaclesSpawned() 
    {
        return obstacleSpawned;
    }

    public int GetScore() 
    {
        return scoreCount;
    }

    public void PlayMusic() 
    {
        musicSource.Play();

    }

    public void StopMusic() 
    {
        musicSource.Stop();
    }

    private class Obstacle 
    {
        public GameObject obstacleTransform;

        public Obstacle(GameObject obstacleTransform) 
        {
            this.obstacleTransform = obstacleTransform;
        }


        public void Move() 
        {
            obstacleTransform.transform.Translate(new Vector3(-1, 0, 0) * OBSTACLE_MOVE_SPEED * Time.deltaTime);

        }

        public float GetXPos() 
        {
            return obstacleTransform.transform.position.x;
        }

        
    }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
}
