using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class OnHealthChangedEvent: UnityEvent<float, Damageable, Vector3> 
{ }

[Serializable]
public class OnDeathEvent: UnityEvent<Damageable, Vector3> 
{ }