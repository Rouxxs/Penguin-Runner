using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
   protected PlayerManager Manager;

   public virtual void Construct()
   {
     // Debug.Log("Constructing: " + this.ToString());
   }
   public virtual void Destruct() {}
   public virtual void Transition() {}

   private void Awake()
   {
      Manager = GetComponent<PlayerManager>();
   }

   public virtual Vector3 ProcessMotion()
   {
      //Debug.Log("ProcessMotion is not implemented in " + this.ToString());
      return Vector3.zero;
   }
   
   
}
