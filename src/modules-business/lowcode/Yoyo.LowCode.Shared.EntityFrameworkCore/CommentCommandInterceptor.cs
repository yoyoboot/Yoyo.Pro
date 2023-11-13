// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Yoyo.LowCode
{
    public class CommentCommandInterceptor : DbCommandInterceptor
    {
        #region NonQueryExecuting

        public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
        {
            ReplaceCommandTextCommentN(command);
            return base.NonQueryExecuting(command, eventData, result);
        }

        public override ValueTask<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            ReplaceCommandTextCommentN(command);
            return base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
        }

        #endregion NonQueryExecuting

        public static void ReplaceCommandTextCommentN(DbCommand command)
        {
            if (command.CommandText.Contains("is N'"))
            {
                command.CommandText = command.CommandText.Replace("is N'", "is '");
            }
        }
    }
}
