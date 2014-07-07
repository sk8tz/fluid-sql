﻿// <copyright company="TTRider, L.L.C.">
// Copyright (c) 2014 All Rights Reserved
// </copyright>

namespace TTRider.FluidSql
{
    public class DropIndexStatement : IStatement
    {
        public DropIndexStatement()
        {
            this.With = new IndexOptions();
        }

        public Name Name { get; set; }

        public Name On { get; set; }

        public IDropIndexOptions With { get; private set; }
    }
}