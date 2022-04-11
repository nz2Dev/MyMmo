using System.Linq;
using MyMmo.Commons.Scripts;
using UnityEngine;
using UnityEngine.Assertions;

namespace Player {
    public class UnityScriptsTimeline : MonoBehaviour {

        public void PlayChangesClipImmediately(int locationId, ScriptsClipData clip) {
            var locationScriptsPlayer = FindObjectsOfType<Location>()
                .FirstOrDefault(location => location.Id == locationId);
            Assert.IsNotNull(locationScriptsPlayer);
            locationScriptsPlayer.PlayClipImmediately(clip);
        }

    }
}