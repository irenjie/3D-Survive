using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Extensions {
    public static class CollectionExtensions {
        /// <summary>
        /// 打乱数组
        /// 洗牌算法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list) {
            if (list == null)
                return;

            Random _rand = new Random(DateTime.Now.Millisecond);
            int n = list.Count;
            while (n > 1) {
                n--;
                var k = _rand.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T TryGetAt<T>(this IList<T> list, int index) {
            if (list == null || index < 0 || list.Count < index)
                return default;
            return list[index];
        }

        public static bool TryFind<T>(this LinkedList<T> list, Predicate<T> match, out T result) {
            result = default;

            if (list == null || list.Count == 0)
                return false;
            if (match == null) {
                result = list.First.Value;
                return true;
            }

            foreach (T item in list) {
                if (match(item)) {
                    result = item;
                    return true;
                }
            }

            return false;
        }

        public static void AddBefore<T>(this LinkedList<T> list, Predicate<T> match, T value) {
            if (list == null)
                return;
            if (list.Count == 0) {
                list.AddFirst(value);
                return;
            }

            LinkedListNode<T> newNode = new LinkedListNode<T>(value);
            LinkedListNode<T> curNode = list.First;
            while (curNode != null && !match(curNode.Value)) {
                curNode = curNode.Next;
            }
            if (curNode != null) {
                list.AddBefore(curNode, newNode);
            } else {
                list.AddLast(newNode);
            }
        }

    }
}