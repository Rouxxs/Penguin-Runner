using UnityEngine;
using UnityEngine.Serialization;

namespace GameFlow
{
    public enum GameCamera {
        Init = 0,
        Play = 1,
        Shop = 2,
        Respawn = 3
    }
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance
        {
            get { return instance; }
        }
        private static GameManager instance;
        private GameState _state;
        [FormerlySerializedAs("playerMotor")] public PlayerManager playerManager;
        public WorldGeneration worldGeneration;
        public SceneChunkGeneration sceneChunkGeneration;

    
        public GameObject[] cameras;
        public GameStateInit GameStateInit { get; private set; }
        public GameStatePlay GameStatePlay { get; private set; }
        public GameStateShop GameStateShop { get; private set; }
        public GameStateDeath GameStateDeath { get; private set; }
        private void Awake()
        {
            GameStateInit = GetComponent<GameStateInit>();
            GameStatePlay = GetComponent<GameStatePlay>();
            GameStateShop = GetComponent<GameStateShop>();
            GameStateDeath = GetComponent<GameStateDeath>();
        }

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _state = GameStateInit;
            _state.Construct();
        }

        private void Update()
        {
            if (_state)
            {
                _state.UpdateState();
            }
        }
    
        public void ChangeState(GameState newState)
        {
            if (_state)
            {
                _state.Destruct();
            }
            _state = newState;
            _state.Construct();
        }
    
        public void ChangeCamera(GameCamera c)
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].SetActive(false);
            }
            cameras[(int)c].SetActive(true);
        }
    }
}