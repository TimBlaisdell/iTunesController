using System;
using System.Collections.Generic;
using iTunesLib;

namespace iTunesController {
    public class TrackInfo {
        private static Dictionary<int, string> _artistList = new Dictionary<int, string>();
        private static Dictionary<int, string> _albumList = new Dictionary<int, string>(); 
        public TrackInfo ( IITFileOrCDTrack track ) {
            int hash = track.Artist.GetHashCode();
            if (!_artistList.ContainsKey(hash)) _artistList.Add(hash, track.Artist);
            else if (_artistList[hash] != track.Artist) throw new Exception("Duplicate artist hash.");
            hash = track.Album.GetHashCode();
            if (!_albumList.ContainsKey(hash)) _albumList.Add(hash, track.Album);
            else if (_albumList[hash] != track.Album) throw new Exception("Duplicate album hash.");
            

        }
}}