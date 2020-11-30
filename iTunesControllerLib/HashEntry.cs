using System.IO;

namespace iTunesControllerLib {
    public class HashEntry {
        public string FilenameNoPath => Path.GetFileName(Filename);
        public string Filename;
        public int[] Hash;
    }
}