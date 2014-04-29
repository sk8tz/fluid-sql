﻿// <copyright company="TTRider, L.L.C.">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
namespace TTRider.FluidSql
{
    public class IfStatement : IStatement
    {
        public Token Condition { get; set; }
        public StatementsStatement Then { get; set; }
        public StatementsStatement Else { get; set; }
    }
}