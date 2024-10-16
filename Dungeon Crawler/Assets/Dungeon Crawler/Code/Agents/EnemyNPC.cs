using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SotomaYorch.DungeonCrawler
{
    #region Enums

    public enum EnemyBehaviourState
    {
        PATROL,
        PERSECUTION
    }

    #endregion

    #region Structs


    #endregion

    public class EnemyNPC : Agent
    {
        #region Knobs

        public EnemyBehaviours_ScriptableObject scriptBehaviours;

        #endregion

        #region References

        #endregion

        #region RuntimeVariables

        protected EnemyBehaviourState _currentEnemyBehaviourState;
        protected EnemyBehaviour _currentEnemyBehaviour;
        protected int _currentEnemyBehaviourIndex;
        protected Transform _avatarsTransform;
        protected StateMechanics _previousMovementDirection;

        #endregion

        #region LocalMethods

        protected void InvokeStateMechanic()
        {
            switch (_currentEnemyBehaviour.type)
            {
                case EnemyBehaviourType.STOP:
                    _fsm.StateMechanic(StateMechanics.STOP);
                    break;
                case EnemyBehaviourType.FIRE:
                    //_fsm.StateMechanic(StateMechanics.MOVE_RIGHT);
                    break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    //TODO: Obtain the State Mechanic direction, 
                    //according to the direction of the enemy
                    _fsm.StateMechanic(_movementStateMechanic);
                    break;
            }
        }

        protected IEnumerator TimerForEnemyBehaviour()
        {
            yield return new WaitForSeconds(_currentEnemyBehaviour.time);
            FinalizeSubState();
            GoToNextEnemyBehaviour();
        }

        protected void GoToNextEnemyBehaviour()
        {
            _currentEnemyBehaviourIndex++;
            if (_currentEnemyBehaviourState == EnemyBehaviourState.PATROL)
            {
                if (_currentEnemyBehaviourIndex >= scriptBehaviours.patrolBehaviours.Length)
                    _currentEnemyBehaviourIndex = 0;
                _currentEnemyBehaviour = scriptBehaviours.patrolBehaviours[_currentEnemyBehaviourIndex];
            }
            else // PERSECUTING
            {
                if (_currentEnemyBehaviourIndex >= scriptBehaviours.persecutionBehaviours.Length)
                    _currentEnemyBehaviourIndex = 0;
                _currentEnemyBehaviour = scriptBehaviours.persecutionBehaviours[_currentEnemyBehaviourIndex];
            }
            InitializeSubState();
            CalculateStateMechanicDirection();
            InvokeStateMechanic();
            if (_currentEnemyBehaviour.time > 0)
            {
                //It is not a perpetual finite state,
                //so we will start the clock ;)
                StartCoroutine(TimerForEnemyBehaviour());
            }
        }

        protected void InitializeSubState()
        {
            switch (_currentEnemyBehaviour.type)
            {
                case EnemyBehaviourType.STOP:
                    InitializeStopSubStateMachine();
                    break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                    InitializeMoveToRandomDirectionSubStateMachine();
                    break;
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    InitializePersecuteTheAvatarSubStateMachine();
                    break;
            }
        }

        protected void FinalizeSubState()
        {
            switch (_currentEnemyBehaviour.type)
            {
                case EnemyBehaviourType.STOP:
                    FinalizeStopSubStateMachine();
                    break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                    FinalizeMoveToRandomDirectionSubStateMachine();
                    break;
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    FinalizePersecuteTheAvatarSubStateMachine();
                    break;
            }
        }

        protected void IntializePatrolBehaviour()
        {
            StopAllCoroutines();
            //To initialize the sub-finite state machine
            _currentEnemyBehaviourState = EnemyBehaviourState.PATROL;
            _currentEnemyBehaviourIndex = 0;

            if (scriptBehaviours.patrolBehaviours.Length > 0)
            {
                _currentEnemyBehaviour = scriptBehaviours.patrolBehaviours[0];
            }
            else
            {
                //Plan if the array is empty for this enemy NPC
                _currentEnemyBehaviour.type = EnemyBehaviourType.STOP;
                _currentEnemyBehaviour.time = -1; //-1 equals an infinity / perpetual state
            }
            //Initialize the proper sub-state
            InitializeSubState();
            CalculateStateMechanicDirection();
            InvokeStateMechanic();


            if (_currentEnemyBehaviour.time > 0)
            {
                //It is not a perpetual finite state,
                //so we will start the clock ;)
                StartCoroutine(TimerForEnemyBehaviour());
            }
        }
        #endregion

        #region UnityMethods
        public void InitializePersecutingBehaviour()
        {
            //To initialize the sub-finite state machine
            _currentEnemyBehaviourState = EnemyBehaviourState.PERSECUTION;
            _currentEnemyBehaviourIndex = 0;

            if (scriptBehaviours.patrolBehaviours.Length > 0)
            {
                _currentEnemyBehaviour = scriptBehaviours.persecutionBehaviours[0];
            }
            else
            {
                //Plan if the array is empty for this enemy NPC
                _currentEnemyBehaviour.type = EnemyBehaviourType.PERSECUTE_THE_AVATAR;
                _currentEnemyBehaviour.time = -1; //-1 equals an infinity / perpetual state
                _currentEnemyBehaviour.speed = 1.0f;
            }
            InvokeStateMechanic();
            InitializeSubState();

            if (_currentEnemyBehaviour.time > 0)
            {
                //It is not a perpetual finite state,
                //so we will start the clock ;)
                StartCoroutine(TimerForEnemyBehaviour());
            }
        }


        void Start()
        {
            InitializeAgent();
        }

        void FixedUpdate()
        {
            switch (_currentEnemyBehaviour.type)
            {
                case EnemyBehaviourType.STOP:
                    ExecutingStopSubStateMachine();
                    break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                    ExecutingMoveToRandomDirectionSubStateMachine();
                    break;
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    ExecutingPersecuteTheAvatarSubStateMachine();
                    break;
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                _avatarsTransform = other.gameObject.transform;
                InitializePersecutingBehaviour();
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                _avatarsTransform = null;
                InitializeAgent();
            }
        }
        #endregion

        #region PublicMethods

        public override void InitializeAgent()
        {
            IntializePatrolBehaviour();
        }

        #endregion

        #region GettersSetters

        #endregion

        #region SubStateMachineStates

        #region StopSubStateMachineMethods

        protected void InitializeStopSubStateMachine()
        {
            _fsm.SetMovementSpeed = 0.0f;
            _fsm.SetMovementDirection = Vector2.zero;
        }

        protected void ExecutingStopSubStateMachine()
        {
            //do nothing
        }

        protected void FinalizeStopSubStateMachine()
        {
            //do nothing
        }

        #endregion StopSubStateMachineMethods

        #region MoveToRandomDirectionSubStateMachineMethods

        protected void InitializeMoveToRandomDirectionSubStateMachine()
        {
            do
            {

                _movementDirection =
                    new Vector2(
                        UnityEngine.Random.Range(-1.0f, 1.0f),
                        UnityEngine.Random.Range(-1.0f, 1.0f)
                    ).normalized;
            } while (_movementDirection.magnitude == 0.0f);
            _fsm.SetMovementDirection = _movementDirection;
            _fsm.SetMovementSpeed = _currentEnemyBehaviour.speed;
        }

        protected void ExecutingMoveToRandomDirectionSubStateMachine()
        {
            //Do nothing
        }

        protected void FinalizeMoveToRandomDirectionSubStateMachine()
        {
            _rigidbody.velocity = Vector2.zero;
        }

        #endregion MoveToRandomDirectionSubStateMachineMethods

        #region PersecuteTheAvatarSubStateMachineMethods

        protected void InitializePersecuteTheAvatarSubStateMachine()
        {
            _fsm.SetMovementDirection = (_avatarsTransform.position - transform.position).normalized;
            _fsm.SetMovementSpeed = _currentEnemyBehaviour.speed;
            _previousMovementDirection = _movementStateMechanic;
        }

        protected void ExecutingPersecuteTheAvatarSubStateMachine()
        {// as the avatar may move, we have to update the direction towards him/her
            _movementDirection = (_avatarsTransform.position - transform.position).normalized;
            _fsm.SetMovementDirection = _movementDirection;
            CalculateStateMechanicDirection();
            if (_previousMovementDirection != _movementStateMechanic)
            {
                InvokeStateMechanic();
                _previousMovementDirection = _movementStateMechanic;
            }
        }

        protected void FinalizePersecuteTheAvatarSubStateMachine()
        {
            _fsm.SetMovementSpeed = 0.0f;
            _fsm.SetMovementDirection = Vector2.zero;
        }

        #endregion PersecuteTheAvatarSubStateMachineMethods

        #endregion SubStateMachineStates

    }
}