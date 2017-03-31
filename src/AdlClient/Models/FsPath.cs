namespace AdlClient.Models
{
    public class FsPath
    {
        string _path;

        public FsPath(string path)
        {
            if (path == null)
            {
                throw new System.ArgumentNullException(nameof(path));
            }

            if (path.Length == 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(path));

            }
            this._path=path;
        }

        public override string ToString()
        {
            return this._path;
        }

        public bool IsRooted
        {
            get { return this._path[0] == '/'; }
        }


        public static FsPath Root
        {
            get { return FsPath.root; }
        }

        private static readonly FsPath root = new FsPath("/");

        public static FsPath Combine(FsPath left, FsPath right)
        {
            if (right.IsRooted)
            {
                throw new System.ArgumentException(nameof(right));
            }


            if (left.EndsWithSeparator)
            {
                var p = new FsPath(left.ToString() + right.ToString());
                return p;

            }
            else
            {
                var p = new FsPath(left.ToString() + "/" + right.ToString());
                return p;
            }

        }

        public FsPath Append(FsPath p)
        {
            return FsPath.Combine(this, p);
        }

        public FsPath Append(string p)
        {
            return FsPath.Combine(this, new FsPath(p));
        }

        public bool EndsWithSeparator
        {
            get { return this._path[this._path.Length - 1] == '/'; }
        }

    }
}