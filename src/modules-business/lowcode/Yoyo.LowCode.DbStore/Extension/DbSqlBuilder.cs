using System;
using System.Collections;
using System.Text;
using DatabaseType = SqlSugar.DbType;

namespace Yoyo.LowCode.Extension
{
    /// <summary>
    /// 拼接SQL语句
    /// </summary>
    public static class DbSqlBuilder
    {
        /// <summary>
        /// 生成Insert语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">Hashtable</param>
        /// <returns>返回sql</returns>
        public static StringBuilder InsertSql(string tableName, Hashtable ht)
        {
            var sb = new StringBuilder();
            sb.Append(" Insert Into ");
            sb.Append(tableName);
            sb.Append("(");
            var sp = new StringBuilder();
            var sbPregame = new StringBuilder();
            foreach (string key in ht.Keys)
            {
                if (ht[key] != null)
                {
                    sbPregame.Append("," + key);
                    sp.Append(",@" + key);
                }
            }
            sb.Append(sbPregame.ToString().Substring(1, sbPregame.ToString().Length - 1) + ") Values (");
            sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
            return sb;
        }

        /// <summary>
        /// 生成UpdateSql语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="pkName">主键</param>
        /// <returns></returns>
        public static StringBuilder UpdateSql(string tableName, Hashtable ht, string pkName)
        {
            var sb = new StringBuilder();
            sb.Append(" Update ");
            sb.Append(tableName);
            sb.Append(" Set ");
            var isFirstValue = true;
            foreach (string key in ht.Keys)
            {
                if (ht[key] != null && pkName != key)
                {
                    if (isFirstValue)
                    {
                        isFirstValue = false;
                        sb.Append(key);
                        sb.Append("=");
                        sb.Append("@" + key);
                    }
                    else
                    {
                        sb.Append("," + key);
                        sb.Append("=");
                        sb.Append("@" + key);
                    }
                }
            }
            sb.Append(" Where ").Append(pkName).Append("=").Append("@" + pkName);
            return sb;
        }

        /// <summary>
        /// 生成Delete语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>返回sql</returns>
        public static StringBuilder DeleteSql(string tableName)
        {
            var sb = new StringBuilder();
            sb.Append($"DELETE FROM {tableName} ");
            return sb;
        }

