using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Visuals;
using Random = UnityEngine.Random;

namespace Logic
{
    public class MazeGenerator : MonoBehaviour
    {
        [Range(5, 100)] [SerializeField] private int sizeX = 5;
        [Range(5, 100)] [SerializeField] private int sizeY = 5;
        [SerializeField] private MazeNode nodePrefab;
        [SerializeField] private Button generateButton;
        [SerializeField] private Button destroyButton;
        
        private List<MazeNode> nodes = new List<MazeNode>();
        private Vector2Int mazeSize;
        private Coroutine activeCoroutine;

        private void Awake()
        {
            Init();
        }

        private void OnDestroy()
        {
            DeInit();
        }

        public void Init()
        {
            generateButton.onClick.AddListener(GenerateMaze);
            destroyButton.onClick.AddListener(DestroyMaze);
        }

        public void DeInit()
        {
            generateButton.onClick.RemoveAllListeners();
        }

        private void GenerateMaze()
        {
            mazeSize.x = sizeX;
            mazeSize.y = sizeY;
            
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
                DestroyMaze();
            }

            activeCoroutine = StartCoroutine(GenerateMaze(mazeSize));
        }

        IEnumerator GenerateMaze(Vector2Int size)
        {
            if (nodes.Count != 0)
            {
                DestroyMaze();
            }
            
            //Create nodes
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector3 nodePos = new Vector3(x - (size.x / 2), 0, y - (size.y / 2));
                    MazeNode newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
                    nodes.Add(newNode);
                    
                    yield return null;
                }
            }

            List<MazeNode> currentPath = new List<MazeNode>();
            List<MazeNode> completedNodes = new List<MazeNode>();
            
            // Choose starting node
            currentPath.Add(nodes[Random.Range(0, nodes.Count)]);
            currentPath[0].SetState(NodeState.Current);
        }
        
        private void DestroyMaze()
        {
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
            }
            
            foreach (var node in nodes)
            {
                Destroy(node.gameObject);
            }
            
            nodes.Clear();
        }
    }
}
