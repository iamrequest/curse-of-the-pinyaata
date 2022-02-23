using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class OnHealthChangedEvent: UnityEvent<float, Damageable> 
{ }

[Serializable]
public class OnDeathEvent: UnityEvent<Damageable> 
{ }