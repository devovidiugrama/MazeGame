using UnityEngine;

namespace Visuals
{
    public enum NodeState
    {
        Available,
        Start,
        Finish
    }

    public class MazeNode : MonoBehaviour
    {
        [SerializeField] private GameObject[] walls;
        [SerializeField] private MeshRenderer floor;
        [SerializeField] private Texture finishFloor;

        public void RemoveWall(int wallToRemove)
        {
            walls[wallToRemove].gameObject.SetActive(false);
        }
        
        public void SetState(NodeState state)
        {
            switch(state)
            {
                case NodeState.Available:
                    floor.material.color = Color.white;
                    break;
                case NodeState.Start:
                    floor.material.color = Color.green;
                    break;
                case NodeState.Finish:
                    floor.material.mainTexture = finishFloor;
                    break;
            }
        }
    }
}