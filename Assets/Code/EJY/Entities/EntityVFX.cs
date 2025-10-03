using System.Collections.Generic;
using System.Linq;
using Blade.Effects;
using UnityEngine;

namespace Blade.Entities
{
    public class EntityVFX : MonoBehaviour, IEntityComponent
    {
        private Dictionary<string, IPlayableVFX> _playableDictionary;
        private Entity _entity;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _playableDictionary = new Dictionary<string, IPlayableVFX>();
            GetComponentsInChildren<IPlayableVFX>().ToList()
                .ForEach(playable => _playableDictionary.Add(playable.VfxName, playable));
        }

        public void PlayVfx(string vfxName, Vector3 position, Quaternion rotation)
        {
            IPlayableVFX vfx = _playableDictionary.GetValueOrDefault(vfxName);
            Debug.Assert(vfx != default(IPlayableVFX), $"{vfxName} is not exist in dictionary");
            
            vfx.PlayVfx(position, rotation);
        }

        public void StopVfx(string vfxName)
        {
            IPlayableVFX vfx = _playableDictionary.GetValueOrDefault(vfxName);
            Debug.Assert(vfx != default(IPlayableVFX), $"{vfxName} is not exist in dictionary");
            
            vfx.StopVfx();
        }
    }
}