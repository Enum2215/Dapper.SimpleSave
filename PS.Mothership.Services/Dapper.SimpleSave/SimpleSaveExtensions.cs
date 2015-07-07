﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper.SimpleSave.Impl;

namespace Dapper.SimpleSave
{
    public static class SimpleSaveExtensions
    {
        private static readonly DtoMetadataCache _dtoMetadataCache = new DtoMetadataCache();

        public static void Update<T>(
            this IDbConnection connection,
            T oldObject,
            T newObject,
            IDbTransaction transaction = null)
        {
            var builder = new TransactionBuilder(_dtoMetadataCache);
            var scripts = builder.BuildUpdateScripts(oldObject, newObject);

            if (transaction == null)
            {
                using (var myTransaction = connection.BeginTransaction())
                {
                    try
                    {
                        ExecuteScripts<T>(connection, scripts, myTransaction);
                        myTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        myTransaction.Rollback();
                        throw;
                    }
                }
            }
            else
            {
                ExecuteScripts<T>(connection, scripts, transaction);
            }
        }

        private static void ExecuteScripts<T>(IDbConnection connection, IList<Script> scripts, IDbTransaction transaction)
        {
            for (int index = 0, count = scripts.Count; index < count; ++index)
            {
                var script = scripts[index];

                ResolvePrimaryKeyValues<T>(script);

                ExecuteCommandForScript<T>(
                    connection,
                    transaction,
                    script);
            }
        }

        public static void Create<T>(
            this IDbConnection connection,
            T obj,
            IDbTransaction transaction = null)
        {
            Update(connection, default(T), obj, transaction);
        }

        public static void Delete<T>(
            this IDbConnection connection,
            T obj,
            IDbTransaction transaction = null)
        {
            Update(connection, obj, default(T), transaction);
        }

        private static void ExecuteCommandForScript<T>(
            IDbConnection connection,
            IDbTransaction transaction,
            Script script)
        {
            var commandDefinition = new CommandDefinition(
                script.Buffer.ToString(),
                script.Parameters,
                transaction,
                30,
                CommandType.Text,
                CommandFlags.Buffered | CommandFlags.NoCache);

            var insertedPk = connection.ExecuteScalar(commandDefinition);
            if (null != insertedPk
                && null != script.InsertedValue)
            {
                //  Allows primary key of INSERTed row to be resolved
                //  in subsequent scripts.
                SetPrimaryKeyForInsertedRowOnCorrespondingObject(
                    script,
                    insertedPk);
            }
        }

        private static void SetPrimaryKeyForInsertedRowOnCorrespondingObject(
            Script script,
            object insertedPk)
        {
            var metadata = script.InsertedValueMetadata;
            var type = metadata.PrimaryKey.Prop.PropertyType;
            if (type == typeof(int?) || type == typeof(int))
            {
                metadata.SetPrimaryKey(
                    script.InsertedValue,
                    Decimal.ToInt32((decimal) insertedPk));
            }
            else if (type == typeof (long?) || type == typeof (long))
            {
                metadata.SetPrimaryKey(
                    script.InsertedValue,
                    Decimal.ToInt64((decimal) insertedPk));
            }
            else
            {
                metadata.SetPrimaryKey(
                    script.InsertedValue,
                    insertedPk);
            }
        }

        private static void ResolvePrimaryKeyValues<T>(Script script)
        {
            // ToArray() dodges exception due to concurrent modification
            foreach (string key in script.Parameters.Keys.ToArray())
            {
                var value = script.Parameters [key];
                if (value is Func<object>)
                {
                    script.Parameters [key] = (value as Func<object>)();
                }
            }
        }
    }
}
