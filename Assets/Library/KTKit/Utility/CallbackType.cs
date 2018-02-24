using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CallbackType : MonoBehaviour {

	public delegate void CallbackV ();
	public delegate void CallbackI (int iArgu);
	public delegate void CallbackF (float fArgu);
	public delegate void CallbackB (bool bArgu);
	public delegate void CallbackMaterial (Material material);

	[Serializable]
	public class UnityEventV : UnityEvent {}
	[Serializable]
	public class UnityEventI : UnityEvent <int> {}
	[Serializable]
	public class UnityEventF : UnityEvent <float> {}
	[Serializable]
	public class UnityEventS : UnityEvent <string> {}

}
