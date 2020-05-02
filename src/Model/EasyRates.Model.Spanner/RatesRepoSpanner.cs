using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Google.Cloud.Spanner.Data;
using DateTime = System.DateTime;

namespace EasyRates.Model.Spanner
{
    public class RatesRepoSpanner
    {
        private readonly IMapper mapper;
        private readonly string connectionString;

        private long lastHistoryItemId = -1;
        
        public RatesRepoSpanner(string connectionString, IMapper mapper)
        {
            this.mapper = mapper;
            this.connectionString = connectionString;
        }

        public async Task<CurrencyRate> GetRate(string from, string to)
        {
            await using var connection = new SpannerConnection(connectionString);
            await connection.OpenAsync();
            
            var result = await GetRatesImpl(connection, from, to);

            return result.FirstOrDefault();
        }

        public async Task<CurrencyRate[]> GetRates(string from)
        {
            await using var connection = new SpannerConnection(connectionString);
            await connection.OpenAsync();
            
            var result = await GetRatesImpl(connection, from);
            return result.ToArray();
        }
        
        private async Task<ICollection<CurrencyRate>> GetRatesImpl(SpannerConnection connection, string from = null, string to = null)
        {
            var sql =
                "SELECT currency_from, currency_to, expiration_time, original_published_time, provider_name, time_of_receipt, value FROM actual_rates ";
            if (!string.IsNullOrEmpty(from))
            {
                sql += "where currency_from=@currency_from ";
            }
            if (!string.IsNullOrEmpty(to))
            {
                sql += "and currency_to=@currency_to";
            }
            sql += ";";
                
            var cmd = connection.CreateSelectCommand(sql);
            cmd.Parameters.Add("currency_from", SpannerDbType.String, from);
            cmd.Parameters.Add("currency_to", SpannerDbType.String, to);
            
            var result = new List<CurrencyRate>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new CurrencyRate
                {
                    CurrencyFrom = reader.GetFieldValue<string>("currency_from"),
                    CurrencyTo = reader.GetFieldValue<string>("currency_to"),
                    ExpirationTime = reader.GetFieldValue<DateTime>("expiration_time"),
                    OriginalPublishedTime = reader.GetFieldValue<DateTime>("original_published_time"),
                    ProviderName = reader.GetFieldValue<string>("provider_name"),
                    TimeOfReceipt = reader.GetFieldValue<DateTime>("time_of_receipt"),
                    Value = reader.GetFieldValue<Decimal>("value"),
                });
            }

            return result;
        }
        
        public async Task SetActualRates(ICollection<CurrencyRate> rates)
        {
            await using var connection = new SpannerConnection(connectionString);
            await connection.OpenAsync();

            var actualRates = await GetRatesImpl(connection);

            var tasks = new List<Task>();
            foreach (var newRate in rates)
            {
                var actualRate = actualRates.FirstOrDefault(r => r.Key == newRate.Key);
                if (actualRate == null)
                {
                    var insertActualRate = connection.CreateInsertCommand("actual_rates");
                    insertActualRate.Parameters.Add("currency_from", SpannerDbType.String, newRate.CurrencyFrom);
                    insertActualRate.Parameters.Add("currency_to", SpannerDbType.String, newRate.CurrencyTo);
                    insertActualRate.Parameters.Add("provider_name", SpannerDbType.String,  newRate.ProviderName);
                    insertActualRate.Parameters.Add("expiration_time", SpannerDbType.Timestamp, newRate.ExpirationTime);
                    insertActualRate.Parameters.Add("original_published_time", SpannerDbType.Timestamp, newRate.OriginalPublishedTime);
                    insertActualRate.Parameters.Add("time_of_receipt", SpannerDbType.Timestamp, newRate.TimeOfReceipt);
                    insertActualRate.Parameters.Add("value", SpannerDbType.Int64, newRate.Value);
                    tasks.Add(insertActualRate.ExecuteNonQueryAsync());
                }
                else
                {
                    actualRate.Update(newRate, mapper);
                    
                    var cmd = connection.CreateDmlCommand(
                        "UPDATE actual_rates " + 
                        "SET expiration_time = @expiration_time, " +
                        "original_published_time = @original_published_time, " +
                        "provider_name = @provider_name, " +
                        "time_of_receipt = @time_of_receipt, " +
                        "value = @value " +
                        "WHERE currency_from = @currency_from and currency_to = @currency_to");
                    cmd.Parameters.Add("provider_name", SpannerDbType.String, newRate.ProviderName);
                    cmd.Parameters.Add("expiration_time", SpannerDbType.Timestamp, newRate.ExpirationTime);
                    cmd.Parameters.Add("original_published_time", SpannerDbType.Timestamp, newRate.OriginalPublishedTime);
                    cmd.Parameters.Add("time_of_receipt", SpannerDbType.Timestamp, newRate.TimeOfReceipt);
                    cmd.Parameters.Add("value", SpannerDbType.Int64, newRate.Value);
                    tasks.Add(cmd.ExecuteNonQueryAsync());
                }
            }

            await Task.WhenAll(tasks);
        }

        public async Task AddRatesToHistory(ICollection<CurrencyRateHistoryItem> historyItems)
        {
            await using var connection = new SpannerConnection(connectionString);
            await connection.OpenAsync();
            
            await DefineLastIdIfNeed(connection);
            foreach (var hi in historyItems)
            {
                SetId(hi);
            }
            
            // Insert rows into the Singers table.
            var cmd = connection.CreateInsertCommand("rates_history",
                new SpannerParameterCollection
                {
                    {"id", SpannerDbType.Int64},
                    {"currency_from", SpannerDbType.String},
                    {"currency_to", SpannerDbType.String},
                    {"provider_name", SpannerDbType.String},
                    {"expiration_time", SpannerDbType.Timestamp},
                    {"original_published_time", SpannerDbType.Timestamp},
                    {"time_of_receipt", SpannerDbType.Timestamp},
                    {"value", SpannerDbType.Int64}
                });
            await Task.WhenAll(historyItems.Select(hi =>
            {
                cmd.Parameters["id"].Value = hi.Id;
                cmd.Parameters["currency_from"].Value = hi.CurrencyFrom;
                cmd.Parameters["currency_to"].Value = hi.CurrencyTo;
                cmd.Parameters["provider_name"].Value = hi.ProviderName;
                cmd.Parameters["expiration_time"].Value = hi.ExpirationTime;
                cmd.Parameters["original_published_time"].Value = hi.OriginalPublishedTime;
                cmd.Parameters["time_of_receipt"].Value = hi.TimeOfReceipt;
                cmd.Parameters["value"].Value = hi.Value;
                return cmd.ExecuteNonQueryAsync();
            }));
        }

        private void SetId(CurrencyRateHistoryItem historyItem)
        {
            lastHistoryItemId++;
            historyItem.Id = lastHistoryItemId;
        }

        private async Task DefineLastIdIfNeed(SpannerConnection connection)
        {
            if (lastHistoryItemId != -1) return;
            var cmd = connection.CreateSelectCommand("SELECT max(id) FROM rates_history;");
            
            await using var reader = await cmd.ExecuteReaderAsync();
            await reader.ReadAsync();

            var result = reader.GetValue(0);
            if (result is DBNull)
            {
                lastHistoryItemId = 0;
            }
            else
            {
                lastHistoryItemId = (long) result;
            }
        }

    }
}