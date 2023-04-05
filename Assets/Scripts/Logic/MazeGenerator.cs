using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [Range(5, 100)] [SerializeField] private int sizeX = 25;
        [Range(5, 100)] [SerializeField] private int sizeY = 25;
        [SerializeField] private MazeNode nodePrefab;
        [SerializeField] private Button generateButton;
        [SerializeField] private Button destroyButton;
        
        private List<MazeNode> nodes = new List<MazeNode>();
        private Vector2Int mazeSize;

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

            GenerateMaze(mazeSize);
        }

        private void GenerateMaze(Vector2Int size)
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
                }
            }

            List<MazeNode> currentPath = new List<MazeNode>();
            List<MazeNode> completedNodes = new List<MazeNode>();
            
            // Choose starting node
            currentPath.Add(nodes[Random.Range(0, nodes.Count)]);
            currentPath[0].SetState(NodeState.Start);

            while (completedNodes.Count < nodes.Count)
            {
                // Check nodes next to the current node
                List<int> possibleNextNodes = new List<int>();
                List<int> possibleDirections = new List<int>();

                int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
                int currentNodeX = currentNodeIndex / size.y;
                int currentNodeY = currentNodeIndex % size.y;

                if (currentNodeX < size.x - 1)
                {
                    // Check node to the right
                    if (!completedNodes.Contains(nodes[currentNodeIndex + size.y]) &&
                        !currentPath.Contains(nodes[currentNodeIndex + size.y]))
                    {
                        possibleDirections.Add(1);
                        possibleNextNodes.Add(currentNodeIndex + size.y);
                    }
                }
                if (currentNodeX > 0)
                {
                    // Check node to the left
                    if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) &&
                        !currentPath.Contains(nodes[currentNodeIndex - size.y]))
                    {
                        possibleDirections.Add(2);
                        possibleNextNodes.Add(currentNodeIndex - size.y);
                    }
                }

                if (currentNodeY < size.y - 1)
                {
                    //Check node above
                    if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                        !currentPath.Contains(nodes[currentNodeIndex + 1]))
                    {
                        possibleDirections.Add(3);
                        possibleNextNodes.Add(currentNodeIndex + 1);
                    }
                }
                if (currentNodeY > 0)
                {
                    //Check node below
                    if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                        !currentPath.Contains(nodes[currentNodeIndex - 1]))
                    {
                        possibleDirections.Add(4);
                        possibleNextNodes.Add(currentNodeIndex - 1);
                    }
                }
                
                // Pick next node
                if (possibleDirections.Count > 0)
                {
                    int chosenDirection = Random.Range(0, possibleDirections.Count);
                    MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

                    switch (possibleDirections[chosenDirection])
                    {
                        case 1:
                            chosenNode.RemoveWall(1);
                            currentPath[currentPath.Count - 1].RemoveWall(0);
                            break;
                        case 2:
                            chosenNode.RemoveWall(0);
                            currentPath[currentPath.Count - 1].RemoveWall(1);
                            break;
                        case 3:
                            chosenNode.RemoveWall(3);
                            currentPath[currentPath.Count - 1].RemoveWall(2);
                            break;
                        case 4:
                            chosenNode.RemoveWall(2);
                            currentPath[currentPath.Count - 1].RemoveWall(3);
                            break;
                    }
                    
                    currentPath.Add(chosenNode);
                }
                else
                {
                    completedNodes.Add(currentPath[currentPath.Count - 1]);
                    currentPath.RemoveAt(currentPath.Count - 1);
                }
            }
            
            nodes[nodes.Count - 1].SetState(NodeState.Finish);
        }
        
        private void DestroyMaze()
        {
            foreach (var node in nodes)
            {
                Destroy(node.gameObject);
            }
            
            nodes.Clear();
        }
    }
}
