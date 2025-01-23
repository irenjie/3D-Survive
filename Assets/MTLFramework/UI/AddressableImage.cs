using MTLFramework.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace MTLFramework.UI {
    public class AddressableImage : Image {
        private bool _addressable = false;

        public void SetSprite(Sprite sprite, bool addressable) {
            ReleaseAsset();
            this.sprite = sprite;
            _addressable = addressable;
        }

        public void SetSprite(string address) {
            this.sprite = LoaderHelper.Get().GetAsset<Sprite>(address);
            _addressable = sprite != null;
        }

        public void SetAtlasSprite(SpriteAtlas atlas, string spriteName) {
            ReleaseAsset();
            sprite = atlas.GetSprite(spriteName);
        }

        #region ÊÍ·Å×ÊÔ´
        void ReleaseAsset() {
            if (_addressable && sprite != null) {
                Addressables.Release(sprite);
            }

            _addressable = false;
        }

        protected override void OnDestroy() {
            ReleaseAsset();
            base.OnDestroy();
        }

        #endregion

    }
}