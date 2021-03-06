﻿// <copyright company="TTRider, L.L.C.">
//     Copyright (c) 2014-2016 All Rights Reserved
// </copyright>

using TTRider.FluidSql;
using TTRider.FluidSql.Providers.Sqlite;
using Xunit;

namespace xUnit.Sqlite
{
    public class SelectTest
    {
        public static SqliteProvider Provider = new SqliteProvider();

        [Fact]
        public IStatement Select1()
        {
            var statement = Sql.Select.Output(Sql.Scalar(1));

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT 1;", text);
            return statement;
        }

        [Fact]
        public IStatement SelectStarFromSysObjects()
        {
            var statement = Sql.Select.From("sys.objects");

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT * FROM \"sys\".\"objects\";", text);
            return statement;
        }

        [Fact]
        public IStatement SelectSkipSchema()
        {
            var statement = Sql.Select.From(Sql.Name("DB", null, "objects"));

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT * FROM \"DB\"..\"objects\";", text);
            return statement;
        }

        [Fact]
        public IStatement SelectSkipDbAndSchema()
        {
            var statement = Sql.Select.From(Sql.Name("server", null, null, "objects"));

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT * FROM \"server\"...\"objects\";", text);
            return statement;
        }

        [Fact]
        public IStatement SelectStarFromSysObjectsInto()
        {
            var statement = Sql.Select.From("sys.objects").Into(Sql.Name("#mytable"));

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("CREATE TABLE \"#mytable\" AS SELECT * FROM \"sys\".\"objects\";", text);
            return statement;
        }

        [Fact]
        public IStatement SelectCountStarFromSysObjects()
        {
            var statement = Sql.Select.Output(Sql.Function("COUNT", Sql.Star())).From("sys.objects");

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT COUNT( * ) FROM \"sys\".\"objects\";", text);
            return statement;
        }

        [Fact]
        public IStatement SelectFoo()
        {
            var statement = Sql.Select.Output(Sql.Scalar("Foo1"));

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT 'Foo1';", text);
            return statement;
        }

        [Fact]
        public IStatement SelectParam1()
        {
            var statement = Sql.Select.Output(Parameter.Any("@param1"));

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT @param1;", text);

            //command.Parameters.SetValue("@param1", "Foo");

            //var result = command.ExecuteScalar();
            //Assert.Equal(result, "Foo");
            return statement;
        }

        [Fact]
        public IStatement SelectParam1WithDefault()
        {
            var statement = Sql.Select.Output(Parameter.Any("@param1").DefaultValue("Foo"));

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT @param1;", text);
            return statement;

            //var result = command.ExecuteScalar();
            //Assert.Equal(result, "Foo");
        }

        [Fact]
        public IStatement SelectParam1WithDefault2()
        {
            var statement = Sql.Select.Output(Parameter.Any("@param1", "Foo"));

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT @param1;", text);
            return statement;

            //var result = command.ExecuteScalar();
            //Assert.Equal(result, "Foo");
        }

        [Fact]
        public IStatement SelectParam1As()
        {
            var statement = Sql.Select.Output(Parameter.Any("@param1").As("p1"));

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT @param1 AS \"p1\";", text);
            return statement;

            //command.Parameters.SetValue("@param1", "Foo");

            //var result = command.ExecuteScalar();
            //Assert.Equal(result, "Foo");
        }

        [Fact]
        public IStatement Select1Top1()
        {
            var statement = Sql.Select.Output(Sql.Scalar(1).As("foo")).Top(1);

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT 1 AS \"foo\" LIMIT 1;", text);
            return statement;
        }

        [Fact]
        public IStatement Select1Top1Param()
        {
            var statement = Sql.Select.Output(Sql.Scalar(1).As("foo")).Top(1);

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT 1 AS \"foo\" LIMIT 1;", text);
            return statement;
        }

        [Fact]
        public IStatement SelectStar()
        {
            var statement = Sql.Select.Output(Sql.Star());

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT *;", text);
            return statement;
        }

        [Fact]
        public IStatement SelectStarPlus()
        {
            var statement = Sql.Select.Output(Sql.Star()).Distinct();

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT DISTINCT *;", text);
            return statement;
        }

        [Fact]
        public IStatement SelectStarPlusPlus()
        {
            var statement = Sql.Select.Output(Sql.Star("src")).From("sys.objects", "src");

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT \"src\".* FROM \"sys\".\"objects\" AS \"src\";", text);
            return statement;
        }

        [Fact]
        public IStatement SelectGroupBy()
        {
            var statement =
                Sql.Select.Output(Sql.Name("src", "object_id"))
                    .From("sys.objects", "src")
                    .GroupBy(Sql.Name("src", "object_id"), Sql.Name("src", "name"));

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal(
                "SELECT \"src\".\"object_id\" FROM \"sys\".\"objects\" AS \"src\" GROUP BY \"src\".\"object_id\", \"src\".\"name\";",
                text);
            return statement;
        }

        [Fact]
        public IStatement SelectGroupByHaving()
        {
            var statement =
                Sql.Select.Output(Sql.Name("src", "object_id"))
                    .From("sys.objects", "src")
                    .GroupBy(Sql.Name("src", "object_id"), Sql.Name("src", "name"))
                    .Having(Sql.Name("src", "object_id").IsNull());

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal(
                "SELECT \"src\".\"object_id\" FROM \"sys\".\"objects\" AS \"src\" GROUP BY \"src\".\"object_id\", \"src\".\"name\" HAVING \"src\".\"object_id\" IS NULL;",
                text);
            return statement;
        }

        [Fact]
        public IStatement SelectOrderBy()
        {
            var statement =
                Sql.Select.Output(Sql.Name("src.object_id"))
                    .From("sys.objects", "src")
                    .OrderBy(Sql.Name("src", "object_id"), Direction.Asc)
                    .OrderBy(Sql.Name("src", "name"), Direction.Desc);

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal(
                "SELECT \"src\".\"object_id\" FROM \"sys\".\"objects\" AS \"src\" ORDER BY \"src\".\"object_id\" ASC, \"src\".\"name\" DESC;",
                text);
            return statement;
        }

        [Fact]
        public IStatement SelectUnionSelect()
        {
            var statement =
                Sql.Select.Output(Sql.Star())
                    .From("sys.objects")
                    .Union(Sql.Select.Output(Sql.Star()).From("sys.objects"));

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT * FROM \"sys\".\"objects\" UNION SELECT * FROM \"sys\".\"objects\";", text);
            return statement;
        }

        [Fact]
        public IStatement SelectUnionSelectWrapped()
        {
            var statement =
                Sql.Select.Output(Sql.Star())
                    .From("sys.objects")
                    .Union(Sql.Select.Output(Sql.Star()).From("sys.objects"))
                    .WrapAsSelect("wrap");

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal(
                "SELECT \"wrap\".* FROM ( SELECT * FROM \"sys\".\"objects\" UNION SELECT * FROM \"sys\".\"objects\" ) AS \"wrap\";",
                text);
            return statement;
        }

        [Fact]
        public IStatement SelectWrapped2()
        {
            var statement = Sql.Select.Output(Sql.Name("Name")).From("sys.objects").WrapAsSelect("wrap");

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT \"wrap\".\"Name\" FROM ( SELECT \"Name\" FROM \"sys\".\"objects\" ) AS \"wrap\";",
                text);
            return statement;
        }

        [Fact]
        public IStatement SelectWrapped3()
        {
            var statement = Sql.Select.Output(Sql.Name("Name").As("nm")).From("sys.objects").WrapAsSelect("wrap");

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal(
                "SELECT \"wrap\".\"nm\" FROM ( SELECT \"Name\" AS \"nm\" FROM \"sys\".\"objects\" ) AS \"wrap\";", text);
            return statement;
        }

        [Fact]
        public IStatement SelectUnionAllSelect()
        {
            var statement =
                Sql.Select.Output(Sql.Star())
                    .From("sys.objects")
                    .UnionAll(Sql.Select.Output(Sql.Star()).From("sys.objects"));

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT * FROM \"sys\".\"objects\" UNION ALL SELECT * FROM \"sys\".\"objects\";", text);
            return statement;
        }

        [Fact]
        public IStatement SelectExceptSelect()
        {
            var statement =
                Sql.Select.Output(Sql.Star())
                    .From("sys.objects")
                    .Except(Sql.Select.Output(Sql.Star()).From("sys.objects"));

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("SELECT * FROM \"sys\".\"objects\" EXCEPT SELECT * FROM \"sys\".\"objects\";", text);
            return statement;
        }

        [Fact]
        public IStatement SelectIntersectSelect()
        {
            var statement =
                Sql.Select.Output(Sql.Star("First"))
                    .From("sys.objects", "First")
                    .Intersect(Sql.Select.Output(Sql.Star("Second")).From("sys.objects", "Second"));

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal(
                "SELECT \"First\".* FROM \"sys\".\"objects\" AS \"First\" INTERSECT SELECT \"Second\".* FROM \"sys\".\"objects\" AS \"Second\";",
                text);
            return statement;
        }

        [Fact]
        public IStatement SelectIntersectWrapped()
        {
            var statement =
                Sql.Select.Output(Sql.Star("First"))
                    .From("sys.objects", "First")
                    .Intersect(Sql.Select.Output(Sql.Star("Second")).From("sys.objects", "Second"))
                    .WrapAsSelect("wrap");

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal(
                "SELECT \"wrap\".* FROM ( SELECT \"First\".* FROM \"sys\".\"objects\" AS \"First\" INTERSECT SELECT \"Second\".* FROM \"sys\".\"objects\" AS \"Second\" ) AS \"wrap\";",
                text);
            return statement;
        }

        [Fact]
        public IStatement SelectInnerJoin()
        {
            var statement = Sql.Select
                .Output(Sql.Star())
                .From("sys.objects", "o")
                .InnerJoin(Sql.Name("sys.tables").As("t"), Sql.Name("t.object_id").IsEqual(Sql.Name("o.object_id")));

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal(
                "SELECT * FROM \"sys\".\"objects\" AS \"o\" INNER JOIN \"sys\".\"tables\" AS \"t\" ON \"t\".\"object_id\" = \"o\".\"object_id\";",
                text);
            return statement;
        }

        [Fact]
        public IStatement SelectLike()
        {
            var statement = Sql.Select
                .Output(Sql.Star())
                .From("sys.objects")
                .Where(
                    Sql.Name("name").StartsWith(Sql.Scalar("foo"))
                        .And(
                            Sql.Name("name").EndsWith(Sql.Scalar("foo")))
                        .And(
                            Sql.Name("name").Contains(Sql.Scalar("foo")))
                        .And(
                            Sql.Name("name").Like(Sql.Scalar("%foo%"))));


            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal(
                "SELECT * FROM \"sys\".\"objects\" WHERE \"name\" LIKE 'foo' + '%' AND \"name\" LIKE '%' + 'foo' AND \"name\" LIKE '%' + 'foo' + '%' AND \"name\" LIKE '%foo%';",
                text);
            return statement;
        }

        [Fact]
        public IStatement SelectSupeExpression()
        {
            var statement = Sql.Select
                .Output(Sql.Star())
                .From("sys.objects")
                .Where(
                    Sql.Name("object_id")
                        .IsNotNull()
                        .And(
                            Sql.Name("object_id").IsNull()
                                .And(
                                    Sql.Group(
                                        Sql.Name("object_id")
                                            .Plus(Sql.Scalar(1))
                                            .Minus(Sql.Scalar(1))
                                            .Multiply(Sql.Scalar(1))
                                            .Divide(Sql.Scalar(1))
                                            .Modulo(Sql.Scalar(1))
                                            .BitwiseAnd(Sql.Scalar(1))
                                            .BitwiseOr(Sql.Scalar(1))
                                            .BitwiseXor(Sql.Scalar(1))
                                        ).Less(Sql.Scalar(1)))));


            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal(
                "SELECT * FROM \"sys\".\"objects\" WHERE \"object_id\" IS NOT NULL AND \"object_id\" IS NULL AND ( \"object_id\" + 1 - 1 * 1 / 1 % 1 & 1 | 1 ^ 1 ) < 1;",
                text);
            return statement;
        }

        [Fact]
        public IStatement CommentOutDropTable()
        {
            var statement = Sql.DropTable(Sql.Name("some.table")).CommentOut();

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal("/* DROP TABLE \"some\".\"table\" */", text);
            return statement;
        }

        [Fact]
        public IStatement SelectWithCTE()
        {
            var statement = Sql.With("name", "a").Recursive()
                .As(Sql.Select.From("sys.objects").Output(Sql.Name("object_id").As("a")))
                .With("name2").As(Sql.Select.From("sys.tables"))
                .Select().From("name");

            var text = Provider.GenerateStatement(statement);

            Assert.NotNull(text);
            Assert.Equal(
                "WITH RECURSIVE \"name\" ( \"a\" ) AS ( SELECT \"object_id\" AS \"a\" FROM \"sys\".\"objects\" ), \"name2\" AS ( SELECT * FROM \"sys\".\"tables\" ) SELECT * FROM \"name\";",
                text);
            return statement;
        }
    }
}