using UnityEngine;public class EnemyMovement : MonoBehaviour{    private CharacterController controller;    private Vector3 velocity;    public Transform groundCheck;    public float gravity = -9.81f;    public float groundDistance = 0.4f;    public LayerMask environmentMask;    // Start is called once before the first execution of Update after the MonoBehaviour is created    void Start()    {        controller = GetComponent<CharacterController>();    }    // Update is called once per frame    void Update()    {        velocity.y += gravity * Time.deltaTime;        controller.Move(velocity* Time.deltaTime);    }    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(""))
        {

        }
    }}