        /// <summary>
        /// 数据库表SQL
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static string DBTableSql(DatabaseType dbType)
        {
            var sb = new StringBuilder();
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    sb.Append(@"DECLARE @TABLEINFO TABLE ( NAME VARCHAR(50) , SUMROWS VARCHAR(11) , RESERVED VARCHAR(50) , DATA VARCHAR(50) , INDEX_SIZE VARCHAR(50) , UNUSED VARCHAR(50) , PK VARCHAR(50) ) DECLARE @TABLENAME TABLE ( NAME VARCHAR(50) ) DECLARE @NAME VARCHAR(50) DECLARE @PK VARCHAR(50) INSERT INTO @TABLENAME ( NAME ) SELECT O.NAME FROM SYSOBJECTS O , SYSINDEXES I WHERE O.ID = I.ID AND O.XTYPE = 'U' AND O.UID =1 AND I.INDID < 2 ORDER BY I.ROWS DESC , O.NAME WHILE EXISTS ( SELECT 1 FROM @TABLENAME ) BEGIN SELECT TOP 1 @NAME = NAME FROM @TABLENAME DELETE @TABLENAME WHERE NAME = @NAME DECLARE @OBJECTID INT SET @OBJECTID = OBJECT_ID(@NAME) SELECT @PK = COL_NAME(@OBJECTID, COLID) FROM SYSOBJECTS AS O INNER JOIN SYSINDEXES AS I ON I.NAME = O.NAME INNER JOIN SYSINDEXKEYS AS K ON K.INDID = I.INDID WHERE O.XTYPE = 'PK' AND PARENT_OBJ = @OBJECTID AND K.ID = @OBJECTID INSERT INTO @TABLEINFO ( NAME , SUMROWS , RESERVED , DATA , INDEX_SIZE , UNUSED ) EXEC SYS.SP_SPACEUSED @NAME UPDATE @TABLEINFO SET PK = @PK WHERE NAME = @NAME END SELECT F.NAME AS F_TABLE,ISNULL(P.TDESCRIPTION,' ') AS F_TABLENAME, F.RESERVED AS F_SIZE, RTRIM(F.SUMROWS) AS F_SUM, F.PK AS F_PRIMARYKEY FROM @TABLEINFO F LEFT JOIN ( SELECT NAME = CASE WHEN A.COLORDER = 1 THEN D.NAME ELSE '' END , TDESCRIPTION = CASE WHEN A.COLORDER = 1 THEN ISNULL(F.VALUE, '') ELSE '' END FROM SYSCOLUMNS A LEFT JOIN SYSTYPES B ON A.XUSERTYPE = B.XUSERTYPE INNER JOIN SYSOBJECTS D ON A.ID = D.ID AND D.XTYPE = 'U' AND D.NAME <> 'DTPROPERTIES' LEFT JOIN SYS.EXTENDED_PROPERTIES F ON D.ID = F.MAJOR_ID WHERE A.COLORDER = 1 AND F.MINOR_ID = 0 ) P ON F.NAME = P.NAME WHERE 1 = 1 ORDER BY F_TABLE");
                    break;

                case DatabaseType.Oracle:
                    sb.Append(@"SELECT DISTINCT COL.TABLE_NAME AS F_TABLE,TAB.COMMENTS AS F_TABLENAME,0 AS F_SIZE,NVL(T.NUM_ROWS,0)AS F_SUM,COLUMN_NAME AS F_PRIMARYKEY FROM USER_CONS_COLUMNS COL INNER JOIN USER_CONSTRAINTS CON ON CON.CONSTRAINT_NAME=COL.CONSTRAINT_NAME INNER JOIN USER_TAB_COMMENTS TAB ON TAB.TABLE_NAME=COL.TABLE_NAME INNER JOIN USER_TABLES T ON T.TABLE_NAME=COL.TABLE_NAME WHERE CON.CONSTRAINT_TYPE NOT IN('C','R')ORDER BY COL.TABLE_NAME");
                    break;

                case DatabaseType.MySql:
                    sb.Append(@"SELECT T1.*,(SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.`COLUMNS`WHERE TABLE_SCHEMA=DATABASE()AND TABLE_NAME=T1.F_TABLE AND COLUMN_KEY='PRI')F_PRIMARYKEY FROM(SELECT TABLE_NAME F_TABLE,0 F_SIZE,TABLE_ROWS F_SUM,(SELECT IF(LENGTH(TRIM(TABLE_COMMENT))<1,TABLE_NAME,TABLE_COMMENT))F_TABLENAME FROM INFORMATION_SCHEMA.`TABLES`WHERE TABLE_SCHEMA=DATABASE())T1 ORDER BY T1.F_TABLE");
                    break;

                default:
                    throw new Exception("暂不支持此数据库");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 数据库表字段SQL
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static string DBTableField(DatabaseType dbType)
        {
            var sb = new StringBuilder();
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    sb.Append(@"SELECT [F_FIELD] =A.NAME , [F_DATATYPE]=B.NAME, [F_DATALENGTH]=COLUMNPROPERTY(A.ID,A.NAME,'PRECISION'), [F_IDENTITY]=CASE WHEN COLUMNPROPERTY( A.ID,A.NAME,'ISIDENTITY')=1 THEN '√'ELSE '' END, [F_PRIMARYKEY]=CASE WHEN EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE='PK' AND PARENT_OBJ=A.ID AND NAME IN ( SELECT NAME FROM SYSINDEXES WHERE INDID IN( SELECT INDID FROM SYSINDEXKEYS WHERE ID = A.ID AND COLID=A.COLID ))) THEN '1' ELSE '0' END, [F_ALLOWNULL]=CASE WHEN A.ISNULLABLE=1 THEN '1'ELSE '0' END, [F_DEFAULTS]=ISNULL(E.TEXT,''), [F_FIELDNAME]=ISNULL(G.[VALUE],A.NAME) FROM SYSCOLUMNS A LEFT JOIN SYSTYPES B ON A.XUSERTYPE=B.XUSERTYPE INNER JOIN SYSOBJECTS D ON A.ID=D.ID AND D.XTYPE='U' AND D.NAME<>'DTPROPERTIES' LEFT JOIN SYSCOMMENTS E ON A.CDEFAULT=E.ID LEFT JOIN SYS.EXTENDED_PROPERTIES G ON A.ID=G.MAJOR_ID AND A.COLID=G.MINOR_ID LEFT JOIN SYS.EXTENDED_PROPERTIES F ON D.ID=F.MAJOR_ID AND F.MINOR_ID=0 WHERE D.NAME='{0}' ORDER BY A.ID,A.COLORDER");
                    break;

                case DatabaseType.Oracle:
                    sb.Append(@"SELECT COL.COLUMN_NAME F_FIELD, COL.DATA_TYPE F_DATATYPE, COL.DATA_LENGTH F_DATALENGTH, NULL F_IDENTITY, CASE UC.CONSTRAINT_TYPE WHEN 'P' THEN 1 ELSE NULL END F_PRIMARYKEY, CASE COL.NULLABLE WHEN 'N' THEN 0 ELSE 1 END F_ALLOWNULL, COL.DATA_DEFAULT F_DEFAULTS, COMM.COMMENTS AS F_FIELDNAME FROM USER_TAB_COLUMNS COL INNER JOIN USER_COL_COMMENTS COMM ON COMM.TABLE_NAME = COL.TABLE_NAME AND COMM.COLUMN_NAME = COL.COLUMN_NAME LEFT JOIN USER_CONS_COLUMNS UCC ON UCC.TABLE_NAME = COL.TABLE_NAME AND UCC.COLUMN_NAME = COL.COLUMN_NAME AND UCC.POSITION=1 LEFT JOIN USER_CONSTRAINTS UC ON UC.CONSTRAINT_NAME = UCC.CONSTRAINT_NAME AND UC.CONSTRAINT_TYPE = 'P' WHERE COL.TABLE_NAME = '{0}' ORDER BY COL.COLUMN_ID");
                    break;

                case DatabaseType.MySql:
                    sb.Append(@"SELECT ORDINAL_POSITION F_NUMBER,COLUMN_NAME F_FIELD,DATA_TYPE F_DATATYPE,IF(CHARACTER_MAXIMUM_LENGTH IS NULL,IF(LOCATE('INT',COLUMN_TYPE)>0,REPLACE(REPLACE(COLUMN_TYPE,'INT(',''),')',''),0),CHARACTER_MAXIMUM_LENGTH)F_DATALENGTH,''F_IDENTITY,IF(COLUMN_KEY='PRI','1','')F_PRIMARYKEY,IF(IS_NULLABLE='YES','1','')F_ALLOWNULL,COLUMN_DEFAULT F_DEFAULTS,CASE WHEN COLUMN_COMMENT=''THEN COLUMN_NAME ELSE COLUMN_COMMENT END F_FIELDNAME FROM(SELECT*FROM INFORMATION_SCHEMA.`COLUMNS`T1 WHERE TABLE_SCHEMA=DATABASE()AND TABLE_NAME='{0}')T2 ORDER BY F_NUMBER");
                    break;

                default:
                    throw new Exception("不支持");
            }
            return sb.ToString();
        }
    }
}
