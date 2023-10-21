using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terraforming.Dominoes
{
    public class DominoToken : MonoBehaviour
    {
        DominoPole[] poles;

        private void Awake()
        {
            poles = GetComponentsInChildren<DominoPole>();
        }

    }

}
