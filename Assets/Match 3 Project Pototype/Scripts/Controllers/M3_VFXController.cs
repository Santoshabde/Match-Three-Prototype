using UnityEngine;
using SNGames.CommonModule;
using System.Collections.Generic;

namespace SNGames.M3
{
    [System.Serializable]
    public class VFXData
    {
        public string id;
        public ParticleSystem vfx;
    }

    public class M3_VFXController : SerializeSingleton<M3_VFXController>
    {
        [SerializeField] private List<VFXData> vfxData;

        private Dictionary<string, VFXData> vfxDictionary;

        private void Start()
        {
            vfxDictionary = new Dictionary<string, VFXData>();
            foreach (var item in vfxData)
            {
                if (item.vfx != null && !vfxDictionary.ContainsKey(item.id))
                    vfxDictionary.Add(item.id, item);
            }
        }

        public void SpawnVFX(string id, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            if (vfxDictionary.ContainsKey(id))
            {
                var vfx = Instantiate(vfxDictionary[id].vfx, position, rotation);
                vfx.transform.localScale = scale * 1.6f;
            }
        }
    }
}
