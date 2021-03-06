using Newtonsoft.Json;

namespace WalletConnectSharp.Core.Models.Ethereum
{
    public sealed class EthSign : JsonRpcRequest
    {
        [JsonProperty("params")] 
        private string[] _parameters;

        [JsonIgnore]
        public string[] Parameters => _parameters;

        public EthSign(string hexData, string address) : base()
        {
            this.Method = "eth_sign";
            this._parameters = new[] {address, hexData};
        }
    }
}