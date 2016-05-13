using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class ObjectCache {
	public GameObject prefab ;
	public int cacheSize = 10;

	public GameObject[] objects ;
	private int cacheIndex = 0;

	public void Initialize (Transform a_parent)
	{
		objects = new GameObject[cacheSize];

		// Instantiate the objects in the array and set them to be inactive
		for (var i = 0; i < cacheSize; i++)
		{
			objects[i] = MonoBehaviour.Instantiate (prefab) as GameObject;
			objects[i].SetActive (false);
            objects[i].name = prefab.name + i; //=============================== objects[i].name + i;
            objects[i].transform.SetParent(a_parent);
            //if(SpawnerPool._instance)
            //    SpawnerPool._instance.m_lstGameObject.Add(objects[i]); // edit 에서는 null 인경우 많음.

        }
	}
    public GameObject GetNextObjectInCache ()  {
		GameObject obj = null;

		// The cacheIndex starts out at the position of the object created
		// the longest time ago, so that one is usually free,
		// but in case not, loop through the cache until we find a free one.
		for (int i = 0; i < cacheSize; i++) {
            try
            {
                obj = objects[cacheIndex];

                // If we found an inactive object in the cache, use that.
                if (!obj.activeSelf)
                    break;

                // If not, increment index and make it loop around
                // if it exceeds the size of the cache
                cacheIndex = (cacheIndex + 1) % cacheSize;
            }
            catch
            {
                return null;
            }
			
		}

		// The object should be inactive. If it's not, log a warning and use
		// the object created the longest ago even though it's still active.
		if (obj.activeSelf) {

            Debug.LogError(
                "Spawn of " + prefab.name +
                " exceeds cache size of " + cacheSize +
                "! Reusing already active object.", obj);
            //SpawnerPool.Destroy (obj);
            return null;
        }

		// Increment index and make it loop around
		// if it exceeds the size of the cache
		cacheIndex = (cacheIndex + 1) % cacheSize;

		return obj;
	}
}

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class SpawnerPool : MonoBehaviour {

    public static SpawnerPool _instance ;
    public Transform m_SpawnRoot;
    public ObjectCache[] caches ;
//    public List<GameObject> m_lstGameObject = new List<GameObject>();

#if UNITY_EDITOR
    public bool _isInitTransList = true;
    void OnEnable_Editor()
    {
        if (_isInitTransList == false) return;
        _isInitTransList = false;
//        m_lstGameObject = new List<GameObject>();
        Setup_Object();
    }
#endif
    protected void OnEnable()
    {
#if UNITY_EDITOR
        OnEnable_Editor();
#endif

    }
    void Setup_Object()
    {
        // Loop through the caches
        for (var i = 0; i < caches.Length; i++)
        {
            // Initialize each cache
            caches[i].Initialize(m_SpawnRoot);
        }

        //// Child 활용
        //for(int i=0; i<m_SpawnRoot.childCount; i++)
        //{
        //    m_lstGameObject
        //}
    }
    void Awake () 
    {
	    // Set the global variable
	    _instance = this;

	    

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public static GameObject Spawn(string str_name, Vector3 position, Quaternion rotation)
    {
        ObjectCache cache = null;

        // Find the cache for the specified prefab
        if (_instance)
        {
            for (var i = 0; i < _instance.caches.Length; i++)
            {
                if (_instance.caches[i].prefab.name == str_name)
                {
                    cache = _instance.caches[i];
                }
            }
        }
        if (cache == null)
        {
            Debug.LogError("~~~~~~~~~~~~Spawn cache == null !");
            return null;
        }
        GameObject obj = cache.GetNextObjectInCache();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }

    public static GameObject Spawn (GameObject prefab , Vector3 position, Quaternion rotation )  
    {
	    ObjectCache cache  = null;

	    // Find the cache for the specified prefab
	    if (_instance) {
		    for (var i = 0; i < _instance.caches.Length; i++) {
			    if (_instance.caches[i].prefab == prefab) {
				    cache = _instance.caches[i];
			    }
		    }
	    }

	    // If there's no cache for this prefab type, just instantiate normally
	    if (cache == null) {
            Debug.LogError("~~~~~~~~~~~~Spawn cache == null !");
		    return Instantiate (prefab, position, rotation) as GameObject;
	    }

	    // Find the next object in the cache
	    GameObject obj = cache.GetNextObjectInCache ();

	    // Set the position and rotation of the object
	    obj.transform.position = position;
	    obj.transform.rotation = rotation;

	    // Set the object to be active
	    obj.SetActive (true);

	    return obj;
    }

    
    public static void Destroy (GameObject objectToDestroy ) {
        Transform tmp = objectToDestroy.transform;
        if (tmp.parent != _instance.m_SpawnRoot)
        {
            tmp.SetParent(_instance.m_SpawnRoot);
        }

        tmp.localPosition = Vector3.zero;
        objectToDestroy.SetActive(false);

    }
}
