using UnityEngine;

namespace AIEvent
{
    public class EventScript
    {
        private bool isFireAllowed = false;    //false - fire is not allowed || true - fire is allowed 
        private bool canAttack = true;
        
        private GameObject Player;
        private GameObject Enemy;
    
        public EventScript(GameObject Player, GameObject Enemy)
        {
            this.Player = Player;
            this.Enemy = Enemy;

        }
    
    
    }
}
