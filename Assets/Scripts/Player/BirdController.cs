using System;
using ScriptableObjects;
using UnityEngine;


namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BirdController : MonoBehaviour
    {
        [Header("Scripts")] 
        [SerializeField] private GameManager gameManager;

        [Header("Game Assets")] 
        [SerializeField] private BirdCollection _birdCollection;

        [Space] [Header("Properties")] 
        [SerializeField] private float jumpAmount = 30f;
        [SerializeField] private float eulerAngleAmount = 1f;
        [SerializeField] private BirdType _birdType = BirdType.YellowBird;

        [Space] [Header("Components")] [SerializeField]
        private Rigidbody2D birdRigidbody2D;
        
        private Animator birdAnimator;
        private static readonly int Dead = Animator.StringToHash("Dead");

        public delegate void OnBirdDied();
        public event OnBirdDied BirdDied;
            
        public delegate void OnStartedPlaying();
        public event OnStartedPlaying GameStarted;

        private void Start()
        {
            GameObject birdPrefab = _birdCollection.Birds.Find(bird => bird.BirdType == _birdType).Prefab;
            birdAnimator = Instantiate(birdPrefab, transform).GetComponent<Animator>();
            birdRigidbody2D.bodyType = RigidbodyType2D.Static;
            BirdDied += gameManager.BirdOnDied;
            GameStarted += gameManager.BirdOnStartedPlaying;
        }

        void Update()
        {
            switch (gameManager.State)
            {
                case State.WaitingToStart:
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                    {
                        birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                        Jump();
                        GameStarted?.Invoke();
                    }

                    break;
                case State.Playing:
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                    {
                        Jump();
                    }

                    transform.eulerAngles = new Vector3(0, 0, birdRigidbody2D.velocity.y * eulerAngleAmount);
                    break;
                case State.BirdDead:

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void Jump()
        {
            birdRigidbody2D.velocity = Vector2.up * jumpAmount;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            birdRigidbody2D.bodyType = RigidbodyType2D.Static;
            birdAnimator.SetBool(Dead, true);
            BirdDied?.Invoke();
        }
    }
}