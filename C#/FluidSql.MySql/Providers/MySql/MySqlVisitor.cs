﻿// <license>
// The MIT License (MIT)
// </license>
// <copyright company="TTRider, L.L.C.">
// Copyright (c) 2014-2015 All Rights Reserved
// </copyright>
using System;
// ReSharper disable InconsistentNaming


namespace TTRider.FluidSql.Providers.MySql
{
    internal partial class MySqlVisitor : Visitor
    {
		private static readonly string[] supportedDialects = new [] {"mysql", "ansi"};

        private readonly string[] DbTypeStrings =
        {
            "BIGINT", // BigInt = 0,
            "BINARY", // Binary = 1,
            "BIT", // Bit = 2,
            "CHAR", // Char = 3,
            "DATETIME", // DateTime = 4,
            "DECIMAL", // Decimal = 5,
            "FLOAT", // Float = 6,
            "LONGBLOB", // Image = 7,
            "INTEGER", // Int = 8,
            "DECIMAL", // Money = 9,
            "CHAR", // NChar = 10,
            "TEXT", // NText = 11,
            "VARCHAR", // NVarChar = 12,
            "REAL", // Real = 13,
            "CHAR ( 38 )", // UniqueIdentifier = 14,
            "DATETIME", // SmallDateTime = 15,
            "SMALLINT", // SmallInt = 16,
            "DECIMAL", // SmallMoney = 17,
            "TEXT", // Text = 18,
            "TIMESTAMP", // Timestamp = 19,
            "TINYINT", // TinyInt = 20,
            "VARBINARY", // VarBinary = 21,
            "VARCHAR", // VarChar = 22,
            "BLOB", // Variant = 23,
            "LONGTEXT", // Xml = 24,
            "DATE", // Date = 25,
            "TIME", // Time = 26,
            "DATETIME", // DateTime2 = 27,
            "DATETIME" // DateTimeOffset = 28,
        };

        protected override void VisitNowFunctionToken(NowFunctionToken token)
        {
            throw new NotImplementedException();
        }

        protected override void VisitUuidFunctionToken(UuidFunctionToken token)
        {
            throw new NotImplementedException();
        }

        protected override void VisitIIFFunctionToken(IifFunctionToken token)
        {
            throw new NotImplementedException();
        }

        protected override void VisitDatePartFunctionToken(DatePartFunctionToken token)
        {
            throw new NotImplementedException();
        }

        protected override void VisitDateAddFunctionToken(DateAddFunctionToken token)
        {
            throw new NotImplementedException();
        }

        protected override void VisitDurationFunctionToken(DurationFunctionToken token)
        {
            throw new NotImplementedException();
        }

        protected override void VisitMakeDateFunctionToken(MakeDateFunctionToken token)
        {
            throw new NotImplementedException();
        }

        protected override void VisitMakeTimeFunctionToken(MakeTimeFunctionToken token)
        {
            throw new NotImplementedException();
        }

        protected override string[] SupportedDialects { get { return supportedDialects;}}

        public MySqlVisitor()
        {
            this.IdentifierOpenQuote = "`";
            this.IdentifierCloseQuote = "`";
            this.LiteralOpenQuote = "'";
            this.LiteralCloseQuote = "'";
            this.CommentOpenQuote = "/*";
            this.CommentCloseQuote = "*/";            
        }

        

        protected override void VisitJoinType(Joins join)
        {
            State.Write(JoinStrings[(int)join]);
        }


         void VisitType(TypedToken typedToken)
        {
            if (typedToken.DbType.HasValue)
            {
                State.Write(DbTypeStrings[(int)typedToken.DbType]);
            }
        }

        protected class MySqlSymbols : Symbols
        {
            public const string AUTO_INCREMENT = "AUTO_INCREMENT";
        }
    
    }
}