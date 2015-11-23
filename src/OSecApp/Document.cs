﻿namespace OSecApp
{
    using System;
    using System.Threading;

    public class Document : IEquatable<Document>
    {
        public const string GeneratedNamePrefix = "Document-";
        private static volatile int idx = 0;

        private readonly DocumentOptions opts;
        private readonly string name;

        public Document(DocumentOptions opts)
        {
            this.opts = opts;
            if (this.opts == null)
            {
                throw new ArgumentNullException("opts", Properties.Resources.OptionsAreRequiredException);
            }

            name = MakeName(this.opts);
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public override string ToString()
        {
            return name;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Document);
        }

        public bool Equals(Document other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return GetHashCode() == other.GetHashCode();
        }

        private static string MakeName(DocumentOptions opts)
        {
            string name = opts.Name;

            if (string.IsNullOrWhiteSpace(opts.Name))
            {
                if (opts.File != null)
                {
                    name = opts.File.Name;
                }
                else
                {
                    name = string.Format("{0}{1}", GeneratedNamePrefix, Interlocked.Increment(ref idx));
                }
            }

            return name;
        }
    }
}
