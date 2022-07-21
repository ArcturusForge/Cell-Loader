using System;
using UnityEngine.Events;

namespace Arcturus.MapLoader
{
    [Serializable]
    public class OnLoadStart : UnityEvent
    {

    }

    [Serializable]
    public class OnLoadEnd : UnityEvent
    {

    }

    [Serializable]
    public class OnLoadProgress : UnityEvent<float>
    {

    }
}
