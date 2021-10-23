using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using SygEthlisbon.Contracts.GalaxyToken.ContractDefinition;

namespace SygEthlisbon.Contracts.GalaxyToken
{
    public partial class GalaxyTokenService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, GalaxyTokenDeployment galaxyTokenDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<GalaxyTokenDeployment>().SendRequestAndWaitForReceiptAsync(galaxyTokenDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, GalaxyTokenDeployment galaxyTokenDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<GalaxyTokenDeployment>().SendRequestAsync(galaxyTokenDeployment);
        }

        public static async Task<GalaxyTokenService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, GalaxyTokenDeployment galaxyTokenDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, galaxyTokenDeployment, cancellationTokenSource);
            return new GalaxyTokenService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public GalaxyTokenService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> AddOperatorRequestAsync(AddOperatorFunction addOperatorFunction)
        {
             return ContractHandler.SendRequestAsync(addOperatorFunction);
        }

        public Task<TransactionReceipt> AddOperatorRequestAndWaitForReceiptAsync(AddOperatorFunction addOperatorFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addOperatorFunction, cancellationToken);
        }

        public Task<string> AddOperatorRequestAsync(string account, BigInteger type)
        {
            var addOperatorFunction = new AddOperatorFunction();
                addOperatorFunction.Account = account;
                addOperatorFunction.Type = type;
            
             return ContractHandler.SendRequestAsync(addOperatorFunction);
        }

        public Task<TransactionReceipt> AddOperatorRequestAndWaitForReceiptAsync(string account, BigInteger type, CancellationTokenSource cancellationToken = null)
        {
            var addOperatorFunction = new AddOperatorFunction();
                addOperatorFunction.Account = account;
                addOperatorFunction.Type = type;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addOperatorFunction, cancellationToken);
        }

        public Task<BigInteger> BalanceOfQueryAsync(BalanceOfFunction balanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        
        public Task<BigInteger> BalanceOfQueryAsync(string account, BigInteger id, BlockParameter blockParameter = null)
        {
            var balanceOfFunction = new BalanceOfFunction();
                balanceOfFunction.Account = account;
                balanceOfFunction.Id = id;
            
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        public Task<List<BigInteger>> BalanceOfBatchQueryAsync(BalanceOfBatchFunction balanceOfBatchFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfBatchFunction, List<BigInteger>>(balanceOfBatchFunction, blockParameter);
        }

        
        public Task<List<BigInteger>> BalanceOfBatchQueryAsync(List<string> accounts, List<BigInteger> ids, BlockParameter blockParameter = null)
        {
            var balanceOfBatchFunction = new BalanceOfBatchFunction();
                balanceOfBatchFunction.Accounts = accounts;
                balanceOfBatchFunction.Ids = ids;
            
            return ContractHandler.QueryAsync<BalanceOfBatchFunction, List<BigInteger>>(balanceOfBatchFunction, blockParameter);
        }

        public Task<BigInteger> ClaimableAmountQueryAsync(ClaimableAmountFunction claimableAmountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ClaimableAmountFunction, BigInteger>(claimableAmountFunction, blockParameter);
        }

        
        public Task<BigInteger> ClaimableAmountQueryAsync(string returnValue1, BigInteger returnValue2, BlockParameter blockParameter = null)
        {
            var claimableAmountFunction = new ClaimableAmountFunction();
                claimableAmountFunction.ReturnValue1 = returnValue1;
                claimableAmountFunction.ReturnValue2 = returnValue2;
            
            return ContractHandler.QueryAsync<ClaimableAmountFunction, BigInteger>(claimableAmountFunction, blockParameter);
        }

        public Task<string> CreateTokenTypeRequestAsync(CreateTokenTypeFunction createTokenTypeFunction)
        {
             return ContractHandler.SendRequestAsync(createTokenTypeFunction);
        }

        public Task<TransactionReceipt> CreateTokenTypeRequestAndWaitForReceiptAsync(CreateTokenTypeFunction createTokenTypeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createTokenTypeFunction, cancellationToken);
        }

        public Task<string> CreateTokenTypeRequestAsync(bool isNF)
        {
            var createTokenTypeFunction = new CreateTokenTypeFunction();
                createTokenTypeFunction.IsNF = isNF;
            
             return ContractHandler.SendRequestAsync(createTokenTypeFunction);
        }

        public Task<TransactionReceipt> CreateTokenTypeRequestAndWaitForReceiptAsync(bool isNF, CancellationTokenSource cancellationToken = null)
        {
            var createTokenTypeFunction = new CreateTokenTypeFunction();
                createTokenTypeFunction.IsNF = isNF;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createTokenTypeFunction, cancellationToken);
        }

        public Task<string> DividendClaimRequestAsync(DividendClaimFunction dividendClaimFunction)
        {
             return ContractHandler.SendRequestAsync(dividendClaimFunction);
        }

        public Task<TransactionReceipt> DividendClaimRequestAndWaitForReceiptAsync(DividendClaimFunction dividendClaimFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(dividendClaimFunction, cancellationToken);
        }

        public Task<string> DividendClaimRequestAsync(BigInteger type, string account)
        {
            var dividendClaimFunction = new DividendClaimFunction();
                dividendClaimFunction.Type = type;
                dividendClaimFunction.Account = account;
            
             return ContractHandler.SendRequestAsync(dividendClaimFunction);
        }

        public Task<TransactionReceipt> DividendClaimRequestAndWaitForReceiptAsync(BigInteger type, string account, CancellationTokenSource cancellationToken = null)
        {
            var dividendClaimFunction = new DividendClaimFunction();
                dividendClaimFunction.Type = type;
                dividendClaimFunction.Account = account;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(dividendClaimFunction, cancellationToken);
        }

        public Task<string> FungibleBurnRequestAsync(FungibleBurnFunction fungibleBurnFunction)
        {
             return ContractHandler.SendRequestAsync(fungibleBurnFunction);
        }

        public Task<TransactionReceipt> FungibleBurnRequestAndWaitForReceiptAsync(FungibleBurnFunction fungibleBurnFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(fungibleBurnFunction, cancellationToken);
        }

        public Task<string> FungibleBurnRequestAsync(string account, BigInteger type, BigInteger amount)
        {
            var fungibleBurnFunction = new FungibleBurnFunction();
                fungibleBurnFunction.Account = account;
                fungibleBurnFunction.Type = type;
                fungibleBurnFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(fungibleBurnFunction);
        }

        public Task<TransactionReceipt> FungibleBurnRequestAndWaitForReceiptAsync(string account, BigInteger type, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var fungibleBurnFunction = new FungibleBurnFunction();
                fungibleBurnFunction.Account = account;
                fungibleBurnFunction.Type = type;
                fungibleBurnFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(fungibleBurnFunction, cancellationToken);
        }

        public Task<string> FungibleMintRequestAsync(FungibleMintFunction fungibleMintFunction)
        {
             return ContractHandler.SendRequestAsync(fungibleMintFunction);
        }

        public Task<TransactionReceipt> FungibleMintRequestAndWaitForReceiptAsync(FungibleMintFunction fungibleMintFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(fungibleMintFunction, cancellationToken);
        }

        public Task<string> FungibleMintRequestAsync(string account, BigInteger type, BigInteger amount, byte[] data)
        {
            var fungibleMintFunction = new FungibleMintFunction();
                fungibleMintFunction.Account = account;
                fungibleMintFunction.Type = type;
                fungibleMintFunction.Amount = amount;
                fungibleMintFunction.Data = data;
            
             return ContractHandler.SendRequestAsync(fungibleMintFunction);
        }

        public Task<TransactionReceipt> FungibleMintRequestAndWaitForReceiptAsync(string account, BigInteger type, BigInteger amount, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var fungibleMintFunction = new FungibleMintFunction();
                fungibleMintFunction.Account = account;
                fungibleMintFunction.Type = type;
                fungibleMintFunction.Amount = amount;
                fungibleMintFunction.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(fungibleMintFunction, cancellationToken);
        }

        public Task<string> GetNfOwnerQueryAsync(GetNfOwnerFunction getNfOwnerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetNfOwnerFunction, string>(getNfOwnerFunction, blockParameter);
        }

        
        public Task<string> GetNfOwnerQueryAsync(BigInteger id, BlockParameter blockParameter = null)
        {
            var getNfOwnerFunction = new GetNfOwnerFunction();
                getNfOwnerFunction.Id = id;
            
            return ContractHandler.QueryAsync<GetNfOwnerFunction, string>(getNfOwnerFunction, blockParameter);
        }

        public Task<BigInteger> GetNonFungibleIndexQueryAsync(GetNonFungibleIndexFunction getNonFungibleIndexFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetNonFungibleIndexFunction, BigInteger>(getNonFungibleIndexFunction, blockParameter);
        }

        
        public Task<BigInteger> GetNonFungibleIndexQueryAsync(BigInteger id, BlockParameter blockParameter = null)
        {
            var getNonFungibleIndexFunction = new GetNonFungibleIndexFunction();
                getNonFungibleIndexFunction.Id = id;
            
            return ContractHandler.QueryAsync<GetNonFungibleIndexFunction, BigInteger>(getNonFungibleIndexFunction, blockParameter);
        }

        public Task<BigInteger> GetNonFungibleTokenTypeQueryAsync(GetNonFungibleTokenTypeFunction getNonFungibleTokenTypeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetNonFungibleTokenTypeFunction, BigInteger>(getNonFungibleTokenTypeFunction, blockParameter);
        }

        
        public Task<BigInteger> GetNonFungibleTokenTypeQueryAsync(BigInteger id, BlockParameter blockParameter = null)
        {
            var getNonFungibleTokenTypeFunction = new GetNonFungibleTokenTypeFunction();
                getNonFungibleTokenTypeFunction.Id = id;
            
            return ContractHandler.QueryAsync<GetNonFungibleTokenTypeFunction, BigInteger>(getNonFungibleTokenTypeFunction, blockParameter);
        }

        public Task<BigInteger> HolderAmountQueryAsync(HolderAmountFunction holderAmountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<HolderAmountFunction, BigInteger>(holderAmountFunction, blockParameter);
        }

        
        public Task<BigInteger> HolderAmountQueryAsync(string returnValue1, BigInteger returnValue2, BlockParameter blockParameter = null)
        {
            var holderAmountFunction = new HolderAmountFunction();
                holderAmountFunction.ReturnValue1 = returnValue1;
                holderAmountFunction.ReturnValue2 = returnValue2;
            
            return ContractHandler.QueryAsync<HolderAmountFunction, BigInteger>(holderAmountFunction, blockParameter);
        }

        public Task<bool> IsApprovedForAllQueryAsync(IsApprovedForAllFunction isApprovedForAllFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsApprovedForAllFunction, bool>(isApprovedForAllFunction, blockParameter);
        }

        
        public Task<bool> IsApprovedForAllQueryAsync(string account, string @operator, BlockParameter blockParameter = null)
        {
            var isApprovedForAllFunction = new IsApprovedForAllFunction();
                isApprovedForAllFunction.Account = account;
                isApprovedForAllFunction.Operator = @operator;
            
            return ContractHandler.QueryAsync<IsApprovedForAllFunction, bool>(isApprovedForAllFunction, blockParameter);
        }

        public Task<bool> IsFungibleQueryAsync(IsFungibleFunction isFungibleFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsFungibleFunction, bool>(isFungibleFunction, blockParameter);
        }

        
        public Task<bool> IsFungibleQueryAsync(BigInteger id, BlockParameter blockParameter = null)
        {
            var isFungibleFunction = new IsFungibleFunction();
                isFungibleFunction.Id = id;
            
            return ContractHandler.QueryAsync<IsFungibleFunction, bool>(isFungibleFunction, blockParameter);
        }

        public Task<bool> IsNonFungibleQueryAsync(IsNonFungibleFunction isNonFungibleFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsNonFungibleFunction, bool>(isNonFungibleFunction, blockParameter);
        }

        
        public Task<bool> IsNonFungibleQueryAsync(BigInteger id, BlockParameter blockParameter = null)
        {
            var isNonFungibleFunction = new IsNonFungibleFunction();
                isNonFungibleFunction.Id = id;
            
            return ContractHandler.QueryAsync<IsNonFungibleFunction, bool>(isNonFungibleFunction, blockParameter);
        }

        public Task<string> NonFungibleMintRequestAsync(NonFungibleMintFunction nonFungibleMintFunction)
        {
             return ContractHandler.SendRequestAsync(nonFungibleMintFunction);
        }

        public Task<TransactionReceipt> NonFungibleMintRequestAndWaitForReceiptAsync(NonFungibleMintFunction nonFungibleMintFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(nonFungibleMintFunction, cancellationToken);
        }

        public Task<string> NonFungibleMintRequestAsync(string account, BigInteger type, string tokenURI)
        {
            var nonFungibleMintFunction = new NonFungibleMintFunction();
                nonFungibleMintFunction.Account = account;
                nonFungibleMintFunction.Type = type;
                nonFungibleMintFunction.TokenURI = tokenURI;
            
             return ContractHandler.SendRequestAsync(nonFungibleMintFunction);
        }

        public Task<TransactionReceipt> NonFungibleMintRequestAndWaitForReceiptAsync(string account, BigInteger type, string tokenURI, CancellationTokenSource cancellationToken = null)
        {
            var nonFungibleMintFunction = new NonFungibleMintFunction();
                nonFungibleMintFunction.Account = account;
                nonFungibleMintFunction.Type = type;
                nonFungibleMintFunction.TokenURI = tokenURI;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(nonFungibleMintFunction, cancellationToken);
        }

        public Task<bool> OperatorsQueryAsync(OperatorsFunction operatorsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OperatorsFunction, bool>(operatorsFunction, blockParameter);
        }

        
        public Task<bool> OperatorsQueryAsync(BigInteger returnValue1, string returnValue2, BlockParameter blockParameter = null)
        {
            var operatorsFunction = new OperatorsFunction();
                operatorsFunction.ReturnValue1 = returnValue1;
                operatorsFunction.ReturnValue2 = returnValue2;
            
            return ContractHandler.QueryAsync<OperatorsFunction, bool>(operatorsFunction, blockParameter);
        }

        public Task<string> OwnerQueryAsync(OwnerFunction ownerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }

        
        public Task<string> OwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(null, blockParameter);
        }

        public Task<string> ProvideDividendRequestAsync(ProvideDividendFunction provideDividendFunction)
        {
             return ContractHandler.SendRequestAsync(provideDividendFunction);
        }

        public Task<TransactionReceipt> ProvideDividendRequestAndWaitForReceiptAsync(ProvideDividendFunction provideDividendFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(provideDividendFunction, cancellationToken);
        }

        public Task<string> ProvideDividendRequestAsync(BigInteger type, string account, BigInteger amount)
        {
            var provideDividendFunction = new ProvideDividendFunction();
                provideDividendFunction.Type = type;
                provideDividendFunction.Account = account;
                provideDividendFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(provideDividendFunction);
        }

        public Task<TransactionReceipt> ProvideDividendRequestAndWaitForReceiptAsync(BigInteger type, string account, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var provideDividendFunction = new ProvideDividendFunction();
                provideDividendFunction.Type = type;
                provideDividendFunction.Account = account;
                provideDividendFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(provideDividendFunction, cancellationToken);
        }

        public Task<string> RemoveOperatorRequestAsync(RemoveOperatorFunction removeOperatorFunction)
        {
             return ContractHandler.SendRequestAsync(removeOperatorFunction);
        }

        public Task<TransactionReceipt> RemoveOperatorRequestAndWaitForReceiptAsync(RemoveOperatorFunction removeOperatorFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(removeOperatorFunction, cancellationToken);
        }

        public Task<string> RemoveOperatorRequestAsync(string account, BigInteger type)
        {
            var removeOperatorFunction = new RemoveOperatorFunction();
                removeOperatorFunction.Account = account;
                removeOperatorFunction.Type = type;
            
             return ContractHandler.SendRequestAsync(removeOperatorFunction);
        }

        public Task<TransactionReceipt> RemoveOperatorRequestAndWaitForReceiptAsync(string account, BigInteger type, CancellationTokenSource cancellationToken = null)
        {
            var removeOperatorFunction = new RemoveOperatorFunction();
                removeOperatorFunction.Account = account;
                removeOperatorFunction.Type = type;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(removeOperatorFunction, cancellationToken);
        }

        public Task<string> RenounceOwnershipRequestAsync(RenounceOwnershipFunction renounceOwnershipFunction)
        {
             return ContractHandler.SendRequestAsync(renounceOwnershipFunction);
        }

        public Task<string> RenounceOwnershipRequestAsync()
        {
             return ContractHandler.SendRequestAsync<RenounceOwnershipFunction>();
        }

        public Task<TransactionReceipt> RenounceOwnershipRequestAndWaitForReceiptAsync(RenounceOwnershipFunction renounceOwnershipFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(renounceOwnershipFunction, cancellationToken);
        }

        public Task<TransactionReceipt> RenounceOwnershipRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<RenounceOwnershipFunction>(null, cancellationToken);
        }

        public Task<string> SafeBatchTransferFromRequestAsync(SafeBatchTransferFromFunction safeBatchTransferFromFunction)
        {
             return ContractHandler.SendRequestAsync(safeBatchTransferFromFunction);
        }

        public Task<TransactionReceipt> SafeBatchTransferFromRequestAndWaitForReceiptAsync(SafeBatchTransferFromFunction safeBatchTransferFromFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(safeBatchTransferFromFunction, cancellationToken);
        }

        public Task<string> SafeBatchTransferFromRequestAsync(string from, string to, List<BigInteger> ids, List<BigInteger> amounts, byte[] data)
        {
            var safeBatchTransferFromFunction = new SafeBatchTransferFromFunction();
                safeBatchTransferFromFunction.From = from;
                safeBatchTransferFromFunction.To = to;
                safeBatchTransferFromFunction.Ids = ids;
                safeBatchTransferFromFunction.Amounts = amounts;
                safeBatchTransferFromFunction.Data = data;
            
             return ContractHandler.SendRequestAsync(safeBatchTransferFromFunction);
        }

        public Task<TransactionReceipt> SafeBatchTransferFromRequestAndWaitForReceiptAsync(string from, string to, List<BigInteger> ids, List<BigInteger> amounts, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var safeBatchTransferFromFunction = new SafeBatchTransferFromFunction();
                safeBatchTransferFromFunction.From = from;
                safeBatchTransferFromFunction.To = to;
                safeBatchTransferFromFunction.Ids = ids;
                safeBatchTransferFromFunction.Amounts = amounts;
                safeBatchTransferFromFunction.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(safeBatchTransferFromFunction, cancellationToken);
        }

        public Task<string> SafeTransferFromRequestAsync(SafeTransferFromFunction safeTransferFromFunction)
        {
             return ContractHandler.SendRequestAsync(safeTransferFromFunction);
        }

        public Task<TransactionReceipt> SafeTransferFromRequestAndWaitForReceiptAsync(SafeTransferFromFunction safeTransferFromFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(safeTransferFromFunction, cancellationToken);
        }

        public Task<string> SafeTransferFromRequestAsync(string from, string to, BigInteger id, BigInteger amount, byte[] data)
        {
            var safeTransferFromFunction = new SafeTransferFromFunction();
                safeTransferFromFunction.From = from;
                safeTransferFromFunction.To = to;
                safeTransferFromFunction.Id = id;
                safeTransferFromFunction.Amount = amount;
                safeTransferFromFunction.Data = data;
            
             return ContractHandler.SendRequestAsync(safeTransferFromFunction);
        }

        public Task<TransactionReceipt> SafeTransferFromRequestAndWaitForReceiptAsync(string from, string to, BigInteger id, BigInteger amount, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var safeTransferFromFunction = new SafeTransferFromFunction();
                safeTransferFromFunction.From = from;
                safeTransferFromFunction.To = to;
                safeTransferFromFunction.Id = id;
                safeTransferFromFunction.Amount = amount;
                safeTransferFromFunction.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(safeTransferFromFunction, cancellationToken);
        }

        public Task<string> SetApprovalForAllRequestAsync(SetApprovalForAllFunction setApprovalForAllFunction)
        {
             return ContractHandler.SendRequestAsync(setApprovalForAllFunction);
        }

        public Task<TransactionReceipt> SetApprovalForAllRequestAndWaitForReceiptAsync(SetApprovalForAllFunction setApprovalForAllFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setApprovalForAllFunction, cancellationToken);
        }

        public Task<string> SetApprovalForAllRequestAsync(string @operator, bool approved)
        {
            var setApprovalForAllFunction = new SetApprovalForAllFunction();
                setApprovalForAllFunction.Operator = @operator;
                setApprovalForAllFunction.Approved = approved;
            
             return ContractHandler.SendRequestAsync(setApprovalForAllFunction);
        }

        public Task<TransactionReceipt> SetApprovalForAllRequestAndWaitForReceiptAsync(string @operator, bool approved, CancellationTokenSource cancellationToken = null)
        {
            var setApprovalForAllFunction = new SetApprovalForAllFunction();
                setApprovalForAllFunction.Operator = @operator;
                setApprovalForAllFunction.Approved = approved;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setApprovalForAllFunction, cancellationToken);
        }

        public Task<bool> SupportsInterfaceQueryAsync(SupportsInterfaceFunction supportsInterfaceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SupportsInterfaceFunction, bool>(supportsInterfaceFunction, blockParameter);
        }

        
        public Task<bool> SupportsInterfaceQueryAsync(byte[] interfaceId, BlockParameter blockParameter = null)
        {
            var supportsInterfaceFunction = new SupportsInterfaceFunction();
                supportsInterfaceFunction.InterfaceId = interfaceId;
            
            return ContractHandler.QueryAsync<SupportsInterfaceFunction, bool>(supportsInterfaceFunction, blockParameter);
        }

        public Task<string> TokenMetadataQueryAsync(TokenMetadataFunction tokenMetadataFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TokenMetadataFunction, string>(tokenMetadataFunction, blockParameter);
        }

        
        public Task<string> TokenMetadataQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var tokenMetadataFunction = new TokenMetadataFunction();
                tokenMetadataFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<TokenMetadataFunction, string>(tokenMetadataFunction, blockParameter);
        }

        public Task<BigInteger> TotalSupplyQueryAsync(TotalSupplyFunction totalSupplyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(totalSupplyFunction, blockParameter);
        }

        
        public Task<BigInteger> TotalSupplyQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var totalSupplyFunction = new TotalSupplyFunction();
                totalSupplyFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(totalSupplyFunction, blockParameter);
        }

        public Task<string> TransferOwnershipRequestAsync(TransferOwnershipFunction transferOwnershipFunction)
        {
             return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public Task<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(TransferOwnershipFunction transferOwnershipFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public Task<string> TransferOwnershipRequestAsync(string newOwner)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
                transferOwnershipFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public Task<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(string newOwner, CancellationTokenSource cancellationToken = null)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
                transferOwnershipFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public Task<string> UriQueryAsync(UriFunction uriFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<UriFunction, string>(uriFunction, blockParameter);
        }

        
        public Task<string> UriQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var uriFunction = new UriFunction();
                uriFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<UriFunction, string>(uriFunction, blockParameter);
        }
    }
}
