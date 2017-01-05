﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using IntranetPortal.Data;
using RedQ.UnderwritingService.Models.NewYork;

public partial class UnderwritingEntity : DbContext
{

    public DbSet<Underwriting> Underwritings { get; set; }
    public DbSet<UnderwritingArchived> UnderwritingArchived { get; set; }
    public DbSet<UnderwritingPropertyInfo> UnderwritingPropertyInfos { get; set; }
    public DbSet<UnderwritingDealCosts> UnderwritingDealCosts { get; set; }
    public DbSet<UnderwritingRehabInfo> UnderwritingRehabInfo { get; set; }
    public DbSet<UnderwritingLienInfo> UnderwritingLienInfo { get; set; }
    public DbSet<UnderwritingLienCosts> UnderwritingLienCosts { get; set; }
    public DbSet<UnderwritingRentalInfo> UnderwritingRentalInfo { get; set; }

    // AuditLog From Other Entity
    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public UnderwritingEntity() : base("name=UnderwritingEntity")
    {
        this.Configuration.LazyLoadingEnabled = false;
        this.Configuration.ProxyCreationEnabled = false;
    }

    public int SaveChanges(string userName)
    {
        List<AuditLog> logs = new List<AuditLog>();
        var modifiedEntities = from en in ChangeTracker.Entries()
                               where en.State == EntityState.Added || en.State == EntityState.Deleted || en.State == EntityState.Modified
                               select en;

        foreach (var entry in modifiedEntities)
        {
            logs.AddRange(GetAuditRecordsForChange(entry, userName));
        }

        var result = base.SaveChanges();

        foreach (var log in logs)
        {
                if (log.Entity.State != EntityState.Detached)
                {
                    dynamic key = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(log.Entity.Entity).EntityKey.EntityKeyValues;
                    log.RecordId = key(0).Value;
                }

                this.AuditLogs.Add(log);
        }

        base.SaveChanges();
        return result;
    }

    private List<AuditLog> GetAuditRecordsForChange(DbEntityEntry dbEntry, string userName)
    {
        List<AuditLog> logs = new List<AuditLog>();
        dynamic eventTime = DateTime.Now;
        dynamic entityConfig = GetAuditConfig(dbEntry);

        dynamic log = new AuditLog
        {
            UserName = userName,
            EventDate = eventTime,
            TableName = entityConfig.TableName,
            Entity = dbEntry
        };

        foreach (var prop in entityConfig.Properties)
        {
            log.ColumnName = prop.Key;

            if (dbEntry.State == EntityState.Deleted)
            {
                if (!Enumerable.Contains(dbEntry.OriginalValues.PropertyNames, prop.Key))
                {
                    continue;
                }

                if (dbEntry.OriginalValues.GetValue<object>(prop.Key) == null)
                {
                    continue;
                }

                logs.Add(new AuditLog
                {
                    UserName = userName,
                    EventDate = eventTime,
                    EventType = (int)AuditLog.LogType.Deleted,
                    ColumnName = prop.Key,
                    TableName = entityConfig.TableName,
                    OriginalValue = dbEntry.OriginalValues.GetValue<object>(prop.Key),
                    Entity = dbEntry,
                    RecordId = dbEntry.OriginalValues.GetValue<object>(entityConfig.KeyNames(0))
                });
                continue;
            }

            if (!Enumerable.Contains(dbEntry.CurrentValues.PropertyNames, prop.Key))
            {
                continue;
            }

            dynamic propValue = dbEntry.CurrentValues.GetValue<object>(prop.Key);


            if (dbEntry.State == EntityState.Added)
            {
                if (propValue == null)
                {
                    continue;
                }

                logs.Add(AddLog(AuditLog.LogType.Added, log));
            }

            if (dbEntry.State == EntityState.Modified)
            {
                var originalValue = dbEntry.OriginalValues.GetValue<object>(prop.Key);

                if (Equals(originalValue, propValue))
                {
                    continue;
                }

                if (object.ReferenceEquals(prop.Value, typeof(decimal?)) || object.ReferenceEquals(prop.Value, typeof(decimal)))
                {
                    if (originalValue != null && propValue != null && string.Format("{0:0.00}", Math.Truncate(originalValue * 100) / 100) == string.Format("{0:0.00}", Math.Truncate(propValue * 100) / 100))
                    {
                        continue;
                    }
                }
                else if (originalValue != null && propValue != null && originalValue.ToString().Trim() == propValue.ToString().Trim())
                {
                    continue;
                }


                logs.Add(AddLog(AuditLog.LogType.Modified, log));
            }
        }

        return logs;
    }

    private AuditLog AddLog(AuditLog.LogType logType, DbEntityEntry dbEntry, string userName, DateTime eventTime, string tableName, string propName)
    {

        return new AuditLog
        {
            UserName = userName,
            EventDate = eventTime,
            EventType = (int)logType,
            ColumnName = propName,
            TableName = tableName,
            OriginalValue = GetVaule(dbEntry, "OriginalValues", propName),
            NewValue = GetVaule(dbEntry, "CurrentValues", propName),
            Entity = dbEntry
        };
    }

    private AuditLog AddLog(AuditLog.LogType logType, AuditLog log)
    {
        return AddLog(logType, log.Entity, log.UserName, log.EventDate, log.TableName, log.ColumnName);
    }

    private string GetVaule(DbEntityEntry dbEntry, string values, string propName)
    {
        return GetVaule(GetVaule(dbEntry, values), propName).ToString();
    }

    private DbPropertyValues GetVaule(DbEntityEntry dbEntry, string value)
    {
        var map = new Dictionary<string, DbPropertyValues>();
        map.Add("OriginalValues", null);
        map.Add("CurrentValues", null);
        if ((dbEntry.State != EntityState.Added))
        {
            map["OriginalValues"] = dbEntry.OriginalValues;
        }
        if ((dbEntry.State != EntityState.Deleted))
        {
            map["CurrentValues"] = dbEntry.CurrentValues;
        }
        return map[value];
    }

    private object GetVaule(DbPropertyValues dbValues, string propName)
    {
        return dbValues?.GetValue<object>(propName);
    }

    private AuditConfig GetAuditConfig(DbEntityEntry dbEntry)
    {
        if (AuditConfigSetting.AuditInfo == null)
        {
            AuditConfigSetting.AuditInfo = new Dictionary<Type, AuditConfig>();
        }
        var entityType = dbEntry.Entity.GetType();
        if (!AuditConfigSetting.AuditInfo.ContainsKey(entityType))
        {
            AuditConfig config = new AuditConfig();
            var tableAttr = (TableAttribute)dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), true).SingleOrDefault();
            var tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;
            var setBase = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(dbEntry.Entity).EntitySet;
            var keyNames = setBase.ElementType.KeyMembers.Select(k => k.Name).ToList();
            config.TableName = tableName;
            config.KeyNames = keyNames;

            var propInfo = new Dictionary<string, Type>();
            var fields = entityType.GetProperties().Select(p => new
            {
                p.Name,
                p.PropertyType
            }).ToList();

            foreach (var propLoopVariable in fields)
            {
                var prop = propLoopVariable;
                if (AuditConfigSetting.IgnoreColumns.Contains(prop.Name))
                {
                    continue;
                }

                if (keyNames.Contains(prop.Name))
                {
                    continue;
                }

                propInfo.Add(prop.Name, prop.PropertyType);
            }
            config.Properties = propInfo;

            AuditConfigSetting.AuditInfo.Add(entityType, config);
        }

        return AuditConfigSetting.AuditInfo[entityType];
    }

}
