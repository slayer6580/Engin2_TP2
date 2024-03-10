//using Mirror;
//using UnityEngine;

//public class SpawnAShell : NetworkBehaviour
//{
//    [SerializeField] private GameObject greenShellPrefab;
//    [SerializeField] private GameObject redShellPrefab;
//    [SerializeField] private GameObject blueShellPrefab;
//    [SerializeField] private float spawnInterval = 2f; // Time between spawns
//    [SerializeField] private float spawnCooldown = 1f;

//    private float timeSinceLastSpawn;

//    private void Start()
//    {
//        timeSinceLastSpawn = spawnCooldown;
//    }

//    void Update()
//    {
//        if (!isLocalPlayer) return; // Ensure this is the local player

//        timeSinceLastSpawn += Time.deltaTime;

//        if (Input.GetMouseButtonDown(1) && timeSinceLastSpawn >= spawnCooldown)
//        {
//            CmdSpawnRandomShell();
//            timeSinceLastSpawn = 0; // Reset the timer
//        }
//    }

//    [Command]
//    void CmdSpawnRandomShell()
//    {
//        int shellType = Random.Range(0, 3); // Random number between 0 and 2
//        GameObject shellPrefab = null;

//        switch (shellType)
//        {
//            case 0: shellPrefab = greenShellPrefab; break;
//            case 1: shellPrefab = redShellPrefab; break;
//            case 2: shellPrefab = blueShellPrefab; break;
//        }

//        if (shellPrefab != null)
//        {
//            var shellInstance = Instantiate(shellPrefab, transform.position, Quaternion.identity); // Adjust spawn position as needed
//            NetworkServer.Spawn(shellInstance);
//        }
//    }
//}