using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MTLFramework.Helper {
    public static class CsvHelper {
        public static string[] ReadLines(string address) {
            var content = LoaderHelper.Get().GetAsset<TextAsset>(address);
            if (content == null)
                return null;

            var lines = content.text.Split('\n').Where(s => !string.IsNullOrEmpty(s)).ToArray();
            Addressables.Release<TextAsset>(content);

            return lines;
        }

    }
}