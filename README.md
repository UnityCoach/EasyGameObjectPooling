# EasyGameObjectPooling
GameObject Pooling and Spawning made easy.

Author:
	Fred Moreau <info@unitycoach.ca>
Copyright (c) 2019 Frederic Moreau - Unity Coach / Jikkou Publishing Inc.

This Object Pooling system has been designed to minimize the work implementing it.

1 - Easy implementation :
Add a using to UnityCoach.ObjectPooling
	using UnityCoach.ObjectPooling;

Replace your Instantiate() method calls with ObjectPool.GetInstance()
	Instantiate (reference, position, rotation, parent);
	ObjectPool.GetInstance (reference, position, rotation, parent);

Replace your Destroy() method calls with ObjectPool.Release()
	Destroy (gameObject);
	ObjectPool.Release(gameObject);

2 - Optmimal implementation :
Initialize an ObjectPool with the reference, getting a referenceId :
	referenceID = ObjectPool.InitPool (reference, poolTotalObjects);

Then replace your Instantiate() method calls with ObjectPool.GetInstance(), using the referenceId instead
	ObjectPool.GetInstance (referenceId, position, rotation, parent);

3 - Scene Management and Persistant objects
An optional parameter passed to ObjectPool.InitPool() allows flagging the pool as persistent (aka DontDestroyOnLoad)
	ObjectPool.InitPool (reference, poolTotalObjects, persistent);
As a result, the pool will remain when a new scene is loaded. Although, if pooled objects were reparented to objects being destroyed, this will result in them being destroyed as well.
For this reason, the SceneLoading static class, and SceneLoadingHelper (MonoBehaviour), wrap all loading operations from SceneManager, with an extra pre-loading event the ObjectPool subscribes to when made persistent, to receive notifications and regather pooled objects before loading.
If you mean to keep pooled objects from a scene to another, use those wrapper method instead.

4 - Automatic Release Components
Several Release components make it easier to release objects in Gameplay, on events such as Collisions, Rigidbody Sleep, Delay, Renderer No Longer Visible or simply calling Release() on them from Unity Events.
Simply add these components to Gameobjects.

5 - Spawning Objects
A sample Spawner components set is provided, allowing spawning objects at variable rate, in random 2D or 3D locations, with a minimum distance from a Player object, and spawning new objects upon collisions.
These will work with (default) the Pooling System, or without. Simply comment the #define USE_POOLING symbol in the scripts.

