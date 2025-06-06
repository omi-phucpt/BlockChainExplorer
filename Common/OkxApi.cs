using System.Dynamic;
using System.Reflection;
using System.Text.Json;

namespace BlockchainExplorer.Common
{
    public class OkxApi
    {
        public class OkxAccounts
        {
            public string GetAccount { get; } = "https://web3.okx.com/api/v5/wallet/account/accounts";
            public string CreateAccount { get; } = "https://web3.okx.com/api/v5/wallet/account/create-wallet-account";
            public string UpdateAccount { get; } = "https://web3.okx.com/api/v5/wallet/account/update-wallet-account";
        }

        public class BalanceEndpoints
        {
            public string SupportedChain { get; } = "https://web3.okx.com/api/v5/dex/balance/supported/chain";
            public string Account { get; } = "https://web3.okx.com/api/v5/wallet/asset/total-value";
            public string Address { get; } = "https://web3.okx.com/api/v5/wallet/asset/total-value-by-address";
            public string AllTokenAccount { get; } = "https://web3.okx.com/api/wallet/asset/wallet-all-token-balances";
            public string AllTokenAddress { get; } = "https://web3.okx.com/api/v5/wallet/asset/all-token-balances-by-address";
            public string TokenBalancesByAddress { get; } = "https://web3.okx.com/api/v5/dex/balance/token-balances-by-address";
        }

        public class IndexPriceEndpoints
        {
            public string CurrentPrice { get; } = "https://web3.okx.com/api/dex/index/current-price";
            public string HistoricalPrice { get; } = "https://web3.okx.com/api/v5/dex/index/historical-price";
        }

        public class TransactionHistoryEndpoints
        {
            public string TransactionsByAddress { get; } = "https://web3.okx.com/api/v5/dex/post-transaction/transactions-by-address";
            public string TransactionDetailByTxHash { get; } = "https://web3.okx.com/api/v5/dex/post-transaction/transaction-detail-by-txhash";

            public static class EvmTransactionTypes
            {
                public const int OuterLayerMainnetCoinTransfer = 0;
                public const int InnerLayerMainnetCoinTransfer = 1;
                public const int TokenTransfer = 2;
            }
        }

        public class MarketPriceEndpoints
        {
            public string SupportedChain { get; } = "https://web3.okx.com/api/v5/dex/market/supported/chain";
            public string Price { get; } = "https://web3.okx.com/api/v5/dex/market/price";
            public string PriceInfo { get; } = "https://web3.okx.com/api/v5/dex/market/price-info";
            public string Trades { get; } = "https://web3.okx.com/api/v5/dex/market/trades";
            public string Candles { get; } = "https://web3.okx.com/api/v5/dex/market/candles";
            public string HistoricalCandles { get; } = "https://web3.okx.com/api/v5/dex/market/historical-candles";
        }

        public OkxAccounts Account { get; } = new OkxAccounts();
        public BalanceEndpoints Balance { get; } = new BalanceEndpoints();
        public IndexPriceEndpoints IndexPrice { get; } = new IndexPriceEndpoints();
        public TransactionHistoryEndpoints TransactionHistory { get; } = new TransactionHistoryEndpoints();
        public MarketPriceEndpoints MarketPrice { get; } = new MarketPriceEndpoints();

        public static object ConvertJsonElement<TModel>(JsonElement jsonElement)
            where TModel : class, new()
        {
            if (jsonElement.ValueKind == JsonValueKind.Array)
                return ConvertJsonElementToModelList<TModel>(jsonElement);

            return ConvertJsonElementToModel<TModel>(jsonElement);
        }

        public static List<TModel> ConvertJsonElementToModelList<TModel>(JsonElement jsonElement)
            where TModel : class, new()
        {
            if (jsonElement.ValueKind != JsonValueKind.Array)
                throw new InvalidOperationException("JSON không phải là mảng.");

            var result = new List<TModel>();

            foreach (var element in jsonElement.EnumerateArray())
            {
                if (element.ValueKind != JsonValueKind.Object)
                    throw new InvalidOperationException("Phần tử trong mảng không phải là object.");

                var model = new TModel();
                var properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var property in properties)
                {
                    if (element.TryGetProperty(property.Name, out var value))
                    {
                        object? propertyValue = value.ValueKind switch
                        {
                            JsonValueKind.String => value.GetString(),
                            JsonValueKind.Number => GetNumberValue(value, property.PropertyType),
                            JsonValueKind.True => true,
                            JsonValueKind.False => false,
                            JsonValueKind.Array => JsonSerializer.Deserialize(value.GetRawText(), property.PropertyType),
                            JsonValueKind.Object => JsonSerializer.Deserialize(value.GetRawText(), property.PropertyType),
                            JsonValueKind.Null => null,
                            _ => throw new InvalidOperationException($"Kiểu JsonValueKind không hỗ trợ: {value.ValueKind}")
                        };

                        if (propertyValue != null && property.PropertyType.IsAssignableFrom(propertyValue.GetType()))
                        {
                            property.SetValue(model, propertyValue);
                        }
                        else if (propertyValue == null && property.PropertyType.IsClass)
                        {
                            property.SetValue(model, null);
                        }
                        else
                        {
                            property.SetValue(model, Convert.ChangeType(propertyValue, property.PropertyType));
                        }
                    }
                }

                result.Add(model);
            }

            return result;
        }

        public static TModel ConvertJsonElementToModel<TModel>(JsonElement jsonElement)
            where TModel : class, new()
        {
            JsonElement targetElement = jsonElement.ValueKind == JsonValueKind.Array
        ? jsonElement.EnumerateArray().FirstOrDefault()
        : jsonElement;

            if (targetElement.ValueKind == JsonValueKind.Undefined)
                throw new InvalidOperationException("JSON array is empty or invalid.");

            var model = new TModel();

            var properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (targetElement.TryGetProperty(property.Name, out var value))
                {
                    object? propertyValue = value.ValueKind switch
                    {
                        JsonValueKind.String => value.GetString(),
                        JsonValueKind.Number => GetNumberValue(value, property.PropertyType),
                        JsonValueKind.True => true,
                        JsonValueKind.False => false,
                        JsonValueKind.Array => JsonSerializer.Deserialize(value.GetRawText(), property.PropertyType),
                        JsonValueKind.Object => JsonSerializer.Deserialize(value.GetRawText(), property.PropertyType),
                        JsonValueKind.Null => null,
                        _ => throw new InvalidOperationException($"Unsupported JsonValueKind: {value.ValueKind}")
                    };

                    if (propertyValue != null && property.PropertyType.IsAssignableFrom(propertyValue.GetType()))
                    {
                        property.SetValue(model, propertyValue);
                    }
                    else if (propertyValue == null && property.PropertyType.IsClass)
                    {
                        property.SetValue(model, null);
                    }
                    else
                    {
                        property.SetValue(model, Convert.ChangeType(propertyValue, property.PropertyType));
                    }
                }
            }

            return model;
        }

        private static object GetNumberValue(JsonElement element, Type targetType)
        {
            if (targetType == typeof(int) && element.TryGetInt32(out var intValue))
                return intValue;
            if (targetType == typeof(long) && element.TryGetInt64(out var longValue))
                return longValue;
            if (targetType == typeof(double) && element.TryGetDouble(out var doubleValue))
                return doubleValue;
            if (targetType == typeof(decimal) && element.TryGetDecimal(out var decimalValue))
                return decimalValue;
            if (targetType == typeof(float) && element.TryGetSingle(out var floatValue))
                return floatValue;

            throw new InvalidOperationException($"Cannot convert JSON number to {targetType.Name}");
        }
    }
}
