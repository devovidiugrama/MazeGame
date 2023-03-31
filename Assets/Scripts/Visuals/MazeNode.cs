using UnityEngine;

namespace Visuals
{
    public enum NodeState
    {
        Available,
        Current,
        Completed
    }

    public class MazeNode : MonoBehaviour
    {
        [SerializeField] private GameObject[] walls;
        [SerializeField] private MeshRenderer floor;

        public void SetState(NodeState state)
        {
            switch(state)
            {
                case NodeState.Available:
                    floor.material.color = Color.white;
                    break;
                case NodeState.Current:
                    floor.material.color = Color.blue;
                    break;
                case NodeState.Completed:
                    floor.material.color = Color.green;
                    break;
            }
        }
    }
}