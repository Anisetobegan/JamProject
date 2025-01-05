using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] Vector3 gravityDirection;

    public enum Direction
    {
        Up,
        Down, 
        Left, 
        Right
    }

    public Direction direction;

    public Vector3 GravityDirection { get { return gravityDirection; } }

    void Start()
    {
        
    }
}
