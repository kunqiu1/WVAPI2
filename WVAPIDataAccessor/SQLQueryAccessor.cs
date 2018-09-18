using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVAPIDbEF;
using WVAPIDataModels;
using System.Data.Entity.Migrations;

namespace WVAPIDataAccessor
{
    public static class SQLQueryAccessor
    {
        public static IEnumerable<IBStrategyMapping> GetIbStrategyMapping(string account)
        {
            using (wvDB entity = new wvDB())
            {
                return entity.IBStrategyMappings.Where(x => x.AccountName == account).ToList();
            }
        }
        public static IEnumerable<IBStrategy> GetIbStrategy()
        {
            using (wvDB entity = new wvDB())
            {
                return entity.IBStrategies.ToList();
            }
        }
        public static IEnumerable<IBLongShortRatio> GetIbLongShort()
        {
            using (wvDB entity = new wvDB())
            {
                return entity.IBLongShortRatios.ToList();
            }
        }
        public static decimal GetCashActivity(string accountname)
        {
            using (wvDB entity = new wvDB())
            {
                return entity.IBCashActivities.Where(x=>x.AccountName==accountname).Sum(x=>x.Amount);
            }
        }
        internal static void InsertIbStrategyMapping(List<IBStrategyMapping> newmapping)
        {
            using (wvDB entity = new wvDB())
            {
                try
                {
                    entity.Configuration.AutoDetectChangesEnabled = false;

                    foreach (IBStrategyMapping ibmap in newmapping)
                    {
                        entity.IBStrategyMappings.Add(ibmap);
                    }
                    entity.SaveChanges();

                }
                finally
                {
                    entity.Configuration.AutoDetectChangesEnabled = true;
                }
            }
        }
        public static bool UpdateIbStrategyMapping(IBStrategyMapping ibmap)
        {
            using (wvDB entity = new wvDB())
            {
                try
                {
                    entity.Configuration.AutoDetectChangesEnabled = false;
                    ibmap.LastUpdated = DateTime.Now;
                    entity.IBStrategyMappings.AddOrUpdate(ibmap);
                    entity.SaveChanges();
                }
                finally
                {
                    entity.Configuration.AutoDetectChangesEnabled = true;
                }
                return true;
            }
        }
        public static bool UpdateIbStaticData(IEnumerable<IBStaticData> ibmaps)
        {
            using (wvDB entity = new wvDB())
            {
                try
                {
                    foreach (var ibmap in ibmaps)
                    {
                        entity.Configuration.AutoDetectChangesEnabled = false;
                        ibmap.LastUpdated = DateTime.Now;
                        entity.IBStaticDatas.AddOrUpdate(ibmap);
                    }
                    entity.SaveChanges();
                }
                finally
                {
                    entity.Configuration.AutoDetectChangesEnabled = true;
                }
                return true;
            }
        }
        internal static void InsertIbLongShortRatio(List<IBLongShortRatio> longshort)
        {
            using (wvDB entity = new wvDB())
            {
                try
                {
                    entity.Configuration.AutoDetectChangesEnabled = false;

                    foreach (IBLongShortRatio i in longshort)
                    {
                        entity.IBLongShortRatios.Add(i);
                    }
                    entity.SaveChanges();

                }
                finally
                {
                    entity.Configuration.AutoDetectChangesEnabled = true;
                }
            }
        }
    }
}
