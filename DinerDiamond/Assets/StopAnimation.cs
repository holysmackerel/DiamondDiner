using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAnimation : MonoBehaviour
{
   public void AnimStop()
   {
      CustomerController.Instance.isAnimating = false;
   }
}
