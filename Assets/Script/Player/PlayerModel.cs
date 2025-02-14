using MTLFramework.Helper;
using MTLFramework.Helpers;
using NUnit.Framework;
using UnityEngine;

namespace Survive3D.Player {
    public class PlayerModel : MonoBehaviour {
        AudioClip[] footClips;

        public void Init() {
            footClips = new AudioClip[2] {
                LoaderHelper.Get().GetAsset<AudioClip>("Audio/Player/footsteps grass  1.wav"),
                LoaderHelper.Get().GetAsset<AudioClip>("Audio/Player/footsteps grass  2.wav")
            };
        }

        #region ¶¯»­ÊÂ¼þ
        private void OnFootStep1() {
            OnFootStep(0);
        }

        private void OnFootStep2() {
            OnFootStep(1);
        }

        private void OnFootStep(int index) {
            AudioHelper.Get().Play(footClips[index]);
        }
        #endregion
    }
}