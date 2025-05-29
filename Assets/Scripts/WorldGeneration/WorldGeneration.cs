using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldGeneration : MonoBehaviour
{
    // Gameplay
    private float chunkSpawnZ;
    private Queue<Chunk> activeChunks = new Queue<Chunk>();
    private List<Chunk> chunkPool = new List<Chunk>();
    
    // Configuration
    [SerializeField] private int firstChunkSpawnPosition = 5;
    [SerializeField] private int chunkOnScreen = 5;
    [SerializeField] private float despawnDistance = 5.0f;

    [SerializeField] private List<GameObject> chunkPrefabs;
    [SerializeField] private Transform cameraTransform;
    
    private void Awake()
    {
        ResetWorld();
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (chunkPrefabs.Count == 0)
        {
            Debug.LogError("No chunk prefabs found on World Generator!");
            return;
        }
        
        if (cameraTransform == null)
        {
            if (Camera.main != null) cameraTransform = Camera.main.transform;
            Debug.Log("Assigning Camera.main to cameraTransform automatically.");
        }
    }


    public void ScanPosition()
    {
        float cameraPositionZ = cameraTransform.position.z;
        Chunk lastChunk = activeChunks.Peek();
        if (cameraPositionZ >=  lastChunk.transform.position.z + lastChunk._chunkSize + despawnDistance)
        {
            SpawnNewChunk();
            DeleteLastChunk();
        }
    }

    private void SpawnNewChunk()
    {
        // Get a random chunk prefab
        int randomIndex = Random.Range(0, chunkPrefabs.Count);
        
        // Check if chunk is available in the pool
        Chunk newChunk = chunkPool.Find(chunk => !chunk.gameObject.activeSelf && chunk.name == (chunkPrefabs[randomIndex].name + "(Clone)"));
        
        // If chunk not in the pool, instantiate a new one
        if (!newChunk)
        {
            newChunk = Instantiate(chunkPrefabs[randomIndex], transform).GetComponent<Chunk>();
        }
        
        // Set the chunk position
        newChunk.transform.position = new Vector3(0, 0, chunkSpawnZ);
        chunkSpawnZ += newChunk._chunkSize;
        
        // Add the chunk to the active chunks queue
        activeChunks.Enqueue(newChunk);
        newChunk.ShowChunk();
    }
    
    private void DeleteLastChunk()
    {
        Chunk chunk = activeChunks.Dequeue();
        chunk.HideChunk();
        chunkPool.Add(chunk);
    }
    
    public void ResetWorld()
    {
        chunkSpawnZ = firstChunkSpawnPosition;

        for (int i = activeChunks.Count; i > 0; i--)
        {
            DeleteLastChunk();            
        }
        
        for (int i = 0; i < chunkOnScreen; i++)
        {
            SpawnNewChunk();
        }
    }
}
