using System;
using System.Collections.Generic;

namespace iTunesController {
    public class ItemList {
        private object _lockObj = new object();
        private Dictionary<int, ItemInfo> _itemInfos = new Dictionary<int, ItemInfo>();
        public bool Exists ( string name ) {
            lock (_lockObj) {
                return _itemInfos.ContainsKey(name.GetHashCode());
            }
        }
        public ItemInfo Get ( string name ) {
            lock (_lockObj) {
                int hash = name.GetHashCode();
                return _itemInfos.ContainsKey(hash) ? _itemInfos[hash] : null;
            }
        }
        public void Add ( ItemInfo iteminfo ) {
            lock (_lockObj) {
                int hash = iteminfo.Name.GetHashCode();
                if (_itemInfos.ContainsKey(hash)) throw new DuplicateItemException(iteminfo);
                _itemInfos.Add(hash, iteminfo);
            }
        }
    }
    public class DuplicateItemException : Exception {
        public DuplicateItemException ( ItemInfo artistinfo ) {
            ItemInfo = artistinfo;
        }
        public ItemInfo ItemInfo { get; private set; }
    }
}