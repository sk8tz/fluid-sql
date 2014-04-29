﻿// <copyright company="TTRider, L.L.C.">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
using System;
using System.Collections.Generic;

namespace TTRider.FluidSql
{
    public class Top
    {
        public int? Value { get; set; }
        public bool Percent { get; set; }
        public bool WithTies { get; set; }


        public Top(int value, bool percent, bool withTies)
        {
            this.Parameters = new List<Parameter>();
            this.Value = value;
            if (this.Value.Value < 1) throw new ArgumentException("value");
            this.Percent = percent;
            this.WithTies = withTies;
        }
        public Top(string value, bool percent, bool withTies)
        {
            this.Parameters = new List<Parameter>();
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("value");
            value = value.Trim();
            if (value.StartsWith("@"))
            {
                this.Parameters.Add(Parameter.Any(value));
            }
            else
            {
                this.Value = int.Parse(value);
                if (this.Value.Value < 1) throw new ArgumentException("value");
            }

            this.Percent = percent;
            this.WithTies = withTies;
        }

        public List<Parameter> Parameters { get; set; }
    }
}
