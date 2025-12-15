using System.IO;

namespace BaconBinary.Core.IO.Spr
{
    public class SprFile : IDisposable
    {
        private FileStream _fileStream;
        private BinaryReader _reader;
        private readonly object _lock = new object();
        
        private long[] _offsets;

        public int SpriteCount { get; private set; }

        public SprFile(string filePath)
        {
            _fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            _reader = new BinaryReader(_fileStream);
        }

        public void SetOffsets(long[] offsets, int count)
        {
            _offsets = offsets;
            SpriteCount = count;
        }
        
        public byte[] GetSpritePixels(int spriteId)
        {
            if (spriteId <= 0 || spriteId >= _offsets.Length)
                return null;

            long address = _offsets[spriteId];
            
            if (address == 0) return null;

            lock (_lock)
            {
                _fileStream.Seek(address, SeekOrigin.Begin);
                return null; 
            }
        }
        
        public BinaryReader GetReaderAt(int spriteId)
        {
             if (spriteId <= 0 || spriteId > SpriteCount) return null;
             long address = _offsets[spriteId];
             if (address == 0) return null;

             lock (_lock)
             {
                 _fileStream.Seek(address, SeekOrigin.Begin);
                 return _reader;
             }
        }

        public void Dispose()
        {
            _reader?.Close();
            _fileStream?.Dispose();
        }
        
        public Stream GetStreamForHeaderReading()
        {
            _fileStream.Seek(0, SeekOrigin.Begin);
            return _fileStream;
        }
    }
}