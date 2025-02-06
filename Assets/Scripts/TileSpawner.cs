using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct BlockSpawnData
    {
        public GameObject blockPrefab;
        public Transform spawnPoint;
    }

    public BlockSpawnData[] leftSideBlocks;
    public BlockSpawnData[] rightSideBlocks;

    private void Start()
    {
        SpawnBlocks(leftSideBlocks);
        SpawnBlocks(rightSideBlocks);
    }

    void SpawnBlocks(BlockSpawnData[] blockDataArray)
    {
        foreach (BlockSpawnData data in blockDataArray)
        {
            if (!data.blockPrefab || !data.spawnPoint) return;
            var block = Instantiate(data.blockPrefab, data.spawnPoint.position, Quaternion.identity);
            block.transform.SetParent(data.spawnPoint.transform);
            var blockController = block.GetComponent<BlockController>();
            if (blockController)
            {
                blockController.InitialPosition = block.transform.position;
            }
        }
    }
}