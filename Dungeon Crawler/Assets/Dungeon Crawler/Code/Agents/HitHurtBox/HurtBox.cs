using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SotomaYorch.DungeonCrawler
{
    [RequireComponent(typeof(Collider2D))]
    public class HurtBox : MonoBehaviour
    {
        #region KnobsParameters

        //TODO: Stored in a Sciptable Object for robustness
        public int maxHealthPoints = 3; //HP
        public float cooldownTime = 1f; //Damage (Hit Box) Per Second (Cooldown)

        #endregion

        #region References

        [SerializeField] protected Agent _agent;

        #endregion

        #region RuntimeVariables

        protected bool _isInCooldown;
        [SerializeField] protected int _currentHealthPoints;

        #endregion

        #region UnityMethods

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            if (_agent == null)
            {
                _agent = transform.parent.gameObject.GetComponent<Agent>();
            }
            #endif
        }

        private void Start()
        {
            _currentHealthPoints = maxHealthPoints;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!_isInCooldown) //to be immune or not ;)
            {
                //I have the potential to be harmed by a Hit Box :O
                if (other.gameObject.CompareTag("HitBox"))
                {
                    //This Hit Box may hurt me if it is from another type
                    //of entity -> Friendly Fire = false
                    if (other.gameObject.layer != gameObject.layer)
                    {
                        //Damn, I am about to be hurt DX
                        _currentHealthPoints -= 1; //other.gameObject.GetComponent<HitBox>().GetDamage;
                        //check if I am already dead
                        if (_currentHealthPoints <= 0)
                        {
                            //so I will die, it is time :'(
                            _agent.StateMechanic(StateMechanics.DIE); //DEATH
                            // TODO: Complete the administration of this state
                            //Animator
                            //Initialize, Executing and Finalize
                        }
                        else
                        {
                            StartCoroutine(Cooldown());
                        }
                    }
                }
            }
        }

        #endregion

        #region Coroutines

        protected IEnumerator Cooldown()
        {
            _isInCooldown = true; //To be Inmune for a certain time ;)
            yield return new WaitForSeconds(cooldownTime);
            _isInCooldown = false;
        }

        #endregion
    }
}