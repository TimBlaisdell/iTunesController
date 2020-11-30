using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace iTunesControllerLib {
    public class HashCollection : ICollection<HashEntry> {
        public HashEntry this[int i] => _hashes[i];
        public int Count => _hashes.Count;
        public bool IsReadOnly => false;
        public void Add(HashEntry item) {
            _hashes.Add(item);
        }
        public void Clear() {
            _hashes.Clear();
        }
        public bool Contains(HashEntry item) {
            if (item == null) return false;
            foreach (var h in _hashes) {
                if (string.Compare(h.Filename, item.Filename, StringComparison.OrdinalIgnoreCase) == 0) return true;
                if (h.Hash.Length == item.Hash.Length) {
                    bool same = true;
                    for (int i = 0; i < h.Hash.Length; ++i) {
                        if (item.Hash[i] != h.Hash[i]) {
                            same = false;
                            break;
                        }
                    }
                    if (same) return true;
                }
            }
            return false;
        }
        public void CopyTo(HashEntry[] array, int arrayIndex) {
            _hashes.CopyTo(array, arrayIndex);
        }
        public IEnumerator<HashEntry> GetEnumerator() {
            return _hashes.GetEnumerator();
        }
        public bool Remove(HashEntry item) {
            if (item == null) return false;
            foreach (var h in _hashes.ToArray()) {
                if (string.Compare(h.Filename, item.Filename, StringComparison.OrdinalIgnoreCase) == 0) {
                    _hashes.Remove(h);
                    return true;
                }
                if (h.Hash.Length == item.Hash.Length) {
                    bool same = true;
                    for (int i = 0; i < h.Hash.Length; ++i) {
                        if (item.Hash[i] != h.Hash[i]) {
                            same = false;
                            break;
                        }
                    }
                    if (same) {
                        _hashes.Remove(h);
                        return true;
                    }
                }
            }
            return false;
        }
        public void Write(string filename) {
            var lines = _hashes.Select(h => h.Filename + "|!|" + string.Join(",", h.Hash)).ToArray();
            File.WriteAllLines(filename, lines);
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return _hashes.GetEnumerator();
        }
        public static double CompareHashes(int[] hash1, int[] hash2) {
            int[] difs = hash1.Select((h, i) => Math.Abs(h - hash2[i])).ToArray();
            double dif = 1 - difs.Sum() / (double) (hash1.Length * 255);
            return dif * 100;
        }
        public static HashCollection Load(string filename) {
            if (!File.Exists(filename)) return new HashCollection();
            var hashes = new HashCollection();
            var lines = File.ReadAllLines(filename);
            foreach (var line in lines) {
                if (!line.Contains("|!|")) continue;
                int i = line.IndexOf("|!|", StringComparison.Ordinal);
                var parts = new[] {line.Substring(0, i), line.Substring(i + 3)};
                var h = new HashEntry {Filename = parts[0].Trim(), Hash = parts[1].Split(',').Select(int.Parse).ToArray()};
                if (!hashes.Contains(h) && File.Exists(h.Filename)) hashes.Add(h);
            }
            return hashes;
        }
        private readonly List<HashEntry> _hashes = new List<HashEntry>();
    }
}