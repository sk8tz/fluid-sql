// <license>
// The MIT License (MIT)
// </license>
// <copyright company="TTRider, L.L.C.">
// Copyright (c) 2014-2015 All Rights Reserved
// </copyright>

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TTRider.FluidSql
{
    public class Name : AliasedToken
                      , IList<string>
    {
        private static readonly Regex ParseName = new Regex(@"(\[(?<name>[^\]]*)]\.?)|(\`(?<name>[^\`]*)`\.?)|(\""(?<name>[^\""]*)""\.?)|((?<name>[^\.]*)\.?)", RegexOptions.Compiled);

        private readonly List<string> parts = new List<string>();

        static IEnumerable<string> GetParts(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                yield return string.Empty;
                yield break;
            }
            var match = ParseName.Match(name);
            while (match.Success)
            {
                if (match.Length > 0)
                {
                    yield return match.Groups["name"].Value;
                }
                match = match.NextMatch();
            }
        }

        public Name()
        {
        }


        public Name(params string[] names)
        {
            this.Add(names);
        }

        public Name(string name1, Name name2)
        {
            this.Add(name1, name2);
        }
        public Name(string name1, string name2, Name name3)
        {
            this.Add(name1, name2, name3);
        }
        public Name(string name1, string name2, string name3, Name name4)
        {
            this.Add(name1, name2, name3, name4);
        }
        public Name(Name name, params string[] names)
        {
            this.Add(name, names);
        }

        public Name(IEnumerable<string> names)
        {
            this.Add(names);
        }

        public string LastPart
        {
            get { return this.parts.LastOrDefault(); }
        }
        public string FirstPart
        {
            get { return this.parts.FirstOrDefault(); }
        }


        public string GetFullName(string openQuote = null, string closeQuote = null)
        {
            openQuote = string.IsNullOrWhiteSpace(openQuote) ? "\"" : openQuote.Trim();
            closeQuote = string.IsNullOrWhiteSpace(closeQuote) ? openQuote : closeQuote.Trim();

            return string.Join(".",
                this.parts
                    .Select(
                        item =>
                            string.IsNullOrWhiteSpace(item) ||
                            string.Equals(item, "*") ||
                            item.TrimStart().StartsWith("@")
                                ? item
                                : openQuote + item + closeQuote));
        }

        public static implicit operator Name(string value)
        {
            return new Name(value);
        }

        #region IList

        public IEnumerator<string> GetEnumerator()
        {
            return this.parts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string item)
        {
            this.parts.Add(item);
        }

        public void Add(params string[] names)
        {
            this.parts.AddRange(names.SelectMany(GetParts));
        }

        public void Add(string name1, Name name2)
        {
            this.parts.Add(name1);
            this.parts.AddRange(name2.parts);
        }
        public void Add(string name1, string name2, Name name3)
        {
            this.parts.Add(name1);
            this.parts.Add(name2);
            this.parts.AddRange(name3.parts);
        }
        public void Add(string name1, string name2, string name3, Name name4)
        {
            this.parts.Add(name1);
            this.parts.Add(name2);
            this.parts.Add(name3);
            this.parts.AddRange(name4.parts);
        }
        public void Add(Name name, params string[] names)
        {
            this.parts.AddRange(name.parts);
            this.parts.AddRange(names.SelectMany(GetParts));
        }

        public void Add(IEnumerable<string> names)
        {
            if (names != null)
            {
                this.parts.AddRange(names.SelectMany(GetParts));
            }
        }

        public void Clear()
        {
            this.parts.Clear();
        }

        public bool Contains(string item)
        {
            return this.parts.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            this.parts.CopyTo(array, arrayIndex);
        }

        public bool Remove(string item)
        {
            return this.parts.Remove(item);
        }

        public int Count
        {
            get { return this.parts.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(string item)
        {
            return this.parts.IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            this.parts.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.parts.RemoveAt(index);
        }

        public string this[int index]
        {
            get { return this.parts[index]; }
            set { this.parts[index] = value; }
        }

        #endregion IList
    }
}