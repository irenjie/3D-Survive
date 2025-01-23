using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MTLFramework.Helper {
    public static class FileHelper {
        public static void SaveBytes(string path, string fileName, byte[] data) {
            SaveBytes(Path.Combine(path, fileName), data);
        }

        public static void SaveBytes(string fileName, byte[] bytes) {

            try {
                if (!Directory.Exists(Path.GetDirectoryName(fileName))) {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                }
                if (File.Exists(fileName))
                    File.Delete(fileName);
                File.WriteAllBytes(fileName, bytes);
            } catch (Exception ex) {
                UnityDebugHelper.LogException(ex);
            }
        }

        public static void SaveLines(string path, string fileName, string[] lines) {
            SaveLines(Path.Combine(path, fileName), lines);
        }

        public static void SaveLines(string fileName, string[] lines) {
            if (!Directory.Exists(Path.GetDirectoryName(fileName))) { Directory.CreateDirectory(Path.GetDirectoryName(fileName)); }
            if (File.Exists(fileName))
                File.Delete(fileName);
            File.WriteAllLines(fileName, lines);
        }

        public static void SaveText(string path, string fileName, string content) {
            SaveText(Path.Combine(path, fileName), content);
        }

        public static void SaveText(string fileName, string content) {
            if (!Directory.Exists(Path.GetDirectoryName(fileName))) { Directory.CreateDirectory(Path.GetDirectoryName(fileName)); }
            if (File.Exists(fileName))
                File.Delete(fileName);
            File.AppendAllText(fileName, content, System.Text.Encoding.UTF8);
        }

        public static void AppendText(string path, string fileName, string content) {
            AppendText(Path.Combine(path, fileName), content);
        }

        public static void AppendText(string fileName, string content) {
            if (!Directory.Exists(Path.GetDirectoryName(fileName))) { Directory.CreateDirectory(Path.GetDirectoryName(fileName)); }
            File.AppendAllText(fileName, content);
        }

        /// <summary>
        /// 获取指定目录下的所有文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetAllFiles(string path) {
            var arrays = new List<string>();
            if (Directory.Exists(path)) {
                arrays.AddRange(Directory.GetFiles(path));
                Directory.GetDirectories(path).ToList().ForEach((e) => {
                    arrays.AddRange(GetAllFiles(e).ToList());
                });
            }
            return arrays.ToArray();
        }

        /// <summary>
        /// 获取指定目录下的文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetFiles(string path) {
            var arrays = new List<string>();
            if (Directory.Exists(path)) {
                arrays.AddRange(Directory.GetFiles(path));
            }
            return arrays.ToArray();
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool Exists(string path, string fileName) {
            return File.Exists(Path.Combine(path, fileName));
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool Exists(string fileName) {
            return File.Exists(fileName);
        }

        /// <summary>
        /// 读文本文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadText(string fileName) {
            if (Exists(fileName)) { return File.ReadAllText(fileName); }
            return "";
        }

        /// <summary>
        /// 读文本文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string[] ReadAllLines(string path, string fileName) {
            return ReadAllLines(Path.Combine(path, fileName));
        }

        public static string[] ReadAllLines(string fileName) {
            if (Exists(fileName)) { return File.ReadAllLines(fileName); }
            return new string[0];
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="cacheFile"></param>
        public static void DeleteFile(string cacheFile) {
            if (File.Exists(cacheFile)) {
                File.Delete(cacheFile);
            }
        }

    }
}