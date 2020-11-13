using System;
using System.IO;

namespace Common.IO
{
    public enum TransferType : byte
    {
        Upload = 1,
        Download
    }

    public class FileTransfer : IDisposable
    {
        public string LocalFilePath { get; private set; }
        public string RemoteFilePath { get; private set; }
        public string TransferId { get; private set; }
        public long FileLength { get; private set; }
        public TransferType TransferType { get; private set; }
        public bool Paused { get; private set; }
        public bool Transfering => FileLength > 0;
        public long BytesPerSecond { get; private set; } = 0;
        private long _lastTick = 0;
        private long _perSecondDelta = 0;
        private FileStream _fileStream;
        private byte[] _buffer;
        private bool _disposed = false;
        private long _fullLength;

        public FileTransfer(string local, string remote, long bufferSize)
        {
            // Sending file
            TransferType = TransferType.Upload;
            LocalFilePath = local;
            RemoteFilePath = remote;

            TransferId = Guid.NewGuid().ToString();
            _fileStream = new FileStream(Explorer.FixPath(LocalFilePath), FileMode.Open, FileAccess.Read, FileShare.Read);
            FileLength = _fileStream.Length;
            _fullLength = FileLength;
            _buffer = new byte[bufferSize];
        }

        public FileTransfer(string local, string remote, long length, long buffer)
        {
            // Receive file 
            TransferType = TransferType.Download;
            LocalFilePath = local;
            RemoteFilePath = remote;

            TransferId = Guid.NewGuid().ToString();
            _fileStream = new FileStream(Explorer.FixPath(LocalFilePath), FileMode.Create, FileAccess.Write, FileShare.None);
            FileLength = length;
            _fullLength = length;
            _buffer = new byte[buffer];
        }

        public void WriteChunk(byte[] chunk)
        {
            if (chunk.Length == 0 || _fileStream == null) return;

            _fileStream.Write(chunk, 0, chunk.Length);
            _fileStream.Flush();

            FileLength -= chunk.Length;

            if (FileLength == 0)
            {
                Finish();
            }

            UpdateSpeed(chunk.Length);
        }

        public byte[] NextChunk()
        {
            if (_fileStream == null) return new byte[] { };

            int read = _fileStream.Read(_buffer, 0, _buffer.Length);

            if (read < _buffer.Length)
            {
                Array.Resize(ref _buffer, read);
            }

            FileLength -= read;

            if (FileLength == 0)
            {
                Finish();
            }

            UpdateSpeed(read);

            return _buffer;
        }

        public void Finish()
        {
            if (_fileStream != null)
            {
                _fileStream.Close();
                _fileStream.Dispose();
            }
        }

        public void Cancel()
        {
            Finish();

            if (TransferType == TransferType.Download)
            {
                File.Delete(LocalFilePath);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                if (_fileStream != null)
                {
                    _fileStream.Dispose();
                    _fileStream = null;
                }

                _buffer = null;
            }
        }

        private void UpdateSpeed(int amount)
        {
            long now = Environment.TickCount;

            if (now - _lastTick >= 1000)
            {
                BytesPerSecond = _perSecondDelta;
                _perSecondDelta = amount;
                _lastTick = now;
            }
            else
            {
                _perSecondDelta += amount;
            }
        }

        public int CalculatePercentage()
        {
            double max = _fullLength;
            double current = FileLength;

            return 100 - (int)(current / max * 100);
        }

        public long Transferred()
        {
            return _fullLength - FileLength;
        }

        public void TogglePause()
        {
            Paused = !Paused;
        }

        public override string ToString()
        {
            string transferType = $"TransferType={TransferType}";
            string local = $"Local Path={LocalFilePath}";
            string remote = $"Remote Path={RemoteFilePath}";
            string length = $"File Length={FileLength}";
            return $"FileTransfer {{ {transferType}, {local}, {remote}, {length} }}";
        }

    }
}
