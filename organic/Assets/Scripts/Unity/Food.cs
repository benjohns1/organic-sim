using Capabilities;
using UnityEngine;

namespace Unity
{
    [RequireComponent(typeof(Rigidbody))]
    public class Food : MonoBehaviour, IEdible
    {
        private Sim.Food food;
        
        [SerializeField]
        private long energy = 10000;
        
        public void Awake()
        {
            food = new Sim.Food(energy);
        }

        public long Eat()
        {
            var eaten = food.Eat();
            if (food.Energy <= 0)
            {
                Destroy(gameObject);
            }

            return eaten;
        }
    }
}
