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
using SygEthlisbon.Contracts.SpaceMafia.ContractDefinition;

namespace SygEthlisbon.Contracts.SpaceMafia
{
    public partial class SpaceMafiaService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, SpaceMafiaDeployment spaceMafiaDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<SpaceMafiaDeployment>().SendRequestAndWaitForReceiptAsync(spaceMafiaDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, SpaceMafiaDeployment spaceMafiaDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<SpaceMafiaDeployment>().SendRequestAsync(spaceMafiaDeployment);
        }

        public static async Task<SpaceMafiaService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, SpaceMafiaDeployment spaceMafiaDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, spaceMafiaDeployment, cancellationTokenSource);
            return new SpaceMafiaService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public SpaceMafiaService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<bool> CheckRandomQueryAsync(CheckRandomFunction checkRandomFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CheckRandomFunction, bool>(checkRandomFunction, blockParameter);
        }

        
        public Task<bool> CheckRandomQueryAsync(BigInteger random, BigInteger threshold, BlockParameter blockParameter = null)
        {
            var checkRandomFunction = new CheckRandomFunction();
                checkRandomFunction.Random = random;
                checkRandomFunction.Threshold = threshold;
            
            return ContractHandler.QueryAsync<CheckRandomFunction, bool>(checkRandomFunction, blockParameter);
        }

        public Task<string> ClaimDividendsRequestAsync(ClaimDividendsFunction claimDividendsFunction)
        {
             return ContractHandler.SendRequestAsync(claimDividendsFunction);
        }

        public Task<TransactionReceipt> ClaimDividendsRequestAndWaitForReceiptAsync(ClaimDividendsFunction claimDividendsFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(claimDividendsFunction, cancellationToken);
        }

        public Task<string> ClaimDividendsRequestAsync(BigInteger tokenId)
        {
            var claimDividendsFunction = new ClaimDividendsFunction();
                claimDividendsFunction.TokenId = tokenId;
            
             return ContractHandler.SendRequestAsync(claimDividendsFunction);
        }

        public Task<TransactionReceipt> ClaimDividendsRequestAndWaitForReceiptAsync(BigInteger tokenId, CancellationTokenSource cancellationToken = null)
        {
            var claimDividendsFunction = new ClaimDividendsFunction();
                claimDividendsFunction.TokenId = tokenId;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(claimDividendsFunction, cancellationToken);
        }

        public Task<BigInteger> ClaimableDividendsQueryAsync(ClaimableDividendsFunction claimableDividendsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ClaimableDividendsFunction, BigInteger>(claimableDividendsFunction, blockParameter);
        }

        
        public Task<BigInteger> ClaimableDividendsQueryAsync(BigInteger tokenId, BlockParameter blockParameter = null)
        {
            var claimableDividendsFunction = new ClaimableDividendsFunction();
                claimableDividendsFunction.TokenId = tokenId;
            
            return ContractHandler.QueryAsync<ClaimableDividendsFunction, BigInteger>(claimableDividendsFunction, blockParameter);
        }

        public Task<string> CompleteAttackRequestAsync(CompleteAttackFunction completeAttackFunction)
        {
             return ContractHandler.SendRequestAsync(completeAttackFunction);
        }

        public Task<TransactionReceipt> CompleteAttackRequestAndWaitForReceiptAsync(CompleteAttackFunction completeAttackFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(completeAttackFunction, cancellationToken);
        }

        public Task<string> CompleteAttackRequestAsync(BigInteger nukeId)
        {
            var completeAttackFunction = new CompleteAttackFunction();
                completeAttackFunction.NukeId = nukeId;
            
             return ContractHandler.SendRequestAsync(completeAttackFunction);
        }

        public Task<TransactionReceipt> CompleteAttackRequestAndWaitForReceiptAsync(BigInteger nukeId, CancellationTokenSource cancellationToken = null)
        {
            var completeAttackFunction = new CompleteAttackFunction();
                completeAttackFunction.NukeId = nukeId;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(completeAttackFunction, cancellationToken);
        }

        public Task<BigInteger> DefenseProbabilityQueryAsync(DefenseProbabilityFunction defenseProbabilityFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DefenseProbabilityFunction, BigInteger>(defenseProbabilityFunction, blockParameter);
        }

        
        public Task<BigInteger> DefenseProbabilityQueryAsync(BigInteger nukeId, BlockParameter blockParameter = null)
        {
            var defenseProbabilityFunction = new DefenseProbabilityFunction();
                defenseProbabilityFunction.NukeId = nukeId;
            
            return ContractHandler.QueryAsync<DefenseProbabilityFunction, BigInteger>(defenseProbabilityFunction, blockParameter);
        }

        public Task<string> DepositInAAVERequestAsync(DepositInAAVEFunction depositInAAVEFunction)
        {
             return ContractHandler.SendRequestAsync(depositInAAVEFunction);
        }

        public Task<TransactionReceipt> DepositInAAVERequestAndWaitForReceiptAsync(DepositInAAVEFunction depositInAAVEFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(depositInAAVEFunction, cancellationToken);
        }

        public Task<string> DepositInAAVERequestAsync(BigInteger amount)
        {
            var depositInAAVEFunction = new DepositInAAVEFunction();
                depositInAAVEFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(depositInAAVEFunction);
        }

        public Task<TransactionReceipt> DepositInAAVERequestAndWaitForReceiptAsync(BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var depositInAAVEFunction = new DepositInAAVEFunction();
                depositInAAVEFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(depositInAAVEFunction, cancellationToken);
        }

        public Task<string> GalaxyTokenQueryAsync(GalaxyTokenFunction galaxyTokenFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GalaxyTokenFunction, string>(galaxyTokenFunction, blockParameter);
        }

        
        public Task<string> GalaxyTokenQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GalaxyTokenFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> GetMafiaBalanceOfQueryAsync(GetMafiaBalanceOfFunction getMafiaBalanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetMafiaBalanceOfFunction, BigInteger>(getMafiaBalanceOfFunction, blockParameter);
        }

        
        public Task<BigInteger> GetMafiaBalanceOfQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var getMafiaBalanceOfFunction = new GetMafiaBalanceOfFunction();
                getMafiaBalanceOfFunction.Account = account;
            
            return ContractHandler.QueryAsync<GetMafiaBalanceOfFunction, BigInteger>(getMafiaBalanceOfFunction, blockParameter);
        }

        public Task<BigInteger> GetMafiaTokenSupplyQueryAsync(GetMafiaTokenSupplyFunction getMafiaTokenSupplyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetMafiaTokenSupplyFunction, BigInteger>(getMafiaTokenSupplyFunction, blockParameter);
        }

        
        public Task<BigInteger> GetMafiaTokenSupplyQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetMafiaTokenSupplyFunction, BigInteger>(null, blockParameter);
        }

        public Task<GetPlanetOutputDTO> GetPlanetQueryAsync(GetPlanetFunction getPlanetFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetPlanetFunction, GetPlanetOutputDTO>(getPlanetFunction, blockParameter);
        }

        public Task<GetPlanetOutputDTO> GetPlanetQueryAsync(BigInteger id, BlockParameter blockParameter = null)
        {
            var getPlanetFunction = new GetPlanetFunction();
                getPlanetFunction.Id = id;
            
            return ContractHandler.QueryDeserializingToObjectAsync<GetPlanetFunction, GetPlanetOutputDTO>(getPlanetFunction, blockParameter);
        }

        public Task<BigInteger> GetPlanetSupplyQueryAsync(GetPlanetSupplyFunction getPlanetSupplyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetPlanetSupplyFunction, BigInteger>(getPlanetSupplyFunction, blockParameter);
        }

        
        public Task<BigInteger> GetPlanetSupplyQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetPlanetSupplyFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> GetRandomQueryAsync(GetRandomFunction getRandomFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetRandomFunction, BigInteger>(getRandomFunction, blockParameter);
        }

        
        public Task<BigInteger> GetRandomQueryAsync(byte[] entropy, BlockParameter blockParameter = null)
        {
            var getRandomFunction = new GetRandomFunction();
                getRandomFunction.Entropy = entropy;
            
            return ContractHandler.QueryAsync<GetRandomFunction, BigInteger>(getRandomFunction, blockParameter);
        }

        public Task<GetRocketOutputDTO> GetRocketQueryAsync(GetRocketFunction getRocketFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetRocketFunction, GetRocketOutputDTO>(getRocketFunction, blockParameter);
        }

        public Task<GetRocketOutputDTO> GetRocketQueryAsync(BigInteger id, BlockParameter blockParameter = null)
        {
            var getRocketFunction = new GetRocketFunction();
                getRocketFunction.Id = id;
            
            return ContractHandler.QueryDeserializingToObjectAsync<GetRocketFunction, GetRocketOutputDTO>(getRocketFunction, blockParameter);
        }

        public Task<BigInteger> GetRocketSupplyQueryAsync(GetRocketSupplyFunction getRocketSupplyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetRocketSupplyFunction, BigInteger>(getRocketSupplyFunction, blockParameter);
        }

        
        public Task<BigInteger> GetRocketSupplyQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetRocketSupplyFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> GetThresholdQueryAsync(GetThresholdFunction getThresholdFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetThresholdFunction, BigInteger>(getThresholdFunction, blockParameter);
        }

        
        public Task<BigInteger> GetThresholdQueryAsync(BigInteger numerator, BigInteger denominator, BlockParameter blockParameter = null)
        {
            var getThresholdFunction = new GetThresholdFunction();
                getThresholdFunction.Numerator = numerator;
                getThresholdFunction.Denominator = denominator;
            
            return ContractHandler.QueryAsync<GetThresholdFunction, BigInteger>(getThresholdFunction, blockParameter);
        }

        public Task<string> HijackRequestAsync(HijackFunction hijackFunction)
        {
             return ContractHandler.SendRequestAsync(hijackFunction);
        }

        public Task<TransactionReceipt> HijackRequestAndWaitForReceiptAsync(HijackFunction hijackFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(hijackFunction, cancellationToken);
        }

        public Task<string> HijackRequestAsync(BigInteger nukeId, BigInteger hijackCost)
        {
            var hijackFunction = new HijackFunction();
                hijackFunction.NukeId = nukeId;
                hijackFunction.HijackCost = hijackCost;
            
             return ContractHandler.SendRequestAsync(hijackFunction);
        }

        public Task<TransactionReceipt> HijackRequestAndWaitForReceiptAsync(BigInteger nukeId, BigInteger hijackCost, CancellationTokenSource cancellationToken = null)
        {
            var hijackFunction = new HijackFunction();
                hijackFunction.NukeId = nukeId;
                hijackFunction.HijackCost = hijackCost;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(hijackFunction, cancellationToken);
        }

        public Task<string> InitializeRequestAsync(InitializeFunction initializeFunction)
        {
             return ContractHandler.SendRequestAsync(initializeFunction);
        }

        public Task<TransactionReceipt> InitializeRequestAndWaitForReceiptAsync(InitializeFunction initializeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(initializeFunction, cancellationToken);
        }

        public Task<string> InitializeRequestAsync(string galaxyToken)
        {
            var initializeFunction = new InitializeFunction();
                initializeFunction.GalaxyToken = galaxyToken;
            
             return ContractHandler.SendRequestAsync(initializeFunction);
        }

        public Task<TransactionReceipt> InitializeRequestAndWaitForReceiptAsync(string galaxyToken, CancellationTokenSource cancellationToken = null)
        {
            var initializeFunction = new InitializeFunction();
                initializeFunction.GalaxyToken = galaxyToken;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(initializeFunction, cancellationToken);
        }

        public Task<BigInteger> LastStakedTimeQueryAsync(LastStakedTimeFunction lastStakedTimeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<LastStakedTimeFunction, BigInteger>(lastStakedTimeFunction, blockParameter);
        }

        
        public Task<BigInteger> LastStakedTimeQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var lastStakedTimeFunction = new LastStakedTimeFunction();
                lastStakedTimeFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<LastStakedTimeFunction, BigInteger>(lastStakedTimeFunction, blockParameter);
        }

        public Task<string> LendingpoolQueryAsync(LendingpoolFunction lendingpoolFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<LendingpoolFunction, string>(lendingpoolFunction, blockParameter);
        }

        
        public Task<string> LendingpoolQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<LendingpoolFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> MafiaTokenQueryAsync(MafiaTokenFunction mafiaTokenFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MafiaTokenFunction, BigInteger>(mafiaTokenFunction, blockParameter);
        }

        
        public Task<BigInteger> MafiaTokenQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MafiaTokenFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> MintPlanetRequestAsync(MintPlanetFunction mintPlanetFunction)
        {
             return ContractHandler.SendRequestAsync(mintPlanetFunction);
        }

        public Task<TransactionReceipt> MintPlanetRequestAndWaitForReceiptAsync(MintPlanetFunction mintPlanetFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintPlanetFunction, cancellationToken);
        }

        public Task<string> MintPlanetRequestAsync(string account, string tokenURI)
        {
            var mintPlanetFunction = new MintPlanetFunction();
                mintPlanetFunction.Account = account;
                mintPlanetFunction.TokenURI = tokenURI;
            
             return ContractHandler.SendRequestAsync(mintPlanetFunction);
        }

        public Task<TransactionReceipt> MintPlanetRequestAndWaitForReceiptAsync(string account, string tokenURI, CancellationTokenSource cancellationToken = null)
        {
            var mintPlanetFunction = new MintPlanetFunction();
                mintPlanetFunction.Account = account;
                mintPlanetFunction.TokenURI = tokenURI;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintPlanetFunction, cancellationToken);
        }

        public Task<string> MintRocketRequestAsync(MintRocketFunction mintRocketFunction)
        {
             return ContractHandler.SendRequestAsync(mintRocketFunction);
        }

        public Task<TransactionReceipt> MintRocketRequestAndWaitForReceiptAsync(MintRocketFunction mintRocketFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintRocketFunction, cancellationToken);
        }

        public Task<string> MintRocketRequestAsync(BigInteger planetId)
        {
            var mintRocketFunction = new MintRocketFunction();
                mintRocketFunction.PlanetId = planetId;
            
             return ContractHandler.SendRequestAsync(mintRocketFunction);
        }

        public Task<TransactionReceipt> MintRocketRequestAndWaitForReceiptAsync(BigInteger planetId, CancellationTokenSource cancellationToken = null)
        {
            var mintRocketFunction = new MintRocketFunction();
                mintRocketFunction.PlanetId = planetId;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintRocketFunction, cancellationToken);
        }

        public Task<string> NukeRequestAsync(NukeFunction nukeFunction)
        {
             return ContractHandler.SendRequestAsync(nukeFunction);
        }

        public Task<TransactionReceipt> NukeRequestAndWaitForReceiptAsync(NukeFunction nukeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(nukeFunction, cancellationToken);
        }

        public Task<string> NukeRequestAsync(BigInteger planetId, BigInteger rocketId, BigInteger missionCost)
        {
            var nukeFunction = new NukeFunction();
                nukeFunction.PlanetId = planetId;
                nukeFunction.RocketId = rocketId;
                nukeFunction.MissionCost = missionCost;
            
             return ContractHandler.SendRequestAsync(nukeFunction);
        }

        public Task<TransactionReceipt> NukeRequestAndWaitForReceiptAsync(BigInteger planetId, BigInteger rocketId, BigInteger missionCost, CancellationTokenSource cancellationToken = null)
        {
            var nukeFunction = new NukeFunction();
                nukeFunction.PlanetId = planetId;
                nukeFunction.RocketId = rocketId;
                nukeFunction.MissionCost = missionCost;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(nukeFunction, cancellationToken);
        }

        public Task<BigInteger> NukeCountQueryAsync(NukeCountFunction nukeCountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NukeCountFunction, BigInteger>(nukeCountFunction, blockParameter);
        }

        
        public Task<BigInteger> NukeCountQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NukeCountFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> OwnerQueryAsync(OwnerFunction ownerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }

        
        public Task<string> OwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> PlanetTypeQueryAsync(PlanetTypeFunction planetTypeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PlanetTypeFunction, BigInteger>(planetTypeFunction, blockParameter);
        }

        
        public Task<BigInteger> PlanetTypeQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PlanetTypeFunction, BigInteger>(null, blockParameter);
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

        public Task<BigInteger> RocketTypeQueryAsync(RocketTypeFunction rocketTypeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RocketTypeFunction, BigInteger>(rocketTypeFunction, blockParameter);
        }

        
        public Task<BigInteger> RocketTypeQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RocketTypeFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> StakeEthOnPlanetRequestAsync(StakeEthOnPlanetFunction stakeEthOnPlanetFunction)
        {
             return ContractHandler.SendRequestAsync(stakeEthOnPlanetFunction);
        }

        public Task<TransactionReceipt> StakeEthOnPlanetRequestAndWaitForReceiptAsync(StakeEthOnPlanetFunction stakeEthOnPlanetFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(stakeEthOnPlanetFunction, cancellationToken);
        }

        public Task<string> StakeEthOnPlanetRequestAsync(BigInteger tokenId)
        {
            var stakeEthOnPlanetFunction = new StakeEthOnPlanetFunction();
                stakeEthOnPlanetFunction.TokenId = tokenId;
            
             return ContractHandler.SendRequestAsync(stakeEthOnPlanetFunction);
        }

        public Task<TransactionReceipt> StakeEthOnPlanetRequestAndWaitForReceiptAsync(BigInteger tokenId, CancellationTokenSource cancellationToken = null)
        {
            var stakeEthOnPlanetFunction = new StakeEthOnPlanetFunction();
                stakeEthOnPlanetFunction.TokenId = tokenId;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(stakeEthOnPlanetFunction, cancellationToken);
        }

        public Task<BigInteger> StakedEthQueryAsync(StakedEthFunction stakedEthFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StakedEthFunction, BigInteger>(stakedEthFunction, blockParameter);
        }

        
        public Task<BigInteger> StakedEthQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var stakedEthFunction = new StakedEthFunction();
                stakedEthFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<StakedEthFunction, BigInteger>(stakedEthFunction, blockParameter);
        }

        public Task<string> TransferMafiaTokenRequestAsync(TransferMafiaTokenFunction transferMafiaTokenFunction)
        {
             return ContractHandler.SendRequestAsync(transferMafiaTokenFunction);
        }

        public Task<TransactionReceipt> TransferMafiaTokenRequestAndWaitForReceiptAsync(TransferMafiaTokenFunction transferMafiaTokenFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferMafiaTokenFunction, cancellationToken);
        }

        public Task<string> TransferMafiaTokenRequestAsync(string to, BigInteger amount)
        {
            var transferMafiaTokenFunction = new TransferMafiaTokenFunction();
                transferMafiaTokenFunction.To = to;
                transferMafiaTokenFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(transferMafiaTokenFunction);
        }

        public Task<TransactionReceipt> TransferMafiaTokenRequestAndWaitForReceiptAsync(string to, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferMafiaTokenFunction = new TransferMafiaTokenFunction();
                transferMafiaTokenFunction.To = to;
                transferMafiaTokenFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferMafiaTokenFunction, cancellationToken);
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

        public Task<string> TransferPlanetRequestAsync(TransferPlanetFunction transferPlanetFunction)
        {
             return ContractHandler.SendRequestAsync(transferPlanetFunction);
        }

        public Task<TransactionReceipt> TransferPlanetRequestAndWaitForReceiptAsync(TransferPlanetFunction transferPlanetFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferPlanetFunction, cancellationToken);
        }

        public Task<string> TransferPlanetRequestAsync(string to, BigInteger id)
        {
            var transferPlanetFunction = new TransferPlanetFunction();
                transferPlanetFunction.To = to;
                transferPlanetFunction.Id = id;
            
             return ContractHandler.SendRequestAsync(transferPlanetFunction);
        }

        public Task<TransactionReceipt> TransferPlanetRequestAndWaitForReceiptAsync(string to, BigInteger id, CancellationTokenSource cancellationToken = null)
        {
            var transferPlanetFunction = new TransferPlanetFunction();
                transferPlanetFunction.To = to;
                transferPlanetFunction.Id = id;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferPlanetFunction, cancellationToken);
        }

        public Task<string> TransferRocketRequestAsync(TransferRocketFunction transferRocketFunction)
        {
             return ContractHandler.SendRequestAsync(transferRocketFunction);
        }

        public Task<TransactionReceipt> TransferRocketRequestAndWaitForReceiptAsync(TransferRocketFunction transferRocketFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferRocketFunction, cancellationToken);
        }

        public Task<string> TransferRocketRequestAsync(string to, BigInteger id)
        {
            var transferRocketFunction = new TransferRocketFunction();
                transferRocketFunction.To = to;
                transferRocketFunction.Id = id;
            
             return ContractHandler.SendRequestAsync(transferRocketFunction);
        }

        public Task<TransactionReceipt> TransferRocketRequestAndWaitForReceiptAsync(string to, BigInteger id, CancellationTokenSource cancellationToken = null)
        {
            var transferRocketFunction = new TransferRocketFunction();
                transferRocketFunction.To = to;
                transferRocketFunction.Id = id;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferRocketFunction, cancellationToken);
        }
    }
}
