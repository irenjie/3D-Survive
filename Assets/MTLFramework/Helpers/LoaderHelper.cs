using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MTLFramework.Helper {
    /// <summary>
    /// 资源加载帮助类
    /// </summary>
    public class LoaderHelper : SingletonBehaviour<LoaderHelper> {

        #region 同步加载
        public T GetAsset<T>(string address) {
            var handle = Addressables.LoadAssetAsync<T>(address);
            var asset = handle.WaitForCompletion();
            return asset;
        }

        public List<T> GetAssetsByLabel<T>(string label) {
            var handle = Addressables.LoadAssetsAsync<T>(label);
            var asset = handle.WaitForCompletion();
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                return asset.ToList();

            return null;
        }

        public GameObject InstantiatePrefab(string address, Transform parent = null, bool InstantiateInWorldSpace = false) {
            var handle = Addressables.InstantiateAsync(address, parent, InstantiateInWorldSpace);
            return handle.WaitForCompletion();
        }
        #endregion

        #region

        #endregion
    }
}