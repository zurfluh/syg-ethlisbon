# Galaxy Beyond. Welcome to the worlds of lobster finance.

Galaxy Beyond is a fully decentralized DeFi platform embedded into a fully featured multiplayer strategy game running on Ethereum. 
By playing Galaxy Beyond users can challenge other players on an interplanetary environment, reaping exciting rewards through yield generating NFTs.

In a nutshell, the game allows its users to own NFT planets in the milky way and grow an economy on top of each of them by staking ETH and mining precious resources, needed for __space colonization__. Once ready for space travel attack other planet and try to conquer the whole galaxy.

## MVP Rules:
- Get a planet in the initial NFT auction 
- Stake some ETH on your planet and start generating MOB tokens
- Claim your accrued MOB whenever you are ready
- Build lobsterNukes on your planet using MOB token
- Nuke another planet with a lobsterNuke in order to gain ownership of the planet (and its staked ETH!!!)
- Hijack a threathening lobsterNuke that is threatening one of your planet, and if you succeed reap a juicy MOB reward.

## Features
- Unity 2D game using with WebGL 
- ERC1155 token contracts using Hardhat 
- WalletConnect enabled login
- Deposit staked collateral to AAVE lending pools to compound staked ETH yield
- Chainlink VRF for attack and hijack resolution
- Opensea integration for planet NFTs auction

## Roadmap
- Finalize Alpha version
- Expand available planet resources and lobster economics for more exciting interplanetary battles
- The age of the guilds: Participate to the development of a planet and build a colonizing community
- Explore the Metaverse: Explore the planet of your guild in a 3D VR world to explore what inhabitants are building for space-colonization

## Requirements
 - Unity (version 2020.3.21f1)
 - Nodejs v14

## Getting Started
To run the project locally:

__Clone__
- `git clone https://github.com/zurfluh/syg-ethlisbon`

__Compile and deploy contracts__
 - `cd syg-ethlisbon/smartcontract`
 - `npm i`
 - `npx hardhat compile`
 - `npx ganache-cli`
 - `npx hardhat deploy` _Will deploy to local ganache_

 __Run game__
  - Open `SygUnity` project folder using Unity
  - Open `Scene0` to start the game





